using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Serv_Suf_Ejer10
{
    internal class Juego
    {
        readonly object l = new object();

        String pathApp = Environment.GetEnvironmentVariable("appdata") + "\\historialNumeros.txt";

        List<Socket> list = new List<Socket>();
        Socket s;

        Random random = new Random();
        List<double> numeros = new List<double>();
        double numero;
        double numeroM=0;
        String ipM;
        String puertoM;
        String junto;

        List<String> ips = new List<string>();
        List<String> puertos = new List<string>();

        int tiempo = 10;

        bool terminar = false;
        

        public void hiloSuprm()
        {
            do
            {
                tiempo--;

                try
                {
                    foreach(Socket sockt in list)
                    {
                        IPEndPoint ieCliente = (IPEndPoint)sockt.RemoteEndPoint;
                        using (NetworkStream ns = new NetworkStream(sockt))
                        using (StreamReader sr = new StreamReader(ns))
                        using (StreamWriter sw = new StreamWriter(ns))
                        {
                            lock (l)
                            {
                                sw.WriteLine("Quedan " + tiempo + " segundos");
                                sw.Flush();


                                if (tiempo == 0)
                                {
                                    numero = random.Next(1, 20);
                                    sw.WriteLine("Su numero es: " + numero);
                                    sw.Flush();

                                    numeros.Add(numero);
                                    ips.Add(ieCliente.Address.ToString());
                                    puertos.Add(ieCliente.Port.ToString());

                                }

                            }
                        }

                    }

                }
                catch (IOException ex)
                {

                }

                Thread.Sleep(1000);


            } while (tiempo != 0);

            for (int i = 0; i < numeros.Count; i++)
            {
                if (numeros[i] > numeroM)
                {
                    numeroM = numeros[i];
                    ipM = ips[i];
                    puertoM = puertos[i];
                    junto=ipM+" "+puertoM;
                }
                else if (numeros[i] == numeroM)
                {
                    junto += ", " + ips[i]+" " + puertos[i];
                }
            }

            foreach(Socket socktM in list)
            {
                try
                {
                    using (NetworkStream ns = new NetworkStream(socktM))
                    using (StreamReader sr = new StreamReader(ns))
                    using (StreamWriter sw = new StreamWriter(ns))
                    {
                        lock (l)
                        {
                            sw.WriteLine("El ganador es " + junto + " con el numero " + numeroM);
                            sw.Flush();

                            socktM.Close();
                        }
                    }
                }
                catch (IOException ex)
                {

                }
            }

            using(StreamWriter sw = new StreamWriter(pathApp, true))
            {
                sw.WriteLine(DateTime.Now+" -> El ganador fue "+junto+" con el numero "+numeroM);
            }
        }

        public void Init()
        {
            IPEndPoint ie = null;

            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                int puerto = Convert.ToInt32(Environment.GetEnvironmentVariable("homepath") + "\\puerto.txt");
                ie = new IPEndPoint(IPAddress.Any, puerto);
            }
            catch (Exception ex)
            {
                ie = new IPEndPoint(IPAddress.Any, 31416);
            }

            s.Bind(ie);
            s.Listen(10);
            Console.WriteLine($"Server listening at port:{ie.Port}");

            do
            {
                try
                {
                    Socket sCliente = s.Accept();
                    list.Add(sCliente);

                    if(list.Count > 1)
                    {
                        Thread hilo = new Thread(hiloSuprm);
                        hilo.Start();
                    }

                    
                } 
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (!terminar);
        }
    }
}
