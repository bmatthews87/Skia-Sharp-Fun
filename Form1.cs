using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Plasma_Test
{
    public partial class Form1 : Form
    {
        int lineCount = 50;
        Random rand = new Random();
        int hValue = 0;
        int sValue = 20;
        bool isDesc = false;
        int windowHeight = 500;
        int windowWidth = 500;
        int currentMouseX;
        int currentMouseY;

        public Form1()
        {
            InitializeComponent();

            SetupWindow();

            Timer timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 1;
            timer.Start();
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
            e.Surface.Canvas.Clear(SKColor.FromHsl(hValue, sValue, 0));


            DrawPlasma(sender, e);
            //DisplayMouseCoordinates(sender, e);
            //DrawTriangle(sender, e);
            //SwapBackground(sender, e);
            //RandomLines(sender, e);
        }

        private void DrawPlasma(object sender, SKPaintGLSurfaceEventArgs e)
        {
            for (int i = 0; i < this.Width; i++)
            {
                for (int j = 0; j < this.Height; j++)
                {
                    e.Surface.Canvas.DrawPoint(new SKPoint(i, j), GetPlasmaColor(i, j));
                }
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

        private void DisplayMouseCoordinates(object sender, SKPaintGLSurfaceEventArgs e)
        {
            SKPaint paint = new SKPaint();
            paint.Color = new SKColor((byte)0, (byte)255, (byte)0);

            string MouseCoordText = @"X: " + currentMouseX.ToString() + " | Y: " + currentMouseY.ToString();

            e.Surface.Canvas.DrawText(MouseCoordText, new SKPoint(0, windowHeight - 50), paint);
        }

        private void DrawTriangle(object sender, SKPaintGLSurfaceEventArgs e)
        {
            var paint = new SKPaint
            {
                Color = new SKColor((byte)0, (byte)255, (byte)0)
            };

            //draw triangle here
            e.Surface.Canvas.DrawVertices(
                SKVertexMode.Triangles,
                new SKPoint[]
                {
                    new SKPoint(100,10),
                    new SKPoint(190,190),
                    new SKPoint(10,190)
                },
                new SKColor[]
                {
                    new SKColor((byte)0, (byte)255, (byte)0),
                    new SKColor((byte)0, (byte)255, (byte)0),
                    new SKColor((byte)0, (byte)255, (byte)0)
                },
                paint
                );
        }

        public void SwapBackground(object sender, SKPaintGLSurfaceEventArgs e)
        {
            if (isDesc)
            {
                sValue--;
                if (sValue == 0)
                {
                    //before we start going up, pick a new color
                    hValue = rand.Next(0, 360);
                    isDesc = false;
                }
            }
            else
            {
                sValue++;
                if (sValue == 80)
                {
                    isDesc = true;
                }
            }
        }

        public void RandomLines(object sender, SKPaintGLSurfaceEventArgs e)
        {
            for (int i = 0; i < lineCount; i++)
            {
                var paint = new SKPaint
                {
                    Color = new SKColor(
                        red: (byte)rand.Next(255),
                        green: (byte)rand.Next(255),
                        blue: (byte)rand.Next(255),
                        alpha: (byte)rand.Next(255)),
                    StrokeWidth = rand.Next(1, 10),
                    IsAntialias = true
                };
                e.Surface.Canvas.DrawLine(
                    x0: rand.Next(skglControl1.Width),
                    y0: rand.Next(skglControl1.Height),
                    x1: rand.Next(skglControl1.Width),
                    y1: rand.Next(skglControl1.Height),
                    paint: paint);
            }
        }

        private void skglControl1_MouseMove(object sender, MouseEventArgs e)
        {
            currentMouseX = e.X;
            currentMouseY = e.Y;
        }
    }
}
