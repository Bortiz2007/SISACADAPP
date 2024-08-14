using Controls.UserDialogs.Maui;

using SISACAD_APP.Servicios;
using SISACAD_APP.ViewModel;

namespace SISACAD_APP.Views;

public partial class TranferenciasyDepositosView : ContentPage
{

    TranferenciasyDepositosViewModel mp;
    EstudianteViewModel ep;
    public TranferenciasyDepositosView()
    {
        InitializeComponent();
        mp = new TranferenciasyDepositosViewModel(ServicioSession.GetCedula());
        //ep = new EstudianteViewModel(ServicioSession.GetCedula());
        BindingContext = mp;
        //BindingContext = ep;
        idBusqueda.Text = string.Empty;
        this.Appearing += ConciliacionesAutomaticas_Appearing;
    }

    private async void ConciliacionesAutomaticas_Appearing(object sender, EventArgs e)
    {
        CargaInformacion();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        try
        {

            if (mp.CarritodeCompras.Count > 0)
            {
                string identificacionBusqueda = String.IsNullOrEmpty(idBusqueda.Text) ? ServicioSession.GetCedula() : idBusqueda.Text;
                var page = new TransferenciaDetalleView(identificacionBusqueda, mp.CarritodeCompras, mp.Total, "");
                NavigationPage.SetHasNavigationBar(page, false);
                await Navigation.PushAsync(page);

            }
            else
            {
            
                UserDialogs.Instance.Alert("Estimado usuario debe al menos seleccionar un ítem para continuar", "Error", "Aceptar", "@drawable/error_login.png");

            }

        }
        catch (Exception ex)
        {
       
            UserDialogs.Instance.Alert("Estimado usuario debe al menos seleccionar un ítem para continuar", "Error", "Aceptar", "@drawable/error_login.png");

        }
    }

    private void chkCarrito_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkBox = sender as CheckBox;
        var item = checkBox.BindingContext; // Obtén el elemento vinculado al CheckBox

        if (e.Value) // Si el CheckBox está marcado
        {
            // Llamar al comando AgregarCarrito
            var command = mp.AgregarCarrito;
            if (command.CanExecute(item))
            {
                command.Execute(item);
            }
        }
        else // Si el CheckBox está desmarcado
        {
            // Llamar al comando QuitarCarrito
            var command = mp.QuitarCarrito;
            if (command.CanExecute(item))
            {
                command.Execute(item);
            }
        }
    }

    private void chkCarritoI_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {

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
        CargaInformacion();

    }
    protected async Task CargaInformacion()
    {
        try
        {
            base.OnAppearing();

            mp.Total = 0;
            mp.CarritodeCompras = new List<Entidades.PagosDetalleE>();
            
            mp.Busqueda = String.IsNullOrEmpty(idBusqueda.Text) ? ServicioSession.GetCedula() : idBusqueda.Text;
            ep = new EstudianteViewModel(mp.Busqueda);
            ep.Busqueda = mp.Busqueda;
            string carrera = "NoCarrera";
            if (ServicioSession.GetIdRol() == "4" || ServicioSession.GetIdRol() == "3" || !String.IsNullOrEmpty(idBusqueda.Text))
            {
                //CarreraPicker.SelectedItem = null;
                //CarreraPicker.SelectedIndex = -1;
                //CarreraPicker.ItemsSource = null;
                await mp.CargarCarrera();

                //// Reestablece el Binding si es necesario
                //CarreraPicker.SetBinding(Picker.ItemsSourceProperty, new Binding("DatosEstudiante"));
                //CarreraPicker.SetBinding(Picker.SelectedItemProperty, new Binding("SelectedCarrera"));

                //if (CarreraPicker.ItemsSource != null && CarreraPicker.ItemsSource.Cast<object>().Any())
                //{
                //    CarreraPicker.SelectedIndex = 1;
                //    carrera = CarreraPicker.Items[CarreraPicker.SelectedIndex].ToString();
                //    String[] cod_carrera = carrera.Split('|');
                //    carrera = cod_carrera[0].ToString().Trim();
                //}


            }

            mp.Carrera = String.IsNullOrEmpty(lblCodCarrera.Text) ? carrera : lblCodCarrera.Text;
            await mp.MostrarProductos();
        }
        catch (Exception ex)
        {

        }
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        UserDialogs.Instance.Alert("Para consultar pagos, ingrese una cédula.", "Informativo", "Cerrar", "@drawable/info.png");
    }
}