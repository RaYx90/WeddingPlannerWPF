using Entidades;
using Negocio;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
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

namespace presentacion
{
    /// <summary>
    /// Lógica de interacción para PageMensajes.xaml
    /// </summary>
    public partial class PageMensajes : Page
    {
        /// <summary>
        /// variables
        /// </summary>
        private proveedorEntidad proveedor;
        private List<eventoEntidad> leventoEntidad;
        private Wedding wedding;
        private ObservableCollection<MensajeEntidad> listMensajeEntidad;
        private eventoEntidad evento;
        private BreadCrumb bcMensaje;
        private string tokenProv;
        private System.Windows.Threading.DispatcherTimer timer;

        /// <summary>
        /// constructor sin argumentos
        /// </summary>
        public PageMensajes(proveedorEntidad pe)
        {
            InitializeComponent();
            this.proveedor = pe;
            this.wedding = new Wedding();
            this.tokenProv = "";
            this.leventoEntidad = this.wedding.ObtenerEventosActivos(pe);
            if (leventoEntidad.Count > 0)
            {
                this.lvEventos.ItemsSource = leventoEntidad;
            }
            else
            {
                List<eventoEntidad> listEventosvacios = new List<eventoEntidad>();
                eventoEntidad ee = new eventoEntidad
                {
                    nombre = "Sin eventos activos"
                };
                listEventosvacios.Add(ee);
                this.lvEventos.ItemsSource = listEventosvacios;
                this.lvEventos.IsEnabled = false;
            }
            this.lvEventos.SelectionChanged += LvClientes_SelectionChanged;
            this.listMensajeEntidad = new ObservableCollection<MensajeEntidad>();
            lvMensajes.ItemsSource = listMensajeEntidad;
            this.bcMensaje = new BreadCrumb { Nombre = "Mensajes" };
            MainWindow.CargaBread(this.bcMensaje, false);
            this.bEnviarMensaje.Click += BEnviarMensaje_Click;
            ObtenerTokenProveedor();
        }

        /// <summary>
        /// funcion para llamar al timer
        /// </summary>
        private void InvocaTimer()
        {
            try
            {
                if (lvEventos.Items.Count > 0)
                {
                    timer = new System.Windows.Threading.DispatcherTimer();
                    timer.Tick += new EventHandler(EjecutaAccionTimer);
                    timer.Interval = new TimeSpan(0, 0, 25);
                    timer.Start();
                }
            }
            catch
            {
                ///SI el timer da un error no se bloquea o cierra la app
            }
        }

        /// <summary>
        /// El timer llamara a esta funcion para que recargue los mensajes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EjecutaAccionTimer(object sender, EventArgs e)
        {
            ObtenMensajeThread();
        }

        /// <summary>
        /// Bloquea los controles y llama mediante un hilo a la funcion de obtener mensajes del api
        /// </summary>
        private void ObtenMensajeThread()
        {
            try
            {
                if (listMensajeEntidad.Count > 0)
                {
                    listMensajeEntidad.Clear();
                    lvMensajes.Visibility = Visibility.Hidden;
                }

                ///BLOQUEAMOS LOS CONTROLES
                spinner.Visibility = Visibility.Visible;
                lvEventos.IsEnabled = false;
                lvMensajes.IsEnabled = false;
                //spEnvioMensajes.IsEnabled = false;

                if (lvEventos.SelectedItem != null)
                {
                    evento = lvEventos.SelectedItem as eventoEntidad;

                    //Creamos el delegado 
                    ThreadStart delegado = new ThreadStart(ObtenMensajesAPI);
                    //Creamos la instancia del hilo 
                    Thread hilo = new Thread(delegado);
                    //Iniciamos el hilo 
                    hilo.Start();
                }
            }
            catch (Exception ex)
            {
                utils.MuestraMensajeError(ex.Message);
                spinner.Visibility = Visibility.Hidden;
                lvEventos.IsEnabled = true;
                lvMensajes.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Funcion para enviar mensajes al cliente mediante el api
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BEnviarMensaje_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(tbMensajeEnviar.Text))
                {
                    if (Api.EnviaMensaje(proveedor, evento.ID, this.tokenProv, tbMensajeEnviar.Text))
                    {
                        MensajeEntidad me = new MensajeEntidad
                        {
                            Descripcion = tbMensajeEnviar.Text,
                            FechaCompleta = DateTime.Now,
                            Propietario = true
                        };
                        listMensajeEntidad.Add(me);
                        lvMensajes.ItemsSource = listMensajeEntidad;
                    }
                    else
                    {
                        utils.MuestraMensajeInformacion("No se ha podido enviar el mensaje al cliente. Inténtelo de nuevo pasados unos minutos.");
                    }
                }
            }
            catch (Exception ex)
            {

                utils.MuestraMensajeError(ex.Message);
            }
            finally
            {
                tbMensajeEnviar.Text = "";
                lvMensajes.SelectedIndex = lvMensajes.Items.Count - 1;
                lvMensajes.ScrollIntoView(lvMensajes.SelectedItem);
            }
        }

