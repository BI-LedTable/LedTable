using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Security;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;


namespace RgbLibrary
{
    public delegate void ChangedEventhandler(object sender,BoolArgs a);
    public class BoolArgs : EventArgs 
    {
        private bool status;
        public BoolArgs(bool status) 
        {
            this.status = status;
        }
    }
    /// <summary>
    /// DataOut 
    /// </summary>
    public class DataOut 
    {
       
        private string SerialPortName;
        public SerialPort serial;
        private Thread T_DataOut;
        private byte[] transferBytes;
        byte[] rgb;
        private bool connected;
        
        public event ChangedEventhandler Changed;
       
        public bool Connected
        {
            get { return connected; }
            set 
            {
                    connected = value;
                    OnChanged(new BoolArgs(value));
            }
        }
        /// <summary>
        /// If the PortName of the DataOut object
        /// gets set the DataOut object gets intitialized
        /// </summary>
        public string PortName
        {
            set { 
                    SerialPortName = value;
                    if(SerialPortName != "")
                        init_DataOut();
                }
        }
        /// <summary>
        /// The TransferBytes are sent over the serial ComPort
        /// and are updatet periodically in <see cref="Aurora.MainWindow.monitorTimer_Tick"/>
        /// </summary>
        public byte[] TransferBytes
        {
            set
            {
                    transferBytes = value;
            }
        }
        private void init_DataOut()
        {
            try
            {
                    //initates Serial Propertys
                    serial = new SerialPort();
                    rgb = new byte[(11424 / 4) * 3];
                    serial.PortName = SerialPortName;
                    serial.Handshake = Handshake.None;
                   
                    T_DataOut = new Thread(SendData);
                    T_DataOut.IsBackground = true;
                    T_DataOut.Start();
                    Connected = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public bool AllowUpdate { get; set; }
        public void SendData()
        {
            try
            {
                lock (this)
                {
                    for (; ; )
                    {
                        if (!serial.IsOpen)
                        {
                            serial.Open();
                            serial.BaseStream.WriteTimeout = SerialPort.InfiniteTimeout;
                        }
                        else if (transferBytes != null)
                        {
                            convertArgbToRgb();
                            serial.BaseStream.WriteByte(1);
                            Thread.Sleep(10);
                            serial.BaseStream.Write(rgb, 0, rgb.Length);
                        }
                    }
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }
        /// <summary>
        /// The converting step is necessary,because the led stripes need rgb values
        /// the writeablebitmap<see cref="Aurora.MainWindow.monitor"/> though has 
        /// a alpha channel
        /// </summary>
        private void convertArgbToRgb() 
        {
            int c = 0;
            rgb = new byte[(11424 / 4) * 3];
            for (int i = 0; i < transferBytes.Length; i++)
            {
                if ((i % 4) == 0)
                {
                    if (transferBytes[i] == 1)
                        transferBytes[i] = 0;
                    if (transferBytes[i + 1] == 1)
                        transferBytes[i + 1] = 0;
                    if (transferBytes[i + 2] == 1)
                        transferBytes[i + 2] = 0;
                    rgb[c] = transferBytes[i + 2];
                    rgb[c + 1] = transferBytes[i + 1];
                    rgb[c + 2] = transferBytes[i];
                    c += 3;
                }

            }
         
        }
        private void OnChanged(BoolArgs e)
        {
            if (Changed != null)
                Changed(this, e);
        }
       
       
    }
}
