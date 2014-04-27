using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InTheHand;
using InTheHand.Net;
using InTheHand.Net.Ports;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;

using System.IO;
using System.Threading;

using System.Diagnostics;
using System.Windows.Controls;


namespace RgbLibrary
{


    public class PropertyChangeArgs : EventArgs
    {
        //Für Strings
        public String mesg { get; set; }
        public PropertyChangeArgs(String mesg)
        {
            this.mesg = mesg;
        }

        //Für String Listen
        public List<String> liste { get; set; }
        public PropertyChangeArgs(List<String> liste)
        {
            this.liste = liste;
        }

        //Für Bool connected
        public bool is_connected { get; set; }
        public PropertyChangeArgs(bool is_connected)
        {
            this.is_connected = is_connected;
        }

        public byte[] ev_buffer { get; set; }
        public PropertyChangeArgs(byte[] ev_buffer)
        {
            this.ev_buffer = ev_buffer;
        }
    }


    /// <summary>
    /// Diese Klasse wird verwendet, um die Kommunikation zwischen Android Devices und PC zu vereinfachen.
    /// Die Klasse baut auf den Bibliotheken von InTheHand auf.
    /// Diese sind ebenfalls in das Projekt einzubinden!
    /// </summary>
    public class Bluetooth
    {

        private static Bluetooth instance;

