using SkiaSharp;

namespace ForgejoApiClient.Tests.helper;

public static class TestResourceGenerator
{
    public static byte[] CreateTextImage(string text, int imgWidth = 200, int imgHeight = 150, uint fgcolor = 0xFF000000, uint bgcolor = 0xFF808080, string format = "png")
        => CreateImage((canvas, painter, font) => canvas.DrawText(text, 5f, font.Size / 2 + imgHeight / 2, font, painter), imgWidth, imgHeight, fgcolor, bgcolor, format);

    private static byte[] CreateImage(Action<SKCanvas, SKPaint, SKFont> drawer, int width, int height, uint fgcolor, uint bgcolor, string format)
    {
        using var surface = SKSurface.Create(new SKImageInfo(width, height, SKColorType.Rgba8888));
        var paint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            Color = new SKColor(fgcolor),
        };
        surface.Canvas.Clear(new SKColor(bgcolor));
        drawer(surface.Canvas, paint, new SKFont());
        using var image = surface.Snapshot();
        using var data = image.Encode(Enum.Parse<SKEncodedImageFormat>(format, ignoreCase: true), 100);

        return data.ToArray();
    }

}
