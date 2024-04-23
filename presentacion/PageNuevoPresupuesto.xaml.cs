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

/// <summary>
/// <author>JOSE MANUEL ESPARCIA</author>
/// </summary>
namespace presentacion
{
    /// <summary>
    /// Lógica de interacción para PageNuevoPresupuesto.xaml
    /// </summary>
    public partial class PageNuevoPresupuesto : Page
    {
        /// <summary>
        /// declaracion de variables
        /// </summary>
        private proveedorEntidad proveedorEntidad;
        private clienteEntidad clienteEntidad;
        private Wedding wedding;
        private List<clienteEntidad> listClientes;
        private BreadCrumb bcNuevoPresupuesto;
        private ObservableCollection<PresupuestoLineaEntidad> lineasPresupuesto;

        /// <summary>
        /// contructor con argumentos
        /// </summary>
        /// <param name="pe"></param>
        /// <param name="ce"></param>
        public PageNuevoPresupuesto(proveedorEntidad pe, clienteEntidad ce, PresupuestoEntidad pree)
        {
            InitializeComponent();
            this.proveedorEntidad = pe;
            this.clienteEntidad = ce;
            this.wedding = new Wedding();
            this.InicializaDatos();
            this.bcNuevoPresupuesto = new BreadCrumb { Nombre = "Nuevo Presupuesto" };
            MainWindow.CargaBread(bcNuevoPresupuesto, false);
            this.dpFecha.SelectedDate = DateTime.Now;
            this.bAcceptar.Click += BAcceptar_Click;
            this.cbClientes.SelectionChanged += CbClientes_SelectionChanged;
            this.bCancelar.Click += BCancelar_Click;
        }

        /// <summary>
        /// boton para cancelar y volver al historico de presupuestos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PagePresupuestos(proveedorEntidad));
        }

