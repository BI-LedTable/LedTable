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

namespace RGB_Libary
{

    public class ImagePixelate
    {

        private WriteableBitmap wbsource;
        private WriteableBitmap wbtarget;

        public String Path { get; set; }
        public void path()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Bilddateien (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
            dlg.ShowDialog();

            if (dlg.FileName != "")
            {

                var uri = new System.Uri(dlg.FileName);
                var converted = uri.AbsoluteUri;
                Path = converted;

            }
            execute();
        }
        private Color[] c;
        public ImagePixelate(WriteableBitmap wbtarget)
        {

            this.wbtarget = wbtarget;
            execute();

        }
        public void execute()
        {
            if (Path != null)
                wbsource = LoadBitmap(Path);
            c = new Color[42 * 68];
            int count = 0;


            try
            {
                if (wbsource != null)
                {
                    //// http://wiki.delphigl.com/index.php/Convolution-Filter
                    //// http://writeablebitmapex.codeplex.com/SourceControl/latest#branches/WBX_1.0_WinMD/Source/WriteableBitmapEx/WriteableBitmapFilterExtensions.cs

                    //int[,] KernelGaussianBlur5x5 = {
                    //                            {1,  4,  7,  4, 1},
                    //                            {4, 16, 26, 16, 4},
                    //                            {7, 26, 41, 26, 7},
                    //                            {4, 16, 26, 16, 4},
                    //                            {1,  4,  7,  4, 1}
                    //                        };

                    //int[,] KernelGaussianBlur3x3 = {
                    //                                   {16, 26, 16},
                    //                                   {26, 41, 26},
                    //                                   {16, 26, 16}
                    //                                };


                    //int[,] KernelSharp = {
                    //                {-1, -2, -1},
                    //                {0, 1, 0},
                    //                {1, 2, 1}
                    //            };
                  


                    //int scalex = (int)( wbsource.DpiX * wbsource.Width / (wbtarget.Width * wbtarget.DpiX));
                    //int scaley = (int)( wbsource.DpiY * wbsource.Height / (wbtarget.Height * wbtarget.DpiY ));
                    int scalex = (int)(wbsource.PixelWidth / (wbtarget.PixelWidth));
                    int scaley = (int)(wbsource.PixelHeight / (wbtarget.PixelHeight));

                    if (scalex <= 0)// für bilder die kleiner als 68x42 sind (werden dann öfter angezeigt)
                    {
                        scalex = 1;
                    }

                    if (scaley <= 0)
                    {
                        scaley = 1;
                    }

                    for (int i = 0; i < 42; i++)
                    {
                        for (int j = 0; j < 68; j++)
                        {
                            c[count] = wbsource.GetPixel((int)(j * scalex), (int)(i * scaley));
                            wbtarget.SetPixel(j, i, c[count]);
                            count++;
                        }
                    }

                }
            }
            catch (AccessViolationException e)
            {
                MessageBox.Show(e.Message);
            }


        }
        WriteableBitmap LoadBitmap(string path)
        {

            if (path != "")
            {
                var imgn = new BitmapImage();
                var uri = new System.Uri(path);
                var converted = uri.AbsoluteUri;
                imgn = new BitmapImage(new Uri(converted));
                return BitmapFactory.ConvertToPbgra32Format(imgn);
            }
            else return null;



        }
        public void ImagePixelate_execute() { }
    }
}
