using Entidades;
using Negocio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace presentacion
{
    /// <summary>
    /// Lógica de interacción para PageCliente.xaml
    /// </summary>
    public partial class PageCliente : Page
    {
        /// <summary>
        /// declaracion de variables
        /// </summary>
        private proveedorEntidad proveedorEntidad;
        private Wedding wedding;
        private List<clienteEntidad> listClientes;
        private BreadCrumb bcCliente;

        /// <summary>
        /// Contructor con argumentos
        /// </summary>
        /// <param name="pe"></param>
        public PageCliente(proveedorEntidad pe)
        {
            InitializeComponent();
            this.proveedorEntidad = pe;
            this.wedding = new Wedding();
            this.RellenaDataGrid(pe);
            this.tbNombre.TextChanged += TbNombre_TextChanged;
            this.tbApellidos.TextChanged += TbApellidos_TextChanged;
            this.tbDireccion.TextChanged += TbDireccion_TextChanged;
            this.tbDNI.TextChanged += TbDNI_TextChanged;
            this.tbProvincia.TextChanged += TbProvincia_TextChanged;
            this.tbPoblacion.TextChanged += TbPoblacion_TextChanged;
            this.tbEmail.TextChanged += TbEmail_TextChanged;
            this.tbCP.TextChanged += TbCP_TextChanged;
            this.bLimpiar.Click += BLimpiar_Click;
            this.bPresupuesto.Click += BPresupuesto_Click;
            this.bcCliente = new BreadCrumb { Nombre = "Clientes" };
            MainWindow.CargaBread(this.bcCliente, false);
            this.bImprimir.Click += BImprimir_Click;
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
        /// GENERAMOS EL DOCUMENTO A IMPRIMIR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BImprimir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                gridClientes.Visibility = Visibility.Hidden;
                FlowDocClientesViewer.Visibility = Visibility.Visible;

                FlowDocumentScrollViewer visual = FlowDocClientesViewer;
                
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
                gridClientes.Visibility = Visibility.Visible;
                FlowDocClientesViewer.Visibility = Visibility.Hidden;

            }
        }

        /// <summary>
        /// funcion para crear un presupuesto a un cliente de la tabla, previa selección
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BPresupuesto_Click(object sender, RoutedEventArgs e)
        {
            if(dgClientes.SelectedItem != null)
            {
                clienteEntidad ce = dgClientes.SelectedItem as clienteEntidad;
                this.NavigationService.Navigate(new PageNuevoPresupuesto(proveedorEntidad, ce, null));
            }
            else
            {
                utils.MuestraMensajeInformacion("Debe seleccionar un cliente para generar un presupuesto.");
            }
        }

        /// <summary>
        /// funcion para limpiar los filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BLimpiar_Click(object sender, RoutedEventArgs e)
        {
            this.tbNombre.Text = "";
            this.tbApellidos.Text = "";
            this.tbDireccion.Text = "";
            this.tbDNI.Text = "";
            this.tbProvincia.Text = "";
            this.tbPoblacion.Text = "";
            this.tbEmail.Text = "";
            this.tbCP.Text = "";
            this.RellenaDataGrid(this.proveedorEntidad);
            this.bLimpiar.IsEnabled = false;
        }

        /// <summary>
        /// LLamamos a la función para aplicar filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbCP_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.FiltrosDataGrid();
        }

        /// <summary>
        /// LLamamos a la función para aplicar filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.FiltrosDataGrid();
        }

        /// <summary>
        /// LLamamos a la función para aplicar filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbPoblacion_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.FiltrosDataGrid();
        }

        /// <summary>
        /// LLamamos a la función para aplicar filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbProvincia_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.FiltrosDataGrid();
        }

        /// <summary>
        /// LLamamos a la función para aplicar filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbDNI_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.FiltrosDataGrid();
        }

        /// <summary>
        /// LLamamos a la función para aplicar filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbDireccion_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.FiltrosDataGrid();
        }

        /// <summary>
        /// LLamamos a la función para aplicar filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbApellidos_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.FiltrosDataGrid();
        }

        /// <summary>
        /// LLamamos a la función para aplicar filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.FiltrosDataGrid();
        }

        /// <summary>
        /// Funcion para filtrar los datos del listClientes
        /// </summary>
        private void FiltrosDataGrid()
        {
            try
            {
                List<clienteEntidad> lclientesFiltrados = new List<clienteEntidad>();
                foreach (var p in listClientes.Where(p => (p.Nombre.ToUpper().Contains(tbNombre.Text.ToUpper())
                                                        && p.Apellidos.ToUpper().Contains(tbApellidos.Text.ToUpper())
                                                        && p.DNI.ToUpper().Contains(tbDNI.Text.ToUpper())
                                                        && p.Email.ToUpper().Contains(tbEmail.Text.ToUpper())
                                                        && p.Direccion.ToUpper().Contains(tbDireccion.Text.ToUpper())
                                                        && p.Provincia.ToUpper().Contains(tbProvincia.Text.ToUpper())
                                                        && p.Poblacion.ToUpper().Contains(tbPoblacion.Text.ToUpper())
                                                        && p.CP.ToUpper().Contains(tbCP.Text.ToUpper()))))
                {
                    lclientesFiltrados.Add(p);
                }

                this.dgClientes.ItemsSource = lclientesFiltrados;
                this.bLimpiar.IsEnabled = true;
            }
            catch
            {
                RellenaDataGrid(this.proveedorEntidad);
            }
        }

        /// <summary>
        /// funcion para rellenar el datagrid
        /// </summary>
        /// <param name="pe"></param>
        private void RellenaDataGrid(proveedorEntidad pe)
        {
            try
            {
                this.dgClientes.ItemsSource = null;
                this.dgClientes.Items.Clear();
                dgListadoClientes.ItemsSource = null;
                dgListadoClientes.Items.Clear();

                this.listClientes = this.wedding.ObtenerClientes(pe);
                this.dgClientes.ItemsSource = this.listClientes;
                dgListadoClientes.ItemsSource = this.listClientes;
            }
            catch(Exception ex)
            {
                utils.MuestraMensajeError(ex.Message);
            }
        }
    }
}
