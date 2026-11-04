namespace Core.Formatting;

using QRCoder;

/// <summary>
/// Generador de códigos QR bidimensionales en bytes PNG conforme a la Regla 2.7.1.7 de la RMF.
/// </summary>
public static class SatQrCodeGenerator
{
    public static byte[] GeneratePngBytes(string content, int pixelsPerModule = 10)
    {
        using QRCodeGenerator qrGenerator = new();
        using QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
        using PngByteQRCode qrCode = new(qrCodeData);

        return qrCode.GetGraphic(pixelsPerModule, false);
    }
}
