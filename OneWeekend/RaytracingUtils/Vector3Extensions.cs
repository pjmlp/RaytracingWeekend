using System;
using System.Numerics;

namespace RaytracingUtils;
public static class Vector3Extensions
{
    public static bool NearZero (this Vector3 vect)
    {
        // Return true if the vector is close to zero in all dimensions.
        const float s = 1e-8F;
        return (Math.Abs(vect.X) < s) && (Math.Abs(vect.Y)  < s) && (Math.Abs(vect.Z) < s);
    }
}