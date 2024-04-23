using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

/// <summary>
/// <author>JOSE MANUEL ESPARCIA CAÑIZARES</author>
/// </summary>
namespace Entidades
{
    public class MensajeEntidad
    {
        /// <summary>
        /// variables
        /// </summary>
        public DateTime FechaCompleta { get; set; }
        public string Descripcion { get; set; }
        public bool Propietario { get; set; }

        public string NombreCliente { get; set; }

        public Thickness Posicion
        {
            get
            {
                if (Propietario)
                {
                    return new Thickness(200,0,0,0);
                }
                else
                {
                    return new Thickness(0, 0, 200, 0);
                }
            }
        }

        public string Color
        {
            get
            {
                if (Propietario)
                {
                    return "LightBlue";
                }
                else
                {
                    return "#74ff94";
                }
            }
        }

        /// <summary>
        /// Devolvemos la fecha formateada
        /// </summary>
        public string FechaFormateada
        {
            get
            {
                return FechaCompleta.ToString("dd/MM/yyyy HH:mm");
            }
        }
    }
}
