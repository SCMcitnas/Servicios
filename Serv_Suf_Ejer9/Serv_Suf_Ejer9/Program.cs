using System.Net.Sockets;
using System.Net;

namespace Serv_Suf_Ejer9;

public class Program
{
    static void Main(string[] args)
    {
        Servidor servidor = new Servidor();
        servidor.Init();
    }
    
}
