﻿using RaytracingUtils;
using System.Numerics;


static Vector3 RayColor(Ray r)
{
    Vector3 unitDirection = Vector3.Normalize(r.Direction);
    float t = 0.5f * (unitDirection.Y + 1.0f);
    return (1.0f - t) * Vector3.One + t * new Vector3(0.5f, 0.7f, 1.0f);
}


const int bytesPerPixel = 3;
const int imageWidth = 200;
const int imageHeight = 100;
var imageBuffer = new byte[imageWidth * imageHeight * bytesPerPixel];

var lowerLeftCorner = new Vector3(-2.0f, -1.0f, -1.0f);
var horizontal = new Vector3(4.0f, 0.0f, 0.0f);
var vertical = new Vector3(0.0f, 0.2f, 0.0f);
var origin = new Vector3(0.0f, 0.0f, 0.0f);

for (int j = imageHeight - 1, y = 0; j >= 0; j--, y++)
{
    for (int i = 0; i < imageWidth; i++)
    {
        float u = (float)i / (float)imageWidth;
        float v = (float)j / (float)imageHeight;

        var ray = new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical);
        Vector3 col = RayColor(ray);

        byte ir = (byte)(255.999 * col.X);
        byte ig = (byte)(255.999 * col.Y);
        byte ib = (byte)(255.999 * col.Z);

        int current = (y * imageWidth * bytesPerPixel) + (i * bytesPerPixel);
        imageBuffer[current] = ir;
        imageBuffer[current + 1] = ig;
        imageBuffer[current + 2] = ib;

        //Console.WriteLine($"{ir} {ig} {ib} => {i} {y} {current} => {(y * imageWidth * bytesPerPixel)} {i * bytesPerPixel}");
    }
}


ImageWriter.SaveAsBmp("demo.bmp", imageBuffer, imageWidth, imageHeight);
