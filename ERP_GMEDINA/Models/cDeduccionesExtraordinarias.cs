using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP_GMEDINA.Models
{

    [MetadataType(typeof(cDeduccionesExtraordinarias))]
    public partial class tbDeduccionesExtraordinarias
    {
    }

    public class cDeduccionesExtraordinarias
    {
        [Display(Name = "Id Deducciones Extraordinarias")]
        public int dex_IdDeduccionesExtra { get; set; }

        [Display(Name = "Monto Inicial")]
        public decimal dex_MontoInicial { get; set; }

        [Display(Name = "Monto Restante")]
        public decimal dex_MontoRestante { get; set; }

        [Display(Name = "Observaciones")]
        public string dex_ObservacionesComentarios { get; set; }

        [Display(Name = "Id Deducción")]
        public int cde_IdDeducciones { get; set; }

        [Display(Name = "Cuota")]
        public decimal dex_Cuota { get; set; }

        [Display(Name = "Creado por")]
        public int dex_UsuarioCrea { get; set; }

        [Display(Name = "Fecha de Creacion")]
        public System.DateTime dex_FechaCrea { get; set; }

        [Display(Name = "Modificado por")]
        public Nullable<int> dex_UsuarioModifica { get; set; }

        [Display(Name = "Fecha Modificacion")]
        public Nullable<System.DateTime> dex_FechaModifica { get; set; }

        [Display(Name = "Activo")]
        public bool dex_Activo { get; set; }
    }
}