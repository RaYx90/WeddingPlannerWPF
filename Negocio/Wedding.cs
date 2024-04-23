using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// JOSE MANUEL ESPARCIA CAÑIZARES
/// </summary>
namespace Negocio
{
    public class Wedding
    {
        /// <summary>
        /// Variables
        /// </summary>
        private proveedorDAO proveedorDAO;
        private eventoDAO eventoDAO;
        private clienteDAO clienteDAO;
        private servicioDAO servicioDAO;
        private PresupuestoDAO presupuestoDAO;
        private PresupuestoLineaDAO presupuestoLineaDAO;

        /// <summary>
        /// contructor sin argumentos
        /// </summary>
        public Wedding()
        {
            this.proveedorDAO = new proveedorDAO();
            this.eventoDAO = new eventoDAO();
            this.clienteDAO = new clienteDAO();
            this.servicioDAO = new servicioDAO();
            this.presupuestoDAO = new PresupuestoDAO();
            this.presupuestoLineaDAO = new PresupuestoLineaDAO();
        }

        /// <summary>
        /// devuelve un objeto proveedorEntidad
        /// </summary>
        /// <returns></returns>
        public proveedorEntidad CompruebaLogin(string userLogin, string passLogin)
        {
            return this.proveedorDAO.CompruebaProveedorLogin(userLogin, passLogin);
        }

        /// <summary>
        /// devuelve un int con el contador de proveedores
        /// </summary>
        /// <returns></returns>
        public int ContadorProveedores(proveedorEntidad pe)
        {
            return this.proveedorDAO.CompruebaExistenciaProveedor(pe);
        }

