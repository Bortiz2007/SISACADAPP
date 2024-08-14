using Controls.UserDialogs.Maui;
using SISACAD_APP.Model;
using SISACAD_APP.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISACAD_APP.ViewModel
{
    public class EstudianteViewModel : BaseViewModel
    {

        #region VARIABLES
        public List<EstudiantesModel> _datosEstudiante;
        public EstudiantesModel _selectedPeriodo;
        public string identificacion;
        public EstudiantesModel _selectedCarrera;
        public string _periodo, _carrera;
        string _busqueda;
        bool _mostrarElementos = false;

        #endregion
        #region OBJETOS


        public List<EstudiantesModel> DatosEstudiante
        {
            get { return _datosEstudiante; }
            set { SetValue(ref _datosEstudiante, value); }
        }
        public string Carrera
        {
            get { return _carrera; }
            set
            {
                if (_carrera != value)
                {
                    _carrera = value;
                    OnpropertyChanged();
                }
            }
        }
        public string Periodo
        {
            get { return _periodo; }
            set
            {
                if (_periodo != value)
                {
                    _periodo = value;
                    OnpropertyChanged();
                }
            }
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
        #endregion
        #region CONSTRUCTORES

        public EstudianteViewModel(string idConsulta)
        {
            
            Busqueda = idConsulta;
            MostrarElementos = ServicioSession.GetIdRol() == "4" || ServicioSession.GetIdRol() == "3" ? false : true;
        }
        #endregion
        #region PICKER SELECTER
        public EstudiantesModel SelectedPeriodo
        {
            get { return _selectedPeriodo; }
            set
            {
                if (_selectedPeriodo != value)
                {
                    _selectedPeriodo = value;
                    Periodo = _selectedPeriodo.Periodo_academico;
                }
            }
        }

        public EstudiantesModel SelectedCarrera
        {
            get { return _selectedCarrera; }
            set
            {
                if (_selectedCarrera != value)
                {
                    _selectedCarrera = value;
                    Carrera = _selectedCarrera.FullCarrera;
                }
            }
        }


        #endregion
        #region PROCESOS
        public async Task DatosEstudianteCarga()
        {
            try
            {
                var funcion = new PeriodoServicio();
                Busqueda = String.IsNullOrEmpty(Busqueda) ? ServicioSession.GetCedula() : Busqueda;
                DatosEstudiante = await funcion.GetPeriodos(Busqueda);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Por favor, contacte a soporte técnico al +593 98 464 1470.", "Error", "Aceptar", "@drawable/error_login.png");
            }
        }
        #endregion
    }
}
