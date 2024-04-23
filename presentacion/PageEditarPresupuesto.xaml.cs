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
    /// Lógica de interacción para PageEditarPresupuesto.xaml
    /// </summary>
    public partial class PageEditarPresupuesto : Page
    {
        /// <summary>
        /// declaracion de variables
        /// </summary>
        private proveedorEntidad proveedorEntidad;
        private clienteEntidad clienteEntidad;
        private PresupuestoEntidad presupuestoEntidad;
        private Wedding wedding;
        private BreadCrumb bcEditarPresupuesto;
        private ObservableCollection<PresupuestoLineaEntidad> lineasPresupuesto;
        private List<PresupuestoLineaEntidad> listLineasEditar;

        public PageEditarPresupuesto(proveedorEntidad pe, PresupuestoEntidad pree, List<PresupuestoLineaEntidad> listPreLineas, clienteEntidad ce)
        {
            InitializeComponent();
            this.proveedorEntidad = pe;
            this.clienteEntidad = ce;
            this.presupuestoEntidad = pree;
            this.wedding = new Wedding();
            this.InicializaDatos();
            this.bcEditarPresupuesto = new BreadCrumb { Nombre = "Editar Presupuesto" };
            MainWindow.CargaBread(bcEditarPresupuesto, false);
            this.dpFecha.SelectedDate = DateTime.Now;
            this.bAcceptar.Click += BAcceptar_Click;
            listLineasEditar = new List<PresupuestoLineaEntidad>();
            listLineasEditar = listPreLineas;
            CargarDatosPresupuesto();
            bCancelar.Click += BCancelar_Click;
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
        /// funcion para cargar las lineas a editar
        /// </summary>
        private void CargarDatosPresupuesto()
        {
            foreach(var linea in listLineasEditar)
            {
                lineasPresupuesto.Add(linea);
            }
            
        }

        /// <summary>
        /// evento al pulsar en el boton aceptar, donde generamos el presupuesto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BAcceptar_Click(object sender, RoutedEventArgs e)
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
                    }
                }
                importeIVATotal += Math.Round(importeBrutoTotal * ivaDecimal, 2);
                importeNetoTotal = Math.Round(importeBrutoTotal + importeIVATotal, 2);
                presupuestoEntidad.TipoIVA = tipoIVA;

                if (lineaDescripcionVacia)
                {
                    if (MessageBox.Show("Se ha detectado líneas sin descripción, a continuación se creará el presupuesto pero sin estas. ¿Esta seguro que desea continuar?", "Mensaje de confirmación", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                    {
                        GeneraPresupuesto(listLineasPresupuesto, importeBrutoTotal, importeIVATotal, importeNetoTotal);
                    }
                }
                else
                {
                    GeneraPresupuesto(listLineasPresupuesto, importeBrutoTotal, importeIVATotal, importeNetoTotal);
                }

            }
            catch (Exception ex)
            {
                utils.MuestraMensajeError(ex.Message);
            }
        }

        /// <summary>
        /// Funcion para crear el presupuesto y lineas, se utiliza prinicpalmente para evitar duplicar codigo, ya que se llama 2 veces
        /// </summary>
        /// <param name="bruto"></param>
        /// <param name="iva"></param>
        /// <param name="neto"></param>
        private void GeneraPresupuesto(List<PresupuestoLineaEntidad> lple, decimal bruto, decimal iva, decimal neto)
        {
            try
            {
                bAcceptar.IsEnabled = false;
                cbAceptado.IsEnabled = false;
                presupuestoEntidad.Importe_Bruto = bruto;
                presupuestoEntidad.Importe_IVA = iva;
                presupuestoEntidad.Importe_Neto = neto;
                if(cbAceptado.IsChecked == true)
                {
                    presupuestoEntidad.Aceptado = true;
                }
                else
                {
                    presupuestoEntidad.Aceptado = false;
                }

                if (this.wedding.ActualizarPresupuesto(presupuestoEntidad) > 0)
                {
                    this.wedding.EliminaLineasPresupuesto(presupuestoEntidad.Id);
                    this.wedding.InsertaLineasPresupuesto(lple, presupuestoEntidad.Id);

                    int idEvento = wedding.ObtenerIDEvento(this.proveedorEntidad, this.clienteEntidad);

                    string token = Api.ObtenerToken(proveedorEntidad);

                    if (presupuestoEntidad.Aceptado)
                    {
                        Api.EnviaMensaje(proveedorEntidad, idEvento, token, "Su proveedor " + proveedorEntidad.nombre + " le acaba de actualizar el presupuesto número " + presupuestoEntidad.Numero + " y se lo ha marcado como aceptado, se lo enviará por email a la mayor brevedad posible con los cambios."); 
                    }
                    else
                    {
                        Api.EnviaMensaje(proveedorEntidad, idEvento, token, "Su proveedor " + proveedorEntidad.nombre + " le acaba de actualizar el presupuesto número " + presupuestoEntidad.Numero + "y está pendiente de que lo acepte, se lo enviará por email a la mayor brevedad posible con los cambios.");

                    }
                }
                else
                {
                    utils.MuestraMensajeInformacion("Ha ocurrido un problema al actualizar el presupuesto.");
                }
            }
            catch (Exception ex)
            {
                //SI entra aqui, eliminamos presupuesto
                try
                {
                    this.wedding.EliminaPresupuesto(presupuestoEntidad.Id);
                    utils.MuestraMensajeError(ex.Message);
                }
                catch (Exception ex2)
                {
                    utils.MuestraMensajeError(ex2.Message);
                }
            }
            finally
            {
                cbAceptado.IsEnabled = true;
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
                tbCliente.Text = clienteEntidad.Nombre+" "+clienteEntidad.Apellidos;
                tbDireccion.Text = clienteEntidad.Direccion;
                if(presupuestoEntidad.Aceptado)
                {
                    cbAceptado.IsChecked = true;
                }
                else
                {
                    cbAceptado.IsChecked = false;
                }
                Dictionary<string, string> tipoIVAS = new Dictionary<string, string>();
                tipoIVAS.Add("0.4", "IVA Super reducido (4%)");
                tipoIVAS.Add("0.10", "IVA Reducido (10%)");
                tipoIVAS.Add("0.21", "IVA General (21%)");
                cbTipoIVA.ItemsSource = tipoIVAS;
                cbTipoIVA.DisplayMemberPath = "Value";
                cbTipoIVA.SelectedValuePath = "Key";
                cbTipoIVA.SelectedIndex = 2;

                switch(presupuestoEntidad.TipoIVA)
                {
                    case 21:
                        cbTipoIVA.SelectedIndex = 2;
                        break;
                    case 10:
                        cbTipoIVA.SelectedIndex = 1;
                        break;
                    case 4:
                        cbTipoIVA.SelectedIndex = 0;
                        break;
                }

                lineasPresupuesto = new ObservableCollection<PresupuestoLineaEntidad>();
                this.dgPresupuestosLineas.ItemsSource = lineasPresupuesto;
            }
            catch (Exception ex)
            {
                utils.MuestraMensajeError(ex.Message);
            }
        }
    }
}
