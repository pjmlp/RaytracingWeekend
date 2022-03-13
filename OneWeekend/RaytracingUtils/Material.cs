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

        Vector3 unitDirection = Vector3.Normalize(rIn.Direction);

        float cosTheta = MathF.Min(Vector3.Dot(-unitDirection, rec.Normal), 1.0f);
        float sinTheta = MathF.Sqrt(1.0f - cosTheta*cosTheta);

        bool cannotRefract = refractionRatio * sinTheta > 1.0;
        Vector3 direction;

        if (cannotRefract || Reflectance(cosTheta, refractionRatio) > RandomDouble())
            direction = Reflect(unitDirection, rec.Normal);
        else
            direction = Refract(unitDirection, rec.Normal, refractionRatio);

        scattered = new Ray(rec.p, direction);

        return true;
    }

    private double Reflectance(float cosine, float refIdx)
    {
        // Use Schlick's approximation for reflectance.
        var r0 = (1-refIdx) / (1+refIdx);
        r0 = r0*r0;

        return r0 + (1.0f - r0) * MathF.Pow((1.0f - cosine),5.0f);
    }
}