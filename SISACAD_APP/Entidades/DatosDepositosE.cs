using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISACAD_APP.Entidades
{
    public class DatosDepositosE
    {
        public string? StrEstudiante { get; set; }
        public string? StrBancoEstudiante { set; get; }
        public string? StrBancoDestino { set; get; }
        public string? StrCuentaDestino { set; get; }
        public DateTime DatFechaPago { set; get; }
        public string? StrDocumento { set; get; }
        public byte[] ImgImagenDocumento { set; get; }
        public decimal? DecValor { set; get; }
        public string? StrCodigoUsuario { set; get; }
        public string? StrObservaciones { set; get; }
        public string? StrCodigoBancoPichincha { set; get; }
        public List<PagosDetalleE> CarritoCompras { get; set; }

    }
    public class DatosReportesPagosM
    {
        public string Estado { get; set; }
        public decimal Total { set; get; }
        public string Color { set; get; }
       
    }
    public class DatosPagosDetalleM
    {
        public string TipoProducto { get; set; }
        public List<DatosReportesPagosM> DatosReportes { get; set; }
    }

    public class DepositosE
    {
        public int DepId { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string DepBancoEstudiante { get; set; }
        public string DepCuentaDestino { get; set; }
        public string DepFechaPago { get; set; }
        public string DepDocumento { get; set; }
        public decimal DepValor { get; set; }
        public string DepUsuario { get; set; }
        public DateTime? DepFechaRegistro { get; set; }
        public string DepEstadoDeposito { get; set; }
        public string DepObservaciones { get; set; }
        public string Nombres { get; set; }
        public List<DepositosDetalleE> Productos { get; set; }
    }
    public class DepositosDetalleE
    {
        public int DepId { get; set; }
        public string DpdCodigoProducto { get; set; }
        public decimal? DpdValorProducto { get; set; }
        public decimal? DpdValorPagado { get; set; }
    }
}
