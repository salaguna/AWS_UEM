using System;
using System.Collections.Generic;
using System.Text;

namespace AWSS3_UEM.Clases
{
    internal class Linea
    {
        public string Id { get; set; }
        public string Nombre { get; set; }

        public static List<Linea> DameListaLineas()
        {
            List<Linea> listaLineas = new List<Linea>();
            Linea miLinea = new Linea();
            miLinea.Id = "1";
            miLinea.Nombre = "Linea 1";
            listaLineas.Add(miLinea);
            miLinea = new Linea();
            miLinea.Id = "2";
            miLinea.Nombre = "Linea 2";
            listaLineas.Add(miLinea);
            miLinea = new Linea();
            miLinea.Id = "3";
            miLinea.Nombre = "Linea 3";
            listaLineas.Add(miLinea);

            return listaLineas;
        }
    }
}
