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
    
    public partial class tbTipoPlanillaDetalleDeduccion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbTipoPlanillaDetalleDeduccion()
        {
            this.tbHistorialDeduccionPago = new HashSet<tbHistorialDeduccionPago>();
        }
    
        public int tpdd_IdPlanillaDetDeduccion { get; set; }
        public int cpla_IdPlanilla { get; set; }
        public int cde_IdDeducciones { get; set; }
        public int tpdd_UsuarioCrea { get; set; }
        public System.DateTime tpdd_FechaCrea { get; set; }
        public Nullable<int> tpdd_UsuarioModifica { get; set; }
        public Nullable<System.DateTime> tpdd_FechaModifica { get; set; }
        public bool tpdd_Activo { get; set; }
    
        public virtual tbUsuario tbUsuario { get; set; }
        public virtual tbUsuario tbUsuario1 { get; set; }
        public virtual tbCatalogoDeDeducciones tbCatalogoDeDeducciones { get; set; }
        public virtual tbCatalogoDePlanillas tbCatalogoDePlanillas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbHistorialDeduccionPago> tbHistorialDeduccionPago { get; set; }
    }
}