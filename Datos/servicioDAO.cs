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
    public class servicioDAO
    {
        /// <summary>
        /// declaracion de variables
        /// </summary>
        private BaseDeDatos bd;

        /// <summary>
        /// contructor de la clase
        /// </summary>
        public servicioDAO()
        {
            this.bd = new BaseDeDatos();
        }

        /// <summary>
        /// funcion para devolver los servicios totales de un proveedor en concreto
        /// </summary>
        public List<servicioEntidad> ObtenerTotalServiciosProveedor(proveedorEntidad pe)
        {
            List<servicioEntidad> lservicios;
            servicioEntidad servicioEntidad;
            MySqlDataReader lector;
            string sqlSelect;

            try
            {
                lservicios = new List<servicioEntidad>();
                this.bd.RealizaConexion();
                sqlSelect = @"SELECT s.nombre FROM servicios s, proveedores_servicios ps, proveedores pr WHERE s.id = ps.ref_servicio AND ps.ref_proveedor = pr.id AND pr.email = '" + pe.email + "'";
                lector = this.bd.ConsultaDML(sqlSelect);
                while (lector.Read())
                {
                    servicioEntidad = new servicioEntidad
                    {
                        Nombre = lector[0].ToString()
                    };
                    lservicios.Add(servicioEntidad);
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                lservicios = null;
                Debug.WriteLine("Ha ocurrido un error al obtener el total de servicios por proveedor: " + ex.Message);
                throw new Exception("Ha ocurrido un error al obtener el total de servicios por proveedor: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return lservicios;
        }

        /// <summary>
        /// funcion para devolver todos los servicios
        /// </summary>
        public List<servicioEntidad> ObtenerTotalServicios()
        {
            List<servicioEntidad> lservicios;
            servicioEntidad servicioEntidad;
            MySqlDataReader lector;
            string sqlSelect;

            try
            {
                lservicios = new List<servicioEntidad>();
                this.bd.RealizaConexion();
                sqlSelect = @"SELECT nombre FROM servicios";
                lector = this.bd.ConsultaDML(sqlSelect);
                while (lector.Read())
                {
                    servicioEntidad = new servicioEntidad
                    {
                        Nombre = lector[0].ToString()
                    };
                    lservicios.Add(servicioEntidad);
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                lservicios = null;
                Debug.WriteLine("Ha ocurrido un error al obtener el total de servicios: " + ex.Message);
                throw new Exception("Ha ocurrido un error al obtener el total de servicios: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return lservicios;
        }

        /// <summary>
        /// funcion para devolver los IDS de los servicios
        /// </summary>
        public List<int> ObtenerIDServicios(List<servicioEntidad> lse)
        {
            List<int> idsServicios;
            MySqlDataReader lector;
            string sqlSelect;

            try
            {
                idsServicios = new List<int>();
                sqlSelect = @"SELECT id FROM servicios WHERE nombre in (";
                int contador = 1;

                foreach (var servicio in lse)
                {
                    if(contador == lse.Count)
                    {
                        sqlSelect += "'"+ servicio.Nombre +"')";
                    }
                    else
                    {
                        sqlSelect += "'" + servicio.Nombre + "',";
                    }
                    contador++;
                }

                this.bd.RealizaConexion();
                lector = this.bd.ConsultaDML(sqlSelect);
                while (lector.Read())
                {
                    idsServicios.Add(Convert.ToInt32(lector[0]));
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                idsServicios = null;
                Debug.WriteLine("Ha ocurrido un error al obtener los ids de servicios: " + ex.Message);
                throw new Exception("Ha ocurrido un error al obtener los ids de servicios: " + ex.Message);
            }
            finally
            {
                this.bd.CerrarConexion();
            }
            return idsServicios;
        }
    }
}
