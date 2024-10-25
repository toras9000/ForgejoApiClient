using SkiaSharp;

namespace ForgejoApiClient.Tests.helper;

public static class TestResourceGenerator
{
    public static byte[] CreateTextImage(string text, int imgWidth = 200, int imgHeight = 150, uint fgcolor = 0xFF000000, uint bgcolor = 0xFF808080, string format = "png")
        => CreateImage((canvas, painter) => canvas.DrawText(text, 5f, painter.TextSize / 2 + imgHeight / 2, painter), imgWidth, imgHeight, fgcolor, bgcolor, format);

    private static byte[] CreateImage(Action<SKCanvas, SKPaint> drawer, int width, int height, uint fgcolor, uint bgcolor, string format)
    {
        using var surface = SKSurface.Create(new SKImageInfo(width, height, SKColorType.Rgba8888));
        var paint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            Color = new SKColor(fgcolor),
        };
        surface.Canvas.Clear(new SKColor(bgcolor));
        drawer(surface.Canvas, paint);
        using var image = surface.Snapshot();
        using var data = image.Encode(Enum.Parse<SKEncodedImageFormat>(format, ignoreCase: true), 100);

        return data.ToArray();
    }

}
