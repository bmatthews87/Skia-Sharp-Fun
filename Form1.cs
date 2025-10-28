using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Plasma_Test
{
    public partial class Form1 : Form
    {
        int windowHeight = 500;
        int windowWidth = 500;
        SKBitmap screen;

        public Form1()
        {
            InitializeComponent();

            SetupWindow();

            Timer timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 1;
            timer.Start();

            //store initial screen pixels
            screen = new SKBitmap(windowWidth, windowHeight);
        }

        private void SetupWindow()
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Size = new Size(windowWidth, windowHeight);

            skglControl1.Location = new Point(0, 0);
            skglControl1.Size = new Size(windowWidth, windowHeight);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            skglControl1.Invalidate();
        }

        private void skglControl1_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            e.Surface.Canvas.Clear(SKColor.FromHsl(0, 0, 0));

            DrawPlasma(sender, e);
        }

        private void DrawPlasma(object sender, SKPaintGLSurfaceEventArgs e)
        {
            IntPtr pixelsPtr = screen.GetPixels();

            //write directly to buffer
            unsafe
            {
                for (int x = 0; x < windowWidth; x++)
                {
                    for (int y = 0; y < windowHeight; y++)
                    {
                        uint* pixelData = (uint*)pixelsPtr;
                        pixelData[y * screen.Width + x] = (uint)GetPlasmaColor(x, y);
                        //screen.SetPixel(x, y, GetPlasmaColor(x, y));
                    }
                }

                e.Surface.Canvas.DrawBitmap(screen, new SKPoint(0, 0));
            }
        }

        private SKColor GetPlasmaColor(int x, int y)
        {
            double plasmaValue =
                Math.Sin(x * 0.05 + DateTime.Now.Second * 0.1) +
                Math.Sin(y * 0.03 + DateTime.Now.Second * 0.05) +
                Math.Sin((x + y) * 0.04 + DateTime.Now.Second * 0.08);

            SKColor color = new SKColor((byte)0, (byte)0, (byte)(plasmaValue * (255 - 1)));

            return color;
        }
    }
}
