using Controls.UserDialogs.Maui;
using SISACAD_APP.Servicios;
using SISACAD_APP.ViewModel;

namespace SISACAD_APP.Views;

public partial class FacturasView : ContentPage
{
    FacturasViewModel mp;
    public FacturasView()
	{
		InitializeComponent();
        
        fechaInicio.MaximumDate = DateTime.Today.Date;
        fechaFin.MaximumDate = DateTime.Today.Date.AddDays(1);
        mp = new FacturasViewModel(ServicioSession.GetCedula());
        BindingContext = mp;
        this.Appearing += Facturas_Appearing;
    }

    private async void Facturas_Appearing(object sender, EventArgs e)
    {
      
        mp.identificacion = ServicioSession.GetCedula();
        await mp.BusquedaFacturasDetalle();

    }

    private void fechaInicio_DateSelected(object sender, DateChangedEventArgs e)
    {
        mp.Fecha_inicio = e.NewDate;
    }

    private void fechaFin_DateSelected(object sender, DateChangedEventArgs e)
    {
        mp.Fecha_fin = e.NewDate;
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        UserDialogs.Instance.Alert("Por favor, contacte a soporte técnico al +593 98 464 1470.", "Error", "Aceptar", "@drawable/error_login.png");
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
    private async void btnBuscar_Clicked(object sender, EventArgs e)
    {
        var loading = UserDialogs.Instance.Loading("Cargando...");
        await Task.Delay(1000);
        mp.Busqueda = String.IsNullOrEmpty(idBusqueda.Text) ? ServicioSession.GetCedula() : idBusqueda.Text;
        mp.BusquedaFacturasDetalle();
        if(mp.FacturasDetalle.Count> 0)
        {
            UserDialogs.Instance.Alert("Estimado usuario no existe al momento información para mostrar", "Informativo", "Aceptar", "@drawable/info.png");
        }
        await Task.Delay(1000);
        loading.Dispose();
    }
}