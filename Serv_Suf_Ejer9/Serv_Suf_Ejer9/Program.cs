using System.Net.Sockets;
using System.Net;

namespace Serv_Suf_Ejer9;

class Program
{

    static void Main(string[] args)
    {
        IPEndPoint ie = new IPEndPoint(IPAddress.Any, 31416);
        using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
        ProtocolType.Tcp))
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

                string msg = "";
                while (msg != null)
                {
                    try
                    {
                        msg = sr.ReadLine();

                        switch (msg)
                        {
                            case "time":
                                sw.WriteLine(String.Format("{0}", DateTime.Now));
                                sw.Flush();
                                break;

                            case "date":
                                break;

                            case "all":
                                break;

                            case "close":
                                break;

                            default:
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
