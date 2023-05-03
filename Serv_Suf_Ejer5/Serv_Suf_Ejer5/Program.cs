namespace Serv_Suf_Ejer5;

public class Program
{
    static readonly private object l = new object();
    
    static int posicionCaballo = 1;
    public static String[] recuentoCaballos()
    {
        try
        {
            String path = Environment.GetEnvironmentVariable("appdata") + "\\caballos.txt";
            int nCaballos=0;
            String[] caballos = {};
            using (StreamReader sr = new StreamReader(path))
            {
                String fichero = sr.ReadToEnd();
                if(fichero.Trim() != "")
                {
                   caballos = fichero.Split(';');
                }
            }

            return caballos;
        }
        catch (FileNotFoundException e)
        {
            Console.Write("Archivo no encontrado");
            return null;
        }
        
    }

    public static void carrera()
    {
        Random random = new Random();
        bool terminar = false;
        int avance=0;
        int posicionEsteCaballo=posicionCaballo;

        while (!terminar)
        {
            lock (l)
            {
                if (!terminar)
                {
                    avance += random.Next(1, 10);
                    Console.SetCursorPosition(avance,posicionEsteCaballo);
                    
                    Console.WriteLine(posicionEsteCaballo);

                    if(avance >= 50)
                    {
                        terminar = true;
                    }
                    else
                    {
                        Monitor.Pulse(l);
                        Thread.Sleep(random.Next(500, 1000));
                    }

                }
            }
        }
    }

    static void Main(string[] args)
    {
        recuentoCaballos();
        int edad;
        String apuesta;

        Console.WriteLine("Bienvenido\nIntroduzca su edad:");
        edad=Convert.ToInt16(Console.ReadLine());
        if (edad >= 18)
        {
            Console.WriteLine("Introduzca su apuesta| Caballos: ("+String.Join(" | ",recuentoCaballos())+")");
            apuesta = Console.ReadLine().ToLower().Trim();
            Console.Clear();
        }
        else 
        {
            Console.WriteLine("Juego prohibido para menores de edad");
            Environment.Exit(1);
        }

        foreach (String caballo in recuentoCaballos())
        {
            Thread thread = new Thread(carrera);
            thread.Start();

            posicionCaballo++;
        }

     }
}