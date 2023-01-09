using Amazon.S3.Model;
using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Controls;

namespace AWSS3_UEM.Clases
{
    internal class s3Objeto
    {
        /// <summary>
        /// Devuelve la lista de objetos alojados en un bucket
        /// </summary>
        /// <param name="s3Client">Conexión cliente</param>
        /// <param name="bucketName">Nombre del bucket donde buscar los objetos</param>
        /// <returns></returns>
        public static async Task<ListObjectsResponse> DameListaOBJsAsync(AmazonS3Client s3Client, string bucketName)
        {
            return await s3Client.ListObjectsAsync(bucketName);
        }

        /// <summary>
        /// Crea en la cuenta de cliente un objeto
        /// </summary>
        /// <param name="s3Client">Conexión cliente</param>
        /// <param name="miRequest">Petición con el objeto configurado</param>
        /// <returns></returns>
        public static async Task<PutObjectResponse> CrearOBJAsync(AmazonS3Client s3Client, PutObjectRequest miRequest)
        {
            return await s3Client.PutObjectAsync(miRequest);
        }

        /// <summary>
        /// Elimina de la cuenta de cliente un objeto
        /// </summary>
        /// <param name="s3Client">Conexión cliente</param>
        /// <param name="nombreBucket">Nombre del bucket desde donde eliminar el objeto</param>
        /// <param name="keyOBJ">ID del objeto a eliminar</param>
        /// <returns></returns>
        public static async Task<DeleteObjectResponse> EliminarOBJAsync(AmazonS3Client s3Client, string nombreBucket, string keyOBJ)
        {
            return await s3Client.DeleteObjectAsync(nombreBucket,keyOBJ);
        }

        /// <summary>
        /// Descarga un objeto desde la cuenta cliente
        /// </summary>
        /// <param name="s3Client">Conexión cliente</param>
        /// <param name="nombreBucket">Nombre del bucket desde donde descargar el objeto</param>
        /// <param name="keyOBJ">ID del objeto a descargar</param>
        /// <returns></returns>
        public static async Task<GetObjectResponse> DameOBJAsync(AmazonS3Client s3Client, string nombreBucket, string keyOBJ)
        {
            return await s3Client.GetObjectAsync(nombreBucket, keyOBJ);
        }

