using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfazServidor
{
    public partial class Form1 : Form
    {
        public static String IP_SERVER = "192.168.20.1";
        public static int port = 31416;

        String msg;
        String UserMsg;

        IPEndPoint ie;
        Socket server;

        public Form1()
        {
            InitializeComponent();     
        }

        private void metodoBtns(object sender , EventArgs e)
        {
            try
            {
                ie = new IPEndPoint(IPAddress.Parse(IP_SERVER), port);
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Connect(ie);
                lblError.Text = "";

                using (NetworkStream ns = new NetworkStream(server))
                using (StreamReader sr = new StreamReader(ns))
                using (StreamWriter sw = new StreamWriter(ns))
                {
                    UserMsg = ((Button)sender).Text.ToLower();

                    if(UserMsg == "close")
                    {
                        UserMsg += " " + txtbContraseña.Text;
                    }

                    sw.WriteLine(UserMsg);
                    sw.Flush();

                    lblResult.Text = sr.ReadToEnd();
                }

                server.Close();
            }
            catch (SocketException ex)
            {
                lblError.Text = "No es posible conectar al servidor";
            }catch(System.IO.IOException ex)
            {
                lblError.Text = "No es posible conectar al servidor";
            }

            
        }

        private void btnCambia_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }
    }
}
