using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Serv_Suf_Ejer9_C
{
    internal class Hilo
    {
        readonly object l = new object();

        bool termina = false;
        Socket s;

        void hilo(Socket socket)
        {
            IPEndPoint ieClient = (IPEndPoint)socket.RemoteEndPoint;
            Console.WriteLine("Client connected:{0} at port {1}", ieClient.Address, ieClient.Port);


            using (NetworkStream ns = new NetworkStream(socket))
            using (StreamReader sr = new StreamReader(ns))
            using (StreamWriter sw = new StreamWriter(ns))
            {

                string msg = "";
                if (msg != null)
                {
                    try
                    {

                        msg = sr.ReadLine();

                        switch (msg)
                        {
                            case "time":
                                sw.WriteLine(String.Format("{0}", DateTime.Now.TimeOfDay));
                                sw.Flush();
                                break;

                            case "date":
                                sw.WriteLine(String.Format("{0}", DateTime.Now.Date));
                                sw.Flush();
                                break;

                            case "all":
                                sw.WriteLine(String.Format("{0}", DateTime.Now));
                                sw.Flush();
                                break;

                            case String closeCase when closeCase.StartsWith("close "):

                                String passWrit = closeCase.Substring(6);
                                String pass;

                                using (StreamReader sr2 = new StreamReader(Environment.ExpandEnvironmentVariables("%PROGRAMDATA%") + "\\contraseña.txt"))
                                {
                                    pass = sr2.ReadToEnd();
                                }

                                if (passWrit == pass)
                                {
                                    sw.WriteLine("Contraseña correcta\nServidor cerrado");
                                    sw.Flush();

                                    lock (l)
                                    {
                                        termina = true;
                                        s.Close();
                                    }
                                }
                                else
                                {
                                    sw.WriteLine("Contraseña incorrecta");
                                    sw.Flush();
                                }

                                break;

                            default:
                                sw.WriteLine("Opcion incorrecta");
                                sw.Flush();
                                break;
                        }
                    }
                    catch (IOException e)
                    {
                        msg = null;
                    }
                }
                Console.WriteLine("Client disconnected.\nConnection closed");

            }
            socket.Close();
        }
}
