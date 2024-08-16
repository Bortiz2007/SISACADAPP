using Controls.UserDialogs.Maui;
using Microcharts;
using Microcharts.Maui;
using SISACAD_APP.Model;
using SISACAD_APP.Servicios;
using SkiaSharp;
using System.Collections;


namespace SISACAD_APP.Views;

public partial class MenuPrincipal : ContentPage
{
    MenuPrincipalViewModel mp;


    public MenuPrincipal()
    {

        InitializeComponent();
        mp = new MenuPrincipalViewModel(ServicioSession.GetCedula());
        fechaInicio.MaximumDate = DateTime.Today.Date;
        if(ServicioSession.GetIdRol() == "4" || ServicioSession.GetIdRol() == "3")
        {
            fechaFin.MaximumDate = DateTime.Today.Date;
        }
        else
        {
            fechaFin.MaximumDate = DateTime.Today.Date.AddDays(1);
        }
       
        //lblHorarios.Text = ServicioSession.GetCedula();
        BindingContext = mp;

        this.Appearing += Menu_Appearing;
        

    }
    
    private async void Menu_Appearing(object sender, EventArgs e)
    {
        CargarInformacion();

    }
    private void fechaInicio_DateSelected(object sender, DateChangedEventArgs e)
    {
        mp.Fecha_inicio = e.NewDate;
    }

    private void fechaFin_DateSelected(object sender, DateChangedEventArgs e)
    {
        mp.Fecha_fin = e.NewDate;
    }
    private void btnAnterior_Clicked(object sender, EventArgs e)
    {
        if (carousel.Position > 0)
        {
            // Si no es el primer elemento, retrocede uno
            carousel.Position -= 1;
        }
        else
        {
            // Si es el primer elemento, ve al último
            carousel.Position = ((IList)carousel.ItemsSource).Count - 1;
        }
    }

    private void Siguiente_Clicked(object sender, EventArgs e)
    {
        if (carousel.Position < ((IList)carousel.ItemsSource).Count - 1)
        {
            // Si no es el último elemento, avanza uno
            carousel.Position += 1;
        }
        else
        {
            // Si es el último elemento, vuelve al primero
            carousel.Position = 0;
        }
    }
    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        UserDialogs.Instance.Alert("Para una consulta específica, ingrese una cédula y seleccione un rango de fechas.", "Informativo", "Cerrar", "@drawable/info.png");
    }

    protected async void CargarInformacion()
    {
        int bandera = 0;
        await mp.ObtieneInformacionPanel();
               
        if (mp.PanelPagos.Count > 0)
        {
           
            bandera++;
            chName1.Chart = new DonutChart
            {
                Entries = mp.PanelPagos,
                BackgroundColor = SKColor.Parse("#E9E4F0"),
                LabelTextSize = 16
            };
           
        }
       
        if (mp.PanelProductos.Count > 0)
        {
           
            bandera++;
            chName2.Chart = new LineChart
            {
                Entries = mp.PanelProductos,
                LineMode = LineMode.Straight,
                LineSize = 8,
                PointMode = PointMode.Square,
                PointSize = 18,
                BackgroundColor = SKColor.Parse("#E9E4F0"),
                LabelTextSize = 16
            };
        }
     
        if (mp.PanelDepositos.Count > 0)
        {
            
            bandera++;
            chName.Chart = new RadialGaugeChart { Entries = mp.PanelDepositos, LabelTextSize = 16, BackgroundColor = SKColor.Parse("#E9E4F0"), IsAnimated = true };
        }
       

        if(bandera ==0)
        {
            UserDialogs.Instance.Alert("Estimado usuario no existe al momento información para mostrar", "Informativo", "Aceptar", "@drawable/info.png");
        }
        
       
       
       
    }
    private async void btnBuscar_Clicked(object sender, EventArgs e)
    {
        if(mp.Fecha_inicio > mp.Fecha_fin)
        {
            UserDialogs.Instance.Alert("Estimado usuario la fecha inicial no puede ser mayor a la de fin", "Informativo", "Aceptar", "@drawable/info.png");
        }
        else if(mp.Fecha_fin < mp.Fecha_inicio)
        {
            UserDialogs.Instance.Alert("Estimado usuario la fecha fin no puede ser menor a la de inicio", "Informativo", "Aceptar", "@drawable/info.png");
        }
        else
        {
            mp.Busqueda = idBusqueda.Text;
            CargarInformacion();
        }
      
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

}