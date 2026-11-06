using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using AppUI.Core.Models;
using AppUI.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AppUI.Features.Diagnostics.ViewModels;

public sealed record LogEntry(
    string Timestamp,
    string Level,
    string Message,
    StatusTone StatusTone);

public partial class DiagnosticsViewModel : ObservableObject
{
    private readonly IProcessLauncherService _processLauncher;
    private readonly IToastService _toastService;
    private readonly string _logsDirectory;

    [ObservableProperty]
    private ObservableCollection<LogEntry> _logs = new();

    [ObservableProperty]
    private ObservableCollection<LogEntry> _filteredLogs = new();

    [ObservableProperty]
    private string _searchQuery = string.Empty;

    [ObservableProperty]
    private string _selectedLevelFilter = "Todos"; // Todos, INFO, WARN, ERROR

    public IReadOnlyList<string> LevelFilterOptions { get; } = ["Todos", "INFO", "WARN", "ERROR"];

    public bool HasLogs => Logs.Count > 0;
    public bool HasFilteredLogs => FilteredLogs.Count > 0;
    public bool HasNoLogs => FilteredLogs.Count == 0;

    public DiagnosticsViewModel(IProcessLauncherService processLauncher, IToastService toastService)
    {
        _processLauncher = processLauncher;
        _toastService = toastService;

        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        _logsDirectory = Path.Combine(appData, "XPressPDF", "logs");

        LoadLogs();
    }

    [RelayCommand]
    public void LoadLogs()
    {
        Logs.Clear();

        try
        {
            if (Directory.Exists(_logsDirectory))
            {
                var files = Directory.GetFiles(_logsDirectory, "*.log")
                    .OrderByDescending(File.GetLastWriteTime)
                    .ToList();

                if (files.Count > 0)
                {
                    var lines = new List<string>();
                    using (var fs = new FileStream(files[0], FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var reader = new StreamReader(fs, Encoding.UTF8))
                    {
                        string? line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (!string.IsNullOrWhiteSpace(line))
                                lines.Add(line);
                        }
                    }

                    // Eventos más recientes arriba
                    lines.Reverse();
                    foreach (var line in lines)
                    {
                        var entry = ParseLine(line);
                        Logs.Add(entry);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _toastService.Danger("Error al Leer Bitácora", ex.Message);
        }

        ApplyFilter();
    }

    private static LogEntry ParseLine(string line)
    {
        // Formato esperado: [2026-11-06 12:30:00.123] [INFO] Mensaje
        var status = StatusTone.Neutral;
        var level = "INFO";
        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        var message = line;

        try
        {
            if (line.StartsWith('[') && line.IndexOf(']') is int firstClose && firstClose > 1)
            {
                var rawTs = line[1..firstClose];
                if (DateTime.TryParse(rawTs, out var dt))
                {
                    timestamp = dt.ToString("HH:mm:ss");
                }
                else
                {
                    timestamp = rawTs;
                }

                var rest = line[(firstClose + 1)..].TrimStart();
                if (rest.StartsWith('[') && rest.IndexOf(']') is int secondClose && secondClose > 1)
                {
                    level = rest[1..secondClose].ToUpperInvariant();
                    message = rest[(secondClose + 1)..].TrimStart();
                }
            }
        }
        catch
        {
            // Conserva mensaje original como fallback
        }

        status = level switch
        {
            "ERROR" => StatusTone.Danger,
            "WARN" or "WARNING" => StatusTone.Warning,
            "INFO" => StatusTone.Info,
            _ => StatusTone.Neutral
        };

        return new LogEntry(timestamp, level, message, status);
    }

    partial void OnSearchQueryChanged(string value) => ApplyFilter();
    partial void OnSelectedLevelFilterChanged(string value) => ApplyFilter();

    private void ApplyFilter()
    {
        var query = Logs.AsEnumerable();

        if (SelectedLevelFilter != "Todos")
        {
            query = query.Where(l => string.Equals(l.Level, SelectedLevelFilter, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(SearchQuery))
        {
            query = query.Where(l => l.Message.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));
        }

        FilteredLogs = new ObservableCollection<LogEntry>(query);
        OnPropertyChanged(nameof(HasLogs));
        OnPropertyChanged(nameof(HasFilteredLogs));
        OnPropertyChanged(nameof(HasNoLogs));
    }

    [RelayCommand]
    private void OpenLogsFolder()
    {
        if (Directory.Exists(_logsDirectory))
        {
            _processLauncher.OpenDirectory(_logsDirectory);
        }
        else
        {
            _toastService.Info("Bitácora", "Aún no se ha generado ningún archivo de log en esta sesión.");
        }
    }
}
