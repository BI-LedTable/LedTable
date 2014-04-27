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
namespace RgbLibrary
{
    public class MovingText
    {
    
        private WriteableBitmap monitor;
        private TextBox textBox;

        private Palettes palette;
        private RenderTargetBitmap renderTargetBitmap;
        private Point Pos;
        private String mode;
        FormattedText text;
        DrawingVisual drawingVisual;
        DrawingContext drawingContext;
        SolidColorBrush c;
        Color[] colors;
        public Palettes ColorPalette 
        {
            set { 
                    palette = value;
                    InitColorPallette();
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

        public bool ColorScroll { get; set; }
        public String Text 
        {
            set { textBox.Text = value; }
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
            set { textBox.FontSize = value; }
            
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
           
            this.monitor = wb;
            init_MovingText();
        }
        private void init_MovingText() 
        {

           
            colors = HSV.RedGreenBlue();
            textBox = new TextBox();
            textBox.FontSize = 20;
            textBox.Text = ("Text");
            textBox.FontWeight = FontWeights.UltraLight;
            textBox.Foreground = Brushes.White;
         
            renderTargetBitmap = new RenderTargetBitmap(monitor.PixelWidth, monitor.PixelHeight, monitor.DpiX, monitor.DpiY, PixelFormats.Default);
            Pos = new Point(0,0);
            drawingVisual = new DrawingVisual();

           
        }

        int counter;
        public void MovingText_execute() 
        {
            
            switch (mode)
            {
                case "ScrollX":
                    counter++;
                   textBox.Foreground = new SolidColorBrush(colors[counter % 360]);
                    if (Pos.X < 68)
                        Pos.X++;
                    else Pos.X = -text.Width;
                    break;
                case "ScrollY":
                    counter++;
                   textBox.Foreground = new SolidColorBrush(colors[counter % 360]);
                    if (Pos.Y < 42)
                        Pos.Y++;
                    else Pos.Y= -text.Height;
                    break;
            }

            renderTargetBitmap.Clear();
            using( monitor.GetBitmapContext())
            {

                //string time = DateTime.Now.Hour + " : " + DateTime.Now.Minute + " : " + DateTime.Now.Second;
                
                text = new FormattedText(textBox.Text,
                new CultureInfo("de-de"),
                FlowDirection.LeftToRight,
                new Typeface(textBox.FontFamily, FontStyles.Normal, textBox.FontWeight, new FontStretch()),
                textBox.FontSize,
                textBox.Foreground);

                text.LineHeight = textBox.FontSize;  
                drawingContext = drawingVisual.RenderOpen();
                drawingContext.DrawText(text, Pos);
                drawingContext.Close();
      
                renderTargetBitmap.Render(drawingVisual);
                renderTargetBitmap.CopyPixels(new Int32Rect(0, 0, renderTargetBitmap.PixelWidth, renderTargetBitmap.PixelHeight),
                                            monitor.BackBuffer, monitor.BackBufferStride * monitor.PixelHeight, monitor.BackBufferStride);
               
            }

          
            
        }
    }
}
