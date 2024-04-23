using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// JOSE MANUEL ESPARCIA CAÑIZARES
/// </summary>
namespace Datos
{
    public class proveedorDAO
    {
        /// <summary>
        /// declaracion de variables
        /// </summary>
        private BaseDeDatos bd;

        /// <summary>
        /// contructor de la clase
        /// </summary>
        public proveedorDAO()
        {
            this.bd = new BaseDeDatos();
        }

        /// <summary>
        /// funcion para comprobar si el usuario existe y devolver sus datos
        /// </summary>
        /// <param name="userlogin">nombre del usuario pasado por el form de login</param>
        /// <param name="passwordlogin">password del usuario pasado por el form de login</param>
        /// <returns></returns>
        public proveedorEntidad CompruebaProveedorLogin(string userlogin, string passwordlogin)
        {
            proveedorEntidad proveedorEntidad = null;
            MySqlDataReader lector;
            string sqlSelect;

            try
            {
                this.bd.RealizaConexion();
                sqlSelect = @"SELECT nombre, direccion, poblacion, provincia, cp, telefono, movil, email, password, cif
                              FROM proveedores
                              WHERE email = '" + userlogin +
                              "' AND password = '" + passwordlogin +"'";
                lector = this.bd.ConsultaDML(sqlSelect);
                while (lector.Read())
                {

                    proveedorEntidad = new proveedorEntidad
                    {
                        nombre = lector[0].ToString(),
                        direccion = lector[1].ToString(),
                        poblacion = lector[2].ToString(),
                        provincia = lector[3].ToString(),
                        cp = lector[4].ToString(),
                        telefono = lector[5].ToString(),
                        movil = lector[6].ToString(),
                        email = lector[7].ToString(),
                        password = lector[8].ToString(),
                        cif = lector[9].ToString(),
                    };
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ha ocurrido un error al obtener los datos del proveedor: " + ex.Message);
                proveedorEntidad = null;
                throw new Exception("Ha ocurrido un error al obtener los datos del proveedor: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return proveedorEntidad;
        }

        /// <summary>
        /// funcion para actualizar el password
        /// </summary>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public int ModificaPasswordProveedor(string newPassword, string nickUser, string cif)
        {
            string sql;
            int filasActualizadas = 0;

            try
            {
                this.bd.RealizaConexion();
                sql = @"UPDATE proveedores SET password = '"+ newPassword + "' WHERE email = '"+ nickUser + "' AND cif = '"+ cif + "'";
                this.bd.ordenMySQL = new MySqlCommand(sql, this.bd.mySqlConnection);
                this.bd.ordenMySQL.CommandText = sql;
                filasActualizadas = this.bd.EjecutarDML();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al actualizar el password del usuario: " + ex.Message);
                filasActualizadas = 0;
                throw new Exception(ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return filasActualizadas;
        }

        /// <summary>
        /// funcion para obtener el ID del proveedor en concreto
        /// </summary>
        /// <param name="pe"></param>
        /// <returns></returns>
        public int ObtenIDProveedor(proveedorEntidad pe)
        {
            int idProveedor = 0;
            MySqlDataReader lector;
            string sqlSelect;

            try
            {
                this.bd.RealizaConexion();
                sqlSelect = @"SELECT id FROM proveedores WHERE (email = '" + pe.email + "' AND cif = '" + pe.cif + "')";
                lector = this.bd.ConsultaDML(sqlSelect);
                while (lector.Read())
                {
                    idProveedor = Convert.ToInt16(lector[0]);
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ha ocurrido un error al obtener el Identificador de proveedores: " + ex.Message);
                throw new Exception("Ha ocurrido un error al obtener el Identificador de proveedores: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return idProveedor;
        }

        /// <summary>
        /// funcion para comprobar si el usuario existe y no permitir darlo de alta nuevamente
        /// </summary>
        public int CompruebaExistenciaProveedor(proveedorEntidad pe)
        {
            int contadorProveedores = 0;
            MySqlDataReader lector;
            string sqlSelect;

            try
            {
                this.bd.RealizaConexion();
                sqlSelect = @"SELECT count(*) FROM proveedores WHERE (email = '" + pe.email +"' OR cif = '"+ pe.cif+"')";
                lector = this.bd.ConsultaDML(sqlSelect);
                while (lector.Read())
                {
                    contadorProveedores = Convert.ToInt16(lector[0]);
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ha ocurrido un error al obtener el total de proveedores: " + ex.Message);
                throw new Exception("Ha ocurrido un error al obtener el total de proveedores: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return contadorProveedores;
        }

        /// <summary>
        /// Inserta un proveedor en la base de datos -> tabla proveedores
        /// </summary>
        /// <returns></returns>
        public int InsertProveedor(proveedorEntidad pe)
        {
            string sql;
            int filasInsertadas = 0;

            try
            {
                this.bd.RealizaConexion();

                sql = @"INSERT INTO proveedores(nombre, direccion, poblacion, provincia, cp, telefono, movil, email, password, cif) VALUES('"+ pe.nombre+"','"+pe.direccion + "','" +pe.poblacion + "','" + pe.provincia +"','" + pe.cp + "','" + pe.telefono + "','" + pe.movil + "','" + pe.email + "','" + pe.password + "','" + pe.cif + "'); ";
                this.bd.ordenMySQL = new MySqlCommand(sql, this.bd.mySqlConnection);
                this.bd.ordenMySQL.CommandText = sql;
                filasInsertadas = this.bd.EjecutarDML();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al insertar el proveedor: " + ex.Message);
                filasInsertadas = 0;
                throw new Exception("Error al insertar el proveedor: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return filasInsertadas;
        }

        /// <summary>
        /// Modifica un proveedor en la base de datos -> tabla proveedores
        /// </summary>
        /// <returns></returns>
        public int UpdateProveedor(proveedorEntidad pe, List<servicioEntidad> lse)
        {
            string sql;
            int filasActualizadas = 0;
            string sqlDeleteServicio;
            string sqlInsertServicio;

            try
            {
                this.bd.RealizaConexion();

                sql = @"UPDATE proveedores SET nombre = '"+pe.nombre+ "', direccion = '" + pe.direccion + "', poblacion = '" + pe.poblacion + "', provincia = '" + pe.provincia + "', cp = '" + pe.cp + "', telefono = '" + pe.telefono + "', movil = '" + pe.movil + "' WHERE email = '" + pe.email + "' AND cif = '" + pe.cif + "'; ";
                this.bd.ordenMySQL = new MySqlCommand(sql, this.bd.mySqlConnection);
                this.bd.ordenMySQL.CommandText = sql;
                filasActualizadas = this.bd.EjecutarDML();
                this.bd.CerrarConexion();

                try
                {
                    int idProveedor = ObtenIDProveedor(pe);
                    this.bd.RealizaConexion();
                    ///Eliminamos todos los servicios de ese proveedor
                    sqlDeleteServicio = "DELETE FROM proveedores_servicios WHERE ref_proveedor = " + idProveedor;
                    this.bd.ordenMySQL = new MySqlCommand(sql, this.bd.mySqlConnection);
                    this.bd.ordenMySQL.CommandText = sqlDeleteServicio;
                    filasActualizadas = this.bd.EjecutarDML();

                    servicioDAO sd = new servicioDAO();

                    foreach (int idServicioInsertar in sd.ObtenerIDServicios(lse))
                    {
                        sqlInsertServicio = "INSERT INTO proveedores_servicios (ref_proveedor, ref_servicio) VALUES ('"+idProveedor+"', '"+idServicioInsertar+"')";
                        this.bd.ordenMySQL = new MySqlCommand(sqlInsertServicio, this.bd.mySqlConnection);
                        this.bd.ordenMySQL.CommandText = sqlInsertServicio;
                        filasActualizadas = this.bd.EjecutarDML();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error al actualizar los servicios: " + ex.Message);
                    filasActualizadas = -2;
                    throw new Exception("Error al actualizar los servicios: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al modificar el proveedor: " + ex.Message);
                filasActualizadas = -1;
                throw new Exception("Error al modificar el proveedor: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return filasActualizadas;
        }
    }
}
