namespace RaytracingUtils;
using System.Numerics;
using static RaytracingUtils.MathUtils;

public static class Scene
{
    public static HittableList RandomScene()
    {
        var world = new HittableList();

        for (int a = -11; a < 11; a++)
        {
            for (int b = -11; b < 11; b++)
            {
                var choose_mat = RandomFloat();
                var center = new Vector3(a + 0.9f * RandomFloat(), 0.2f, b + 0.9f * RandomFloat());

                if ((center -  new Vector3(4, 0.2f, 0)).Length() > 0.9)
                {
                    if (choose_mat < 0.8f)
                    {
                        // diffuse
                        var albedo = RandomVector() * RandomVector();

                        var sphereMaterial = new Lambertian(albedo);
                        world.Add(new Sphere(center, 0.2, sphereMaterial));
                    }
                    else if (choose_mat < 0.95)
                    {
                        // metal
                        var albedo = RandomVector(0.5, 1);
                        var fuzz = RandomFloat(0, 0.5f);

                        var sphereMaterial = new Metal(albedo, fuzz);
                        world.Add(new Sphere(center, 0.2, sphereMaterial));

                    }
                    else
                    {
                        // glass
                        var sphereMaterial = new Dielectric(1.5f);
                        world.Add(new Sphere(center, 0.2, sphereMaterial));

                    }
                }
            }
        }

        var materialGround = new Lambertian(new Vector3(0.5f, 0.5f, 0.5f));
        world.Add(new Sphere(new Vector3(0,-1000,0), 1000, materialGround));


        var material1 = new Dielectric(1.5f);
        world.Add(new Sphere(new Vector3(0, 1, 0), 1.0, material1));

        var material2 = new Lambertian(new Vector3(0.4f, 0.2f, 0.1f));
        world.Add(new Sphere(new Vector3(-4, 1, 0), 1.0, material2));

        var material3 = new Metal(new Vector3(0.7f, 0.6f, 0.5f), 0.0f);
        world.Add(new Sphere(new Vector3(4, 1, 0), 1.0, material3));

        return world;
    }
}