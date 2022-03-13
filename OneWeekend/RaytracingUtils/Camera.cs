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
    private Vector3 lower_left_corner;
    private Vector3 horizontal;
    private Vector3 vertical;

    /// <param name="vfov">vertical field-of-view in degrees</param>
    /// <param name="aspectRatio"></param>
    public Camera(
            Vector3 lookfrom,
            Vector3 lookat,
            Vector3   vup,
            double vfov, double aspectRatio)
    {
        var theta = (float)DegreesToRadians(vfov);
        var h = MathF.Tan(theta/2.0f);
        var viewportHeight = 2.0f * h;
        var viewportWidth = (float)aspectRatio * viewportHeight;

        var w = Vector3.Normalize(lookfrom - lookat);
        var u = Vector3.Normalize(Vector3.Cross(vup, w));
        var v = Vector3.Cross(w, u);

        origin = lookfrom;
        horizontal = viewportWidth * u;
        vertical = viewportHeight * v;
        lower_left_corner = origin - horizontal/2 - vertical/2 - w;
    }

    public Ray GetRay(double s, double t)
    {
        return new Ray(origin, lower_left_corner + ((float)s) * horizontal + ((float)t) * vertical - origin);
    }
}