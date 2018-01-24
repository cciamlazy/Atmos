using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmos.Classes
{
    public static class Helper
    {
        public static Color getDominantColor(System.Drawing.Bitmap bmp)
        {
            BitmapData srcData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            int stride = srcData.Stride;

            IntPtr Scan0 = srcData.Scan0;

            uint[] totals = new uint[] { 0, 0, 0 };

            uint width = (uint)bmp.Width;
            uint height = (uint)bmp.Height;

            unsafe
            {
                uint* p = (uint*)(void*)Scan0;

                uint pixelCount = width * height;
                uint idx = 0;
                while (idx < (pixelCount & ~0xff))
                {
                    uint sumRR00BB = 0;
                    uint sum00GG00 = 0;
                    for (int j = 0; j < 0x100; j++)
                    {
                        sumRR00BB += p[idx] & 0xff00ff;
                        sum00GG00 += p[idx] & 0x00ff00;
                        idx++;
                    }

                    totals[0] += sumRR00BB >> 16;
                    totals[1] += sum00GG00 >> 8;
                    totals[2] += sumRR00BB & 0xffff;
                }

                // And the final partial block of fewer than 0x100 pixels.
                {
                    uint sumRR00BB = 0;
                    uint sum00GG00 = 0;
                    while (idx < pixelCount)
                    {
                        sumRR00BB += p[idx] & 0xff00ff;
                        sum00GG00 += p[idx] & 0x00ff00;
                        idx++;
                    }

                    totals[0] += sumRR00BB >> 16;
                    totals[1] += sum00GG00 >> 8;
                    totals[2] += sumRR00BB & 0xffff;
                }
            }

            uint avgB = totals[0] / (width * height);
            uint avgG = totals[1] / (width * height);
            uint avgR = totals[2] / (width * height);

            bmp.UnlockBits(srcData);

            return Color.FromArgb((int)avgR, (int)avgG, (int)avgB);
        }
    }
}
