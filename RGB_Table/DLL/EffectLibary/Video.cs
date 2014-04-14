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
   
   public class Video
    {
       RenderTargetBitmap rtb;
       WriteableBitmap wb;
       MediaPlayer mp;
       
       DrawingVisual drawingVisual;
     
       public Video(WriteableBitmap wb) 
       {
           this.wb = wb;
         
           drawingVisual = new DrawingVisual();
           mp = new MediaPlayer();
           rtb = new RenderTargetBitmap(wb.PixelWidth, wb.PixelHeight, wb.DpiX, wb.DpiY, PixelFormats.Default);
         
           run = true;
           mp.MediaEnded += mp_MediaEnded;
         
       }

       void mp_MediaEnded(object sender, EventArgs e)
       {
           //play();
           mp.Play();
       }

       public String Path { get; set; }

       bool run;
       public bool RunVideo
       {
           
           set 
           {
               if (value == false)
               {
                   run = false;
                   mp.Close();
               }
               else {
                   run = true;
                   
               }
           }
       }

       public void play() 
       {
           if (Path != "")
           {
               var uri = new System.Uri(Path);
               var converted = uri.AbsoluteUri;
               Path = converted;
               mp.Open(uri);
               mp.Play();
           }
       }
       
       public void path()
       {
          
               Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
               dlg.Filter = "Bilddateien (*.avi, *.mp4, *.wmv)|*.avi;*.mp4;*.wmv;";
               dlg.ShowDialog();
               if (dlg.FileName != "")
               {

                   var uri = new System.Uri(dlg.FileName);
                   var converted = uri.AbsoluteUri;
                   Path = converted;
                   mp.Open(uri);
                   mp.Play();

               }
           
           
           
       }
       public void StartVideoWithPath(Uri uri)
       {
           mp.Open(uri);
           mp.Play();
          
       }
      
       public void video_execute() 
       {

           if (run)
           {

               using (DrawingContext drawingContext = drawingVisual.RenderOpen())
               {

                  
                   drawingContext.DrawVideo(mp, new Rect(new Size(68, 42)));
               }

              
               rtb.Render(drawingVisual);
               rtb.CopyPixels(new Int32Rect(0, 0, rtb.PixelWidth, rtb.PixelHeight), wb.BackBuffer, wb.BackBufferStride * wb.PixelHeight, wb.BackBufferStride);
           }
           
       }
      
       
    }
}
