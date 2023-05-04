namespace Serv_Suf_Ejer5;

public class Program
{
    static readonly private object l = new object();
    
    static int posicionCaballo = 0;
    static String ganador = "";
    static bool terminar = false;
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

    public static void carrera(String nombre)
    {
        Random random = new Random();
        
        int avance=0;
        int posicionEsteCaballo=posicionCaballo;

        while (!terminar)
        {
            lock (l)
            {
                if (!terminar)
                {
                    avance += random.Next(5, 10);
                    Console.SetCursorPosition(avance,posicionEsteCaballo);

                    Console.WriteLine(posicionEsteCaballo);

                    if(avance >= 50)
                    {
                        terminar = true;
                        ganador = nombre;
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
        String apuesta = "";
        bool correcto = false;
        bool repetir = false;
        String opcion;
        String path = Environment.GetEnvironmentVariable("appdata") + "\\datos.bin";


        Thread thread = new Thread(() => carrera(""));

        

            Console.WriteLine("Bienvenido\nIntroduzca su edad:");
            edad = Convert.ToInt16(Console.ReadLine());

            do
            {
                if (edad >= 18)
                {
                    do
                    {
                        Console.WriteLine("Introduzca su apuesta| Caballos: (" + String.Join(" | ", recuentoCaballos()) + ")");
                        apuesta = Console.ReadLine().ToLower().Trim();
                        foreach (String caballo in recuentoCaballos())
                        {
                            if (apuesta == caballo)
                            {
                                correcto = true;
                            }
                        }

                        if (!correcto)
                        {
                            Console.WriteLine("Nombre del caballo incorrecto");
                        }

                    } while (!correcto);

                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Juego prohibido para menores de edad");
                    Environment.Exit(1);
                }

                foreach (String caballo in recuentoCaballos())
                {
                    if (posicionCaballo != 0)
                    {
                        lock (l)
                        {
                            Monitor.Wait(l);
                        }
                    }

                    posicionCaballo++;

                    thread = new Thread(() => carrera(caballo));


                    thread.Start();

                }

                thread.Join();

                Console.Clear();
                Console.WriteLine("Ganador: " + ganador);

                using (FileStream fs = new FileStream(path, FileMode.Append))
                {

                    //fs.WriteByte(Convert.ToByte(ganador));
                    fs.WriteByte(Convert.ToByte(edad));


                    if (apuesta == ganador)
                    {
                        Console.WriteLine("Usted ha ganado la apuesta");
                        //fs.WriteByte(Convert.ToByte("El usuario gano la apuesta"));
                    }
                    else
                    {
                        Console.WriteLine("Su apuesta fue incorrecta");
                        //fs.WriteByte(Convert.ToByte("El usuario perdio la apuesta"));
                    }
                }

                do
                {
                    Console.WriteLine("¿Quiere repetir el juego? (Y/N)");
                    opcion = Console.ReadLine();
                    if (opcion.ToLower().Trim() == "y")
                    {
                        posicionCaballo = 0;
                        terminar = false;
                        repetir = true;
                    }
                    else if (opcion.ToLower().Trim() == "n")
                    {
                        using (FileStream fs = new FileStream(path, FileMode.Open))
                        {
                            for (long i = 0; i < fs.Length; i++)
                            {
                                Console.WriteLine(fs.ReadByte());
                            }
                        }
                        
                        repetir = false;
                    }
                    else
                    {
                        Console.WriteLine("Opcion incorrecta, pruebe de nuevo");
                    }
                } while (opcion.ToLower().Trim() != "y" && opcion.ToLower().Trim() != "n");

            } while (repetir);

        
    }
}