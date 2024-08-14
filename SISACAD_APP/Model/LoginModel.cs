using Newtonsoft.Json;
using SISACAD_APP.Servicios;
using SISACAD_APP.ViewModel1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SISACAD_APP.Model
{
    public class LoginModel
    {


        public async Task<bool> ValidaCredenciales(string cod_usu, string clave)
        {

            bool respuesta = false;
            
            string url = ServicioSession.GetKeyAPI()+"cod_usu=" + cod_usu + "&pass=" + clave;
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get
            };
            request.Headers.Add("Accpet", "aplication/json");

            var client = new HttpClient();


            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<Entidades.UsuariosE>>(content);


                if (resultado.Count != 0 && ((resultado[0].Id_rol == 3) || (resultado[0].Id_rol == 4) || (resultado[0].Id_rol == 1) || (resultado[0].Id_rol == 14)))
                {

                    ServicioSession.GuardarDatos(resultado[0].Identificacion, resultado[0].Id_rol.ToString(), resultado[0].Cambio_clave);

                    respuesta = true;

                }
                
            }

            return respuesta;
        }





    }


}
