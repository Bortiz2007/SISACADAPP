using Newtonsoft.Json;
using SISACAD_APP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SISACAD_APP.Servicios
{
    public class PeriodoServicio
    {
        public List<EstudiantesModel> estudiantes;

        public async Task<List<EstudiantesModel>> GetPeriodos(string identificacion)
        {
            string url = "https://api.itsqmet.edu.ec/Api/SISACAD?identificacion=" + identificacion;
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
                estudiantes = JsonConvert.DeserializeObject<List<EstudiantesModel>>(content);


            }
            return estudiantes;
        }
    }
}
