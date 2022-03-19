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

        // Returns a random single in [0,1).
        public static float RandomFloat() => Random.Shared.NextSingle();

        // Returns a random real in [min,max).
        public static double RandomDouble(double min, double max) => min + (max - min) * Random.Shared.NextDouble();

        public static float RandomFloat(float min, float max) => min + (max - min) * Random.Shared.NextSingle();

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

        public static Vector3 RandomInUnitDisk()
        {
            while(true)
            {
                var p = new Vector3 (RandomFloat(-1, 1), RandomFloat(-1, 1), 0f);
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

        public static Vector3 Reflect(in Vector3 v, in Vector3 n) {
            return v - 2*Vector3.Dot(v,n)*n;
        }

        public static Vector3 Refract(in Vector3 uv, in Vector3 n, float etaiOverEtat) {
            var cosTheta = MathF.Min(Vector3.Dot(-uv, n), 1.0f);
            var rOutPerp =  etaiOverEtat * (uv + cosTheta*n);
            var rOutParallel = -MathF.Sqrt(MathF.Abs(1.0f - rOutPerp.LengthSquared())) * n;
            return rOutPerp + rOutParallel;
        } 

      
    }
}