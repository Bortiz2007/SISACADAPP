using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISACAD_APP.Servicios
{
    public static class ServicioSession
    {
        private const string UserKey = "UserKey";
        private const string CedulaKey = "CedulaKey";
        private const string IdRolKey = "IdRolKey";
        private static bool CambioClave = false;
        private const string KeyApi = "https://api.itsqmet.edu.ec/Api/SISACAD?";

        public static void SaveUser(string username)
        {
            Preferences.Set(UserKey, username);
        }

        public static string GetKeyAPI()
        {
            return KeyApi;
        }

        public static string GetUser()
        {
            return Preferences.Get(UserKey, string.Empty);
        }

        public static void GuardarDatos(string cedula, string idRol, bool cambioClave)
        {
            Preferences.Set(CedulaKey, cedula);
            Preferences.Set(IdRolKey, idRol);
            CambioClave = cambioClave;
        }

        public static string GetCedula()
        {
            return Preferences.Get(CedulaKey, string.Empty);
        }

        public static string GetIdRol()
        {
            return Preferences.Get(IdRolKey, string.Empty);
        }

        public static bool GetCambioClave()
        {
            return CambioClave;
        }

        public static void ClearSession()
        {
            Preferences.Remove(UserKey);
            Preferences.Remove(CedulaKey);
            Preferences.Remove(IdRolKey);
            CambioClave = false;
        }
    }
}