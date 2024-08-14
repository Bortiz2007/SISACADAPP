
using Controls.UserDialogs.Maui;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SISACAD_APP.Entidades;
using SISACAD_APP.ViewModel;

namespace SISACAD_APP.Views;

public partial class TransferenciaDetalleView : ContentPage
{
    
    TransferenciaDetalleViewModel mp;
    byte[] imgbit;
    String fecha = DateTime.Today.Date.ToString(), cod_usu;
    public  TransferenciaDetalleView(string cedula, List<PagosDetalleE> CarritoCompras, decimal total, string codUsuario)
	{
        InitializeComponent();
        mp = new TransferenciaDetalleViewModel(cedula, CarritoCompras, total);
        BindingContext = mp;
        foto.Clicked += foto_Clicked;
        mp.Total = total;
        cod_usu = codUsuario;
        this.Appearing += CargaInformacion__Appearing;
    }

    protected async void CargaInformacion__Appearing(object sender, EventArgs e)
    {
        var loading = UserDialogs.Instance.Loading("Cargando...");
        fechaInfo.Date = DateTime.Today.Date;
        fechaInfo.MaximumDate = DateTime.Today.AddDays(1).Date;
        await  Task.Delay(2000);
        loading.Dispose();
    }
    private async void foto_Clicked(object sender, EventArgs e)
    {
        try { 
        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
        {
            await DisplayAlert("Error", "La camara no se encuentra disponible.", "OK");
            return;
        }
        else
        {
            FileResult photo = await MediaPicker.CapturePhotoAsync();
            if (photo != null)
            {
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    // Guarda la foto en el almacenamiento local
                    using (Stream source = await photo.OpenReadAsync())
                    using (FileStream localFile = File.OpenWrite(localFilePath))
                    {
                        await source.CopyToAsync(localFile);
                    }

                    // Asegúrate de que el archivo está cerrado antes de leerlo
                    await Task.Delay(500); // Opcional: Pequeña espera para asegurar que el archivo se ha liberado

                    // Lee los bytes del archivo local
                    
                    using (FileStream fileStream = File.OpenRead(localFilePath))
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        await fileStream.CopyToAsync(memoryStream);
                        imgbit = memoryStream.ToArray();
                    }
                    Photo.Source = ImageSource.FromStream(() => new MemoryStream(imgbit));

                }
            }
        }catch(Exception ex)
        {
            //await DisplayAlert("Error", $"Ocurrió un error al capturar la foto: {ex.Message}", "OK");
            UserDialogs.Instance.Alert($"Ocurrió un error al capturar la foto: {ex.Message}", "Error", "Aceptar", "@drawable/error_login.png");
        }


    }

    private void fechaInfo_DateSelected(object sender, DateChangedEventArgs e)
    {
        fecha = e.NewDate.ToString("d");
    }

    private async void archivo_Clicked(object sender, EventArgs e)
    {
        try
        {

            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
            else
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                   // await DisplayAlert("Error", "La camara no se encuentra disponible.", "OK");
                    UserDialogs.Instance.Alert("La camara no se encuentra disponible.", "Error", "Aceptar", "@drawable/error_login.png");
                    return;
                }
                var archi = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = PhotoSize.Custom,
                    CustomPhotoSize = 45
                });
                if (archi != null)
                {

                    Photo.Source = archi.Path;
                    imgbit = System.IO.File.ReadAllBytes(archi.Path);
                }
            }
            if (status != PermissionStatus.Granted)
            {
                return;
            }


        }
        catch (Exception ex)
        {
          
            UserDialogs.Instance.Alert("Accesos denegados", "Error", "Aceptar", "@drawable/error_login.png");
            return;
        }


    }

    private async void btn_guardar_Clicked(object sender, EventArgs e)
    {
        if (pickerCuenta.SelectedIndex == -1)
        {
         
            UserDialogs.Instance.Alert("Estimado Usuario debe seleccionar una opcion para continuar.", "Error", "Aceptar", "@drawable/error_login.png");
            pickerCuenta.Focus();
        }
        else if (pickerBanco.SelectedIndex == -1)
        {
            UserDialogs.Instance.Alert("Estimado Usuario debe seleccionar una opcion para continuar.", "Error", "Aceptar", "@drawable/error_login.png");
            pickerBanco.Focus();
        }
        //else if (ValorTrans.Text.Length == 0 || Convert.ToDecimal(ValorTrans.Text.ToString().Trim()) != mp.Total)
        //{
        //    await DisplayAlert("Error", "Estimado Usuario el campo no puede se vacio ni exceder al valor total de la transacción", "Ok");
        //    ValorTrans.Focus();
        //}
        else if (numRef.ToString().Length == 0)
        {
            
            UserDialogs.Instance.Alert("Estimado Usuario debe seleccionar una opcion para continuar.", "Error", "Aceptar", "@drawable/error_login.png");
            numRef.Focus();
        }
        else if (imgbit.Length == 0)
        {

            UserDialogs.Instance.Alert("Estimado Usuario debe subir la imagen del deposito para continuar.", "Error", "Aceptar", "@drawable/error_login.png");
            numRef.Focus();
        }
        else
        {
            string cuenta = String.Empty;
            string strCodigoBancoPichincha = pickerBanco.SelectedItem.ToString() == "PICHINCHA" ? numRef.Text : "";
            if (pickerCuenta.SelectedItem.ToString() == "Pichincha")
            {
                cuenta = "2100145316";
            }
            else
            {
                cuenta = "019037810293";
            }

          
            mp.InsertarDeposito(pickerCuenta.SelectedItem.ToString(), pickerBanco.SelectedItem.ToString(), cuenta,
               fecha, imgbit, Convert.ToDecimal(ValorTrans.Text.ToString()), cod_usu, "", strCodigoBancoPichincha
                );
        }


    }
}


