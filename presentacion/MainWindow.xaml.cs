using Entidades;
using Negocio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
/// <AUTHOR>JOSE MANUEL ESPARCIA</AUTHOR>
/// </summary>
namespace presentacion
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// declaracion de variables
        /// </summary>
        public static proveedorEntidad proveedorEntidad;
        private bool realizarLogout;
        public static ObservableCollection<BreadCrumb> listBreadCrumbs { get; set; }
        private static BreadCrumb breadCrumbInicio;
        private BreadCrumb bcCliente;
        private BreadCrumb bcNuevoPresupuesto;
        private BreadCrumb bcPresupuestos;
        private BreadCrumb bcEventos;
        private BreadCrumb bcNuevoEvento;
        private BreadCrumb bcPerfil;
        private BreadCrumb bcAyuda;
        private BreadCrumb bcMensajes;
        public static int idproveedor;
        private Wedding wedding;

        /// <summary>
        /// Constructor con argumentos
        /// </summary>
        /// <param name="pe"></param>
        public MainWindow(proveedorEntidad pe)
        {
            InitializeComponent();
            proveedorEntidad = pe;
            this.bLogout.Click += BLogout_Click;
            this.Closing += MainWindow_Closing;
            this.realizarLogout = false;
            this.NavigationFrame.Navigate(new PageHome(proveedorEntidad));
            listBreadCrumbs = new ObservableCollection<BreadCrumb>();
            lvBreadCrumbs.ItemsSource = listBreadCrumbs;
            breadCrumbInicio = new BreadCrumb { Nombre = "Inicio" };
            bcCliente = new BreadCrumb { Nombre = "Clientes" };
            bcNuevoPresupuesto = new BreadCrumb { Nombre = "Nuevo Presupuesto" };
            bcPresupuestos = new BreadCrumb { Nombre = "Presupuestos" };
            bcEventos = new BreadCrumb { Nombre = "Eventos" };
            bcNuevoEvento = new BreadCrumb { Nombre = "Nuevo evento" };
            bcPerfil = new BreadCrumb { Nombre = "Perfil" };
            bcAyuda = new BreadCrumb { Nombre = "Ayuda" };
            bcMensajes = new BreadCrumb { Nombre = "Mensajes" };
            CargaBread(breadCrumbInicio, false);
            lvBreadCrumbs.SelectionChanged += LvBreadCrumbs_SelectionChanged;
            this.wedding = new Wedding();
            idproveedor = this.wedding.ObtenerIDProveedorLogin(pe);
            bCuenta.Click += BCuenta_Click;
            bAyuda.Click += BAyuda_Click;
        }

        /// <summary>
        /// funcion para ir a la página de ayuda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BAyuda_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.NavigationFrame.Navigate(new PageAyuda());
            }
            catch (Exception ex)
            {
                utils.MuestraMensajeError(ex.Message);
            }
        }

        /// <summary>
        /// funcion para ir a la pagina de perfil
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BCuenta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.NavigationFrame.Navigate(new PagePerfil(proveedorEntidad));
            }
            catch(Exception ex)
            {
                utils.MuestraMensajeError(ex.Message);
            }
        }

        /// <summary>
        /// funcion del listview para cargar las ventanas elegidas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LvBreadCrumbs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lvBreadCrumbs.SelectedItem != null)
                {
                    BreadCrumb bc = (BreadCrumb)((ListView)sender).SelectedItem;
                    switch (bc.Nombre)
                    {
                        case "Inicio":
                            CargaBread(breadCrumbInicio, true);
                            this.NavigationFrame.Navigate(new PageHome(proveedorEntidad));
                            break;
                        case "Nuevo Presupuesto":
                            CargaBread(bcNuevoPresupuesto, true);
                            this.NavigationFrame.Navigate(new PageNuevoPresupuesto(proveedorEntidad, null,null));
                            break;
                        case "Clientes":
                            CargaBread(bcCliente, true);
                            this.NavigationFrame.Navigate(new PageCliente(proveedorEntidad));
                            break;
                        case "Presupuestos":
                            CargaBread(bcPresupuestos, true);
                            this.NavigationFrame.Navigate(new PagePresupuestos(proveedorEntidad));
                            break;
                        case "Eventos":
                            CargaBread(bcEventos, true);
                            this.NavigationFrame.Navigate(new PageEvento(proveedorEntidad));
                            break;
                        case "Nuevo Evento":
                            CargaBread(bcNuevoEvento, true);
                            this.NavigationFrame.Navigate(new PageNuevoEvento(proveedorEntidad));
                            break;
                        case "Perfil":
                            CargaBread(bcPerfil, true);
                            this.NavigationFrame.Navigate(new PagePerfil(proveedorEntidad));
                            break;
                        case "Ayuda":
                            CargaBread(bcAyuda, true);
                            this.NavigationFrame.Navigate(new PageAyuda());
                            break;
                        case "Mensajes":
                            CargaBread(bcMensajes, true);
                            this.NavigationFrame.Navigate(new PageMensajes(proveedorEntidad));
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
                utils.MuestraMensajeError(ex.Message);
            }
            finally
            {
                ListViewMenu.SelectedItem = null;
                lvBreadCrumbs.SelectedItem = null;
            }
        }

        /// <summary>
        /// Funcion para cargar las "miguitas de pan"  o bien eliminar según donde se pulse
        /// </summary>
        public static void CargaBread(BreadCrumb breadCrumb, bool eliminar)
        {
            try
            {
                if (eliminar)
                {
                    var crumbsAEliminar = listBreadCrumbs.Where(b=>b.Nombre != "Inicio" && b.Nombre != breadCrumb.Nombre).ToList();

                    foreach (var cbe in crumbsAEliminar)
                    {
                        listBreadCrumbs.Remove(cbe);
                    }
                }
                else
                {
                    bool agregaBreadCrumb = true;

                    foreach (var bc in listBreadCrumbs)
                    {
                        if (bc.Nombre == breadCrumb.Nombre)
                        {
                            agregaBreadCrumb = false;
                        }
                    }

                    if (agregaBreadCrumb)
                    {
                        listBreadCrumbs.Add(breadCrumb);
                    }
                }
            }
            catch (Exception ex)
            {
                utils.MuestraMensajeError(ex.Message);
            }
        }

        /// <summary>
        /// Evento de cierre del wpf, preguntamos si se desea salir.
        /// </summary>
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!this.realizarLogout)
            {
                if (MessageBox.Show("¿Esta seguro que desea salir de la aplicación?", "Mensaje de confirmación", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// funcion para cerrar sesion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BLogout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Esta seguro que desea cerrar sesión?", "Mensaje de confirmación", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                try
                {
                    if (File.Exists(Rutas.rutaCredentials))
                    {
                        File.Delete(Rutas.rutaCredentials);
                    }
                    LoginWindows loginWindows = new LoginWindows();
                    loginWindows.Show();
                    this.realizarLogout = true;
                    this.Close();
                }
                catch(Exception ex)
                {
                    utils.MuestraMensajeError(ex.Message);
                }
            }
        }

        /// <summary>
        /// funcion para expandir el menu de la izquierda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            this.tbNombreProveedor.Text = proveedorEntidad.nombre;
        }
        
        /// <summary>
        /// funcion para cerrar el menu de la izquierda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
            this.tbNombreProveedor.Text = "";
        }

        /// <summary>
        /// Funcion para ir al page del boton pulsado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ListViewMenu.SelectedItem != null)
                {
                    switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
                    {
                        case "ItemHome":
                            CargaBread(breadCrumbInicio, true);
                            NavigationFrame.Navigate(new PageHome(proveedorEntidad));
                            break;
                        case "ItemEvento":
                            NavigationFrame.Navigate(new PageEvento(proveedorEntidad));
                            break;
                        case "ItemClientes":
                            CargaBread(bcCliente, true);
                            NavigationFrame.Navigate(new PageCliente(proveedorEntidad));
                            break;
                        case "ItemPresupuestos":
                            NavigationFrame.Navigate(new PagePresupuestos(proveedorEntidad));
                            break;
                        case "ItemMensajes":
                            NavigationFrame.Navigate(new PageMensajes(proveedorEntidad));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
                utils.MuestraMensajeError(ex.Message);
            }
            finally
            {
                ListViewMenu.SelectedItem = null;
                lvBreadCrumbs.SelectedItem = null;
            }
        }
    }
}
