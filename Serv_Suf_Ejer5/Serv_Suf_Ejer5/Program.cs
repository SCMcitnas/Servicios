using System.Linq.Expressions;
using System.Text;

namespace Serv_Suf_Ejer5;

public class Program
{
    static readonly private object l = new object();

    static Random random = new Random();

    static int posicionCaballo = 0;
    static String ganador = "";
    static bool terminar = false;

    static bool[] tropieceCaballos;
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

    public static void carrera(String nombre, int posicionEsteCaballo)
    {
        
        
        int avance=0;
        

        while (!terminar)
        {
            lock (l)
            {
                if (!terminar)
                {
                    if (!tropieceCaballos[posicionEsteCaballo-1])
                    {
                        avance += 1;// random.Next(5, 10);
                    }
                    else
                    {
                        tropieceCaballos[posicionEsteCaballo - 1] = false;
                        Console.SetCursorPosition(0, recuentoCaballos().Length + 1);
                        Console.WriteLine(String.Format("Se ha tropezado el caballo {0}",posicionEsteCaballo));
                    }
                    
                    Console.SetCursorPosition(avance,posicionEsteCaballo);

                    Console.WriteLine(posicionEsteCaballo);

                    if(avance >= 50)
                    {
                        terminar = true;
                        ganador = nombre;
                    }

                    Monitor.Pulse(l);
                }
            }
            Thread.Sleep(100);// random.Next(50, 200));
            
        }
    }

    public static void tropezar()
    {
        int aleatorio;
        while (!terminar)
        {
            lock (l)
            {
                if (!terminar)
                {
                    aleatorio = random.Next(0, recuentoCaballos().Length);
                    tropieceCaballos[aleatorio]= true;
                }
            }
            Thread.Sleep(2000);
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
        String premio;
        bool incorrecto = false;
        tropieceCaballos = new bool[recuentoCaballos().Length];

        Thread thread = null;// = new Thread(() => carrera("",0));
        Console.WriteLine("Bienvenido\n");
        
        do
        {
            try
            {
                Console.WriteLine("Introduzca su edad:");
                edad = Convert.ToInt16(Console.ReadLine());
                incorrecto = false;

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
                        Environment.Exit(0);
                    }

                    foreach (String caballo in recuentoCaballos())
                    {


                        tropieceCaballos[posicionCaballo] = false;


                        posicionCaballo++;
                        String aux = caballo;
                        int aux2 = posicionCaballo;
                        thread = new Thread(() => carrera(aux, aux2));


                        thread.Start();

                    }
                    Thread hiloT = new Thread(tropezar);
                    hiloT.Start();

                    thread.Join();

                    Console.SetCursorPosition(0, recuentoCaballos().Length + 2);
                    Console.WriteLine("Ganador: " + ganador);

                    using (BinaryWriter bw = new BinaryWriter(new FileStream(path, FileMode.Append)))
                    {

                        bw.Write(ganador);
                        bw.Write(edad);

                        if (apuesta == ganador)
                        {
                            Console.WriteLine("Usted ha ganado la apuesta");
                            premio = "El usuario gano la apuesta";
                        }
                        else
                        {
                            Console.WriteLine("Su apuesta fue incorrecta");
                            premio = "El usuario perdio la apuesta";
                        }

                        bw.Write(premio);
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
                            using (BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open)))
                            {
                                int pos = 0;
                                int lenght = (int)br.BaseStream.Length;
                                try
                                {
                                    while (true)
                                    {

                                        Console.WriteLine(br.ReadString());
                                        Console.WriteLine(br.ReadInt32());
                                        Console.WriteLine(br.ReadString());
                                        Console.WriteLine("\n");
                                    }
                                }
                                catch (EndOfStreamException ex)
                                {

                                }
                            }

                            repetir = false;
                            incorrecto = true;
                        }
                        else
                        {
                            Console.WriteLine("Opcion incorrecta, pruebe de nuevo");
                        }
                    } while (opcion.ToLower().Trim() != "y" && opcion.ToLower().Trim() != "n");

                } while (repetir);
            }
            catch (FormatException)
            {
                Console.WriteLine("Dato incorrecto");
            }

        } while (!incorrecto);
        


    }
}