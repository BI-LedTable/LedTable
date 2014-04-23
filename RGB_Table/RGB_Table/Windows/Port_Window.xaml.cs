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
using RgbLibrary;
using MahApps.Metro.Controls;


namespace Aurora.Windows
{
    /// <summary>
    /// PortWindow is the userinterface window to connect with the LedTable.
    /// The user selects the ComPort, where the data are send out and confirms his selection.
    /// </summary>
    public partial class PortWindow : MetroWindow
    {


        private SerialPort serial;
        private string[] ports;
        private string Port;
        private bool connected = false;
        public DataOut data;


        public PortWindow()
        {
            InitializeComponent();
            init_Serial_Com();
        }

        private void init_Serial_Com()
        {
            serial = new SerialPort();
            data = new DataOut();
            ports = SerialPort.GetPortNames();
            Comm_Port_Names.DataContext = ports;
            tb_info.Text = ports.Length + " " + "Com Ports found";

            if (ports.Length == 0)
            {
                Comm_Port_Names.IsEnabled = false;

            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Control_Label_MouseDown(object sender, RoutedEventArgs e)
        {
            FrameworkElement feSource = e.Source as FrameworkElement;
            switch (feSource.Name)
            {

                case "Connect":
                    data.PortName = Comm_Port_Names.SelectionBoxItem.ToString();
                    Connect.IsEnabled = false;
                    data.Connected = true;
                    break;
                case "Disconnect":
                    data.Connected = false;
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
