using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSS3_UEM.Clases
{
    internal class Metadata
    {
        public string Key { get; set; }
        public string Valor { get; set; }

        /// <summary>
        /// Constructor de Metadato vacío
        /// </summary>
        public Metadata()
        {
            Key = string.Empty;
            Valor = string.Empty;
        }

        /// <summary>
        /// Constructor de metadato cargado con parámetros
        /// </summary>
        /// <param name="key">ID clave del metadato</param>
        /// <param name="valor">Valor del metadato</param>
        public Metadata(string key, string valor)
        {
            Key = key;
            Valor = valor;
        }

        /// <summary>
        /// Devuelve una lista de metadatos de un objeto
        /// </summary>
        /// <param name="s3Client">Conexión cliente</param>
        /// <param name="nombreBucket">Nombre del bucket donde se aloja el objeto</param>
        /// <param name="keyOBJ">ID del objeto que contiene los metadatos</param>
        /// <returns></returns>
        public static async Task<List<Metadata>> DameMetadataAsync(AmazonS3Client s3Client, string nombreBucket, string keyOBJ)
        {
            List<Clases.Metadata> listaMetadata = new List<Clases.Metadata>();
            using (GetObjectResponse response = await s3Client.GetObjectAsync(nombreBucket, keyOBJ))
            {
                foreach (string item in response.Metadata.Keys)
                {
                    Clases.Metadata miMD = new Clases.Metadata(item, response.Metadata[item]);
                    listaMetadata.Add(miMD);
                }
            }
            return listaMetadata;
        }
    }
}
