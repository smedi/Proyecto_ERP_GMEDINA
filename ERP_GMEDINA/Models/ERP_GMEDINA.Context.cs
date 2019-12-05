﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
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
    
        public virtual DbSet<tbAccesoRol> tbAccesoRol { get; set; }
        public virtual DbSet<tbObjeto> tbObjeto { get; set; }
        public virtual DbSet<tbRol> tbRol { get; set; }
        public virtual DbSet<tbRolesUsuario> tbRolesUsuario { get; set; }
        public virtual DbSet<tbUsuario> tbUsuario { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<tbAcumuladosISR> tbAcumuladosISR { get; set; }
        public virtual DbSet<tbAdelantoSueldo> tbAdelantoSueldo { get; set; }
        public virtual DbSet<tbAFP> tbAFP { get; set; }
        public virtual DbSet<tbAuxilioDeCesantias> tbAuxilioDeCesantias { get; set; }
        public virtual DbSet<tbCatalogoDeDeducciones> tbCatalogoDeDeducciones { get; set; }
        public virtual DbSet<tbCatalogoDeIngresos> tbCatalogoDeIngresos { get; set; }
        public virtual DbSet<tbCatalogoDePlanillas> tbCatalogoDePlanillas { get; set; }
        public virtual DbSet<tbDecimoCuartoMes> tbDecimoCuartoMes { get; set; }
        public virtual DbSet<tbDecimoTercerMes> tbDecimoTercerMes { get; set; }
        public virtual DbSet<tbDeduccionAFP> tbDeduccionAFP { get; set; }
        public virtual DbSet<tbDeduccionesExtraordinarias> tbDeduccionesExtraordinarias { get; set; }
        public virtual DbSet<tbDeduccionInstitucionFinanciera> tbDeduccionInstitucionFinanciera { get; set; }
        public virtual DbSet<tbEmpleadoBonos> tbEmpleadoBonos { get; set; }
        public virtual DbSet<tbEmpleadoComisiones> tbEmpleadoComisiones { get; set; }
        public virtual DbSet<tbFormaPago> tbFormaPago { get; set; }
        public virtual DbSet<tbHistorialDeduccionPago> tbHistorialDeduccionPago { get; set; }
        public virtual DbSet<tbHistorialDeIngresosPago> tbHistorialDeIngresosPago { get; set; }
        public virtual DbSet<tbHistorialDePago> tbHistorialDePago { get; set; }
        public virtual DbSet<tbHistorialLiquidaciones> tbHistorialLiquidaciones { get; set; }
        public virtual DbSet<tbInstitucionesFinancieras> tbInstitucionesFinancieras { get; set; }
        public virtual DbSet<tbISR> tbISR { get; set; }
        public virtual DbSet<tbLiquidaciones> tbLiquidaciones { get; set; }
        public virtual DbSet<tbLiquidacionVacaciones> tbLiquidacionVacaciones { get; set; }
        public virtual DbSet<tbMotivoLiquidaciones> tbMotivoLiquidaciones { get; set; }
        public virtual DbSet<tbPeriodos> tbPeriodos { get; set; }
        public virtual DbSet<tbPreaviso> tbPreaviso { get; set; }
        public virtual DbSet<tbRamaActividad> tbRamaActividad { get; set; }
        public virtual DbSet<tbSalarioPorHora> tbSalarioPorHora { get; set; }
        public virtual DbSet<tbTechosDeducciones> tbTechosDeducciones { get; set; }
        public virtual DbSet<tbTipoDeduccion> tbTipoDeduccion { get; set; }
        public virtual DbSet<tbTipoPlanillaDetalleDeduccion> tbTipoPlanillaDetalleDeduccion { get; set; }
        public virtual DbSet<tbTipoPlanillaDetalleIngreso> tbTipoPlanillaDetalleIngreso { get; set; }
        public virtual DbSet<tbAreas> tbAreas { get; set; }
        public virtual DbSet<tbCargos> tbCargos { get; set; }
        public virtual DbSet<tbCompetencias> tbCompetencias { get; set; }
        public virtual DbSet<tbCompetenciasPersona> tbCompetenciasPersona { get; set; }
        public virtual DbSet<tbCompetenciasRequisicion> tbCompetenciasRequisicion { get; set; }
        public virtual DbSet<tbDepartamentos> tbDepartamentos { get; set; }
        public virtual DbSet<tbEmpleados> tbEmpleados { get; set; }
        public virtual DbSet<tbEmpresas> tbEmpresas { get; set; }
        public virtual DbSet<tbEquipoEmpleados> tbEquipoEmpleados { get; set; }
        public virtual DbSet<tbEquipoTrabajo> tbEquipoTrabajo { get; set; }
        public virtual DbSet<tbFaseSeleccion> tbFaseSeleccion { get; set; }
        public virtual DbSet<tbFasesReclutamiento> tbFasesReclutamiento { get; set; }
        public virtual DbSet<tbHabilidades> tbHabilidades { get; set; }
        public virtual DbSet<tbHabilidadesPersona> tbHabilidadesPersona { get; set; }
        public virtual DbSet<tbHabilidadesRequisicion> tbHabilidadesRequisicion { get; set; }
        public virtual DbSet<tbHistorialAmonestaciones> tbHistorialAmonestaciones { get; set; }
        public virtual DbSet<tbHistorialAudienciaDescargo> tbHistorialAudienciaDescargo { get; set; }
        public virtual DbSet<tbHistorialCargos> tbHistorialCargos { get; set; }
        public virtual DbSet<tbHistorialContrataciones> tbHistorialContrataciones { get; set; }
        public virtual DbSet<tbHistorialHorasTrabajadas> tbHistorialHorasTrabajadas { get; set; }
        public virtual DbSet<tbHistorialIncapacidades> tbHistorialIncapacidades { get; set; }
        public virtual DbSet<tbHistorialPermisos> tbHistorialPermisos { get; set; }
        public virtual DbSet<tbHistorialRefrendamientos> tbHistorialRefrendamientos { get; set; }
        public virtual DbSet<tbHistorialSalidas> tbHistorialSalidas { get; set; }
        public virtual DbSet<tbHistorialVacaciones> tbHistorialVacaciones { get; set; }
        public virtual DbSet<tbHorarios> tbHorarios { get; set; }
        public virtual DbSet<tbIdiomaPersona> tbIdiomaPersona { get; set; }
        public virtual DbSet<tbIdiomas> tbIdiomas { get; set; }
        public virtual DbSet<tbIdiomasRequisicion> tbIdiomasRequisicion { get; set; }
        public virtual DbSet<tbJornadas> tbJornadas { get; set; }
        public virtual DbSet<tbNacionalidades> tbNacionalidades { get; set; }
        public virtual DbSet<tbPersonas> tbPersonas { get; set; }
        public virtual DbSet<tbPrestaciones> tbPrestaciones { get; set; }
        public virtual DbSet<tbRazonSalidas> tbRazonSalidas { get; set; }
        public virtual DbSet<tbRequerimientosEspeciales> tbRequerimientosEspeciales { get; set; }
        public virtual DbSet<tbRequerimientosEspecialesPersona> tbRequerimientosEspecialesPersona { get; set; }
        public virtual DbSet<tbRequerimientosEspecialesRequisicion> tbRequerimientosEspecialesRequisicion { get; set; }
        public virtual DbSet<tbRequisiciones> tbRequisiciones { get; set; }
        public virtual DbSet<tbSeleccionCandidatos> tbSeleccionCandidatos { get; set; }
        public virtual DbSet<tbSucursales> tbSucursales { get; set; }
        public virtual DbSet<tbSueldos> tbSueldos { get; set; }
        public virtual DbSet<tbTipoAmonestaciones> tbTipoAmonestaciones { get; set; }
        public virtual DbSet<tbTipoHoras> tbTipoHoras { get; set; }
        public virtual DbSet<tbTipoIncapacidades> tbTipoIncapacidades { get; set; }
        public virtual DbSet<tbTipoMonedas> tbTipoMonedas { get; set; }
        public virtual DbSet<tbTipoPermisos> tbTipoPermisos { get; set; }
        public virtual DbSet<tbTipoSalidas> tbTipoSalidas { get; set; }
        public virtual DbSet<tbTitulos> tbTitulos { get; set; }
        public virtual DbSet<tbTitulosPersona> tbTitulosPersona { get; set; }
        public virtual DbSet<tbTitulosRequisicion> tbTitulosRequisicion { get; set; }
        public virtual DbSet<V_AFP_RPT> V_AFP_RPT { get; set; }
        public virtual DbSet<V_BonosColaborador> V_BonosColaborador { get; set; }
        public virtual DbSet<V_CatalogoDeIngresos> V_CatalogoDeIngresos { get; set; }
        public virtual DbSet<V_CatalogoDePlanillasConIngresosYDeducciones> V_CatalogoDePlanillasConIngresosYDeducciones { get; set; }
        public virtual DbSet<V_ColaboradoresPorPlanilla> V_ColaboradoresPorPlanilla { get; set; }
        public virtual DbSet<V_ComisionesColaborador> V_ComisionesColaborador { get; set; }
        public virtual DbSet<V_DecimoCuartoMes> V_DecimoCuartoMes { get; set; }
        public virtual DbSet<V_DecimoCuartoMes_RPT> V_DecimoCuartoMes_RPT { get; set; }
        public virtual DbSet<V_DecimoTercerMes> V_DecimoTercerMes { get; set; }
        public virtual DbSet<V_DecimoTercerMes_RPT> V_DecimoTercerMes_RPT { get; set; }
        public virtual DbSet<V_DeduccionesExtraordinarias> V_DeduccionesExtraordinarias { get; set; }
        public virtual DbSet<V_DeduccionesExtraordinarias_Detalles> V_DeduccionesExtraordinarias_Detalles { get; set; }
        public virtual DbSet<V_DeduccionesExtraordinarias_Empleados> V_DeduccionesExtraordinarias_Empleados { get; set; }
        public virtual DbSet<V_DeduccionesExtraordinarias_EquipoEmpleado> V_DeduccionesExtraordinarias_EquipoEmpleado { get; set; }
        public virtual DbSet<V_DeduccionesExtrasColaboradores> V_DeduccionesExtrasColaboradores { get; set; }
        public virtual DbSet<V_DeduccionesInstitucionesFinancierasColaboradres> V_DeduccionesInstitucionesFinancierasColaboradres { get; set; }
        public virtual DbSet<V_EmpleadoBonos> V_EmpleadoBonos { get; set; }
        public virtual DbSet<V_FormaDePago> V_FormaDePago { get; set; }
        public virtual DbSet<V_GeneralTotales_RPT> V_GeneralTotales_RPT { get; set; }
        public virtual DbSet<V_IHSS_RPT> V_IHSS_RPT { get; set; }
        public virtual DbSet<V_INFOP_RPT> V_INFOP_RPT { get; set; }
        public virtual DbSet<V_InformacionColaborador> V_InformacionColaborador { get; set; }
        public virtual DbSet<V_InstitucionesFinancieras_RPT> V_InstitucionesFinancieras_RPT { get; set; }
        public virtual DbSet<V_ISR_RPT> V_ISR_RPT { get; set; }
        public virtual DbSet<V_Liquidaciones_RPT> V_Liquidaciones_RPT { get; set; }
        public virtual DbSet<V_Plani_EmpleadoPorPlanilla> V_Plani_EmpleadoPorPlanilla { get; set; }
        public virtual DbSet<V_Plani_FechaPlanilla> V_Plani_FechaPlanilla { get; set; }
        public virtual DbSet<V_Plani_TipoPlani> V_Plani_TipoPlani { get; set; }
        public virtual DbSet<V_PlanillaDeducciones> V_PlanillaDeducciones { get; set; }
        public virtual DbSet<V_PlanillaIngresos> V_PlanillaIngresos { get; set; }
        public virtual DbSet<V_PreviewPlanilla> V_PreviewPlanilla { get; set; }
        public virtual DbSet<V_RAP_RPT> V_RAP_RPT { get; set; }
        public virtual DbSet<V_tbAdelantoSueldo> V_tbAdelantoSueldo { get; set; }
        public virtual DbSet<V_tbCatalogoDeDeducciones> V_tbCatalogoDeDeducciones { get; set; }
        public virtual DbSet<V_tbCatalogoDeIngresos> V_tbCatalogoDeIngresos { get; set; }
        public virtual DbSet<V_tbEmpleadoComisiones> V_tbEmpleadoComisiones { get; set; }
        public virtual DbSet<V_TipoDeduccion> V_TipoDeduccion { get; set; }
    }
}
