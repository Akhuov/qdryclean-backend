using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace QDryClean.Application.Common.Helpers
{
    public static class EscPosQrHelper
    {
        public static byte[] ConvertQrToEscPos(byte[] png)
        {
            using var img = Image.Load<Rgba32>(png);
            img.Mutate(x => x.BinaryDither(KnownDitherings.FloydSteinberg));

            var width = img.Width;
            var height = img.Height;
            var result = new List<byte>();

            for (int y = 0; y < height; y += 24)
            {
                result.AddRange(new byte[] { 0x1B, 0x2A, 33, (byte)(width & 0xFF), (byte)((width >> 8) & 0xFF) });
                for (int x = 0; x < width; x++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        byte slice = 0;
                        for (int b = 0; b < 8; b++)
                        {
                            int yPos = y + k * 8 + b;
                            if (yPos >= height) continue;
                            var pixel = img[x, yPos];
                            if (pixel.R < 128) slice |= (byte)(1 << (7 - b));
                        }
                        result.Add(slice);
                    }
                }
                result.Add(0x0A);
            }

            return result.ToArray();
        }
    }
}