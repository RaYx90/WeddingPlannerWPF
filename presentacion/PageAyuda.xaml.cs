using Entidades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace presentacion
{
    /// <summary>
    /// Lógica de interacción para PageAyuda.xaml
    /// </summary>
    public partial class PageAyuda : Page
    {
        /// <summary>
        /// variables
        /// </summary>
        private ObservableCollection<AyudaEntidad> ocAyudas;
        private BreadCrumb bcAyuda;

        /// <summary>
        /// constructor sin argumentos
        /// </summary>
        public PageAyuda()
        {
            InitializeComponent();
            ocAyudas = new ObservableCollection<AyudaEntidad>();
            lvAyudas.ItemsSource = ocAyudas;
            GeneraAyudas();
            bcAyuda = new BreadCrumb { Nombre = "Ayuda" };
            MainWindow.CargaBread(this.bcAyuda, false);
        }

        /// <summary>
        /// Generamos el menu de ayuda
        /// </summary>
        private void GeneraAyudas()
        {
            AyudaEntidad ae_MainWindows = new AyudaEntidad()
            {
                Titulo = "1 - Página Principal",
                Cuerpo = "Esta ventana esta compuesta por 3 zonas diferenciadas, por un lado, tenemos a la izquierda un panel de opciones a elegir según lo que debamos realizar (Ver clientes, crear presupuesto), " +
                          Environment.NewLine + "este panel nos permite desplegarlo si preferimos tenerlo de esa manera visual y es autoajustable, la ventana igualmente se puede maximizar para verlo todo bien." + 
                          Environment.NewLine + "Arriba tenemos las famosas 'migas de pan' que nos permiten saber el recorrido o bien nos permiten pulsar en anteriores opciones para realizar nuevamente la opción. "+
                          Environment.NewLine + "Por otro lado, disponemos arriba a la izquierda de un ícono de 3 puntos, que nos permite pulsar en el y desplegar opciones como por ejemplo: Ver nuestro perfil "+
                          Environment.NewLine + "y modificarlo, también podremos darle a Cerrar sesión, pero nos sacará a la ventana de LOGIN para volver a introducir nuestras credenciales. Para terminar en la "+
                          Environment.NewLine + "parte central tendremos cargado el contenido de la ventana en la que pulsemos y es donde se realizan las operaciones, por defecto, en INICIO, nos saldrán tarjetas "+
                          Environment.NewLine + "con estadísticas o datos."
            };

            AyudaEntidad ae_PageClientes = new AyudaEntidad()
            {
                Titulo = "2 - Página Clientes",
                Cuerpo = "Esta ventana nos permite consultar nuestros clientes, arriba tendremos los diferentes filtros que podremos aplicar al listado. No podemos crear, editar ni eliminar clientes, ya que, " +
                          Environment.NewLine + "solo ellos mismos tienen dicha posibilidad desde la app, el proveedor solo podrá ver a aquellos clientes que le hayan asignado a su evento." +
                          Environment.NewLine + "Abajo dispondremos de 2 botones, uno de ellos, es para crear un presupuesto marcando previamente a un cliente y el otro es para eliminar los filtros de golpe."
            };

            AyudaEntidad ae_Presupuestos = new AyudaEntidad()
            {
                Titulo = "4 - Página Presupuestos",
                Cuerpo = "Esta ventana nos permite consultar nuestros presupuestos, arriba tendremos los diferentes filtros que podremos aplicar al listado, hay que tener en cuenta que al entrar la fecha que esta marcada " +
                          Environment.NewLine + "será la del día actual, por lo tanto, hay que modificarla para que aparezcan presupuestos de otros días. Adicionalmente podemos crear un nuevo presupuesto con el botón de abajo."
            };

            AyudaEntidad ae_NuevoPresupuesto = new AyudaEntidad()
            {
                Titulo = "5 - Nuevo Presupuestos",
                Cuerpo = "Esta ventana nos permite crear un nuevo presupuesto, seleccionaremos un cliente del desplegable, indicar que si se crea presupuesto desde Página Clientes, donde se marcaba un cliente, al entrar en esta " +
                          Environment.NewLine + "ventana automáticamente el cliente ya esta marcado y bloqueado sin posibilidad de seleccionar otro, por lo demás, seleccionaremos el IVA, fecha y abajo en la tabla crearemos las ." +
                          Environment.NewLine + "líneas de nuestro presupuesto, al aceptar se genera el presupuesto y ya lo tendremos disponible en el histórico."
            };

            AyudaEntidad ae_Eventos = new AyudaEntidad()
            {
                Titulo = "6 - Eventos",
                Cuerpo = "Esta ventana nos resultará útil para manejar el calendario de eventos que tenemos desde esta misma aplicación, dentro de la ventana veremos un calendario que tiene marcado por defecto todos los eventos " +
                          Environment.NewLine + "junto a la fecha actual, en la parte izquierda del calendario nos aparecerá la descripción de los eventos, donde nos indicará: Nombre del evento, fecha, si está o no activo y una " +
                          Environment.NewLine + "descripción que los novios hayan escrito al dar de alta el evento desde la app."
            };

            AyudaEntidad ae_Mensajes = new AyudaEntidad()
            {
                Titulo = "8 - Mensajes",
                Cuerpo = "Esta ventana permite tener una conversación con el cliente en forma de mensajes, el cliente desde la app nos puede pedir información, presupuesto u otros datos, y por la parte del proveedor podrá "+
                          Environment.NewLine + "escribirle mensajes directamente desde esta ventana y consultarlos a modo de histórico."
            };

            ocAyudas.Add(ae_MainWindows);
            ocAyudas.Add(ae_PageClientes);
            ocAyudas.Add(ae_Presupuestos);
            ocAyudas.Add(ae_NuevoPresupuesto);
            ocAyudas.Add(ae_Eventos);
            ocAyudas.Add(ae_Mensajes);
        }
    }
}
