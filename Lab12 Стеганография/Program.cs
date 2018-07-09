using System;
using System.Drawing;

namespace Lab12
{
    class Program
    {
        static Color GetNewColor(byte newValue, Color oldColor)
        {
            return Color.FromArgb(newValue, oldColor.G, oldColor.B);
        }
        static void Main()
        {
            var basePath = @"E:\Study\3-2\Криптографические основы защиты информации\Лабы\Crypto_9_13\";
            var picturePath = basePath + "picture.bmp";
            var newPicturePath = basePath + "picture2.bmp";
            Bitmap image = new Bitmap(picturePath);
            var image2 = image;
            var text = "Hello world from JUDE";

            // write
            for (int i = 0; i < text.Length; i++)
            {
                var wd = i / image2.Width;
                var hg = i % image2.Width;
                Color oldColor = image2.GetPixel(wd, hg);
                image2.SetPixel(wd, hg, GetNewColor((byte)text[i], oldColor));
            }

            {
                var wd = text.Length / image2.Width;
                var hg = text.Length % image2.Width;
                Color oldColor = image2.GetPixel(wd, hg);
                image2.SetPixel(wd, hg, GetNewColor(255, oldColor));
            }

            image2.Save(newPicturePath);
            var image3 = new Bitmap(newPicturePath);
            var result = "";
            for (int i = 0; i < image3.Width && image3.GetPixel(0, i).R != 255; i++)
            {
                result += (char)image3.GetPixel(0, i).R;
            }

            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
