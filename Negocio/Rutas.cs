using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/// <summary>
/// <AUTHOR>JOSE MANUEL ESPARCIA</AUTHOR>
/// </summary>
namespace Negocio
{
    public class Rutas
    {
        /// <summary>
        /// declaracion de variables
        /// </summary>
        public static string rutaCarpetaCredentials { get; } = @"c:\weddingplanning\credentials\";
        public static string rutaCredentials { get; } = Path.Combine(rutaCarpetaCredentials, "credentials_config.txt");
    }
}
