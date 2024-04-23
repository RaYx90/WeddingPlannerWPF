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
/// <Author>JOSE MANUEL ESPARCIA</Author>
/// </summary>
namespace presentacion
{
    /// <summary>
    /// Lógica de interacción para PageNuevoEvento.xaml
    /// </summary>
    public partial class PageNuevoEvento : Page
    {
        /// <summary>
        /// declaración de variables
        /// </summary>
        private proveedorEntidad proveedorEntidad;
        private Wedding wedding;
        private BreadCrumb bcNuevoEvento;

        /// <summary>
        /// contructor con argumentos
        /// </summary>
        /// <param name="pe"></param>
        public PageNuevoEvento(proveedorEntidad pe)
        {
            InitializeComponent();
            this.proveedorEntidad = pe;
            this.wedding = new Wedding();
            this.bcNuevoEvento = new BreadCrumb { Nombre = "Nuevo Evento" };
            MainWindow.CargaBread(this.bcNuevoEvento, false);
            bEvento.Click += BEvento_Click;
            bCancelar.Click += BCancelar_Click;
            RellenaCiudades();
        }

        /// <summary>
        /// funcion para rellenar el combobox de ciudades
        /// </summary>
        private void RellenaCiudades()
        {
            string textoPorDefectoComboBoxCiudades = "Provincia";
            cbProvincia.Items.Add(textoPorDefectoComboBoxCiudades);
            cbProvincia2.Items.Add(textoPorDefectoComboBoxCiudades);
            List<string> lCiudades = utils.ListadoCiudades();
            foreach (var ciudad in lCiudades)
            {
                cbProvincia.Items.Add(ciudad);
                cbProvincia2.Items.Add(ciudad);
            }
            cbProvincia.SelectedIndex = 0;
            cbProvincia2.SelectedIndex = 0;
        }

        /// <summary>
        /// funcion para cancelar y volver a page eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PageEvento(proveedorEntidad));
        }

