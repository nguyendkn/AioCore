using System.Drawing;
using System.Drawing.Imaging;

namespace AioCore.Shared.Extensions;

public static class ImageExtensions
{
    public static byte[] ImageToByte2(this Image img, ImageFormat imageFormat)
    {
        using var stream = new MemoryStream();
        img.Save(stream, imageFormat);
        return stream.ToArray();
    }
}