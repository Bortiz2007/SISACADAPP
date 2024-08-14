using SISACAD_APP.Views;

namespace SISACAD_APP
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
           
            MainPage = new LoginView();
        }
    }
}
