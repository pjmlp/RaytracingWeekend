using System.Collections.Generic;
using System.Numerics;
using static RaytracingUtils.MathUtils;

namespace RaytracingUtils;

public class Lambertian : IMaterial
{
    Vector3 albedo;

    public Lambertian(in Vector3 a)
    {
        albedo = a;
    }

    public bool Scatter(Ray rIn, in HitRecord rec, ref Vector3 attenuation, out Ray scattered)
    {
        var scatterDirection = rec.Normal + RandomUnitVector();

        // Catch degenerate scatter direction
        if (scatterDirection.NearZero())
        {
            scatterDirection = rec.Normal;
        }

        scattered = new Ray(rec.p, scatterDirection);
        attenuation = albedo;
        return true;
    }
}


public class Metal : IMaterial
{
    Vector3 albedo;

    public Metal(in Vector3 a)
    {
        albedo = a;
    }

    public bool Scatter(Ray rIn, in HitRecord rec, ref Vector3 attenuation, out Ray scattered)
    {
        var reflected = Reflect(Vector3.Normalize(rIn.Direction), rec.Normal);

        scattered = new Ray(rec.p, reflected);
        attenuation = albedo;
        return Vector3.Dot(scattered.Direction, rec.Normal) > 0.0f;
    }
}