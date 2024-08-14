
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISACAD_APP.Entidades
{
    public class PagosDetalleE
    {
        
            public string? efiCodigoEstadoFinanciero { get; set; }
            public string? efdCodigoEstadoFinancieroDetalle { get; set; }
            public string? CodigoEstadoFinancieroDetalle { get; set; }
            public string? efdNumeroCuota { get; set; }
            public string? efdCodigoProducto { get; set; }
            public string? CodigoProducto { get; set; }
            public decimal? efdValorProducto { get; set; }
            public decimal? ValorProducto { get; set; }
            public decimal? ValorPagado { get; set; }
            public decimal? efdTotal { get; set; }
            public decimal? efdDescuento { get; set; }
            public decimal? efdTotalEnviado { get; set; }
            public decimal? efdValorBeca { get; set; }
            public decimal? efdPorcentajeBeca { get; set; }
            public string? efdCodigoEstadoDeuda { get; set; }
            public string? Tipo { get; set; }
            public string? producto { get; set; }
            public string? estadoTransaccionTarjeta { get; set; }
            public string? dpdCodigoEstadoFinancieroDetalle { get; set; }
            public string? dpdCodigoProducto { get; set; }
            public string? dpdValorProducto { get; set; }
            public string? dpdValorPagado { get; set; }
            public decimal? descuentoAdicional { get; set; }
            public bool botonAgregarP { get; set; }
            public bool botonEliminarP { get; set; }
            public string? efdFechaVencimiento { get; set; }
            public DateTime? efdFechaPago { get; set; }
            public string? efdNumeroFactura { get; set; }
            public string? efdSucursal { get; set; }
            public string? efdCodigoCarrera { get; set; }
            public string? efdPeriodoAcademico { get; set; }


    }
    public class HistoricoPagos
    {
        public string Tipo { get; set; }
        public decimal Total { get; set; }
        public string Color { get; set; }
        public string TipoCuenta { get; set; }
    }

}
