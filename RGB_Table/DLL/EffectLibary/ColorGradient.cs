using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace RGB_Libary
{
    public enum Gradient
    {
        Horizontal,
        Vertical,
        Diagonal,
        Radial,
        Square
    };
   

    public enum Motion
    {
        Static,
        Sinus
    };
   public class ColorGradient
    {
 
        private Gradient gm;
        
        int[,] buffer;


        public Gradient GradientMode
        {
          get { return gm; } 
          set
          {

              gm = value;
              initColorGradient();
          } 
        }

        private Palettes palette;

        public Palettes ColorPalette
        {
            get { return palette; }
            set 
            { 
                palette = value;
                InitColorPallette();
            }
        }
        
        private WriteableBitmap wb;
        
        public ColorGradient(WriteableBitmap wb) 
        {
            this.wb = wb;
            colors = HSV.RedGreenBlue();
            counter = 0;
            InitBuffer();
           
        }
        Color[] colors;
       

        int counter;
        private void InitBuffer() 
        {
            buffer = new int[68, 42];
            for (int y = 0; y < wb.PixelHeight; y++)
            {
                for (int x = 0; x < wb.PixelWidth; x++)
                {
                    int color = (int)((x + y) * 360 / 110);
                    buffer[x, y] = color;
                }
            }
           
        }
        private void InitColorPallette() 
        {
            switch (palette)
            {
                case Palettes.RedGreenBlue:
                    colors = HSV.RedGreenBlue();
                    break;
                case Palettes.BlackWhite:
                    colors = HSV.BlackWhite();
                    break;
                case Palettes.RedGreen:
                    colors = HSV.RedGreen();
                    break;
                case Palettes.RedBlue:
                    colors = HSV.RedBlue();
                    break;
                case Palettes.BlueGreen:
                    colors = HSV.BlueGreen();
                    break;
                
            }   
        }

        private void initColorGradient()
        {
            int color;
            switch (gm)
            {
                case Gradient.Diagonal:
                    for (int y = 0; y < wb.PixelHeight; y++)
                    {
                        for (int x = 0; x < wb.PixelWidth; x++)
                        {

                            color = (int)((x + y) * 360 / 110);
                            buffer[x, y] = color;
                        }
                    }
                    break;
                case Gradient.Horizontal:
                    for (int y = 0; y < wb.PixelHeight; y++)
                    {
                        for (int x = 0; x < wb.PixelWidth; x++)
                        {

                            color = (int)(y * 360 / 42);
                            buffer[x, y] = color;
                        }
                    }
                    break;
                case Gradient.Square:
                    for (int y = 0; y < wb.PixelHeight; y++)
                    {
                        for (int x = 0; x < wb.PixelWidth; x++)
                        {

                            color =(int)((y^2+x)*4);
                            buffer[x, y] = color;
                        }
                    }
                    break;
                case Gradient.Vertical:
                    for (int y = 0; y < wb.PixelHeight; y++)
                    {
                        for (int x = 0; x < wb.PixelWidth; x++)
                        {

                            color = (int)(x * 360 / 68);
                            buffer[x, y] = color;
                        }
                    }
                    break;
                case Gradient.Radial:
                    for (int y = 0; y < wb.PixelHeight; y++)
                    {
                        for (int x = 0; x < wb.PixelWidth; x++)
                        {
                            color = (int)(180.0 + (180.0 * Math.Sin(Math.Sqrt((x - wb.PixelWidth / 2.0) * (x - wb.PixelWidth / 2.0) + (y - wb.PixelHeight / 2.0) * (y - wb.PixelHeight / 2.0)) / 8.0)));
                            buffer[x, y] = color;
                        }
                    }
                    break;

            }
        }
       
        public void ColorGradient_execute()
        {

            counter += 1;
            if(counter > HSV.colorcount)
                counter/=HSV.colorcount;
           
            for (int y = 0; y < wb.PixelHeight; y++)
            {
                for (int x = 0; x < wb.PixelWidth; x++)
                {
                    wb.SetPixel(x, y, colors[(buffer[x, y] + counter) % HSV.colorcount]);
                }
            }
        }

        

    }
}
