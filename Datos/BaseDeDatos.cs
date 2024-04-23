using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

/// <summary>
/// JOSE MANUEL ESPARCIA CAÑIZARES
/// </summary>
namespace Datos
{
    public class BaseDeDatos
    {
        /// <summary>
        /// Definición de las variables de la clase basededatos
        /// </summary>
        public MySqlConnection mySqlConnection { get; set; }
        public MySqlDataAdapter mySqlDataAdapter { get; set; }
        public MySqlCommand ordenMySQL { get; set; }

        /// <summary>
        /// contructor de la clase
        /// </summary>
        public BaseDeDatos()
        {
            this.mySqlConnection = new MySqlConnection();
            this.mySqlConnection.ConnectionString = string.Format("server={0};port={1};user id={2}; password={3}; database={4};", "remotemysql.com", "3306", "y6CQ6X1U7Z", "u7oUzvMGHu", "y6CQ6X1U7Z");
        }

        /// <summary>
        /// Funcion para dejar en escucha la conexion a la bd
        /// </summary>
        public void RealizaConexion()
        {
            this.mySqlConnection.Open();
        }

        /// <summary>
        /// Funcion para ejecutar una select
        /// </summary>
        /// <param name="consultaSQL"></param>
        /// <returns></returns>
        public MySqlDataReader ConsultaDML(string consultaSQL)
        {
            this.ordenMySQL = new MySqlCommand(consultaSQL, mySqlConnection);
            MySqlDataReader mySqlDataReader = ordenMySQL.ExecuteReader();
            return mySqlDataReader;
        }

        /// <summary>
        /// Funcion para ejecutar DDL: Insert, Update y Delete.
        /// </summary>
        /// <returns></returns>
        public int EjecutarDML()
        {
            int filasAfectadas = 0;
            filasAfectadas = this.ordenMySQL.ExecuteNonQuery();
            this.ordenMySQL.Dispose();
            return filasAfectadas;
        }

        /// <summary>
        /// Funcion para cerrar la conexion a la base de datos
        /// </summary>
        public void CerrarConexion()
        {
            this.mySqlConnection.Close();
        }
    }
}
