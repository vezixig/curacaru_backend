namespace Curacaru.Backend.Infrastructure.Services.Implementations;

using SkiaSharp;

internal class ImageService : IImageService
{
    public string ReduceImage(string base64Image)
    {
        // Base64 encoded image string
        var imageBytes = Convert.FromBase64String(base64Image[22..]);

        // Decode the base64 encoded image
        using var bitmap = SKBitmap.Decode(imageBytes);
        var firstRow = FindFirstNonTransparentRow(bitmap);
        var lastRow = FindLastNonTransparentRow(bitmap);

        var firstColumn = FindFirstNonTransparentColumn(bitmap);
        var lastColumn = FindLastNonTransparentColumn(bitmap);

        // crop image
        var croppedBitmap = new SKBitmap(lastColumn - firstColumn + 1, lastRow - firstRow + 1);
        for (var x = firstColumn; x <= lastColumn; x++)
        for (var y = firstRow; y <= lastRow; y++)
        {
            var pixelColor = bitmap.GetPixel(x, y);
            croppedBitmap.SetPixel(x - firstColumn, y - firstRow, pixelColor);
        }

        // convert back to base64
        using var image = SKImage.FromBitmap(croppedBitmap);
        using var data = image.Encode();
        var result = Convert.ToBase64String(data.ToArray());
        return $"data:image/png;base64,{result}";
    }

    private static SKImage CropImage(SKBitmap bitmap, int firstRow, int lastRow)
    {
        var width = bitmap.Width;
        var height = lastRow - firstRow + 1;
        var croppedBitmap = new SKBitmap(width, height);

        using (var canvas = new SKCanvas(croppedBitmap)) { canvas.DrawBitmap(bitmap, 0, -firstRow); }

        return SKImage.FromBitmap(croppedBitmap);
    }

    private int FindFirstNonTransparentColumn(SKBitmap bitmap)
    {
        for (var x = 0; x < bitmap.Width; x++)
        for (var y = 0; y < bitmap.Height; y++)
            if (bitmap.GetPixel(x, y).Alpha != 0)
                return x;

        return 0;
    }

    private int FindFirstNonTransparentRow(SKBitmap bitmap)
    {
        for (var y = 0; y < bitmap.Height; y++)
        for (var x = 0; x < bitmap.Width; x++)
            if (bitmap.GetPixel(x, y).Alpha != 0)
                return y;

        return 0;
    }

    private int FindLastNonTransparentColumn(SKBitmap bitmap)
    {
        for (var x = bitmap.Width - 1; x >= 0; x--)
        for (var y = 0; y < bitmap.Height; y++)
            if (bitmap.GetPixel(x, y).Alpha != 0)
                return x;

        return bitmap.Width - 1;
    }

    private int FindLastNonTransparentRow(SKBitmap bitmap)
    {
        for (var y = bitmap.Height - 1; y >= 0; y--)
        for (var x = 0; x < bitmap.Width; x++)
            if (bitmap.GetPixel(x, y).Alpha != 0)
                return y;

        return bitmap.Height - 1;
    }
}