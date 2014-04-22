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

namespace Aurora.Windows
{
    /// <summary>
    /// Interaktionslogik für Plasma_Window.xaml
    /// </summary>
    public partial class PlasmaOptionsWindow : MetroWindow
    {
        private Plasma p;
        public PlasmaOptionsWindow(Plasma p)
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
