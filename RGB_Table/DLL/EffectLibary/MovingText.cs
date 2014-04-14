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
using System.Globalization;
namespace RGB_Libary
{
    public class MovingText
    {
    
        private WriteableBitmap wb;
        private TextBox tb;
       
      
        private RenderTargetBitmap rtb;
        private Point Pos;
        private String mode;
        FormattedText text;
        DrawingVisual dv;
        DrawingContext drawingContext;
        SolidColorBrush c;
        Color[] color;
        public Color TextColor 
        {
            set { tb.Foreground = new SolidColorBrush(value); }
            
        }

        public bool ColorScroll { get; set; }
        public String Text 
        {
            set { tb.Text = value; }
        }
        public String Mode
        {
            set { 
                    mode = value;
                    mode = mode.Substring(38);
                }
        }
        public Double FontSize
        {
            set { tb.FontSize = value; }
            
        }
        public double PosY 
        {
            set { Pos.Y = value; }
        }
        public double PosX
        {
            set { Pos.X = value; }
        }

        public MovingText(WriteableBitmap wb) 
        {
           
            this.wb = wb;
            init_MovingText();
        }
        private void init_MovingText() 
        {

           
            color = HSV.RedGreenBlue();
            tb = new TextBox();
            tb.FontSize = 20;
            tb.Text = ("Text");
            tb.FontWeight = FontWeights.UltraLight;
            tb.Foreground = Brushes.Purple;
         
            rtb = new RenderTargetBitmap(wb.PixelWidth, wb.PixelHeight, wb.DpiX, wb.DpiY, PixelFormats.Default);
            Pos = new Point(0,0);
            dv = new DrawingVisual();

           
        }

        int counter;
        public void MovingText_execute() 
        {
            
            switch (mode)
            {
                case "ScrollX":
                    counter++;
                    tb.Foreground = new SolidColorBrush(color[counter % 360]);
                    if (Pos.X < 68)
                        Pos.X++;
                    else Pos.X = -text.Width;
                    break;
                case "ScrollY":
                    counter++;
                    tb.Foreground = new SolidColorBrush(color[counter % 360]);
                    if (Pos.Y < 42)
                        Pos.Y++;
                    else Pos.Y= -text.Height;
                    break;
            }

            rtb.Clear();
            using( wb.GetBitmapContext())
            {

                //string time = DateTime.Now.Hour + " : " + DateTime.Now.Minute + " : " + DateTime.Now.Second;
                
                text = new FormattedText(tb.Text,
                new CultureInfo("de-de"),
                FlowDirection.LeftToRight,
                new Typeface(tb.FontFamily, FontStyles.Normal, tb.FontWeight, new FontStretch()),
                tb.FontSize,
                tb.Foreground);
                text.LineHeight = tb.FontSize;  
                drawingContext = dv.RenderOpen();
                drawingContext.DrawText(text, Pos);
                drawingContext.Close();
      
                rtb.Render(dv);
                rtb.CopyPixels(new Int32Rect(0, 0, rtb.PixelWidth, rtb.PixelHeight), wb.BackBuffer, wb.BackBufferStride * wb.PixelHeight, wb.BackBufferStride);
               
            }

          
            
        }
    }
}
