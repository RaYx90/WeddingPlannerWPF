using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// <author>JOSE MANUEL ESPARCIA</author>
/// </summary>
namespace Entidades
{
    public class PresupuestoEntidad
    {
        /// <summary>
        /// variables con sus metodos
        /// </summary>
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int Numero { get; set; }
        public decimal Importe_Bruto { get; set; }
        public decimal Importe_IVA { get; set; }
        public decimal Importe_Neto { get; set; }
        public int TipoIVA { get; set; }
        public string Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public bool Aceptado { get; set; }
        private int total;
         

        /// <summary>
        /// devolvemos la fecha en string con el formato que queramos
        /// </summary>
        public string FechaFormateada
        {
            get
            {
                return this.Fecha.ToShortDateString();
            }
        }

        public string ImporteBrutoEuros
        {
            get
            {
                return this.Importe_Bruto+" €";
            }
        }

        public string ImporteIVAEuros
        {
            get
            {
                return this.Importe_IVA + " €";
            }
        }

        public string ImporteNetoEuros
        {
            get
            {
                return this.Importe_Neto + " €";
            }
        }

        public string AceptadoString
        {
            get
            {
                if(Aceptado)
                {
                    return "Aceptado";
                }
                else
                {
                    return "No aceptado";
                }
            }
        }

        public int Total
        {
            get
            {
                return this.total;
            }
            set
            {
                this.total = value;
            }
        }
    }
}
