using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImageProcessingLibrary;

namespace FaceRecognitionTraining
{
    public class IsaksImageProcessor: ImageProcessor
    {
        public IsaksImageProcessor(Bitmap bitmap) : base(bitmap)
        { 

        }

        public double[] AsGrayVector()
        {
            double[] imageAsGrayVector = new double[bitmap.Width * bitmap.Height];
            unsafe
            {
                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;
                Parallel.For(0, bitmapData.Height, y =>
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    int iX = 0;
                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        imageAsGrayVector[iX + y*bitmapData.Width] = (double)currentLine[x];
                        iX++;
                    }
                });
            }
            return imageAsGrayVector;
        }



    }
}
