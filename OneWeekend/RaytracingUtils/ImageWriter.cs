using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;

namespace RaytracingUtils;

/// <summary>
/// Utility class to save the renderings as proper images.
/// </summary>
public static class ImageWriter
{
    /// <summary>
    /// Saves the given buffer using the filename extension as format.
    /// </summary>
    /// <param name="filename">The full pathname for the filename being written</param>
    /// <param name="buffer">The image data stored as RGB buffer.</param>
    /// <param name="width">The image width in pixels.</param>
    /// <param name="height">The image height in pixels.</param>
    public static void SaveAsBmp(string filename, ReadOnlySpan<byte> buffer, int width, int height)
    {
        using var image = Image.LoadPixelData<Rgb24>(buffer, width, height);
        image.Save(filename);
    }
}

