using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;


// TODO: Repetir juego, X si ambos puertos ocupados que no falle, X puerto ocupado y lectura archivo puerto
namespace Serv_Suf_Ejer10
{
    internal class Juego
    {
        readonly object l = new object();

        String pathApp = Environment.GetEnvironmentVariable("appdata") + "\\historialNumeros.txt";

        List<Socket> list = new List<Socket>();
        List<Socket> listEx = new List<Socket>();

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
        int tiempoInit = 10;

        bool terminar = false;
        bool exception = false;
        

        public void hiloSuprm()
        {
            if (list.Count <= 1)
            {
                
                using (NetworkStream ns = new NetworkStream(list[0]))
                using (StreamWriter sw = new StreamWriter(ns))
                {
                    sw.WriteLine("Esperando otro jugador");
                    sw.Flush();
                }
            }
            else
            {
                do
                {
                    exception = false;
                    tiempo--;

                    try
                    {
                        foreach (Socket sockt in list)
                        {
                            IPEndPoint ieCliente = (IPEndPoint)sockt.RemoteEndPoint;
                            using (NetworkStream ns = new NetworkStream(sockt))
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
                        try
                        {
                            listEx = new List<Socket>();

                            foreach (Socket sockt in list)
                            {
                                if (sockt.Connected)
                                {
                                    listEx.Add(sockt);
                                }
                            }

                            list = listEx;

                            if (list.Count <= 1)
                            {
                                exception = true;
                                tiempo = tiempoInit;

                                using (NetworkStream ns = new NetworkStream(list[0]))
                                using (StreamWriter sw = new StreamWriter(ns))
                                {
                                    sw.WriteLine("Jugadores insuficientes");
                                    sw.Flush();
                                }


                            }
                        }                            
                        catch (IOException ex2)
                        {
                            tiempo = tiempoInit;
                        }
                        catch(InvalidOperationException ex2)
                        {
                            Socket clienteGuardado = list[list.Count - 1];
                            list = new List<Socket>();
                            list.Add(clienteGuardado);
                            tiempo = tiempoInit;
                            exception = true;
                        }
                        

                    }

                    Thread.Sleep(1000);


                } while (tiempo != 0 && exception==false);

                if (list.Count > 1)
                {
                    for (int i = 0; i < numeros.Count; i++)
                    {
                        if (numeros[i] > numeroM)
                        {
                            numeroM = numeros[i];
                            ipM = ips[i];
                            puertoM = puertos[i];
                            junto = ipM + " " + puertoM;
                        }
                        else if (numeros[i] == numeroM)
                        {
                            junto += ", " + ips[i] + " " + puertos[i];
                        }
                    }

                    foreach (Socket socktM in list)
                    {
                        try
                        {
                            using (NetworkStream ns = new NetworkStream(socktM))
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

                    using (StreamWriter sw = new StreamWriter(pathApp, true))
                    {
                        sw.WriteLine(DateTime.Now + " -> El ganador fue " + junto + " con el numero " + numeroM);
                    }

                    list = new List<Socket>();
                    tiempo = tiempoInit;
                }
                
            }

            
        }

        public void Init()
        {

            bool abierto = true;
            IPEndPoint ie = null;
            String puertoArchivo="";
            using (StreamReader sw = new StreamReader(Environment.GetEnvironmentVariable("homepath") + "\\puerto.txt"))
            {
               puertoArchivo = sw.ReadToEnd();
            }
            

            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                int puerto = Convert.ToInt32(puertoArchivo);
                ie = new IPEndPoint(IPAddress.Any, puerto);
                s.Bind(ie);
            }
            catch (Exception ex)
            {
                try
                {
                    ie = new IPEndPoint(IPAddress.Any, 31416);
                    s.Bind(ie);
                }
                catch(Exception ex2)
                {
                    abierto = false;
                }
                
            }

            if (abierto)
            {
                s.Listen(10);
                Console.WriteLine($"Server listening at port:{ie.Port}");

                do
                {
                    try
                    {
                        Socket sCliente = s.Accept();
                        list.Add(sCliente);

                        if (tiempo == tiempoInit || tiempo == 0)
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
}
