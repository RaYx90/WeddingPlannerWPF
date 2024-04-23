using Entidades;
using Negocio;
using System;
using System.Collections.Generic;
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
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

/// <summary>
/// <author>JOSE MANUEL ESPARCIA</author>
/// </summary>
namespace presentacion
{
    /// <summary>
    /// Lógica de interacción para PagePresupuestos.xaml
    /// </summary>
    public partial class PagePresupuestos : Page
    {
        /// <summary>
        /// declaracion de variables
        /// </summary>
        private proveedorEntidad proveedorEntidad;
        private Wedding wedding;
        private List<PresupuestoEntidad> listPresupuestos;
        private BreadCrumb bcPresupuestos;
        private clienteEntidad clienteEntidad;

        /// <summary>
        /// constructor con argumentos
        /// </summary>
        /// <param name="pe"></param>
        public PagePresupuestos(proveedorEntidad pe)
        {
            InitializeComponent();
            this.proveedorEntidad = pe;
            this.wedding = new Wedding();
            bcPresupuestos = new BreadCrumb { Nombre = "Presupuestos" };
            MainWindow.CargaBread(this.bcPresupuestos, false);
            RellenaDataGrid(proveedorEntidad);
            dpFecha.SelectedDate = DateTime.Now;
            this.tbNumero.TextChanged += TbNumero_TextChanged;
            this.dpFecha.SelectedDateChanged += DpFecha_SelectedDateChanged;
            this.tbNombre.TextChanged += TbNombre_TextChanged;
            this.bCrear.Click += BCrear_Click;
            this.FiltrosDataGrid();
            this.bEditar.Click += BEditar_Click;
            this.bImprimir.Click += BImprimir_Click;
            this.dgPresupuestos.SelectionChanged += DgPresupuestos_SelectionChanged;
            clienteEntidad = new clienteEntidad();
            cbActivaFecha.Checked += CbActivaFecha_Checked;
            cbActivaFecha.Click += CbActivaFecha_Checked;
            bEliminar.Click += BEliminar_Click;
        }

        /// <summary>
        /// funcion para eliminar un presupuesto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgPresupuestos.SelectedItem != null)
                {
                    if (MessageBox.Show("¿Esta seguro que desea eliminar el presupuesto seleccionado?", "Mensaje de confirmación", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        var presupuesto = dgPresupuestos.SelectedItem as PresupuestoEntidad;
                        this.wedding.EliminaLineasPresupuesto(presupuesto.Id);
                        this.wedding.EliminaPresupuesto(presupuesto.Id);
                        utils.MuestraMensajeInformacion("El presupuesto se ha eliminado correctamente");
                        this.RellenaDataGrid(this.proveedorEntidad);

                        int idEvento = wedding.ObtenerIDEvento(this.proveedorEntidad, this.clienteEntidad);
                        string token = Api.ObtenerToken(proveedorEntidad);
                        Api.EnviaMensaje(proveedorEntidad, idEvento, token, "Su proveedor " + proveedorEntidad.nombre + " ha eliminado el presupuesto "+presupuesto.Numero+". Si cree que esto no es correcto, escríbale un mensaje.");
                    }
                }
                else
                {
                    utils.MuestraMensajeInformacion("Debe seleccionar un presupuesto a eliminar.");
                }
            }
            catch (Exception ex)
            {
                utils.MuestraMensajeError(ex.Message);
            }
        }


        /// <summary>
        /// Cuando se pulsa en check se activa el filtro de fecha
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbActivaFecha_Checked(object sender, RoutedEventArgs e)
        {
            this.FiltrosDataGrid();
        }

