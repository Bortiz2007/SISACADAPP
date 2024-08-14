using Controls.UserDialogs.Maui;
using SISACAD_APP.Entidades;
using SISACAD_APP.Model;
using SISACAD_APP.Servicios;
using SISACAD_APP.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SISACAD_APP.ViewModel
{
    public class HistorialPagosViewModel : BaseViewModel
    {

        #region Clases
        public class PickerItem
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }
        #endregion

        #region VARIABLES

        public List<PagosDetalleE> _pagosDetalle;
        
        string _filtro;
        private PickerItem _selectedFiltro;
        private ObservableCollection<PickerItem> _filtroItems;
        public string identificacion;
        public bool h1 = false, h2 = false, h3 = false, h4 = false, h5 = false, hIng = false, hOtro = false;
        public bool _existePago = true;
        public double op1;
        string _busqueda;
        bool _mostrarElementos = false;
        #endregion
        #region OBJETOS

        public List<PagosDetalleE> PagosDetalle
        {
            get { return _pagosDetalle; }
            set { SetValue(ref _pagosDetalle, value); }
        }

        public ObservableCollection<PickerItem> FiltroItems
        {
            get => _filtroItems;
            set => SetProperty(ref _filtroItems, value);
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
        public PickerItem SelectedFiltro
        {
            get => _selectedFiltro;
            set => SetProperty(ref _selectedFiltro, value);
        }
        public bool H1
        {
            get { return h1; }
            set { SetValue(ref h1, value); }
        }

        public bool H2
        {
            get { return h2; }
            set { SetValue(ref h2, value); }
        }
        public bool H3
        {
            get { return h3; }
            set { SetValue(ref h3, value); }
        }
        public bool H4
        {
            get { return h4; }
            set { SetValue(ref h4, value); }
        }
        public bool H5
        {
            get { return h5; }
            set { SetValue(ref h5, value); }
        }
        public bool Hotro
        {
            get { return hOtro; }
            set { SetValue(ref hOtro, value); }
        }
        public bool Hingles
        {
            get { return hIng; }
            set { SetValue(ref hIng, value); }
        }
      
        public string Filtro
        {
            get { return _filtro; }
            set
            {
                if (_filtro != value)
                {
                    _filtro = value;
                    OnpropertyChanged();
                }
            }
        }

   
        #endregion

        #region CONSTRUCTORES

        public HistorialPagosViewModel(string identificacionC, string filtroC)
        {
            Busqueda = identificacionC;
            _filtro = filtroC;
            Filtro = filtroC;
            MostrarElementos = ServicioSession.GetIdRol() == "4" || ServicioSession.GetIdRol() == "3" ? false : true;
            ValidaProductos = new Command(async () => await validaProductosDisponibles());
          
        }

        #endregion
    
        #region PROCESOS

        public async Task MostrarProductos()
        {
            var loading = UserDialogs.Instance.Loading("Cargando...");
            try
            {
                CargarSemestres();
                var funcion = new PagosModel();
                Busqueda = String.IsNullOrEmpty(Busqueda) ? ServicioSession.GetCedula() : Busqueda;
                PagosDetalle = await funcion.GetHistorialPagos(Busqueda, Filtro);
                H1 = PagosDetalle.Count > 0 ? true : false;
                await Task.Delay(1000);
                loading.Dispose();
            }
            catch (Exception ex)
            {
          
                loading.Dispose();
                UserDialogs.Instance.Alert("Por favor, contacte a soporte técnico al +593 98 464 1470.", "Error", "Aceptar", "@drawable/error_login.png");
            }
       



        }
        public  List<PagosDetalleE> MostrarProductosFiltros()
        {
            List<PagosDetalleE> ProductosFiltro = new List<PagosDetalleE>();
           
            try
            {
                if (_selectedFiltro.Value == "TODOS")
                {
                    ProductosFiltro = PagosDetalle;
                }
                else
                {
                    foreach (var item in PagosDetalle)
                    {
                        if (item.efdCodigoEstadoDeuda == _selectedFiltro.Value)
                        {
                            ProductosFiltro.Add(item);
                        }
                    }
                }
               

            }
            catch (Exception ex)
            {

                 ProductosFiltro = PagosDetalle;
                UserDialogs.Instance.Alert("Por favor, contacte a soporte técnico al +593 98 464 1470.", "Error", "Aceptar", "@drawable/error_login.png");
            }

         
            return ProductosFiltro;

        }
        public void CargarSemestres()
        {
            FiltroItems = new ObservableCollection<PickerItem>();
            FiltroItems.Clear(); // Limpiar la colección existente
                                
            FiltroItems = new ObservableCollection<PickerItem>
            {
                new PickerItem { Text = "TODOS", Value = "TODOS" },
                new PickerItem { Text = "PAGADO", Value = "PAGADO" },
                new PickerItem { Text = "POR PAGAR", Value = "POR PAGAR" },
                new PickerItem { Text = "SALDO POR PAGAR", Value = "SALDO POR PAGAR" },
                new PickerItem { Text = "PAGO ANULADO", Value = "PAGO ANULADO" }
            };
       

        }
        public async Task validaProductosDisponibles()
        {
            var loading = UserDialogs.Instance.Loading("Cargando...");

            try
            {
               
                var funcion = new PagosModel();
                Busqueda = String.IsNullOrEmpty(Busqueda) ? ServicioSession.GetCedula() : Busqueda;
                PagosDetalle = await funcion.GetHistorialPagos(Busqueda, "TODO");
                int con1 = 0, con2 = 0, con3 = 0, con4 = 0, con5 = 0, conIn = 0, conOt = 0;

                foreach (PagosDetalleE pd in PagosDetalle)
                {
                    if ((pd.efdCodigoProducto.StartsWith("MATR") && pd.efdCodigoProducto.Contains("N1") ||
                       (pd.efdCodigoProducto.StartsWith("COL") && pd.efdCodigoProducto.Contains("N1"))) && !pd.efdCodigoProducto.Contains("ING"))
                    {
                        con1++;
                    }

                    if (((pd.efdCodigoProducto.StartsWith("MATR") && pd.efdCodigoProducto.Contains("N2")) ||
                    (pd.efdCodigoProducto.StartsWith("COL") && pd.efdCodigoProducto.Contains("N2"))) && !pd.efdCodigoProducto.Contains("ING"))
                    {
                        con2++;
                    }

                    if ((pd.efdCodigoProducto.StartsWith("MATR") && pd.efdCodigoProducto.Contains("N3") ||
                       (pd.efdCodigoProducto.StartsWith("COL") && pd.efdCodigoProducto.Contains("N3"))) && !pd.efdCodigoProducto.Contains("ING"))
                    {
                        con3++;
                    }

                    if ((pd.efdCodigoProducto.StartsWith("MATR") && pd.efdCodigoProducto.Contains("N4") ||
                       (pd.efdCodigoProducto.StartsWith("COL") && pd.efdCodigoProducto.Contains("N4"))) && !pd.efdCodigoProducto.Contains("ING"))
                    {
                        con4++;
                    }

                    if ((pd.efdCodigoProducto.StartsWith("MATR") && pd.efdCodigoProducto.Contains("N5") ||
                       (pd.efdCodigoProducto.StartsWith("COL") && pd.efdCodigoProducto.Contains("N5"))) && !pd.efdCodigoProducto.Contains("ING"))
                    {
                        con5++;
                    }

                    if (pd.efdCodigoProducto.Contains("ING"))
                    {
                        conIn++;
                    }

                    if (!pd.efdCodigoProducto.Contains("ING") && !pd.efdCodigoProducto.Contains("MATR") && !pd.efdCodigoProducto.StartsWith("COL"))
                    {
                        conOt++;
                    }


                }

                H1 = con1 > 0 ? true : false;
                H2 = con2 > 0 ? true : false;
                H3 = con3 > 0 ? true : false;
                H4 = con4 > 0 ? true : false;
                H5 = con5 > 0 ? true : false;
                Hingles = conIn > 0 ? true : false;
                Hotro = conOt > 0 ? true : false;
            
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Por favor, contacte a soporte técnico al +593 98 464 1470.", "Error", "Aceptar", "@drawable/error_login.png");
            }

            await Task.Delay(2000);
            loading.Dispose();

        }
    
        #endregion
        #region COMANDOS
        public ICommand MuestraProductos => new Command(async () => await MostrarProductos());
        public ICommand ValidaProductos { get; }

        #endregion

   
    }
}
