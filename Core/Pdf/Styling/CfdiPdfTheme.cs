namespace Core.Pdf.Styling;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

/// <summary>
/// Centraliza la paleta de colores, tipografías y estilos de texto para la representación impresa de comprobantes.
/// </summary>
public static class CfdiPdfTheme
{
    public static readonly string PrimaryFont = Fonts.Arial;

    public static readonly string PrimaryColor = Colors.Grey.Darken3;
    public static readonly string SecondaryColor = Colors.Grey.Darken2;
    public static readonly string BorderColor = Colors.Grey.Lighten2;
    public static readonly string LightBackground = Colors.Grey.Lighten3;
    public static readonly string HeaderBackground = Colors.Grey.Lighten2;

    public static readonly TextStyle TitleStyle = TextStyle.Default.FontFamily(PrimaryFont).FontSize(9).FontColor(PrimaryColor).Bold();
    public static readonly TextStyle SectionTitleStyle = TextStyle.Default.FontFamily(PrimaryFont).FontSize(8).FontColor(PrimaryColor).Bold();
    public static readonly TextStyle LabelStyle = TextStyle.Default.FontFamily(PrimaryFont).FontSize(7.5F).FontColor(PrimaryColor).Bold();
    public static readonly TextStyle ValueStyle = TextStyle.Default.FontFamily(PrimaryFont).FontSize(7.5F).FontColor(SecondaryColor);
    public static readonly TextStyle HeaderLabelStyle = TextStyle.Default.FontFamily(PrimaryFont).FontSize(7.5F).FontColor(SecondaryColor).Bold();
    public static readonly TextStyle SmallValueStyle = TextStyle.Default.FontFamily(PrimaryFont).FontSize(6).FontColor(SecondaryColor);
    public static readonly TextStyle SealStyle = TextStyle.Default.FontFamily(PrimaryFont).FontSize(4.5F).FontColor(SecondaryColor);

    public static IContainer CellHeaderStyle(IContainer container) =>
        container.Background(LightBackground).PaddingVertical(2.5F).PaddingHorizontal(4);

    public static IContainer CellBodyStyle(IContainer container) =>
        container.BorderBottom(0.5F).BorderColor(BorderColor).PaddingVertical(1.5F).PaddingHorizontal(4);
}
