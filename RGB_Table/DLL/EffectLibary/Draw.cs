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

       private void Event_Paint_Handler(object sender, PropertyChangeArgs args)
       {
           try
           {


               string data = args.mesg;
               if (data == "Tetris")  // Alle Hauptbefehle hier unterbringen
               {
                   bluetooth.CommandControlChange -= new Bluetooth.PropertyChangeHandler(Event_Paint_Handler);
                   return;
               }

               //Überprüfung, ob das Argument eine Zahl ist - > eventuell sollte man dies durch eine Modus ABfrage MainWindow seitig ersetzen
               //string Str = args.mesg.Split(' ')[0];
               //double Num = 0;
               //bool isNum = double.TryParse(Str, out Num);  --> wirft noch einen Fehler?
               bool isNum = true;
               if (isNum)
               {
                   int draw_pos_x = Convert.ToInt16(data.Split(' ')[0]);
                   int draw_pos_y = Convert.ToInt16(data.Split(' ')[1]);
                   byte draw_col_r = Convert.ToByte(data.Split(' ')[2]);
                   byte draw_col_g = Convert.ToByte(data.Split(' ')[3]);
                   //byte draw_col_b = Convert.ToByte(data.Split(' ')[4]);
                   byte draw_col_b = 100;
                   Color draw_col = Color.FromRgb(draw_col_r, draw_col_g, draw_col_b);

                   setDrawtype = Drawtype.point;
                   mousepos.X = draw_pos_x;
                   mousepos.Y = draw_pos_y;


                   int a = 68;
                   int ba = 42;
                   origin.X = 0;
                   origin.Y = 0;

                //Zum testen
               //Random rnd = new Random();
               //byte r = Convert.ToByte(rnd.Next(0, 255));
               //byte g = Convert.ToByte(rnd.Next(0, 255));
               //byte b = Convert.ToByte(rnd.Next(0, 255));

               //Color c = Color.FromRgb(r, g, b);

                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => wh.SetPixel((int)draw_pos_x, (int)draw_pos_y, draw_col)));
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

        img.Source = wh;
        wh.Blit(new Rect(new Size(68, 42)), wbm, new Rect(new Size(68, 42)));

           
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
           //if (origin.X < mousepos.X && origin.Y < mousepos.Y ||dt == Drawtype.line)
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
