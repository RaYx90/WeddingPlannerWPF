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
    public class clienteEntidad
    {
        /// <summary>
        /// Variables con sus metodos.
        /// </summary>
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        public string Poblacion { get; set; }
        public string Provincia { get; set; }
        public string CP { get; set; }
        public string Telefono { get; set; }
        public string Movil { get; set; }
        public string Email { get; set; }
        public int Edad { get; set; }
        public string DNI { get; set; }
        public string Evento { get; set; }

        //Sobreescribimos el método ToString para devolver un string personalizado
        public override string ToString()
        {
            return this.Nombre + " " + this.Apellidos;
        }
    }
}
