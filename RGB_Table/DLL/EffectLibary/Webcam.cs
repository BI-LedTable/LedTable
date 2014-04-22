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
        WriteableBitmap wbtarget;
        WriteableBitmap wbsource;
        System.Drawing.Bitmap bitmap;
        public Webcam(WriteableBitmap wb)
        {
      
            this.wbtarget = wb;
            runWebcam();
        }
        MemoryStream memory;
        public void Webcam_execute() 
        {
            BitmapImage bitmapImage;
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

                Color c = new Color();
              
                wbsource = BitmapFactory.ConvertToPbgra32Format(bitmapImage);
                int scalex = (int)(wbsource.DpiX * wbsource.Width / (wbtarget.Width * wbtarget.DpiX));
                int scaley = (int)(wbsource.DpiY * wbsource.Height / (wbtarget.Height * wbtarget.DpiY));
                for (int i = 0; i < 42; i++)
                {
                    for (int j = 0; j < 68; j++)
                    {
                        c = wbsource.GetPixel((int)(j * scalex), (int)(i * scaley));
                        wbtarget.SetPixel(j, i, c);
                        

                    }
                }
             
            }

            
           
               
        }
        public void resumeWebcam(WriteableBitmap wb) 
        {
            videoSource.Start();
            this.wbtarget = wb;
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
