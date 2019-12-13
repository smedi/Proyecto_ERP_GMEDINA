﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ERP_GMEDINAEntities : DbContext
    {
        public ERP_GMEDINAEntities()
            : base("name=ERP_GMEDINAEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbUsuario> tbUsuario { get; set; }
        public virtual DbSet<tbFaseSeleccion> tbFaseSeleccion { get; set; }
        public virtual DbSet<tbFasesReclutamiento> tbFasesReclutamiento { get; set; }
        public virtual DbSet<tbRequisiciones> tbRequisiciones { get; set; }
        public virtual DbSet<tbSeleccionCandidatos> tbSeleccionCandidatos { get; set; }
        public virtual DbSet<V_SeleccionCandidatos> V_SeleccionCandidatos { get; set; }
        public virtual DbSet<tbPersonas> tbPersonas { get; set; }
        public virtual DbSet<tbHistorialAmonestaciones> tbHistorialAmonestaciones { get; set; }
        public virtual DbSet<tbSueldos> tbSueldos { get; set; }
    
        public virtual ObjectResult<UDP_RRHH_tbSeleccionCandidatos_Delete_Result> UDP_RRHH_tbSeleccionCandidatos_Delete(Nullable<int> scan_Id, string scan_RazonInactivo, Nullable<int> scan_UsuarioModifica, Nullable<System.DateTime> scan_FechaModifica)
        {
            var scan_IdParameter = scan_Id.HasValue ?
                new ObjectParameter("scan_Id", scan_Id) :
                new ObjectParameter("scan_Id", typeof(int));
    
            var scan_RazonInactivoParameter = scan_RazonInactivo != null ?
                new ObjectParameter("scan_RazonInactivo", scan_RazonInactivo) :
                new ObjectParameter("scan_RazonInactivo", typeof(string));
    
            var scan_UsuarioModificaParameter = scan_UsuarioModifica.HasValue ?
                new ObjectParameter("scan_UsuarioModifica", scan_UsuarioModifica) :
                new ObjectParameter("scan_UsuarioModifica", typeof(int));
    
            var scan_FechaModificaParameter = scan_FechaModifica.HasValue ?
                new ObjectParameter("scan_FechaModifica", scan_FechaModifica) :
                new ObjectParameter("scan_FechaModifica", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<UDP_RRHH_tbSeleccionCandidatos_Delete_Result>("UDP_RRHH_tbSeleccionCandidatos_Delete", scan_IdParameter, scan_RazonInactivoParameter, scan_UsuarioModificaParameter, scan_FechaModificaParameter);
        }
    
        public virtual ObjectResult<UDP_RRHH_tbSeleccionCandidatos_Insert_Result> UDP_RRHH_tbSeleccionCandidatos_Insert(Nullable<int> per_Id, Nullable<int> fare_Id, Nullable<System.DateTime> scan_Fecha, Nullable<int> rper_Id, Nullable<int> scan_UsuarioCrea, Nullable<System.DateTime> scan_FechaCrea)
        {
            var per_IdParameter = per_Id.HasValue ?
                new ObjectParameter("per_Id", per_Id) :
                new ObjectParameter("per_Id", typeof(int));
    
            var fare_IdParameter = fare_Id.HasValue ?
                new ObjectParameter("fare_Id", fare_Id) :
                new ObjectParameter("fare_Id", typeof(int));
    
            var scan_FechaParameter = scan_Fecha.HasValue ?
                new ObjectParameter("scan_Fecha", scan_Fecha) :
                new ObjectParameter("scan_Fecha", typeof(System.DateTime));
    
            var rper_IdParameter = rper_Id.HasValue ?
                new ObjectParameter("rper_Id", rper_Id) :
                new ObjectParameter("rper_Id", typeof(int));
    
            var scan_UsuarioCreaParameter = scan_UsuarioCrea.HasValue ?
                new ObjectParameter("scan_UsuarioCrea", scan_UsuarioCrea) :
                new ObjectParameter("scan_UsuarioCrea", typeof(int));
    
            var scan_FechaCreaParameter = scan_FechaCrea.HasValue ?
                new ObjectParameter("scan_FechaCrea", scan_FechaCrea) :
                new ObjectParameter("scan_FechaCrea", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<UDP_RRHH_tbSeleccionCandidatos_Insert_Result>("UDP_RRHH_tbSeleccionCandidatos_Insert", per_IdParameter, fare_IdParameter, scan_FechaParameter, rper_IdParameter, scan_UsuarioCreaParameter, scan_FechaCreaParameter);
        }
    
        public virtual ObjectResult<UDP_RRHH_tbSeleccionCandidatos_Update_Result> UDP_RRHH_tbSeleccionCandidatos_Update(Nullable<int> scan_Id, Nullable<int> per_Id, Nullable<int> fare_Id, Nullable<System.DateTime> scan_Fecha, Nullable<int> rper_Id, Nullable<int> scan_UsuarioModifica, Nullable<System.DateTime> scan_FechaModifica)
        {
            var scan_IdParameter = scan_Id.HasValue ?
                new ObjectParameter("scan_Id", scan_Id) :
                new ObjectParameter("scan_Id", typeof(int));
    
            var per_IdParameter = per_Id.HasValue ?
                new ObjectParameter("per_Id", per_Id) :
                new ObjectParameter("per_Id", typeof(int));
    
            var fare_IdParameter = fare_Id.HasValue ?
                new ObjectParameter("fare_Id", fare_Id) :
                new ObjectParameter("fare_Id", typeof(int));
    
            var scan_FechaParameter = scan_Fecha.HasValue ?
                new ObjectParameter("scan_Fecha", scan_Fecha) :
                new ObjectParameter("scan_Fecha", typeof(System.DateTime));
    
            var rper_IdParameter = rper_Id.HasValue ?
                new ObjectParameter("rper_Id", rper_Id) :
                new ObjectParameter("rper_Id", typeof(int));
    
            var scan_UsuarioModificaParameter = scan_UsuarioModifica.HasValue ?
                new ObjectParameter("scan_UsuarioModifica", scan_UsuarioModifica) :
                new ObjectParameter("scan_UsuarioModifica", typeof(int));
    
            var scan_FechaModificaParameter = scan_FechaModifica.HasValue ?
                new ObjectParameter("scan_FechaModifica", scan_FechaModifica) :
                new ObjectParameter("scan_FechaModifica", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<UDP_RRHH_tbSeleccionCandidatos_Update_Result>("UDP_RRHH_tbSeleccionCandidatos_Update", scan_IdParameter, per_IdParameter, fare_IdParameter, scan_FechaParameter, rper_IdParameter, scan_UsuarioModificaParameter, scan_FechaModificaParameter);
        }
    }
}
