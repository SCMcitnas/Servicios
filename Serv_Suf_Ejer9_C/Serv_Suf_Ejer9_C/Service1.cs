using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Serv_Suf_Ejer9_C
{
    public partial class Service1 : ServiceBase
    {
        

        public Service1()
        {
            InitializeComponent();
        }

        public void writeEvent(string mensaje)
        {
            string nombre = "EjerServicio";
            string logDestino = "Application";

            if (!EventLog.SourceExists(nombre))
            {
                EventLog.CreateEventSource(nombre, logDestino);
            }

            EventLog.WriteEntry(nombre, mensaje);
        }

        //Se ejecuta cuando inicia el servicio
        protected override void OnStart(string[] args)
        {
        }
        //Se ejecuta cuando para el servicio
        protected override void OnStop()
        {
        }
        //Se ejecuta cuando pausa el servicio
        protected override void OnPause()
        {
        }
        //Se ejecuta cuando continua tras la pausa el servicio
        protected override void OnContinue()
        {
        }
    }
}