        public static Bluetooth Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Bluetooth();
                }
                return instance;
            }
        }

        private Bluetooth()
        {
            Connected = false;
        }




        public delegate void PropertyChangeHandler(object sender, PropertyChangeArgs data);
        // The event
        public event PropertyChangeHandler PropertyChange;


        protected void OnPropertyChange(object sender, PropertyChangeArgs data)
        {
            // Check if there are any Subscribers
            if (PropertyChange != null)
            {
                // Call the Event
                PropertyChange(this, data);
            }
        }



        public event PropertyChangeHandler ConnectionChange;

        protected void OnConnectedChange(object sender, PropertyChangeArgs conn)
        {
            // Check if there are any Subscribers
            if (ConnectionChange != null)
            {
                // Call the Event
                ConnectionChange(this, conn);
            }
        }




        public event PropertyChangeHandler DeviceListChange;
        protected void OnDeviceListChange(object sender, PropertyChangeArgs devl)
        {
            // Check if there are any Subscribers
            if (DeviceListChange != null)
            {
                // Call the Event
                DeviceListChange(this, devl);

            }
        }



        public event PropertyChangeHandler CommandControlChange;
        protected void OnCommandControlChange(object sender, PropertyChangeArgs cc)
        {

            if (CommandControlChange != null)
            {
                CommandControlChange(this, cc);
            }
        }

        public event PropertyChangeHandler PhotoDataChange;
        protected void OnPhotoDataChange(object sender, PropertyChangeArgs ph_dat)
        {
            if (PhotoDataChange != null)
            {
                PhotoDataChange(this, ph_dat);
            }
        }





        private List<String> devices = new List<String>();

        //<summary>
        //Die Liste aller Geräte die ermittelt wurden.
        //Alle gefundenen, bekannten oder authentifizierten Geräte werden hier gespeichert.
        //</summary>
        public List<String> device_list
        {
            get
            {
                return devices;
            }
            set
            {
                devices = value;
                DeviceListChange(this, new PropertyChangeArgs(devices));

            }
        }



        private string recieved_txt;

        /// <summary>
        /// Erhaltene Daten aus dem stream
        /// </summary>
        public string Recieved
        {
            get
            {
                return recieved_txt;
            }
            set
            {
                if (value != recieved_txt)
                {
                    recieved_txt = value;
                    OnPropertyChange(this, new PropertyChangeArgs(recieved_txt));
                    data_interpret(recieved_txt);  // - > Daten werden interpretiert.
                }
            }
        }




        private bool connected;


        public bool Connected
        {
            get
            {
                return connected;
            }
            set
            {
                connected = value;
                OnConnectedChange(this, new PropertyChangeArgs(connected));

            }
        }


        private String control_command;

        public String Control_Command
        {
            get
            {
                return control_command;
            }
            set
            {
                control_command = value;
                OnCommandControlChange(this, new PropertyChangeArgs(control_command));
            }
        }

        private byte[] photo_buffer;

        public byte[] Photo_buffer
        {
            get
            {
                return photo_buffer;
            }
            set
            {
                photo_buffer = value;
                OnPhotoDataChange(this, new PropertyChangeArgs(photo_buffer));
            }
        }

        //Wird verwendet, um das verwendete Device zwischenzuspeichern, um dieses auch wieder in der Auswahl anzuzeigen.
        private String device;
        public String Device
        {
            get
            {
                return device;
            }
            set
            {
                device = value;
            }
        }




        int Mode = 0;  //Modus beim Lesen




        /// <summary>
        /// Die Fehlermeldung, welche beim Verbindungsaufbau auftritt. Zu Debugging Zwecken.
        /// </summary>
        public string exception_connect;



        /// <summary>
        /// Der Text der Exception, die beim Lesen auftreten kann. Nur zu Debugging Zwecken.
        /// </summary>
        public string exception_read = "";






        private BluetoothDeviceInfo[] device_inf_list;     //Globale DeviceInfo Liste

        private static readonly Guid BluetoothUUID = Guid.Parse("fa87c0d0-afac-11de-8a39-0800200c9a66");  //ID welche von Client & Server verwendet werden muss


        private Stream stream;                          //Der Stream, welcher zum Datensenden verwendet wird
        private BluetoothAddress add;                   //Die einzigartige Bluetooth Addresse des Device

        private BluetoothClient client = new BluetoothClient(); //Ein neuer Bluetooth Client wird angelegt



        /// <summary>
        /// Startet einen Thread, welcher asynchron nach erreichbaren geräten scannt.
        /// Sollte ihr Gerät nicht in der Liste sein, gehen sie sicher, dass Bluetooth aktiviert ist.
        /// </summary>
        public void scan_discoverable()
        {

            Thread scan_disc = new Thread(scan_for_devices);
            scan_disc.Name = "ScanDiscoverable";
            scan_disc.IsBackground = true;
            scan_disc.Start();
        }



        /// <summary>
        /// Scannt nach erreich- und auffindbaren Geräten. Gehen sie sicher dass ihr Gerät auch Sichtbar ist!
        /// </summary>
        /// <returns>Eine Liste mit den Namen der gefundenen Geräten. (String List)</returns>
        private void scan_for_devices()
        {
            device_inf_list = null;
            device_inf_list = client.DiscoverDevicesInRange();

            devices.Clear();

            foreach (BluetoothDeviceInfo dI in device_inf_list)
            {
                devices.Add(dI.DeviceName);

            }

            device_list = devices.ToList<string>();
        }




        /// <summary>
        /// Liefert alle bereits gepairten, dem Computer bekannten Geräte zurück.
        /// </summary>
        /// <returns>List String Known Devices></returns>
        public List<string> get_known_devices()
        {
            devices = new List<String>();

            int maxDevices = 100;
            bool authenticated = true;
            bool remembered = true;
            bool unknown = false;                   //Default: true -> in diesem Fall werden aber nur die Bekannten (=Known) Geräte benötigt
            bool discoverableOnly = false;

            //Erstelle eine Liste aller bekannten Devices  (Typ: Bluetooth.DeviceInfo)
            device_inf_list = client.DiscoverDevices(maxDevices, authenticated, remembered, unknown, discoverableOnly);

            foreach (BluetoothDeviceInfo dI in device_inf_list)
            {
                devices.Add(dI.DeviceName);
            }

            return devices;
        }







        /// <summary>
        /// Liefert die einzigartige Hardware Addresse eines Geräts.
        /// </summary>
        /// <param name="name_of_device">Der Name des gesuchten Geräts.</param>
        /// <returns>Liefert eine Addresse vom Typ BluetoothAddress(InTheHand.Net) zurück.</returns>
        public BluetoothAddress get_device_address(string name_of_device)
        {
            BluetoothAddress blu_addr = null;

            device = name_of_device;

            foreach (BluetoothDeviceInfo d_inf in device_inf_list)
            {
                if (d_inf.DeviceName == name_of_device)
                {
                    blu_addr = d_inf.DeviceAddress;
                }
            }

            return blu_addr;
        }



        /// <summary>
        /// Asynchrones Verbinden zu dem Client.
        /// </summary>
        /// <param name="dev_name">Der Name des Geräts.</param>
        public void connect_to_be(string dev_name)
        {

            add = get_device_address(dev_name);

            BluetoothEndPoint BEP = new BluetoothEndPoint(add, BluetoothUUID);
            client = new BluetoothClient(); //Versuchsweise eingefügt

            object connection = false;
            try
            {
                client.BeginConnect(BEP, new AsyncCallback(start_read), client);
            }
            catch (Exception excep)
            {
                Debug.WriteLine(excep.ToString());
            }


        }

        private void start_read(IAsyncResult ar)
        {
            BluetoothClient client = (BluetoothClient)ar.AsyncState;

            try
            {
                client.EndConnect(ar);
            }
            catch (Exception exc)
            {
                exception_connect = "" + exc;
                Connected = false;

                if (exc.HResult == -2147467259) //Definierter Fehlercode für diese Fehlermeldung  - Verbindung konnte nicht zustande kommen
                {
                    Debug.WriteLine("Verbindung wurde vom Remotehost beendet!");
                }
                return;
            }

            Connected = true;

            stream = client.GetStream();

            Thread th_read = new Thread(delegate() { read_data(); });
            th_read.SetApartmentState(ApartmentState.STA);
            th_read.Name = "Read_Data";
            th_read.IsBackground = true;
            th_read.Start();

        }







        /// <summary>
        /// Schreibt die übergebenen Daten in den Stream.
        /// </summary>
        /// <param name="message">Die Daten als String.</param>
        public void send_data(String message)
        {
            byte[] mess = Encoding.ASCII.GetBytes(message);  //Convert String to Byte Array


            if (stream != null)
            {
                try
                {
                    stream.Write(mess, 0, mess.Length);
                }
                catch (Exception)
                {
                    connected = false;
                }

            }
            else
            {
                connected = false;
            }
        }



        /// <summary>
        /// Schreibt die übergebenen Daten in den Stream.
        /// </summary>
        /// <param name="byte_message">Die Daten als Byte Array.</param>
        public void send_data(byte[] byte_message)
        {
            if (Connected == true)
            {
                try
                {
                    stream.Write(byte_message, 9, byte_message.Length);
                }
                catch (Exception)
                {
                    connected = false;
                }
            }
        }




        string temp_rec = ""; //Temporare Erhaltenen Daten aus dem Stream - für Vergleichszwecke angelegt

        /// <summary>
        /// Wenn der Stream Daten erhält, werden die Daten von dieser Funktion ausgelesen.
        /// </summary>
        private void read_data()
        {

            byte[] received_bytes = new byte[68 * 42 * 3];
            while (stream != null)  //Sollte der Stream abbrechen (Die Verbindung abbrechen), so soll das Lesen beendet werden
            {
                try
                {
                  

                    if (Connected == true)
                    {
                        try
                        {
                            
                            int length = stream.Read(received_bytes, 0, received_bytes.Length);
                            stream.Flush();// Bedenken von F. Wenigwieser bzgl. der Flush Funktion und dem Leeren von Daten
                            
                            //if (length > 15)
                            //{
                            //}
                            Photo_buffer = new byte[length];
                            
                            try
                            {
                                for (int u = 0; u < length; u++)
                                {
                                    Photo_buffer[u] = received_bytes[u];
                                }
                            }
                            catch
                            {

                            }
                            
                        }
                        catch (Exception e)
                        {
                            exception_read = e.ToString();
                        }
                        //stream.Flush(); // Stream leeren??  - Versuchsweise


                        temp_rec = Encoding.ASCII.GetString(received_bytes);


                    }


                    try
                    {
                        data_interpret(temp_rec);
                       
                    }
                    catch (Exception interpret_exception)
                    {
                        Debug.WriteLine("Interpretation Exception: " + interpret_exception);
                    }
                }

                catch (Exception exception) //Fehler beim Lesen
                {
                    connected = false;
                    exception_read = exception.ToString();
                    try
                    {
                        Thread.CurrentThread.Abort();
                    }
                    catch (Exception excep)
                    {
                        exception_read = excep.ToString();  //Bisher wird noch eine unerwartete Thread.Abort Exception geworfen.
                        Debug.WriteLine("Thread.Abort - Exception: " + excep);
                    }
                    Debug.WriteLine("Read Exception:" + exception);
                }

            }

        }






        public void data_interpret(string recieved_data)
        {
            string str_data = "";

            str_data = recieved_data.Split('\0')[0];  //Leere Nullen aus dem Stream werden abgeschnitten/ignoriert


            if (str_data == "Paint")
            {
                Mode = 1;
            }
            if (str_data == "Tetris")
            {
                Mode = 2;
            }
            switch (Mode)
            {

                case 1:  //-> Nur bei Paint wichtig um keine doppelten Commands zu bekommen, bei Tetris sind doppelte COmmands wichtig!!
                    {
                        if (str_data != Recieved)
                        {
                            Recieved = str_data;    //Dient zur Vermeidung von oftmaliegen aufrufen des Events durch schnelles Lesen aus dem Stream
                            Control_Command = str_data;
                        }
                        break;
                    }
                case 2:
                    {
                        Recieved = str_data;
                        Control_Command = str_data;
                        break;
                    }
            }

        }



        /////Connection Detective
        bool conn_dev_enabled = false;

        /// <summary>
        /// Diese Funktion ermöglicht es, Verbindungsunterbrechungen zyklisch abzufragen.
        /// Sollte das Gerät aus der Reichweite gelangen, so kann die Verbindung abbrechen und zu Fehlern führen.
        /// </summary>
        /// <param name="run_detective">Zum aktivieren und deaktivieren des Detectives.</param>
        public bool connection_detective(bool run_detective, int wait_time)
        {

            conn_dev_enabled = run_detective;


            if (conn_dev_enabled == true)
            {
                if (connected == true) //Nur starten wenn auch verbunden
                {
                    Thread th_detective = new Thread(delegate() { conn_dev(wait_time); });

                    th_detective.Name = "Connection Detective";
                    th_detective.IsBackground = true;
                    th_detective.Start();
                    Debug.WriteLine("Detective started!");

                    return true;  //Detective erfolgreich gestartet
                }
                return false; //Keine Verbindung - Detective nkann nicht gestartet werden
            }
            return true; //Detective sollte geschlossen werden


        }

        private void conn_dev(int wait)
        {
            //Notiz -  der Client.Connected Status wird nur anhand der letzten Operation festgestellt.
            //darum muss eine neue Operation versucht werden um die Connected Variable neu zu setzen.
            while (conn_dev_enabled != false)
            {
                if (Connected == true)
                {
                    try
                    {
                        byte[] mess = Encoding.ASCII.GetBytes("test");  //test string
                        stream.Write(mess, 0, mess.Length);

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Connection Detective Exception: \n" + ex);
                        Connected = false;
                    }
                }

                Thread.Sleep(wait);
            }

            //End Detective (Verbindung wurde unterbrochen)
            if (conn_dev_enabled == false)
            {
                Thread.CurrentThread.Abort();
                Debug.WriteLine("Detective beendet!");
            }
        }



        /// <summary>
        /// Schließt den Stream und gibt alle Daten wieder frei.
        /// </summary>
        /// <param name="stream">Der zu schließende Stream.</param>
        public void disconnect_from_stream()
        {
            //stream.Dispose();

            try
            {
                conn_dev_enabled = false; // eventuell durch ein Event ersetzen welches den Thread abbricht.
                client.Close();
                stream.Close();

                stream = null;

                Connected = false;
            }
            catch (Exception)
            {
                //Kein Stream

            }

        }



    }
}
