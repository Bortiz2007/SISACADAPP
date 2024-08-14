using Controls.UserDialogs.Maui;
using SISACAD_APP.Entidades;
using SISACAD_APP.Model;
using SISACAD_APP.Servicios;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SISACAD_APP.ViewModel
{
    public class ListaDepositosViewModel:BaseViewModel
    {
        #region Clases
        public class PickerItem
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }
        #endregion
        #region VARIABLES

 
        public DateTime _fecha_inicio = DateTime.Now.AddDays(-30);
        public DateTime _fecha_fin = DateTime.Now;
        public List<DepositosE> _depositosDetalle;
        string _busqueda;
        string _filtro;
        private PickerItem _selectedFiltro;
        private ObservableCollection<PickerItem> _filtroItems;
        bool _mostrarElementos = false;

        #endregion
        #region OBJETOS
        public bool MostrarElementos
        {
            get { return _mostrarElementos; }
            set { SetValue(ref _mostrarElementos, value); }
        }
        public ObservableCollection<PickerItem> FiltroItems
        {
            get => _filtroItems;
            set => SetProperty(ref _filtroItems, value);
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
        public PickerItem SelectedFiltro
        {
            get => _selectedFiltro;
            set => SetProperty(ref _selectedFiltro, value);
        }

        public List<DepositosE> DepositosDetalle
        {


            get { return _depositosDetalle; }
            set { SetValue(ref _depositosDetalle, value); OnPropertyChanged(nameof(DepositosDetalle)); }
        }
        public string Busqueda
        {
            get { return _busqueda; }
            set { SetValue(ref _busqueda, value); OnPropertyChanged(); }
        }
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

  


        #endregion

        #region CONSTRUCTORES

        public ListaDepositosViewModel(string identificacionC)
        {
            Busqueda = identificacionC;
            MostrarElementos = ServicioSession.GetIdRol() == "4" || ServicioSession.GetIdRol() == "3" ? false : true;
        }


        #endregion
        #region OBJETOS
        #endregion
        #region PROCESOS


        public void CargarEstadoDepositos()
        {
            FiltroItems = new ObservableCollection<PickerItem>();
            FiltroItems.Clear(); // Limpiar la colección existente

            FiltroItems = new ObservableCollection<PickerItem>
            {
                 new PickerItem{ Text = "TODOS", Value = "TODOS" },
                new PickerItem { Text = "FACTURADO", Value = "FACTURADO" },
                new PickerItem { Text = "DEVUELTO", Value = "DEVUELTO" },
                new PickerItem { Text = "RECHAZADO", Value = "RECHAZADO" },
                new PickerItem { Text = "INGRESADO", Value = "INGRESADO" },
                new PickerItem { Text = "ESPERA FACTURA", Value = "ESPERA FACTURA" }
            };


        }
        public List<DepositosE> MostrarDepositosFiltros()
        {
            List<DepositosE> DepositosFiltro = new List<DepositosE>();

            try
            {
                if (_selectedFiltro.Value == "TODOS")
                {
                    DepositosFiltro = DepositosDetalle;
                }
                else
                {
                    foreach (var item in DepositosDetalle)
                    {
                        if(_selectedFiltro.Value == "FACTURADO"){
                            if ((item.DepEstadoDeposito.Contains("FACTURADO") || item.DepEstadoDeposito.Contains("FACTURACION")) && !item.DepEstadoDeposito.Contains("ESPERA"))
                            {
                                DepositosFiltro.Add(item);
                            }
                        }
                        else
                        {
                            if (item.DepEstadoDeposito == _selectedFiltro.Value)
                            {
                                DepositosFiltro.Add(item);
                            }
                        }
                        
                    }
                }


            }
            catch (Exception ex)
            {

                DepositosFiltro = DepositosDetalle;
                UserDialogs.Instance.Alert("Por favor, contacte a soporte técnico al +593 98 464 1470.", "Error", "Aceptar", "@drawable/error_login.png");
            }


            return DepositosFiltro;

        }
        public async Task BusquedaDepositosDetalle()
        {

            var loading = UserDialogs.Instance.Loading("Cargando...");
            try
            {
                var funcion = new PagosModel();
                string fechaI = Fecha_inicio.Year + "-" + Fecha_inicio.Month + "-" + Fecha_inicio.Day;
                string fechaF = Fecha_fin.Year + "-" + Fecha_fin.Month + "-" + Fecha_fin.Day;
                DepositosDetalle = new List<DepositosE>();
                DepositosDetalle = await funcion.GetDespositosDetalle(Busqueda, fechaI, fechaF);
          
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Por favor, contacte a soporte técnico al +593 98 464 1470.", "Error", "Aceptar", "@drawable/error_login.png");
            }
            finally
            {

                await Task.Delay(200);
                loading.Dispose();
            }



        }
        public async Task BorrarDeposito(DepositosE producto)
        {
            try 
            {
                var funcion = new PagosModel();

                DepositosDetalleE depositos = new DepositosDetalleE();
                depositos.DepId = producto.DepId;

                if (await funcion.BorrarDeposito(depositos))
                {
                  
                    UserDialogs.Instance.Alert("El depósito se eliminó con éxito", "Depósito Eliminado", "Aceptar", "@drawable/info.png");
                }
                else
                {
                    UserDialogs.Instance.Alert("Hubo un error al intentar eliminar el depósito. Por favor, inténtelo más tarde.", "Error", "Aceptar", "@drawable/error_login.png");
                }

            }
            catch(Exception ex) 
            {
                UserDialogs.Instance.Alert("Por favor, contacte a soporte técnico al +593 98 464 1470.", "Error", "Aceptar", "@drawable/error_login.png");

            }

        }


        #endregion
        #region COMANDOS

        public ICommand BusquedaDepositosDetail => new Command(async () => await BusquedaDepositosDetalle());
        public ICommand BorrarDepositoId => new Command<DepositosE>(async (p) => await BorrarDeposito(p));
        #endregion
    }
}
