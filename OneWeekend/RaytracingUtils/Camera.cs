namespace RaytracingUtils;

using System;
using System.Numerics;
using static RaytracingUtils.MathUtils;

/// <summary>
/// Scene camera
/// </summary>
public class Camera
{
    private Vector3 origin;
    private Vector3 lowerLeftCorner;
    private Vector3 horizontal;
    private Vector3 vertical;

    private Vector3 u, w, v;

    private float lensRadius;

    /// <param name="vfov">vertical field-of-view in degrees</param>
    /// <param name="aspectRatio"></param>
    public Camera(
            Vector3 lookfrom,
            Vector3 lookat,
            Vector3   vup,
            double vfov,
            double aspectRatio,
            float aperture,
            float focusDist)
    {
        var theta = (float)DegreesToRadians(vfov);
        var h = MathF.Tan(theta/2.0f);
        var viewportHeight = 2.0f * h;
        var viewportWidth = (float)aspectRatio * viewportHeight;

        w = Vector3.Normalize(lookfrom - lookat);
        u = Vector3.Normalize(Vector3.Cross(vup, w));
        v = Vector3.Cross(w, u);

        origin = lookfrom;
        horizontal = focusDist * viewportWidth * u;
        vertical = focusDist * viewportHeight * v;
        lowerLeftCorner = origin - horizontal/2 - vertical/2 - focusDist * w;

        lensRadius = aperture / 2.0f;
    }

    public Ray GetRay(double s, double t)
    {
        Vector3 rd = lensRadius * RandomInUnitDisk();
        Vector3 offset = u * rd.X + v * rd.Y;

        return new Ray(origin + offset, lowerLeftCorner + ((float)s) * horizontal + ((float)t) * vertical - origin - offset);
    }
}