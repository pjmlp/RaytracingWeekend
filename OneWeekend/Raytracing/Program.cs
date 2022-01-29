using RaytracingUtils;
using System.Numerics;
using static RaytracingUtils.MathUtils;


static Vector3 RayColor(Ray r, IHittable world, int depth)
{
        // If we've exceeded the ray bounce limit, no more light is gathered.
    if (depth <= 0)
    {
        return Vector3.Zero;
    }

    HitRecord rec = default;
    if (world.Hit(r, 001f, Single.PositiveInfinity, ref rec))
    {
        Vector3 target = rec.p + RandomInHemisphere(rec.Normal);
        return 0.5f * RayColor(new Ray(rec.p, target - rec.p), world, depth - 1);
    }

    Vector3 unitDirection = Vector3.Normalize(r.Direction);
    float t = 0.5f * (unitDirection.Y + 1.0f);
    return (1.0f - t) * Vector3.One + t * new Vector3(0.5f, 0.7f, 1.0f);
}

// Image
const int maxDepth = 50;
const int samplesPerPixel = 100;
const int bytesPerPixel = 3;
const int imageWidth = 200;
const int imageHeight = 100;
var imageBuffer = new RenderBuffer(imageWidth, imageHeight, bytesPerPixel);

// World
var world = new HittableList();
world.Add(new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5));
world.Add(new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100));

// Camera
var cam = new Camera();

for (int j = imageHeight - 1, y = 0; j >= 0; j--, y++)
{
    Console.Error.Write($"\rScanlines remaining: {j}");

    for (int i = 0; i < imageWidth; i++)
    {
        var pixelColor = Vector3.Zero;

        for (int s = 0; s < samplesPerPixel; ++s) {
            var u = (i + RandomDouble()) / (imageWidth-1);
            var v = (j + RandomDouble()) / (imageHeight-1);
            Ray r = cam.GetRay(u, v);
            pixelColor += RayColor(r, world, maxDepth);
        }

        imageBuffer.WriteColor(i, y, pixelColor, samplesPerPixel);
    }
}

Console.Error.WriteLine();

ImageWriter.SaveAsBmp("demo.bmp", imageBuffer.GetBuffer(), imageWidth, imageHeight);
