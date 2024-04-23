using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// <author>JOSE MANUEL ESPARCIA</author> 
/// </summary>
namespace Datos
{
    public class PresupuestoDAO
    {
        /// <summary>
        /// declaracion de variables
        /// </summary>
        private BaseDeDatos bd;

        /// <summary>
        /// contructor de la clase
        /// </summary>
        public PresupuestoDAO()
        {
            this.bd = new BaseDeDatos();
        }

        /// <summary>
        /// funcion devolver los presupuestos totales de un proveedor y cliente en concreto
        /// </summary>
        public List<PresupuestoEntidad> ObtenerTotalPresupuestos(int idProveedor)
        {
            List<PresupuestoEntidad> lpresupuestos;
            PresupuestoEntidad presupuestoEntidad;
            MySqlDataReader lector;
            string sqlSelect;

            try
            {
                lpresupuestos = new List<PresupuestoEntidad>();
                this.bd.RealizaConexion();
                sqlSelect = @"SELECT p.id, p.ref_cliente, p.numero, p.importe_bruto, p.importe_iva, p.importa_neto, p.fecha, concat(c.nombre,' ',c.apellidos) as cliente, p.tipo_iva, p.aceptado FROM presupuestos p, clientes c WHERE p.ref_cliente = c.id AND p.ref_proveedor = '" + idProveedor + "' ORDER BY p.id DESC";
                lector = this.bd.ConsultaDML(sqlSelect);
                while (lector.Read())
                {
                    presupuestoEntidad = new PresupuestoEntidad
                    {
                        Id = Convert.ToInt32(lector[0]),
                        IdCliente = Convert.ToInt32(lector[1]),
                        Numero = Convert.ToInt32(lector[2]),
                        Importe_Bruto = Convert.ToDecimal(lector[3]),
                        Importe_IVA = Convert.ToDecimal(lector[4]),
                        Importe_Neto = Convert.ToDecimal(lector[5]),
                        Fecha = Convert.ToDateTime(lector[6]),
                        Cliente = lector[7].ToString(),
                        TipoIVA = Convert.ToInt32(lector[8]),
                        Aceptado = (bool)lector[9]
                    };
                    lpresupuestos.Add(presupuestoEntidad);
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                lpresupuestos = null;
                Debug.WriteLine("Ha ocurrido un error al obtener el total de presupuestos: " + ex.Message);
                throw new Exception("Ha ocurrido un error al obtener el total de presupuestos: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return lpresupuestos;
        }

        /// <summary>
        /// Inserta un presupuesto en la base de datos -> tabla presupuestos
        /// </summary>
        /// <returns></returns>
        public int InsertPresupuesto(PresupuestoEntidad pree, int idProveedor, int idCliente)
        {
            string sql;
            int filasInsertadas = 0;

            try
            {
                this.bd.RealizaConexion();
                sql = @"INSERT INTO presupuestos(numero,importe_bruto, importe_iva, importa_neto, ref_cliente, ref_proveedor, fecha, tipo_iva, aceptado) VALUES('"+pree.Numero+ "','" + pree.Importe_Bruto.ToString().Replace(',', '.') + "','" + pree.Importe_IVA.ToString().Replace(',', '.') + "','" + pree.Importe_Neto.ToString().Replace(',', '.') + "','" + idCliente + "','" + idProveedor + "','" + pree.Fecha.ToString("yyyy-MM-dd") + "','" + pree.TipoIVA + "',"+ pree.Aceptado + "); ";
                this.bd.ordenMySQL = new MySqlCommand(sql, this.bd.mySqlConnection);
                this.bd.ordenMySQL.CommandText = sql;
                filasInsertadas = this.bd.EjecutarDML();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al insertar el presupuesto: " + ex.Message);
                filasInsertadas = 0;
                throw new Exception("Error al insertar el presupuesto: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return filasInsertadas;
        }

        /// <summary>
        /// elimina un presupuesto en la base de datos -> tabla presupuestos
        /// </summary>
        /// <returns></returns>
        public int EliminaPresupuesto(int idPresupuesto)
        {
            string sql;
            int filasEliminadas = 0;

            try
            {
                this.bd.RealizaConexion();

                sql = @"DELETE FROM presupuestos WHERE id = "+ idPresupuesto;
                this.bd.ordenMySQL = new MySqlCommand(sql, this.bd.mySqlConnection);
                this.bd.ordenMySQL.CommandText = sql;
                filasEliminadas = this.bd.EjecutarDML();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al eliminar el presupuesto: " + ex.Message);
                filasEliminadas = 0;
                throw new Exception("Error al eliminar el presupuesto: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return filasEliminadas;
        }

        /// <summary>
        /// funcion para obtener el ultimo ID del presupuesto en concreto de un proveedor, servirá para luego vincular sus líneas
        /// </summary>
        /// <param name="pe"></param>
        /// <returns></returns>
        public int ObtenIDUltimoPresupuestoProveedor(int idProveedor, int idCliente)
        {
            int idPresupuesto = 0;
            MySqlDataReader lector;
            string sqlSelect;

            try
            {
                this.bd.RealizaConexion();
                sqlSelect = @"SELECT MAX(id) FROM presupuestos WHERE ref_cliente = '"+idCliente+"' AND ref_proveedor = '"+idProveedor+"'";
                lector = this.bd.ConsultaDML(sqlSelect);
                while (lector.Read())
                {
                    idPresupuesto = Convert.ToInt32(lector[0]);
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ha ocurrido un error al obtener el Identificador de presupuesto: " + ex.Message);
                throw new Exception("Ha ocurrido un error al obtener el Identificador de presupuesto: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return idPresupuesto;
        }

        /// <summary>
        /// funcion para obtener el ultimo numero del presupuesto en concreto de un proveedor, servirá para que le funcione en orden
        /// </summary>
        /// <param name="pe"></param>
        /// <returns></returns>
        public int ObtenUltimoNumeroPresupuesto(int idProv)
        {
            int numPresupuesto = 0; 
            MySqlDataReader lector;
            string sqlSelect;

            try
            {
                this.bd.RealizaConexion();
                sqlSelect = @"SELECT MAX(numero) FROM presupuestos WHERE ref_proveedor = '"+idProv+"'";
                lector = this.bd.ConsultaDML(sqlSelect);
                while (lector.Read())
                {
                    if (lector[0] is DBNull)
                    {
                        numPresupuesto = 0;
                    }
                    else
                    {
                        numPresupuesto = Convert.ToInt32(lector[0]);
                    }
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ha ocurrido un error al obtener el número de presupuesto: " + ex.Message);
                throw new Exception("Ha ocurrido un error al obtener el número de presupuesto: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return numPresupuesto;
        }

        /// <summary>
        /// funcion para actualizar el presupuesto
        /// </summary>
        /// <param name="pe"></param>
        /// <returns></returns>
        public int ActualizarPresupuesto(PresupuestoEntidad pe)
        {
            int presupuestoActualizado = 0;
            string sqlUpdate;

            try
            {
                this.bd.RealizaConexion();
                sqlUpdate = @"UPDATE presupuestos SET importe_bruto = '" + pe.Importe_Bruto.ToString().Replace(',', '.') + "', importe_iva ='" + pe.Importe_IVA.ToString().Replace(',', '.') + "', importa_neto ='" + pe.Importe_Neto.ToString().Replace(',', '.') + "', fecha = '" + pe.Fecha.ToString("yyyy-MM-dd") + "', tipo_iva ='" + pe.TipoIVA+"', aceptado = "+ pe.Aceptado+" WHERE id = " + pe.Id;
                this.bd.ordenMySQL = new MySqlCommand(sqlUpdate, this.bd.mySqlConnection);
                this.bd.ordenMySQL.CommandText = sqlUpdate;
                presupuestoActualizado = this.bd.EjecutarDML();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ha ocurrido un error al actualizar el presupuesto: " + ex.Message);
                throw new Exception("Ha ocurrido un error al actualizar el presupuesto: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return presupuestoActualizado;
        }
    }
}
