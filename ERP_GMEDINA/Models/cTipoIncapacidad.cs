﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP_GMEDINA.Models
{
    [MetadataType(typeof(cTipoIncapacidad))]

    public partial class tbTipoIncapacidades
    {

    }
    public class cTipoIncapacidad
    {
        [Display(Name = "Tipo Incapacidad")]
        public int ticn_Id { get; set; }

        [Display(Name = "Tipo Incapacidad")]
        public string ticn_Descripcion { get; set; }

        [Display(Name = "Estado")]
        public bool ticn_Estado { get; set; }

        [Display(Name = "Razon Inactivo")]
        public string ticn_RazonInactivo { get; set; }

        [Display(Name = "Usuario Crea")]
        public int ticn_UsuarioCrea { get; set; }

        [Display(Name = "Fecha Crea")]
        public System.DateTime ticn_FechaCrea { get; set; }

        [Display(Name = "Usuario Modifica")]
        public Nullable<int> ticn_UsuarioModifica { get; set; }

        [Display(Name = "Fecha Modifica")]
        public Nullable<System.DateTime> ticn_FechaModifica { get; set; }

    }
}