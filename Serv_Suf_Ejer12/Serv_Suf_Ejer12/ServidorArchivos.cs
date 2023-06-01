using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Serv_Suf_Ejer12
{
    internal class ServidorArchivos
    {
        readonly object l = new object();
        Socket s;
        bool terminar = false;

        public String leeArchivo(String nombreArchivo, int nLineas)
        {
            int contadorL = 0;
            int contadorBucle = 0;
            String texto = "";
            try
            {
                String path = Environment.ExpandEnvironmentVariables("EXAMEN") + "\\" + nombreArchivo;
                StreamReader sr = new StreamReader(path);
                texto=sr.ReadLine();

                while(texto != null)
                {
                    contadorL++;
                    texto=sr.ReadLine();
                }

                sr.Close();

                StreamReader sr2 = new StreamReader(path);
                texto = sr2.ReadLine();

                if (contadorL <= nLineas)
                {
                    while(texto != null && contadorBucle<=nLineas)
                    {
                        contadorBucle++;
                        texto+=sr2.ReadLine();
                    }
                }
                else
                {
                    texto = sr2.ReadToEnd();
                }

                sr2.Close();

                return texto;
            }
            catch(Exception ex)
            {
                return "<ERROR_IO>";
            }
        }

        public int leePuerto()
        {
            int puerto;
            IPEndPoint ie = null;
            try
            {
                puerto = Convert.ToInt32(leeArchivo("puerto.txt", 1));
                ie = new IPEndPoint(IPAddress.Any, puerto);
            }
            catch (Exception ex)
            {
                puerto = 31416;
            }

            return puerto;
        }

        public void guardaPuerto(int numero)
        {
            try
            {
                String path = Environment.ExpandEnvironmentVariables("EXAMEN") + "\\puerto.txt";
                StreamWriter sw = new StreamWriter(path, false);

                sw.WriteLine(numero);

                sw.Close();
            }
            catch(Exception ex)
            {

            }
        }

        public String listaArchivos()
        {
            String nombres="";

            try
            {
                String path = Environment.ExpandEnvironmentVariables("EXAMEN");
                FileInfo[] ficheros;
                DirectoryInfo di = new DirectoryInfo(path);

                ficheros = di.GetFiles("*.txt");


                foreach (FileInfo fi in ficheros)
                {
                    nombres += fi.Name;
                }

            }
            catch(Exception ex)
            {

            }
            return nombres;
        }

        public void iniciaServidorArchivos()
        {
            IPEndPoint ie = null;
            bool valido;

            try
            {
                ie = new IPEndPoint(IPAddress.Any, leePuerto());
                Console.WriteLine("Conectado al puerto " + ie.Port);
                valido = true;
            }
            catch(SocketException ex)
            {
                valido = false;
                Console.WriteLine("El puerto esta ocupado. No se puede iniciar el servidor");
            }

            if (valido)
            {
                s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                s.Bind(ie);
                s.Listen(10);

                do
                {
                    try
                    {
                        Socket sCliente = s.Accept();
                        Thread hiloC = new Thread(hiloCliente);
                        hiloC.Start(sCliente);
                    }
                    catch (SocketException ex)
                    {

                    }
                } while (!terminar);
            }
        }

        public void hiloCliente(object socket)
        {
            Socket cliente = (Socket)socket;

            IPEndPoint ieC = (IPEndPoint)cliente.LocalEndPoint;
            Console.WriteLine("Conectado el cliente " + ieC.Address + " " + ieC.Port);

            using(NetworkStream ns= new NetworkStream(cliente))
            using(StreamReader sr = new StreamReader(ns))
            using(StreamWriter sw = new StreamWriter(ns))
            {
                string msg = "";
                bool conectado = true;

                do
                {
                    if (msg != null)
                    {

                        try
                        {
                            msg = sr.ReadLine();

                            switch (msg)
                            {
                                case String archivo when archivo.StartsWith("GET "):

                                    String[] argumentos = msg.Substring(4).Split(",");

                                    String nArchivo = argumentos[0];
                                    int n;

                                    try
                                    {
                                        n = Convert.ToInt32(argumentos[1]);
                                    }
                                    catch(Exception ex)
                                    {
                                        n = 1;
                                    }

                                    sw.WriteLine(leeArchivo(nArchivo, n));
                                    sw.Flush();

                                    break;

                                case String numero when numero.StartsWith("PORT "):

                                    String dato = numero.Substring(5);
                                    try
                                    {
                                        guardaPuerto(Convert.ToInt32(dato));
                                        sw.WriteLine("Puerto guardado");
                                        sw.Flush();
                                    }
                                    catch (FormatException ex)
                                    {
                                        sw.WriteLine("Numero no valido");
                                        sw.Flush();
                                    }
                                    catch (OverflowException ex)
                                    {
                                        sw.WriteLine("Numero demasiado grande");
                                        sw.Flush();
                                    }

                                    break;

                                case "LIST":
                                    sw.WriteLine("Lista de archivos:");
                                    sw.Flush();
                                    sw.WriteLine(listaArchivos());
                                    sw.Flush();
                                    break;

                                case "CLOSE":
                                    sw.WriteLine("Cliente cerrado");
                                    sw.Flush();
                                    conectado = false;
                                    cliente.Close();
                                    break;

                                case "HALT":
                                    conectado = false;
                                    Console.WriteLine("Servidor cerrado");
                                    lock (l)
                                    {
                                        terminar = true;
                                        s.Close();
                                    }
                                    break;

                                default:
                                    sw.WriteLine("Opcion no valida");
                                    sw.Flush();
                                    break;
                            }
                        }
                        catch (IOException ex)
                        {
                            msg = null;
                        }
                    }
                } while (conectado);
                
            }
            cliente.Close();
        }
    }
}
