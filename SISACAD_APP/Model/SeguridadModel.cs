using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISACAD_APP.ViewModel1
{
    class SeguridadModel
    {
        public static string EncriptarTexto(string texto)
        {
            try
            {
                System.Security.Cryptography.MD5 md5;
                md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

                byte[] encodedBytes = md5.ComputeHash(Encoding.Default.GetBytes(texto));
                return System.Text.RegularExpressions.Regex.Replace(BitConverter.ToString(encodedBytes).ToLower(), @"-", "");
            }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            {
                return string.Empty;
            }
        }
    }
}
