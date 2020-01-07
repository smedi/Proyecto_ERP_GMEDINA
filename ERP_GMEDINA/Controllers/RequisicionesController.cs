using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERP_GMEDINA.Models;

namespace ERP_GMEDINA.Controllers
{
    public class RequisicionesController : Controller
    {
        private ERP_GMEDINAEntities db = new ERP_GMEDINAEntities();

        // GET: Requisiciones
        public ActionResult Index()
        {
            var tbRequisiciones = db.tbRequisiciones.Include(t => t.tbUsuario).Include(t => t.tbUsuario1);

            return View(tbRequisiciones.ToList());
        }

        // GET: Requisiciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbRequisiciones tbRequisiciones = db.tbRequisiciones.Find(id);
            if (tbRequisiciones == null)
            {
                return HttpNotFound();
            }
            return View(tbRequisiciones);
        }

        // GET: Requisiciones/Create
        public ActionResult Create()
        {
            Session["Usuario"] = new tbUsuario { usu_Id = 1 };
            ViewBag.req_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario");
            ViewBag.req_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario");
            return View();
        }


        public ActionResult Detalles(int? id)
        {
            try
            {
                using (db = new ERP_GMEDINAEntities())
                {
                    var tbRequisiciones = db.tbRequisiciones.Select(x => new
                    {
                        req_Id = x.req_Id,
                        req_Experiencia = x.req_Experiencia,
                        req_Sexo = x.req_Sexo,
                        req_Descripcion = x.req_Descripcion,
                        req_EdadMinima = x.req_EdadMinima,
                        req_EdadMaxima = x.req_EdadMaxima,
                        req_EstadoCivil = x.req_EstadoCivil,
                        req_EducacionSuperior = x.req_EducacionSuperior,
                        req_Permanente = x.req_Permanente,
                        req_Duracion = x.req_Duracion,
                        req_Vacantes = x.req_Vacantes,
                        req_FechaRequisicion = x.req_FechaRequisicion,
                        req_FechaContratacion = x.req_FechaContratacion,
                        req_FechaCrea = x.req_FechaCrea,
                        req_FechaModifica = x.req_FechaModifica,
                        req_UsuarioCrea = x.tbUsuario.usu_Nombres,
                        req_UsuarioModifica = x.tbUsuario.usu_Nombres
                    }).Where(x => x.req_Id == id).ToList();
                    return Json(tbRequisiciones, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json("-2", JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Requisiciones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public JsonResult Create(tbRequisiciones tbRequisiciones, DatosProfesionalesArray DatosProfesionales)
        {
            string msj = "...";
            if (tbRequisiciones != null)
            {
                try
                {
                    using (db = new ERP_GMEDINAEntities())
                    {

                        var List = db.UDP_RRHH_tbRequisiciones_Insert(tbRequisiciones.req_Experiencia, tbRequisiciones.req_Sexo, tbRequisiciones.req_Descripcion, tbRequisiciones.req_EdadMinima, tbRequisiciones.req_EdadMaxima, tbRequisiciones.req_EstadoCivil, tbRequisiciones.req_EducacionSuperior, tbRequisiciones.req_Permanente, tbRequisiciones.req_Duracion, tbRequisiciones.req_Vacantes, tbRequisiciones.req_FechaRequisicion, tbRequisiciones.req_FechaContratacion, 1, DateTime.Now);

                        foreach (UDP_RRHH_tbRequisiciones_Insert_Result item in List)
                        {
                            msj = item.MensajeError + "";
                            //Competencias
                            if (DatosProfesionales.Competencias != null & msj != "-1")
                            {
                                for (int i = 0; i < DatosProfesionales.Competencias.Length; i++)
                                {
                                    var Competencias = db.rrhh_tbCompetenciasRequisicion_Insert(Int32.Parse(msj), DatosProfesionales.Competencias[i], 1, DateTime.Now);
                                    foreach (rrhh_tbCompetenciasRequisicion_Insert_Result comp in Competencias)
                                    {
                                        var result = comp.MensajeError + "";
                                    }
                                }
                            }
                            //Habilidades
                            if (DatosProfesionales.Habilidades != null & msj != "-1")
                            {
                                for (int i = 0; i < DatosProfesionales.Habilidades.Length; i++)
                                {
                                    var Habilidades = db.rrhh_tbHabilidadesRequisicion_Insert(Int32.Parse(msj), DatosProfesionales.Habilidades[i], 1, DateTime.Now);
                                    foreach (rrhh_tbHabilidadesRequisicion_Insert_Result hab in Habilidades)
                                    {
                                        var result = hab.MensajeError + "";
                                    }
                                }
                            }
                            //Idiomas
                            if (DatosProfesionales.Idiomas != null & msj != "-1")
                            {
                                for (int i = 0; i < DatosProfesionales.Idiomas.Length; i++)
                                {
                                    var Idiomas = db.rrhh_tbIdiomasRequisicion_Insert(Int32.Parse(msj), DatosProfesionales.Idiomas[i], 1, DateTime.Now);
                                    foreach (rrhh_tbIdiomasRequisicion_Insert_Result idi in Idiomas)
                                    {
                                        var result = idi.MensajeError + "";
                                    }
                                }
                            }
                            //Requerimientos Especiales
                            if (DatosProfesionales.ReqEspeciales != null & msj != "-1")
                            {
                                for (int i = 0; i < DatosProfesionales.ReqEspeciales.Length; i++)
                                {
                                    var ReqEspeciales = db.rrhh_tbRequerimientosEspecialesRequisicion_Insert(Int32.Parse(msj), DatosProfesionales.ReqEspeciales[i], 1, DateTime.Now);
                                    foreach (rrhh_tbRequerimientosEspecialesRequisicion_Insert_Result rep in ReqEspeciales)
                                    {
                                        var result = rep.MensajeError + "";
                                    }
                                }
                            }
                            //Titulos
                            if (DatosProfesionales.Titulos != null & msj != "-1")
                            {
                                for (int i = 0; i < DatosProfesionales.Titulos.Length; i++)
                                {
                                    var Titulos = db.rrhh_tbTitulosRequisicion_Insert(Int32.Parse(msj), DatosProfesionales.Titulos[i], 1, DateTime.Now);
                                    foreach (rrhh_tbTitulosRequisicion_Insert_Result rep in Titulos)
                                    {
                                        var result = rep.MensajeError + "";
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    msj = "-2";
                    ex.Message.ToString();
                }
            }
            else
            {
                msj = "-3";
            }
            return Json(msj, JsonRequestBehavior.AllowGet);
            
        }

        public ActionResult ChildRowData(int? id)
        {
            //declaramos la variable de coneccion solo para recuperar los datos necesarios.
            //posteriormente es destruida.
            //List<tbHorarios> lista = new List<tbHorarios> { };
            using (db = new ERP_GMEDINAEntities())
            {
                try
                {
                    var lista = db.V_DatosRequisicion.Where(x => x.req_Id == id)
                        .Select(tabla => new { Descripcion = tabla.Descripcion, TipoDato = tabla.TipoDato, req_Id = tabla.req_Id }).ToList();
                    DatosProfesionales Data = new DatosProfesionales();
                    Data.req_Id = Convert.ToInt32(id);
                    foreach(var X in lista)
                    {
                        switch(X.TipoDato)
                        {
                            case "C":
                                tbCompetencias Comp = new tbCompetencias();
                                Comp.comp_Descripcion = X.Descripcion;
                                Data.Competencias.Add(Comp);
                                break;

                            case "H":
                                tbHabilidades Habi = new tbHabilidades();
                                Habi.habi_Descripcion = X.Descripcion;
                                Data.Habilidades.Add(Habi);
                                break;

                            case "I":
                                tbIdiomas Idi = new tbIdiomas();
                                Idi.idi_Descripcion = X.Descripcion;
                                Data.Idiomas.Add(Idi);
                                break;

                            case "T":
                                tbTitulos Titu = new tbTitulos();
                                Titu.titu_Descripcion = X.Descripcion;
                                Data.Titulos.Add(Titu);
                                break;

                            case "R":
                                tbRequerimientosEspeciales Reqs = new tbRequerimientosEspeciales();
                                Reqs.resp_Descripcion = X.Descripcion;
                                Data.ReqEspeciales.Add(Reqs);
                                break;
                        }
                    }
                    
                    return Json(Data, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                }
            }
            return Json("-2", JsonRequestBehavior.AllowGet);
        }

        public JsonResult llenarTabla()
        {
            List<tbRequisiciones> tbRequisiciones =
                new List<Models.tbRequisiciones> { };
            foreach (tbRequisiciones x in db.tbRequisiciones.ToList().Where(x => x.req_Estado == true))
            {
                tbRequisiciones.Add(new tbRequisiciones
                {
                    req_Id = x.req_Id,
                    req_Experiencia = x.req_Experiencia,
                    req_Sexo = x.req_Sexo,
                    req_Descripcion = x.req_Descripcion,
                    req_EdadMinima = x.req_EdadMinima,
                    req_EdadMaxima = x.req_EdadMaxima,
                    req_EstadoCivil = x.req_EstadoCivil,
                    req_EducacionSuperior = x.req_EducacionSuperior,
                    req_Permanente = x.req_Permanente,
                    req_Duracion = x.req_Duracion,
                    req_Vacantes = x.req_Vacantes,
                    req_FechaRequisicion = x.req_FechaRequisicion,
                    req_FechaContratacion = x.req_FechaContratacion
                });
            }
            return Json(tbRequisiciones, JsonRequestBehavior.AllowGet);

        }


        public ActionResult DualListBoxData(int? id)
        {
            //declaramos la variable de coneccion solo para recuperar los datos necesarios.
            //posteriormente es destruida.
            //List<tbHorarios> lista = new List<tbHorarios> { };
            using (db = new ERP_GMEDINAEntities())
            {
                try
                {
                    if (id == null)
                    {
                        var lista = db.V_DatosProfesionales.Select(tabla => new { TipoDato = tabla.TipoDato, Id = tabla.Data_Id, Descripcion = tabla.Descripcion }).ToList();
                        DatosProfesionales Data = new DatosProfesionales();
                        foreach (var X in lista)
                        {
                            switch (X.TipoDato)
                            {
                                case "C":
                                    tbCompetencias Comp = new tbCompetencias();
                                    Comp.comp_Descripcion = X.Descripcion;
                                    Comp.comp_Id = X.Id;
                                    Data.Competencias.Add(Comp);
                                    break;

                                case "H":
                                    tbHabilidades Habi = new tbHabilidades();
                                    Habi.habi_Descripcion = X.Descripcion;
                                    Habi.habi_Id = X.Id;
                                    Data.Habilidades.Add(Habi);
                                    break;

                                case "I":
                                    tbIdiomas Idi = new tbIdiomas();
                                    Idi.idi_Descripcion = X.Descripcion;
                                    Idi.idi_Id = X.Id;
                                    Data.Idiomas.Add(Idi);
                                    break;

                                case "T":
                                    tbTitulos Titu = new tbTitulos();
                                    Titu.titu_Descripcion = X.Descripcion;
                                    Titu.titu_Id = X.Id;
                                    Data.Titulos.Add(Titu);
                                    break;

                                case "R":
                                    tbRequerimientosEspeciales Reqs = new tbRequerimientosEspeciales();
                                    Reqs.resp_Descripcion = X.Descripcion;
                                    Reqs.resp_Id = X.Id;
                                    Data.ReqEspeciales.Add(Reqs);
                                    break;
                            }
                        }

                        return Json(Data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        List<DatosProfessionalesEdit> Data = new List<DatosProfessionalesEdit> { };
                        var lista = db.V_DatosProfesionales.Select(tabla => new { TipoDato = tabla.TipoDato, Id = tabla.Data_Id, Descripcion = tabla.Descripcion }).ToList();
                        foreach (var X in lista)
                        {
                            switch (X.TipoDato)
                            {
                                case "C":
                                    var Comp = db.tbCompetenciasRequisicion.Select(c => new { comp_Id = c.comp_Id, Descripcion = c.tbCompetencias.comp_Descripcion, req_Id = c.req_Id }).Where(c => c.req_Id == id && c.comp_Id == X.Id).ToList();
                                    foreach (var cmp in Comp)
                                    {
                                        Data.Add(new DatosProfessionalesEdit { Id = cmp.comp_Id, Descripcion = cmp.Descripcion, TipoDato = "C", Seleccionado = 1 });
                                    }
                                    if (Comp.Count == 0)
                                        Data.Add(new DatosProfessionalesEdit { Id = X.Id, Descripcion = X.Descripcion, TipoDato = "C", Seleccionado = 0 });
                                    break;

                                case "H":
                                    var Hab = db.tbHabilidadesRequisicion.Select(h => new { habi_Id = h.habi_Id, Descripcion = h.tbHabilidades.habi_Descripcion, req_Id = h.req_Id }).Where(h => h.req_Id == id && h.habi_Id == X.Id).ToList();
                                    foreach (var habi in Hab)
                                    {
                                        if (X.Id == habi.habi_Id)
                                            Data.Add(new DatosProfessionalesEdit { Id = habi.habi_Id, Descripcion = habi.Descripcion, TipoDato = "H", Seleccionado = 1 });
                                    }
                                    if (Hab.Count == 0)
                                        Data.Add(new DatosProfessionalesEdit { Id = X.Id, Descripcion = X.Descripcion, TipoDato = "H", Seleccionado = 0 });
                                    break;

                                case "I":
                                    var Idi = db.tbIdiomasRequisicion.Select(i => new { idi_Id = i.idi_Id, Descripcion = i.tbIdiomas.idi_Descripcion, req_Id = i.req_Id }).Where(i => i.req_Id == id && i.idi_Id == X.Id).ToList();
                                    foreach (var idm in Idi)
                                    {
                                        if (X.Id == idm.idi_Id)
                                            Data.Add(new DatosProfessionalesEdit { Id = idm.idi_Id, Descripcion = idm.Descripcion, TipoDato = "I", Seleccionado = 1 });
                                    }
                                    if (Idi.Count == 0)
                                        Data.Add(new DatosProfessionalesEdit { Id = X.Id, Descripcion = X.Descripcion, TipoDato = "I", Seleccionado = 0 });
                                    break;

                                case "T":
                                    var Tit = db.tbTitulosRequisicion.Select(t => new { titu_Id = t.titu_Id, Descripcion = t.tbTitulos.titu_Descripcion, req_Id = t.req_Id }).Where(t => t.req_Id == id && t.titu_Id == X.Id).ToList();
                                    foreach (var Titu in Tit)
                                    {
                                        if (X.Id == Titu.titu_Id)
                                            Data.Add(new DatosProfessionalesEdit { Id = Titu.titu_Id, Descripcion = Titu.Descripcion, TipoDato = "T", Seleccionado = 1 });
                                    }
                                    if (Tit.Count == 0)
                                        Data.Add(new DatosProfessionalesEdit { Id = X.Id, Descripcion = X.Descripcion, TipoDato = "T", Seleccionado = 0 });
                                    break;

                                case "R":
                                    var Reqs = db.tbRequerimientosEspecialesRequisicion.Select(re => new { resp_Id = re.resp_Id, Descripcion = re.tbRequerimientosEspeciales.resp_Descripcion, req_Id = re.req_Id }).Where(re => re.req_Id == id && re.resp_Id == X.Id).ToList();
                                    foreach (var ReEs in Reqs)
                                    {
                                        if (X.Id == ReEs.resp_Id)
                                            Data.Add(new DatosProfessionalesEdit { Id = ReEs.resp_Id, Descripcion = ReEs.Descripcion, TipoDato = "R", Seleccionado = 1 });
                                    }
                                    if (Reqs.Count == 0)
                                        Data.Add(new DatosProfessionalesEdit { Id = X.Id, Descripcion = X.Descripcion, TipoDato = "R", Seleccionado = 0 });
                                    break;
                            }
                        }
                        return Json(Data, JsonRequestBehavior.AllowGet);
                    }


                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            }
            return Json("-2", JsonRequestBehavior.AllowGet);
        }

        // GET: Requisiciones/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                using (db = new ERP_GMEDINAEntities())
                {
                    var tbPersonas = db.tbRequisiciones
                        .Select(
                        x => new
                        {
                            req_Id = x.req_Id,
                            req_Experiencia = x.req_Experiencia,
                            req_Sexo = x.req_Sexo,
                            req_Descripcion = x.req_Descripcion,
                            req_EdadMinima = x.req_EdadMinima,
                            req_EdadMaxima = x.req_EdadMaxima,
                            req_EstadoCivil = x.req_EstadoCivil,
                            req_EducacionSuperior = x.req_EducacionSuperior,
                            req_Permanente = x.req_Permanente,
                            req_Duracion = x.req_Duracion,
                            req_Vacantes = x.req_Vacantes,
                            req_FechaRequisicion = x.req_FechaRequisicion,
                            req_FechaContratacion = x.req_FechaContratacion
                        })
                        .Where(x => x.req_Id == id).ToList();
                    return Json(tbPersonas, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json("-2", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Edit(tbRequisiciones tbRequisiciones, DatosProfesionalesArray DatosProfesionales)
        {
            string msj = "...";
            if (tbRequisiciones != null)
            {
                try
                {
                    using (db = new ERP_GMEDINAEntities())
                    {

                        var List = db.UDP_RRHH_tbRequisiciones_Insert(tbRequisiciones.req_Experiencia, tbRequisiciones.req_Sexo, tbRequisiciones.req_Descripcion, tbRequisiciones.req_EdadMinima, tbRequisiciones.req_EdadMaxima, tbRequisiciones.req_EstadoCivil, tbRequisiciones.req_EducacionSuperior, tbRequisiciones.req_Permanente, tbRequisiciones.req_Duracion, tbRequisiciones.req_Vacantes, tbRequisiciones.req_FechaRequisicion, tbRequisiciones.req_FechaContratacion, 1, DateTime.Now);

                        //Items que no estan en la lista vieja pero si en la lista nueva

                        foreach (UDP_RRHH_tbRequisiciones_Insert_Result item in List)
                        {
                            msj = item.MensajeError + "";



                            //Competencias
                            if (DatosProfesionales.Competencias != null & msj != "-1")
                            {
                                for (int i = 0; i < DatosProfesionales.Competencias.Length; i++)
                                {
                                    var Competencias = db.rrhh_tbCompetenciasRequisicion_Insert(Int32.Parse(msj), DatosProfesionales.Competencias[i], 1, DateTime.Now);
                                    foreach (rrhh_tbCompetenciasRequisicion_Insert_Result comp in Competencias)
                                    {
                                        var result = comp.MensajeError + "";
                                    }
                                }
                            }
                            //Habilidades
                            if (DatosProfesionales.Habilidades != null & msj != "-1")
                            {
                                for (int i = 0; i < DatosProfesionales.Habilidades.Length; i++)
                                {
                                    var Habilidades = db.rrhh_tbHabilidadesRequisicion_Insert(Int32.Parse(msj), DatosProfesionales.Habilidades[i], 1, DateTime.Now);
                                    foreach (rrhh_tbHabilidadesRequisicion_Insert_Result hab in Habilidades)
                                    {
                                        var result = hab.MensajeError + "";
                                    }
                                }
                            }
                            //Idiomas
                            if (DatosProfesionales.Idiomas != null & msj != "-1")
                            {
                                for (int i = 0; i < DatosProfesionales.Idiomas.Length; i++)
                                {
                                    var Idiomas = db.rrhh_tbIdiomasRequisicion_Insert(Int32.Parse(msj), DatosProfesionales.Idiomas[i], 1, DateTime.Now);
                                    foreach (rrhh_tbIdiomasRequisicion_Insert_Result idi in Idiomas)
                                    {
                                        var result = idi.MensajeError + "";
                                    }
                                }
                            }
                            //Requerimientos Especiales
                            if (DatosProfesionales.ReqEspeciales != null & msj != "-1")
                            {
                                for (int i = 0; i < DatosProfesionales.ReqEspeciales.Length; i++)
                                {
                                    var ReqEspeciales = db.rrhh_tbRequerimientosEspecialesRequisicion_Insert(Int32.Parse(msj), DatosProfesionales.ReqEspeciales[i], 1, DateTime.Now);
                                    foreach (rrhh_tbRequerimientosEspecialesRequisicion_Insert_Result rep in ReqEspeciales)
                                    {
                                        var result = rep.MensajeError + "";
                                    }
                                }
                            }
                            //Titulos
                            if (DatosProfesionales.Titulos != null & msj != "-1")
                            {
                                for (int i = 0; i < DatosProfesionales.Titulos.Length; i++)
                                {
                                    var Titulos = db.rrhh_tbTitulosRequisicion_Insert(Int32.Parse(msj), DatosProfesionales.Titulos[i], 1, DateTime.Now);
                                    foreach (rrhh_tbTitulosRequisicion_Insert_Result rep in Titulos)
                                    {
                                        var result = rep.MensajeError + "";
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    msj = "-2";
                    ex.Message.ToString();
                }
            }
            else
            {
                msj = "-3";
            }
            return Json(msj, JsonRequestBehavior.AllowGet);
        }

        // POST: Requisiciones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //==============================================================================**************************************====================================

        private DatosProfesionalesArray IntArray(List<V_DatosRequisicion> DataDB)
        {
            List<int> Competencias = new List<int>();
            List<int> Habilidades = new List<int>();
            List<int> Idiomas = new List<int>();
            List<int> Titulos = new List<int>();
            List<int> RequisitosEsp = new List<int>();

            DatosProfesionalesArray P = new DatosProfesionalesArray();
            foreach(V_DatosRequisicion D in DataDB)
            {
                if (D.TipoDato == "C")
                    Competencias.Add(D.Data_Id);

                if (D.TipoDato == "H")
                    Habilidades.Add(D.Data_Id);

                if (D.TipoDato == "I")
                    Idiomas.Add(D.Data_Id);

                if (D.TipoDato == "T")
                    Titulos.Add(D.Data_Id);

                if (D.TipoDato == "R")
                    RequisitosEsp.Add(D.Data_Id);
            }

            P.Competencias = Competencias.ToArray();
            P.Habilidades = Habilidades.ToArray();
            P.Idiomas = Idiomas.ToArray();
            P.Titulos = Titulos.ToArray();
            P.ReqEspeciales = RequisitosEsp.ToArray();

            return P;
        }


        
        // GET: Requisiciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbRequisiciones tbRequisiciones = db.tbRequisiciones.Find(id);
            if (tbRequisiciones == null)
            {
                return HttpNotFound();
            }
            return View(tbRequisiciones);
        }

        // POST: Requisiciones/Delete/5
        [HttpPost]
        public ActionResult Delete(tbRequisiciones Requisicion)
        {
            string msj = "";
            using (db = new ERP_GMEDINAEntities())
            {
                try
                {
                    var _list = db.UDP_RRHH_tbRequisiciones_Delete(Requisicion.req_Id, Requisicion.req_RazonInactivo, 1, DateTime.Now);
                    foreach (UDP_RRHH_tbRequisiciones_Delete_Result item in _list)
                    {
                        msj = item.MensajeError + "";                        
                    }
                }
                catch
                {
                    msj = "-2";
                }
            }
            return Json(msj, JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
