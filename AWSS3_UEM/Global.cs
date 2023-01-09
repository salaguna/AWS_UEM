using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace AWSS3_UEM
{
    internal class Global
    {
        public static string UserID = "*****";
        public static string UserKey = "**********";
        public static string rutaTemporal = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\TMPAWSS3\\";

        /// <summary>
        /// Abre un objeto con la aplicación por defecto del SO
        /// </summary>
        /// <param name="path"></param>
        public static void AbrirConAppPorDefecto(string path)
        {
            using (Process fileopener = new Process()){

                fileopener.StartInfo.FileName = "explorer";
                fileopener.StartInfo.Arguments = "\"" + path + "\"";
                fileopener.Start();
            }
        }

        /// <summary>
        /// Muestra un mensaje por pantalla
        /// </summary>
        /// <param name="mensaje"></param>
        /// <param name="aviso"></param>
        /// <param name="miBoton"></param>
        /// <param name="miImagen"></param>
        public static void MensajeFeedback(string mensaje, string aviso="TFM Sergio A.L. Info"
                                            ,MessageBoxButton miBoton = MessageBoxButton.OK
                                            ,MessageBoxImage miImagen = MessageBoxImage.Information)
        {
            MessageBox.Show(mensaje, aviso, miBoton, miImagen);
        }

    }
}
