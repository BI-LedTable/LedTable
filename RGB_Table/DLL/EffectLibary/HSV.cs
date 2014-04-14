using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RGB_Libary
{
    public enum Palettes
    {
        RedGreenBlue,
        BlackWhite,
        RedGreen,
        RedBlue,
        BlueGreen

    };
    public class HSV
    {
        public static int colorcount;
       
        
        public static Color[] RedGreenBlue() 
        {
            colorcount = 360;
            Color[] colors = new Color[360];
            Color degreeColor = new Color();
            degreeColor.A = 255;
            int degree = 0;
            int section = 0;
            double factor = 255.0/60.0;
            while (degree < 360)
            {
                if (degree % 60 == 0)
                    section++;
                switch (section)
                {
                    case 1:
                        degreeColor.R = 255;
                        degreeColor.G = (byte)(degree*factor);
                        degreeColor.B = 0;
                        break;
                    case 2:
                        degreeColor.R = (byte)(255-degree % 60* factor);
                        degreeColor.G = 255;
                        degreeColor.B = 0;
                        break;
                    case 3:
                        degreeColor.R = 0;
                        degreeColor.G = 255;
                        degreeColor.B = (byte)(degree % 60 * factor);
                        break;
                    case 4:
                        degreeColor.R = 0;
                        degreeColor.G = (byte)(255-degree % 60* factor);
                        degreeColor.B = 255;
                        break;
                    case 5:
                        degreeColor.R = (byte)(degree % 60 * factor);
                        degreeColor.G = 0;
                        degreeColor.B = 255;
                        break;
                    case 6:
                         degreeColor.R = 255;
                        degreeColor.G = 0;
                        degreeColor.B = (byte)(255-degree % 60* factor);
                        break;
                }
                
                colors[degree]=degreeColor;
                degree++;
            }

            return colors;
        }
        public static Color[] RedBlue()
        {
            colorcount = 360;
            Color[] colors = new Color[360];
            Color degreeColor = new Color();
            degreeColor.A = 255;
            int degree = 0;
            int section = 0;
            double factor = 255.0 / 90;
            while (degree < 360)
            {
                if (degree % 90 == 0)
                    section++;
                switch (section)
                {
                    case 1:
                        degreeColor.R = 255;
                        degreeColor.G = 0;
                        degreeColor.B = (byte)(degree % 90 * factor);
                        break;
                    case 2:
                        degreeColor.R = (byte)(255-degree % 90 * factor);
                        degreeColor.G = 0;
                        degreeColor.B = 255;
                        break;
                    case 3:
                        degreeColor.R = (byte)(degree % 90 * factor);
                        degreeColor.G = 0;
                        degreeColor.B = 255;
                        break;
                    case 4:
                        degreeColor.R = 255;
                        degreeColor.G = 0;
                        degreeColor.B = (byte)(255 - degree % 90 * factor);
                        break;
                  
                   
                   
                }

                colors[degree] = degreeColor;
                degree++;
            }

            return colors;
        }
        public static Color[] RedGreen()
        {
            colorcount = 360;
            Color[] colors = new Color[360];
            Color degreeColor = new Color();
            degreeColor.A = 255;
            int degree = 0;
            int section = 0;
            double factor = 255.0 / 90;
            while (degree < 360)
            {
                if (degree % 90 == 0)
                    section++;
                switch (section)
                {
                    case 1:
                        degreeColor.R = 255;
                        degreeColor.B = 0;
                        degreeColor.G = (byte)(degree % 90 * factor);
                        break;
                    case 2:
                        degreeColor.R = (byte)(255-degree % 90 * factor);
                        degreeColor.B = 0;
                        degreeColor.G = 255;
                        break;
                    case 3:
                        degreeColor.R = (byte)(degree % 90 * factor);
                        degreeColor.B = 0;
                        degreeColor.G = 255;
                        break;
                    case 4:
                        degreeColor.R = 255;
                        degreeColor.B = 0;
                        degreeColor.G = (byte)(255 - degree % 90 * factor);
                        break;
                  
                   
                   
                }

                colors[degree] = degreeColor;
                degree++;
            }

            return colors;
        }
        public static Color[] BlueGreen()
        {
            colorcount = 360;
            Color[] colors = new Color[360];
            Color degreeColor = new Color();
            degreeColor.A = 255;
            int degree = 0;
            int section = 0;
            double factor = 255.0 / 90;
            while (degree < 360)
            {
                if (degree % 90 == 0)
                    section++;
                switch (section)
                {
                    case 1:
                        degreeColor.B = 255;
                        degreeColor.R = 0;
                        degreeColor.G = (byte)(degree % 90 * factor);
                        break;
                    case 2:
                        degreeColor.B = (byte)(255 - degree % 90 * factor);
                        degreeColor.R = 0;
                        degreeColor.G = 255;
                        break;
                    case 3:
                        degreeColor.B = (byte)(degree % 90 * factor);
                        degreeColor.R = 0;
                        degreeColor.G = 255;
                        break;
                    case 4:
                        degreeColor.B = 255;
                        degreeColor.R = 0;
                        degreeColor.G = (byte)(255 - degree % 90 * factor);
                        break;



                }

                colors[degree] = degreeColor;
                degree++;
            }

            return colors;
        }
        public static Color[] BlackWhite()
        {
            colorcount = 360;
            Color[] colors = new Color[360];
            Color degreeColor = new Color();
            degreeColor.A = 255;
            int degree = 0;
            int section = 0;
            double factor = 255.0 / 180;
            while (degree < 360)
            {
                if (degree % 180 == 0)
                    section++;
                switch (section)
                {
                    case 1:
                        degreeColor.B = (byte)(degree % 180 * factor);
                        degreeColor.R = (byte)(degree % 180 * factor);
                        degreeColor.G = (byte)(degree % 180 * factor);
                        break;
                    case 2:
                        degreeColor.B = (byte)(255 - degree % 180 * factor);
                        degreeColor.R = (byte)(255 - degree % 180 * factor);
                        degreeColor.G = (byte)(255 - degree % 180 * factor);
                        break;
                  



                }

                colors[degree] = degreeColor;
                degree++;
            }

            return colors;
           
        }

    }
   
}



