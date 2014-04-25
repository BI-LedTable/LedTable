
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
using RgbLibrary;
using Aurora.Windows;
using MahApps.Metro.Controls;
using Blue.Windows;

namespace Aurora
{
  
    /// <summary>
    /// Running Task is a <see cref="System.Enum"/> needed for the interface, to specify which user controls have to be hided or shown.
    /// <remarks>
    /// They are set in <see cref="MainWindow.Menu_Mouse_Down"/>, if the user changes to another menu item.
    /// </remarks>
    /// </summary>
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
    /// The delegate which is executet in <see cref="Aurora.MainWindow.monitorTimer_Tick"/>
    /// </summary>
    public delegate void execute_effect();
    
    /// <summary>
    /// The MainWindow is the entry point of the application
    /// 
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// The monitor is the core element of the Aurora application.
        /// It has a size of 68 x 42, which is exactly the size of the table.
        /// To stay fast and at low processor utilization the WriteableBitmap is the best decision.
        /// In combination  with the WriteableBitmapEx.Wpf Extension from the developer site http://writeablebitmapex.codeplex.com/
        /// the useability of the Writeablebitmap gets extremly improved.
        /// <para>The Extension includes a lot of features, the most important are
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// SetPixel method with various overloads
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// GetPixel method, to get the pixel color at a specified x,y coordinate
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Fast Clone methode to copy a WriteableBitmap
        /// </description>
        /// </item>
        /// <item>
        /// <description>shapes, filled shapes</description>
        /// </item>
        /// <item>
        /// <description>blitting functionalities, with different blend modes (filter for bitmaps)</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        WriteableBitmap monitor;
        /// <summary>
        /// The drawlayer, as it name say, is needed, to not overdraw the painted picture.
        /// Whatever you want to draw is saved in the layer and after it joined together with the <see cref="Aurora.MainWindow.monitor"/>
        /// </summary>
        WriteableBitmap drawlayer;
        /// <summary>
        /// The filterBitmap is an extra layer that is joined with the <see cref="Aurora.MainWindow.monitor"/>
        /// to maipulate the color, whether it is alpha channel or color channel.
        /// The WriteablebitmapEx.Wpf dll provides multiple functions,how the layers are combined.
        /// <para>Multiply blend mode multiplies the numbers for each pixel of the top layer(<see cref="Aurora.MainWindow.filterBitmap"/>) with the corresponding pixel for the bottom layer(<see cref="Aurora.MainWindow.monitor"/>).</para>
        /// </summary>
        WriteableBitmap filterBitmap;
        /// <summary>
        /// The monitorTimer is the heartbeat of the application 
        /// in it`s <see cref="Aurora.MainWindow.monitorTimer_Tick"/> event, the <see cref="Aurora.MainWindow.monitor"/> is updatet
        /// and as result the data to send.
        /// The DispatcherTimer is a special sort of timer, which is allowed to access to the ui or apply changes.
        /// </summary>
        DispatcherTimer monitorTimer;
        /// <summary>
        /// <seealso cref="Aurora.RunningTask"/>
        /// </summary>
        RunningTask runningTask;
        /// <summary>
        /// <see cref="Aurora.Windows.MovingTextOptionsWindow"/>
        /// </summary>
        MovingTextOptionsWindow movingTextOptionWindow;
        GradientColorOptionsWindow gradientOptionWindow;
        FilterOptionWindow filterOptionWindow;
        PlasmaOptionsWindow plasmaOptionWindow;
        ExpandingObjectsOptionsWindow expandingObjectsWindow;
        PortWindow portWindow;
       
        ExpandingObjects expandingObjects;
        ColorGradient colorGradient;
        MovingText movingText;
        Plasma plasma;
        CollisonBalls exp;

