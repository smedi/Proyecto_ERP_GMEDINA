//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERP_GMEDINA.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbPrestaciones
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbPrestaciones()
        {
            this.tbHistorialSalidas = new HashSet<tbHistorialSalidas>();
        }
    
        public int pres_Id { get; set; }
        public bool pres_DerechoPreaviso { get; set; }
        public decimal pres_Preaviso { get; set; }
        public decimal pres_DecimoTercer { get; set; }
        public decimal pres_Catorceavo { get; set; }
        public bool pres_Estado { get; set; }
        public string pres_RazonInactivo { get; set; }
        public int pres_UsuarioCrea { get; set; }
        public System.DateTime pres_FechaCrea { get; set; }
        public Nullable<int> pres_UsuarioModifica { get; set; }
        public Nullable<System.DateTime> pres_FechaModifica { get; set; }
    
        public virtual tbUsuario tbUsuario { get; set; }
        public virtual tbUsuario tbUsuario1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbHistorialSalidas> tbHistorialSalidas { get; set; }
    }
}
