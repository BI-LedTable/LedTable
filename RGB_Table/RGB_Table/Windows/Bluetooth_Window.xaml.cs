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
using System.Threading;
using MahApps.Metro.Controls;
using System.Diagnostics;

namespace Aurora.Windows
{
    /// <summary>
    /// Interaktionslogik für Bluetooth_Window.xaml
    /// </summary>
    public partial class BluetoothWindow : MetroWindow
    {
        Bluetooth bluetooth = Bluetooth.Instance;


        List<String> found_devices;
        List<String> known_devices;

        public BluetoothWindow(MainWindow mw)
        {
            InitializeComponent();
            bluetooth = Bluetooth.Instance;

            //Besteht eine Verbindung, so wird die Schaltfläche auf "Trennen" gesetzt.
            if (bluetooth.Connected == true)
            {
                Connect.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (ThreadStart)delegate
                {
                    Connect.Content = "Trennen";
                });
            }
            //Zeigt das aktuell verbundene Gerät an, wenn bereits eine Verbindung besteht
            if (bluetooth.Device != null)
            {
                Devices.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (ThreadStart)delegate
                {

                    for (int i = 0; i < bluetooth.device_list.Count; i++)
                    {
                        Devices.Items.Add(bluetooth.device_list[i]);
                        if (bluetooth.device_list[i].ToString() == bluetooth.Device)
                        {
                            Devices.SelectedItem = Devices.Items[i];
                            Devices.Text = Devices.Items[i].ToString();
                        }

                    }
                });
            }


            found_devices = new List<string>();
            known_devices = new List<string>();

            bluetooth.DeviceListChange += new Bluetooth.PropertyChangeHandler(scan_finished);
            bluetooth.ConnectionChange += new Bluetooth.PropertyChangeHandler(connection_changed);
            //bluetooth.PropertyChange += new Bluetooth.PropertyChangeHandler(read_data);  --> hier unnötig
            // bluetooth.CommandControlChange += new Bluetooth.PropertyChangeHandler(commca);

        }





        //void commca(object sender, PropertyChangeArgs data)
        //{
        //    string command = data.mesg;
        //}

        void scan_finished(object sender, PropertyChangeArgs data)
        {
            found_devices = data.liste;

            //Clear Devices
            Devices.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (ThreadStart)delegate
            {
                Devices.Items.Clear();
            });

            //Found Devices
            foreach (String str in found_devices)
            {

                Devices.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (ThreadStart)delegate
                {
                    Devices.Items.Add(str);
                    Devices.SelectedItem = Devices.Items[0];


                });
            }
            ProgressRing.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (ThreadStart)delegate
            {
                ProgressRing.IsActive = false;
                ProgressRing.Visibility = Visibility.Collapsed;
            });


            //Known Devices

            List<String> known_dev = bluetooth.get_known_devices();

            foreach (String str in known_dev)
            {
                Devices.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (ThreadStart)delegate
                {
                    Devices.Items.Add(str);
                    Devices.SelectedItem = Devices.Items[0];


                });

            }


        }
        void connection_changed(object sender, PropertyChangeArgs data)
        {
            if (bluetooth.Connected == true)
            {
                // Verbunden Status - evtl das Event in das Haupfenster einbinden für ein Symbol??
                Debug.WriteLine("Verbunden!");
                //Connection Detective aktivieren
                //bluetooth.connection_detective(true);
                Connect.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (ThreadStart)delegate
                {
                    Connect.Content = "Trennen";
                    Thread.Sleep(300);
                    Close();
                });
            }
            else
            {
                //Gerät wurde getrennt -> Meldung ausgeben in allen Fenstern???
                Debug.WriteLine("Verbindung getrennt!");
                //bluetooth.connection_detective(false, 4000);
                Connect.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (ThreadStart)delegate
                {
                    Connect.Content = "Verbinden";
                });
            }
        }



        private void Control_MouseDown(object sender, EventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            switch (fe.Name)
            {
                case "Scan":
                    bluetooth.scan_discoverable();
                    ProgressRing.Visibility = Visibility.Visible;
                    ProgressRing.IsActive = true;

                    break;
                case "Connect":
                    {
                        if (bluetooth.Connected == false)
                        {
                            bluetooth.connect_to_be(Devices.SelectedItem.ToString());
                        }
                        else
                        {
                            bluetooth.disconnect_from_stream();
                        }


                        break;
                    }
            }
        }

        private void StackPanel_Loaded_1(object sender, RoutedEventArgs e)
        {
            List<String> known_dev = bluetooth.get_known_devices();

            if (bluetooth.Device == null) //-> noch kein Gerät verbunden
            {
                foreach (String str in known_dev)
                {
                    Devices.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (ThreadStart)delegate
                    {
                        Devices.Items.Add(str);
                        Devices.SelectedItem = Devices.Items[0];
                    });
                }
            }
        }

        private void onClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Veraltet! - > kein trennen beim schließen des Fensters!!
            //if (bluetooth.Connected == true)
            //{
            //    bluetooth.disconnect_from_stream();
            //}
        }
    }
}
