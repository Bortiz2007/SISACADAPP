using Controls.UserDialogs.Maui;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using SISACAD_APP.Servicios;
using SISACAD_APP.ViewModel;

namespace SISACAD_APP.Views;

public partial class HistorialPagosDetalleView : ContentPage
{
    HistorialPagosViewModel mp;
    
    public HistorialPagosDetalleView(string identificiacionC,string filtro)
	{
       
        InitializeComponent();

       
        mp = new HistorialPagosViewModel(identificiacionC, filtro);
        mp.Busqueda = identificiacionC;
        BindingContext = mp;

        this.Appearing += Productos_Appearing;
       
    }
    private async void Productos_Appearing(object sender, EventArgs e)
    {
       
        //string identificacion = ServicioSession.GetCedula();
        await mp.MostrarProductos();
        if(mp.PagosDetalle.Count == 0)
        {
            UserDialogs.Instance.Alert("Estimado usuario, no hay datos disponibles que coincidan con el filtro aplicado.", "Informativo", "Aceptar", "@drawable/info.png");
        }
        // Ocultar el ActivityIndicator

    }

    private async void btnBuscar_Clicked(object sender, EventArgs e)
    {
        var loading = UserDialogs.Instance.Loading("Cargando...");
        await Task.Delay(1000);
        ClassesCollectionView.ItemsSource = mp.MostrarProductosFiltros();
        if (mp.PagosDetalle.Count == 0)
        {
            UserDialogs.Instance.Alert("Estimado usuario, no hay datos disponibles que coincidan con el filtro aplicado.", "Informativo", "Aceptar", "@drawable/info.png");
        }
        await Task.Delay(1000);
        loading.Dispose();

    }
}