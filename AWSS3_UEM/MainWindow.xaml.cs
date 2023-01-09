using Amazon.S3.Model;
using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Amazon;
using System.IO;
using System.Collections.ObjectModel;

namespace AWSS3_UEM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                System.IO.Directory.CreateDirectory(Global.rutaTemporal);
            }
            catch
            {
                Global.MensajeFeedback("No se pudo crear la carpeta de trabajo temporal");
                return;
            }
        }

        private void menuGestorDatos_Click(object sender, RoutedEventArgs e)
        {
            Ventanas.Gestor_datos miVentana = new Ventanas.Gestor_datos();
            miVentana.Show();
        }

        private void menuConsultas_Click(object sender, RoutedEventArgs e)
        {
            Ventanas.Consultas miVentana = new Ventanas.Consultas();
            miVentana.Show();
        }

        private void menuCargaMasiva_Click(object sender, RoutedEventArgs e)
        {
            Ventanas.Carga_masiva miVentana = new Ventanas.Carga_masiva();
            miVentana.Show();
        }
    }
}