        /// <summary>
        /// Funcion para que al pulsar en un item evento se carguen sus mensajes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LvClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(timer != null)
                {
                    timer.Stop();
                }
                
                ObtenMensajeThread();
                this.InvocaTimer();
            }
            catch
            {
                if (timer != null)
                {
                    timer.Stop();
                }
            }
        }

        /// <summary>
        /// funcion para obtener un token para el proveedor
        /// </summary>
        private void ObtenerTokenProveedor()
        {
            try
            {
                this.tokenProv = Api.ObtenerToken(this.proveedor);
            }
            catch(Exception ex)
            {
                utils.MuestraMensajeError("Ha ocurrido un error al obtener un token del proveedor: "+ ex.Message);
            }
        }

        /// <summary>
        /// funcion que se ejecutara en otro hilo para no bloquear la app
        /// </summary>
        /// <param name="ev"></param>
        /// <returns></returns>
        private void ObtenMensajesAPI()
        {
            try
            {
                Thread.Sleep(2000);
                //this.tokenProv = Api.ObtenerToken(this.proveedor);
                listMensajeEntidad = Api.ObtenMensajesEvento(this.evento, this.tokenProv);

                if (listMensajeEntidad.Count > 0)
                {
                    ///desde otro hilo no se puede tocar componentes graficos de la UI, por eso realizo esto.
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        lvMensajes.ItemsSource = listMensajeEntidad;
                    }));
                }
                else
                {
                    List<MensajeEntidad> listMensajesVacios = new List<MensajeEntidad>();
                    MensajeEntidad me = new MensajeEntidad
                    {
                        Descripcion = "Este evento no tiene mensajes.",
                        FechaCompleta = DateTime.Now,
                        Propietario = false
                    };

                    listMensajesVacios.Add(me);

                    ///desde otro hilo no se puede tocar componentes graficos de la UI, por eso realizo esto.
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        lvMensajes.ItemsSource = listMensajesVacios;
                    }));
                }
            }
            catch(Exception ex)
            {
                if (timer != null)
                {
                    timer.Stop();
                }
                utils.MuestraMensajeError("Ha ocurrido un error al obtener los mensajes de este evento: " + ex.Message);
            }
            finally
            {
                ///desde otro hilo no se puede tocar componentes graficos de la UI, por eso realizo esto.
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    spinner.Visibility = Visibility.Hidden;
                    lvMensajes.Visibility = Visibility.Visible;
                    lvMensajes.IsEnabled = true;
                    lvEventos.IsEnabled = true;
                    spEnvioMensajes.IsEnabled = true;
                    lvMensajes.SelectedIndex = lvMensajes.Items.Count - 1;
                    lvMensajes.ScrollIntoView(lvMensajes.SelectedItem);
                }));
            }
        }
    }
}