        /// <summary>
        /// funcion para que al marcar un presupesto se pueda editar o imprimir rellenando todo los datos del impreso XPS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgPresupuestos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgPresupuestos.SelectedItem != null)
                {
                    var presupuesto = dgPresupuestos.SelectedItem as PresupuestoEntidad;
                    int idpresupuesto = presupuesto.Id;
                    tbClienteImpr.Text = presupuesto.Cliente;
                    tbNumeroPresupuestoImpr.Text = presupuesto.Numero.ToString();
                    dpFechaPresupuestoImpr.SelectedDate = dpFecha.SelectedDate;
                    List<PresupuestoLineaEntidad> lple = new List<PresupuestoLineaEntidad>();
                    List<PresupuestoEntidad> lp = new List<PresupuestoEntidad>();
                    lp.Add(presupuesto);
                    lple = this.wedding.ObtenerLineasPresupuesto(idpresupuesto);
                    dgPresupuestosLineas.ItemsSource = lple;
                    dgPresupuestosTotal.ItemsSource = lp;
                    int idCliente = presupuesto.IdCliente;
                    clienteEntidad = this.wedding.ObtenerClientePorID(idCliente);
                    tbDireccionImpr.Text = clienteEntidad.Direccion;
                    bImprimir.IsEnabled = true;
                    bEditar.IsEnabled = true;
                    bEliminar.IsEnabled = true;
                }
            }
            catch(Exception ex)
            {
                utils.MuestraMensajeError(ex.Message);
            }
        }

        /// <summary>
        /// GENERAMOS EL DOCUMENTO A IMPRIMIR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BImprimir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                gHistorico.Visibility = Visibility.Hidden;
                FlowDocPresupuestoViewer.Visibility = Visibility.Visible;

                FlowDocumentScrollViewer visual = FlowDocPresupuestoViewer;

                this.EliminaImpresosXPS("printPreview.xps");

                // write the XPS document
                using (XpsDocument doc = new XpsDocument("printPreview.xps", FileAccess.ReadWrite))
                {
                    XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
                    writer.Write(visual);
                }

                // Read the XPS document into a dynamically generated
                // preview Window 
                Window preview = new PrintWindow();
                using (XpsDocument doc = new XpsDocument("printPreview.xps", FileAccess.Read))
                {
                    FixedDocumentSequence fds = doc.GetFixedDocumentSequence();

                    DocumentViewer dv1 = LogicalTreeHelper.FindLogicalNode(preview, "PreviewD") as DocumentViewer;
                    dv1.Document = fds as IDocumentPaginatorSource;

                    // Eliminamos la ventana de búsqueda
                    ContentControl cc = dv1.Template.FindName("PART_FindToolBarHost", dv1) as ContentControl;
                    cc.Visibility = Visibility.Collapsed;
                }
                preview.ShowDialog();
            }
            catch (Exception ex)
            {
                utils.MuestraMensajeError(ex.Message);
            }
            finally
            {
                this.EliminaImpresosXPS("printPreview.xps");
                gHistorico.Visibility = Visibility.Visible;
                FlowDocPresupuestoViewer.Visibility = Visibility.Hidden;

            }
        }

        /// <summary>
        /// Funcion para que en caso de existir, elimine el fichero xps generado
        /// </summary>
        /// <param name="ficheroXPS"></param>
        private void EliminaImpresosXPS(string ficheroXPS)
        {
            try
            {
                if (File.Exists(ficheroXPS))
                {

                    File.Delete(ficheroXPS);
                }

            }
            catch (Exception ex)
            {
               utils.MuestraMensajeError(ex.Message);
            }
        }

        /// <summary>
        /// Funcion para ir a la ventana de editar un presupuesto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgPresupuestos.SelectedItem != null)
                {
                    var presupuesto = dgPresupuestos.SelectedItem as PresupuestoEntidad;
                    List<PresupuestoLineaEntidad> lple = new List<PresupuestoLineaEntidad>();
                    lple = this.wedding.ObtenerLineasPresupuesto(presupuesto.Id);
                    this.NavigationService.Navigate(new PageEditarPresupuesto(proveedorEntidad, presupuesto, lple, clienteEntidad));
                }
                else
                {
                    utils.MuestraMensajeInformacion("Debe seleccionar un presupuesto a editar.");
                }
            }
            catch(Exception ex)
            {
                utils.MuestraMensajeError(ex.Message);
            }
        }

        /// <summary>
        /// Evento para ir a la ventana de nuevo presupuesto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BCrear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<clienteEntidad> listClientes = this.wedding.ObtenerClientes(proveedorEntidad);
                if (listClientes.Count > 0)
                {
                    this.NavigationService.Navigate(new PageNuevoPresupuesto(proveedorEntidad, null, null));
                }
                else
                {
                    utils.MuestraMensajeInformacion("No tiene clientes asociados, para crear un presupuesto primero debe generar un evento y vincularle los clientes.");
                }
            }
            catch(Exception ex)
            {
                utils.MuestraMensajeError(ex.Message);
            }
        }

        /// <summary>
        /// aplicamos filtro nombre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            FiltrosDataGrid();
        }

        /// <summary>
        /// aplicamos filtro fecha
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DpFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FiltrosDataGrid();
        }

        /// <summary>
        /// aplicamos filtro numero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbNumero_TextChanged(object sender, TextChangedEventArgs e)
        {
            FiltrosDataGrid();
        }

        /// <summary>
        /// funcion para rellenar el datagrid
        /// </summary>
        /// <param name="pe"></param>
        private void RellenaDataGrid(proveedorEntidad pe)
        {
            try
            {
                this.dgPresupuestos.ItemsSource = null;
                this.dgPresupuestos.Items.Clear();
                this.listPresupuestos = this.wedding.ObtenerPresupuestos(MainWindow.idproveedor);
                this.dgPresupuestos.ItemsSource = this.listPresupuestos;
            }
            catch (Exception ex)
            {
                utils.MuestraMensajeError(ex.Message);
            }
        }

        /// <summary>
        /// Funcion para filtrar los datos del listpresupuestos
        /// </summary>
        private void FiltrosDataGrid()
        {
            try
            {
                List<PresupuestoEntidad> lpresupuestosFiltrados = new List<PresupuestoEntidad>();

                if (cbActivaFecha.IsChecked == true)
                {
                    foreach (var p in listPresupuestos.Where(p => (p.Numero.ToString().Contains(tbNumero.Text.ToUpper())
                                                            && p.Fecha.ToString().Contains(dpFecha.SelectedDate.ToString())
                                                            && p.Cliente.ToUpper().Contains(tbNombre.Text.ToUpper()))))
                    {
                        lpresupuestosFiltrados.Add(p);
                    }
                }
                else
                {
                    foreach (var p in listPresupuestos.Where(p => (p.Numero.ToString().Contains(tbNumero.Text.ToUpper())
                                                            && p.Cliente.ToUpper().Contains(tbNombre.Text.ToUpper()))))
                    {
                        lpresupuestosFiltrados.Add(p);
                    }
                }
                this.dgPresupuestos.ItemsSource = lpresupuestosFiltrados;
            }
            catch (Exception ex)
            {
                utils.MuestraMensajeError(ex.Message);
                RellenaDataGrid(this.proveedorEntidad);
            }
        }
    }
}
