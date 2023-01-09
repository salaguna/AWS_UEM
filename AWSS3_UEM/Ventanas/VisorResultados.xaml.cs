using Amazon;
using Amazon.S3;
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
    /// Lógica de interacción para VisorResultados.xaml
    /// </summary>
    public partial class VisorResultados : Window
    {
        public VisorResultados(List<Clases.ResultadoMaquina> misResultados)
        {
            InitializeComponent();
            dtGridResultados.ItemsSource = misResultados;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Doble click en un resultado, abre el objeto asociado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void dtGridResultados_MouseDoubleClickAsync(object sender, MouseButtonEventArgs e)
        {
            AmazonS3Client s3Client = new AmazonS3Client(Global.UserID, Global.UserKey, RegionEndpoint.EUWest3);
            ResultadoMaquina miResultado = (ResultadoMaquina)dtGridResultados.SelectedItem;
            if (miResultado != null)
                await Clases.s3Objeto.MuestraImagenObjeto(s3Client, "tfmsal-planta"+miResultado.planta, miResultado.id+".jpg");
        }
    }
}
