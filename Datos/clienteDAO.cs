using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// JOSE MANUEL ESPARCIA CAÑIZARES
/// </summary>
namespace Datos
{
    public class clienteDAO
    {
        /// <summary>
        /// declaracion de variables
        /// </summary>
        private BaseDeDatos bd;

        /// <summary>
        /// contructor de la clase
        /// </summary>
        public clienteDAO()
        {
            this.bd = new BaseDeDatos();
        }

        /// <summary>
        /// funcion devolver los clientes totales de un proveedor en concreto
        /// </summary>
        public List<clienteEntidad> ObtenerTotalClientes(proveedorEntidad pe)
        {
            List<clienteEntidad> lclientes;
            clienteEntidad clienteEntidad;
            MySqlDataReader lector;
            string sqlSelect;

            try
            {
                lclientes = new List<clienteEntidad>();
                this.bd.RealizaConexion();
                // sqlSelect = @"SELECT c.nombre, c.apellidos, c.direccion, c.poblacion, c.provincia, c.cp, c.email, c.edad, c.telefono, c.movil, c.dni, e.nombre FROM proveedores_eventos_clientes pec, clientes c, proveedores pr, eventos e WHERE pec.ref_proveedor = pr.id AND pec.ref_cliente = c.id AND pec.ref_evento = e.id AND pr.email = '" + pe.email+ "' ORDER BY c.id DESC";

                sqlSelect = @"SELECT c.nombre, c.apellidos, c.direccion, c.poblacion, c.provincia, c.cp, c.email, c.edad, c.telefono, c.movil, c.dni, GROUP_CONCAT(e.nombre)
                            FROM proveedores_eventos_clientes pec, clientes c, eventos e
                            WHERE pec.ref_cliente = c.id
                            AND pec.ref_evento = e.id
                            AND ref_cliente IS NOT NULL
                            AND pec.ref_evento IN(SELECT DISTINCT pec.ref_evento
                                                    FROM proveedores_eventos_clientes pec, proveedores pr
                                                    WHERE pec.ref_proveedor = pr.id
                                                    AND pr.email = '" + pe.email + "') GROUP BY c.id ORDER BY c.id DESC";



                lector = this.bd.ConsultaDML(sqlSelect);
                while (lector.Read())
                {
                    clienteEntidad = new clienteEntidad
                    {
                        Nombre = lector[0].ToString(),
                        Apellidos = lector[1].ToString(),
                        Direccion = lector[2].ToString(),
                        Poblacion = lector[3].ToString(),
                        Provincia = lector[4].ToString(),
                        CP = lector[5].ToString(),
                        Email = lector[6].ToString(),
                        Edad = Convert.ToInt32(lector[7]),
                        Telefono = lector[8].ToString(),
                        Movil = lector[9].ToString(),
                        DNI = lector[10].ToString(),
                        Evento = lector[11].ToString()
                    };
                    lclientes.Add(clienteEntidad);
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                lclientes = null;
                Debug.WriteLine("Ha ocurrido un error al obtener el total de clientes: " + ex.Message);
                throw new Exception("Ha ocurrido un error al obtener el total de clientes: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return lclientes;
        }

        /// <summary>
        /// funcion para obtener el ID del cliente en concreto
        /// </summary>
        /// <param name="pe"></param>
        /// <returns></returns>
        public int ObtenIDCliente(clienteEntidad ce)
        {
            int idCliente = 0;
            MySqlDataReader lector;
            string sqlSelect;

            try
            {
                this.bd.RealizaConexion();
                sqlSelect = @"SELECT id FROM clientes WHERE (email = '" + ce.Email + "' AND dni = '" + ce.DNI + "')";
                lector = this.bd.ConsultaDML(sqlSelect);
                while (lector.Read())
                {
                    idCliente = Convert.ToInt16(lector[0]);
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ha ocurrido un error al obtener el Identificador de cliente: " + ex.Message);
                throw new Exception("Ha ocurrido un error al obtener el Identificador de cliente: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return idCliente;
        }

        /// <summary>
        /// funcion para obtener el ID del cliente en concreto
        /// </summary>
        /// <param name="pe"></param>
        /// <returns></returns>
        public clienteEntidad ObtenDatosClientePorID(int idCliente)
        {
            clienteEntidad ce;
            MySqlDataReader lector;
            string sqlSelect;

            try
            {
                ce = new clienteEntidad();
                this.bd.RealizaConexion();
                sqlSelect = @"SELECT nombre, apellidos, direccion, poblacion, provincia, cp, email, edad, telefono, movil, dni FROM clientes WHERE id = " + idCliente + "";
                lector = this.bd.ConsultaDML(sqlSelect);
                while (lector.Read())
                {
                    ce = new clienteEntidad
                    {
                        Nombre = lector[0].ToString(),
                        Apellidos = lector[1].ToString(),
                        Direccion = lector[2].ToString(),
                        Poblacion = lector[3].ToString(),
                        Provincia = lector[4].ToString(),
                        CP = lector[5].ToString(),
                        Email = lector[6].ToString(),
                        Edad = Convert.ToInt32(lector[7]),
                        Telefono = lector[8].ToString(),
                        Movil = lector[9].ToString(),
                        DNI = lector[10].ToString()
                    };
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ha ocurrido un error al obtener los datos de cliente: " + ex.Message);
                throw new Exception("Ha ocurrido un error al obtener los datos de cliente: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return ce;
        }

        /// <summary>
        /// Inserta un cliente en la base de datos -> tabla clientes
        /// </summary>
        /// <returns></returns>
        public int InsertCliente(clienteEntidad ce, int fechaNacTimestamp)
        {
            string sql;
            int filasInsertadas = 0;

            try
            {
                this.bd.RealizaConexion();

                sql = @"INSERT INTO clientes(nombre, apellidos, email, movil, dni, direccion, poblacion, provincia, cp, fnac, edad, telefono, password) 
                                      VALUES('" + ce.Nombre + "','" + ce.Apellidos + "','" + ce.Email + "','" + ce.Movil + "','"  + ce.DNI + "', '" + ce.Direccion + "', '" + ce.Poblacion + "', '" + ce.Provincia + "', '" + ce.CP + "'," + fechaNacTimestamp + ", "+ce.Edad+", '', '" + this.MD5Hash(ce.Email)+"'); ";
                this.bd.ordenMySQL = new MySqlCommand(sql, this.bd.mySqlConnection);
                this.bd.ordenMySQL.CommandText = sql;
                filasInsertadas = this.bd.EjecutarDML();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al insertar el cliente: " + ex.Message);
                filasInsertadas = 0;
                throw new Exception("Error al insertar el cliente: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return filasInsertadas;
        }

        /// <summary>
        /// Encriptamos el pass del usuario en MD5
        /// </summary>
        /// <param name="pass">password sin encriptar</param>
        /// <returns></returns>
        private string MD5Hash(string pass)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(pass));
            byte[] passEncrypt = md5.Hash;
            StringBuilder passMD5 = new StringBuilder();

            for (int i = 0; i < passEncrypt.Length; i++)
            {
                passMD5.Append(passEncrypt[i].ToString("x2"));
            }

            return passMD5.ToString();
        }
    }
}
