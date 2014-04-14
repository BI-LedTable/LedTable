using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace RGB_Libary
{
    
    public class Sin
    {
       
        #region Felder
       
        private int ys;
        private double index = 0, u;
        private WriteableBitmap writeableBmp;
    
        private Color Color;
       
        #endregion 
        #region Eigenschaften
        public Color Sinus_Color 
        {
            set { Color = value; }
            get { return Color; }
        }
        #endregion
     


        public Sin(WriteableBitmap writeableBmp)
        {
           
            this.writeableBmp = writeableBmp;
            Sin_init();

        }
        private int factor;
        private void Sin_init() 
        {
                Color = new Color();
                Color.A = 255;
                Color.R = 255;
                Color.G = 255;
                Color.B = 255;

        }
      
        public void Sin_execute()
        {
            index += 0.045;

            factor = (int)(writeableBmp.PixelWidth / Math.PI);

            using (writeableBmp.GetBitmapContext())
            {
                writeableBmp.Clear();
             
                for (int l = 0; l < 64; l++)
                {

                    u = (l - 34);
                   
                    //Color.A= (byte)(Color.A - Math.Pow(u, 2.0) * 0.220588);

                 
                    ys = (writeableBmp.PixelHeight / 2 - ((int)(Math.Sin((l * factor) + index) * (writeableBmp.PixelHeight) / 2)));
                   
                    writeableBmp.SetPixel(l, ys, Color);
                    
                   
                }
            }
           

        }
       
    }
        
    
}
