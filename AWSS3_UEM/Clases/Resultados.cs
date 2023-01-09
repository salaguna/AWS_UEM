using System;
using System.Collections.Generic;
using System.Text;

namespace AWSS3_UEM.Clases
{
    internal class Resultados
    {
        public string Marca { get; set; }
        public string Nombre { get; set; }
        public static List<Resultados> DameListaResultados()
        {
            List<Resultados> listaResultados = new List<Resultados>();
            Resultados miResultado = new Resultados();
            miResultado.Nombre = "OK";
            listaResultados.Add(miResultado);
            miResultado = new Resultados();
            miResultado.Nombre = "KO";
            listaResultados.Add(miResultado);
            miResultado = new Resultados();
            miResultado.Nombre = "TODOS";
            listaResultados.Add(miResultado);

            return listaResultados;
        }
    }
}
