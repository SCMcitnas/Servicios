using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serv_Suf_Ejer4
{

    /*
     * titulo
    icono y  X
    using X
    catch con text vacion en run X
    info de hilos X
    cierre natural de proceso X
    procesos sin permisos (4) X
    Uso de Array.ForEach en al menos dos sitios X
     */
    public partial class Procesos : Form
    {
        public delegate String Mensaje(Process p);
        Mensaje m;
        static String pathApp = Environment.GetEnvironmentVariable("appdata") + "\\log.txt";

        StreamWriter sw;


        public Procesos()
        {
            InitializeComponent();
        }

        private void btnViewProc_Click(object sender, EventArgs e)
        {
            tbAmpliado.Text = "";
            Process[] processes = Process.GetProcesses();

            foreach(Process process in processes)   // Array.ForEach
            {
                String processName = process.ProcessName;
                String mainWindowTitle= process.MainWindowTitle;
                
                if(process.ProcessName.Length > 15)
                {
                    processName=process.ProcessName.Substring(0,15)+"...";
                }
                else if(process.MainWindowTitle.Length > 15)
                {
                    mainWindowTitle = process.MainWindowTitle.Substring(0, 15)+"...";
                }
                
                m = (p) => String.Format("{0,6} \t {1,-15} \t {2,-15}\r\n", p.Id, processName, mainWindowTitle);
                tbAmpliado.Text += m(process);
            }
        }

        private void btnProcessInfo_Click(object sender, EventArgs e)
        {
            try
            {
                using(sw = new StreamWriter(pathApp, true))
                {
                    String lineas;

                    tbAmpliado.Text = "";
                    int PIDescrita = Convert.ToInt16(tbLinea.Text.Trim());

                    Process proceso = Process.GetProcessById(PIDescrita);
                    m = (p) => String.Format("Nombre: {0} \r\nId: {1} \r\nHora de comiento: {2} \r\n \r\n", p.ProcessName, p.Id, p.StartTime);
                    tbAmpliado.Text += m(proceso);

                    tbAmpliado.Text += "Subprocesos: \r\n";
                    Array.ForEach(proceso.Threads.Cast<ProcessThread>().ToArray(), hilosMet);

                    tbAmpliado.Text += "Modulos: \r\n";
                    Array.ForEach(proceso.Modules.Cast<ProcessModule>().ToArray(),modulosMet);

                    sw.Write(DateTime.Now + " " + proceso.Id + " Mostrar informacion -> Exito");
                    sw.WriteLine();

                }

            }
            catch(FormatException ex)
            {
                tbAmpliado.Text = "Numero de IP invalido";
            }
            catch(ArgumentException ex)
            {
                using (sw = new StreamWriter(pathApp, true))
                {
                    tbAmpliado.Text = "Numero de IP no encontrado";

                    sw.Write(DateTime.Now + " " + tbLinea.Text.Trim() + " Mostrar informacion -> Fallo");
                    sw.WriteLine();
                }
                
            }
            catch (System.ComponentModel.Win32Exception)
            {
                tbAmpliado.Text = "Acceso al proceso denegado";
            }

        }

        public void hilosMet(ProcessThread pt)
        {
            tbAmpliado.Text += String.Format("Id del subproceso: {0} \r\nHora de comienzo: {1} \r\n \r\n", pt.Id, pt.StartTime);
        }

        public void modulosMet(ProcessModule pm)
        {
            tbAmpliado.Text += String.Format("Nombre modulo: {0} \r\nNombre archivo: {1} \r\n \r\n", pm.ModuleName, pm.FileName);
        }

        private void closeORkill(object sender, EventArgs e)
        {

            try
            {
                using (sw = new StreamWriter(pathApp, true))
                {
                    tbAmpliado.Text = "";
                    int PIDescrita = Convert.ToInt16(tbLinea.Text.Trim());

                    Process proceso = Process.GetProcessById(PIDescrita);

                    if (sender.Equals(btnCloseProces))
                    {
                        sw.Write(DateTime.Now + " " + proceso.Id + " Cerrar -> Exito");
                        sw.WriteLine();
                        proceso.CloseMainWindow();
                        tbAmpliado.Text = "Proceso cerrado con exito";
                    }
                    else if (sender.Equals(btnKillProces))
                    {
                        sw.Write(DateTime.Now + " " + proceso.Id + " Kill -> Exito");
                        sw.WriteLine();
                        proceso.Kill();
                        tbAmpliado.Text = "Proceso cerrado con exito";
                    }
                }
                
            }
            catch (FormatException ex)
            {
              //  using (sw = new StreamWriter(pathApp, true))
                {
                    tbAmpliado.Text = "Numero de IP invalido";
                }
            }
            catch (ArgumentException ex)
            {
                using (sw = new StreamWriter(pathApp, true))
                {
                    tbAmpliado.Text = "Numero de IP no encontrado";

                    if (sender.Equals(btnCloseProces))
                    {
                        sw.Write(DateTime.Now + " " + tbLinea.Text.Trim() + " Cerrar -> Fallo");
                        sw.WriteLine();
                    }
                    else if (sender.Equals(btnKillProces))
                    {
                        sw.Write(DateTime.Now + " " + tbLinea.Text.Trim() + " Kill -> Fallo");
                        sw.WriteLine();
                    }
                }
                
            }
            catch (System.ComponentModel.Win32Exception)
            {
                tbAmpliado.Text = "Acceso al proceso denegado";
            }
        }

        private void btnRunApp_Click(object sender, EventArgs e)
        {
            
            try
            {
                using (sw = new StreamWriter(pathApp, true))
                {
                    tbAmpliado.Text = "";
                    String path = tbLinea.Text.Trim();

                    Process proceso = Process.Start(path);

                    sw.Write(DateTime.Now + " " + proceso.Id + " Run -> Exito");
                    sw.WriteLine();
                }
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                using (sw = new StreamWriter(pathApp, true))
                {
                    tbAmpliado.Text = "Proceso no encontrado";
                    sw.Write(DateTime.Now + " " + tbLinea.Text.Trim() + " Run -> Fallo");
                    sw.WriteLine();
                }
                
            }
            catch(InvalidOperationException ex)
            {
                using (sw = new StreamWriter(pathApp, true))
                {
                    tbAmpliado.Text = "Proceso no encontrado";
                    sw.Write(DateTime.Now + " " + tbLinea.Text.Trim() + " Run -> Fallo");
                    sw.WriteLine();
                }
            }
        }

        private void btnStartWith_Click(object sender, EventArgs e)
        {
            tbAmpliado.Text = "";
            String referencia = tbLinea.Text.Trim();

            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                if(process.ProcessName.Substring(0,referencia.Length).ToLower() == referencia.ToLower() && referencia!="")
                {
                    m = (p) => String.Format("{0}",process.ProcessName);
                    tbAmpliado.Text += m(process)+"\r\n";
                }
            }
        }

    }
}
