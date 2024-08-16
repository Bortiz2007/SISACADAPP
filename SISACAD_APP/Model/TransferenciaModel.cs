using Controls.UserDialogs.Maui;
using Newtonsoft.Json;
using SISACAD_APP.Entidades;
using SISACAD_APP.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SISACAD_APP.Model
{
    public class TransferenciaModel
    {

        public List<List<PagosDetalleE>> resultado;
        public async Task<List<List<PagosDetalleE>>> ObtenerProductos(string cedula, string consulta,string carrera)
        {
            // string url = "https://api.itsqmet.edu.ec/Api/SISACAD?strCedula=" + cedula + "&consulta=" + consulta;
           
            string busqueda = "strCedula="+ cedula + "&consulta=" + consulta + "&carrera="+ carrera;
            ConexionAPIModel conexion = new ConexionAPIModel();
            HttpResponseMessage response = await conexion.ConexionAPI(busqueda);
             
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                resultado = JsonConvert.DeserializeObject<List<List<PagosDetalleE>>>(content);

            }
            return resultado;

        }
        public async Task<bool> ExistenPagosPendientes(string cedula)
        {
            bool res = false;
            string busqueda ="id_estudiante=" + cedula;
            ConexionAPIModel conexion = new ConexionAPIModel();
            HttpResponseMessage response = await conexion.ConexionAPI(busqueda);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                res = Convert.ToBoolean(Convert.ToInt32(JsonConvert.DeserializeObject(content)));

            }
            return res;
        }
        public async Task IngresaDeposito(DatosDepositosE datosDepositos)
        {
            string  envio= "postQr";
            ConexionAPIModel conexion = new ConexionAPIModel();
            HttpResponseMessage response = await conexion.ConexionAPIPOST(envio, datosDepositos);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                string respuesta = JsonConvert.DeserializeObject<String>(content);
                if (respuesta == "EXITO")
                {
                   
                    UserDialogs.Instance.Alert("Registro con Exito", "Informativo", "Aceptar", "@drawable/info.png");
                    if(ServicioSession.GetIdRol() == "4" || ServicioSession.GetIdRol() == "3")
                    {
                        Application.Current.MainPage = new AppShell();
                        await Shell.Current.GoToAsync("listadoDepositos");
                    }
                    else
                    {
                        Application.Current.MainPage = new AppShell();
                    }
                    
                }
                else if (respuesta == "ERROR1")
                {
                   
                    UserDialogs.Instance.Alert("Ya existe un registro con el número de Documento " +datosDepositos.StrDocumento + " por favor revíse los datos enviados", "Informativo", "Aceptar", "@drawable/info.png");

                }
                else
                {
                   UserDialogs.Instance.Alert("Por favor, contacte a soporte técnico al +593 98 464 1470.", "Error", "Aceptar", "@drawable/error_login.png");

                }
            }
        }


    }
}
