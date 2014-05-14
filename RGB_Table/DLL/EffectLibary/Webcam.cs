using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Threading;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Controls;


namespace RgbLibrary
{
    public class Webcam
    {
       
       
        VideoCaptureDevice videoSource;
        WriteableBitmap monitor;
        WriteableBitmap origin;
        System.Drawing.Bitmap bitmap;
        BitmapImage bitmapImage;
         MemoryStream memory;
        public Webcam(WriteableBitmap wb)
        {

           
            this.monitor = wb;
            runWebcam();
        }
       
        public void Webcam_execute() 
        {
           
           if (bitmap != null)
            {
                using (memory = new MemoryStream())
                {

                    bitmap.Save(memory, ImageFormat.Bmp);
                    memory.Position = 0;
                    bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();

                }

            }

           if (bitmapImage != null)
           {
               origin = BitmapFactory.ConvertToPbgra32Format(bitmapImage);
               origin = origin.Resize(68, 42, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
               origin.CopyPixels(new Int32Rect(0, 0, origin.PixelWidth, origin.PixelHeight), monitor.BackBuffer,
                                  monitor.BackBufferStride * monitor.PixelHeight, monitor.BackBufferStride);
           }

            
           
               
        }
        public void resumeWebcam(WriteableBitmap wb) 
        {
            videoSource.Start();
            this.monitor = wb;
        }
        public void runWebcam() 
        {
            FilterInfoCollection videosources = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            //Überprüfen, ob mindestens eine Aufnahmequelle vorhanden ist
            if (videosources != null)
            {
                //Die erste Aufnahmequelle an unser Webcam Objekt binden
                //(habt ihr mehrere Quellen, muss nicht immer die erste Quelle die
                //gewünschte Webcam sein!)
                videoSource = new VideoCaptureDevice(videosources[0].MonikerString);
                if (!videoSource.IsRunning)
                {
                   
                    videoSource.NewFrame += new AForge.Video.NewFrameEventHandler(videoSource_NewFrame);
                    //Das Aufnahmegerät aktivieren

                    videoSource.Start();
                }
            }
        }
       
        private void videoSource_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {

            bitmap = (System.Drawing.Bitmap)eventArgs.Frame.Clone();

        }
        public void closeWebcam() 
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.Stop();
              
            }
        }
    }
}
