using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RaytracingUtils;

// no need for the warning, we use the open source version of System.Drawing
// on other platforms
#pragma warning disable CA1416

/// <summary>
/// Utility class to save the renderings as proper images.
/// </summary>
public static class ImageWriter
{
    /// <summary>
    /// Saves the given buffer as a BMP.
    /// </summary>
    /// <param name="filename">The full pathname for the filename being written</param>
    /// <param name="buffer">The image data stored as RGB buffer.</param>
    /// <param name="width">The image width in pixels.</param>
    /// <param name="height">The image height in pixels.</param>
    public static void SaveAsBmpBuffer(string filename, byte[] buffer, int width, int height)
    {
        // https://stackoverflow.com/questions/47918451/creating-a-bitmap-from-rgb-array-in-c-result-image-difference/47919041

        // https://www.programmersought.com/article/82161417092/
        var bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        Rectangle BoundsRect = new Rectangle(0, 0, width, height);
        BitmapData bmpData = bitmap.LockBits(BoundsRect,
                                        ImageLockMode.WriteOnly,
                                        bitmap.PixelFormat);

        // add back dummy bytes between lines, make each line be a multiple of 4 bytes
        int skipByte = bmpData.Stride - width * 3;
        byte[] newBuff = new byte[buffer.Length + skipByte * height];
        for (int j = 0; j < height; j++)
        {
            Buffer.BlockCopy(buffer, j * width * 3, newBuff, j * (width * 3 + skipByte), width * 3);
        }

        // fill in rgbValues
        Marshal.Copy(newBuff, 0, bmpData.Scan0, newBuff.Length);
        bitmap.UnlockBits(bmpData);
        bitmap.Save(filename, ImageFormat.Bmp);
    }

    /// <summary>
    /// Saves the given buffer as a BMP.
    /// </summary>
    /// <param name="filename">The full pathname for the filename being written</param>
    /// <param name="buffer">The image data stored as RGB buffer.</param>
    /// <param name="width">The image width in pixels.</param>
    /// <param name="height">The image height in pixels.</param>
    public static void SaveAsBmp(string filename, ReadOnlySpan<byte> buffer, int width, int height)
    {
        // Not the best alternative, but it works out for the purpose 
        var bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);


        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int current = (y * width * 3) + (x * 3);
                bitmap.SetPixel(x, y, Color.FromArgb(buffer[current], buffer[current + 1], buffer[current + 2]));
            }
        }
                    
        bitmap.Save(filename, ImageFormat.Bmp);
    }
}

