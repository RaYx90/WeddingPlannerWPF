using Entidades;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// <Author>JOSE MANUEL ESPARCIA CAÑIZARES</Author>
/// </summary>
namespace Negocio
{
    public class Api
    {
        /// <summary>
        /// Declaracion de variables
        /// </summary>
        private static Wedding wedding = new Wedding();

        /// <summary>
        /// Funcion para obtener el token del proveedor que lo solicita
        /// </summary>
        /// <param name="proveedor"></param>
        /// <returns></returns>
        public static string ObtenerToken(proveedorEntidad proveedor)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://tfg-weddingplanning-node.herokuapp.com/api/v1/providers/login");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"email\":\"" + proveedor.email + "\"," +
                                  "\"password\":\"" + proveedor.password + "\"}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string result;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                JObject jsonPeticionMensajes = JObject.Parse(result);
                string token = jsonPeticionMensajes["token"].ToString();

                return token;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// funcion que devuelve los mensajes de un evento en concreto
        /// </summary>
        /// <param name="evento"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static ObservableCollection<MensajeEntidad> ObtenMensajesEvento(eventoEntidad evento, string token)
        {
            try
            {
                ObservableCollection<MensajeEntidad> lmensajes = new ObservableCollection<MensajeEntidad>();
                HttpWebRequest httpWebRequestMensajes = (HttpWebRequest)WebRequest.Create("https://tfg-weddingplanning-node.herokuapp.com/api/v1/messages/" + evento.ID);
                httpWebRequestMensajes.ContentType = "application/json";
                httpWebRequestMensajes.Method = "GET";
                httpWebRequestMensajes.PreAuthenticate = true;
                httpWebRequestMensajes.Headers.Add("Authorization", "Bearer " + token);

                var httpRespuestaMensajes = (HttpWebResponse)httpWebRequestMensajes.GetResponse();
                string resultMensajes;
                using (var streamReader = new StreamReader(httpRespuestaMensajes.GetResponseStream()))
                {
                    resultMensajes = streamReader.ReadToEnd();
                }
                JObject jsonMensajes = JObject.Parse(resultMensajes);

                var mensajes = jsonMensajes["messages"].ToArray();

                foreach (var mensaje in mensajes)
                {
                    if (mensaje["propietario"] != null)
                    {
                        if ((bool)mensaje["propietario"])
                        {
                            lmensajes.Add(new MensajeEntidad
                            {
                                Descripcion = (string)mensaje["mensaje"],
                                FechaCompleta = utils.UnixTimestampToDateTime((double)mensaje["fecha"]),
                                Propietario = (bool)mensaje["propietario"],
                            });
                        }
                        else
                        {
                            lmensajes.Add(new MensajeEntidad
                            {
                                Descripcion = (string)mensaje["mensaje"],
                                FechaCompleta = utils.UnixTimestampToDateTime((double)mensaje["fecha"]),
                                Propietario = (bool)mensaje["propietario"],
                                NombreCliente = (string)mensaje["cliente"]["nombre"] + " " + (string)mensaje["cliente"]["apellidos"]
                            });
                        }
                    }
                }
                return lmensajes;
            }
            catch(Exception ex)
            {
                throw ex; 
            }
        }

        /// <summary>
        /// Funcion para enviar un mensaje
        /// </summary>
        /// <param name="pe"></param>
        /// <param name="ee"></param>
        /// <param name="token"></param>
        public static bool EnviaMensaje(proveedorEntidad pe, int idEvento, string tokenProv, string mensaje)
        {
            bool resultado;

            try
            {
                int idProveedor = wedding.ObtenerIDProveedorLogin(pe);
                var httpWebRequestSendMessage = (HttpWebRequest)WebRequest.Create("https://tfg-weddingplanning-node.herokuapp.com/api/v1/messages");
                httpWebRequestSendMessage.ContentType = "application/json";
                httpWebRequestSendMessage.Method = "POST";
                httpWebRequestSendMessage.PreAuthenticate = true;
                httpWebRequestSendMessage.Headers.Add("Authorization", "Bearer " + tokenProv);


                using (var streamWriter = new StreamWriter(httpWebRequestSendMessage.GetRequestStream()))
                {
                    string json = "{" +
                                    "\"evento\": {" +
                                        "\"id\":\"" + idEvento + "\""+
                                    "}," +
                                    "\"proveedor\": {" +
                                        "\"id\":\"" + idProveedor + "\"" +
                                    "}," +
                                    "\"mensaje\":\"" + mensaje + "\"" +
                                  "}";

                    json = json.Replace(Environment.NewLine, " ");

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequestSendMessage.GetResponse();
                string result;

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                JObject jsonPeticionMensajes = JObject.Parse(result);
                resultado = (bool)jsonPeticionMensajes["ok"];

                return resultado;
            }
            catch
            {
                resultado = false;
                return resultado;
            }
        }
    }
}
