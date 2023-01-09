using Amazon.S3.Model;
using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Forms;

using Amazon;
using System.IO;
using System.Diagnostics;
using MessageBox = System.Windows.MessageBox;
using AWSS3_UEM.Clases;

namespace AWSS3_UEM.Ventanas
{
    /// <summary>
    /// Lógica de interacción para Gestor_datos.xaml
    /// </summary>
    public partial class Gestor_datos : Window
    {
        AmazonS3Client s3Client;
        List<S3Bucket> listabucket = new List<S3Bucket>();
        public Gestor_datos()
        {
            InitializeComponent();
            dtGridBKTS.ItemsSource = listabucket;
            dtGridOBJS.ItemsSource = null;
            dtGridMetadata.ItemsSource = null;
            s3Client = new AmazonS3Client(Global.UserID, Global.UserKey, RegionEndpoint.EUWest3);
            

            Task response = CargaBuckets();
        }

        #region ----- BUCKETS -----
        /// <summary>
        /// Rellena la ventana de onjetos al cambiar de bucket
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtGridBKTS_SelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            Task cargaMetadataResponse = CargaListaOBJSAsync();
            dtGridMetadata.ItemsSource = null;
        }

        /// <summary>
        /// Crear un nuevo bucket
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnCreaBKT_Click(object sender, RoutedEventArgs e)
        {
            if (txtNombreNuevoBucket.Text != String.Empty)
            {
                if (txtNombreNuevoBucket.Text.Length < 3 || txtNombreNuevoBucket.Text.Length > 63)
                {
                    Global.MensajeFeedback("El nombre del Bucket debe tener entre 3 y 63 caracteres");
                    return;
                }
                PutBucketResponse tskCreateResponse = await Clases.s3Contenedor.CrearBucket(s3Client, txtNombreNuevoBucket.Text);
                Task response = CargaBuckets();
                txtNombreNuevoBucket.Text = String.Empty;
            }
        }

        /// <summary>
        /// Botón para eliminar un Bucket seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnEliminaBKT_Click(object sender, RoutedEventArgs e)
        {
            ///TO-DO: Asegurar que no hay objetos en el interior

            S3Bucket miBKT = (S3Bucket)dtGridBKTS.SelectedItem;
            if (miBKT == null)
            {
                Global.MensajeFeedback("Debe seleccionar un Bucket");
                return;
            }
            List<S3Object> listaOBJs = (List<S3Object>)dtGridOBJS.ItemsSource;
            if (listaOBJs.Count > 0)
            {
                Global.MensajeFeedback("Debe eliminar primero todos los objetos");
                return;
            }

            if (MessageBox.Show(String.Format("¿Realmente quiere eliminar el Bucket: {0}", miBKT.BucketName),
                    "Confirmación!",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DeleteBucketResponse tskDeleteResponse = await Clases.s3Contenedor.EliminarBucket(s3Client, miBKT.BucketName);
                Task response = CargaBuckets();
            }
        }
        
        /// <summary>
        /// Carga el listado de buckets en la ventana
        /// </summary>
        /// <returns></returns>
        private async Task CargaBuckets()
        {
            ListBucketsResponse cargaBKTSResponse = await Clases.s3Contenedor.DameListaBucketsAsync(s3Client);
            dtGridBKTS.ItemsSource = cargaBKTSResponse.Buckets;
        }

        #endregion

        #region ------ OBJETOS ------
        /// <summary>
        /// Rellena metadatos al cambiar de objeto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtGridOBJS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Task cargaMetadataResponse = CargaMetadataObjAsync();
        }

        /// <summary>
        /// Carga lista de objetos
        /// </summary>
        /// <returns></returns>
        private async Task CargaListaOBJSAsync()
        {
            S3Bucket miBKT = (S3Bucket)dtGridBKTS.SelectedItem;
            if (miBKT != null)
            {
                ListObjectsResponse listaObjetos = await Clases.s3Objeto.DameListaOBJsAsync(s3Client, miBKT.BucketName);
                dtGridOBJS.ItemsSource = listaObjetos.S3Objects;
            }
        }

        /// <summary>
        /// Botón Alta de objeto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAltaOBJ_Click(object sender, RoutedEventArgs e)
        {
            S3Bucket miBKT = (S3Bucket)dtGridBKTS.SelectedItem;
            if (miBKT == null)
            {
                Global.MensajeFeedback("Debe seleccionar un Bucket");
                return;
            }
            Ventanas.AltaObjeto miVentana = new Ventanas.AltaObjeto(s3Client, miBKT);
            miVentana.Show();
        }

        /// <summary>
        /// Botón Eliminar objeto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnEliminaOBJ_Click(object sender, RoutedEventArgs e)
        {
            S3Bucket miBKT = (S3Bucket)dtGridBKTS.SelectedItem;
            if (miBKT == null)
            {
                Global.MensajeFeedback("Debe seleccionar un Bucket");
                return;
            }
            S3Object miOBJ = (S3Object)dtGridOBJS.SelectedItem;
            if (miOBJ == null)
            {
                Global.MensajeFeedback("Debe seleccionar un Objeto");
                return;
            }
            DeleteObjectResponse result = await Clases.s3Objeto.EliminarOBJAsync(s3Client, miBKT.BucketName, miOBJ.Key);
            Task response = CargaListaOBJSAsync();
        }
        #endregion

        #region ----- METADATA -----
        /// <summary>
        /// Carga lista de mtadatos de un objeto
        /// </summary>
        /// <returns></returns>
        private async Task CargaMetadataObjAsync()
        {
            S3Bucket miBKT = (S3Bucket)dtGridBKTS.SelectedItem;
            if (miBKT == null)
            {
                Global.MensajeFeedback("Debe seleccionar un Bucket");
                return;
            }
            S3Object miOBJ = (S3Object)dtGridOBJS.SelectedItem;
            if (miOBJ == null)
            {
                //Global.MensajeFeedback("Debe seleccionar un Objeto");
                return;
            }

            List<Clases.Metadata> listaMetadata = await Clases.Metadata.DameMetadataAsync(s3Client, miBKT.BucketName, miOBJ.Key);
            dtGridMetadata.ItemsSource = listaMetadata;
        }
        #endregion

        /// <summary>
        /// Doble click en un objeto, abre el objeto asociado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void dtGridOBJS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            S3Bucket miBKT = (S3Bucket)dtGridBKTS.SelectedItem;
            if (miBKT == null)
            {
                Global.MensajeFeedback("Debe seleccionar un Bucket");
                return;
            }
            S3Object miOBJ = (S3Object)dtGridOBJS.SelectedItem;
            if (miOBJ == null)
            {
                //Global.MensajeFeedback("Debe seleccionar un Objeto");
                return;
            }
            await Clases.s3Objeto.MuestraImagenObjeto(s3Client, miBKT.BucketName, miOBJ.Key);
        }

    }
}
