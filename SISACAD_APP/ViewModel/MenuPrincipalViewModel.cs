using Controls.UserDialogs.Maui;
using Microcharts;
using SISACAD_APP.Entidades;
using SISACAD_APP.Servicios;
using SISACAD_APP.ViewModel;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SISACAD_APP.Model
{
    public class MenuPrincipalViewModel : BaseViewModel
    {
        #region VARIABLES
      
        public DateTime _fecha_inicio = DateTime.Now.AddDays(-90);
        public DateTime _fecha_fin = DateTime.Now;
        public List<DatosPagosDetalleM> _pagosDetalleTotal;
        List<ChartEntry> _panelPagos;
        List<ChartEntry> _panelDepositos;
        List<ChartEntry> _panelProductos;
        string _busqueda;
        bool _mostrarElementos = false,_carruselView = true,_panelPago= true, _panelDeposito= true, _panelProducto = true;
        #endregion
        #region OBJETOS
        public DateTime Fecha_inicio
        {
            get { return _fecha_inicio; }
            set { SetValue(ref _fecha_inicio, value); }
        }
        public DateTime Fecha_fin
        {
            get { return _fecha_fin; }
            set { SetValue(ref _fecha_fin, value); }
        }
        public List<ChartEntry> PanelPagos
        {
            get { return _panelPagos; }
            set { SetValue(ref _panelPagos, value); }
        }
        public List<ChartEntry> PanelDepositos
        {
            get { return _panelDepositos; }
            set { SetValue(ref _panelDepositos, value); }
        }
        public List<ChartEntry> PanelProductos
        {
            get { return _panelProductos; }
            set { SetValue(ref _panelProductos, value); }
        }
        public List<DatosPagosDetalleM> PagosDetalleTotal
        {
            get { return _pagosDetalleTotal; }
            set { SetValue(ref _pagosDetalleTotal, value); }
        }
        public string Busqueda
        {
            get { return _busqueda; }
            set { SetValue(ref _busqueda, value); }
        }
        public bool MostrarElementos
        {
            get { return _mostrarElementos; }
            set { SetValue(ref _mostrarElementos, value); }
        }
        public bool CarruselView
        {
            get { return _carruselView; }
            set { SetValue(ref _carruselView, value); }
        }
        public bool PanelPago
        {
            get { return _panelPago; }
            set { SetValue(ref _panelPago, value); }
        }
        public bool PanelDeposito
        {
            get { return _panelDeposito; }
            set { SetValue(ref _panelDeposito, value); }
        }
        public bool PanelProducto
        {
            get { return _panelProducto; }
            set { SetValue(ref _panelProducto, value); }
        }
        #endregion

        #region CONSTRUCTORES

        public MenuPrincipalViewModel(string identificacionC)
        {
            Busqueda = identificacionC;
            MostrarElementos = ServicioSession.GetIdRol() == "4" || ServicioSession.GetIdRol() == "3" ? false : true;

        }


        #endregion
        #region OBJETOS
        #endregion
        #region PROCESOS

        public async Task ObtieneInformacionPanel()
        {
            var loading = UserDialogs.Instance.Loading("Cargando...");
            await Task.Delay(1000);
            try
            {
                PanelPago = false;
                CarruselView = false;
                PanelDeposito = false;
                PanelProducto = false;
           
                PagosModel pagos = new PagosModel();
                PanelPagos = new List<ChartEntry>();
                PanelDepositos = new List<ChartEntry>();
                PanelProductos = new List<ChartEntry>();
                Busqueda = String.IsNullOrEmpty(Busqueda) ? ServicioSession.GetCedula() : Busqueda;
                string fechaI = Fecha_inicio.Year + "-" + Fecha_inicio.Month + "-" + Fecha_inicio.Day;
                string fechaF = Fecha_fin.Year + "-" + Fecha_fin.Month + "-" + Fecha_fin.Day;
                List<DataTable> res = await pagos.GetInformacionPanelPagos(Busqueda, fechaI, fechaF);
                if (res.Count > 0)
                {
                   
                    foreach (DataRow pago in res[0].Rows)
                    {
                      
                        float total = 0;
                        if (!string.IsNullOrEmpty(pago["totalProductos"]?.ToString()))
                        {
                            total = float.Parse(pago["totalProductos"].ToString());
                        }
                        PanelPago = true;
                        PanelPagos.Add(new ChartEntry((float)total)
                        {
                            Label = pago["estado"].ToString(),
                            ValueLabel = total.ToString(),
                            Color = SKColor.Parse(pago["Color"].ToString()),
                        

                        });
                    }

                    foreach (DataRow pago in res[2].Rows)
                    {

                        
                        PanelDeposito = true;
                        float total = 0;
                        if (!string.IsNullOrEmpty(pago["totalDepositos"]?.ToString()))
                        {
                            total = float.Parse(pago["totalDepositos"].ToString());
                        }
                        PanelDepositos.Add(new ChartEntry((float)total)
                        {
                            Label = pago["estado"].ToString(),
                            ValueLabel = total.ToString(),
                            Color = SKColor.Parse(pago["Color"].ToString()),
                            

                        });
                    }
                    List<DatosReportesPagosM> pagosMatriculasList = new List<DatosReportesPagosM>();
                    List<DatosReportesPagosM> pagosColegiaturasList = new List<DatosReportesPagosM>();
                    List<DatosReportesPagosM> pagoInglesList = new List<DatosReportesPagosM>();
                    List<DatosReportesPagosM> pagosOtrosList = new List<DatosReportesPagosM>();

                    foreach (DataRow row in res[1].Rows)
                    {
                        
                        
                        switch (row["tipoProducto"].ToString())
                        {
                            case "Colegiatura":
                                DatosReportesPagosM pagosColegiaturas = new DatosReportesPagosM();
                                pagosColegiaturas.Estado = row["estado"].ToString();
                                pagosColegiaturas.Total = Convert.ToDecimal(row["total"].ToString());
                                pagosColegiaturas.Color = row["Color"].ToString();
                                pagosColegiaturasList.Add(pagosColegiaturas);
                                break;
                            case "Matricula":
                                DatosReportesPagosM pagosMatriculas = new DatosReportesPagosM();
                                pagosMatriculas.Estado = row["estado"].ToString();
                                pagosMatriculas.Total = Convert.ToDecimal(row["total"].ToString());
                                pagosMatriculas.Color = row["Color"].ToString();
                                pagosMatriculasList.Add(pagosMatriculas);
                                break;
                            case "Ingles":
                                DatosReportesPagosM pagoIngle = new DatosReportesPagosM();
                                pagoIngle.Estado = row["estado"].ToString();
                                pagoIngle.Total = Convert.ToDecimal(row["total"].ToString());
                                pagoIngle.Color = row["Color"].ToString();
                                pagoInglesList.Add(pagoIngle);
                                break;
                            case "Otros":
                                DatosReportesPagosM pagosOtros = new DatosReportesPagosM();
                                pagosOtros.Estado = row["estado"].ToString();
                                pagosOtros.Total = Convert.ToDecimal(row["total"].ToString());
                                pagosOtros.Color = row["Color"].ToString();
                                pagosOtrosList.Add(pagosOtros);
                                break;
                        }
                        decimal total = 0;
                       
                        if (!string.IsNullOrEmpty(row["total"]?.ToString()))
                        {
                            total = Convert.ToDecimal(row["total"].ToString());
                        }
                       
                        PanelProductos.Add(new ChartEntry((float)total)
                        {
                            Label = row["tipoProducto"].ToString().ToUpper()+"-"+ row["estado"].ToString(),
                            ValueLabel = total.ToString(),
                            Color = SKColor.Parse(row["ColorProducto"].ToString()),


                        });

                    }
                    PagosDetalleTotal = new List<DatosPagosDetalleM>();
                  

                    PagosDetalleTotal.Add(new DatosPagosDetalleM { TipoProducto = "INFORMACIÓN DE MATRÍCULAS", DatosReportes = pagosMatriculasList });
                    PagosDetalleTotal.Add(new DatosPagosDetalleM { TipoProducto = "INFORMACIÓN DE COLEGIATURAS", DatosReportes = pagosColegiaturasList });
                    PagosDetalleTotal.Add(new DatosPagosDetalleM { TipoProducto = "INFORMACIÓN DE INGLES", DatosReportes = pagoInglesList });
                    PagosDetalleTotal.Add(new DatosPagosDetalleM { TipoProducto = "INFORMACIÓN DE OTROS PAGOS", DatosReportes = pagosOtrosList });

                    CarruselView= PagosDetalleTotal.Count > 0 ? true : false;
                    PanelProducto = PanelProductos.Count>0?true:false;
                }
              
            }
            catch (Exception ex)
            {

            }
            finally
            {
                await Task.Delay(1000);
                loading.Dispose();
            }
        }



        #endregion
        #region COMANDOS

        public ICommand ObtieneInformacion => new Command(async () => await ObtieneInformacionPanel());

        #endregion
    }
}
