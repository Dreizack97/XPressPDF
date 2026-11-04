using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
                    var lines = File.ReadAllLines(files[0]);
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
        // Formato: [2026-11-04 12:30:00] [INFO] Mensaje
        var status = StatusTone.Neutral;
        var level = "INFO";
        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        var message = line;

        if (line.Contains("[ERROR]", StringComparison.OrdinalIgnoreCase))
        {
            level = "ERROR";
            status = StatusTone.Danger;
        }
        else if (line.Contains("[WARN]", StringComparison.OrdinalIgnoreCase))
        {
            level = "WARN";
            status = StatusTone.Warning;
        }
        else if (line.Contains("[INFO]", StringComparison.OrdinalIgnoreCase))
        {
            level = "INFO";
            status = StatusTone.Info;
        }

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
