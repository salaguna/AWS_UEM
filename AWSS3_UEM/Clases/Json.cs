using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AWSS3_UEM.Clases
{
    internal class Json
    {
        /// <summary>
        /// Convierte un fichero JSON en una lista de objetos tipo ResultadoMaquina
        /// </summary>
        /// <param name="rutaFichero">Ruta del fichero JSON</param>
        /// <returns></returns>
        public static List<ResultadoMaquina> JsonToListaObjetos(string rutaFichero)
        {
            List<ResultadoMaquina> bbddMaquinaSerialized;
            using (StreamReader jsonStream = File.OpenText(rutaFichero))
            {
                //Serializar la BBDD
                var json = jsonStream.ReadToEnd();
                bbddMaquinaSerialized = JsonConvert.DeserializeObject<List<Clases.ResultadoMaquina>>(json);
            }

            return bbddMaquinaSerialized;
        }

        /// <summary>
        /// Convierte un fichero JSON en un objeto tipo ResultadoMaquina
        /// </summary>
        /// <param name="rutaFichero">Ruta del fichero JSON</param>
        /// <returns></returns>
        public static ResultadoMaquina JsonToObjeto(string rutaFichero)
        {
            ResultadoMaquina fileMaquinaSerialized;
            using (StreamReader jsonStream = File.OpenText(rutaFichero))
            {
                //Serializar la BBDD
                var json = jsonStream.ReadToEnd();
                fileMaquinaSerialized = JsonConvert.DeserializeObject<Clases.ResultadoMaquina>(json);
            }

            return fileMaquinaSerialized;
        }
    }
}
