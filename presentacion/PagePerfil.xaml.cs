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
    /// Lógica de interacción para PagePerfil.xaml
    /// </summary>
    public partial class PagePerfil : Page
    {
        private proveedorEntidad proveedorEntidad;
        private BreadCrumb bcPerfil;
        private Wedding wedding;

        /// <summary>
        /// contructor con argumentos
        /// </summary>
        /// <param name="pe"></param>
        public PagePerfil(proveedorEntidad pe)
        {
            InitializeComponent();
            this.proveedorEntidad = pe;
            this.wedding = new Wedding();
            this.bcPerfil = new BreadCrumb { Nombre = "Perfil" };
            MainWindow.CargaBread(bcPerfil, false);
            this.RellenaDatos();
            this.bModificar.Click += BModificar_Click;
        }

        /// <summary>
        /// actualizamos los datos del proveedor al pulsar en modificar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BModificar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                proveedorEntidad pe = new proveedorEntidad
                {
                    cif = proveedorEntidad.cif,
                    cp = tbCP.Text,
                    direccion = tbDireccion.Text,
                    email = proveedorEntidad.email,
                    movil = tbMovil.Text,
                    nombre = tbNombre.Text,
                    password = "",
                    poblacion = tbPoblacion.Text,
                    provincia = tbProvincia.Text,
                    telefono = tbTel.Text
                };

                List<servicioEntidad> lServEntidad = new List<servicioEntidad>();

                foreach(servicioEntidad se in lvServicios.Items)
                {
                    if (se.Activo == 1)
                    {
                        lServEntidad.Add(se);
                    }
                }

                int actualizaPerfil = this.wedding.ActualizaDatosProveedor(pe, lServEntidad);

                if (actualizaPerfil > 0)
                {
                    utils.MuestraMensajeInformacion("Se han actualizado los datos correctamente.");
                    MainWindow.proveedorEntidad = pe;
                    this.NavigationService.Navigate(new PageHome(pe));
                }
                else
                {
                    if (actualizaPerfil == -1)
                    {
                        utils.MuestraMensajeInformacion("Ha ocurrido un problema y no se han modificado los datos. Pruebe de nuevo.");
                    }
                    else if (actualizaPerfil == -1)
                    {
                        utils.MuestraMensajeInformacion("Ha ocurrido un problema y no se han modificado los servicios. Pruebe de nuevo.");
                    }
                    else
                    {
                        utils.MuestraMensajeInformacion("Ha ocurrido un problema, pongase en contacto con servicio técnico.");
                    }
                }
            }
            catch(Exception ex)
            {
                utils.MuestraMensajeError(ex.Message);
            }
        }

        /// <summary>
        /// funcion para rellenar los datos del proveedor
        /// </summary>
        private void RellenaDatos()
        {
            this.tbEmail.Text = this.proveedorEntidad.email;
            this.tbNombre.Text = this.proveedorEntidad.nombre;
            this.tbDireccion.Text = this.proveedorEntidad.direccion;
            this.tbProvincia.Text = this.proveedorEntidad.provincia;
            this.tbPoblacion.Text = this.proveedorEntidad.poblacion;
            this.tbCP.Text = this.proveedorEntidad.cp;
            this.tbDNI.Text = this.proveedorEntidad.cif;
            this.tbMovil.Text = this.proveedorEntidad.movil;
            this.tbTel.Text = this.proveedorEntidad.telefono;
            //Todos los servicios
            List<servicioEntidad> lse_total = new List<servicioEntidad>();
            lse_total = this.wedding.ObtenerTotalServicios();

            //Servicios del proveedor
            List<servicioEntidad> lse = new List<servicioEntidad>();
            lse = this.wedding.ObtenerServiciosProveedor(this.proveedorEntidad);

            foreach (var servicioProv in lse)
            {
                foreach (var servicio in lse_total)
                {
                    if (servicioProv.Nombre == servicio.Nombre)
                    {
                        servicio.Activo = 1;
                    }
                }
            }

            this.lvServicios.ItemsSource = lse_total;
        }
    }
}
