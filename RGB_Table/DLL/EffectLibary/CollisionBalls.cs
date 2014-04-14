using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Shapes;
using System.Threading;

namespace RGB_Libary
{
    public class Ball 
    {
     
        
        private Point position;
        public Point Position
        {
            get { return position; }
            set { position = value; }
        }

        private Color color;
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        private Ellipse ellipse;
        public Ellipse Ellipse
        {
            get { return ellipse; }
            set { ellipse = value; }
        }
        

        public Boolean xDirection;
        public Boolean yDirection;
        public Random random;
        private Vector v;

        public Ball(Random random) 
        {
            this.random = random;
            ellipse = new Ellipse();
            ellipse.Width = random.Next(5,15);
            v = new Vector();
            RandomVector();
            color = new Color();
            RandomColor();
            position = new Point();
            xDirection = RandomDirection();
            yDirection = RandomDirection();
            RandomPosition();

           
        }
        private void collision() 
        {
            //radius = Ellipse.Width
            int r = (int)ellipse.Width / 2;
            for (int i = 0; i < ellipse.Width; i++)
            {
                
            }
        }
        private void RandomVector() 
        {
            v.X = random.Next(-15, 15);
            v.Y = random.Next(-15, 15);
         
        }
        public bool RandomDirection() 
        {
            return random.Next(0, 2) == 0;
        }
        public void RandomPosition()
        {
            position.X = random.Next(0, 68);
            position.Y = random.Next(0, 42);
        }
        public void RandomColor() 
        {
            color = Color.FromArgb(255, (byte)random.Next(25, 255), (byte)random.Next(25, 255), (byte)random.Next(25, 255));
        }
        public void Move() 
        {
            if (position.X < 68 && xDirection)
                xDirection = true;
            else xDirection = false;

            if (position.X > 0 && !xDirection)
                xDirection = false;
            else xDirection = true;

            if (position.Y < 42 && yDirection)
                yDirection = true;
            else yDirection = false;

            if (position.Y > 0 && !yDirection)
                yDirection = false;
            else yDirection = true;
            
            switch (xDirection)
            {
                case false:
                    v.X = - Math.Abs(v.X);
                    break;
                case true:
                    v.X = Math.Abs(v.X);
                    break;
            }
            switch (yDirection)
            {
                case false:
                    v.Y = -Math.Abs(v.Y);
                    break;
                case true:
                    v.Y = Math.Abs(v.Y);
                    break;
            }
            v.Normalize();
            position.X +=v.X*1.5;
            position.Y += v.Y*1.5;
        }
        
    }
    
    public class CollisonBalls
    {
        RadialGradientBrush rgb;
        WriteableBitmap wb;
      
      
        RenderTargetBitmap rtb;
        DrawingVisual drawingVisual;
        Random random;
        int amount = 10;
        Ball[] collisionBalls;
        public CollisonBalls(WriteableBitmap wb)
        {
            this.wb = wb;

            drawingVisual = new DrawingVisual();
            rtb = new RenderTargetBitmap(wb.PixelWidth, wb.PixelHeight, wb.DpiX, wb.DpiY, PixelFormats.Default);
            random = new Random();

            collisionBalls = new Ball[amount];
            for (int i = 0; i < amount; i++)
            {
                collisionBalls[i] = new Ball(random);
                
            }

        }

        public void CollisonBalls_execute()
        {
           

             


                rtb.Clear();
                wb.Clear();
                using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                {
                   
                    for (int i = 0; i < amount; i++)
                    {

                        collisionBalls[i].Move();
                        rgb = new RadialGradientBrush();
                        rgb.GradientStops = new GradientStopCollection {new GradientStop(collisionBalls[i].Color,0),
                            new GradientStop(Colors.Black,1)};
                        drawingContext.DrawEllipse(rgb, null, collisionBalls[i].Position, collisionBalls[i].Ellipse.Width, collisionBalls[i].Ellipse.Width);
                    }
                    
                  
                }

                rtb.Render(drawingVisual);
                rtb.CopyPixels(new Int32Rect(0, 0, rtb.PixelWidth, rtb.PixelHeight), wb.BackBuffer, wb.BackBufferStride * wb.PixelHeight, wb.BackBufferStride);
                
           
        }
    }
}
