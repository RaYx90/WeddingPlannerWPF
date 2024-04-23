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
    public class PresupuestoLineaEntidad
    {
        /// <summary>
        /// declaración de variables
        /// </summary>
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public decimal ImporteBruto { get; set; }
        public decimal ImporteIVA { get; set; }
        public decimal ImporteNeto { get; set; }


        public string CantidadString
        {
            set
            {
                Cantidad = Convert.ToDecimal(value);
            }
            get
            {
                return this.Cantidad.ToString();
            }
        }

        public string ImporteBrutoEuros
        {
            set
            {
                ImporteBruto = Convert.ToDecimal(value);
            }
            get
            {
                return this.ImporteBruto + "€";
            }
        }

        public string ImporteIVAEuros
        {
            set
            {
                ImporteBruto = Convert.ToDecimal(value);
            }
            get
            {
                return ImporteIVA + "€";
            }
        }

        public string ImporteNetoEuros
        {
            set
            {
                ImporteBruto = Convert.ToDecimal(value);
            }
            get
            {
                return ImporteNeto + "€";
            }
        }


        public PresupuestoLineaEntidad()
        {}
    }
}
