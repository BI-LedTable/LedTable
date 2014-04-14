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
using RGB_Libary;
using MahApps.Metro.Controls;

namespace RGB_Window.Windows
{
    /// <summary>
    /// Interaktionslogik für ExpandingObjects_Options.xaml
    /// </summary>
    public partial class ExpandingObjects_Options : MetroWindow
    {
        private ExpandingObjects e_o;
        public ExpandingObjects_Options(ExpandingObjects e_o)
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
      
        public void Drag_Event(object sender, RoutedEventArgs e)
        {
            DragMove();
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
