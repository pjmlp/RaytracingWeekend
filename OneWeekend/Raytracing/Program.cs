using RaytracingUtils;
using System.Numerics;
using static RaytracingUtils.MathUtils;
using static RaytracingUtils.Scene;


static Vector3 RayColor(Ray r, IHittable world, int depth)
{
    // If we've exceeded the ray bounce limit, no more light is gathered.
    if (depth <= 0)
    {
        return Vector3.Zero;
    }

    HitRecord rec = default;
    if (world.Hit(r, 0.001f, Single.PositiveInfinity, ref rec))
    {
        Ray scattered = new();
        Vector3 attenuation = default;
        if (rec.Material.Scatter(r, rec, ref attenuation, out scattered))
            return attenuation * RayColor(scattered, world, depth-1);
        return Vector3.Zero;
    }

    Vector3 unitDirection = Vector3.Normalize(r.Direction);
    float t = 0.5f * (unitDirection.Y + 1.0f);
    return (1.0f - t) * Vector3.One + t * new Vector3(0.5f, 0.7f, 1.0f);
}

// Image
const float aspectRatio = 3.0f / 2.0f;
const int imageWidth = 1200;
const int imageHeight = (int)(imageWidth / aspectRatio);
const int samplesPerPixel = 500;
const int maxDepth = 50;

const int bytesPerPixel = 3;
var imageBuffer = new RenderBuffer(imageWidth, imageHeight, bytesPerPixel);

// World
var world = RandomScene();

var lookfrom = new Vector3(13,2,3);
var lookat = new Vector3(0,0,0);
var vup = new Vector3(0,1,0);
var distToFocus = 10;
float aperture = 0.1f;

var cam = new Camera(lookfrom, lookat, vup, 20, aspectRatio, aperture, distToFocus);

int counter = imageHeight - 1;
Parallel.For(0, imageHeight - 1, index =>
{
    int y = index;
    int j = imageHeight - index - 1;
    var remaining = Interlocked.Decrement(ref counter);
    Console.Error.Write($"\rScanlines remaining: {remaining} ");

    for (int i = 0; i < imageWidth; i++)
    {
        var pixelColor = Vector3.Zero;

        for (int s = 0; s < samplesPerPixel; ++s)
        {
            var u = (i + RandomDouble()) / (imageWidth - 1);
            var v = (j + RandomDouble()) / (imageHeight - 1);
            Ray r = cam.GetRay(u, v);
            pixelColor += RayColor(r, world, maxDepth);
        }

        imageBuffer.WriteColor(i, y, pixelColor, samplesPerPixel);
    }
});

Console.Error.WriteLine();

ImageWriter.SaveAsBmp("demo.bmp", imageBuffer.GetBuffer(), imageWidth, imageHeight);
