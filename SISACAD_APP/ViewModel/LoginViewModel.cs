
using Controls.UserDialogs.Maui;
using Newtonsoft.Json;
using Plugin.LocalNotification;
using SISACAD_APP.Model;
using SISACAD_APP.Servicios;
using SISACAD_APP.ViewModel;
using SISACAD_APP.ViewModel1;
using SISACAD_APP.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;



namespace SISACAD_APP.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        #region VARIABLES
        private string _cod_usu;
        private string _clave;

        private bool _isNavigating;
        #endregion
        #region OBJETOS
        public event EventHandler<string> LoginMessageChanged;
        public string Cod_usu
        {
            get => _cod_usu;
            set
            {
                if (_cod_usu != value)
                {
                    _cod_usu = value;
                    OnPropertyChanged(nameof(Cod_usu));
                }
            }
        }

        public string Clave
        {
            get => _clave;
            set
            {
                if (_clave != value)
                {
                    _clave = value;
                    OnPropertyChanged(nameof(Clave));
                }
            }
        }
    
        #endregion
        #region CONSTRUCTORES
        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await OnLoginAsync(), () => !_isNavigating);
            SendInitialNotification();
        }
        #endregion
        #region PROCESOS
        private async Task OnLoginAsync()
        {
            string message;
            _isNavigating = true;
            LoginCommand.ChangeCanExecute();
            var loading = UserDialogs.Instance.Loading("Cargando...");
            try
            {
                if (!String.IsNullOrEmpty(Cod_usu) && !String.IsNullOrEmpty(Clave))
                {
                    LoginModel loginModel = new LoginModel();
                    var claveEncript = SeguridadModel.EncriptarTexto(Clave);
                    if (await loginModel.ValidaCredenciales(Cod_usu, claveEncript))
                    {
                        Application.Current.MainPage = new AppShell();

                    }
                    else
                    {
                        UserDialogs.Instance.Alert("Estimado usuario debe ingresar las credenciales correctas", "Credenciales Incorrectas", "Aceptar", "@drawable/error_login.png");

                    }
                }
                else
                {
                    UserDialogs.Instance.Alert("Estimado usuario Campos vacios", "Formulario vacio", "Aceptar", "@drawable/campos_vacios.png");

                }
            }
            //catch (Java.Lang.IllegalArgumentException exp)
            //{
            //    UserDialogs.Instance.Alert("Por favor intente nuevamente", "Error al inicial Sesión", "Aceptar", "@drawable/error_login.png");
            //}
            catch (Exception ex)
            {

                UserDialogs.Instance.Alert("Por favor, contacte a soporte técnico al +593 98 464 1470.", "Error al inicial Sesión", "Aceptar", "@drawable/error_login.png");
            }
            finally
            {
                await Task.Delay(200);
                loading.Dispose();
                _isNavigating = false;
                LoginCommand.ChangeCanExecute();
            }
        }
        private void SendInitialNotification()
        {
            var request = new NotificationRequest
            {
                NotificationId = 1123,
                Title = "Pago pendiente en sistema",
                Subtitle = "SISACAD Push",
                Description = "Esto es una prueba",
                BadgeNumber = 35,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(5),
                    NotifyRepeatInterval = TimeSpan.FromHours(1)
                }
            };
            LocalNotificationCenter.Current.Show(request);
        } 


    #endregion
    #region COMANDOS
    public Command LoginCommand { get; }
        #endregion
    }
}
