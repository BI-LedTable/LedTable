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

namespace RgbLibrary
{
    public class ExpandingObject 
    {
        private EllipseGeometry ell;
        private int radius;
        private Color ColorEllipse;
        private int range;
        private int alphafallfactor;
        private Random r;

        public ExpandingObject(Random r)
        {
            this.r = r;
            generate();
        }
        public Color Color 
        {
            set { ColorEllipse = value; }
            get { return ColorEllipse; }
        }

        public int Range
        {
            set { range = value; }
            get { return range; }
        }

        public int Radius
        { 
            get { return radius; }
        }
        public int xPos
        {
            get { return x; }
        }
        public int yPos
        {
            get { return y; }
        }
        int x, y;
        private void generate() 
        {
           
            ell = new EllipseGeometry();
            radius = 0;
            ell.RadiusX = radius;
            ell.RadiusY = radius;
            ColorEllipse.G = 255;
            ColorEllipse.A = 255;
            ColorEllipse.B = 0;
            ColorEllipse.R = 0;
            randomRage();
            randomPos();
            
        }

        private void randomRage() 
        {
          
            range = r.Next(20, 30);
        }
        private void randomPos()
        {
            x = r.Next(0, 68);
            y = r.Next(0, 42);
        }
        double ease;
        double ease_x;

        public void expand() 
        {
            
            if (radius < range)
            {
          
               
              
                ease_x = (double)radius / (double)(range);
                ease = Math.Pow(ease_x,1) / (Math.Pow(ease_x,1) +Math.Pow((1 - ease_x),1));

               
                ColorEllipse.A = (byte)(255-255*ease);
                radius++;
                ell.RadiusX = radius;
                ell.RadiusY = radius;
            }
            else 
            {
              
                radius=0;
                randomPos();
                randomRage();
            }
          
            
        }
        
        
    }
    public class ExpandingObjects
    {
        private WriteableBitmap writeableBmp;
        private ExpandingObject[] Ellipsen;
        private int amount;
        Random r;
        private Color c;
        private String selected_obj;

        public String Object 
        {
            set {   
                    
                    selected_obj = value;
                    selected_obj = selected_obj.Substring(38);

                }
            get {
                    return selected_obj;
                }
        }

        public Color Color
        {
            set 
            {
                c = value;
                for (int i = 0; i < amount; i++)
                {
                    Ellipsen[i].Color = value;
                }
            
            }
            get 
            {
                    return Ellipsen[0].Color;
            }
        }
        private double dhelp;
        public Double Amount 
        {
            set 
            {
                dhelp = value;
                amount = (int)dhelp;
                generate_ellips();
            }
        }

        public ExpandingObjects(WriteableBitmap writeableBmp) 
        {
           
            this.writeableBmp = writeableBmp;
            init();
        }
        private void generate_ellips() 
        {
            Ellipsen = new ExpandingObject[amount];
            for (int i = 0; i < amount; i++)
            {
                Ellipsen[i] = new ExpandingObject(r);
                Ellipsen[i].Color = c;
            }
        }
        private void init() 
        {
            selected_obj = "ellipse";
            amount = 5;
          
            Ellipsen = new ExpandingObject[amount];
            r = new Random();
            for (int i = 0; i < amount; i++)
            {
                Ellipsen[i] = new ExpandingObject(r);
            }
            c = new Color();
           
        }     
        public void ExpandingObjects_execute()
        {

           writeableBmp.Clear();
            using (writeableBmp.GetBitmapContext())
            {
                for (int i = 0; i < amount; i++)
                {
                    Ellipsen[i].expand();

                    switch (selected_obj)
                    {
                        case"ellipse":
                            writeableBmp.DrawEllipse(Ellipsen[i].xPos - Ellipsen[i].Radius / 2, Ellipsen[i].yPos - Ellipsen[i].Radius / 2, Ellipsen[i].xPos + Ellipsen[i].Radius / 2, Ellipsen[i].yPos + Ellipsen[i].Radius / 2, Ellipsen[i].Color);
                            break;
                        case "filled ellipse":
                            writeableBmp.FillEllipse(Ellipsen[i].xPos - Ellipsen[i].Radius / 2, Ellipsen[i].yPos - Ellipsen[i].Radius / 2, Ellipsen[i].xPos + Ellipsen[i].Radius / 2, Ellipsen[i].yPos + Ellipsen[i].Radius / 2, Ellipsen[i].Color);
                            break;
                        case "rectangle":
                            writeableBmp.DrawRectangle(Ellipsen[i].xPos - Ellipsen[i].Radius / 2, Ellipsen[i].yPos - Ellipsen[i].Radius / 2, Ellipsen[i].xPos + Ellipsen[i].Radius / 2, Ellipsen[i].yPos + Ellipsen[i].Radius / 2, Ellipsen[i].Color);
                            break;
                        case "filled rectangle":
                            writeableBmp.FillRectangle(Ellipsen[i].xPos - Ellipsen[i].Radius / 2, Ellipsen[i].yPos - Ellipsen[i].Radius / 2, Ellipsen[i].xPos + Ellipsen[i].Radius / 2, Ellipsen[i].yPos + Ellipsen[i].Radius / 2, Ellipsen[i].Color);
                            break;
                    }
                 
                
                }
               

            }
           writeableBmp.Blit(new Rect(new Size(68, 42)), writeableBmp, new Rect(new Size(68, 42)),WriteableBitmapExtensions.BlendMode.Mask);
        }
    }
}
