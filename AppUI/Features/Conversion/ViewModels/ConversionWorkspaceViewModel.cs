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
        var addedCount = 0;
        var existingPaths = new HashSet<string>(Items.Select(i => i.FilePath), StringComparer.OrdinalIgnoreCase);

        foreach (var path in filePaths)
        {
            if (!File.Exists(path) || existingPaths.Contains(path)) continue;

            var item = new CfdiQueueItemViewModel(path, _processLauncher);
            item.RequestInspect += OnItemRequestInspect;
            item.RequestRemove += OnItemRequestRemove;

            Items.Add(item);
            existingPaths.Add(path);
            addedCount++;

            // Pre-lectura de metadatos fiscales en segundo plano
            if (_preferencesService.Current.PreScanMetadata)
            {
                _ = item.PreScanFiscalMetadataAsync(_xmlParser).ContinueWith(_ =>
                {
                    Avalonia.Threading.Dispatcher.UIThread.Post(() => ApplyFilter());
                });
            }
        }

        if (addedCount > 0)
        {
            UpdateKpis();
            ApplyFilter();
            _toastService.Info("Archivos Agregados", $"{addedCount} comprobante(s) agregado(s) a la cola.");
        }
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
    }

    public void UpdateKpis()
    {
        TotalCount = Items.Count;
        CompletedCount = Items.Count(i => i.Status == StatusTone.Success);
        PendingCount = Items.Count(i => i.Status == StatusTone.Neutral || i.Status == StatusTone.Info);
        ErrorCount = Items.Count(i => i.Status == StatusTone.Danger);
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
                var files = Directory.GetFiles(folder, "*.xml", SearchOption.AllDirectories);
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
        ProgressText = $"Iniciando lote de {pendingItems.Count} comprobantes...";
        _conversionCts = new CancellationTokenSource();

        var prefs = _preferencesService.Current;
        var options = new CfdiConversionOptions
        {
            OutputDirectory = prefs.DefaultOutputDirectory,
            LogoImagePath = prefs.LogoPath,
            OverwriteExisting = prefs.OverwriteExisting
        };

        var progress = new Progress<CfdiConversionProgress>(p =>
        {
            ProgressPercentage = p.TotalCount > 0 ? (double)p.ProcessedCount / p.TotalCount * 100 : 0;
            ProgressText = $"Procesando {p.ProcessedCount} de {p.TotalCount}: {Path.GetFileName(p.CurrentFile)}";
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
                _toastService.Success("Conversión Completada", $"{successCount} comprobantes convertidos exitosamente a PDF.");
            }
            else
            {
                _toastService.Warning("Conversión con Incidencias", $"{successCount} exitosos, {failCount} con errores.");
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
