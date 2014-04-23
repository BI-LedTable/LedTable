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
using Xceed;
using RgbLibrary;
using MahApps.Metro.Controls;

namespace Aurora.Windows
{
    /// <summary>
    /// Interaktionslogik für ExpandingObjects_Options.xaml
    /// </summary>
    public partial class ExpandingObjectsOptionsWindow : MetroWindow
    {
        private ExpandingObjects e_o;
        public ExpandingObjectsOptionsWindow(ExpandingObjects e_o)
        {
       
            InitializeComponent();
            this.SizeToContent= System.Windows.SizeToContent.WidthAndHeight;
          
            this.e_o = e_o;
            this.Topmost = true;
            
            ColorPicker.UsingAlphaChannel = true;
            ColorPicker.DisplayColorAndName = true;
            ColorPicker.SelectedColor = Colors.White;
          
            Binding ColorBinding = new Binding("Color");
            ColorBinding.Source = this.e_o;
            ColorBinding.Mode = BindingMode.TwoWay;
            ColorPicker.SetBinding(Xceed.Wpf.Toolkit.ColorPicker.SelectedColorProperty, ColorBinding);
            
            Binding AmountBinding = new Binding("Amount");
            AmountBinding.Source = this.e_o;
            AmountBinding.Mode = BindingMode.TwoWay;  
            AmountSlider.SetBinding(Slider.ValueProperty, AmountBinding);

            Binding ObjectBinding = new Binding("Object");
            ObjectBinding.Source = this.e_o;
            ObjectBinding.Mode = BindingMode.TwoWay;
            Selected_Objects.SetBinding(ComboBox.SelectedValueProperty, ObjectBinding);
            
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
    }
}
