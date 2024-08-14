using Controls.UserDialogs.Maui;
using Plugin.LocalNotification;
using SISACAD_APP.ViewModel;
using static SISACAD_APP.Entidades.UsuarioSessionE;

namespace SISACAD_APP.Views;

public partial class LoginView : ContentPage
{
	public LoginView()
	{
		InitializeComponent();
        BindingContext = new LoginViewModel();
        // Quitar la barra de navegación
        NavigationPage.SetHasNavigationBar(this, false);


    }
    private async void OnLoginMessageChanged(object sender, string e)
    {
       
        if (e == "Login successful!")
        {
            await DisplayAlert("Login", e, "OK");

           

        }
        else
        {
            await DisplayAlert("Login", e, "OK");
        }
       
    }
  
}