using Amazon.S3.Model;
using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AWSS3_UEM.Clases
{
    internal class s3Contenedor
    {
        /// <summary>
        /// Devuelve la lista de Buckets del cliente dado como argumento
        /// </summary>
        /// <param name="s3Client">Conexión cliente</param>
        /// <returns></returns>
        public static async Task<ListBucketsResponse> DameListaBucketsAsync(AmazonS3Client s3Client)
        {
            return await s3Client.ListBucketsAsync();            
        }

        /// <summary>
        /// Crea en la cuenta de cliente dado como argumento el bucket con nombre dado en el segundo argumento
        /// </summary>
        /// <param name="s3Client">Conexión cliente</param>
        /// <param name="nombreBucket">Nombre del bucket</param>
        /// <returns></returns>
        public static async Task<PutBucketResponse> CrearBucket(AmazonS3Client s3Client, string nombreBucket)
        {
            return await s3Client.PutBucketAsync(nombreBucket.Trim().ToLower());
        }

        /// <summary>
        /// Elimina de la cuenta del cliente dado como argumento el bucket con nombre dado en el segundo argumento
        /// </summary>
        /// <param name="s3Client">Conexión cliente</param>
        /// <param name="nombreBucket">Nombre del bucket</param>
        /// <returns></returns>
        public static async Task<DeleteBucketResponse> EliminarBucket(AmazonS3Client s3Client, string nombreBucket)
        {
            return await s3Client.DeleteBucketAsync(nombreBucket);
        }
    }
}