        Draw draw;
        Video video;
        Webcam webcam;
        ImagePixelate ip;
        TetrisExecute tetris;
        
      
        execute_effect ex_eff;
        Byte[] bitmapbuffer;
        object SelectedWindow;
        Rect r;
        bool drawAccept;
        Point old;
        Point zw;
       
      
        private int redMask;
        /// <summary>
        /// Set an <seealso cref="System.Int32"/> value <seealso cref="Aurora.MainWindow.redMask"/>, 
        /// to vary the red quantity of the <seealso cref="Aurora.MainWindow.monitor"/>
        /// </summary>
        public int RedMask
        {
            get { return redMask; }
            set
            {
                redMask = value;
                filterBitmap.Clear(Color.FromArgb((byte)bright, 
                                    (byte)redMask, (byte)greenMask, (byte)blueMask));
            }
        }
        private int blueMask;
        /// <summary>
        /// Set an <seealso cref="System.Int32"/> value <seealso cref="Aurora.MainWindow.blueMask"/>, 
        /// to vary the blue quantity of the <seealso cref="Aurora.MainWindow.monitor"/>
        /// </summary>
        public int BlueMask
        {
            get { return blueMask; }
            set
            {
                blueMask = value;
                filterBitmap.Clear(Color.FromArgb((byte)bright, (byte)redMask, (byte)greenMask, (byte)blueMask));
            }
        }
        private int greenMask;
        /// <summary>
        /// Set an <seealso cref="System.Int32"/> value <seealso cref="Aurora.MainWindow.greenMask"/>, 
        /// to vary the geen quantity of the <seealso cref="Aurora.MainWindow.monitor"/>
        /// </summary>
        public int GreenMask
        {
            get { return greenMask; }
            set
            {
                greenMask = value;
                filterBitmap.Clear(Color.FromArgb((byte)bright, (byte)redMask, (byte)greenMask, (byte)blueMask));
            }
        }
        private int bright;
        /// <summary>
        /// Set an <seealso cref="System.Int32"/> value <seealso cref="Aurora.MainWindow.bright"/>, 
        /// to vary the brightness  of the <seealso cref="Aurora.MainWindow.monitor"/>
        /// </summary>
        public int Brightness
        {
            get { return bright; }
            set
            {
                bright = value;
                filterBitmap.Clear(Color.FromArgb((byte)bright, (byte)redMask, (byte)greenMask, (byte)blueMask));
            }
        }
        
