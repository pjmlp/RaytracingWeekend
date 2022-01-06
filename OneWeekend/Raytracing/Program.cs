namespace Raytracing
{
    /// <summary>
    /// Implementation of the PPM generation as described on the 2nd chapter.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            const int imageWidth = 256;
            const int imageHeight = 256;

            Console.WriteLine($"P3\n{imageWidth} {imageHeight}\n255");

            for (int j = imageHeight - 1; j >= 0; --j)
            {
                for (int i = 0; i < imageWidth; ++i)
                {
                    var r = ((double)(i)) / (imageWidth - 1);
                    var g = ((double)(j)) / (imageHeight - 1);
                    var b = 0.25;

                    int ir = (int)(255.999 * r);
                    int ig = (int)(255.999 * g);
                    int ib = (int)(255.999 * b);

                    Console.WriteLine($"{ir} {ig} {ib}");
                }
            }
        }
    }
}
