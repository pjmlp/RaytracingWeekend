using System.Numerics;

namespace RaytracingUtils;

/// <summary>
/// General purpose scene rays.
/// </summary>
public class Ray
{
    public Vector3 Origin { get; }

    public Vector3 Direction { get; }

    public Ray()
    {
        Origin = Vector3.Zero;
        Direction = Vector3.Zero;
    }

    public Ray(in Vector3 origin, in Vector3 direction)
    {
        Origin = origin;
        Direction = direction;
    }

    public Vector3 At(float t)
    {
        return Origin + t * Direction;
    }
}