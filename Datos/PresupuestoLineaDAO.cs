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

    public class PresupuestoLineaDAO
    {
        /// <summary>
        /// declaracion de variables
        /// </summary>
        private BaseDeDatos bd;

        /// <summary>
        /// contructor de la clase
        /// </summary>
        public PresupuestoLineaDAO()
        {
            this.bd = new BaseDeDatos();
        }

        /// <summary>
        /// Elimina las lineas de un presupuesto
        /// </summary>
        /// <returns></returns>
        public int EliminaLineasPresupuesto(int idPresupuesto)
        {
            string sql;
            int filasEliminadas = 0;

            try
            {
                this.bd.RealizaConexion();
                sql = @"DELETE FROM presupuestos_lineas WHERE ref_presupuesto = " + idPresupuesto;
                this.bd.ordenMySQL = new MySqlCommand(sql, this.bd.mySqlConnection);
                this.bd.ordenMySQL.CommandText = sql;
                filasEliminadas += this.bd.EjecutarDML();
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al eliminar las líneas del presupuesto: " + ex.Message);
                filasEliminadas = 0;
                throw new Exception("Error al eliminar las líneas del presupuesto: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return filasEliminadas;
        }

        /// <summary>
        /// Inserta lsa lineas de un presupuesto
        /// </summary>
        /// <returns></returns>
        public int InsertLineasPresupuesto(List<PresupuestoLineaEntidad> lple, int idPresupuesto)
        {
            string sql;
            int filasInsertadas = 0;

            try
            {
                this.bd.RealizaConexion();

                foreach (var linea in lple)
                {
                    sql = @"INSERT INTO presupuestos_lineas(ref_presupuesto, descripcion, cantidad, importe_bruto, importe_iva, importe_neto) VALUES("+ idPresupuesto + ", '" + linea.Descripcion + "', " + linea.Cantidad.ToString().Replace(',', '.') + ", " + linea.ImporteBruto.ToString().Replace(',', '.') + ", " + linea.ImporteIVA.ToString().Replace(',', '.') + ", " + linea.ImporteNeto.ToString().Replace(',', '.') + ")";

                    this.bd.ordenMySQL = new MySqlCommand(sql, this.bd.mySqlConnection);
                    this.bd.ordenMySQL.CommandText = sql;
                    filasInsertadas += this.bd.EjecutarDML();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al insertar las líneas del presupuesto: " + ex.Message);
                filasInsertadas = 0;
                throw new Exception("Error al insertar las líneas del presupuesto: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return filasInsertadas;
        }

        /// <summary>
        /// funcion devolver las lineas de un presupuesto
        /// </summary>
        public List<PresupuestoLineaEntidad> ObtenerLineasPresupuesto(int idPresupuesto)
        {
            List<PresupuestoLineaEntidad> lLineasPresupuestos;
            PresupuestoLineaEntidad presupuestoLineaEntidad;
            MySqlDataReader lector;
            string sqlSelect;

            try
            {
                lLineasPresupuestos = new List<PresupuestoLineaEntidad>();
                this.bd.RealizaConexion();
                sqlSelect = @"SELECT descripcion, cantidad, importe_bruto, importe_iva, importe_neto FROM presupuestos_lineas WHERE ref_presupuesto = " + idPresupuesto;
                lector = this.bd.ConsultaDML(sqlSelect);
                while (lector.Read())
                {
                    presupuestoLineaEntidad = new PresupuestoLineaEntidad
                    {
                        Descripcion = lector[0].ToString(),
                        Cantidad = Convert.ToDecimal(lector[1]),
                        ImporteBruto = Convert.ToDecimal(lector[2]),
                        ImporteIVA = Convert.ToDecimal(lector[3]),
                        ImporteNeto = Convert.ToDecimal(lector[4])
                    };
                    lLineasPresupuestos.Add(presupuestoLineaEntidad);
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                presupuestoLineaEntidad = null;
                Debug.WriteLine("Ha ocurrido un error al obtener el total de lineas de un presupuesto: " + ex.Message);
                throw new Exception("Ha ocurrido un error al obtener el total de lineas de un presupuesto: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return lLineasPresupuestos;
        }
    }
}
