using System;
using System.IO;
using System.Threading.Tasks;
using AppUI.Core.Models;
using AppUI.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Abstractions;
using Core.Common;

namespace AppUI.Features.Conversion.Models;

public partial class CfdiQueueItemViewModel : ObservableObject
{
    private readonly IProcessLauncherService _processLauncher;

    [ObservableProperty]
    private string _filePath;

    [ObservableProperty]
    private string _fileName;

    [ObservableProperty]
    private long _fileSizeBytes;

    [ObservableProperty]
    private DateTime _modificationDate;

    // Metadatos Fiscales Pre-leídos
    [ObservableProperty]
    private string? _uuid;

    [ObservableProperty]
    private string? _tipoDeComprobante; // I, E, P, N, T

    [ObservableProperty]
    private string? _tipoDeComprobanteDescripcion;

    [ObservableProperty]
    private string? _emisorRfc;

    [ObservableProperty]
    private string? _emisorNombre;

    [ObservableProperty]
    private string? _receptorRfc;

    [ObservableProperty]
    private string? _receptorNombre;

    [ObservableProperty]
    private decimal? _total;

    [ObservableProperty]
    private string? _totalFormatted;

    [ObservableProperty]
    private string? _complementosResumen;

    // Estado del ciclo de vida
    [ObservableProperty]
    private StatusTone _status = StatusTone.Neutral;

    [ObservableProperty]
    private string _statusText = "Pendiente";

    [ObservableProperty]
    private string? _errorMessage;

    [ObservableProperty]
    private string? _outputPdfPath;

    public event Action<CfdiQueueItemViewModel>? RequestInspect;
    public event Action<CfdiQueueItemViewModel>? RequestRemove;

    public CfdiQueueItemViewModel(string filePath, IProcessLauncherService processLauncher)
    {
        _filePath = filePath;
        _processLauncher = processLauncher;
        _fileName = Path.GetFileName(filePath);

        try
        {
            var info = new FileInfo(filePath);
            _fileSizeBytes = info.Length;
            _modificationDate = info.LastWriteTime;
        }
        catch
        {
            _fileSizeBytes = 0;
            _modificationDate = DateTime.Now;
        }
    }

    /// <summary>
    /// Lee metadatos fiscales en segundo plano con ICfdiXmlParser sin bloquear la UI.
    /// </summary>
    public async Task PreScanFiscalMetadataAsync(ICfdiXmlParser parser)
    {
        try
        {
            var c = await parser.ParseFileAsync(FilePath).ConfigureAwait(false);

            TipoDeComprobante = c.TipoDeComprobante;
            TipoDeComprobanteDescripcion = c.TipoDeComprobante switch
            {
                "I" => "Ingreso",
                "E" => "Egreso",
                "P" => "Pago 2.0",
                "N" => "Nómina 1.2",
                "T" => "Traslado",
                _ => c.TipoDeComprobante
            };

            EmisorRfc = c.Emisor?.Rfc;
            EmisorNombre = c.Emisor?.Nombre;
            ReceptorRfc = c.Receptor?.Rfc;
            ReceptorNombre = c.Receptor?.Nombre;

            Total = c.Total;
            TotalFormatted = c.Total.ToString("C2", System.Globalization.CultureInfo.GetCultureInfo("es-MX"));

            Uuid = c.TimbreFiscalDigital?.UUID;

            // Detección de complementos
            var compls = new System.Collections.Generic.List<string>();
            if (c.Nomina != null) compls.Add("Nómina 1.2");
            if (c.Pagos != null) compls.Add("Pagos 2.0");
            if (c.CartaPorte != null) compls.Add("Carta Porte 3.1");
            if (c.ValesDespensa != null) compls.Add("Vales");
            if (c.ComercioExterior != null) compls.Add("Com. Exterior");
            if (c.ImpuestosLocales != null) compls.Add("Imp. Locales");
            if (c.LeyendasFiscales != null) compls.Add("Leyendas");

            ComplementosResumen = compls.Count > 0 ? string.Join(", ", compls) : "Estándar";
        }
        catch (Exception ex)
        {
            ComplementosResumen = "Error de lectura";
            ErrorMessage = ex.Message;
        }
    }

    public void MarkProcessing()
    {
        Status = StatusTone.Info;
        StatusText = "Convirtiendo...";
        ErrorMessage = null;
    }

    public void MarkCompleted(string pdfPath)
    {
        Status = StatusTone.Success;
        StatusText = "Completado";
        OutputPdfPath = pdfPath;
        ErrorMessage = null;
    }

    public void MarkError(string error)
    {
        Status = StatusTone.Danger;
        StatusText = "Error";
        ErrorMessage = error;
    }

    [RelayCommand]
    private void Inspect()
    {
        RequestInspect?.Invoke(this);
    }

    [RelayCommand]
    private void Remove()
    {
        RequestRemove?.Invoke(this);
    }

    [RelayCommand]
    private void OpenPdf()
    {
        if (!string.IsNullOrEmpty(OutputPdfPath))
        {
            _processLauncher.OpenFile(OutputPdfPath);
        }
    }

    [RelayCommand]
    private void OpenFolder()
    {
        if (!string.IsNullOrEmpty(OutputPdfPath))
        {
            _processLauncher.OpenDirectoryAndSelect(OutputPdfPath);
        }
        else if (!string.IsNullOrEmpty(FilePath))
        {
            _processLauncher.OpenDirectoryAndSelect(FilePath);
        }
    }
}
