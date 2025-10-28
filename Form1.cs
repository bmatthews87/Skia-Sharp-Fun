using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Plasma_Test
{
    public partial class Form1 : Form
    {
        int windowHeight = 300;
        int windowWidth = 500;
        int skWindowHeight = 300;
        int skWindowWidth = 300;
        SKBitmap screen;
        int frameCounter = 0;
        IntPtr pixelsPtr;

        public Form1()
        {
            InitializeComponent();
            SetupWindow();

            Timer timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 33;
            timer.Start();

            //store initial screen pixels
            screen = new SKBitmap(skWindowWidth, skWindowHeight);
            pixelsPtr = screen.GetPixels();
            hScrollBar1.Value = 44;
        }

        private void SetupWindow()
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Size = new Size(windowWidth, windowHeight);

            skglControl1.Location = new Point(0, 0);
            skglControl1.Size = new Size(skWindowWidth, skWindowHeight);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            frameCounter++;
            skglControl1.Invalidate();
        }

        private void skglControl1_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            e.Surface.Canvas.Clear(SKColor.FromHsl(0, 0, 0));

            DrawPlasma(sender, e);
        }

        private void DrawPlasma(object sender, SKPaintGLSurfaceEventArgs e)
        {
            //write directly to buffer
            unsafe
            {
                for (int x = 0; x < skWindowWidth; x++)
                {
                    for (int y = 0; y < skWindowHeight; y++)
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
                Math.Sin(x * (0.05 / 100) + frameCounter * 0.1) +
                Math.Sin(y * hScrollBar1.Value + frameCounter * 0.05) +
                Math.Sin((x + y) * 0.04 + frameCounter * hScrollBar1.Value);

            SKColor color = new SKColor((byte)(byte)(plasmaValue * (255 - 1)), (byte)(plasmaValue * (30 - 1)), (byte)(plasmaValue * (200 - 1)));

            return color;
        }
    }
}
