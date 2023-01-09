using Amazon.S3;
using Amazon.S3.Model;
using AWSS3_UEM.Clases;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AWSS3_UEM.Ventanas
{
    /// <summary>
    /// Lógica de interacción para AltaObjeto.xaml
    /// </summary>
    public partial class AltaObjeto : Window
    {
        private AmazonS3Client _s3Client;
        private S3Bucket _BKT;
        private string _rutaCompletaObjeto = string.Empty;
        List<Clases.Metadata> listaMetadatos = new List<Clases.Metadata>();

        /// <summary>
        /// Constructor de la clase AltaObjeto
        /// </summary>
        /// <param name="mis3Client">Conexión cliente</param>
        /// <param name="miBKT">Nombre del bucket donde dar de alta los objetos</param>
        public AltaObjeto(AmazonS3Client mis3Client, S3Bucket miBKT)
        {
            InitializeComponent();
            lblBKT.Content = "BUCKET ACTIVO:  " + miBKT.BucketName;

            txtMeta.IsEnabled = false;
            txtValue.IsEnabled = false;
            dtGridMetas.IsEnabled = false;
            btnAddMeta.IsEnabled = false;
            btnDelMeta.IsEnabled = false;
            btnEnviar.IsEnabled = false;
            _s3Client = mis3Client;
            _BKT = miBKT;
        }

        /// <summary>
        /// Botón para seleccionar una imagen con el explorador de ficheros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            string rutaImagen = String.Empty;
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "Imagen (.jpg)|*.jpg";

            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                _rutaCompletaObjeto = dlg.FileName;
                txtRutaObjeto.Text = dlg.SafeFileName;
                txtMeta.IsEnabled = true;
                txtValue.IsEnabled = true;
                dtGridMetas.IsEnabled = true;
                btnAddMeta.IsEnabled = true;
                btnDelMeta.IsEnabled = true;
                btnEnviar.IsEnabled = true;
            }
        }

        /// <summary>
        /// Botón para realizar la subida del objeto a la nube
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnEnviar_ClickAsync(object sender, RoutedEventArgs e)
        {
            await s3Objeto.EnviaObjetoAsync(_s3Client,_BKT.BucketName, txtRutaObjeto.Text, _rutaCompletaObjeto, "image/jpeg", listaMetadatos);
            this.Close();
        }

        /// <summary>
        /// Botón que añade una pareja clave-valor a la lista de metadatos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddMeta_Click(object sender, RoutedEventArgs e)
        {
            dtGridMetas.ItemsSource = null;
            Clases.Metadata miMT = new Clases.Metadata(txtMeta.Text, txtValue.Text);
            listaMetadatos.Add(miMT);

            dtGridMetas.ItemsSource = listaMetadatos;
            txtMeta.Text = String.Empty;
            txtValue.Text = String.Empty;
        }

        /// <summary>
        /// Botón que elimina una pareja clave-valor a la lista de metadatos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelMeta_Click(object sender, RoutedEventArgs e)
        {
            Clases.Metadata miMT = (Clases.Metadata)dtGridMetas.SelectedItem;
            listaMetadatos.Remove(miMT);
            dtGridMetas.ItemsSource = null;
            dtGridMetas.ItemsSource = listaMetadatos;
        }
    }
}
