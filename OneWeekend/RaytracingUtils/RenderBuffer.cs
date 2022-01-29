namespace RaytracingUtils;

using System;
using System.Numerics;

/// <summary>
/// Takes care of the render buffer where the image is generated
/// </summary>
public class RenderBuffer
{
    private byte[] imageBuffer;
    public int BytesPerPixel { get; }
    public int ImageWidth { get; }
    public int ImageHeigth { get; }

    public RenderBuffer(int imageWidth, int imageHeight, int bytesPerPixel)
    {
        ImageWidth = imageWidth;
        ImageHeigth = imageHeight;
        BytesPerPixel = bytesPerPixel;

        imageBuffer = new byte[imageWidth * imageHeight * bytesPerPixel];
    }

    public void WriteColor(int x, int y, in Vector3 col, int samplesPerPixel)
    {
        float r = col.X;
        float g = col.Y;
        float b = col.Z;


        // Divide the color by the number of samples.
        var scale = 1.0f / samplesPerPixel;
        r *= scale;
        g *= scale;
        b *= scale;

        int current = (y * ImageWidth * BytesPerPixel) + (x * BytesPerPixel);
        imageBuffer[current] = (byte)(256 * Math.Clamp(r, 0.0f, 0.999f));
        imageBuffer[current + 1] = (byte)(256 * Math.Clamp(g, 0.0f, 0.999f));
        imageBuffer[current + 2] = (byte)(256 * Math.Clamp(b, 0.0f, 0.999f));
    }

    public ReadOnlySpan<byte> GetBuffer() => imageBuffer;
}

