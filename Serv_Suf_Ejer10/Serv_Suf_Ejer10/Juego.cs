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

        List<Socket> list = new List<Socket>();
        Socket s;

        Random random = new Random();
        List<double> numeros = new List<double>();
        double numero;
        int tiempo=10;
        bool terminar = false;


        public void hiloTiempo()
        {
            lock (l)
            {
                
            }
        }

        public void hilo(object socket)
        {

            Socket socketC = (Socket)socket;

            using (NetworkStream ns = new NetworkStream(socketC))
            using (StreamReader sr = new StreamReader(ns))
            using (StreamWriter sw = new StreamWriter(ns))
            {
                while (tiempo != 0)
                {
                    tiempo--;
                    sw.WriteLine("Quedan " + tiempo + " segundos");
                    sw.Flush();
                    lock (l)
                    {
                        Thread.Sleep(1000);

                    }
                    

                    

                    if (tiempo == 0)
                    {
                        numero = random.Next(1, 20);
                        sw.WriteLine("Su numero es: " + numero);
                        sw.Flush();

                        numeros.Add(numero);

                        socketC.Close();
                    }

                    
                }
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
            s.Listen(10); //Ilimitado
            Console.WriteLine($"Server listening at port:{ie.Port}");

            do
            {
                try
                {
                    Socket sCliente = s.Accept();
                    list.Add(sCliente);
                    if(list.Count >= 2)
                    {
                        Thread hiloC = new Thread(hilo);
                        //Thread hiloT = new Thread(hiloTiempo);
                        hiloC.Start(sCliente);
                       // hiloT.Start();
                    }
                } 
                catch (Exception ex)
                {
                    Console.WriteLine("Fallo main");
                }
            } while (!terminar);
        }
    }
}
