using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using AWSS3_UEM.Clases;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AWSS3_UEM.Ventanas
{
    /// <summary>
    /// Lógica de interacción para Consultas.xaml
    /// </summary>
    public partial class Consultas : Window
    {
        AmazonS3Client s3Client;
        List<S3Bucket> listabucket = new List<S3Bucket>();

        /// <summary>
        /// Configura la pantalla de consulta con los datos iniciales
        /// </summary>
        public Consultas()
        {
            InitializeComponent();
            dtGridFabrica.ItemsSource = listabucket;
            dtGridLinea.ItemsSource = null;
            dtGridEstacion.ItemsSource = null;
            dtGridResultado.ItemsSource = null;
            s3Client = new AmazonS3Client(Global.UserID, Global.UserKey, RegionEndpoint.EUWest3);

            Task response = CargaBuckets();
            dtGridLinea.ItemsSource = Linea.DameListaLineas();
            dtGridEstacion.ItemsSource = Estacion.DameListaEstaciones();
            dtGridResultado.ItemsSource = Resultados.DameListaResultados();
            dtGridProceso.ItemsSource = Proceso.DameListaProcesos();

            //De momento oculto la hora
            lblHoraInicio.Visibility = Visibility.Hidden;
            lblHoraFin.Visibility = Visibility.Hidden;
            txtHoraInicio.Visibility = Visibility.Hidden;
            txtHoraFin.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Configura y lanza la consulta al S3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnBuscar_ClickAsync(object sender, RoutedEventArgs e)
        {
            S3Bucket miPlantaOBJ = (S3Bucket)dtGridFabrica.SelectedItem;
            if (miPlantaOBJ == null)
            {
                Global.MensajeFeedback("Debe seleccionar una FABRICA");
                return;
            }
            Linea miLinea = (Linea)dtGridLinea.SelectedItem;
            if (miLinea == null)
            {
                Global.MensajeFeedback("Debe seleccionar una LINEA");
                return;
            }

            AmazonS3Client s3Client = new AmazonS3Client(Global.UserID, Global.UserKey, RegionEndpoint.EUWest3);
            var endWaitHandle = new AutoResetEvent(false);
            List<ResultadoMaquina> resultados = new List<ResultadoMaquina>();
            ResultadoMaquina resultadosTemp;
 
            string miPlanta = miPlantaOBJ.BucketName;
            string nombreBBDD = miPlanta.Substring(13, 1) + "_" + miLinea.Id + ".json";
            
            string sWhereEstaciones = string.Empty;
            string sWhereResultados = string.Empty;
            string sWhereProcesos = string.Empty;
            List<Estacion> misEstacionesObj = (List<Estacion>)dtGridEstacion.ItemsSource;
            List<Resultados> misResultadosObj = (List<Resultados>)dtGridResultado.ItemsSource;
            List<Proceso> misProcesosObj = (List<Proceso>)dtGridProceso.ItemsSource;

            //Conformamos la query
            foreach (Estacion miEstacion in misEstacionesObj)
            {
                if (miEstacion != null && miEstacion.Id != "T")
                    if (miEstacion.Marca == "X")
                    {
                        if (sWhereEstaciones == string.Empty)
                        {
                            sWhereEstaciones += " and (1<>1 ";
                        }
                        sWhereEstaciones += " or s.estacion='" + miEstacion.Id + "'";
                    }
            }
            if (sWhereEstaciones != string.Empty)
                sWhereEstaciones += ")";

            foreach (Resultados miResultado in misResultadosObj)
            {
                if (miResultado != null && miResultado.Nombre != "TODOS")
                    if (miResultado.Marca == "X")
                    {
                        if (sWhereResultados == string.Empty)
                        {
                            sWhereResultados += " and (1<>1 ";
                        }
                        sWhereResultados += " or s.resultado='" + miResultado.Nombre + "'";
                    }
            }
            if (sWhereResultados != string.Empty)
                sWhereResultados += ")";

            foreach (Proceso miProceso in misProcesosObj)
            {
                if (miProceso != null && miProceso.Nombre != "TODOS")
                    if (miProceso.Marca == "X")
                    {
                        if (sWhereProcesos == string.Empty)
                        {
                            sWhereProcesos += " and (1<>1 ";
                        }
                        sWhereProcesos += " or s.proceso='" + miProceso.Nombre + "'";
                    }
            }
            if (sWhereProcesos != string.Empty)
                sWhereProcesos += ")";

            string sWhereFechas1 = string.Empty;
            string sWhereFechas2 = string.Empty;
            string miFechaInicio = dtPickerInicio.Text;
            if (miFechaInicio != string.Empty) {
                string miDiaInicio = miFechaInicio.Substring(0, 2);
                string miMesInicio = miFechaInicio.Substring(3, 2);
                string miYearInicio = miFechaInicio.Substring(6, 4);
                 sWhereFechas1 = " and ( s.\"year\">= " + miYearInicio + " and ";
                sWhereFechas1 += " (s.mes>= " + miMesInicio + " or s.\"year\">" + miYearInicio + ") and ";  //year es una palabra reservada para AWS, hay que poner el escape de dobles comillas
                sWhereFechas1 += " (s.dia>= " + miDiaInicio + " or s.mes>" + miMesInicio + ")";
                sWhereFechas1 += ")";
            }
            string miFechaFin = dtPickerFin.Text;
            if (miFechaFin != string.Empty)
            {
                string miDiaFin = miFechaFin.Substring(0, 2);
                string miMesFin = miFechaFin.Substring(3, 2);
                string miYearFin = miFechaFin.Substring(6, 4);
                sWhereFechas2 = " and ( s.\"year\"<= " + miYearFin + " and ";
                sWhereFechas2 += " (s.mes<= " + miMesFin + " or s.\"year\"<" + miYearFin + ") and ";
                sWhereFechas2 += " (s.dia<= " + miDiaFin + " or s.mes<" + miMesFin + ")";
                sWhereFechas2 += ")";
            }
            string sWhereFechas = sWhereFechas1 + sWhereFechas2;

            using (var eventStream = await GetSelectObjectContentEventStream(s3Client, miPlanta, nombreBBDD, " 1=1" +sWhereEstaciones + sWhereResultados+ sWhereFechas+ sWhereProcesos))
            {
                foreach (var ev in eventStream)
                {
                    //MessageBox.Show($"Received {ev.GetType().Name}!");
                    if (ev is RecordsEvent records)
                    {
                        //MessageBox.Show("The contents of the Records Event is...");
                        using (var reader = new StreamReader(records.Payload, Encoding.UTF8))
                        {
                            //MessageBox.Show(reader.ReadToEnd());
                            while (true) {
                                var json = reader.ReadLine();
                                if (json is null)
                                {
                                    break;
                                }
                                resultadosTemp = JsonConvert.DeserializeObject<Clases.ResultadoMaquina>(json);
                                resultados.Add(resultadosTemp); 
                            }
                        }
                    }
                }
            }
            if (resultados.Count > 0)
            {
                Ventanas.VisorResultados miVentana = new VisorResultados(resultados);
                miVentana.Show();
            }
            else
            {
                Global.MensajeFeedback("No se han encontrados coincidentes con la búsqueda seleccionada");
            }
            //using (var eventStream = await GetSelectObjectContentEventStream(s3Client, "tfmsal-planta2", "2_1.json"))
            //{
            //    // Since everything happens on a background thread, exceptions are raised as events.
            //    // Here, we are just throwing the exception received.
            //    eventStream.ExceptionReceived += (sender, args) => throw args.EventStreamException;

            //    eventStream.EventReceived += (sender, args) =>
            //        MessageBox.Show($"Received {args.EventStreamEvent.GetType().Name}!");
            //    eventStream.RecordsEventReceived += (sender, args) =>
            //    {
            //        MessageBox.Show("The contents of the Records Event is...");
            //        using (var reader = new StreamReader(args.EventStreamEvent.Payload, Encoding.UTF8))
            //        {
            //            MessageBox.Show(reader.ReadToEnd());
            //            //resultadosTemp = JsonConvert.DeserializeObject<Clases.ResultadoMaquina>(reader);
            //            //resultados.Add(resultadosTemp);
            //        }
            //    };
            //    eventStream.EndEventReceived += (sender, args) => endWaitHandle.Set();

            //    eventStream.StartProcessing();
            //    endWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
            //}
        }

        /// <summary>
        /// Realiza la consulta tipo SQL al objeto en la nube
        /// </summary>
        /// <param name="s3Client">Conexión cliente</param>
        /// <param name="_bucketName">Nombre del bucket donde buscar los objetos</param>
        /// <param name="_keyName">ID del objeto a consultar</param>
        /// <param name="sWhere">Filtros deseados para la búsqueda</param>
        /// <returns></returns>
        private static async Task<ISelectObjectContentEventStream> GetSelectObjectContentEventStream(AmazonS3Client s3Client
                                                                                                    , string _bucketName
                                                                                                    , string _keyName
                                                                                                    , string sWhere)
        {
            var response = await s3Client.SelectObjectContentAsync(new SelectObjectContentRequest()
            {
                Bucket = _bucketName,
                Key = _keyName,
                ExpressionType = ExpressionType.SQL,
                Expression = "SELECT * FROM s3object[*][*] as s where "+ sWhere,
                InputSerialization = new InputSerialization()
                {
                    JSON = new JSONInput()
                    {
                        JsonType = JsonType.Lines
                    }
                },
                OutputSerialization = new OutputSerialization()
                {
                    JSON = new JSONOutput()
                }
            });

            return response.Payload;
        }

        /// <summary>
        /// Rellena el listado de buckts del cliente
        /// </summary>
        /// <returns></returns>
        private async Task CargaBuckets()
        {
            ListBucketsResponse cargaBKTSResponse = await Clases.s3Contenedor.DameListaBucketsAsync(s3Client);
            dtGridFabrica.ItemsSource = cargaBKTSResponse.Buckets;
        }

        /// <summary>
        /// Control de marcas de Estacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtGridEstacion_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            List<Estacion> misEstacionesObj = (List<Estacion>)dtGridEstacion.ItemsSource;
            Estacion miEstacion = (Estacion)dtGridEstacion.SelectedItem;

            dtGridEstacion.ItemsSource = null;
            //Actualizo el resgistro del doble click
            misEstacionesObj.Find(x => x.Id == miEstacion.Id).Marca= miEstacion.Marca == "X" ? String.Empty : "X";
            //Si marco el resgistro de TODOS, desmarco el resto
            if (miEstacion.Id == "T" && miEstacion.Marca=="X")
            {
                foreach (Estacion item in misEstacionesObj)
                {
                    if (item.Id != "T")
                        misEstacionesObj.Find(x => x.Id == item.Id).Marca = String.Empty;
                }
            }
            else
            {
                //si no, desmarco el TODOS
                misEstacionesObj.Find(x => x.Id == "T").Marca = String.Empty;
            }
            dtGridEstacion.ItemsSource = misEstacionesObj;
        }

        /// <summary>
        /// /// Control de marcas de Resultado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtGridResultado_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            List<Resultados> misResultadosObj = (List<Resultados>)dtGridResultado.ItemsSource;
            Resultados miResultado = (Resultados)dtGridResultado.SelectedItem;

            dtGridResultado.ItemsSource = null;
            //Actualizo el resgistro del doble click
            misResultadosObj.Find(x => x.Nombre == miResultado.Nombre).Marca = miResultado.Marca == "X" ? String.Empty : "X";
            //Si marco el resgistro de TODOS, desmarco el resto
            if (miResultado.Nombre == "TODOS" && miResultado.Marca == "X")
            {
                foreach (Resultados item in misResultadosObj)
                {
                    if (item.Nombre != "TODOS")
                        misResultadosObj.Find(x => x.Nombre == item.Nombre).Marca = String.Empty;
                }
            }
            else
            {
                //si no, desmarco el TODOS
                misResultadosObj.Find(x => x.Nombre == "TODOS").Marca = String.Empty;
            }
            dtGridResultado.ItemsSource = misResultadosObj;
        }

        /// <summary>
        /// /// Control de marcas de Proceso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtGridProceso_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            List<Proceso> misProcesoObj = (List<Proceso>)dtGridProceso.ItemsSource;
            Proceso miProceso = (Proceso)dtGridProceso.SelectedItem;

            dtGridProceso.ItemsSource = null;
            //Actualizo el resgistro del doble click
            misProcesoObj.Find(x => x.Id == miProceso.Id).Marca = miProceso.Marca == "X" ? String.Empty : "X";
            //Si marco el resgistro de TODOS, desmarco el resto
            if (miProceso.Id == "T" && miProceso.Marca == "X")
            {
                foreach (Proceso item in misProcesoObj)
                {
                    if (item.Id != "T")
                        misProcesoObj.Find(x => x.Id == item.Id).Marca = String.Empty;
                }
            }
            else
            {
                //si no, desmarco el TODOS
                misProcesoObj.Find(x => x.Id == "T").Marca = String.Empty;
            }
            dtGridProceso.ItemsSource = misProcesoObj;
        }
    }
}
