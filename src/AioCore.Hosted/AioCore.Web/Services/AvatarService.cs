using System.Drawing;
using System.Drawing.Imaging;
using Devcorner.NIdenticon;
using Devcorner.NIdenticon.BrushGenerators;

namespace AioCore.Web.Services;

public interface IAvatarService
{
    MemoryStream Generate(int dimension, string text);
}

public class AvatarService : IAvatarService
{
    public MemoryStream Generate(int dimension, string text)
    {
        var generator = new IdenticonGenerator
        {
            DefaultBrushGenerator =
                new StaticColorBrushGenerator(StaticColorBrushGenerator.ColorFromText(text)),
            DefaultBackgroundColor = Color.Transparent
        };
        var qrCodeImage = generator.Create(text, new Size(dimension, dimension));
        var outputStream = new MemoryStream();
        qrCodeImage.Save(outputStream, ImageFormat.Png);
        outputStream.Seek(0, SeekOrigin.Begin);
        return outputStream;
    }
}