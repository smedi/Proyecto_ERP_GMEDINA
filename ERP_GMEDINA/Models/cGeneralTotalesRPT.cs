using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP_GMEDINA.Models
{

    [MetadataType(typeof(cGeneralTotalesRPT))]
    public partial class V_GeneralTotales_RPT
    {

    }

    public class cGeneralTotalesRPT
    {

        public System.DateTime hipa_FechaPago { get; set; }

        public int cpla_IdPlanilla { get; set; }

        public string cpla_DescripcionPlanilla { get; set; }

        public Nullable<decimal> cde_TotalISR { get; set; }

        public Nullable<decimal> cde_TotalAFP { get; set; }

        public Nullable<decimal> cde_TotalIHSS { get; set; }

        public Nullable<decimal> cde_TotalRAP { get; set; }

        public Nullable<decimal> cde_TotalINFOP { get; set; }

        public Nullable<decimal> cde_OtrasDeducciones { get; set; }

    }
}