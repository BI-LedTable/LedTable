using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using System.ComponentModel;
using RgbLibrary;
using System.Windows.Interop;


namespace RgbLibrary
{
    
    

    public class Tetris 
    {
        WriteableBitmap background;
        WriteableBitmap wb;
        WriteableBitmap wbm;
  
        private int Rows;
        private int Cols;
        int _level;

        private int LinesFilled;
        private Tetramino currTetramino;
        BitmapImage img;

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {

                handler(this, new PropertyChangedEventArgs(name));
                
            }
        }




        public Tetris(WriteableBitmap wbm)
        {

           
            this.wbm = wbm;
            this.wb = BitmapFactory.New(wbm.PixelWidth / 2, wbm.PixelHeight / 2);
            
         
          
            for (int i = 0; i < wb.PixelHeight; i++)
            {
                for (int j = 0; j < wb.PixelWidth; j++)
                {
                    
                    wb.SetPixel(j, i,Colors.Black);   
                }
            }
            Rows = this.wb.PixelHeight;
            Cols = this.wb.PixelWidth;
            _score = 0;
            _level = 1;
          
            Level = _level;
            LinesFilled = 0;
            currTetramino = new Tetramino();
            currTetraminoDraw();
            

        }

       

        public int Level
        {
            get { return level; }
            set
            {
                level = value;
                OnPropertyChanged("Level");
            }
           
        }
           
        public int Score
        {
            get { return score; }
            set 
            {
                score = value;
                OnPropertyChanged("Score");
            }
        }

        private int score;
        private void currTetraminoDraw()
        {


            //Wo malen
            if (lose == false)
            {
                Point position = currTetramino.getCurrPosition();

                //was malen
                Point[] Shape = currTetramino.getCurrShape();

                //Farbe
                SolidColorBrush Colore = currTetramino.getCurrColor() as SolidColorBrush;

                foreach (Point S in Shape)
                {

                    
                    wb.SetPixel((int)(S.X + position.X) + (Cols / 2) - 1, (int)(S.Y + position.Y) + 2, Colore.Color);
                    
                }
            }

        }
        private void currTetraminoErase()
        {
            Point position = currTetramino.getCurrPosition();


            Point[] Shape = currTetramino.getCurrShape();

            foreach (Point S in Shape)
            {
                
                    wb.SetPixel((int)(S.X + position.X) + ((Cols / 2) - 1), (int)(S.Y + position.Y) + 2, Colors.Black);
               
                
            }
        }
        private int level;
        private int _score;
        public bool lose = false;
       
