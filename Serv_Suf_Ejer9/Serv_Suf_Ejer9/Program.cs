using System.Net.Sockets;
using System.Net;

namespace Serv_Suf_Ejer9;

class Program
{

    static void Main(string[] args)
    {
        IPEndPoint ie = new IPEndPoint(IPAddress.Any, 31416);
        using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            s.Bind(ie);
            s.Listen(10);
            Console.WriteLine($"Server listening at port:{ie.Port}");
            Socket sClient = s.Accept();
            IPEndPoint ieClient = (IPEndPoint)sClient.RemoteEndPoint;
            Console.WriteLine("Client connected:{0} at port {1}", ieClient.Address, ieClient.Port);
            using (NetworkStream ns = new NetworkStream(sClient))
            using (StreamReader sr = new StreamReader(ns))
            using (StreamWriter sw = new StreamWriter(ns))
            {

                sw.WriteLine("Bienvenido al servidor");
                sw.Flush();

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

                                if(passWrit == pass)
                                {
                                    sw.WriteLine("Contraseña correcta");
                                    sw.Flush();
                                    Console.WriteLine("Acerto la contraseña");
                                }
                                else
                                {
                                    sw.WriteLine("Contraseña incorrecta");
                                    sw.Flush();
                                    Console.WriteLine("Fallo la contraseña");
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
            sClient.Close(); // Este no se abre con using, pues lo devuelve el accept.
        }
    }
}