        /// <summary>
        /// Constructor MainWindow initialises the components and calls the void function <seealso cref="Aurora.MainWindow.InitMainWindow"/>
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            InitMainWindow();
        }

        /// <summary>
        /// <list type="bullet">
        /// <item>
        /// <description>AuroraWindow size is suitet to the user display</description>
        /// </item>
        /// <item>
        /// <description>The rgb mask and brigthness is set to 256 default</description>
        /// </item>
        /// <item>
        /// <description>The monitorImg <see cref=""/> gets initialised in proportion to the AuroraWindow </description>
        /// </item>
        /// <item>
        /// <description>The portWindow <seealso cref=""/> is initialised and the eventhandler data changed add, which is interrupted if the connection
        /// is cancelated or startet</description>
        /// </item>
        /// <item>
        /// <description>A very important point in InitMainWindow is to set the monitorImg.Source to monitor, otherwise nothing appears on the surface</description>
        /// </item>
        /// <item>
        /// <description>The RenderOptions.SetEdgeMode(monitorImg, EdgeMode.Aliased) EdgeMode property squares the pixel and guarantes clear edges on the 68 x 42 resolution</description>
        /// </item>
        /// <item>
        /// <description>The monitorTimer.Tick event has a frequence of 40 Hz</description>
        /// </item>
        /// </list>
        /// </summary>
        public void InitMainWindow()
        {
       
            Point screenSize = new Point(System.Windows.SystemParameters.FullPrimaryScreenWidth, System.Windows.SystemParameters.FullPrimaryScreenHeight);
            AuroraWindow.Width = screenSize.X * 0.4;
            AuroraWindow.Height = screenSize.Y * 0.9;


            redMask = 255;
            greenMask = 255;
            blueMask = 255;
            bright = 255;


            monitorImg.Width = AuroraWindow.Width * 0.85;
            monitorImg.Height = monitorImg.Width / 1.618;

            //Das SerialportWindow initialisiern 
            portWindow = new PortWindow();
            portWindow.data.Changed += new ChangedEventhandler(StatusChanged);


            //Das Rechteck für die Blit funktion festlegen
            r = new Rect(0, 0, 68, 42);

            //Writeable Bitmaps initialisiern
            monitor = BitmapFactory.New(68, 42);
            drawlayer = BitmapFactory.New(68, 42);


            monitorImg.Source = monitor;

            RenderOptions.SetBitmapScalingMode(monitorImg, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(monitorImg, EdgeMode.Aliased);

            monitorTimer = new DispatcherTimer(DispatcherPriority.Render);
            monitorTimer.Interval = new TimeSpan(0, 0, 0, 0, 25);
            monitorTimer.Tick += monitorTimer_Tick;
            monitorTimer.Start();

            bitmapbuffer = new Byte[monitor.ToByteArray().Length];

            CentralMonitor.IsEnabled = false;

            runningTask = RunningTask.Effect;
            filterBitmap = BitmapFactory.New(68, 42);

            //Init Objects and Options Windows
            expandingObjects = new ExpandingObjects(monitor);
            expandingObjectsWindow = new ExpandingObjectsOptionsWindow(expandingObjects);
            movingText = new MovingText(monitor);
            movingTextOptionWindow = new MovingTextOptionsWindow(movingText);
            plasma = new Plasma(monitor);
            plasmaOptionWindow = new PlasmaOptionsWindow(plasma);
            colorGradient = new ColorGradient(monitor);
            gradientOptionWindow = new GradientColorOptionsWindow(colorGradient);
            StickyWindow.RegisterExternalReferenceForm(this);

            //Bluetooth Event listening
            RgbLibrary.Bluetooth.Instance.CommandControlChange += new Bluetooth.PropertyChangeHandler(Bluetooth_Command_Listener);

        }

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
                                if (tetris != null)
                                {
                                    tetris.Stop_Tetris();
                                    monitor = BitmapFactory.New(68, 42);
                                    monitorImg.Source = monitor;

                                }

                                runningTask = RunningTask.Draw;
                                drawAccept = true;
                                if (video != null)
                                    video.RunVideo = false;
                                if (webcam != null)
                                    webcam.closeWebcam();

                                try
                                {
                                    draw = new Draw(drawlayer, monitorImg, monitor);
                                }
                                catch (Exception exc)
                                {
                                }
                                ex_eff = draw.Draw_execute;

                                draw.setColor = Color.FromArgb(0, 200, 0, 255);
                                draw.setDrawtype = Drawtype.point;

                                Binding ColorBinding = new Binding("setColor");
                                ColorBinding.Source = draw;
                                ColorBinding.Mode = BindingMode.TwoWay;
                                DrawingColorPicker.SetBinding(Xceed.Wpf.Toolkit.ColorCanvas.SelectedColorProperty, ColorBinding);

                                SetUserControls();
             
                                object GlassOfSugar = new  Object();
                                Slider slider = new Slider();
                                Binding AmountBinding = new Binding("Amount");
                                AmountBinding.Source = GlassOfSugar;
                                AmountBinding.Mode = BindingMode.TwoWay;
                                slider.SetBinding(Slider.ValueProperty, AmountBinding);


                            });


                            break;
                        }
                    case "Fotobearbeitung":       //Pixelated Picture
                        {
                            Mode = 1;

                            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (ThreadStart)delegate
                            {
                                if (tetris != null)
                                {
                                    tetris.Stop_Tetris();
                                    monitor = BitmapFactory.New(68, 42);
                                    monitorImg.Source = monitor;

                                }

                                runningTask = RunningTask.Draw;
                                drawAccept = true;
                                if (video != null)
                                    video.RunVideo = false;
                                if (webcam != null)
                                    webcam.closeWebcam();

                                try
                                {
                                    draw = new Draw(drawlayer, monitorImg, monitor);
                                }
                                catch (Exception exc)
                                {
                                }
                                ex_eff = draw.Draw_execute;

                                draw.setColor = Color.FromArgb(0, 200, 0, 255);
                                draw.setDrawtype = Drawtype.point;

                                Binding ColorBinding = new Binding("setColor");
                                ColorBinding.Source = draw;
                                ColorBinding.Mode = BindingMode.TwoWay;
                                DrawingColorPicker.SetBinding(Xceed.Wpf.Toolkit.ColorCanvas.SelectedColorProperty, ColorBinding);

                                SetUserControls();

                                object GlassOfSugar = new Object();
                                Slider slider = new Slider();
                                Binding AmountBinding = new Binding("Amount");
                                AmountBinding.Source = GlassOfSugar;
                                AmountBinding.Mode = BindingMode.TwoWay;
                                slider.SetBinding(Slider.ValueProperty, AmountBinding);


                            });

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
                                drawAccept = false;
                                //Versuchsweise eingebaut -> dt_m.Tick ist fehlerquelle -.-  -> sollte eigentlich nicht sein
                                //dt_m.IsEnabled = false;

                                if (tetris == null)
                                {
                                    monitor = BitmapFactory.New(68, 42);
                                    monitorImg.Source = monitor;
                                    tetris = new TetrisExecute(monitor, monitorTimer, AuroraWindow);
                                    ex_eff = tetris.tetris_exe;
                                

                                    Binding ScoreBinding = new Binding("Score");
                                    ScoreBinding.Mode = BindingMode.OneWay;
                                    ScoreBinding.Source = tetris.t;
                                    Points.SetBinding(Label.ContentProperty, ScoreBinding);

                                    Binding LevelBinding = new Binding("Level");
                                    LevelBinding.Mode = BindingMode.OneWay;
                                    LevelBinding.Source = tetris.t;
                                    Level.SetBinding(Label.ContentProperty, LevelBinding);

                                }
                                else
                                {
                                    tetris.Stop_Tetris();
                                    tetris = new TetrisExecute(monitor, monitorTimer, AuroraWindow);
                                    ex_eff = tetris.tetris_exe;
                                  
                                    Binding ScoreBinding = new Binding("Score");
                                    ScoreBinding.Mode = BindingMode.TwoWay;
                                    ScoreBinding.Source = tetris.t;
                                    Points.SetBinding(Label.ContentProperty, ScoreBinding);
                                }
                                SetUserControls();
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
        /// <summary>
        /// Das ist super
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="b"></param>
        public void StatusChanged(object sender, BoolArgs b)
        {
            if (portWindow != null)
                switch (portWindow.data.Connected)
                {
                    case true:
                        Status.Content = "Verbunden";
                        break;
                    case false:
                        Status.Content = "Getrennt";
                        break;
                }

        }
       
        private void Init_Connect_Window()
        {

            portWindow.Owner = this.AuroraWindow;

            if (portWindow.IsActive == false)
            {
                //pw.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                portWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                portWindow.ShowDialog();

            }
            else if (!portWindow.IsVisible)
            {
                portWindow.Visibility = Visibility.Visible;

            }
        }

        /// <summary>
        /// This event loads the data of the monitorbitmap into a byte array
        /// the bytearray is set as a property of <seealso cref="RgbLibrary.DataOut"/> class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void monitorTimer_Tick(object sender, EventArgs e)
        {

            bitmapbuffer = monitor.ToByteArray();
            portWindow.data.TransferBytes = bitmapbuffer;
            if (ex_eff != null)
                ex_eff();
            monitor.Blit(new Rect(new Size(68, 42)), filterBitmap, new Rect(new Size(68, 42)),
                        WriteableBitmapExtensions.BlendMode.Multiply);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Mouse_Down(object sender, RoutedEventArgs e)
        {
            FrameworkElement feSource = e.Source as FrameworkElement;
            switch (feSource.Name)
            {
                case "Connect":
                    Init_Connect_Window();
                    break;
                case "Bluetooth":
                    BluetoothWindow b_w = new BluetoothWindow(this);
                    b_w.Show();
                    break;
                case "Draw":
                    if (tetris != null)
                    {
                        tetris.Stop_Tetris();
                      
                    }
                    if (video != null)
                        video.RunVideo = false;
                    if (webcam != null)
                        webcam.closeWebcam();

                    runningTask = RunningTask.Draw;
                    drawAccept = true;

                    draw = new Draw(drawlayer,monitorImg,monitor);
                    ex_eff = draw.Draw_execute;
                   

                    draw.setColor = Color.FromArgb(255, 0, 0, 255);
                    draw.setDrawtype = Drawtype.rectangle;

                    Binding ColorBinding = new Binding("setColor");
                    ColorBinding.Source = draw;
                    ColorBinding.Mode = BindingMode.TwoWay;
                    DrawingColorPicker.SetBinding(Xceed.Wpf.Toolkit.ColorCanvas.SelectedColorProperty, ColorBinding);

                    break;
                case "Effects":
                    if (tetris != null)
                    {
                        tetris.Stop_Tetris();
                      
                    }
                    if (video != null)
                        video.RunVideo = false;
                    if (webcam != null)
                        webcam.closeWebcam();

                    
                    runningTask = RunningTask.Effect;
                    drawAccept = false;
                    ex_eff = plasma.Plasma_execute;
                  
                    monitorTimer.IsEnabled = true;

                    break;
                case "Images":
                    if (tetris != null)
                    {
                        tetris.Stop_Tetris();
                        monitor = BitmapFactory.New(68, 42);
                        monitorImg.Source = monitor;

                    }
                    if (webcam != null)
                        webcam.closeWebcam();
                    if (video != null)
                        video.RunVideo = false;


                    drawAccept = false;
                    monitorImg.Source = monitor;
                    runningTask = RunningTask.Image;

                    ip = new ImagePixelate(monitor);
                    ex_eff = ip.execute;
                   



                    break;

                case "Video":
                    if (tetris != null)
                    {
                        tetris.Stop_Tetris();

                        monitorImg.Source = monitor;

                    }
                    if (webcam != null)
                        webcam.closeWebcam();
                    runningTask = RunningTask.Video;

                    CentralMonitor.IsEnabled = false;
                    drawAccept = false;

                    video = new Video(monitor);
                    ex_eff = video.video_execute;
                   

                    video.RunVideo = true;

                    break;
                case "Tetris":

                    if (webcam != null)
                        webcam.closeWebcam();
                    if (video != null)
                        video.RunVideo = false;
                    runningTask = RunningTask.Tetris;
                    drawAccept = false;


                    if (tetris == null)
                    {

                        tetris = new TetrisExecute(monitor, monitorTimer, AuroraWindow);
                        ex_eff = tetris.tetris_exe;
                      


                    }
                    else
                    {
                        tetris.Stop_Tetris();
                        tetris = new TetrisExecute(monitor, monitorTimer, AuroraWindow);
                        ex_eff = tetris.tetris_exe;
                      
                    }
                    break;
                case "Webcam":
                    runningTask = RunningTask.Webcam;
                    CentralMonitor.IsEnabled = false;
                    drawAccept = false;

                    if (tetris != null)
                        tetris.Stop_Tetris();
                    if (webcam == null)
                    {
                        webcam = new Webcam(monitor);
                    }
                    if (video != null)
                        video.RunVideo = false;
                    webcam.resumeWebcam(monitor);
                    ex_eff = webcam.Webcam_execute;
                   
                    break;

            }
            SetUserControls();
        }
        /// <summary>
        /// SetUserControls is called in <seealso cref="Aurora.Menu_Mouse_Down"/>
        /// the layoutpanels are hided or shown with the <seealso cref="System.Windows.Visibility"/> property
        /// </summary>
        private void SetUserControls()
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
                    monitor.Clear();
                    drawlayer.Clear();
                    break;

                case RunningTask.Effect:
                    DrawingBar.Visibility = Visibility.Collapsed;
                    ImageBar.Visibility = Visibility.Collapsed;
                    CentralMonitor.IsEnabled = false;
                    VideoBar.Visibility = Visibility.Collapsed;
                    EffectSelection.Visibility = Visibility.Visible;
                    Tetrisbar.Visibility = Visibility.Collapsed;
                    monitorImg.Source = monitor;
                    monitor.Clear();
                    break;
                case RunningTask.Image:
                    ImageBar.Visibility = Visibility.Visible;
                    VideoBar.Visibility = Visibility.Collapsed;
                    DrawingBar.Visibility = Visibility.Collapsed;
                    EffectSelection.Visibility = Visibility.Collapsed;
                    Tetrisbar.Visibility = Visibility.Collapsed;
                    CentralMonitor.IsEnabled = false;
                    monitorImg.Source = monitor;
                    monitor.Clear();
                    drawlayer.Clear();

                    break;
                case RunningTask.Video:
                    ImageBar.Visibility = Visibility.Collapsed;
                    VideoBar.Visibility = Visibility.Visible;
                    DrawingBar.Visibility = Visibility.Collapsed;
                    EffectSelection.Visibility = Visibility.Collapsed;
                    Tetrisbar.Visibility = Visibility.Collapsed;
                    monitorImg.Source = monitor;
                    monitor.Clear();
                    drawlayer.Clear();
                    break;
                case RunningTask.Tetris:

                    Binding ScoreBinding = new Binding("Score");
                    ScoreBinding.Mode = BindingMode.OneWay;
                    ScoreBinding.Source = tetris.t;
                    Points.SetBinding(Label.ContentProperty, ScoreBinding);

                    Binding LevelBinding = new Binding("Level");
                    LevelBinding.Mode = BindingMode.OneWay;
                    LevelBinding.Source = tetris.t;
                    Level.SetBinding(Label.ContentProperty, LevelBinding);

                    ImageBar.Visibility = Visibility.Collapsed;
                    VideoBar.Visibility = Visibility.Collapsed;
                    DrawingBar.Visibility = Visibility.Collapsed;
                    EffectSelection.Visibility = Visibility.Collapsed;
                    Tetrisbar.Visibility = Visibility.Visible;
                   
                    monitor.Clear();
                    drawlayer.Clear();
                    break;
                case RunningTask.Webcam:
                    ImageBar.Visibility = Visibility.Collapsed;
                    VideoBar.Visibility = Visibility.Collapsed;
                    DrawingBar.Visibility = Visibility.Collapsed;
                    EffectSelection.Visibility = Visibility.Collapsed;
                    Tetrisbar.Visibility = Visibility.Collapsed;
                    monitorImg.Source = monitor;
                    monitor.Clear();
                    drawlayer.Clear();
                    break;
                default:
                    break;
            }


        }
        private void Configure_Mouse_Down(object sender, RoutedEventArgs e)
        {

            if (SelectedWindow != null)
                switch (SelectedWindow.ToString())
                {

                    case "RGB_Window.Windows.ExpandingObjects_Options":
                        expandingObjectsWindow.Show();
                        break;

                    case "RGB_Window.Windows.MovingText_Options":
                        movingTextOptionWindow.Show();
                        break;
                    case "RGB_Window.Windows.GradientColor_Options":
                        gradientOptionWindow.Show();
                        break;
                    case "RGB_Window.Windows.Plasma_Options":
                        plasmaOptionWindow.Show();
                        break;

                }
        }
        private void HideOptionWindow()
        {
            if (SelectedWindow != null)
                switch (SelectedWindow.ToString())
                {

                    case "RGB_Window.Windows.ExpandingObjects_Options":
                        expandingObjectsWindow.Hide();
                        break;

                    case "RGB_Window.Windows.MovingText_Options":
                        movingTextOptionWindow.Hide();
                        break;
                    case "RGB_Window.Windows.GradientColor_Options":
                        gradientOptionWindow.Hide();
                        break;
                    case "RGB_Window.Windows.Plasma_Options":
                        plasmaOptionWindow.Hide();
                        break;

                }
        }
        private void EffectSelectionDropDownClosed(object sender, EventArgs e)
        {
            HideOptionWindow();
            switch (LeftMonitorSelect.SelectionBoxItem.ToString())
            {
                case "ExpandingObjects":
                    ex_eff = expandingObjects.ExpandingObjects_execute;
                   
                    SelectedWindow = expandingObjectsWindow;
                    expandingObjectsWindow.Show();
                    break;
                case "MovingBalls":
                    exp = new CollisonBalls(monitor);
                    ex_eff = exp.CollisonBalls_execute;
                   
                    break;
                case "ScrollingText":
                    movingText.ColorPalette = Palettes.BlueGreen;
                    ex_eff = movingText.MovingText_execute;
                    SelectedWindow = movingTextOptionWindow;
                    movingTextOptionWindow.Show();
                    break;
                case "Plasma":
                    plasmaOptionWindow.Show();
                    SelectedWindow = plasmaOptionWindow;
                    ex_eff = plasma.Plasma_execute;
                    
                    break;
                case "GradientColor":
                    gradientOptionWindow.Show();
                    SelectedWindow = gradientOptionWindow;
                    colorGradient.GradientMode = Gradient.Radial;
                    ex_eff = colorGradient.ColorGradient_execute;
                    
                    break;
                case "None":
                    monitor.Clear();
                    ex_eff = null;
                   
                    SelectedWindow = null;
                    break;
            }
        }
        private void DrawingLabels_MouseDown(object sender, EventArgs e)
        {
            FrameworkElement f = sender as FrameworkElement;
            switch (f.Name)
            {
                case "PlayVideo":
                    if (video != null)
                    {
                        video.Path = VideoPath.Text;
                        video.play();
                    }
                    break;
                case "LoadVideo":
                    if (video != null)
                        video.path();
                    VideoPath.Text = video.Path;
                    break;
                case "LoadImage":
                    if (ip != null)
                    {
                        ip.path();
                        ImagePath.Text = ip.Path;
                    }
                    break;
                case "Rectangle":
                    draw.setDrawtype = Drawtype.rectangle;
                    break;
                case "Line":
                    draw.setDrawtype = Drawtype.line;
                    break;
                case "Point":
                    draw.setDrawtype = Drawtype.point;
                    break;
                case "Ellipse":
                    draw.setDrawtype = Drawtype.circle;
                    break;
                case "FilledEllipse":
                    draw.setDrawtype = Drawtype.fillcircle;
                    break;
                case "FilledRectangle":
                    draw.setDrawtype = Drawtype.fillrectangle;
                    break;

            }
        }
        private void Draw_SetOrgin(object sender, RoutedEventArgs e)
        {

            if (draw != null)
            {

                Point mouse = new Point();
                mouse.X = 68 * Mouse.GetPosition(CentralMonitor).X / CentralMonitor.ActualWidth;
                mouse.Y = 42 * Mouse.GetPosition(CentralMonitor).Y / CentralMonitor.ActualHeight;
                draw.setOrigin = mouse;
                zw = mouse;

            }
        }
        private void BlitWithLayer(object sender, RoutedEventArgs e)
        {
            if (CentralMonitor.IsEnabled)
            {
                monitorImg.Source = monitor;
               
                monitor.Blit(new Rect(new Size(68, 42)), drawlayer, new Rect(new Size(68, 42)));
                drawlayer.Blit(new Rect(new Size(68, 42)), monitor, new Rect(new Size(68, 42)));
            }

        }
        private void DrawOnMonitor(object sender, RoutedEventArgs e)
        {
            monitorImg.Source = drawlayer;
            drawlayer.Blit(new Rect(new Size(68, 42)), monitor, new Rect(new Size(68, 42)));
         
            if (draw != null && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                Point mouse = new Point();
                mouse.X = 68 * Mouse.GetPosition(monitorImg).X / monitorImg.ActualWidth;
                mouse.Y = 42 * Mouse.GetPosition(monitorImg).Y / monitorImg.ActualHeight;

                if (zw != mouse)
                {
                    old = zw;
                    draw.setMousePos = old;
                    draw.del();

                    zw = mouse;

                }
                else
                {
                    zw = mouse;

                }
                draw.setMousePos = mouse;
                draw.draw();

            }
        }
        private void Draw_enableMonitor(object sender, MouseButtonEventArgs e)
        {

            if (drawAccept)
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
            filterOptionWindow = new FilterOptionWindow(this);
            filterOptionWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            try
            {
                var position = this.PointToScreen(new Point(0, 0));
                filterOptionWindow.Left = position.X + this.Width;
                filterOptionWindow.Top = position.Y;
                filterOptionWindow.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void AuroraWindow_StateChanged(object sender, EventArgs e)
        {
            if (filterOptionWindow != null)
            {
                if (this.WindowState == WindowState.Minimized)
                {

                    filterOptionWindow.WindowState = WindowState.Minimized;
                }
                if (this.WindowState == WindowState.Normal)
                    filterOptionWindow.WindowState = WindowState.Normal;
            }

        }

    }
}
