using System;
using System.Collections.Generic;
using System.Text;

namespace AWSS3_UEM.Clases
{
    public class ResultadoMaquina
    {
        public string id{ get; set; }
        public string planta { get; set; }
        public string linea { get; set; }
        public string estacion { get; set; }
        public string proceso { get; set; }
        public string hora { get; set; }
        public string resultado { get; set; }
        public int year { get; set; }
        public int mes { get; set; }
        public int dia { get; set; }
    }
}
