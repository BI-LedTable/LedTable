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
using RgbLibrary;
using MahApps.Metro.Controls;
using StickyWindowLibrary;
using Blue.Windows;


namespace Aurora.Windows
{
    /// <summary>
    /// Interaktionslogik für Plasma_Window.xaml
    /// </summary>
    public partial class FilterOptionWindow : MetroWindow
    {
        private StickyWindow stickyWindow;
        /// <summary>
        /// The FilterOptionWindow needs a <see cref="Aurora.MainWindow"/> object
        /// <para>
        /// <list type="bullet">
        /// <listheader>
        /// <description>
        /// The user can change the properties:
        /// </description>
        /// </listheader>
        /// <item>
        /// <description>Brigthness</description>
        /// </item>
        /// <item>
        /// <description>Red component</description>
        /// </item>
        /// <item>
        /// <description>Blue component</description>
        /// </item>
        /// <item>
        /// <description>Green component</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="mainWindow"></param>
        public FilterOptionWindow(MainWindow mainWindow)
        {
           
            InitializeComponent();
          
            ColorMask.Height = mainWindow.Height;
            this.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
            this.Loaded += Mask_Options_Loaded;


            Binding BrightnessBinding = new Binding("Brightness");
            BrightnessBinding.Source = mainWindow;
            BrightnessBinding.Mode = BindingMode.TwoWay;
            BrightnessSlider.SetBinding(Slider.ValueProperty, BrightnessBinding);

            Binding RedBinding = new Binding("RedMask");
            RedBinding.Source = mainWindow;
            RedBinding.Mode = BindingMode.TwoWay;
            RedSlider.SetBinding(Slider.ValueProperty, RedBinding);

            Binding BlueBinding = new Binding("BlueMask");
            BlueBinding.Source = mainWindow;
            BlueBinding.Mode = BindingMode.TwoWay;
            BlueSlider.SetBinding(Slider.ValueProperty, BlueBinding);

            Binding GreenBinding = new Binding("GreenMask");
            GreenBinding.Source = mainWindow;
            GreenBinding.Mode = BindingMode.TwoWay;
            GreenSlider.SetBinding(Slider.ValueProperty, GreenBinding);


        }
        /// <summary>
        /// A new StickyWindow is initalised, which says the filter window
        /// should stick to the application main window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mask_Options_Loaded(object sender, RoutedEventArgs e)
        {
            stickyWindow = new StickyWindow(this);
            stickyWindow.StickToScreen = false;
            stickyWindow.StickToOther = true;
            stickyWindow.StickOnResize = true;
            stickyWindow.StickOnMove = true;
        }

        /// <summary>
        /// The OnClosing events for the OptionWindow are override.
        /// They should stay in background, in stead of to be close.
        /// Otherwise a new instance of the object has to be created,
        /// which means that the settings, the user has changed would have been lost
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        private void Expander_Expanded_1(object sender, RoutedEventArgs e)
        {
            this.SizeToContent = SizeToContent.WidthAndHeight;
        }
    }
    
}
