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
    
    public partial class V_IHSS_RPT
    {
        public int emp_Id { get; set; }
        public string per_Identidad { get; set; }
        public string NombreCompleto { get; set; }
        public string depto_descripcion { get; set; }
        public string area_Descripcion { get; set; }
        public int cpla_IdPlanilla { get; set; }
        public string cpla_DescripcionPlanilla { get; set; }
        public int cde_IdDeducciones { get; set; }
        public string cde_DescripcionDeduccion { get; set; }
        public Nullable<decimal> hidp_Total { get; set; }
        public System.DateTime hipa_FechaPago { get; set; }
    }
}
