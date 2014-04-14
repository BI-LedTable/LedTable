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
    /// Interaktionslogik für MovintText_Options.xaml
    /// </summary>
    public partial class MovingText_Options : MetroWindow
    {
        private MovingText m_t;
        public MovingText_Options(MovingText m_t)
        {
            InitializeComponent();
            this.m_t = m_t;
            this.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
            this.Topmost = true;
            ColorPicker.UsingAlphaChannel = true;
            ColorPicker.DisplayColorAndName = true;
            ColorPicker.SelectedColor = Colors.White;

            Binding ColorBinding = new Binding("Color");
            ColorBinding.Source = this.m_t;
            ColorBinding.Mode = BindingMode.TwoWay;
            ColorPicker.SetBinding(Xceed.Wpf.Toolkit.ColorPicker.SelectedColorProperty, ColorBinding);

            Binding TextBinding = new Binding("Text");
            TextBinding.Source = this.m_t;
            TextBinding.Mode = BindingMode.TwoWay;
            TextBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            Text.SetBinding(TextBox.TextProperty, TextBinding);

            Binding FontSizeBinding = new Binding("FontSize");
            FontSizeBinding.Source = this.m_t;
            FontSizeBinding.Mode = BindingMode.TwoWay;
            FontSize.SetBinding(Slider.ValueProperty, FontSizeBinding);

            Binding ModeBinding = new Binding("Mode");
            ModeBinding.Source = this.m_t;
            ModeBinding.Mode = BindingMode.TwoWay;
            Selected_Objects.SetBinding(ComboBox.SelectedValueProperty, ModeBinding);

            Binding PosYBinding = new Binding("PosY");
            PosYBinding.Source = this.m_t;
            PosYBinding.Mode = BindingMode.TwoWay;
            PosY.SetBinding(Slider.ValueProperty, PosYBinding);

            Binding PosXBinding = new Binding("PosX");
            PosXBinding.Source = this.m_t;
            PosXBinding.Mode = BindingMode.TwoWay;
            PosX.SetBinding(Slider.ValueProperty, PosXBinding);


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
    }
}
