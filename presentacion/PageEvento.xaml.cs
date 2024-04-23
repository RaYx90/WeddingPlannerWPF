using Entidades;
using Negocio;
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

/// <summary>
/// <author>JOSE MANUEL ESPARCIA</author>
/// </summary>
namespace presentacion
{
    /// <summary>
    /// Lógica de interacción para PageEvento.xaml
    /// </summary>
    public partial class PageEvento : Page
    {
        /// <summary>
        /// declaracion de variables
        /// </summary>
        private Wedding wedding;
        private proveedorEntidad proveedorEntidad;
        private List<eventoEntidad> lEventosEntidad;
        private BreadCrumb bcEventos;

        /// <summary>
        /// constructor con argumentos
        /// </summary>
        /// <param name="pe"></param>
        public PageEvento(proveedorEntidad pe)
        {
            InitializeComponent();
            this.proveedorEntidad = pe;
            this.wedding = new Wedding();
            this.lEventosEntidad = new List<eventoEntidad>();
            this.lEventosEntidad = this.wedding.ObtenerEventos(this.proveedorEntidad);
            this.bcEventos = new BreadCrumb { Nombre = "Eventos" };
            MainWindow.CargaBread(this.bcEventos, false);
            this.Calendar.SelectedDatesChanged += Calendar_SelectedDatesChanged;
            this.lvEventos.SelectionChanged += LvEventos_SelectionChanged;
            this.Calendar.SelectedDate = DateTime.Now;
            this.bEvento.Click += BEvento_Click;
        }

        /// <summary>
        /// funcion para ir a la pagina de crear eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BEvento_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PageNuevoEvento(proveedorEntidad));
        }

        /// <summary>
        /// Cuando se pulsa encima de un item del listview se muestra la info ampliada de la boda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LvEventos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                eventoEntidad eventoEntidad = (eventoEntidad)lvEventos.SelectedItem;
                if (eventoEntidad != null)
                {
                    if (eventoEntidad.fechaInt != -1)
                    {
                        utils.MuestraMensajePersonalizado(eventoEntidad.descripcion, eventoEntidad.nombre + " - " + eventoEntidad.FechaFormateada);
                    }
                }
            }
            catch(Exception ex)
            {
                utils.MuestraMensajeError(ex.Message);
            }
        }

        /// <summary>
        /// Funcion para agregar los items al listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            RellenaEventosCalendar();
            RellenaListView();
        }

        /// <summary>
        /// evento para rellenar el calendario con los eventos
        /// </summary>
        private void RellenaEventosCalendar()
        {
            foreach (var evento in this.lEventosEntidad)
            {
                DateTime fechaEvento = utils.UnixTimestampToDateTime(evento.fechaInt);
                Calendar.SelectedDates.Add(fechaEvento);
            }
        }

        /// <summary>
        /// funcion para rellenar el listview con los eventoEntidad según lo pulsado en el calendario
        /// </summary>
        private void RellenaListView()
        {
            try
            {
                lvEventos.ItemsSource = this.lEventosEntidad.Where(i => utils.UnixTimestampToDateTime(i.fechaInt).Date == Calendar.SelectedDate);

                if (lvEventos.Items.Count == 0)
                {
                    spSinEventos.Visibility = Visibility.Visible;
                }
                else
                {
                    spSinEventos.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                utils.MuestraMensajeError(ex.Message);
            }
        }
    }
}
