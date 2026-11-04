namespace XPressPDF.IoC;

using Core.Abstractions;
using Core.Pdf;
using Core.Pdf.ComplementRenderers;
using Core.Services;
using Core.Xml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

/// <summary>
/// Métodos de extensión para configurar y registrar los servicios del núcleo XPressPDF en el contenedor de dependencias.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Agrega todos los componentes del núcleo de procesamiento XML y generación PDF de CFDI 4.0.
    /// </summary>
    public static IServiceCollection AddXPressPdfCore(
        this IServiceCollection services,
        Action<CfdiComplementRegistry>? configureComplements = null,
        string? logDirectory = null,
        QuestPDF.Infrastructure.LicenseType licenseType = QuestPDF.Infrastructure.LicenseType.Community)
    {
        // 0. Configuración de licencia QuestPDF
        QuestPDF.Settings.License = licenseType;

        // 1. Registro de complementos XML
        var complementRegistry = new CfdiComplementRegistry();
        configureComplements?.Invoke(complementRegistry);
        services.TryAddSingleton(complementRegistry);

        // 2. Parser y Deserializador XML
        services.TryAddSingleton<ICfdiComplementResolver, CfdiComplementResolver>();
        services.TryAddSingleton<ICfdiXmlParser, CfdiXmlParser>();

        // 3. Renderizadores modulares de secciones de complementos
        services.AddSingleton<ICfdiComplementSectionRenderer, ValesDespensaSectionRenderer>();
        services.AddSingleton<ICfdiComplementSectionRenderer, ImpuestosLocalesSectionRenderer>();
        services.AddSingleton<ICfdiComplementSectionRenderer, LeyendasFiscalesSectionRenderer>();
        services.AddSingleton<ICfdiComplementSectionRenderer, ComercioExteriorSectionRenderer>();
        services.AddSingleton<ICfdiComplementSectionRenderer, DivisasSectionRenderer>();
        services.AddSingleton<ICfdiComplementSectionRenderer, CartaPorteSectionRenderer>();

        // 4. Fábrica y Generador de Documentos QuestPDF
        services.TryAddSingleton<ICfdiPdfDocumentFactory, CfdiPdfDocumentFactory>();
        services.TryAddSingleton<ICfdiPdfGenerator, CfdiPdfGenerator>();

        // 5. Diagnóstico y Registro de Eventos
        services.TryAddSingleton<ILogService>(_ => new FileLogService(logDirectory));

        // 6. Orquestador de Conversiones
        services.TryAddTransient<CfdiConversionService>();

        return services;
    }
}
