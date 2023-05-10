namespace Serv_Suf_Ejer6;

public class Program
{
    static readonly private object l = new object();
    static Random rand = new Random();
    static int numeroSacado;
    static int contador;
    static bool movimiento = true;
    static int posicionBarra=0;
    static bool terminar = false;
    static String ganador;
    static bool principio = true;

    static void Main(string[] args)
    {
        Thread jugador1 = new Thread(player1);
        Thread jugador2 = new Thread(player2);
        Thread hiloB = new Thread(display);

        jugador1.Start();
        jugador2.Start();
        hiloB.Start();

        hiloB.Join();

        Console.SetCursorPosition(1, 6);
        Console.WriteLine(String.Format("El ganador es el {0}",ganador));
    }

    static public void player1()
    {
        while (!terminar)
        {
            lock (l)
            {
                if (!terminar)
                {
                    numeroSacado = rand.Next(1, 10);
                    if (numeroSacado == 5 || numeroSacado == 7)
                    {
                        if (movimiento)
                        {
                            movimiento = false;
                            contador++;
                        }
                        else
                        {
                            contador = contador + 5;
                        }
                    }

                    principio = false;

                    if (contador > 14)
                    {
                        ganador = "Jugador 1";
                        terminar = true;
                    }

                    Console.SetCursorPosition(1, 1);
                    Console.Write(String.Format("Contador: {0,3}", contador));
                    Console.SetCursorPosition(1, 2);
                    Console.Write(String.Format("Jugador 1: {0}", numeroSacado));
                }
            }
            Thread.Sleep(rand.Next(100, 100 * numeroSacado));
        }
    }

    static public void player2()
    {
        while (!terminar)
        {
            lock (l)
            {
                if (!terminar)
                {
                    numeroSacado = rand.Next(1, 10);
                    if (numeroSacado == 5 || numeroSacado == 7)
                    {
                        if (!movimiento)
                        {
                            movimiento = true;
                            contador--;
                        }
                        else
                        {
                            if (!principio)
                            {
                                contador = contador - 5;
                            }
                        }
                    }

                    principio = false;

                    if (contador < -14)
                    {
                        ganador = "Jugador 2";
                        terminar = true;
                    }

                    Console.SetCursorPosition(1, 1);
                    Console.Write(String.Format("Contador: {0,3}" ,contador));
                    Console.SetCursorPosition(1, 3);
                    Console.Write(String.Format("Jugador 2: {0}", numeroSacado));


                }
            }
            Thread.Sleep(rand.Next(100, 100 * numeroSacado));
        }
    }

    static public void display()
    {
        char[] barra = { '|','/','-','\\'};

        
        while (!terminar)
        {
            lock (l)
            {

                if (movimiento)
                {
                    if (posicionBarra < barra.Length - 1)
                    {
                        posicionBarra++;
                    }
                    else
                    {
                        posicionBarra = 0;
                    }

                    Console.SetCursorPosition(1, 4);
                    Console.Write(new String(' ', Console.WindowWidth));
                    Console.Write(barra[posicionBarra]);
                }
            }
            Thread.Sleep(100);
        }
        
    }

    
}


//     5