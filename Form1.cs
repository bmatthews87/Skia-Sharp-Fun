using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Plasma_Test
{
    public partial class Form1 : Form
    {
        int lineCount = 50;
        Random rand = new Random();
        int hValue = 0;
        int sValue = 20;
        bool isDesc = false;
        int screenHeight = 0;
        int screenWidth = 0;
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
            this.WindowState = FormWindowState.Maximized;

            screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
            screenWidth = Screen.PrimaryScreen.WorkingArea.Width;

            skglControl1.Location = new Point(0, 0);
            skglControl1.Size = new Size(screenWidth, screenHeight);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            skglControl1.Invalidate();
        }

        private void skglControl1_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            e.Surface.Canvas.Clear(SKColor.FromHsl(hValue, sValue, 0));

            //SwapBackground(sender, e);
            DrawTriangle(sender, e);
            //RandomLines(sender, e);
            DisplayMouseCoordinates(sender, e);
        }

        private void DisplayMouseCoordinates(object sender, SKPaintGLSurfaceEventArgs e)
        {
            SKPaint paint = new SKPaint();
            paint.Color = new SKColor((byte)0, (byte)255, (byte)0);

            string MouseCoordText = @"X: " + currentMouseX.ToString() + " | Y: " + currentMouseY.ToString();

            e.Surface.Canvas.DrawText(MouseCoordText, new SKPoint(0, screenHeight - 30), paint);
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
