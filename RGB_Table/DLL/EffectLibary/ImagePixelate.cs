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

    public class ImagePixelate
    {

        private WriteableBitmap origin;
        private WriteableBitmap monitor;

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

            this.monitor = wbtarget;
            execute();

        }
        public void execute()
        {
            if (Path != null)
                origin = LoadBitmap(Path);
            c = new Color[42 * 68];
            int count = 0;

            try
            {
                if (origin != null)
                {

                    origin = origin.Resize(68, 42, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
                    origin.CopyPixels(new Int32Rect(0,0,origin.PixelWidth,origin.PixelHeight), monitor.BackBuffer, monitor.BackBufferStride * monitor.PixelHeight, monitor.BackBufferStride);
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
