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
    /// Interaktionslogik für GradientColor_Options.xaml
    /// </summary>
    public partial class GradientColorOptionsWindow : MetroWindow
    {
        private ColorGradient colorgradient;
        /// <summary>
        /// GradientColorOptionsWindow needs the <see cref="RgbLibrary.ColorGradient"/> object
        /// <list type="bullet">
        /// <listheader>
        /// <description>The user can change the properties:</description>
        /// </listheader>
        /// <item>
        /// <description>GradientMode</description>
        /// </item>
        /// <item>
        /// <description>ColorPalette</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="colorgradient"></param>
        public GradientColorOptionsWindow(ColorGradient colorgradient)
        {
            InitializeComponent();
            this.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
            this.colorgradient = colorgradient;
            
            Binding ObjectBinding = new Binding("GradientMode");
            ObjectBinding.Source = this.colorgradient;
            ObjectBinding.Mode = BindingMode.TwoWay;
            Selected_Objects.SetBinding(ComboBox.TextProperty, ObjectBinding);

            Binding ColorPaletteBinding = new Binding("ColorPalette");
            ColorPaletteBinding.Source = this.colorgradient;
            ColorPaletteBinding.Mode = BindingMode.TwoWay;
            Palettes.SetBinding(ComboBox.TextProperty, ColorPaletteBinding);
            
        }
        
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        
    }
}
