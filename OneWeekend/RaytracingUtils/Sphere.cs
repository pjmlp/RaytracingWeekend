using System;
using System.Numerics;

namespace RaytracingUtils
{
    public class Sphere : IHittable
    {
        Vector3 center;
        readonly double radius;

        public Sphere(Vector3 cen, double r)
        {
            center = cen;
            radius = r;
        }

        public bool Hit(Ray r, float tMin, float tMax, ref HitRecord rec)
        {
            var oc = r.Origin - center;
            var a = r.Direction.LengthSquared();
            var halfB = Vector3.Dot(oc, r.Direction);
            var c = oc.LengthSquared() - radius * radius;

            var discriminant = halfB * halfB - a * c;
            if (discriminant < 0) return false;
            var sqrtd = Math.Sqrt(discriminant);

            // Find the nearest root that lies in the acceptable range.
            var root = (-halfB - sqrtd) / a;
            if (root < tMin || tMax < root)
            {
                root = (-halfB + sqrtd) / a;
                if (root < tMin || tMax < root)
                    return false;
            }

            rec.t = (float)root;
            rec.p = r.At(rec.t);

            Vector3 outwardNormal = (rec.p - center) / (float)radius;
            rec.SetFaceNormal(r, outwardNormal);

            return true;
        }
    }
}
