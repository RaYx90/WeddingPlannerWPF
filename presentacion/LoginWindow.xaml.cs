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
using System.Windows.Shapes;

/// <summary>
/// <author>JOSE MANUEL ESPARCIA CAÑIZARES</author>
/// </summary>
namespace presentacion
{
    /// <summary>
    /// Lógica de interacción para LoginWindows.xaml
    /// </summary>
    public partial class LoginWindows : Window
    {
        /// <summary>
        /// Variables
        /// </summary>
        private Wedding wedding;

        /// <summary>
        /// constructor sin argumentos
        /// </summary>
        public LoginWindows()
        {
            InitializeComponent();
            this.bAcceder.Click += BAcceder_Click;
            this.wedding = new Wedding();
            this.bRegistrarse.Click += BRegistrarse_Click;
            this.bCancelarRegistro.Click += BCancelarRegistro_Click;
            this.bRealizarRegistro.Click += BRealizarRegistro_Click;
            this.RellenaCiudades();
            this.bRecuperaPass.Click += BRecuperaPass_Click;
            this.bCambiarPassword.Click += BCambiarPassword_Click;
            this.bCancelarRecPassword.Click += BCancelarRecPassword_Click;
            this.CompruebaFicheroCredenciales(null);
        }

        /// <summary>
        /// funcion para salir de la ventana de recuperar password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BCancelarRecPassword_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Desea cancelar el cambio de contraseña?", "Mensaje de confirmación", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                this.ReseteaDatosRecuperacionPass();
            }
        }

        /// <summary>
        /// funcion para resetear datos de recuperacion de pass
        /// </summary>
        private void ReseteaDatosRecuperacionPass()
        {
            this.tbUsuarioRecuperar.Text = "";
            this.tbNifRecuperar.Text = "";
            this.pbContrasenyaRecuperar.Password = "";
            this.pbContrasenyaConfirmarRecuperar.Password = "";
            this.gbLogin.Visibility = Visibility.Visible;
            this.gbRecuperarPassword.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// funcion para actualizar el password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BCambiarPassword_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.tbUsuarioRecuperar.Text) && !string.IsNullOrEmpty(this.tbNifRecuperar.Text) && !string.IsNullOrEmpty(this.pbContrasenyaRecuperar.Password) && !string.IsNullOrEmpty(this.pbContrasenyaConfirmarRecuperar.Password))
            {
                if (this.pbContrasenyaRecuperar.Password == this.pbContrasenyaConfirmarRecuperar.Password)
                {
                    if (utils.ValidaEmail(this.tbUsuarioRecuperar.Text))
                    {
                        if(utils.ValidaPassword(this.pbContrasenyaRecuperar.Password))
                        {
                            try
                            {
                                if (this.wedding.ActualizarPasswordProveedor(utils.MD5Hash(this.pbContrasenyaRecuperar.Password), this.tbUsuarioRecuperar.Text, this.tbNifRecuperar.Text) > 0)
                                {
                                    utils.MuestraMensajeInformacion("La contraseña se ha actualizado correctamente.");
                                    this.ReseteaDatosRecuperacionPass();
                                }
                                else
                                {
                                    utils.MuestraMensajeInformacion("No se actualizó la contraseña.");
                                }
                            }
                            catch(Exception ex)
                            {
                                utils.MuestraMensajeError(ex.Message);
                            }
                        }
                        else
                        {
                            utils.MuestraMensajeInformacion("La contraseñas debe contener una letra minúscula, una mayúscula y un número.");
                        }
                    }
                    else
                    {
                        utils.MuestraMensajeInformacion("Debe escribir una cuenta de email válida.");
                    }
                }
                else
                {
                    utils.MuestraMensajeInformacion("Las contraseñas deben coincidir.");
                }
            }
            else
            {
                utils.MuestraMensajeInformacion("Todos los campos son obligatorios para poder realizar el cambio de contraseña.");
            }
        }

        /// <summary>
        /// funcion para cambiar a la pantalla de recuperar contraseña
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BRecuperaPass_Click(object sender, RoutedEventArgs e)
        {
            gbLogin.Visibility = Visibility.Hidden;
            gbRecuperarPassword.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// funcion para realizar el registro de un nuevo proveedor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BRealizarRegistro_Click(object sender, RoutedEventArgs e)
        {
            List<string> lcampos = new List<string>();
            lcampos.Add(tbUsuarioRegistro.Text);
            lcampos.Add(pbContrasenyaRegistro.Password);
            lcampos.Add(pbContrasenyaRepiteRegistro.Password);
            lcampos.Add(tbNombreRegistro.Text);
            lcampos.Add(tbDNIRegistro.Text);
            lcampos.Add(tbDireccionRegistro.Text);
            lcampos.Add(cbProvinciaRegistro.SelectedItem.ToString());
            lcampos.Add(tbPoblacionRegistro.Text);
            lcampos.Add(tbCPRegistro.Text);
            lcampos.Add(tbTelefonoRegistro.Text);
            lcampos.Add(tbMovilRegistro.Text);
            ValidarCampos();
        }
        /// <summary>
        /// Funcion para cancelar el registro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BCancelarRegistro_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Desea cancelar el registro?", "Mensaje de confirmación", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                ReseteaDatosRegistro();
            } 
        }

        /// <summary>
        /// Funcion para cambiar de login a registro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BRegistrarse_Click(object sender, RoutedEventArgs e)
        {
            this.gbLogin.Visibility = Visibility.Hidden;
            this.gbRegistro.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// funcion para crear el fichero de credenciales u obtener sus valores
        /// </summary>
        private void CompruebaFicheroCredenciales(proveedorEntidad pe)
        {
            try
            {
                if (!Directory.Exists(Rutas.rutaCarpetaCredentials))
                {
                    Directory.CreateDirectory(Rutas.rutaCarpetaCredentials);
                }

                if (!File.Exists(Rutas.rutaCredentials) && (pe != null))
                {
                    using (StreamWriter sw = new StreamWriter(Rutas.rutaCredentials))
                    {
                        //Guardamos los valores
                        sw.WriteLine("user:" + pe.email);
                        sw.WriteLine("pass:" + pe.password);

                        //Cerramos el fichero
                        sw.Close();
                    }     
                }
                else
                {
                    if (File.Exists(Rutas.rutaCredentials))
                    {
                        string linea;

                        using (StreamReader sr = new StreamReader(Rutas.rutaCredentials))
                        {
                            linea = sr.ReadLine();

                            while (linea != null)
                            {
                                string[] palabras = linea.Split(':');

                                if (palabras[0] == "user")
                                {
                                    this.tbUsuario.Text = palabras[1];
                                }

                                if (palabras[0] == "pass")
                                {
                                    this.pbContrasenya.Password = palabras[1];
                                }

                                linea = sr.ReadLine();

                            }
                        }
                        this.cbRecordarCredenciales.IsChecked = true;
                        proveedorEntidad proveedorEntidad = this.wedding.CompruebaLogin(this.tbUsuario.Text, this.pbContrasenya.Password);
                        if (proveedorEntidad != null)
                        {
                            MainWindow mw = new MainWindow(proveedorEntidad);
                            mw.Show();
                            this.Close();
                        }
                        else
                        {
                            utils.MuestraMensajeInformacion("El usuario o contraseña son incorrectos.");
                            this.pbContrasenya.Password = "";
                            File.Delete(Rutas.rutaCredentials);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                utils.MuestraMensajeInformacion(ex.Message);
            }
        }

        /// <summary>
        /// funcion para entrar al menu principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BAcceder_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(tbUsuario.Text) && !string.IsNullOrEmpty(pbContrasenya.Password))
            {
                try
                {
                    proveedorEntidad pe = this.wedding.CompruebaLogin(tbUsuario.Text, utils.MD5Hash(pbContrasenya.Password));
                    if (pe != null)
                    {
                        if(cbRecordarCredenciales.IsChecked == true)
                        {
                            CompruebaFicheroCredenciales(pe);
                        }
                        MainWindow mw = new MainWindow(pe);
                        mw.Show();
                        this.Close();
                    }
                    else
                    {
                        utils.MuestraMensajeInformacion("El usuario o contraseña son incorrectos.");
                    }
                }
                catch(Exception ex)
                {
                    utils.MuestraMensajeError(ex.Message);
                }
            }
            else
            {
                utils.MuestraMensajeError("El usuario y la contraseña son obligatorios.");
            }
        }


        /// <summary>
        /// Funcion para validar todos los campos, si devuelve true, lo modifica, sino no. No lo meto en utils porque hay campos menos campos
        /// </summary>
        /// <returns></returns>
        private bool ValidarCampos()
        {
            if (utils.ValidaEmail(this.tbUsuarioRegistro.Text))
            {
                if ((this.pbContrasenyaRegistro.Password == this.pbContrasenyaRepiteRegistro.Password) && (!string.IsNullOrEmpty(this.pbContrasenyaRegistro.Password)) && (!string.IsNullOrEmpty(this.pbContrasenyaRegistro.Password)))
                {
                    if(utils.ValidaPassword(this.pbContrasenyaRegistro.Password))
                    {
                        if((this.tbNombreRegistro.Text.Contains("Nombre o razón social")) || (this.tbDireccionRegistro.Text.Contains("Dirección")) ||(this.tbPoblacionRegistro.Text.Contains("Población")) || (this.cbProvinciaRegistro.Text.Contains("Seleccione una provincia")) || (this.tbCPRegistro.Text.Contains("Código Postal")) || (this.tbDNIRegistro.Text.Contains("DNI/CIF")) || (this.tbTelefonoRegistro.Text.Contains("Teléfono")) || (this.tbMovilRegistro.Text.Contains("Móvil")))
                        {
                            utils.MuestraMensajeInformacion("Todos los campos son obligatorios, por favor rellenelos.");
                            return false;
                        }
                        else
                        {
                            try
                            {
                                proveedorEntidad pe = new proveedorEntidad
                                {
                                    nombre = this.tbNombreRegistro.Text,
                                    direccion = this.tbDireccionRegistro.Text,
                                    poblacion = this.tbPoblacionRegistro.Text,
                                    provincia = this.cbProvinciaRegistro.SelectedItem.ToString(),
                                    cp = this.tbCPRegistro.Text,
                                    cif = this.tbDNIRegistro.Text,
                                    email = this.tbUsuarioRegistro.Text,
                                    password = utils.MD5Hash(this.pbContrasenyaRegistro.Password),
                                    movil = this.tbMovilRegistro.Text,
                                    telefono = this.tbTelefonoRegistro.Text
                                };

                                int contador = this.wedding.ContadorProveedores(pe);

                                if(contador == 0)
                                {
                                    if (this.wedding.InsertaProveedor(pe) > 0)
                                    {
                                        utils.MuestraMensajeInformacion("Se ha registrado correctamente en la aplicación.");
                                        this.tbUsuario.Text = tbUsuarioRegistro.Text;
                                        this.pbContrasenya.Visibility = Visibility.Visible;
                                        this.pbContrasenya.Password = pbContrasenyaRegistro.Password;
                                        this.ReseteaDatosRegistro();
                                        this.gbLogin.Visibility = Visibility.Visible;
                                        this.gbRegistro.Visibility = Visibility.Hidden;

                                        return true;
                                    }
                                    else
                                    {
                                        utils.MuestraMensajeInformacion("Ha ocurrido un error durante el registro.");
                                        return true;
                                    }
                                }
                                else
                                {
                                    utils.MuestraMensajeInformacion("No es posible realizar el registro, el usuario ya existe (ponga otro CIF/NIF y email).");
                                    return false;
                                }
                            }
                            catch(Exception ex)
                            {
                                utils.MuestraMensajeError("Ha ocurrido el siguiente error: "+ex.Message);
                                return false;
                            }
                        }
                    }
                    else
                    {
                        utils.MuestraMensajeInformacion("La contraseña debe contener almenos 1 letra mayúscula, 1 letra minúscula y un número.");
                        return false;
                    }
                }
                else
                {
                    utils.MuestraMensajeInformacion("Las contraseñas no coinciden.");
                    return false;
                }
            }
            else
            {
                utils.MuestraMensajeInformacion("La cuenta de correo no tiene un formato válido.");
                return false;
            }
        }

        /// <summary>
        /// funcion para restablecer los controles del registro y volver al menu de login
        /// </summary>
        private void ReseteaDatosRegistro()
        {
            this.pbContrasenyaRegistro.Password = "";
            this.pbContrasenyaRepiteRegistro.Password = "";
            this.tbUsuarioRegistro.Text = "";
            this.tbNombreRegistro.Text = "";
            this.tbDireccionRegistro.Text = "";
            this.tbTelefonoRegistro.Text = "";
            this.tbCPRegistro.Text = "";
            this.tbMovilRegistro.Text = "";
            this.cbProvinciaRegistro.Text = "Seleccione una provincia";
            this.tbPoblacionRegistro.Text = "";
            this.tbDNIRegistro.Text = "";
            this.gbRegistro.Visibility = Visibility.Hidden;
            this.gbLogin.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// funcion para rellenar el combobox de ciudades
        /// </summary>
        private void RellenaCiudades()
        {
            string textoPorDefectoComboBoxCiudades = "Seleccione una provincia";
            cbProvinciaRegistro.Items.Add(textoPorDefectoComboBoxCiudades);
            List<string> lCiudades = utils.ListadoCiudades();
            foreach(var ciudad in lCiudades)
            {
                cbProvinciaRegistro.Items.Add(ciudad);
            }
            cbProvinciaRegistro.SelectedIndex = 0;
        }
    }
}
