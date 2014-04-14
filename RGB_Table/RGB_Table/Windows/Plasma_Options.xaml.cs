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

namespace RGB_Window.Windows
{
    /// <summary>
    /// Interaktionslogik für Plasma_Window.xaml
    /// </summary>
    public partial class Plasma_Options : MetroWindow
    {
        private Plasma p;
        public Plasma_Options(Plasma p)
        {
            InitializeComponent();
            this.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
            this.p = p;
           // this.cg = cg;
            //Binding ObjectBinding = new Binding("GradientMode");
            //ObjectBinding.Source = this.cg;
            //ObjectBinding.Mode = BindingMode.TwoWay;
            //Selected_Objects.SetBinding(ComboBox.TextProperty, ObjectBinding);

            Binding ColorPaletteBinding = new Binding("ColorPalette");
            ColorPaletteBinding.Source = this.p;
            ColorPaletteBinding.Mode = BindingMode.TwoWay;
            Palettes.SetBinding(ComboBox.TextProperty, ColorPaletteBinding);
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
