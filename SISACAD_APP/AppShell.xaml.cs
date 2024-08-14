using SISACAD_APP.Entidades;
using SISACAD_APP.Views;
using System.Windows.Input;

namespace SISACAD_APP
{
    public partial class AppShell : Shell
    {
     

        public AppShell()
        {
            InitializeComponent();

            // Registra la ruta para direccionar
            Routing.RegisterRoute("listadoDepositos", typeof(ListaDepositosView));


        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            Preferences.Clear();
            Device.BeginInvokeOnMainThread(() =>
            {
                // Redirigir al usuario a la página de inicio de sesión
                App.Current.MainPage = new NavigationPage(new LoginView());
            });
        }
    }
}
