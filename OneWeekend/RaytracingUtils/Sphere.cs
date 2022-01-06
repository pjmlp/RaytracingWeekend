using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RaytracingUtils
{
    public class Sphere : IHitable
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
            var half_b = Vector3.Dot(oc, r.Direction);
            var c = oc.LengthSquared() - radius * radius;

            var discriminant = half_b * half_b - a * c;
            if (discriminant < 0) return false;
            var sqrtd = Math.Sqrt(discriminant);

            // Find the nearest root that lies in the acceptable range.
            var root = (-half_b - sqrtd) / a;
            if (root < tMin || tMax < root)
            {
                root = (-half_b + sqrtd) / a;
                if (root < tMin || tMax < root)
                    return false;
            }

            rec.t = (float)root;
            rec.p = r.At(rec.t);
            rec.Normal = (rec.p - center) / (float)radius;

            return true;
        }
    }
}
