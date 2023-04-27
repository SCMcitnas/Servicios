public class Program
{
    public delegate int Variable(int a,int b);
    static readonly private object l = new object();
    static int x=0;
    int y = 1;
    static bool terminar = false;
    static bool ganador = false;
    Variable v;

    static void Main(string[] args)
    {
        Thread incremento = new Thread(hilo1);
        Thread descenso = new Thread(hilo2);
        incremento.Start();
        descenso.Start();

        // Espera a que acaben join o wait/pulse

        // Informa de quién gana

        descenso.Join();

        if (ganador)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ha ganado el hilo 2");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Ha ganado el hilo 1");
        }

    }

    static void hilo1()
    {
        Program clase = new Program();
        clase.v = (x,y) => x+y;


        while (!terminar)
        {
            lock (l)
            {
                if (!terminar)
                {

                    Console.ForegroundColor = ConsoleColor.Green;

                   
                        x = clase.v(x, clase.y);
                        Console.WriteLine("Hilo 1:{0}", x);
                        Monitor.Pulse(l);

                     
                    if (clase.v(x, clase.y) >= 501)
                    {
                        terminar = true;
                        ganador = false;
                    }

                }
            }
        }
        
    }

    static void hilo2()
    {
        Program clase = new Program();
        clase.v = (x,y) => x-y;

        lock (l)
        {
            Monitor.Wait(l);
        }

        while (!terminar)
        {
            lock (l)
            {
                if(!terminar)
                {
                    
                    Console.ForegroundColor = ConsoleColor.Red;

                    x = clase.v(x, clase.y);
                    Console.WriteLine("Hilo 2:{0}", x);
                    Monitor.Pulse(l);

                    if (clase.v(x, clase.y) <= -501)
                    {
                        terminar = true;
                        ganador = true;
                    }
                }
            }
        }
    }
}
