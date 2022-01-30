using System.Collections.Generic;
using System.Numerics;

namespace RaytracingUtils;

public struct HitRecord
{
    public float t;

    public Vector3 p;

    public Vector3 Normal;

    public bool FrontFace;

    public IMaterial Material;

    public void SetFaceNormal(Ray r, in Vector3 outwardNormal) {
        FrontFace = Vector3.Dot(r.Direction, outwardNormal) < 0;
        Normal = FrontFace ? outwardNormal : -outwardNormal;
        Material = null;
    }
}

public interface IHittable
{
    bool Hit(Ray r, float tMin, float tMax, ref HitRecord rec);
}

public class HittableList : IHittable
{
    private IList<IHittable> objects;

    public HittableList()
    {
        objects = new List<IHittable>();
    }

    public void Clear()
    {
        objects.Clear();
    }

    public void Add(IHittable obj)
    {
        objects.Add(obj);
    }

    public bool Hit(Ray r, float tMin, float tMax, ref HitRecord rec)
    {
        HitRecord temp_rec = default;
        bool hitAnything = false;
        var closestSoFar = tMax;

        foreach (var obj in objects) {
            if (obj.Hit(r, tMin, closestSoFar, ref temp_rec))
            {
                hitAnything = true;
                closestSoFar = temp_rec.t;
                rec = temp_rec;
            }
        }

        return hitAnything;
    }
}

public interface IMaterial {
    bool Scatter(Ray rIn, in HitRecord rec, ref Vector3 attenuation, out Ray scattered);
}
