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
    public Camera(double vfov, double aspectRatio)
    {
        var theta = (float)DegreesToRadians(vfov);
        var h = MathF.Tan(theta/2.0f);
        var viewportHeight = 2.0f * h;
        var viewportWidth = (float)aspectRatio * viewportHeight;

        var focalLength = 1.0f;

        origin = Vector3.Zero;
        horizontal = new Vector3(viewportWidth, 0.0f, 0.0f);
        vertical = new Vector3(0.0f, viewportHeight, 0.0f);
        lower_left_corner = origin - horizontal / 2 - vertical / 2 - new Vector3(0.0f, 0.0f, focalLength);
    }

    public Ray GetRay(double u, double v)
    {
        return new Ray(origin, lower_left_corner + ((float)u) * horizontal + ((float)v) * vertical - origin);
    }
}