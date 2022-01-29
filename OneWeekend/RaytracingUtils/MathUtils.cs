using System;
using System.Numerics;

namespace RaytracingUtils
{
    /// <summary>
    /// General purpose math utility functions.
    /// </summary>
    public static class MathUtils
    {
        public static double DegreesToRadians(double degrees) => degrees * Math.PI / 180.0;

        // Returns a random real in [0,1).
        public static double RandomDouble() => Random.Shared.NextDouble();

        // Returns a random real in [min,max).
        public static double RandomDouble(double min, double max) => min + (max - min) * Random.Shared.NextDouble();

        // Returns a vector with coordinates using random real in [0,1).
        public static Vector3 RandomVector() => new Vector3((float)RandomDouble(), (float)RandomDouble(), (float)RandomDouble());

         // Returns a vector with coordinates using random real in [[min,max).
        public static Vector3 RandomVector(double min, double max) => new Vector3((float)RandomDouble(min, max), (float)RandomDouble(min, max), (float)RandomDouble(min, max));

        public static Vector3 RandomInUnitSphere()
        {
            while(true)
            {
                var p = RandomVector(-1, 1);
                if (p.LengthSquared() >= 1) continue;
                return p;
            }
        }

        public static Vector3 RandomUnitVector() => Vector3.Normalize(RandomVector());

        public static Vector3 RandomInHemisphere(in Vector3 normal)
        {
            var in_unit_sphere = RandomInUnitSphere();
            if (Vector3.Dot(in_unit_sphere, normal) > 0.0) // In the same hemisphere as the normal
            {
                return in_unit_sphere;
            }
            else
            {
                return -in_unit_sphere;
            }
        }
    }
}