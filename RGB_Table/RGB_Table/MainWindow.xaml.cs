using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Threading;
using System.Windows.Threading;
using RGB_Libary;
using RGB_Window.Windows;
using MahApps.Metro.Controls;
using Blue.Windows;




namespace RGB_Window
{

    enum RunningTask
    {
        Draw,
        Effect,
        Image,
        Video,
        Tetris,
        Webcam
    }
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        Port_Window pw;
        ExpandingObjects ex;
        ExpandingObjects_Options e_o;
        MovingText mt;
        MovingText_Options mt_o;
        Plasma p;
        Plasma_Options p_o;
        ColorGradient cg;
        GradientColor_Options gc_o;
        Mask_Options m_o;
        WriteableBitmap wbm, wh;
        DispatcherTimer dt_m;
        RunningTask runningTask;
        Dispatcher_Execute dis_exe;
        execute_effect ex_eff;
        object Configure_Window;
        Rect r;
        Byte[] bitmapbuffer;
        TetrisExecute t_e;
        Draw d;
        Video v;
        Webcam webcam;
        ImagePixelate ip;
        CollisonBalls exp;
        bool draw;
        Point old;
        Point zw;
        WriteableBitmap maskBitmap;
        private int redMask;
        public int RedMask
        {
            get { return redMask; }
            set
            {
                redMask = value;
                maskBitmap.Clear(Color.FromArgb((byte)bright, (byte)redMask, (byte)greenMask, (byte)blueMask));
            }
        }
        private int blueMask;
        public int BlueMask
        {
            get { return blueMask; }
            set
            {
                blueMask = value;
                maskBitmap.Clear(Color.FromArgb((byte)bright, (byte)redMask, (byte)greenMask, (byte)blueMask));
            }
        }
        private int greenMask;
        public int GreenMask
        {
            get { return greenMask; }
            set
            {
                greenMask = value;
                maskBitmap.Clear(Color.FromArgb((byte)bright, (byte)redMask, (byte)greenMask, (byte)blueMask));
            }
        }
        int bright;
        public MainWindow()
        {
            this.InitializeComponent();
            Init_MainWindow();

        }
        public void Init_MainWindow()
        {
            //Die Bildschirmabhängige Anzeige der Software
            Point screenSize = new Point(System.Windows.SystemParameters.FullPrimaryScreenWidth, System.Windows.SystemParameters.FullPrimaryScreenHeight);
            AuroraWindow.Width = screenSize.X * 0.5;
            AuroraWindow.Height = screenSize.Y * 0.9;


            redMask = 255;
            greenMask = 255;
            blueMask = 255;
            bright = 255;


            img3.Width = AuroraWindow.Width * 0.8;
            img3.Height = img3.Width / 1.618;

            //Das SerialportWindow initialisiern 
            pw = new Port_Window();
            pw.data.Changed += new ChangedEventhandler(StatusChanged);


            //Das Rechteck für die Blit funktion festlegen
            r = new Rect(0, 0, 68, 42);

            //Writeable Bitmaps initialisiern
            wbm = BitmapFactory.New(68, 42);
            wh = BitmapFactory.New(68, 42);


            img3.Source = wbm;

            RenderOptions.SetBitmapScalingMode(img3, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(img3, EdgeMode.Aliased);

            dt_m = new DispatcherTimer(DispatcherPriority.Render);
            dt_m.Interval = new TimeSpan(0, 0, 0, 0, 25);
            dt_m.Tick += Aurora_Cycle_Tick;
            dt_m.Start();

            bitmapbuffer = new Byte[wbm.ToByteArray().Length];

            CentralMonitor.IsEnabled = false;





            runningTask = RunningTask.Effect;
            dis_exe = new Dispatcher_Execute(dt_m);
            maskBitmap = BitmapFactory.New(68, 42);

            //Init Objects and Options Windows
            ex = new ExpandingObjects(wbm);
            e_o = new ExpandingObjects_Options(ex);
            mt = new MovingText(wbm);
            mt_o = new MovingText_Options(mt);
            p = new Plasma(wbm);
            p_o = new Plasma_Options(p);
            cg = new ColorGradient(wbm);
            gc_o = new GradientColor_Options(cg);
            StickyWindow.RegisterExternalReferenceForm(this);

            //Bluetooth Event listening
            RGB_Libary.Bluetooth.Instance.CommandControlChange += new Bluetooth.PropertyChangeHandler(Bluetooth_Command_Listener);

        }
        public int Brightness
        {
            get { return bright; }
            set
            {
                bright = value;
                maskBitmap.Clear(Color.FromArgb((byte)bright, (byte)redMask, (byte)greenMask, (byte)blueMask));
            }
        }

        //Verwalten der Commands
        private void Bluetooth_Command_Listener(object sender, PropertyChangeArgs command)
        {
            try
            {
                int Mode = -1;
                string comm = command.mesg;
                System.Diagnostics.Debug.WriteLine("Command wurde übergeben: " + comm);
                switch (comm)
                {
                    case "Paint":         //Paint
                        {
                            Mode = 0;
                            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (ThreadStart)delegate
                            {
                                if (t_e != null)
                                {
                                    t_e.Stop_Tetris();
                                    wbm = BitmapFactory.New(68, 42);
                                    img3.Source = wbm;

                                }

                                runningTask = RunningTask.Draw;
                                draw = true;
                                if (v != null)
                                    v.RunVideo = false;
                                if (webcam != null)
                                    webcam.closeWebcam();

                                try
                                {
                                    d = new Draw(wh, img3, wbm);
                                }
                                catch (Exception exc)
                                {
                                }
                                ex_eff = d.Draw_execute;
                                dis_exe.Execute_Effect = ex_eff;

                                d.setColor = Color.FromArgb(0, 200, 0, 255);
                                d.setDrawtype = Drawtype.point;

                                Binding ColorBinding = new Binding("setColor");
                                ColorBinding.Source = d;
                                ColorBinding.Mode = BindingMode.TwoWay;
                                DrawingColorPicker.SetBinding(Xceed.Wpf.Toolkit.ColorCanvas.SelectedColorProperty, ColorBinding);

                                TaskSettings();
                            });


                            break;
                        }
                    case "Picture":       //Pixelated Picture
                        {
                            Mode = 1;
                            break;
                        }
                    case "Tetris":       //Tetris
                        {
                            Mode = 2;

                            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (ThreadStart)delegate
                            {
                                if (webcam != null)
                                    webcam.closeWebcam();

                                runningTask = RunningTask.Tetris;
                                draw = false;
                                //Versuchsweise eingebaut -> dt_m.Tick ist fehlerquelle -.-  -> sollte eigentlich nicht sein
                                //dt_m.IsEnabled = false;

                                if (t_e == null)
                                {
                                    wbm = BitmapFactory.New(68, 42);
                                    img3.Source = wbm;
                                    t_e = new TetrisExecute(wbm, dt_m, AuroraWindow);
                                    ex_eff = t_e.tetris_exe;
                                    dis_exe.Execute_Effect = ex_eff;

                                    Binding ScoreBinding = new Binding("Score");
                                    ScoreBinding.Mode = BindingMode.OneWay;
                                    ScoreBinding.Source = t_e.t;
                                    Points.SetBinding(Label.ContentProperty, ScoreBinding);

                                    Binding LevelBinding = new Binding("Level");
                                    LevelBinding.Mode = BindingMode.OneWay;
                                    LevelBinding.Source = t_e.t;
                                    Level.SetBinding(Label.ContentProperty, LevelBinding);

                                }
                                else
                                {
                                    t_e.Stop_Tetris();
                                    t_e = new TetrisExecute(wbm, dt_m, AuroraWindow);
                                    ex_eff = t_e.tetris_exe;
                                    dis_exe.Execute_Effect = ex_eff;
                                    Binding ScoreBinding = new Binding("Score");
                                    ScoreBinding.Mode = BindingMode.TwoWay;
                                    ScoreBinding.Source = t_e.t;
                                    Points.SetBinding(Label.ContentProperty, ScoreBinding);
                                }
                                TaskSettings();
                            });


                            break;
                        }

                    default:
                        {
                            //hier wäre normalerweise ein switch(mode) -> unnötig?

                            break;
                        }
                }

            }
            catch (Exception exc)
            {

            }
        }

        public void StatusChanged(object sender, BoolArgs b)
        {
            if (pw != null)
                switch (pw.data.Connected)
                {
                    case true:
                        Status.Content = "Verbunden";
                        break;
                    case false:
                        Status.Content = "Getrennt";
                        break;
                }

        }

        private void Window_Bar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void Init_Connect_Window()
        {

            pw.Owner = this.AuroraWindow;

            if (pw.IsActive == false)
            {
                //pw.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                pw.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                pw.ShowDialog();

            }
            else if (!pw.IsVisible)
            {
                pw.Visibility = Visibility.Visible;

            }
        }
        private void Aurora_Cycle_Tick(object sender, EventArgs e)
        {


            bitmapbuffer = wbm.ToByteArray();
            pw.data.setTransferBytes = bitmapbuffer;
            if (ex_eff != null)
                ex_eff();


            wbm.Blit(new Rect(new Size(68, 42)), maskBitmap, new Rect(new Size(68, 42)), WriteableBitmapExtensions.BlendMode.Multiply);


        }
        private void Menu_Mouse_Down(object sender, RoutedEventArgs e)
        {
            FrameworkElement feSource = e.Source as FrameworkElement;
            switch (feSource.Name)
            {
                case "Connect":
                    Init_Connect_Window();
                    break;
                case "Bluetooth":
                    Bluetooth_Window b_w = new Bluetooth_Window(this);
                    b_w.Show();
                    break;
                case "Draw":
                    if (t_e != null)
                    {
                        t_e.Stop_Tetris();
                        wbm = BitmapFactory.New(68, 42);
                        img3.Source = wbm;
                    }
                    if (v != null)
                        v.RunVideo = false;
                    if (webcam != null)
                        webcam.closeWebcam();

                    runningTask = RunningTask.Draw;
                    draw = true;


                    d = new Draw(wh,img3,wbm);
                    ex_eff = d.Draw_execute;
                    dis_exe.Execute_Effect = ex_eff;

                    d.setColor = Color.FromArgb(255, 0, 0, 255);
                    d.setDrawtype = Drawtype.rectangle;

                    Binding ColorBinding = new Binding("setColor");
                    ColorBinding.Source = d;
                    ColorBinding.Mode = BindingMode.TwoWay;
                    DrawingColorPicker.SetBinding(Xceed.Wpf.Toolkit.ColorCanvas.SelectedColorProperty, ColorBinding);

                    break;
                case "Effects":
                    if (t_e != null)
                    {
                        t_e.Stop_Tetris();
                        wbm = BitmapFactory.New(68, 42);
                        img3.Source = wbm;

                    }
                    if (v != null)
                        v.RunVideo = false;
                    if (webcam != null)
                        webcam.closeWebcam();

                    runningTask = RunningTask.Effect;
                    draw = false;
                    ex_eff = p.Plasma_execute;
                    dis_exe.Execute_Effect = ex_eff;
                    dt_m.IsEnabled = true;

                    break;
                case "Images":
                    if (t_e != null)
                    {
                        t_e.Stop_Tetris();
                        wbm = BitmapFactory.New(68, 42);
                        img3.Source = wbm;

                    }
                    if (webcam != null)
                        webcam.closeWebcam();
                    if (v != null)
                        v.RunVideo = false;


                    draw = false;
                    img3.Source = wbm;
                    runningTask = RunningTask.Image;

                    ip = new ImagePixelate(wbm);
                    ex_eff = ip.execute;
                    dis_exe.Execute_Effect = ex_eff;



                    break;

                case "Video":
                    if (t_e != null)
                    {
                        t_e.Stop_Tetris();

                        img3.Source = wbm;

                    }
                    if (webcam != null)
                        webcam.closeWebcam();
                    runningTask = RunningTask.Video;

                    CentralMonitor.IsEnabled = false;
                    draw = false;

                    v = new Video(wbm);
                    ex_eff = v.video_execute;
                    dis_exe.Execute_Effect = ex_eff;

                    v.RunVideo = true;

                    break;
                case "Tetris":

                    if (webcam != null)
                        webcam.closeWebcam();
                    if (v != null)
                        v.RunVideo = false;
                    runningTask = RunningTask.Tetris;
                    draw = false;


                    if (t_e == null)
                    {

                        t_e = new TetrisExecute(wbm, dt_m, AuroraWindow);
                        ex_eff = t_e.tetris_exe;
                        dis_exe.Execute_Effect = ex_eff;


                    }
                    else
                    {
                        t_e.Stop_Tetris();
                        t_e = new TetrisExecute(wbm, dt_m, AuroraWindow);
                        ex_eff = t_e.tetris_exe;
                        dis_exe.Execute_Effect = ex_eff;
                    }
                    break;
                case "Webcam":
                    wbm = BitmapFactory.New(68, 42);
                    img3.Source = wbm;
                    runningTask = RunningTask.Webcam;
                    CentralMonitor.IsEnabled = false;
                    draw = false;

                    if (t_e != null)
                        t_e.Stop_Tetris();
                    if (webcam == null)
                    {
                        webcam = new Webcam(wbm);
                    }
                    if (v != null)
                        v.RunVideo = false;
                    webcam.resumeWebcam(wbm);
                    ex_eff = webcam.Webcam_execute;
                    dis_exe.Execute_Effect = ex_eff;
                    break;

            }
            TaskSettings();
        }
        private void TaskSettings()
        {
            switch (runningTask)
            {
                case RunningTask.Draw:
                    VideoBar.Visibility = Visibility.Hidden;
                    DrawingBar.Visibility = Visibility.Visible;
                    EffectSelection.Visibility = Visibility.Collapsed;
                    ImageBar.Visibility = Visibility.Collapsed;
                    Tetrisbar.Visibility = Visibility.Collapsed;
                    CentralMonitor.IsEnabled = true;

                    wbm.Clear();
                    wh.Clear();
                    break;

                case RunningTask.Effect:
                    DrawingBar.Visibility = Visibility.Hidden;
                    ImageBar.Visibility = Visibility.Collapsed;
                    CentralMonitor.IsEnabled = false;
                    VideoBar.Visibility = Visibility.Collapsed;
                    EffectSelection.Visibility = Visibility.Visible;
                    Tetrisbar.Visibility = Visibility.Collapsed;
                    img3.Source = wbm;
                    wbm.Clear();
                    break;
                case RunningTask.Image:
                    ImageBar.Visibility = Visibility.Visible;
                    VideoBar.Visibility = Visibility.Collapsed;
                    DrawingBar.Visibility = Visibility.Hidden;
                    EffectSelection.Visibility = Visibility.Collapsed;
                    Tetrisbar.Visibility = Visibility.Collapsed;
                    img3.Source = wbm;
                    wbm.Clear();
                    wh.Clear();

                    break;
                case RunningTask.Video:
                    ImageBar.Visibility = Visibility.Collapsed;
                    VideoBar.Visibility = Visibility.Visible;
                    DrawingBar.Visibility = Visibility.Hidden;
                    EffectSelection.Visibility = Visibility.Collapsed;
                    Tetrisbar.Visibility = Visibility.Collapsed;
                    img3.Source = wbm;
                    wbm.Clear();
                    wh.Clear();
                    break;
                case RunningTask.Tetris:

                    Binding ScoreBinding = new Binding("Score");
                    ScoreBinding.Mode = BindingMode.OneWay;
                    ScoreBinding.Source = t_e.t;
                    Points.SetBinding(Label.ContentProperty, ScoreBinding);

                    Binding LevelBinding = new Binding("Level");
                    LevelBinding.Mode = BindingMode.OneWay;
                    LevelBinding.Source = t_e.t;
                    Level.SetBinding(Label.ContentProperty, LevelBinding);

                    ImageBar.Visibility = Visibility.Collapsed;
                    VideoBar.Visibility = Visibility.Collapsed;
                    DrawingBar.Visibility = Visibility.Collapsed;
                    EffectSelection.Visibility = Visibility.Collapsed;
                    Tetrisbar.Visibility = Visibility.Visible;
                    img3.Source = wbm;
                    wbm.Clear();
                    wh.Clear();
                    break;
                case RunningTask.Webcam:
                    ImageBar.Visibility = Visibility.Collapsed;
                    VideoBar.Visibility = Visibility.Collapsed;
                    DrawingBar.Visibility = Visibility.Hidden;
                    EffectSelection.Visibility = Visibility.Collapsed;
                    Tetrisbar.Visibility = Visibility.Collapsed;
                    img3.Source = wbm;
                    wbm.Clear();
                    wh.Clear();
                    break;
                default:
                    break;
            }


        }
        private void Configure_Mouse_Down(object sender, RoutedEventArgs e)
        {

            if (Configure_Window != null)
                switch (Configure_Window.ToString())
                {

                    case "RGB_Window.Windows.ExpandingObjects_Options":
                        e_o.Show();
                        break;

                    case "RGB_Window.Windows.MovingText_Options":
                        mt_o.Show();
                        break;
                    case "RGB_Window.Windows.GradientColor_Options":
                        gc_o.Show();
                        break;
                    case "RGB_Window.Windows.Plasma_Options":
                        p_o.Show();
                        break;

                }
        }
        private void HideOptionWindow()
        {
            if (Configure_Window != null)
                switch (Configure_Window.ToString())
                {

                    case "RGB_Window.Windows.ExpandingObjects_Options":
                        e_o.Hide();
                        break;

                    case "RGB_Window.Windows.MovingText_Options":
                        mt_o.Hide();
                        break;
                    case "RGB_Window.Windows.GradientColor_Options":
                        gc_o.Hide();
                        break;
                    case "RGB_Window.Windows.Plasma_Options":
                        p_o.Hide();
                        break;

                }
        }
        private void EffectSelectionDropDownClosed(object sender, EventArgs e)
        {
            HideOptionWindow();
            switch (LeftMonitorSelect.SelectionBoxItem.ToString())
            {
                case "ExpandingObjects":
                    ex_eff = ex.ExpandingObjects_execute;
                    dis_exe.Execute_Effect = ex_eff;
                    Configure_Window = e_o;
                    e_o.Show();
                    break;
                case "MovingBalls":
                    exp = new CollisonBalls(wbm);
                    ex_eff = exp.CollisonBalls_execute;
                    dis_exe.Execute_Effect = ex_eff;
                    break;
                case "ScrollingText":
                    ex_eff = mt.MovingText_execute;
                    dis_exe.Execute_Effect = ex_eff;
                    Configure_Window = mt_o;
                    mt_o.Show();
                    break;
                case "Plasma":
                    p_o.Show();
                    Configure_Window = p_o;
                    ex_eff = p.Plasma_execute;
                    dis_exe.Execute_Effect = ex_eff;
                    break;
                case "GradientColor":
                    gc_o.Show();
                    Configure_Window = gc_o;
                    cg.GradientMode = Gradient.Radial;
                    ex_eff = cg.ColorGradient_execute;
                    dis_exe.Execute_Effect = ex_eff;
                    break;
                case "None":
                    wbm.Clear();
                    ex_eff = null;
                    dis_exe.Execute_Effect = ex_eff;
                    Configure_Window = null;
                    break;
            }
        }
        private void DrawingLabels_MouseDown(object sender, EventArgs e)
        {
            FrameworkElement f = sender as FrameworkElement;
            switch (f.Name)
            {
                case "PlayVideo":
                    if (v != null)
                    {
                        v.Path = VideoPath.Text;
                        v.play();
                    }
                    break;
                case "LoadVideo":
                    if (v != null)
                        v.path();
                    VideoPath.Text = v.Path;
                    break;
                case "LoadImage":
                    if (ip != null)
                    {
                        ip.path();
                        ImagePath.Text = ip.Path;
                    }
                    break;
                case "Rectangle":
                    d.setDrawtype = Drawtype.rectangle;
                    break;
                case "Line":
                    d.setDrawtype = Drawtype.line;
                    break;
                case "Point":
                    d.setDrawtype = Drawtype.point;
                    break;
                case "Ellipse":
                    d.setDrawtype = Drawtype.circle;
                    break;
                case "FilledEllipse":
                    d.setDrawtype = Drawtype.fillcircle;
                    break;
                case "FilledRectangle":
                    d.setDrawtype = Drawtype.fillrectangle;
                    break;

            }
        }
        private void Draw_SetOrgin(object sender, RoutedEventArgs e)
        {

            if (d != null)
            {

                Point mouse = new Point();
                mouse.X = 68 * Mouse.GetPosition(CentralMonitor).X / CentralMonitor.ActualWidth;
                mouse.Y = 42 * Mouse.GetPosition(CentralMonitor).Y / CentralMonitor.ActualHeight;
                d.setOrigin = mouse;
                zw = mouse;

            }
        }
        private void BlitWithLayer(object sender, RoutedEventArgs e)
        {
            if (CentralMonitor.IsEnabled)
            {
                img3.Source = wbm;
                wbm.Blit(new Rect(new Size(68, 42)), wh, new Rect(new Size(68, 42)));
                wh.Blit(new Rect(new Size(68, 42)), wbm, new Rect(new Size(68, 42)));
            }

        }
        private void DrawOnMonitor(object sender, RoutedEventArgs e)
        {
            if (d != null && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                Point mouse = new Point();
                mouse.X = 68 * Mouse.GetPosition(img3).X / img3.ActualWidth;
                mouse.Y = 42 * Mouse.GetPosition(img3).Y / img3.ActualHeight;

                if (zw != mouse)
                {
                    old = zw;
                    d.setMousePos = old;
                    d.del();

                    zw = mouse;

                }
                else
                {
                    zw = mouse;

                }
                d.setMousePos = mouse;
                d.draw();

            }
        }
        private void Draw_enableMonitor(object sender, MouseButtonEventArgs e)
        {

            if (draw)
                CentralMonitor.IsEnabled = true;
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (HeaderColor.IsSelected || HeaderShape.IsSelected)
                CentralMonitor.Visibility = Visibility.Collapsed;
            else
                CentralMonitor.Visibility = Visibility.Visible;
            this.UpdateDefaultStyle();

        }
        private void AuroraWindow_Closed_1(object sender, EventArgs e)
        {
            try
            {
                if (webcam != null)
                {
                    webcam.closeWebcam();
                }

                this.Close();
            }
            catch { }

        }
        private void AuroraWindow_Loaded_1(object sender, RoutedEventArgs e)
        {
            m_o = new Mask_Options(this);
            m_o.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            try
            {
                var position = this.PointToScreen(new Point(0, 0));
                m_o.Left = position.X + this.Width;
                m_o.Top = position.Y;
                m_o.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void AuroraWindow_StateChanged(object sender, EventArgs e)
        {
            if (m_o != null)
            {
                if (this.WindowState == WindowState.Minimized)
                {

                    m_o.WindowState = WindowState.Minimized;
                }
                if (this.WindowState == WindowState.Normal)
                    m_o.WindowState = WindowState.Normal;
            }

        }

    }
}
