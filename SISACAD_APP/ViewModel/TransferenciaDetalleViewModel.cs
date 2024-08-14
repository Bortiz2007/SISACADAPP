using Controls.UserDialogs.Maui;
using Newtonsoft.Json;
using SISACAD_APP.Entidades;
using SISACAD_APP.Model;
using SISACAD_APP.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISACAD_APP.ViewModel
{
    public class TransferenciaDetalleViewModel:BaseViewModel
    {
        #region VARIABLES
        string strEstudiante;
        decimal _total;
        string _referencia;
        string _busqueda;
        public List<PagosDetalleE> CarritoCompra;
        private bool _isNavigating;
        #endregion
        #region OBJETOS
        public string Referencia 
        { 
            get { return _referencia; } 
            set { _referencia = value; OnPropertyChanged(nameof(Referencia)); } 
        }
        public decimal Total 
        {
            get { return _total; } 
            set { _total = value; OnPropertyChanged(nameof(Total)); }  
        }
        public string Busqueda
        {
            get { return _busqueda; }
            set { SetValue(ref _busqueda, value); }
        }
        #endregion
        #region CONSTRUCTORES
        public TransferenciaDetalleViewModel(string cedula, List<PagosDetalleE> CarritoCompras, decimal total)
        {

            Busqueda =  cedula;
            CarritoCompra = CarritoCompras;
            Total = total;


        }

        #endregion

        #region PROCESOS
        public async void InsertarDeposito(string strBancoEstudiante, string strBancoDestino,
                                                    string strCuentaDestino, string datFechaPago,
                                                     byte[] imgImagenDocumento, decimal decValor, string strCodigoUsuario, string strObservaciones,
                                                     string strCodigoBancoPichincha)
        {
          
            var loading = UserDialogs.Instance.Loading("Cargando...");
            try
            {
                string script = string.Empty;
                DatosDepositosE datosDepositosE = new DatosDepositosE
                {
                    StrEstudiante = Busqueda,
                    StrBancoEstudiante = strBancoEstudiante,
                    StrBancoDestino = strBancoDestino,
                    StrCuentaDestino = strCuentaDestino,
                    DatFechaPago = Convert.ToDateTime(datFechaPago),
                    StrDocumento = Referencia,
                    ImgImagenDocumento = imgImagenDocumento,
                    DecValor = decValor,
                    StrCodigoUsuario = strCodigoUsuario,
                    StrObservaciones = strObservaciones,
                    StrCodigoBancoPichincha = strCodigoBancoPichincha,
                    CarritoCompras = CarritoCompra
                };

                TransferenciaModel transferencia = new TransferenciaModel();
                await transferencia.IngresaDeposito(datosDepositosE);

             
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Alert("Por favor, contacte a soporte técnico al +593 98 464 1470.", "Error", "Aceptar", "@drawable/error_login.png");
            }
            finally
            {
                await Task.Delay(200);
                loading.Dispose();
          
            }


        }

        #endregion
        #region COMANDOS

        #endregion
    }
}
