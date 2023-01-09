using System;
using System.Collections.Generic;
using System.Text;

namespace AWSS3_UEM.Clases
{
    internal class Estacion
    {
        public string Marca { get; set; }
        public string Id { get; set; }
        public string Nombre { get; set; }
        public static List<Estacion> DameListaEstaciones()
        {
            List<Estacion> listaEstaciones = new List<Estacion>();
            Estacion miEstacion = new Estacion();
            miEstacion.Id = "1";
            miEstacion.Nombre = "Estación 1";
            listaEstaciones.Add(miEstacion);
            miEstacion = new Estacion();
            miEstacion.Id = "2";
            miEstacion.Nombre = "Estación 2";
            listaEstaciones.Add(miEstacion);
            miEstacion = new Estacion();
            miEstacion.Id = "3";
            miEstacion.Nombre = "Estación 3";
            listaEstaciones.Add(miEstacion);
            miEstacion = new Estacion();
            miEstacion.Id = "T";
            miEstacion.Nombre = "TODAS";
            listaEstaciones.Add(miEstacion);

            return listaEstaciones;
        }
    }
}
