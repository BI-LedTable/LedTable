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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Threading;
using System.Windows.Threading;
using RGB_Libary;
using MahApps.Metro.Controls;


namespace RGB_Window.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Port_Window : MetroWindow
    {
        #region variables

        //Serial 
        private SerialPort serial = new SerialPort();
        private string[] ports;
        private string Port;
        private bool connected = false;
        #endregion
        public Data_Out data;

        public Port_Window()
        {
            InitializeComponent();
            init_Serial_Com();

        }

        private void init_Serial_Com()
        {
            data = new Data_Out();
            ports = SerialPort.GetPortNames();
            Comm_Port_Names.DataContext = ports;
            tb_info.Text = ports.Length + " " + "Com Ports found";

            if (ports.Length == 0)
            {
                Comm_Port_Names.IsEnabled = false;

            }

        }


        public void Control_Label_MouseDown(object sender, RoutedEventArgs e)
        {
            FrameworkElement feSource = e.Source as FrameworkElement;
            switch (feSource.Name)
            {
              
                case "Connect":
                        data.set_PortName = Comm_Port_Names.SelectionBoxItem.ToString();
                        Connect.IsEnabled = false;
                    break;
                case "Disconnect":
                    data.Connected = true;
                    break;
            }

        }


        public bool Connection_Status
        {
            get { return connected; }
        }

        private void MetroWindow_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            
        }
    }

}
