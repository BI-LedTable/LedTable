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



using System.Diagnostics;

namespace RgbLibrary
{
    public enum Drawtype 
    {
        line,
        circle,
        rectangle,
        point,
        fillrectangle,
        fillcircle,
    };
   public class Draw
    {

       WriteableBitmap wh;
       WriteableBitmap wbm;
       Image img;
       Point mousepos;
       Point origin;
       Drawtype dt;
       Color c;
       Bluetooth bluetooth = Bluetooth.Instance;

       String data_Stream;

       public Draw(WriteableBitmap wh, Image img, WriteableBitmap wbm) 
       {
           this.wbm = wbm;
           this.img = img;
           this.wh = wh;
           mousepos = new Point();
           origin = new Point();
           c = new Color();

          

           bluetooth.CommandControlChange += new Bluetooth.PropertyChangeHandler(Event_Paint_Handler);
       }

       int Draw_Mode = 0; //Zur entscheidung zwischen Paint & Fotobearbeitungsmodus

       private void Event_Paint_Handler(object sender, PropertyChangeArgs args)
       {
           try
           {

               string data = args.mesg;


               if (data == "Tetris")  // Alle Hauptbefehle hier unterbringen
               {
                   data_Stream = "";
                   bluetooth.CommandControlChange -= new Bluetooth.PropertyChangeHandler(Event_Paint_Handler);
                   return;
               }

               if (data == "Paint")
               {
                   Draw_Mode = 1;
                   data_Stream = "";
               }
               if (data == "Fotobearbeitung")
               {
                   Draw_Mode = 2;
                   data_Stream = "";
                   data = "";
               }

               if (data == "Fertig")  //Das Übertragen der Fotodaten wurde abgeschlossen
               {
                   //neue Bitmap anlegen!!
                   byte[] buffer = new byte[50000];

                   ////daten aus dem Stream in ein Byte Array
                   //string[] data_buffer = new string[68*42*3];
                   //char[] separator = {' '};
                   
                   //data_buffer = data_Stream.Split(separator,StringSplitOptions.RemoveEmptyEntries);
                   
                   //Neuer Ansatz:
                   //Zuerst wird "Fotobearbeitung" als String gesendet und in der Bluetooth Klasse dementsprechend weitergeleitet, da dies dort konvertiert wird.
                   //Dann sollen Android seitig die Bilddaten als BYTES gesendet werden, ohne konvertierung.
                   //Diese Daten werden dann in Bluetooth.Read konvertiert in Strings, was jedoch nur Kauderwelsch ist.
                   //Diese Daten werden an den aktuellen modus (Fotobearbeitung) weitergeleitet, wo sie als String in einen großen String gespeichert werden
                   //Am Ende des sendens wird wieder ein String ("Fertig") gesendet
                   //Mit diesem String beginnt die Konvertierung der String Daten zurück in byte Form.
                   //Diese Bytes werden in ein großes Array geschrieben und an das Bitmap übergeben.


                   buffer = Encoding.ASCII.GetBytes(data_Stream);

                   try
                   {
                   WriteableBitmap bmp_foto = BitmapFactory.New(0, 0).FromByteArray(buffer);
                   
                  

                   Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => wh = bmp_foto));
                   }
                   catch(Exception exc)
                   {
                   }
               }
               
