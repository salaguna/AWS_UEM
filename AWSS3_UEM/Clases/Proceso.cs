using System;
using System.Collections.Generic;
using System.Text;

namespace AWSS3_UEM.Clases
{
    internal class Proceso
    {
        public string Marca { get; set; }
        public string Id { get; set; }
        public string Nombre { get; set; }
        public static List<Proceso> DameListaProcesos()
        {
            List<Proceso> listaEstaciones = new List<Proceso>();
            Proceso miProceso = new Proceso();
            miProceso.Id = "Foto1";
            miProceso.Nombre = "Foto1";
            listaEstaciones.Add(miProceso);
            miProceso = new Proceso();
            miProceso.Id = "Foto2";
            miProceso.Nombre = "Foto2";
            listaEstaciones.Add(miProceso);
            miProceso = new Proceso();
            miProceso.Id = "Foto3";
            miProceso.Nombre = "Foto3";
            listaEstaciones.Add(miProceso);
            miProceso = new Proceso();
            miProceso.Id = "TFM";
            miProceso.Nombre = "TFM";
            listaEstaciones.Add(miProceso);
            miProceso = new Proceso();
            miProceso.Id = "T";
            miProceso.Nombre = "TODAS";
            listaEstaciones.Add(miProceso);

            return listaEstaciones;
        }
    }
}
