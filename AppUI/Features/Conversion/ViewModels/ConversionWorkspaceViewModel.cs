using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppUI.Core.Models;
using AppUI.Core.Services;
using AppUI.Features.Conversion.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Abstractions;
using Core.Common;
using Core.Services;

namespace AppUI.Features.Conversion.ViewModels;

public partial class ConversionWorkspaceViewModel : ObservableObject
{
    private readonly CfdiConversionService _conversionService;
    private readonly ICfdiXmlParser _xmlParser;
    private readonly IStorageService _storageService;
    private readonly IPreferencesService _preferencesService;
    private readonly IToastService _toastService;
    private readonly IProcessLauncherService _processLauncher;

    private readonly SemaphoreSlim _preScanSemaphore = new(Math.Clamp(Environment.ProcessorCount * 2, 4, 16));
    private CancellationTokenSource? _conversionCts;

    [ObservableProperty]
    private ObservableCollection<CfdiQueueItemViewModel> _items = new();

    [ObservableProperty]
    private ObservableCollection<CfdiQueueItemViewModel> _filteredItems = new();

    [ObservableProperty]
    private string _searchQuery = string.Empty;

    [ObservableProperty]
    private string _selectedStatusFilter = "Todos"; // Todos, Pendientes, Completados, Con Error

    [ObservableProperty]
    private string _selectedTypeFilter = "Todos"; // Todos, I, E, P, N, T

    public IReadOnlyList<string> StatusFilterOptions { get; } = ["Todos", "Pendientes", "Completados", "Con Error"];

    public IReadOnlyList<string> TypeFilterOptions { get; } = ["Todos", "I", "E", "P", "N", "T"];

    public bool HasItems => Items.Count > 0;
    public bool HasCompletedItems => CompletedCount > 0;
    public bool HasFilteredItems => FilteredItems.Count > 0;
    public bool HasNoFilterMatches => HasItems && FilteredItems.Count == 0;
    public string QueueSummaryText => $"{FilteredItems.Count:N0} de {TotalCount:N0} visibles";

    // KPIs en Vivo
    [ObservableProperty]
    private int _totalCount;

    [ObservableProperty]
    private int _completedCount;

    [ObservableProperty]
    private int _pendingCount;

    [ObservableProperty]
    private int _errorCount;

    // Estado del Procesamiento por Lote
    [ObservableProperty]
    private bool _isConverting;

    [ObservableProperty]
    private double _progressPercentage;

    [ObservableProperty]
    private string _progressText = string.Empty;

    // Inspector Lateral (Drawer)
    [ObservableProperty]
    private bool _isInspectorOpen;

    [ObservableProperty]
    private CfdiQueueItemViewModel? _selectedItemForInspection;

    public ConversionWorkspaceViewModel(
        CfdiConversionService conversionService,
        ICfdiXmlParser xmlParser,
        IStorageService storageService,
        IPreferencesService preferencesService,
        IToastService toastService,
        IProcessLauncherService processLauncher)
    {
        _conversionService = conversionService;
        _xmlParser = xmlParser;
        _storageService = storageService;
        _preferencesService = preferencesService;
        _toastService = toastService;
        _processLauncher = processLauncher;
    }

    public void AddXmlFiles(IEnumerable<string> filePaths)
    {
        var existingPaths = new HashSet<string>(Items.Select(i => i.FilePath), StringComparer.OrdinalIgnoreCase);
        var newItems = new List<CfdiQueueItemViewModel>();

        foreach (var path in filePaths)
        {
            if (string.IsNullOrWhiteSpace(path) || existingPaths.Contains(path) || !File.Exists(path))
                continue;

            var item = new CfdiQueueItemViewModel(path, _processLauncher);
            item.RequestInspect += OnItemRequestInspect;
            item.RequestRemove += OnItemRequestRemove;

            newItems.Add(item);
            existingPaths.Add(path);
        }

        if (newItems.Count == 0) return;

        foreach (var item in newItems)
        {
            Items.Add(item);
        }

        UpdateKpis();
        ApplyFilter();
        _toastService.Info("Archivos Agregados", $"{newItems.Count:N0} comprobante(s) agregado(s) a la cola.");

        // Pre-lectura de metadatos fiscales en segundo plano con control de concurrencia
        if (_preferencesService.Current.PreScanMetadata)
        {
            _ = Task.Run(async () =>
            {
                var tasks = newItems.Select(async item =>
                {
                    await _preScanSemaphore.WaitAsync().ConfigureAwait(false);
                    try
                    {
                        await item.PreScanFiscalMetadataAsync(_xmlParser).ConfigureAwait(false);
                    }
                    finally
                    {
                        _preScanSemaphore.Release();
                    }
                });

                await Task.WhenAll(tasks).ConfigureAwait(false);

                // Si hay un filtro o búsqueda activa, actualizar una sola vez al terminar el lote
                if (!string.IsNullOrWhiteSpace(SearchQuery) || SelectedTypeFilter != "Todos")
                {
                    Avalonia.Threading.Dispatcher.UIThread.Post(ApplyFilter);
                }
            });
        }
    }

