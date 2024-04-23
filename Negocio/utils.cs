using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

/// <summary>
/// JOSE MANUEL ESPARCIA CAÑIZARES
/// </summary>
namespace Negocio
{
    public class utils
    {
        /// <summary>
        /// Funcion para transformar la fecha a datetime
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime UnixTimestampToDateTime(double unixTime)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            long unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
            return new DateTime(unixStart.Ticks + unixTimeStampInTicks, System.DateTimeKind.Utc);
        }

        /// <summary>
        /// funcion para transformar la fecha a timestamp
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ConvertToTimestamp(DateTime value)
        {
            long epoch = (value.Ticks - 621355968000000000) / 10000000;
            return epoch;
        }

        /// <summary>
        /// funcion para mostrar los mensajes personalizados
        /// </summary>
        /// <param name="cuerpo"></param>
        public static void MuestraMensajePersonalizado(string cuerpo, string cabecera)
        {
            MessageBox.Show(cuerpo, cabecera);
        }

        /// <summary>
        /// funcion para mostrar los mensajes de ayuda
        /// </summary>
        /// <param name="cuerpo"></param>
        public static void MuestraMensajeInformacion(string cuerpo)
        {
            MessageBox.Show(cuerpo, "Mensaje de información");
        }

        /// <summary>
        /// funcion para mostrar los mensajes de error (exception y demás)
        /// </summary>
        /// <param name="cuerpo"></param>
        public static void MuestraMensajeError(string cuerpo)
        {
            MessageBox.Show(cuerpo, "Mensaje de error");
        }

        /// <summary>
        /// Encriptamos el pass del usuario en MD5
        /// </summary>
        /// <param name="pass">password sin encriptar</param>
        /// <returns></returns>
        public static string MD5Hash(string pass)
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

        /// <summary>
        /// Funcion para comprobar la seguridad de la contraseña
        /// </summary>
        /// <returns></returns>
        public static bool ValidaPassword(string pass)
        {
            var expresion = new Regex(@"^(?=\S*?[A-Z])(?=\S*?[a-z])(?=\S*?[0-9])\S+$");
            if (expresion.Match(pass).Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Funcion para validar la cadena del email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool ValidaEmail(string email)
        {
            var expresion = new Regex(@"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");

            if (expresion.IsMatch(email))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Funcion para devolver las ciudades de España
        /// </summary>
        /// <returns></returns>
        public static List<string> ListadoCiudades()
        {
            string[] ciudades = {"A Coruña", "Alava", "Albacete", "Alicante", "Almeria", "Asturias", "Avila", "Badajoz", "Baleares", "Barcelona", "Burgos", "Caceres", "Cadiz", "Cantabria", "Castellon", "Ceuta",
                                 "Ciudad Real", "Cordoba", "Cuenca", "Girona", "Granada", "Guadalajara", "Guipuzcoa", "Huelva", "Huesca", "Jaen", " La Rioja", "Las Palmas", "Leon", "Lleida", "Lugo", "Madrid", "Malaga",
                                 "Melilla", "Murcia", "Navarra", "Ourense", "Palencia", "Pontevedra", "Salamanca", "Segovia", "Sevilla", "Soria", "Tarragona", "Tenerife", "Teruel", "Toledo", "Valencia", "Valladolid",
                                 "Vizcaya", "Zamora", "Zaragoza"};
            List<string> lCiudades = new List<string>(ciudades);
            return lCiudades;
        }
    }
}
