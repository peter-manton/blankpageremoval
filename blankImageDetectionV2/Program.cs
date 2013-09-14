using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace blankImageDetectionV2
{
    class Program
    {
        static Image img = Image.FromFile(@"Scan.jpg");
        static Image imgWithoutBorder = null;

        static void Main(string[] args)
        {
            // Removal black border from image
            imgWithoutBorder = RemoveBorder(img, 20);
            Bitmap bmp = new Bitmap(imgWithoutBorder);

            // offendingPoints
            List<string> offendingPoints = new List<string>();

            bmp.Save("help.bmp");

            // How far of (colour) two pixels can be (calculated by RBG value difference)
            int threshold = 75;
            // Number of times the threshold above can be broken until the program decides the image is blank
            int failThreshold = 35;

            // Is image blank?
            bool isBlank = true;
            int deltaOverCount = 0;

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int y = 0; y < bmp.Height - 1; y++)
                {
                    int rValue1 = bmp.GetPixel(i, y).R;
                    int gValue1 = bmp.GetPixel(i, y).G;
                    int bValue1 = bmp.GetPixel(i, y).B;

                    int rValue2 = bmp.GetPixel(i, y + 1).R;
                    int gValue2 = bmp.GetPixel(i, y + 1).G;
                    int bValue2 = bmp.GetPixel(i, y + 1).B;

                    int rDiff = Math.Abs(rValue1 - rValue2);
                    int gDiff = Math.Abs(gValue1 - gValue2);
                    int bDiff = Math.Abs(bValue1 - bValue2);

                    int delta = rDiff + gDiff + bDiff;

                    // File.AppendAllText("out.txt", delta.ToString() + ".");

                    if (delta > threshold)
                    {
                        deltaOverCount++;
                        offendingPoints.Add("HeightY: " + y + " and WidthX: " + i);
                        // break;
                    }
                }
            }

            if (deltaOverCount > failThreshold)
            {
                isBlank = false;
            }

            if (offendingPoints.Count > 0)
            {
                Console.WriteLine("Offending points:");

                foreach (String str in offendingPoints)
                {

                    Console.WriteLine(str);
                }
            }

            if (isBlank == true)
            {
                Console.WriteLine("The image is blank");
            }
            else
            {
                Console.WriteLine("The image is not blank");
            }

            Console.ReadKey();
        }

        private static Image RemoveBorder(Image img, int borderSize)
        {

            Image imageWithoutBorder = null;

            using (Image imageOriginal = img)
            {
                int x = borderSize;
                int y = borderSize;
                int width = imageOriginal.Width - borderSize;
                int height = imageOriginal.Height - borderSize;

                imageWithoutBorder = new Bitmap(width, height);

                using (Graphics g = Graphics.FromImage(imageWithoutBorder))
                {
                    Rectangle rectSource = new Rectangle(0, 0, imageOriginal.Width, imageOriginal.Height);
                    Rectangle rectDestination = new Rectangle(x, y, width, height);

                    g.DrawImage(imageOriginal, rectSource, rectDestination, GraphicsUnit.Pixel);
                }
            }

            return imageWithoutBorder;
        }
    }
}
