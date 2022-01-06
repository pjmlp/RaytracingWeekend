using System.Numerics;

namespace RaytracingUtils;

public struct HitRecord
{
    public float t;

    public Vector3 p;

    public Vector3 Normal;

    public bool FrontFace;

    void SetFaceNormal(Ray r, in Vector3 outwardNormal) {
        FrontFace = Vector3.Dot(r.Direction, outwardNormal) < 0;
        Normal = FrontFace ? outwardNormal : -outwardNormal;
    }
}

public interface IHitable
{
    bool Hit(Ray r, float tMin, float tMax, ref HitRecord rec);
}
