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
    
    public partial class tbCompetencias
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbCompetencias()
        {
            this.tbCompetenciasRequisicion = new HashSet<tbCompetenciasRequisicion>();
        }
    
        public int comp_Id { get; set; }
        public string comp_Descripcion { get; set; }
        public bool comp_Estado { get; set; }
        public string comp_RazonInactivo { get; set; }
        public int comp_UsuarioCrea { get; set; }
        public System.DateTime comp_FechaCrea { get; set; }
        public Nullable<int> comp_UsuarioModifica { get; set; }
        public Nullable<System.DateTime> comp_FechaModifica { get; set; }
    
        public virtual tbUsuario tbUsuario { get; set; }
        public virtual tbUsuario tbUsuario1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbCompetenciasRequisicion> tbCompetenciasRequisicion { get; set; }
    }
}