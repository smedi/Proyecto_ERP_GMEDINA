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
    
    public partial class tbAdelantoSueldo
    {
        public int adsu_IdAdelantoSueldo { get; set; }
        public int emp_Id { get; set; }
        public System.DateTime adsu_FechaAdelanto { get; set; }
        public string adsu_RazonAdelanto { get; set; }
        public Nullable<decimal> adsu_Monto { get; set; }
        public int peri_IdPeriodo { get; set; }
        public int cde_IdDeducciones { get; set; }
        public bool adsu_Pagado { get; set; }
        public int adsu_UsuarioCrea { get; set; }
        public System.DateTime adsu_FechaCrea { get; set; }
        public Nullable<int> adsu_UsuarioModifica { get; set; }
        public Nullable<System.DateTime> adsu_FechaModifica { get; set; }
        public bool adsu_Activo { get; set; }
    
        public virtual tbUsuario tbUsuario { get; set; }
        public virtual tbUsuario tbUsuario1 { get; set; }
        public virtual tbPeriodos tbPeriodos { get; set; }
        public virtual tbCatalogoDeDeducciones tbCatalogoDeDeducciones { get; set; }
        public virtual tbEmpleados tbEmpleados { get; set; }
    }
}
