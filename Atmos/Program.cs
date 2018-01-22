using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

/// <summary>
/// The .NET Core project will be the translator to gather information to pass the RPI or Arduino the RGB values that it needs to set the LED's to
/// Right now I'm experimenting with different methods to get the most effecient on your processor with the fastest speeds
/// </summary>
namespace Atmos
{
    class Program
    {

        static void Main(string[] args)
        {
            screenSize = new Size(1920, 1080);
            List<int> fullElapsed = new List<int>();
            List<int> condensedElapsed = new List<int>();

            // Stopwatch for seeing how fast this is
            Stopwatch stopwatch = new Stopwatch();
            for (int i = 0; i < 1000; i++)
            {
                stopwatch.Start();

                if (i % 2 == 0)
                    GetFullColor();
                else
                    GetCondensedColor();
                    
                stopwatch.Stop();

                
                Console.WriteLine("Elapsed Time: {0}", stopwatch.ElapsedMilliseconds.ToString());
                if (i % 2 == 0)
                    fullElapsed.Add((int)stopwatch.ElapsedMilliseconds);
                else
                    condensedElapsed.Add((int)stopwatch.ElapsedMilliseconds);
                stopwatch.Reset();
                //System.Threading.Thread.Sleep(1000);
            }
            Console.WriteLine("Full Average Elapsed Time: {0}", fullElapsed.Average());
            Console.WriteLine("Condensed Average Elapsed Time: {0}", condensedElapsed.Average());
            Console.ReadKey();
        }

        static Size screenSize;


        /// 0 1 2
        /// 3   4
        /// 5 6 7
        private static Color[] GetFullColor()
        {
            Color[] colors = new Color[8];
            colors[0] = GetPixelColor(0, 0);
            colors[1] = GetPixelColor((int)(screenSize.Width / 2), 0);
            colors[2] = GetPixelColor(screenSize.Width, 0);
            colors[3] = GetPixelColor(0, (int)(screenSize.Height / 2));
            colors[4] = GetPixelColor(screenSize.Width, (int)(screenSize.Height / 2));
            colors[5] = GetPixelColor(0, screenSize.Height);
            colors[6] = GetPixelColor((int)(screenSize.Width / 2), screenSize.Height);
            colors[7] = GetPixelColor(screenSize.Width, screenSize.Height);

            return colors;
        }

        ///  0 1
        /// 2   3
        ///  4 5
        private static Color[] GetCondensedColor()
        {
            Color[] colors = new Color[6];

            colors[0] = GetPixelColor(screenSize.Width * (1 / 3), 0);
            colors[1] = GetPixelColor(screenSize.Width * (2 / 3), 0);
            colors[2] = GetPixelColor(0, (int)(screenSize.Height / 2));
            colors[3] = GetPixelColor(screenSize.Width, (int)(screenSize.Height / 2));
            colors[4] = GetPixelColor(screenSize.Width * (1 / 3), screenSize.Height);
            colors[5] = GetPixelColor(screenSize.Width * (2 / 3), screenSize.Height);

            return colors;
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        public static Color GetPixelColor(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                         (int)(pixel & 0x0000FF00) >> 8,
                         (int)(pixel & 0x00FF0000) >> 16);
            return color;
        }
    }
}
