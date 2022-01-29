using System;

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
    }
}