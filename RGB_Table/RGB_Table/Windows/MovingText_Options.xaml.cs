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
    /// MovingTextOptions is a window derived from MetroWindow.
    /// The user can change text settings according to the <see cref="RgbLibrary.MovingText"/> object
    /// </summary>
    public partial class MovingTextOptionsWindow : MetroWindow
    {
        private MovingText movingText;
        /// <summary>
        /// The MovingTextOptionsWindow nees a <see cref="RgbLibrary.MovingText"/> object.
        /// <para>
        /// <list type="bullet">
        /// <listheader>
        /// <description>The user can change the properties:</description>
        /// </listheader>
        /// <item>
        /// <description>
        /// Color
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Text
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// FontSize
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Position in x-achses
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Position in y-achses
        /// </description>
        /// <item>
        /// <description>
        /// Scroll Mode
        /// </description>
        /// </item>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="movingText"></param>
        public MovingTextOptionsWindow(MovingText movingText)
        {
            InitializeComponent();
            this.movingText = movingText;
            this.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
            this.Topmost = true;
            
            ColorPicker.UsingAlphaChannel = true;
            ColorPicker.DisplayColorAndName = true;
            ColorPicker.SelectedColor = Colors.White;
            
            Binding ColorBinding = new Binding("Color");
            ColorBinding.Source = this.movingText;
            ColorBinding.Mode = BindingMode.TwoWay;
            ColorPicker.SetBinding(Xceed.Wpf.Toolkit.ColorPicker.SelectedColorProperty, ColorBinding);

            Binding TextBinding = new Binding("Text");
            TextBinding.Source = this.movingText;
            TextBinding.Mode = BindingMode.TwoWay;
            TextBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            Text.SetBinding(TextBox.TextProperty, TextBinding);

            Binding FontSizeBinding = new Binding("FontSize");
            FontSizeBinding.Source = this.movingText;
            FontSizeBinding.Mode = BindingMode.TwoWay;
            FontSize.SetBinding(Slider.ValueProperty, FontSizeBinding);

            Binding ModeBinding = new Binding("Mode");
            ModeBinding.Source = this.movingText;
            ModeBinding.Mode = BindingMode.TwoWay;
            Selected_Objects.SetBinding(ComboBox.SelectedValueProperty, ModeBinding);

            Binding PosYBinding = new Binding("PosY");
            PosYBinding.Source = this.movingText;
            PosYBinding.Mode = BindingMode.TwoWay;
            PosY.SetBinding(Slider.ValueProperty, PosYBinding);

            Binding PosXBinding = new Binding("PosX");
            PosXBinding.Source = this.movingText;
            PosXBinding.Mode = BindingMode.TwoWay;
            PosX.SetBinding(Slider.ValueProperty, PosXBinding);


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
