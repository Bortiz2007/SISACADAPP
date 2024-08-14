using Controls.UserDialogs.Maui;
using SISACAD_APP.Entidades;
using SISACAD_APP.Model;
using SISACAD_APP.Servicios;
using SISACAD_APP.Views;
using System.Windows.Input;


namespace SISACAD_APP.ViewModel
{
    class TranferenciasyDepositosViewModel : BaseViewModel
    {
        #region VARIABLES

        public List<PagosDetalleE> _matriYpens;
        public List<PagosDetalleE> _educaContinua;
        public List<PagosDetalleE> _titulacion;
        public List<PagosDetalleE> _otrosPagos;
        public List<PagosDetalleE> _carritoCompras;
        List<PagosDetalleE> ProductoAgregado = new List<PagosDetalleE>();
        public string identificacion, codigoP, cod_usuario;
        public bool h1 = false, h2 = false, h3 = false, h4 = false, transaccion;
        public bool _existePago = true;
        public double op1, op2, op3, op4;
        decimal _total = 0;
        string _busqueda, _carrera, _fullCarrera, _codigoCarrera;
        public EstudiantesModel _selectedCarrera;
        bool _mostrarElementos = false;
        public List<EstudiantesModel> _datosEstudiante;
        #endregion
        #region OBJETOS
        public decimal Total
        {
            get { return _total; }
            set { SetValue(ref _total, value); }
        }
        public List<PagosDetalleE> MatriYPens
        {
            get { return _matriYpens; }
            set { SetValue(ref _matriYpens, value); OnPropertyChanged(nameof(MatriYPens)); }
        }
        public string Carrera
        {
            get { return _carrera; }
            set { SetValue(ref _carrera, value); OnPropertyChanged(nameof(Carrera)); }
        }
        public string FullCarrera
        {
            get { return _fullCarrera; }
            set { SetValue(ref _fullCarrera, value); OnPropertyChanged(nameof(FullCarrera)); }
        }
        public string CodigoCarrera
        {
            get { return _codigoCarrera; }
            set { SetValue(ref _codigoCarrera, value); OnPropertyChanged(nameof(CodigoCarrera)); }
        }
        public List<PagosDetalleE> CarritodeCompras
        {
            get { return _carritoCompras; }
            set { SetValue(ref _carritoCompras, value); OnPropertyChanged(nameof(CarritodeCompras)); }
        }
        public List<PagosDetalleE> EducaConti
        {
            get { return _educaContinua; }
            set { SetValue(ref _educaContinua, value); OnPropertyChanged(nameof(EducaConti)); }
        }
        public List<PagosDetalleE> TitulacionT
        {
            get { return _titulacion; }
            set { SetValue(ref _titulacion, value); OnPropertyChanged(nameof(TitulacionT)); }
        }
        public List<PagosDetalleE> OtroPagos
        {
            get { return _titulacion; }
            set { SetValue(ref _titulacion, value); OnPropertyChanged(nameof(OtroPagos)); }
        }
        public bool H1
        {
            get { return h1; }
            set { SetValue(ref h1, value); OnPropertyChanged(nameof(H1)); }
        }
        public bool H2
        {
            get { return h2; }
            set { SetValue(ref h2, value); OnPropertyChanged(nameof(H2)); }
        }
        public bool H3
        {
            get { return h3; }
            set { SetValue(ref h3, value); OnPropertyChanged(nameof(H3)); }
        }
        public bool H4
        {
            get { return h4; }
            set { SetValue(ref h4, value); OnPropertyChanged(nameof(H4)); }
        }
        public bool Transaccion
        {
            get { return transaccion; }
            set { SetValue(ref transaccion, value); OnPropertyChanged(nameof(Transaccion)); }
        }
        public bool ExistePago
        {
            get { return _existePago; }
            set { SetValue(ref _existePago, value); OnPropertyChanged(nameof(ExistePago)); }
        }
        public string Busqueda
        {
            get { return _busqueda; }
            set { SetValue(ref _busqueda, value); OnPropertyChanged(nameof(Busqueda)); }
        }
        public bool MostrarElementos
        {
            get { return _mostrarElementos; }
            set { SetValue(ref _mostrarElementos, value); OnPropertyChanged(nameof(MostrarElementos)); }
        }
        public List<EstudiantesModel> DatosEstudiante
        {
            get { return _datosEstudiante; }
            set { SetValue(ref _datosEstudiante, value); OnPropertyChanged(nameof(DatosEstudiante)); }
        }
        //public EstudiantesModel SelectedCarrera
        //{
        //    get { return _selectedCarrera; }
        //    set
        //    {
        //        if (_selectedCarrera != value)
        //        {
        //            _selectedCarrera = value;
        //            Carrera = _selectedCarrera.FullCarrera;
        //        }
        //    }
        //}

