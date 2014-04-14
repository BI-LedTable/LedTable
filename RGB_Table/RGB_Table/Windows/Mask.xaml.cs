using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RGB_Libary;
using MahApps.Metro.Controls;
using StickyWindowLibrary;
using Blue.Windows;


namespace RGB_Window.Windows
{
    /// <summary>
    /// Interaktionslogik für Plasma_Window.xaml
    /// </summary>
    public partial class Mask_Options : MetroWindow
    {
        private StickyWindow stickyWindow;
        public Mask_Options(MainWindow mw)
        {
           
            InitializeComponent();
          
            ColorMask.Height = mw.Height;
            this.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
            this.Loaded += Mask_Options_Loaded;


            Binding BrightnessBinding = new Binding("Brightness");
            BrightnessBinding.Source = mw;
            BrightnessBinding.Mode = BindingMode.TwoWay;
            BrightnessSlider.SetBinding(Slider.ValueProperty, BrightnessBinding);

            Binding RedBinding = new Binding("RedMask");
            RedBinding.Source = mw;
            RedBinding.Mode = BindingMode.TwoWay;
            RedSlider.SetBinding(Slider.ValueProperty, RedBinding);

            Binding BlueBinding = new Binding("BlueMask");
            BlueBinding.Source = mw;
            BlueBinding.Mode = BindingMode.TwoWay;
            BlueSlider.SetBinding(Slider.ValueProperty, BlueBinding);

            Binding GreenBinding = new Binding("GreenMask");
            GreenBinding.Source = mw;
            GreenBinding.Mode = BindingMode.TwoWay;
            GreenSlider.SetBinding(Slider.ValueProperty, GreenBinding);


        }

        void Mask_Options_Loaded(object sender, RoutedEventArgs e)
        {
            stickyWindow = new StickyWindow(this);
            stickyWindow.StickToScreen = true;
            stickyWindow.StickToOther = true;
            stickyWindow.StickOnResize = true;
            stickyWindow.StickOnMove = true;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        public void Drag_Event(object sender, RoutedEventArgs e)
        {
            DragMove();
        }

        private void Expander_Expanded_1(object sender, RoutedEventArgs e)
        {
            this.SizeToContent = SizeToContent.WidthAndHeight;
        }
    }
    
}