    public async Task AddXmlFilesAsync(IEnumerable<string> filePaths)
    {
        var pathsList = filePaths.ToList();
        var existingPaths = new HashSet<string>(Items.Select(i => i.FilePath), StringComparer.OrdinalIgnoreCase);

        var validPaths = await Task.Run(() =>
        {
            return pathsList.Where(p => !string.IsNullOrWhiteSpace(p) && !existingPaths.Contains(p) && File.Exists(p)).ToList();
        }).ConfigureAwait(false);

        if (validPaths.Count == 0) return;

        await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
        {
            AddXmlFiles(validPaths);
        });
    }

    private void OnItemRequestInspect(CfdiQueueItemViewModel item)
    {
        SelectedItemForInspection = item;
        IsInspectorOpen = true;
    }

    private void OnItemRequestRemove(CfdiQueueItemViewModel item)
    {
        Items.Remove(item);
        if (SelectedItemForInspection == item)
        {
            IsInspectorOpen = false;
            SelectedItemForInspection = null;
        }
        UpdateKpis();
        ApplyFilter();
    }

    partial void OnSearchQueryChanged(string value) => ApplyFilter();
    partial void OnSelectedStatusFilterChanged(string value) => ApplyFilter();
    partial void OnSelectedTypeFilterChanged(string value) => ApplyFilter();

    public void ApplyFilter()
    {
        var query = Items.AsEnumerable();

        // 1. Filtro por Estado
        if (SelectedStatusFilter == "Pendientes")
            query = query.Where(i => i.Status == StatusTone.Neutral);
        else if (SelectedStatusFilter == "Completados")
            query = query.Where(i => i.Status == StatusTone.Success);
        else if (SelectedStatusFilter == "Con Error")
            query = query.Where(i => i.Status == StatusTone.Danger);

        // 2. Filtro por Tipo de Comprobante
        if (SelectedTypeFilter != "Todos")
            query = query.Where(i => string.Equals(i.TipoDeComprobante, SelectedTypeFilter, StringComparison.OrdinalIgnoreCase));

        // 3. Filtro por Búsqueda de Texto
        if (!string.IsNullOrWhiteSpace(SearchQuery))
        {
            var q = SearchQuery.Trim();
            query = query.Where(i =>
                i.FileName.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                (i.Uuid != null && i.Uuid.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                (i.EmisorRfc != null && i.EmisorRfc.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                (i.EmisorNombre != null && i.EmisorNombre.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                (i.ReceptorRfc != null && i.ReceptorRfc.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                (i.ReceptorNombre != null && i.ReceptorNombre.Contains(q, StringComparison.OrdinalIgnoreCase)));
        }

        FilteredItems = new ObservableCollection<CfdiQueueItemViewModel>(query);
        NotifyCollectionState();
    }

    public void UpdateKpis()
    {
        TotalCount = Items.Count;
        CompletedCount = Items.Count(i => i.Status == StatusTone.Success);
        PendingCount = Items.Count(i => i.Status == StatusTone.Neutral || i.Status == StatusTone.Info);
        ErrorCount = Items.Count(i => i.Status == StatusTone.Danger);
        NotifyCollectionState();
    }

    private void NotifyCollectionState()
    {
        OnPropertyChanged(nameof(HasItems));
        OnPropertyChanged(nameof(HasCompletedItems));
        OnPropertyChanged(nameof(HasFilteredItems));
        OnPropertyChanged(nameof(HasNoFilterMatches));
        OnPropertyChanged(nameof(QueueSummaryText));
    }

    [RelayCommand]
    private async Task PickFilesAsync()
    {
        var paths = await _storageService.PickXmlFilesAsync();
        if (paths.Count > 0)
        {
            AddXmlFiles(paths);
        }
    }

    [RelayCommand]
    private async Task PickFolderAsync()
    {
        var folder = await _storageService.PickFolderAsync();
        if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
        {
            try
            {
                var files = await Task.Run(() => Directory.GetFiles(folder, "*.xml", SearchOption.AllDirectories));
                AddXmlFiles(files);
            }
            catch (Exception ex)
            {
                _toastService.Danger("Error al Explorar Carpeta", ex.Message);
            }
        }
    }

    [RelayCommand]
    private void ClearCompleted()
    {
        var completed = Items.Where(i => i.Status == StatusTone.Success).ToList();
        foreach (var item in completed)
        {
            Items.Remove(item);
        }
        UpdateKpis();
        ApplyFilter();
    }

    [RelayCommand]
    private void ClearAll()
    {
        Items.Clear();
        IsInspectorOpen = false;
        SelectedItemForInspection = null;
        UpdateKpis();
        ApplyFilter();
    }

    [RelayCommand]
    private async Task ConvertAllAsync()
    {
        var pendingItems = Items.Where(i => i.Status != StatusTone.Success).ToList();
        if (pendingItems.Count == 0)
        {
            _toastService.Info("Sin Comprobantes", "No hay archivos pendientes de conversión en la cola.");
            return;
        }

        IsConverting = true;
        ProgressPercentage = 0;
        ProgressText = $"Iniciando lote de {pendingItems.Count:N0} comprobantes...";
        _conversionCts = new CancellationTokenSource();

        var prefs = _preferencesService.Current;
        var options = new CfdiConversionOptions
        {
            OutputDirectory = prefs.DefaultOutputDirectory,
            LogoImagePath = prefs.LogoPath,
            OverwriteExisting = prefs.OverwriteExisting
        };

        var lastProgressReport = DateTime.MinValue;
        var progress = new Progress<CfdiConversionProgress>(p =>
        {
            var now = DateTime.UtcNow;
            if (p.ProcessedCount == p.TotalCount || (now - lastProgressReport).TotalMilliseconds >= 50)
            {
                lastProgressReport = now;
                ProgressPercentage = p.TotalCount > 0 ? (double)p.ProcessedCount / p.TotalCount * 100 : 0;
                ProgressText = $"Procesando {p.ProcessedCount:N0} de {p.TotalCount:N0}: {Path.GetFileName(p.CurrentFile)}";
            }
        });

        var filePaths = pendingItems.Select(i => i.FilePath).ToList();

        try
        {
            foreach (var item in pendingItems)
            {
                item.MarkProcessing();
            }

            var results = await _conversionService.ConvertXmlFilesBatchAsync(
                filePaths,
                options.OutputDirectory,
                options,
                progress,
                _conversionCts.Token);

            var resultMap = results.ToDictionary(r => r.SourceFilePath, StringComparer.OrdinalIgnoreCase);
            foreach (var item in pendingItems)
            {
                if (resultMap.TryGetValue(item.FilePath, out var res))
                {
                    if (res.Success)
                        item.MarkCompleted(res.OutputPdfPath!);
                    else
                        item.MarkError(res.ErrorMessage ?? "Error desconocido al generar PDF.");
                }
            }

            UpdateKpis();
            ApplyFilter();

            var successCount = results.Count(r => r.Success);
            var failCount = results.Count(r => !r.Success);

            if (failCount == 0)
            {
                _toastService.Success("Conversión Completada", $"{successCount:N0} comprobantes convertidos exitosamente a PDF.");
            }
            else
            {
                _toastService.Warning("Conversión con Incidencias", $"{successCount:N0} exitosos, {failCount:N0} con errores.");
            }
        }
        catch (OperationCanceledException)
        {
            _toastService.Warning("Proceso Cancelado", "La conversión por lotes fue cancelada por el usuario.");
            foreach (var item in pendingItems.Where(i => i.Status == StatusTone.Info))
            {
                item.Status = StatusTone.Neutral;
                item.StatusText = "Cancelado";
            }
        }
        catch (Exception ex)
        {
            _toastService.Danger("Error en Lote", ex.Message);
        }
        finally
        {
            IsConverting = false;
            _conversionCts = null;
            UpdateKpis();
            ApplyFilter();
        }
    }

    [RelayCommand]
    private void CancelConversion()
    {
        _conversionCts?.Cancel();
    }
}