        #endregion
        #region CONSTRUCTORES
        public TranferenciasyDepositosViewModel(string cedula)
        {

            Busqueda = cedula;
            MostrarElementos = ServicioSession.GetIdRol() == "4" || ServicioSession.GetIdRol() == "3" ? false : true;
            //cod_usuario = cod_usu;

        }


        #endregion
        #region OBJETOS
        #endregion
        #region PROCESOS
        public async Task CargarCarrera()
        {

            try
            {
                var funcion = new PeriodoServicio();
                Busqueda = String.IsNullOrEmpty(Busqueda) ? ServicioSession.GetCedula() : Busqueda;
                List<EstudiantesModel> lista = new List<EstudiantesModel>();
                lista = await funcion.GetPeriodos(Busqueda);

                foreach (var estudiante in lista)
                {
                    FullCarrera = estudiante.FullCarrera;
                    CodigoCarrera = estudiante.Codigo_carrera;
                }


            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Por favor, contacte a soporte técnico al +593 98 464 1470.", "Error", "Aceptar", "@drawable/error_login.png");
            }




        }
        public async Task MostrarProductos()
        {
            var loading = UserDialogs.Instance.Loading("Cargando...");
            try
            {

                var funcion = new TransferenciaModel();
                Busqueda = System.String.IsNullOrEmpty(Busqueda) ? ServicioSession.GetCedula() : Busqueda;

                ExistePago = await funcion.ExistenPagosPendientes(Busqueda);
                if (ExistePago)
                {


                    List<List<PagosDetalleE>> list = await funcion.ObtenerProductos(Busqueda, "", Carrera);
                    if (list.Count > 0)
                    {

                        MatriYPens = new List<PagosDetalleE>(FiltrarPorTipo(list, "MATPENS"));
                        EducaConti = new List<PagosDetalleE>(FiltrarPorTipo(list, "EDUCON"));
                        TitulacionT = new List<PagosDetalleE>(FiltrarPorTipo(list, "TIT"));
                        OtroPagos = new List<PagosDetalleE>(FiltrarPorTipo(list, "OTROSPAGOS"));
                    }
                    else
                    {

                    }
                    H1 = MatriYPens.Count > 0 ? true : false;
                    H2 = EducaConti.Count > 0 ? true : false;
                    H3 = TitulacionT.Count > 0 ? true : false;
                    H4 = OtroPagos.Count > 0 ? true : false;
                    if (H1 || H2 || H3 || H4)
                    {
                        Transaccion = true;
                    }
                    else
                    {
                        Transaccion = false;

                        UserDialogs.Instance.Alert("Estimado usuario, no tiene pagos pendientes por realizar en este momento", "Informativo", "Aceptar", "@drawable/info.png");
                    }

                }
                else
                {
                    UserDialogs.Instance.Alert("Estimado usuario tiene depósitos pendientes por facturar.", "Factura Pendiente", "Aceptar", "@drawable/info.png");
                    if (ServicioSession.GetIdRol() == "4" || ServicioSession.GetIdRol() == "3")
                    {
                        Application.Current.MainPage = new AppShell();
                        await Shell.Current.GoToAsync("listadoDepositos");
                    }

                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Por favor contacte a soporte técnico +593 98 464 1470", "Error", "Aceptar", "@drawable/error.png");

            }
            finally
            {
                await Task.Delay(1000);
                loading.Dispose();
            }

        }

        private List<PagosDetalleE> FiltrarPorTipo(List<List<PagosDetalleE>> lista, string tipo)
        {
            return lista
                    .SelectMany(sublista => sublista) // Aplana la lista de listas en una sola lista
                    .Where(item => item.Tipo == tipo) // Filtra según el tipo
                    .ToList(); // Convierte el resultado en una lista
        }

        public async Task AgregarCompra(PagosDetalleE producto)
        {

            if (producto.efdTotalEnviado > 0 && (producto.efdTotalEnviado <= (Convert.ToDecimal(producto.efdValorProducto) - producto.efdDescuento)))
            {

                if (!ProductoAgregado.Contains(producto))
                {
                    producto.botonAgregarP = true;
                    producto.botonEliminarP = false;
                    ProductoAgregado.Add(producto);
                    CarritodeCompras = ProductoAgregado;
                    Total = Total + Convert.ToDecimal(producto.efdTotalEnviado);
                    UserDialogs.Instance.Alert("El producto " + producto.producto + " se agrego con exito", "Producto Agregado", "Aceptar", "@drawable/info.png");
                }
                else
                {
                    UserDialogs.Instance.Alert("Estimado usuario el producto " + producto.producto + " ya se agrego anteriorente", "Producto Existente", "Aceptar", "@drawable/info.png");
                }

            }
            else
            {
                UserDialogs.Instance.Alert("Estimado usuario ingrese correctamente los valores, recuerde que las cantidades ingresadas deben ir con .", "Error", "Aceptar", "@drawable/info.png");
            }

        }
        public async Task QuitarCarritoC(PagosDetalleE producto)
        {
            try
            {

                if (CarritodeCompras.Contains(producto))
                {
                    producto.botonAgregarP = true;
                    producto.botonEliminarP = false;
                    var indexC = ProductoAgregado.IndexOf(producto);
                    ProductoAgregado.RemoveAt(indexC);
                    CarritodeCompras = ProductoAgregado;
                    Total = Total - Convert.ToDecimal(producto.efdTotalEnviado);
                    // await DisplayAlert("Producto Eliminado", "El producto " + producto.producto + " se removio con exito", "ok");
                    UserDialogs.Instance.Alert("El producto " + producto.producto + " se removio con exito" + producto.producto + " se agrego con exito", "Producto Eliminado", "Aceptar", "@drawable/info.png");
                }
                else
                {
                    UserDialogs.Instance.Alert("El producto " + producto.producto + " no ha sido añadido", "Producto Inexistente", "Aceptar", "@drawable/info.png");
                    // await DisplayAlert("Producto Inexistente", "El producto " + producto.producto + " no ha sido añadido", "ok");

                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("El producto " + producto.producto + " no ha sido añadido", "Producto Inexistente", "Aceptar", "@drawable/info.png");
                //await DisplayAlert("Producto Inexistente", "El producto " + producto.producto + " no ha sido añadido", "ok");
            }


        }

        #endregion
        #region COMANDOS
        public ICommand MuestraProductos => new Command(async () => await MostrarProductos());
        public ICommand AgregarCarrito => new Command<PagosDetalleE>(async (p) => await AgregarCompra(p));
        public ICommand QuitarCarrito => new Command<PagosDetalleE>(async (p) => await QuitarCarritoC(p));
        //public ICommand CarritoEnviar => new Command(async () => await EnviaCarritoC());
        #endregion
    }
}
