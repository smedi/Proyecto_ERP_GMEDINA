//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERP_GMEDINA.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbNacionalidades
    {
        public int nac_Id { get; set; }
        public string nac_Descripcion { get; set; }
        public bool nac_Estado { get; set; }
        public string nac_RazonInactivo { get; set; }
        public int nac_UsuarioCrea { get; set; }
        public System.DateTime nac_FechaCrea { get; set; }
        public Nullable<int> nac_UsuarioModifica { get; set; }
        public Nullable<System.DateTime> nac_FechaModifica { get; set; }
    
        public virtual tbUsuario tbUsuario { get; set; }
        public virtual tbUsuario tbUsuario1 { get; set; }
    }
}