        /// <summary>
        /// Descarga un objeto
        /// </summary>
        /// <param name="s3Client">Conexión cliente</param>
        /// <param name="nombreBucket">Nombre del bucket desde donde descargar el objeto</param>
        /// <param name="keyOBJ">ID del objeto a descargar</param>
        /// <param name="ruta">Ruta en la que guardar el objeto descargado</param>
        /// <returns></returns>
        public static async Task DescargaOBJAsync(AmazonS3Client s3Client, string nombreBucket, string keyOBJ, string ruta)
        {
            try
            {
                using (var response = await s3Objeto.DameOBJAsync(s3Client, nombreBucket, keyOBJ))
                {
                    using (FileStream ms = new FileStream(ruta, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        await response.ResponseStream.CopyToAsync(ms);
                        ms.Position = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.MensajeFeedback("Lo sentimos, no se pudo encontrar el fichero en la nube", "Atención!", MessageBoxButton.OK, MessageBoxImage.Warning);
                //Global.MensajeFeedback("Error con el fichero: " + ex.ToString());
            }
        }

        /// <summary>
        /// Envía un objeto a la nube sin metadatos
        /// </summary>
        /// <param name="s3Client">Conexión cliente</param>
        /// <param name="nombreBucket">Nombre del bucket desde donde descargar el objeto</param>
        /// <param name="keyOBJ">ID del objeto a descargar</param>
        /// <param name="rutaFichero">Ruta donde está almacenado el objeto a enviar</param>
        /// <param name="miContentType">Tipo de objeto, por ejemplo "image/jpeg"</param>
        /// <returns></returns>
        //public static async Task EnviObJAsync(AmazonS3Client s3Client, string miNombreBucket, string miKey, string rutaFichero, string miContentType)
        //{
        //    try
        //    {
        //        var putRequest2 = new PutObjectRequest
        //        {
        //            BucketName = miNombreBucket,
        //            Key = miKey,
        //            FilePath = rutaFichero,
        //            ContentType = miContentType,
        //        };

        //        PutObjectResponse response2 = await Clases.s3Objeto.CrearOBJAsync(s3Client, putRequest2);
        //    }
        //    catch (AmazonS3Exception e)
        //    {
        //        Global.MensajeFeedback("Error encountered ***. Message:'{0}' when writing an object", e.Message);
        //    }
        //    catch (Exception e)
        //    {
        //        Global.MensajeFeedback("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
        //    }
        //}

        /// <summary>
        /// Envía una imagen a la nube
        /// </summary>
        /// <param name="s3Client">Conexión cliente</param>
        /// <param name="nombreBucket">Nombre del bucket desde donde descargar el objeto</param>
        /// <param name="keyOBJ">ID del objeto a descargar</param>
        /// <param name="rutaObjeto">Ruta donde está almacenado el objeto a enviar</param>
        /// <param name="miContentType">Tipo de objeto, por ejemplo "image/jpeg"</param>
        /// <returns></returns>
        public static async Task EnviaObjetoAsync(AmazonS3Client s3Client, string miBKTname, string miKey,string rutaObjeto, string contentType, List<Metadata>? listaMetadatos)
        {
            try
            {
                //Put the object-set ContentType and add metadata.
                //Objeto y Metadata definido por el SISTEMA
                var putRequest2 = new PutObjectRequest
                {
                    BucketName = miBKTname,
                    Key = miKey,
                    FilePath = rutaObjeto,
                    ContentType = contentType,
                };

                if (listaMetadatos != null)
                {
                    //Metadata definido por el USUARIO
                    foreach (Clases.Metadata miMD in listaMetadatos)
                    {
                        putRequest2.Metadata.Add(miMD.Key, miMD.Valor);
                    }
                }
                PutObjectResponse response2 = await Clases.s3Objeto.CrearOBJAsync(s3Client, putRequest2);
                
            }
            catch (AmazonS3Exception e)
            {
                Global.MensajeFeedback("Error encountered ***. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Global.MensajeFeedback("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }

        /// <summary>
        /// Añadir a la BBDD en la nube los datos de varios ficheros JSON y adjuntar también el objeto.
        /// </summary>
        /// <param name="s3Cliente">Conexión cliente</param>
        /// <param name="listaJson">Lista de los ficheros a unir a la base de datos</param>
        /// <returns></returns>
        public static async Task EnvíaFicheros_JsonJPG_S3(AmazonS3Client s3Cliente, string[] listaJson)
        {
            //Por cada fichero a subir debo encontrar el JSON de planta correspondiente
            //El patron del nombre del fichero comienza con: PLANTA_LINEA
            //El fichero en la nube cumple el patrón PLANTA_LINEA.JSON
            bool error = false;
            if (listaJson.Length == 0)
            {
                Global.MensajeFeedback("No se han seleccionado ficheros");
                return;
            }
            foreach (string ficheroJson in listaJson)
            {
                try
                {
                    string nombreFichero = Path.GetFileName(ficheroJson);
                    string miFabrica = "tfmsal-planta" + nombreFichero.Substring(0, 1);
                    string miFabrica_Linea_BBDD = nombreFichero.Substring(0, 3) + ".json";
                    List<Clases.ResultadoMaquina> miFabrica_Linea_BBDDDeserialized;
                    Clases.ResultadoMaquina objetoDeserialized;
                    string DescargaBBDDTMP = Global.rutaTemporal + miFabrica_Linea_BBDD;
                    string rutaCompletaJPG = ficheroJson.Substring(0, (ficheroJson.Length - 5)) + ".jpg";

                    //Descarga de la BBDD
                    await Clases.s3Objeto.DescargaOBJAsync(s3Cliente, miFabrica, miFabrica_Linea_BBDD, DescargaBBDDTMP);

                    //Deserializar la BBDD
                    miFabrica_Linea_BBDDDeserialized = Clases.Json.JsonToListaObjetos(DescargaBBDDTMP);
                    //Deserializar el nuevo fichero de resultados
                    objetoDeserialized = Clases.Json.JsonToObjeto(ficheroJson);

                    //Añadir a la BBDD el contenido del fichero
                    miFabrica_Linea_BBDDDeserialized.Add(objetoDeserialized);

                    //Serializar la BBDD
                    string ficheroSerializado = JsonConvert.SerializeObject(miFabrica_Linea_BBDDDeserialized);
                    System.IO.File.WriteAllText(DescargaBBDDTMP, ficheroSerializado);

                    //Volver a enviar la BBDD fichero a la nube
                    await Clases.s3Objeto.EnviaObjetoAsync(s3Cliente, miFabrica, miFabrica_Linea_BBDD, DescargaBBDDTMP, "application/json", null);

                    //SUbir también la imagen asociada
                    var putRequest2 = new PutObjectRequest
                    {
                        BucketName = miFabrica,
                        Key = Path.GetFileName(rutaCompletaJPG),
                        FilePath = rutaCompletaJPG,
                        ContentType = "image/jpeg",
                    };
                    putRequest2.Metadata.Add("id", objetoDeserialized.id);
                    putRequest2.Metadata.Add("planta", objetoDeserialized.planta);
                    putRequest2.Metadata.Add("linea", objetoDeserialized.linea);
                    putRequest2.Metadata.Add("estacion", objetoDeserialized.estacion);
                    putRequest2.Metadata.Add("proceso", objetoDeserialized.proceso);
                    putRequest2.Metadata.Add("hora", objetoDeserialized.hora);
                    putRequest2.Metadata.Add("resultado", objetoDeserialized.resultado);
                    putRequest2.Metadata.Add("year", objetoDeserialized.year.ToString());
                    putRequest2.Metadata.Add("mes", objetoDeserialized.mes.ToString());
                    putRequest2.Metadata.Add("dia", objetoDeserialized.dia.ToString());

                    PutObjectResponse response2 = await Clases.s3Objeto.CrearOBJAsync(s3Cliente, putRequest2);
                    File.Delete(DescargaBBDDTMP);
                }
                catch
                {
                    error = true;
                }
            }
            if (error)
                Global.MensajeFeedback("Se produjo un error al enviar los datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                Global.MensajeFeedback("Se han enviado los datos :)");
        }

        /// <summary>
        /// Descarga y muestra un objeto
        /// </summary>
        /// <param name="s3Client">Conexión cliente</param>
        /// <param name="nombreBucket">Nombre del bucket desde donde descargar el objeto</param>
        /// <param name="keyOBJ">ID del objeto a descargar</param>
        /// <returns></returns>
        public static async Task MuestraImagenObjeto(AmazonS3Client s3Client, string miBucketName, string keyOBJ)
        {
            string rutaImagen = Global.rutaTemporal + keyOBJ;
            try
            {
                await Clases.s3Objeto.DescargaOBJAsync(s3Client, miBucketName, keyOBJ, rutaImagen);
                if (File.Exists(rutaImagen))
                {
                    Global.AbrirConAppPorDefecto(rutaImagen);
                }
            }
            catch
            {
                Global.MensajeFeedback("Lo sentimos, no se pudo encontrar el fichero en la nube","Atención!",MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
