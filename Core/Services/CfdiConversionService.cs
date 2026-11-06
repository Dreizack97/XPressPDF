namespace Core.Services;

using Core.Abstractions;
using Core.Common;
using Core.Models.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Orquestador principal para la conversión de comprobantes CFDI 4.0 individuales y masivos por lotes.
/// </summary>
public class CfdiConversionService
{
    private readonly ICfdiXmlParser _xmlParser;
    private readonly ICfdiPdfGenerator _pdfGenerator;
    private readonly ILogService _logService;

    public CfdiConversionService(
        ICfdiXmlParser xmlParser,
        ICfdiPdfGenerator pdfGenerator,
        ILogService logService)
    {
        _xmlParser = xmlParser;
        _pdfGenerator = pdfGenerator;
        _logService = logService;
    }

    /// <summary>
    /// Convierte un único archivo XML CFDI a su representación impresa en PDF.
    /// </summary>
    public async Task<CfdiConversionResult> ConvertXmlFileAsync(
        string xmlFilePath,
        string? outputPdfPath = null,
        CfdiConversionOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        options ??= new CfdiConversionOptions();

        try
        {
            if (!File.Exists(xmlFilePath))
            {
                string error = $"El archivo XML origen no existe: {xmlFilePath}";
                await _logService.LogAsync(error, "ERROR", cancellationToken).ConfigureAwait(false);
                return new CfdiConversionResult(xmlFilePath, null, null, false, error, stopwatch.Elapsed);
            }

            // Precarga segura de logotipo corporativo si no fue precargado en bytes
            if (options.LogoImageBytes is null &&
                !string.IsNullOrWhiteSpace(options.LogoImagePath) &&
                File.Exists(options.LogoImagePath))
            {
                try
                {
                    byte[] logoBytes = await File.ReadAllBytesAsync(options.LogoImagePath, cancellationToken).ConfigureAwait(false);
                    options = options with { LogoImageBytes = logoBytes };
                }
                catch
                {
                    // Continúa con la conversión si el logotipo no puede leerse
                }
            }

            // Deserialización asíncrona segura
            Comprobante comprobante = await _xmlParser.ParseFileAsync(xmlFilePath, cancellationToken).ConfigureAwait(false);
            string? uuid = comprobante.TimbreFiscalDigital?.UUID;

            // Determinar la ruta de salida
            string targetPath = outputPdfPath ?? DetermineOutputPath(xmlFilePath, options, uuid);

            if (File.Exists(targetPath) && !options.OverwriteExisting)
            {
                string error = $"El archivo PDF de destino ya existe y no se permite sobreescritura: {targetPath}";
                await _logService.LogAsync(error, "WARN", cancellationToken).ConfigureAwait(false);
                return new CfdiConversionResult(xmlFilePath, targetPath, uuid, false, error, stopwatch.Elapsed);
            }

            // Generación de PDF con QuestPDF
            await _pdfGenerator.GeneratePdfFileAsync(comprobante, targetPath, options, cancellationToken).ConfigureAwait(false);

            stopwatch.Stop();
            await _logService.LogAsync($"Conversión exitosa: {Path.GetFileName(xmlFilePath)} -> {Path.GetFileName(targetPath)} (UUID: {uuid}) en {stopwatch.ElapsedMilliseconds} ms", "INFO", cancellationToken).ConfigureAwait(false);

            return new CfdiConversionResult(xmlFilePath, targetPath, uuid, true, null, stopwatch.Elapsed);
        }
        catch (OperationCanceledException)
        {
            stopwatch.Stop();
            return new CfdiConversionResult(xmlFilePath, null, null, false, "Operación cancelada por el usuario", stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            string error = $"Error al convertir {Path.GetFileName(xmlFilePath)}: {ex.Message}";
            await _logService.LogAsync(error, "ERROR", cancellationToken).ConfigureAwait(false);
            return new CfdiConversionResult(xmlFilePath, null, null, false, error, stopwatch.Elapsed);
        }
    }

    /// <summary>
    /// Convierte de manera masiva y concurrente un lote de archivos XML CFDI a PDF reportando progreso en tiempo real.
    /// </summary>
    public async Task<IReadOnlyList<CfdiConversionResult>> ConvertXmlFilesBatchAsync(
        IEnumerable<string> xmlFiles,
        string? outputDirectory = null,
        CfdiConversionOptions? options = null,
        IProgress<CfdiConversionProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        var fileList = xmlFiles.ToList();
        int totalFiles = fileList.Count;
        int processedCount = 0;

        var results = new ConcurrentBag<CfdiConversionResult>();

        if (totalFiles == 0)
        {
            return Array.Empty<CfdiConversionResult>();
        }

        options ??= new CfdiConversionOptions();
        if (!string.IsNullOrEmpty(outputDirectory))
        {
            options = options with { OutputDirectory = outputDirectory };
        }

        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = Math.Clamp(Environment.ProcessorCount, 2, 8),
            CancellationToken = cancellationToken
        };

        await Parallel.ForEachAsync(fileList, parallelOptions, async (filePath, ct) =>
        {
            CfdiConversionResult result = await ConvertXmlFileAsync(filePath, null, options, ct).ConfigureAwait(false);
            results.Add(result);

            int currentCount = Interlocked.Increment(ref processedCount);
            progress?.Report(new CfdiConversionProgress(currentCount, totalFiles, filePath, result.Success));
        }).ConfigureAwait(false);

        return results.ToList();
    }

    private static string DetermineOutputPath(string xmlFilePath, CfdiConversionOptions options, string? uuid)
    {
        string targetDir = !string.IsNullOrEmpty(options.OutputDirectory)
            ? options.OutputDirectory
            : Path.GetDirectoryName(xmlFilePath) ?? Directory.GetCurrentDirectory();

        string fileName = Path.GetFileNameWithoutExtension(xmlFilePath) + ".pdf";
        return Path.Combine(targetDir, fileName);
    }
}
