using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Serv_Suf_Ejer9
{
    internal class Servidor
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
                                sw.WriteLine(String.Format("{0}", TimeOnly.FromDateTime(DateTime.Now)));
                                sw.Flush();
                                break;

                            case "date":
                                sw.WriteLine(String.Format("{0}", DateOnly.FromDateTime(DateTime.Now)));
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


        public void Init()
        {
            int puerto = 31416;
            bool valido = false;


            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ie = null;
            do
            {
                try
                {
                    ie = new IPEndPoint(IPAddress.Any, puerto);
                    valido = true;
                }
                catch (SocketException ex)
                {
                    puerto++;
                    valido = false;
                }
            } while (!valido);


            s.Bind(ie);
            s.Listen(10);
            Console.WriteLine($"Server listening at port:{ie.Port}");

            do
            {
                try
                {
                    Socket sClient = s.Accept();
                    Thread lanzar = new Thread(() => hilo(sClient));
                    lanzar.Start();
                }
                catch (System.Net.Sockets.SocketException ex)
                {

                }

            } while (!termina);

        }
    }
}