               //Paint
               if(Draw_Mode == 1)
               {

                   //Überprüfung, ob das Argument eine Zahl ist - > eventuell sollte man dies durch eine Modus ABfrage MainWindow seitig ersetzen
                   //string Str = args.mesg.Split(' ')[0];
                   //double Num = 0;
                   //bool isNum = double.TryParse(Str, out Num);  --> wirft noch einen Fehler?
                   bool isNum = true;
                   if (isNum)
                   {
                       int draw_pos_x = Convert.ToInt16(data.Split(' ')[0]);
                       int draw_pos_y = Convert.ToInt16(data.Split(' ')[1]);
                       int draw_col_r = Convert.ToInt32(data.Split(' ')[2]);
                       int draw_col_g = Convert.ToInt32(data.Split(' ')[3]);
                       int draw_col_b = Convert.ToInt32(data.Split(' ')[4]);
                       //byte draw_col_b = 100;

                       setDrawtype = Drawtype.point;
                       if (draw_pos_x < 68)
                       {
                           mousepos.X = draw_pos_x;
                       }
                       if (draw_pos_y < 42)
                       {
                           mousepos.Y = draw_pos_y;
                       }

                       

                       if ((draw_col_r < 256) && (draw_col_g < 256) && (draw_col_b < 256))
                       {
                           try
                           {
                               byte r = Convert.ToByte(draw_col_r);
                               byte g = Convert.ToByte(draw_col_g);
                               byte b = Convert.ToByte(draw_col_b);
                               Color draw_col = Color.FromRgb(r, g, b);


                               Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => wh.SetPixel((int)mousepos.X, (int)mousepos.Y, draw_col)));
                           }
                           catch (Exception ex)
                           {
                               string excep = ex.ToString();
                           }
                       }
                   }
                   
               }


               //Fotobearbeitung
               if (Draw_Mode == 2)
               {
                   //Versuchsewise unperfomante Variante - eventuell zur performanteren Variante mit Stringbuilder wechseln!!
                   //Verzögerungen können jedoch nur schwer festgestellt werden.
                   data_Stream = data_Stream + data;
               }

           }
           catch (Exception exc)
           {

           }

       }

       public Point setMousePos 
       {
           set { mousepos = value; }
       }
       public Point setOrigin
       {
           set { origin = value; }
       }
       public Color setColor
       {
           set 
           { 
               c = value;
           }
           get 
           {
               return c;
           }
       }
       public void Draw_execute()
       {
       }
       public Drawtype setDrawtype
       {
           set { dt = value; }
       }
       public void draw() 
       {
          
               switch (dt) 
               {
                   case Drawtype.point:
                       this.wh.SetPixel((int)mousepos.X, (int)mousepos.Y, c);
                       break;
                   case Drawtype.line:
                       wh.DrawLine((int)origin.X,(int)origin.Y,(int)mousepos.X,(int)mousepos.Y,c);
                       break;
                   case Drawtype.circle:
                       wh.DrawEllipse((int)origin.X, (int)origin.Y, (int)mousepos.X, (int)mousepos.Y, c);
                       break;
                   case Drawtype.rectangle:
                       wh.DrawRectangle((int)origin.X, (int)origin.Y, (int)mousepos.X, (int)mousepos.Y, c);
                       break;
                   case Drawtype.fillrectangle:
                       
                       wh.FillRectangle((int)origin.X, (int)origin.Y, (int)mousepos.X, (int)mousepos.Y, c);
                       break;
                   case Drawtype.fillcircle:
                       wh.FillEllipse((int)origin.X, (int)origin.Y, (int)mousepos.X, (int)mousepos.Y, c);
                       break;
                   
               
           }
       }
       public void del()
       {
         
               switch (dt) 
               {
              
                   case Drawtype.line:
                       wh.DrawLine((int)origin.X,(int)origin.Y,(int)mousepos.X,(int)mousepos.Y,Colors.Transparent);
                       break;
                   case Drawtype.circle:
                       wh.DrawEllipse((int)origin.X, (int)origin.Y, (int)mousepos.X, (int)mousepos.Y, Colors.Transparent);
                       break;
                   case Drawtype.rectangle:
                       wh.DrawRectangle((int)origin.X, (int)origin.Y, (int)mousepos.X, (int)mousepos.Y, Colors.Transparent);
                       break;
                   case Drawtype.fillrectangle:
                       wh.FillRectangle((int)origin.X, (int)origin.Y, (int)mousepos.X, (int)mousepos.Y, Colors.Transparent);
                       break;
                   case Drawtype.fillcircle:
                       wh.FillEllipse((int)origin.X, (int)origin.Y, (int)mousepos.X, (int)mousepos.Y, Colors.Transparent);
                       break;
               }
       }
    }
}
