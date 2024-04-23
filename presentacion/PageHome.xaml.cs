using Entidades;
using Negocio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace presentacion
{
    /// <summary>
    /// Lógica de interacción para PageHome.xaml
    /// </summary>
    public partial class PageHome : Page
    {
        /// <summary>
        /// declaracion de variables
        /// </summary>
        private proveedorEntidad proveedorEntidad;
        private Wedding wedding;
        private List<eventoEntidad> leventosTotales;
        private List<eventoEntidad> leventosActivos;
        private List<clienteEntidad> lclientesTotales;
        private List<servicioEntidad> lservicioTotales;

        public PageHome(proveedorEntidad pe)
        {
            InitializeComponent();
            this.proveedorEntidad = pe;
            this.wedding = new Wedding();
            this.InicializaDatos();
        }

        /// <summary>
        /// Funcion para inicializar los datos
        /// </summary>
        private void InicializaDatos()
        {
            this.leventosTotales = this.wedding.ObtenerEventos(proveedorEntidad);
            this.leventosActivos = this.wedding.ObtenerEventosActivos(proveedorEntidad);
            this.lclientesTotales = this.wedding.ObtenerClientes(proveedorEntidad);
            this.lservicioTotales = this.wedding.ObtenerServiciosProveedor(proveedorEntidad);
            this.tbTotalEventosActivos.Text = this.leventosActivos.Count.ToString();
            this.tbTotalEventos.Text = this.leventosTotales.Count.ToString();
            this.tbTotalClientes.Text = this.lclientesTotales.Count.ToString();
            this.tbTotalServicios.Text = this.lservicioTotales.Count.ToString();

            List<PresupuestoEntidad> listPresupuestos = new List<PresupuestoEntidad>();
            int idProveedor = this.wedding.ObtenerIDProveedorLogin(proveedorEntidad);
            listPresupuestos = this.wedding.ObtenerPresupuestos(idProveedor);
            List<PresupuestoEntidad> ListPresupuestoAceptados = listPresupuestos.Where(i => i.Aceptado == true).ToList();
            List<PresupuestoEntidad> ListPresupuestoNoAceptados = listPresupuestos.Where(i => i.Aceptado == false).ToList();


            List<PresupuestoEntidad> listPresupuestoMetro = new List<PresupuestoEntidad>()
            {
                new PresupuestoEntidad{ Aceptado = true, Total=ListPresupuestoAceptados.Count },
                new PresupuestoEntidad{ Aceptado = false, Total=ListPresupuestoNoAceptados.Count}
            };
            
            metroChart.ItemsSource = listPresupuestoMetro;

            btEventos.Click += BtEventos_Click;
            btClientes.Click += BtClientes_Click;
            btServicios.Click += BtServicios_Click;
        }

        /// <summary>
        /// Funcion para ir al listado de servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtServicios_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PagePerfil(proveedorEntidad));
        }

        /// <summary>
        /// funcion para ir a clientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtClientes_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PageCliente(proveedorEntidad));
        }

        /// <summary>
        /// Funcion para ir a eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtEventos_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PageEvento(proveedorEntidad));
        }
    }
}