        /// <summary>
        /// funcion para dar de alta al cliente junto al evento creado y se lo asociamos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BEvento_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(tbNombre.Text) && !string.IsNullOrWhiteSpace(tbApellidos.Text) && !string.IsNullOrWhiteSpace(tbEmail.Text) && !string.IsNullOrWhiteSpace(tbDNI.Text) && !string.IsNullOrWhiteSpace(tbMovil.Text) &&
                   (!string.IsNullOrWhiteSpace(tbNombre2.Text) && !string.IsNullOrWhiteSpace(tbApellidos2.Text) && !string.IsNullOrWhiteSpace(tbEmail2.Text) && !string.IsNullOrWhiteSpace(tbDNI2.Text) && !string.IsNullOrWhiteSpace(tbMovil2.Text)))
                {
                    if (utils.ValidaEmail(tbEmail.Text) && utils.ValidaEmail(tbEmail2.Text))
                    {
                        if (!string.IsNullOrWhiteSpace(tbNombreEvento.Text) && dpFecha.SelectedDate != null)
                        {
                            int edad;
                            int edad2;
                            try
                            {
                                edad = Convert.ToInt32(tbEdad.Text);
                                edad2 = Convert.ToInt32(tbEdad2.Text);
                            }
                            catch
                            {
                                edad = 0;
                                edad2 = 0;
                            }

                            clienteEntidad clienteEnt = new clienteEntidad
                            {
                                Nombre = tbNombre.Text,
                                Apellidos = tbApellidos.Text,
                                DNI = tbDNI.Text,
                                Email = tbEmail.Text,
                                Movil = tbMovil.Text,
                                CP = tbCP.Text,
                                Direccion=tbDireccion.Text,
                                Edad = edad,
                                Evento = tbNombreEvento.Text,
                                Poblacion = tbPoblacion.Text,
                                Provincia = cbProvincia.SelectedItem.ToString(),
                                Telefono =""
                            };

                            clienteEntidad clienteEnt2 = new clienteEntidad
                            {
                                Nombre = tbNombre2.Text,
                                Apellidos = tbApellidos2.Text,
                                DNI = tbDNI2.Text,
                                Email = tbEmail2.Text,
                                Movil = tbMovil2.Text,
                                CP = tbCP2.Text,
                                Direccion = tbDireccion2.Text,
                                Edad = edad2,
                                Evento = tbNombreEvento.Text,
                                Poblacion = tbPoblacion2.Text,
                                Provincia = cbProvincia2.SelectedItem.ToString(),
                                Telefono = ""
                            };

                            long fechaNacClienteINT = utils.ConvertToTimestamp(dpFechaNacCliente.SelectedDate.Value);
                            int fechaNacTimestampInt = Convert.ToInt32(fechaNacClienteINT);

                            long fechaNacClienteINT2 = utils.ConvertToTimestamp(dpFechaNacCliente.SelectedDate.Value);
                            int fechaNacTimestampInt2 = Convert.ToInt32(fechaNacClienteINT2);

                            if (this.wedding.InsertaCliente(clienteEnt, fechaNacTimestampInt) > 0 && this.wedding.InsertaCliente(clienteEnt2, fechaNacTimestampInt2) > 0)
                            {
                                int idCliente = this.wedding.ObtenerIDCliente(clienteEnt);
                                int idCliente2 = this.wedding.ObtenerIDCliente(clienteEnt2);

                                if (idCliente > 0 && idCliente2 > 0)
                                {
                                    int eventoActivo;

                                    if (cbActivo.IsChecked == true)
                                    {
                                        eventoActivo = 1;
                                    }
                                    else
                                    {
                                        eventoActivo = 0;
                                    }

                                    DateTime dt = dpFecha.SelectedDate.Value;
                                    long fechaTimestamp = utils.ConvertToTimestamp(dpFecha.SelectedDate.Value);
                                    int fechaTimestampInt = Convert.ToInt32(fechaTimestamp);

                                    eventoEntidad eventEnt = new eventoEntidad
                                    {
                                        nombre = tbNombreEvento.Text,
                                        descripcion = "Boda de " + tbNombre.Text +" y "+ tbNombre2.Text,
                                        activo = eventoActivo,
                                        fechaInt = fechaTimestampInt
                                    };

                                    int idEventoGenerado = this.wedding.InsertEvento(eventEnt);

                                    if (idEventoGenerado > 0)
                                    {
                                        int idProveedor = this.wedding.ObtenerIDProveedorLogin(proveedorEntidad);
                                        int insertRelacional = this.wedding.InsertProveedoresEventoCliente(idProveedor, idCliente, idEventoGenerado, true);
                                        int insertRelacional2 = this.wedding.InsertProveedoresEventoCliente(idProveedor, idCliente2, idEventoGenerado, false);

                                        if (insertRelacional > 0)
                                        {
                                            //utils.MuestraMensajeInformacion("Se ha creado el evento.");
                                            this.NavigationService.Navigate(new PageEvento(proveedorEntidad));
                                        }
                                        else
                                        {
                                            utils.MuestraMensajeInformacion("Ha ocurrido un problema al generar el evento.");
                                            this.NavigationService.Navigate(new PageEvento(proveedorEntidad));
                                        }
                                    }
                                    else
                                    {
                                        utils.MuestraMensajeInformacion("No se ha podido crear el evento u obtener su identificador.");
                                        this.NavigationService.Navigate(new PageEvento(proveedorEntidad));
                                    }
                                }
                                else
                                {
                                    utils.MuestraMensajeInformacion("No se ha podido obtener el identificador de cliente para generar el evento.");
                                    this.NavigationService.Navigate(new PageEvento(proveedorEntidad));
                                }
                            }
                            else
                            {
                                utils.MuestraMensajeInformacion("No se ha podido crear el cliente.");
                                this.NavigationService.Navigate(new PageEvento(proveedorEntidad));
                            }
                        }
                        else
                        {
                            utils.MuestraMensajeInformacion("Debe rellenar todos los datos del evento");
                        }
                    }
                    else
                    {
                        utils.MuestraMensajeInformacion("El email debe tener un formato válido");
                    }
                }
                else
                {
                    utils.MuestraMensajeInformacion("Debe rellenar todos los datos del cliente");
                }
            }
            catch(Exception ex)
            {
                utils.MuestraMensajeError(ex.Message);
                this.NavigationService.Navigate(new PageEvento(proveedorEntidad));
            }
        }
    }
}
