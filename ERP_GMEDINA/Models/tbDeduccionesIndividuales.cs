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
    
    public partial class tbDeduccionesIndividuales
    {
        public int dei_IdDeduccionesIndividuales { get; set; }
        public string dei_Motivo { get; set; }
        public int emp_Id { get; set; }
        public Nullable<decimal> dei_MontoInicial { get; set; }
        public Nullable<decimal> dei_MontoRestante { get; set; }
        public Nullable<decimal> dei_Cuota { get; set; }
        public int dei_UsuarioCrea { get; set; }
        public System.DateTime dei_FechaCrea { get; set; }
        public Nullable<int> dei_UsuarioModifica { get; set; }
        public Nullable<System.DateTime> dei_FechaModifica { get; set; }
        public bool dei_Activo { get; set; }
    
        public virtual tbUsuario tbUsuario { get; set; }
        public virtual tbUsuario tbUsuario1 { get; set; }
        public virtual tbEmpleados tbEmpleados { get; set; }
    }
}
