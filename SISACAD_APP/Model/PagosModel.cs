using Newtonsoft.Json;
using SISACAD_APP.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SISACAD_APP.Model
{
    public class PagosModel
    {
        public async Task<List<PagosDetalleE>> GetHistorialPagos(string identificacion, string tipoConsulta)
        {
            List<PagosDetalleE> pagos = new List<PagosDetalleE>();
          
            string busqueda = "identificacion=" + identificacion + "&filtro=" + tipoConsulta;
            ConexionAPIModel conexion = new ConexionAPIModel();
            HttpResponseMessage response = await conexion.ConexionAPI(busqueda);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    string content = await response.Content.ReadAsStringAsync();
                    pagos = JsonConvert.DeserializeObject<List<PagosDetalleE>>(content);
                }
                catch (Exception ex)
                {

                }
            }
            return pagos;
        }
        public async Task<List<DataTable>> GetInformacionPanelPagos(string numeroIdentificacion, string fechaInicio, string fechaFin)
        {
            string busqueda = "numeroIdentificacion=" + numeroIdentificacion + "&fechaInicio=" + fechaInicio + "&fechaFin=" + fechaFin;
            List<DataTable> panelPagos = new List<DataTable>();
            ConexionAPIModel conexion = new ConexionAPIModel();
            HttpResponseMessage response = await conexion.ConexionAPI(busqueda);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    string content = await response.Content.ReadAsStringAsync();
                    panelPagos = JsonConvert.DeserializeObject<List<DataTable>>(content);
                }
                catch (Exception ex)
                {

                }
            }
            return panelPagos;
        }
        public async Task<List<FacturasE>> GetFacturasDetalle(string identificacion, string fechaInicio, string FechaFin)
        {
            List<FacturasE> facturas = new List<FacturasE>();
            string busqueda = "filtro=" + identificacion + "&fechaI=" + fechaInicio + "&fechaF=" + FechaFin;
            ConexionAPIModel conexion = new ConexionAPIModel();
            HttpResponseMessage response = await conexion.ConexionAPI(busqueda);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    string content = await response.Content.ReadAsStringAsync();
                    facturas = JsonConvert.DeserializeObject<List<FacturasE>>(content);
                }
                catch (Exception ex)
                {

                }



            }
            return facturas;
        }
        public async Task<List<DepositosE>> GetDespositosDetalle(string identificacion, string fechaInicio, string FechaFin)
        {
            List<DepositosE> despositos = new List<DepositosE>();
            string busqueda = "busquedaDato=" + identificacion + "&fechaIni=" + fechaInicio + "&fechaFin=" + FechaFin;
            ConexionAPIModel conexion = new ConexionAPIModel();
            HttpResponseMessage response = await conexion.ConexionAPI(busqueda);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    string content = await response.Content.ReadAsStringAsync();
                    despositos = JsonConvert.DeserializeObject<List<DepositosE>>(content);
                }
                catch (Exception ex)
                {

                }



            }
            return despositos;
        }
        public async Task<bool> BorrarDeposito(DepositosDetalleE datosDepositos)
        {
            bool res = false;
            try 
            {
                string envio = "postdeposito";
                
                ConexionAPIModel conexion = new ConexionAPIModel();
                HttpResponseMessage response = await conexion.ConexionAPIPOST(envio, datosDepositos);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    var respuesta = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, int>>>>(content);

                    if (respuesta != null && respuesta.ContainsKey("Table"))
                    {
                        int respuestaValue = respuesta["Table"][0]["respuesta"];
                        res = Convert.ToBoolean(respuestaValue);

                  
                    }

                }
            }
            catch(Exception ex)
            {
                res = false;
            }
            
            return res;
        }
    }
}
