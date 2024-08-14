
using Controls.UserDialogs.Maui;
using SISACAD_APP.Servicios;
using SISACAD_APP.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace SISACAD_APP.Views;

public partial class HistorialPagosView : ContentPage
{
    HistorialPagosViewModel mp;
  
    public HistorialPagosView()
	{

		InitializeComponent();
        mp = new HistorialPagosViewModel(ServicioSession.GetCedula(),"");
        BindingContext = mp;
      
        this.Appearing += Productos_Appearing;
    }
    private async void Productos_Appearing(object sender, EventArgs e)
    {
        mp.H1 = false;mp.H2 = false;mp.H3 = false; mp.H4 = false; mp.H5 = false; mp.Hingles = false; mp.Hotro = false;
        mp.validaProductosDisponibles();

    }
    //protected override void OnAppearing()
    //{
    //    base.OnAppearing();
    //    mp = new HistorialPagosViewModel();
    //    BindingContext = mp;
    //    mp.CargarSemestres();
    //}
    private async void btn_N1_Clicked(object sender, EventArgs e)
    {
        string idButton=(sender as ImageButton).AutomationId;
        string filtroC="1";
        switch (idButton)
        {
            case "btn1":
                filtroC = "1";
                break;
            case "btn2":
                filtroC = "2";
                break;
            case "btn3":
                filtroC = "3";
                break;
            case "btn4":
                filtroC = "4";
                break;
            case "btn5":
                filtroC = "5";
                break;
            case "btnIng":
                filtroC = "INGLES";
                break;
            case "btnOtros":
                filtroC = "OTROS";
                break;

        }
        string identificacionBusqueda = String.IsNullOrEmpty(idBusqueda.Text) ? ServicioSession.GetCedula() : idBusqueda.Text;
        var page = new HistorialPagosDetalleView(identificacionBusqueda, filtroC);
        NavigationPage.SetHasNavigationBar(page, false);
        await Navigation.PushAsync(page);

    }

    private async void btnBuscar_Clicked(object sender, EventArgs e)
    {
        mp.H1 = false; mp.H2 = false; mp.H3 = false; mp.H4 = false; mp.H5 = false; mp.Hingles = false; mp.Hotro = false;
        mp.Busqueda = idBusqueda.Text;
        mp.validaProductosDisponibles();
    }
    private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchBar = sender as SearchBar;

        if (string.IsNullOrEmpty(e.NewTextValue))
            return;

        // Filtrar caracteres que no sean números
        string filteredText = new string(e.NewTextValue.Where(char.IsDigit).ToArray());

        if (e.NewTextValue != filteredText)
        {
            searchBar.Text = filteredText; // Corrige el texto del SearchBar
        }
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        if (mp.MostrarElementos)
        {
            UserDialogs.Instance.Alert("Para consultar pagos, ingrese una cédula.", "Informativo", "Cerrar", "@drawable/info.png");
        }
        else
        {
            UserDialogs.Instance.Alert("Haga clic en el tipo de pago para ver los detalles.", "Informativo", "Cerrar", "@drawable/info.png");
        }
        
    }
}