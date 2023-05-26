using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Serv_Suf_Ejer11
{
    internal class Sala
    {
        readonly object l = new object();

        List<Socket> clientes = new List<Socket>();
        bool termina = false;
        int conectados = -1;

        // TODO:  LOCKS
        int leePuerto()
        {


            String puertoS="";
            try
            {
                using (StreamReader sr = new StreamReader(Environment.GetEnvironmentVariable("temp")+"\\puerto.txt"))
                {
                    puertoS = sr.ReadToEnd();
                }
            }
            catch(Exception ex)
            {
                return 0;
            }



            int puerto;  //TRY
            IPEndPoint ie = null;

            try
            {
                puerto = Convert.ToInt32(puertoS);
                ie = new IPEndPoint(IPAddress.Any, puerto);
            }
            catch (SocketException ex)
            {
                puerto = 10000;
            }
            catch(FormatException ex)
            {
                puerto = 10000;
            }
            catch(OverflowException ex)
            {
                puerto = 10000;
            }

            return puerto;
        }

        void envioMensaje(String m, IPEndPoint ie, Socket s)
        {
            try
            {
                using (NetworkStream ns = new NetworkStream(s))
                using (StreamWriter sw = new StreamWriter(ns))
                {
                    sw.WriteLine(ie.Address + " " + ie.Port + ": " + m);
                    sw.Flush();
                }
            }
            catch(IOException ex)
            {

            }
            
        }

        void iniciaServicioChat()
        {
            bool bucle=true;
            IPEndPoint ie = null;
            int puerto = leePuerto();
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            while (bucle)
            {
                try
                {
                    ie=new IPEndPoint(IPAddress.Any, puerto);
                    s.Bind(ie);
                    bucle=false;
                }
                catch(SocketException ex)
                {
                    if (puerto <= IPEndPoint.MaxPort)
                    {
                        puerto++;
                    }
                    else
                    {
                        puerto = 10000;
                    }
                    
                }
            }

            s.Listen(10);
            Console.WriteLine("Conectado al puerto: " + ie.Port);

            while (!termina)
            {
                try
                {
                    Socket cliente = s.Accept();
                    clientes.Add(cliente);
                    conectados++;

                    Thread lanzarHilo = new Thread(hiloCliente);
                    lanzarHilo.Start(cliente);
                }
                catch(SocketException ex){

                }

            }
        }

        void hiloCliente(object socket)
        {

            bool conectado = true;

            Socket sCliente = (Socket)socket;
            IPEndPoint ieCliente = (IPEndPoint)sCliente.RemoteEndPoint;
            String mensaje = "";
            IPEndPoint ieMensaje = null;

            bool controlSalida = false;

            while (mensaje != null)
            {
                try
                {
                    using (NetworkStream ns = new NetworkStream(sCliente))
                    using (StreamReader sr = new StreamReader(ns))
                    using (StreamWriter sw = new StreamWriter(ns))
                    {
                        sw.WriteLine("IP: " + ieCliente.Address + " Port: " + ieCliente.Port);
                        sw.WriteLine("Bienvenido al chat");
                        sw.WriteLine("Actualmente hay " + conectados + " usuarios conectados al chat");
                        sw.Flush();

                        while (conectado)
                        {
                            mensaje = sr.ReadLine();

                            if (mensaje == null)
                            {
                                conectado = false;
                            }


                            if (mensaje == "MELARGO")
                            {
                                lock (l)
                                {
                                    clientes.Remove(sCliente);
                                    conectado = false;
                                    sCliente.Close();
                                }
                            }
                            else
                            {
                                lock (l)
                                {
                                    try
                                    {
                                        foreach (Socket sockt in clientes)
                                        {
                                            ieMensaje = (IPEndPoint)sockt.RemoteEndPoint;

                                            if (sockt != sCliente && mensaje != null)
                                            {
                                                envioMensaje(mensaje, ieMensaje, sockt);
                                            }
                                        }
                                    }
                                    catch (InvalidOperationException ex)
                                    {
                                        mensaje = null;
                                        clientes.Remove(sCliente);
                                        sCliente.Close();
                                        conectados--;
                                    }
                                }
                            }

                            if (!controlSalida)
                            {
                                conectados--;
                                controlSalida = true;
                            }

                        }
                    }
                }
                catch (IOException ex)
                {
                    mensaje = null;
                    clientes.Remove(sCliente);
                    sCliente.Close();
                    conectados--;

                }
            }

        }

        public void Init()
        {
            iniciaServicioChat();
        }
    }
}
