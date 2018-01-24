using Atmos.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atmos
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Controls.Count; i++)
                if(Controls[i].Name != "button1")
                    Controls.Remove(Controls[i]);
            Console.Clear();
            Stopwatch s = new Stopwatch();
            s.Start();
            Bitmap bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format24bppRgb);
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);

            int size = 192;
            int count = Screen.PrimaryScreen.Bounds.Width / size;
            for (int i = 0; i < count; i++)
            {
                Bitmap b = bmpScreenshot.Clone(new Rectangle(i * size, 0, size, size), PixelFormat.Format24bppRgb);
                Clipboard.SetImage((Image)b);
                Panel p = new Panel()
                {
                    Location = new Point(i * 20, 0),
                    Size = new Size(20, 20),
                    BackColor = Helper.getDominantColor((b)),
                    Name = "panel" + i
                };
                Controls.Add(p);
            }
            
            Console.WriteLine(s.ElapsedMilliseconds);
            s.Stop();
        }
    }
}