        public bool full;
        public bool remove;
        private void CheckRows()
        {
            for (int i = Rows - 1; i > 0; i--)
            {
                full = true;
                for (int j = 0; j < Cols; j++)
                {

                    if (wb.GetPixel(j, i) == Colors.Black)
                    {
                        full = false;
                    }
                }
                if (full)
                {
                    RemoveRow(i);
                    LinesFilled++;
                    _score += 10000;
                    Score = _score;
                 
                    if (_score % 30000==0)
                    {
                        _level =1 +_score / 30000;
                        Level = _level;     
                    }
                }
            }
        }
        private void RemoveRow(int row)
        {
            remove = true;
            for (int i = row; i > 2; i--)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Color c = wb.GetPixel(j,i-1);
                    wb.SetPixel(j, i, c);
               
                }
            }

        }
        public void CurrTetraminoMoveRight()
        {
            Point Position = currTetramino.getCurrPosition();
            Point[] Shape = currTetramino.getCurrShape();
            bool move = true;
            currTetraminoErase();
            foreach (Point S in Shape)
            {
                if (((int)(S.X + Position.X) + ((Cols / 2) - 1) + 1) >= Cols)
                {
                    move = false;
                }

                else if (wb.GetPixel(((int)(S.X + Position.X) + ((Cols / 2) - 1) + 1), (int)(S.Y + Position.Y) + 2)!=Colors.Black)
                {
                    move = false;
                }
            }
            if (move)
            {
                currTetramino.MoveRight();
                currTetraminoDraw();

            }
            else
            {
                currTetraminoDraw();
            }
        }
        public bool movedown = true;
        public void CurrTetraminoMoveLeft()
        {
            Point Position = currTetramino.getCurrPosition();
            Point[] Shape = currTetramino.getCurrShape();
            bool move = true;
            currTetraminoErase();

            foreach (Point S in Shape)
            {
                if (((int)(S.X + Position.X) + ((Cols / 2) - 1) - 1) < 0)
                {
                    move = false;


                }

                else if (wb.GetPixel(((int)(S.X + Position.X) + ((Cols / 2) - 1) - 1), (int)(S.Y + Position.Y) + 2) != Colors.Black)
                {
                    move = false;

                }
            }
            if (move)
            {
                currTetramino.MoveLeft();
                currTetraminoDraw();


            }
            else
            {
                currTetraminoDraw();
            }
        }
        public bool tetr = false;
        public bool movedownfs = true;
        public bool shadow = true;

        public void CurrTetraminoMoveDown()
        {
            MediaElement player2 = new MediaElement();
            Point Position = currTetramino.getCurrPosition();
            Point[] Shape = currTetramino.getCurrShape();
            movedown = true;

            currTetraminoErase();
            foreach (Point S in Shape)
            {
                if (((int)(S.Y + Position.Y) + 2 + 1) >= Rows)
                {
                    movedown = false;

                }
                   
                else if (wb.GetPixel((int)((S.X + Position.X) + ((Cols / 2) - 1)),((int)(S.Y + Position.Y) + 2 + 1)) != Colors.Black)
                {
                    movedown = false;
                }
               
                if (wb.GetPixel((int)((S.X) + ((Cols / 2) - 1)),((int)(S.Y)+4))!= Colors.Black)
                {
                    movedown = false;
                    lose = true;
                    for (int i = 0; i < wb.PixelHeight; i++)
                    {
                        for (int j = 0; j < wb.PixelWidth; j++)
                        {

                            wb.SetPixel(j, i, Colors.Black);
                        }
                    }
                    _level = 1;
                    Level = _level;
                }

            }


            if (movedown)
            {
                currTetramino.MoveDown();
                currTetraminoDraw();
                tetr = false;

            }
            else
            {
                tetr = true;
                currTetraminoDraw();
                CheckRows();
                currTetramino = new Tetramino();
            }
          
        }
        public void update() 
        { 
            wbm.Clear();
            wbm.Blit(new Rect(0, 0, wbm.PixelWidth, wbm.PixelHeight), wb, new Rect(new Size(wb.PixelWidth, wb.PixelHeight)));
        }
        public void CurrTetraminoMoveFastDown()
        {
        
            MediaElement player2 = new MediaElement();
            Point Position = currTetramino.getCurrPosition();
            Point[] Shape = currTetramino.getCurrShape();
         

            movedown = true;
            currTetraminoErase();
            foreach (Point S in Shape)
            {
                if (((int)(S.Y + Position.Y) + 2 + 1) >= Rows)
                {

                    movedown = false;


                }

                else if (wb.GetPixel(((int)(S.X + Position.X) + ((Cols / 2) - 1)),
                    ((int)(S.Y + Position.Y) + 2 + 1)) !=Colors.Black)
                {
                    movedown = false;

                }

                if (wb.GetPixel(((int)(S.X) + ((Cols / 2) - 1)),
                    ((int)(S.Y) + 2 + 2)) != Colors.Black)
                {
                    movedown = false;
                    lose = true;
                    

                }

            }
            if (movedown)
            {
                currTetramino.MoveDown();
                currTetraminoDraw();
                tetr = false;

            }
            else
            {
                tetr = true;
                currTetraminoDraw();
                CheckRows();
                currTetramino = new Tetramino();


            }
        }
        public void CurrTetraminoMoveRotate()
        {
            Point Position = currTetramino.getCurrPosition();
            Point[] S = new Point[4];
            Point[] Shape = currTetramino.getCurrShape();
            bool move = true;
            Shape.CopyTo(S, 0);
            currTetraminoErase();
            for (int i = 0; i < S.Length; i++)
            {
                double x = S[i].X;
                S[i].X = S[i].Y * -1;
                S[i].Y = x;

                if (((int)((S[i].Y + Position.Y) + 2)) >= Rows)
                {
                    move = false;
                }
                else if (((int)(S[i].X + Position.X) + ((Cols / 2) - 1)) < 1)
                {
                    move = false;
                }
                else if (((int)(S[i].X + Position.X) + ((Cols / 2) - 1) >= Cols - 1))
                {
                    move = false;
                }

                else if (wb.GetPixel(((int)(S[i].X + Position.X) + ((Cols / 2) - 1)),
                    ((int)(S[i].Y + Position.Y) + 2)) != Colors.Black)
                {
                    move = false;
                }
                if (move)
                {
                    currTetramino.MoveRotate();
                    currTetraminoDraw();
                }
                else
                {
                    currTetraminoDraw();
                }
            }
        }
    }
    public class Tetramino
    {
        private Point currPosition;
        private Point[] currShape;
        private Brush currColor;
        private bool rotate;
        public Tetramino()
        {
            currPosition = new Point(0, 0);
            currColor = Brushes.Transparent;
            currShape = setRandomShape();

        }
        public Brush getCurrColor()
        {
            return currColor;
        }
        public Point getCurrPosition()
        {
            return currPosition;
        }
        public Point[] getCurrShape()
        {
            return currShape;
        }
        public void MoveLeft()
        {
            currPosition.X -= 1;
        }
        public void MoveRight()
        {
            currPosition.X++;
        }
        public void MoveDown()
        {
            currPosition.Y++;
        }
        public void MoveRotate()
        {
            if (rotate)
            {
                for (int i = 0; i < currShape.Length; i++)
                {
                    //Super Rotation System 
                    double x = currShape[i].X;
                    currShape[i].X = currShape[i].Y * -1;
                    currShape[i].Y = x;

                }
            }
        }
        private Point[] setRandomShape()
        {
            Random rand = new Random();
            switch (rand.Next() % 7)
            {
                case 0://I
                    rotate = true;
                    currColor = Brushes.Orange;
                    return new Point[]{
                        new Point(0,0),
                        new Point(-1,0),
                        new Point(1,0),
                        new Point(2,0)
                    };
                case 1://L
                    rotate = true;
                    currColor = Brushes.Blue;
                    return new Point[]{
                        new Point(-1,-1),
                        new Point(-1,0),
                        new Point(0,0),
                        new Point(1,0)
                    };
                case 2://J
                    rotate = true;
                    currColor = Brushes.Purple;
                    return new Point[]{
                        new Point(0,0),
                        new Point(-1,0),
                        new Point(1,0),
                        new Point(1,-1)
                    };
                case 3://o
                    rotate = false;
                    currColor = Brushes.Red;
                    return new Point[]{
                        new Point(0,0),
                        new Point(0,1),
                        new Point(1,0),
                        new Point(1,1)
                    };
                case 4://S
                    rotate = true;
                    currColor = Brushes.CornflowerBlue;
                    return new Point[]{
                        new Point(0,0),
                        new Point(-1,0),
                        new Point(0,-1),
                        new Point(1,-1)
                    };
                case 5://T
                    rotate = true;
                    currColor = Brushes.Yellow;
                    return new Point[]{
                        new Point(0,0),
                        new Point(-1,0),
                        new Point(0,-1),
                        new Point(1,0)
                    };
                case 6://Z
                    rotate = true;
                    currColor = Brushes.LightGreen;
                    return new Point[]{
                        new Point(0,0),
                        new Point(-1,0),
                        new Point(0,1),
                        new Point(1,1)
                    };
                default:
                    return null;
            }

        }
    }
    public class TetrisExecute 
    {
        public Tetris t;
        MediaPlayer collison;
        MediaPlayer lineClear;
        MediaPlayer music;
        DispatcherTimer dt;
        DispatcherTimer dt2;
        Window cm;
        HwndSource hwnds;
        Bluetooth bluetooth = Bluetooth.Instance;
        

        private bool tetris_run;

        public bool Tetris_run
        {
            get { return tetris_run; }
            set { tetris_run = value; }
        }


        //Bluetooth Control COmmands
        private void Event_Control_tetris(object sender, PropertyChangeArgs args)
        {
            String control = args.mesg;
            if (args.mesg.Length > 13)
            {
            }

            try
            {
                switch (control)
                {
                    //Es könnten noch Probleme auftreten wegen "Taste gedrückt halten" - mit Kitzmüller besprechen
                    case "down_click":
                        {
                            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,new Action(() =>
                            CentralMonitor_KeyDown(this, new KeyEventArgs(Keyboard.PrimaryDevice,hwnds, 0, Key.Down))
                            ));
                            
                            break;
                        }
                    case "left_click":
                        {
                            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                          CentralMonitor_KeyDown(this, new KeyEventArgs(Keyboard.PrimaryDevice, hwnds, 0, Key.Left))
                          ));
                                                 
                            break;
                        }
                    case "right_click":
                        {
                               Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                          CentralMonitor_KeyDown(this, new KeyEventArgs(Keyboard.PrimaryDevice, hwnds, 0, Key.Right))));
                            
                            break;
                        }
                    case "up_click":
                        {
                            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                         CentralMonitor_KeyDown(this, new KeyEventArgs(Keyboard.PrimaryDevice, hwnds, 0, Key.Up))));
                            break;
                        }
                }

            }
            catch (Exception ex)
            {
            }
        }
        

        public TetrisExecute(WriteableBitmap wbm,DispatcherTimer dt,Window CentralMonitor) 
        {
           
            this.dt = dt;
            CentralMonitor.KeyDown+=CentralMonitor_KeyDown;
            this.t = new Tetris(wbm);
            cm = CentralMonitor;
            hwnds = HwndSource.FromVisual(cm) as HwndSource; 
            collison = new MediaPlayer();
            lineClear = new MediaPlayer();
            music = new MediaPlayer();
            music.Open(new System.Uri("Sounds/Tetris.mp3", UriKind.Relative));
            music.Play();

            Bluetooth.Instance.CommandControlChange += new Bluetooth.PropertyChangeHandler(Event_Control_tetris);

            music.MediaEnded += music_MediaEnded;
            dt2 = new DispatcherTimer();
            dt2.Interval = new TimeSpan(0, 0, 0, 0, 200);
            dt2.Tick += dt2_Tick;
            dt2.Start();
            fastdown = false;
           
        }
        bool fastdown;
        public void resume_Tetris() 
        {
           
            dt2.Start();
            music.Play();
            
            
        }
        public void Stop_Tetris() 
        {
            dt2.Stop();
            collison.Stop();
            music.Stop();
            lineClear.Stop();
            tetris_run = false;
        }
        void dt2_Tick(object sender, EventArgs e)
        {
            tetris_run = true;
            if (t.lose) 
            {
                music.Stop();
                dt2.Stop();

            }
            dt2.Interval = new TimeSpan(0,0,0,0,(int)(200-(t.Level * 200/8)));
          
            t.CurrTetraminoMoveDown();
            if (t.movedown == false)
            {
                

                collison.Open(new System.Uri("Sounds/Boing.mp3", UriKind.Relative));
                collison.Play();
                fastdown = false;


            }

            if (t.remove)
            {

                lineClear.Open(new System.Uri("Sounds/Tetris_Line.mp3", UriKind.Relative));
                lineClear.Play();

            }

            if (fastdown)
            {
                dt2.Interval = new TimeSpan(0, 0, 0, 0, 1);
            }
           
            t.remove = false;
        }
        void music_MediaEnded(object sender, EventArgs e)
        {
            music.Open(new System.Uri("Sounds/Tetris.mp3", UriKind.Relative));
            music.Play();
        }
        public void tetris_exe() 
        {
            t.update();
        }
        private void CentralMonitor_KeyDown(object sender, KeyEventArgs e)
        {
            
            switch (e.Key)
            {
                case Key.Left:
                    if (dt.IsEnabled) t.CurrTetraminoMoveLeft();
                    break;
                case Key.Right:
                    if (dt.IsEnabled) t.CurrTetraminoMoveRight();
                    break;
                case Key.Down:
                    if (dt.IsEnabled) t.CurrTetraminoMoveDown();
                    break;
                case Key.Up:
                    if (dt.IsEnabled) t.CurrTetraminoMoveRotate();
                    break;
                case Key.Space:
                    t.CurrTetraminoMoveFastDown();
                    fastdown = true;
                    break;
                default:
                    break;
            }
        }
    }
}