        /// <summary>
        /// evento cuando se cambia un cliente en el combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.clienteEntidad = (clienteEntidad)cbClientes.SelectedItem;
            tbDireccion.Text = this.clienteEntidad.Direccion;
        }

        /// <summary>
        /// evento al pulsar en el boton aceptar, donde generamos el presupuesto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BAcceptar_Click(object sender, RoutedEventArgs e)
        {
            if (cbClientes.SelectedItem != null)
            {
                bool lineaDescripcionVacia = false;
                List<PresupuestoLineaEntidad> listLineasPresupuesto = new List<PresupuestoLineaEntidad>();
                var cbValorIVA = cbTipoIVA.SelectedValue;
                decimal ivaDecimal = 0.00m;
                int tipoIVA = 0;

                switch (cbValorIVA)
                {
                    case "0.21":
                        ivaDecimal = 0.21m;
                        tipoIVA = 21;
                        break;
                    case "0.10":
                        ivaDecimal = 0.10m;
                        tipoIVA = 10;
                        break;

                    case "0.4":
                        ivaDecimal = 0.4m;
                        tipoIVA = 4;
                        break;
                }

                decimal importeBrutoTotal = 0;
                decimal importeIVATotal = 0;
                decimal importeNetoTotal = 0;

                try
                {
                    foreach (PresupuestoLineaEntidad linea in dgPresupuestosLineas.Items.SourceCollection)
                    {
                        if (string.IsNullOrEmpty(linea.Descripcion))
                        {
                            lineaDescripcionVacia = true;
                        }
                        else
                        {
                            PresupuestoLineaEntidad ple = new PresupuestoLineaEntidad();
                            ple.Descripcion = linea.Descripcion;
                            ple.Cantidad = linea.Cantidad;
                            ple.ImporteBruto = Math.Round(linea.ImporteBruto, 2);
                            ple.ImporteIVA = Math.Round(linea.ImporteBruto * ivaDecimal, 2);
                            ple.ImporteNeto = Math.Round((linea.ImporteBruto + ple.ImporteIVA), 2);
                            
                            listLineasPresupuesto.Add(ple);

                            ///VALORES TOTALES PARA EL PRESUPUESTO
                            importeBrutoTotal += Math.Round(linea.ImporteBruto * linea.Cantidad, 2);
                            importeIVATotal += Math.Round(importeBrutoTotal * ivaDecimal, 2);
                            importeNetoTotal = Math.Round(importeBrutoTotal + importeIVATotal,2);

                        }
                        
                    }

                    if (lineaDescripcionVacia)
                    {
                        if (MessageBox.Show("Se ha detectado líneas sin descripción, a continuación se creará el presupuesto pero sin estas. ¿Esta seguro que desea continuar?", "Mensaje de confirmación", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                        {
                            GeneraPresupuesto(listLineasPresupuesto, importeBrutoTotal, importeIVATotal, importeNetoTotal, tipoIVA);
                        }
                    }
                    else
                    {
                        GeneraPresupuesto(listLineasPresupuesto, importeBrutoTotal, importeIVATotal, importeNetoTotal, tipoIVA);
                    }

                }
                catch (Exception ex)
                {
                    utils.MuestraMensajeError(ex.Message);
                }
            }
            else
            {
                utils.MuestraMensajeInformacion("Debe seleccionar un cliente.");
            }
        }

        /// <summary>
        /// Funcion para crear el presupuesto y lineas, se utiliza prinicpalmente para evitar duplicar codigo, ya que se llama 2 veces
        /// </summary>
        /// <param name="bruto"></param>
        /// <param name="iva"></param>
        /// <param name="neto"></param>
        private void GeneraPresupuesto(List<PresupuestoLineaEntidad> lple, decimal bruto, decimal iva, decimal neto, int tipoiva)
        {
            int idPresupuesto = 0;

            try
            {
                bAcceptar.IsEnabled = false;
                int idCliente = this.wedding.ObtenerIDCliente(this.clienteEntidad);
                int numPresupuestoObtenido = this.wedding.ObtenerNumeroPresupuesto(MainWindow.idproveedor);

                ///ASIGNAMOS EL NUMERO DEL PRESUPUESTO
                if (numPresupuestoObtenido > 0)
                {
                    numPresupuestoObtenido = numPresupuestoObtenido + 1;
                }
                else
                {
                    numPresupuestoObtenido = int.Parse(DateTime.Now.Year.ToString() + "0001");
                }

                PresupuestoEntidad pe = new PresupuestoEntidad
                {
                    Cliente = null,
                    Numero = numPresupuestoObtenido,
                    Fecha = dpFecha.SelectedDate.Value,
                    Importe_Bruto = Math.Round(bruto, 2),
                    Importe_IVA = Math.Round(iva, 2),
                    Importe_Neto = Math.Round(neto, 2),
                    TipoIVA = tipoiva,
                    Aceptado = false
                };

                //utils.MuestraMensajeInformacion("SIGUES");
                if(this.wedding.InsertaPresupuesto(pe, MainWindow.idproveedor, idCliente) > 0)
                {
                    idPresupuesto = this.wedding.ObtenerIDPresupuesto(MainWindow.idproveedor, idCliente);
                    if (idPresupuesto > 0)
                    {
                        this.wedding.InsertaLineasPresupuesto(lple, idPresupuesto);

                        int idEvento = wedding.ObtenerIDEvento(this.proveedorEntidad, this.clienteEntidad);

                        string token = Api.ObtenerToken(proveedorEntidad);

                        Api.EnviaMensaje(proveedorEntidad, idEvento, token, "Su proveedor "+proveedorEntidad.nombre+" le acaba de generar un presupuesto, se lo enviará por email a la mayor brevedad posible.");
                    }
                    else
                    {
                        utils.MuestraMensajeInformacion("Ha ocurrido un problema al guardar las líneas del presupuesto.");
                    }
                }
                else
                {
                    utils.MuestraMensajeInformacion("Ha ocurrido un problema y el presupuesto no se ha insertado.");
                }
            }
            catch (Exception ex)
            {
                //SI entra aqui, eliminamos presupuesto
                try
                {
                    this.wedding.EliminaPresupuesto(idPresupuesto);
                    utils.MuestraMensajeError(ex.Message);
                }
                catch(Exception ex2)
                {
                    utils.MuestraMensajeError(ex2.Message);
                }
            }
            finally
            {
                bAcceptar.IsEnabled = true;
                this.NavigationService.Navigate(new PagePresupuestos(proveedorEntidad));
            }
        }

        /// <summary>
        /// funcion para inicializar los datos
        /// </summary>
        private void InicializaDatos()
        {
            try
            {
                if (this.clienteEntidad != null)
                {
                    this.cbClientes.Items.Add(this.clienteEntidad);
                    this.cbClientes.SelectedIndex = 0;
                    this.tbDireccion.Text = this.clienteEntidad.Direccion + ", "+ this.clienteEntidad.CP + ", " + this.clienteEntidad.Poblacion + ", " + this.clienteEntidad.Provincia;
                    this.cbClientes.IsEnabled = false;
                    this.tbDireccion.IsEnabled = false;
                }
                else
                {
                    this.listClientes = this.wedding.ObtenerClientes(proveedorEntidad);
                    this.cbClientes.ItemsSource = this.listClientes;
                }


                Dictionary<string, string> tipoIVAS = new Dictionary<string, string>();
                tipoIVAS.Add("0.4", "IVA Super reducido (4%)");
                tipoIVAS.Add("0.10", "IVA Reducido (10%)");
                tipoIVAS.Add("0.21", "IVA General (21%)");
                cbTipoIVA.ItemsSource = tipoIVAS;
                cbTipoIVA.DisplayMemberPath = "Value";
                cbTipoIVA.SelectedValuePath = "Key";
                cbTipoIVA.SelectedIndex = 2;

                lineasPresupuesto = new ObservableCollection<PresupuestoLineaEntidad>();
                this.dgPresupuestosLineas.ItemsSource = lineasPresupuesto;
            }
            catch(Exception ex)
            {
                utils.MuestraMensajeError(ex.Message);
            }
        }
    }
}
