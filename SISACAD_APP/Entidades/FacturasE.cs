using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISACAD_APP.Entidades
{
    public class FacturasE
    {
        public int? NumeroFactura { get; set; }
        public string? RucRepresentante { get; set; }
        public string? FechaTransaccion { get; set; }
        public string? ContadoCredito { get; set; }
        public int? DiasCredito { get; set; }
        public string? AutFactura { get; set; }
        public string? SerieFactura { get; set; }
        public int? IdFormaPago { get; set; }
        public int? IdProyecto { get; set; }
        public string? Notas { get; set; }
        public int? IdComprobante { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? FechaRegistro { get; set; }
        public string? EstadoTransaccion { get; set; }
        public string? OrigenTransaccion { get; set; }
        public string? UsuFacturador { get; set; }
        public string? NumComprobante { get; set; }
        public string? IdentificacionEstudiante { get; set; }
        public Decimal? ValorTotal { get; set; }
        public string? NombreSucursal { get; set; }
        public string? nombres { get; set; }
        public List<ProductoDetalleFactura>? Producto { get; set; }
  
    }
    public class ProductoDetalleFactura
    {
        public string? ProductoF { get; set; }
        public Decimal? Precio { get; set; }
        public Decimal? Descuento { get; set; }
    }
}
