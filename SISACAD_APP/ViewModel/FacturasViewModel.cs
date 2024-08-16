using Controls.UserDialogs.Maui;
using SISACAD_APP.Entidades;
using SISACAD_APP.Model;
using SISACAD_APP.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SISACAD_APP.ViewModel
{
    public class FacturasViewModel:BaseViewModel
    {
        #region VARIABLES
        private bool isLoading;
        public string identificacion;
        public DateTime _fecha_inicio = DateTime.Now.AddDays(-90);
        public DateTime _fecha_fin = DateTime.Now;
        public List<FacturasE> _facturasDetalle;
        string _busqueda;
        bool _mostrarElementos = false;

 

        #endregion
        #region OBJETOS

        public List<FacturasE> FacturasDetalle
        {

        
            get { return _facturasDetalle; }
            set { SetValue(ref _facturasDetalle, value); OnpropertyChanged(nameof(FacturasDetalle)); }
        }

        public DateTime Fecha_inicio
        {


            get { return _fecha_inicio; }
            set { SetValue(ref _fecha_inicio, value); OnpropertyChanged(nameof(Fecha_inicio)); }
        }
        public string Busqueda
        {
            get { return _busqueda; }
            set { SetValue(ref _busqueda, value); OnpropertyChanged(nameof(Busqueda)); }
        }
        public bool MostrarElementos
        {
            get { return _mostrarElementos; }
            set { SetValue(ref _mostrarElementos, value); OnpropertyChanged(nameof(MostrarElementos)); }
        }
        public DateTime Fecha_fin
        {


            get { return _fecha_fin; }
            set { SetValue(ref _fecha_fin, value); OnpropertyChanged(nameof(Fecha_fin)); }
        }


      
        #endregion
        #region CONSTRUCTORES

        public FacturasViewModel(string identificacionC)
        {
            
            Busqueda = identificacionC;
            MostrarElementos = ServicioSession.GetIdRol() == "4" || ServicioSession.GetIdRol() == "3" ? false : true;
          
        }
    
        #endregion
        #region OBJETOS
        #endregion
        #region PROCESOS




        public async Task BusquedaFacturasDetalle()
        {

            var loading = UserDialogs.Instance.Loading("Cargando...");
         
            try
            {
               
                var funcion = new PagosModel();
                string fechaI = Fecha_inicio.Year + "-" + Fecha_inicio.Month + "-" + Fecha_inicio.Day;
                string fechaF = Fecha_fin.Year + "-" + Fecha_fin.Month + "-" + Fecha_fin.Day;
                FacturasDetalle = await funcion.GetFacturasDetalle(Busqueda, fechaI, fechaF);
           
            }
            catch (Exception ex)
            {

            }
            finally
            {

                await Task.Delay(100);
                loading.Dispose();
            }


        }
    
        #endregion
        #region COMANDOS
     
        public ICommand BusquedaFacturasDetail => new Command(async () => await BusquedaFacturasDetalle());
       
        #endregion
    }
}
