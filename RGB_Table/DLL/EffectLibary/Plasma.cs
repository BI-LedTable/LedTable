using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xceed.Wpf.Toolkit;

namespace RgbLibrary
{
    //2D Matrix
    //68x42
    //2 Loops

   public class Plasma
    {
        
        WriteableBitmap monitor;
        Color[] colors;
        int[,] plasma;

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
        
        public Plasma(WriteableBitmap wbm) 
        {
            plasma = new int[wbm.PixelWidth, wbm.PixelHeight];
            colors = HSV.RedGreenBlue();
            timer = 1;
            this.monitor = wbm;

            for (int y = 0; y < wbm.PixelHeight; y++)
            {
                for (int x = 0; x < wbm.PixelWidth; x++)
                {
                    int c = (int)(
                        HSV.colorcount +(HSV.colorcount * Math.Sin(x/8.0))
                        +HSV.colorcount+(HSV.colorcount * Math.Sin(y/8.0))
                        + HSV.colorcount + (HSV.colorcount * Math.Sin((x + y) / 8.0))
                        + HSV.colorcount + (HSV.colorcount * Math.Sin(Math.Sqrt((double)(x * x + y * y)) / 8.0))
                        )/4;
                    plasma[x, y] = c;
                }
            }
            
        }
       int timer;
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
        public void Plasma_execute()
        {
            timer+=1;
            if(timer % HSV.colorcount == 0)
                timer/=HSV.colorcount;

            for (int y = 0; y < monitor.PixelHeight; y++)
            {
                for (int x = 0; x < monitor.PixelWidth; x++)
                {
                    monitor.SetPixel(x, y, colors[(plasma[x, y] + timer)%(HSV.colorcount)]);
                }
            }
          
        }      
    }
}
