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
    public class eventoDAO
    {
        /// <summary>
        /// declaracion de variables
        /// </summary>
        private BaseDeDatos bd;

        /// <summary>
        /// contructor de la clase
        /// </summary>
        public eventoDAO()
        {
            this.bd = new BaseDeDatos();
        }

        /// <summary>
        /// Inserta un evento en la base de datos -> tabla eventos y cliente en la tabla->clientes
        /// </summary>
        /// <returns></returns>
        public int InsertEvento(eventoEntidad ee)
        {
            string sql;
            int filasInsertadas = 0;
            MySqlDataReader lector;
            int idEventoInsertado = 0;

            try
            {
                this.bd.RealizaConexion();

                sql = @"INSERT INTO eventos(nombre, descripcion, fecha, activo) VALUES('" + ee.nombre + "','" + ee.descripcion + "'," + ee.fechaInt + "," + ee.activo + "); ";
                this.bd.ordenMySQL = new MySqlCommand(sql, this.bd.mySqlConnection);
                this.bd.ordenMySQL.CommandText = sql;
                filasInsertadas = this.bd.EjecutarDML();

                if(filasInsertadas > 0)
                {
                    sql = @"SELECT MAX(id) FROM eventos";
                    lector = this.bd.ConsultaDML(sql);
                    while (lector.Read())
                    {
                        idEventoInsertado = Convert.ToInt32(lector[0]);
                    }
                    lector.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al insertar el evento: " + ex.Message);
                filasInsertadas = 0;
                throw new Exception("Error al insertar el evento: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return idEventoInsertado;
        }

        /// <summary>
        /// Inserta un evento en la base de datos -> tabla ProveedoresEventoCliente
        /// </summary>
        /// <returns></returns>
        public int InsertProveedoresEventoCliente(int idPro, int idCli, int idEvent, bool insertaProv)
        {
            string sql;
            int filasInsertadas = 0;

            try
            {
                this.bd.RealizaConexion();
                if (insertaProv)
                {
                    sql = @"INSERT INTO proveedores_eventos_clientes(ref_proveedor, ref_evento) VALUES (" + idPro + ", " + idEvent + ")";
                    this.bd.ordenMySQL = new MySqlCommand(sql, this.bd.mySqlConnection);
                    this.bd.ordenMySQL.CommandText = sql;
                    if (this.bd.EjecutarDML() > 0)
                    {
                        sql = @"INSERT INTO proveedores_eventos_clientes(ref_cliente, ref_evento) VALUES (" + idCli + ", " + idEvent + ")";
                        this.bd.ordenMySQL = new MySqlCommand(sql, this.bd.mySqlConnection);
                        this.bd.ordenMySQL.CommandText = sql;
                        int filasIn = this.bd.EjecutarDML();
                        if (filasIn > 0)
                        {
                            filasInsertadas = filasIn;
                        }
                        else
                        {
                            filasInsertadas = 0;
                        }
                    }
                    else
                    {
                        filasInsertadas = 0;
                    }
                }
                else
                {
                    sql = @"INSERT INTO proveedores_eventos_clientes(ref_cliente, ref_evento) VALUES (" + idCli + ", " + idEvent + ")";
                    this.bd.ordenMySQL = new MySqlCommand(sql, this.bd.mySqlConnection);
                    this.bd.ordenMySQL.CommandText = sql;
                    int filasIn = this.bd.EjecutarDML();
                    if (filasIn > 0)
                    {
                        filasInsertadas = filasIn;
                    }
                    else
                    {
                        filasInsertadas = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al insertar el vinculo entre el cliente y el evento: " + ex.Message);
                filasInsertadas = 0;
                throw new Exception("Error al insertar el vinculo entre el cliente y el evento: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return filasInsertadas;
        }

        /// <summary>
        /// funcion devolver el id del evento de un proveedor y cliente en concreto
        /// </summary>
        public int ObtenerIDEvento(proveedorEntidad pe, clienteEntidad ce)
        {
            int eventoID = 0;
            MySqlDataReader lector;
            string sqlSelect;

            try
            {
                this.bd.RealizaConexion();
                //sqlSelect = @"SELECT e.id FROM eventos e, proveedores_eventos_clientes pec, clientes c , proveedores pr WHERE pec.ref_evento = e.id AND pec.ref_cliente = c.id AND pr.email = '" + pe.email + "' AND c.email = '"+ce.Email+"' GROUP BY e.id";
                sqlSelect = @"SELECT e.id FROM eventos e, proveedores_eventos_clientes pec, clientes c , proveedores pr WHERE pec.ref_evento = e.id AND pec.ref_cliente = c.id AND c.email = '"+ce.Email+"' GROUP BY e.id";
                lector = this.bd.ConsultaDML(sqlSelect);
                while (lector.Read())
                {

                    eventoID = Convert.ToInt32(lector[0]);
                    
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ha ocurrido un error al obtener el ID del evento: " + ex.Message);
                throw new Exception("Ha ocurrido un error al obtener el ID del evento: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return eventoID;
        }

        /// <summary>
        /// funcion devolver los eventos totales de un proveedor en concreto
        /// </summary>
        public List<eventoEntidad> ObtenerTotalEventos(proveedorEntidad pe)
        {
            List<eventoEntidad> leventos;
            eventoEntidad eventoEntidad;
            MySqlDataReader lector;
            string sqlSelect;

            try
            {
                leventos = new List<eventoEntidad>();
                this.bd.RealizaConexion();
                sqlSelect = @"SELECT e.id, e.nombre, e.fecha, e.activo, e.descripcion FROM eventos e, proveedores_eventos_clientes pec, proveedores pr WHERE pec.ref_proveedor = pr.id AND pec.ref_evento = e.id AND pr.email = '" + pe.email+"' GROUP BY e.id";
                lector = this.bd.ConsultaDML(sqlSelect);
                while (lector.Read())
                {
                    eventoEntidad = new eventoEntidad
                    {
                        ID = Convert.ToInt32(lector[0]),
                        nombre = lector[1].ToString(),
                        fechaInt = Convert.ToInt32(lector[2]),
                        activo = Convert.ToInt32(lector[3]),
                        descripcion = lector[4].ToString()
                    };
                    leventos.Add(eventoEntidad);
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                leventos = null;
                Debug.WriteLine("Ha ocurrido un error al obtener el total de eventos: " + ex.Message);
                throw new Exception("Ha ocurrido un error al obtener el total de eventos: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return leventos;
        }

        /// <summary>
        /// funcion devolver los eventos totales activos de un proveedor en concreto
        /// </summary>
        public List<eventoEntidad> ObtenerTotalEventosActivos(proveedorEntidad pe)
        {
            List<eventoEntidad> leventosActvos;
            eventoEntidad eventoEntidad;
            MySqlDataReader lector;
            string sqlSelect;

            try
            {
                leventosActvos = new List<eventoEntidad>();
                this.bd.RealizaConexion();
                sqlSelect = @"SELECT e.id, e.nombre, e.fecha, e.activo, e.descripcion FROM eventos e, proveedores_eventos_clientes pec, proveedores pr WHERE pec.ref_proveedor = pr.id  AND pec.ref_evento = e.id AND pr.email = '" + pe.email + "' AND e.activo = 1 GROUP BY e.id";
                lector = this.bd.ConsultaDML(sqlSelect);
                while (lector.Read())
                {
                    eventoEntidad = new eventoEntidad
                    {
                        ID = Convert.ToInt32(lector[0]),
                        nombre = lector[1].ToString(),
                        fechaInt = Convert.ToInt32(lector[2]),
                        activo = Convert.ToInt32(lector[3]),
                        descripcion = lector[4].ToString()
                    };
                    leventosActvos.Add(eventoEntidad);
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                leventosActvos = null;
                Debug.WriteLine("Ha ocurrido un error al obtener el total de eventos activos: " + ex.Message);
                throw new Exception("Ha ocurrido un error al obtener el total de eventos activos: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return leventosActvos;
        }
    }
}
