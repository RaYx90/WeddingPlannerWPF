using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// JOSE MANUEL ESPARCIA CAÑIZARES
/// </summary>
namespace Entidades
{
    public class eventoEntidad
    {
        /// <summary>
        /// Variables con sus metodos.
        /// </summary>
        public int ID { get; set; }
        public string nombre { get; set; }
        public int fechaInt { get; set; }
        public int activo { get; set; }
        public string descripcion { get; set; }

        /// <summary>
        /// devolvemos la fecha formateada
        /// </summary>
        public string FechaFormateada
        {
            get
            {
                if (fechaInt != -1)
                {
                    return UnixTimestampToDateTime(this.fechaInt).ToShortDateString() + " " + UnixTimestampToDateTime(this.fechaInt).ToShortTimeString();
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// transformamos la fecha a un formato legible
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime UnixTimestampToDateTime(double unixTime)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            long unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
            return new DateTime(unixStart.Ticks + unixTimeStampInTicks, System.DateTimeKind.Utc);
        }
    }
}
