using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SISACAD_APP.Views;
using Microcharts.Maui;
using SISACAD_APP.ViewModel;
using Controls.UserDialogs.Maui;
using Plugin.LocalNotification;

namespace SISACAD_APP
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMicrocharts()
                .UseLocalNotification()
                .UseUserDialogs(() =>
                {
                    //setup your default styles for dialogs
                    AlertConfig.DefaultBackgroundColor = Color.FromArgb("#E7F5FF"); ;
                    AlertConfig.DefaultTitleColor = Color.FromArgb("#1971C2");
                   AlertConfig.DefaultMessageColor = Color.FromArgb("#243B55");
                    AlertConfig.DefaultPositiveButtonTextColor = Color.FromArgb("#1D976C");
#if ANDROID
                  AlertConfig.DefaultMessageFontFamily = "OpenSans-Regular.ttf";
#else
                    AlertConfig.DefaultMessageFontFamily = "OpenSans-Regular";
                #endif

                    ToastConfig.DefaultCornerRadius = 15;
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("FontAwesomeSolid.otf", "AwesomeSolid");
                });

#if DEBUG
            builder.Logging.AddDebug();
            builder.Services.AddSingleton<IMediaPicker>(MediaPicker.Default);
            builder.Services.AddTransient<TransferenciaDetalleView>();
            builder.Services.AddTransient<LoginViewModel>();
            
#endif

            return builder.Build();
        }
    }
}
