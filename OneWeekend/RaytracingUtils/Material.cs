using System;
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
    float fuzz;

    public Metal(in Vector3 a, double f)
    {
        albedo = a;
        fuzz = (float)(f < 1.0f ? f : 1.0f);
    }

    public bool Scatter(Ray rIn, in HitRecord rec, ref Vector3 attenuation, out Ray scattered)
    {
        var reflected = Reflect(Vector3.Normalize(rIn.Direction), rec.Normal);

        scattered = new Ray(rec.p, reflected + fuzz * RandomInUnitSphere());
        attenuation = albedo;
        return Vector3.Dot(scattered.Direction, rec.Normal) > 0.0f;
    }
}

public class Dielectric : IMaterial
{
    float ir; // Index of Refraction

    public Dielectric(double indexOfRefraction)
    {
        ir = (float)indexOfRefraction;
    }

    public bool Scatter(Ray rIn, in HitRecord rec, ref Vector3 attenuation, out Ray scattered)
    {
        attenuation = new Vector3(1.0f, 1.0f, 1.0f);
        float refractionRatio = rec.FrontFace ? (1.0f / ir) : ir;

        Vector3 unit_direction = Vector3.Normalize(rIn.Direction);

        float cos_theta = MathF.Min(Vector3.Dot(-unit_direction, rec.Normal), 1.0f);
        float sin_theta = MathF.Sqrt(1.0f - cos_theta*cos_theta);

        bool cannot_refract = refractionRatio * sin_theta > 1.0;
        Vector3 direction;

        if (cannot_refract)
            direction = Reflect(unit_direction, rec.Normal);
        else
            direction = Refract(unit_direction, rec.Normal, refractionRatio);

        scattered = new Ray(rec.p, direction);

        return true;
    }
}