        /// <summary>
        /// Funcion para actualizar un proveedor
        /// </summary>
        /// <returns></returns>
        public int ActualizarPasswordProveedor(string password,string email, string cif)
        {
            int passwordActualizado = this.proveedorDAO.ModificaPasswordProveedor(password, email, cif);

            if (passwordActualizado > 0)
            {
                return passwordActualizado;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Funcion para insertar un proveedor
        /// </summary>
        /// <returns></returns>
        public int InsertaProveedor(proveedorEntidad pe)
        {
            int proveedorInsertado = this.proveedorDAO.InsertProveedor(pe);

            if (proveedorInsertado > 0)
            {
                return proveedorInsertado;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Funcion para insertar un cliente
        /// </summary>
        /// <returns></returns>
        public int InsertaCliente(clienteEntidad ce, int fechaNacTimestamp)
        {
            int clienteInsertado = this.clienteDAO.InsertCliente(ce, fechaNacTimestamp);

            if (clienteInsertado > 0)
            {
                return clienteInsertado;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Funcion para insertar un evento
        /// </summary>
        /// <returns></returns>
        public int InsertEvento(eventoEntidad ee)
        {
            int eventoInsertado = this.eventoDAO.InsertEvento(ee);

            if (eventoInsertado > 0)
            {
                return eventoInsertado;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Se inserta en la tabla con la relación entre las 3
        /// </summary>
        /// <param name="idPro"></param>
        /// <param name="idCli"></param>
        /// <param name="idEve"></param>
        /// <returns></returns>
        public int InsertProveedoresEventoCliente(int idPro, int idCli, int idEve, bool insertaProvee)
        {
            int eventoCliProvInsertado = this.eventoDAO.InsertProveedoresEventoCliente(idPro, idCli, idEve, insertaProvee);

            if (eventoCliProvInsertado > 0)
            {
                return eventoCliProvInsertado;
            }
            else
            {
                return 0;
            }
        }
        

        /// <summary>
        /// funcion para devolver un listado de eventosEntidad
        /// </summary>
        /// <param name="pe"></param>
        /// <returns></returns>
        public List<eventoEntidad> ObtenerEventos(proveedorEntidad pe)
        {
            return eventoDAO.ObtenerTotalEventos(pe);
        }

        /// <summary>
        /// funcion para devolver un listado de eventosEntidad que esten activos
        /// </summary>
        /// <param name="pe"></param>
        /// <returns></returns>
        public List<eventoEntidad> ObtenerEventosActivos(proveedorEntidad pe)
        {
            return eventoDAO.ObtenerTotalEventosActivos(pe);
        }

        /// <summary>
        /// Funcion para devolver el listado de clientes de un proveedor
        /// </summary>
        /// <param name="pe"></param>
        /// <returns></returns>
        public List<clienteEntidad> ObtenerClientes(proveedorEntidad pe)
        {
            return clienteDAO.ObtenerTotalClientes(pe);
        }

        public List<PresupuestoEntidad> ObtenerPresupuestos(int idProveedor)
        {
            return presupuestoDAO.ObtenerTotalPresupuestos(idProveedor);
        }

        /// <summary>
        /// Funcion para devolver el listado de servicios de un proveedor
        /// </summary>
        /// <param name="pe"></param>
        /// <returns></returns>
        public List<servicioEntidad> ObtenerServiciosProveedor(proveedorEntidad pe)
        {
            return servicioDAO.ObtenerTotalServiciosProveedor(pe);
        }

        /// <summary>
        /// Funcion para devolver el listado de servicios
        /// </summary>
        /// <param name="pe"></param>
        /// <returns></returns>
        public List<servicioEntidad> ObtenerTotalServicios()
        {
            return servicioDAO.ObtenerTotalServicios();
        }

        /// <summary>
        /// funcion para obtener el id del proveedor logueado
        /// </summary>
        /// <param name="pe"></param>
        /// <returns></returns>
        public int ObtenerIDProveedorLogin(proveedorEntidad pe)
        {
            return this.proveedorDAO.ObtenIDProveedor(pe);
        }

        /// <summary>
        /// Funcion para insertar un presupuesto
        /// </summary>
        /// <returns></returns>
        public int InsertaPresupuesto(PresupuestoEntidad pe, int prov, int cli)
        {
            int presupuestoInsertado = this.presupuestoDAO.InsertPresupuesto(pe, prov, cli);

            if (presupuestoInsertado > 0)
            {
                return presupuestoInsertado;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// funcion para obtener el ultimo numero del presupuesto por proveedor
        /// </summary>
        /// <param name="pe"></param>
        /// <returns></returns>
        public int ObtenerNumeroPresupuesto(int idProv)
        {
            return this.presupuestoDAO.ObtenUltimoNumeroPresupuesto(idProv);
        }

        /// <summary>
        /// funcion para obtener el id del presupuesto 
        /// </summary>
        /// <param name="pe"></param>
        /// <returns></returns>
        public int ObtenerIDPresupuesto(int prov, int cli)
        {
            return this.presupuestoDAO.ObtenIDUltimoPresupuestoProveedor(prov, cli);
        }

        /// <summary>
        /// funcion para obtener el id del cliente 
        /// </summary>
        /// <param name="pe"></param>
        /// <returns></returns>
        public int ObtenerIDCliente(clienteEntidad ce)
        {
            return this.clienteDAO.ObtenIDCliente(ce);
        }

        /// <summary>
        /// funcion para obtener el id del Evento 
        /// </summary>
        /// <param name="pe"></param>
        /// <returns></returns>
        public int ObtenerIDEvento(proveedorEntidad pe, clienteEntidad ce)
        {
            return this.eventoDAO.ObtenerIDEvento(pe, ce);
        }

        /// <summary>
        /// Funcion para insertar las líneas de un presupuesto
        /// </summary>
        /// <returns></returns>
        public int InsertaLineasPresupuesto(List<PresupuestoLineaEntidad> listPresupuestoLineas, int idPresupuesto)
        {
            int lineasPresupuestoInsertado = this.presupuestoLineaDAO.InsertLineasPresupuesto(listPresupuestoLineas, idPresupuesto);

            if (lineasPresupuestoInsertado > 0)
            {
                return lineasPresupuestoInsertado;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Funcion para eliminar un presupuesto
        /// </summary>
        /// <param name="idPresupuesto"></param>
        /// <returns></returns>
        public int EliminaPresupuesto(int idPresupuesto)
        {
            int presupuestoEliminado = this.presupuestoDAO.EliminaPresupuesto(idPresupuesto);

            if(presupuestoEliminado > 0)
            {
                return presupuestoEliminado;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// funcion para actualizar datos del proveedor
        /// </summary>
        /// <param name="pe"></param>
        /// <returns></returns>
        public int ActualizaDatosProveedor(proveedorEntidad pe, List<servicioEntidad> lse)
        {
            int proveedorModificado = this.proveedorDAO.UpdateProveedor(pe, lse);

            if (proveedorModificado > 0)
            {
                return proveedorModificado;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// obtiene todas las lineas del presupuesto por su id
        /// </summary>
        /// <param name="idPresupuesto"></param>
        /// <returns></returns>
        public List<PresupuestoLineaEntidad> ObtenerLineasPresupuesto(int idPresupuesto)
        {
            return presupuestoLineaDAO.ObtenerLineasPresupuesto(idPresupuesto);
        }

        /// <summary>
        /// Funcion para eliminar las líneas de un presupuesto
        /// </summary>
        /// <returns></returns>
        public int EliminaLineasPresupuesto(int idPresupuesto)
        {
            int lineasPresupuestoEliminadas = this.presupuestoLineaDAO.EliminaLineasPresupuesto(idPresupuesto);

            if (lineasPresupuestoEliminadas > 0)
            {
                return lineasPresupuestoEliminadas;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// devuelve un objeto clienteEntidad
        /// </summary>
        /// <returns></returns>
        public clienteEntidad ObtenerClientePorID(int idCliente)
        {
            return this.clienteDAO.ObtenDatosClientePorID(idCliente);
        }

        /// <summary>
        /// Funcion para actualizar el presupuesto
        /// </summary>
        /// <returns></returns>
        public int ActualizarPresupuesto(PresupuestoEntidad pe)
        {
            int PresupuestoAct = this.presupuestoDAO.ActualizarPresupuesto(pe);

            if (PresupuestoAct > 0)
            {
                return PresupuestoAct;
            }
            else
            {
                return 0;
            }
        }
    }
}
