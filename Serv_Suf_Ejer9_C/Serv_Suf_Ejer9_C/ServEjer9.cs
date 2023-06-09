﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serv_Suf_Ejer9_C
{
    public partial class ServEjer9 : ServiceBase
    {

        public ServEjer9()
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

        Hilo hilo = new Hilo();

        //Se ejecuta cuando inicia el servicio
        protected override void OnStart(string[] args)
        {
            writeEvent("Running OnStart");
            
            Thread thread = new Thread(hilo.Init);
            thread.Start();
        }
        //Se ejecuta cuando para el servicio
        protected override void OnStop()
        {
            writeEvent("Servicio detenido");

            hilo.termina = true;
            hilo.s.Close();
        }
        //Se ejecuta cuando pausa el servicio
        protected override void OnPause()
        {
            writeEvent("Servicio pausado");
        }
        //Se ejecuta cuando continua tras la pausa el servicio
        protected override void OnContinue()
        {
            writeEvent("Continuar servicio");
        }
    }
}
