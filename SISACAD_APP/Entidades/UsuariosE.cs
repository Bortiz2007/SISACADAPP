using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISACAD_APP.Entidades
{
    class UsuariosE
    {
        public string? Cod_usu { get; set; }
        public string? Identificacion { get; set; }
        public int Id_rol { get; set; }
        public string? Nombre { get; set; }
        public bool Cambio_clave { get; set; }
        public DateTime Fecha_cambio_clave { get; set; }
        public bool Estado { get; set; }
    }
}
