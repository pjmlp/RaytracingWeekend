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
const float aspectRatio = 16.0f / 9.0f;
const int imageWidth = 400;
const int imageHeight = (int)(imageWidth / aspectRatio);
const int samplesPerPixel = 100;
const int maxDepth = 50;

const int bytesPerPixel = 3;
var imageBuffer = new RenderBuffer(imageWidth, imageHeight, bytesPerPixel);

// World
var world = new HittableList();

var materialGround = new Lambertian(new Vector3(0.8f, 0.8f, 0.0f));
var materialCenter = new Lambertian(new Vector3(0.7f, 0.3f, 0.3f));
var materialLeft = new Metal(new Vector3(0.8f, 0.8f, 0.8f), 0.3f);
var materialRight = new Metal(new Vector3(0.8f, 0.6f, 0.2f), 1.0f);

world.Add(new Sphere(new Vector3( 0.0f, -100.5f, -1.0f), 100, materialGround));
world.Add(new Sphere(new Vector3( 0.0f,    0.0f, -1.0f), 0.5f, materialCenter));
world.Add(new Sphere(new Vector3(-1.0f,    0.0f, -1.0f), 0.5f, materialLeft));
world.Add(new Sphere(new Vector3( 1.0f,    0.0f, -1.0f), 0.5f, materialRight));

// Camera
var cam = new Camera();

for (int j = imageHeight - 1, y = 0; j >= 0; j--, y++)
//Parallel.For(0, imageHeight - 1, index =>
{
    //int y = index;
    //int j = imageHeight - index - 1;
    Console.Error.Write($"\rScanlines remaining: {j} ");

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
} //);

Console.Error.WriteLine();

ImageWriter.SaveAsBmp("demo.bmp", imageBuffer.GetBuffer(), imageWidth, imageHeight);
