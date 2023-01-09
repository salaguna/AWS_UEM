using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using AWSS3_UEM.Clases;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AWSS3_UEM.Ventanas
{
    /// <summary>
    /// Lógica de interacción para Carga_masiva.xaml
    /// </summary>
    public partial class Carga_masiva : Window
    {
        string[] listaJsonGlobal;
        AmazonS3Client s3ClientGlobal;
        public Carga_masiva()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Seleccionar carpeta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSeleccion_Click(object sender, RoutedEventArgs e)
        {
            s3ClientGlobal = new AmazonS3Client(Global.UserID, Global.UserKey, RegionEndpoint.EUWest3);
            //Seleccionar carpeta
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.ShowDialog();
            if (dlg.SelectedPath == String.Empty)
                return;

            string rutaCarpeta = dlg.SelectedPath;

            // Obtener archivos
            listaJsonGlobal = Directory.GetFiles(rutaCarpeta, "*.json");
            List<Fichero> listaFicheros = new List<Fichero>();
            foreach (string item in listaJsonGlobal)
            {
                Fichero fichero = new Fichero();
                fichero.Nombre = System.IO.Path.GetFileName(item);
                listaFicheros.Add(fichero);
            }
            dtGrisFicheros.ItemsSource = listaFicheros;
        }       

        /// <summary>
        /// Botón envío de objetos a la nube
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnEnviarFicheros_ClickAsync(object sender, RoutedEventArgs e)
        {
            await s3Objeto.EnvíaFicheros_JsonJPG_S3(s3ClientGlobal, listaJsonGlobal);
        }
    }
}
