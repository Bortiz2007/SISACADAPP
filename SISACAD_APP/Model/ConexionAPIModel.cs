using Newtonsoft.Json;
using SISACAD_APP.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISACAD_APP.Model
{
    public class ConexionAPIModel
    {
        public async Task<HttpResponseMessage> ConexionAPI(string parameters)
        {

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(ServicioSession.GetKeyAPI()+parameters),
                Method = HttpMethod.Get
            };
            request.Headers.Add("Accpet", "aplication/json");

            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            return response;


        }
        public async Task<HttpResponseMessage> ConexionAPIPOST(string parameters,object x)
        {

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(ServicioSession.GetKeyAPI().Replace('?','/') + parameters),
                Method = HttpMethod.Post
            };
            request.Headers.Add("Accpet", "application/json");
            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(x);
            var contectJson = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(request.RequestUri, contectJson);
            return response;


        }
    }
}
