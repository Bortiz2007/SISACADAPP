using Controls.UserDialogs.Maui;
using SISACAD_APP.Servicios;
using SISACAD_APP.ViewModel;

namespace SISACAD_APP.Views;

public partial class ListaDepositosView : ContentPage
{
    ListaDepositosViewModel  mp;
	public ListaDepositosView()
	{
		InitializeComponent();
        mp = new ListaDepositosViewModel(ServicioSession.GetCedula());
        BindingContext = mp;
        this.Appearing += Depositos_Appearing;
    }
    private async void Depositos_Appearing(object sender, EventArgs e)
    {

        mp.Busqueda = ServicioSession.GetCedula();
        mp.CargarEstadoDepositos();
        await mp.BusquedaDepositosDetalle();

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
        if (mp.MostrarElementos)
        {
            UserDialogs.Instance.Alert("Ingrese una cédula y seleccione un rango de fechas para buscar. Use el filtro de estados para refinar los resultados.", "Informativo", "Aceptar", "@drawable/info.png");

        }
        else
        {
            UserDialogs.Instance.Alert("Seleccione un rango de fechas para buscar. Use el filtro de estados para refinar los resultados.", "Informativo", "Aceptar", "@drawable/info.png");

        }
    }
    private async void btnBuscar_Clicked(object sender, EventArgs e)
    {
        if (mp.Fecha_inicio > mp.Fecha_fin)
        {
            UserDialogs.Instance.Alert("Estimado usuario la fecha inicial no puede ser mayor a la de fin", "Informativo", "Aceptar", "@drawable/info.png");
        }
        else if (mp.Fecha_fin < mp.Fecha_inicio)
        {
            UserDialogs.Instance.Alert("Estimado usuario la fecha fin no puede ser menor a la de inicio", "Informativo", "Aceptar", "@drawable/info.png");
        }
        else
        {
            var loading = UserDialogs.Instance.Loading("Cargando...");
            await Task.Delay(1000);
            mp.Busqueda = String.IsNullOrEmpty(idBusqueda.Text) ? ServicioSession.GetCedula() : idBusqueda.Text;
            await mp.BusquedaDepositosDetalle();
            if (mp.DepositosDetalle.Count == 0)
            {
                UserDialogs.Instance.Alert("Estimado usuario no existe al momento información para mostrar", "Informativo", "Aceptar", "@drawable/info.png");
            }
            await Task.Delay(1000);
            loading.Dispose();
        }
    }

    private async void semestrePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var loading = UserDialogs.Instance.Loading("Cargando...");
        await Task.Delay(1000);
        CVDepositos.ItemsSource = mp.MostrarDepositosFiltros();
        if (mp.DepositosDetalle.Count == 0)
        {
            UserDialogs.Instance.Alert("Estimado usuario no existe al momento información para mostrar", "Informativo", "Aceptar", "@drawable/info.png");
        }
        await Task.Delay(1000);
        loading.Dispose();
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

    private async void Borrar_Clicked(object sender, EventArgs e)
    {
        var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
        {
            Message = "¿Está seguro de eliminar el registro?",
            Title = "Informativo",
            OkText = "Confirmar",
            CancelText = "Cancelar",
            Icon = "@drawable/info.png"
        });
        if (result)
        {
            var checkBox = sender as ImageButton;
            var item = checkBox.BindingContext;
            // Llamar al comando AgregarCarrito
            var command = mp.BorrarDepositoId;
            if (command.CanExecute(item))
            {
                command.Execute(item);
            }
            await mp.BusquedaDepositosDetalle();
        }
    }
}