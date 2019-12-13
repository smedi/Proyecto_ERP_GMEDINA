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
    public class SeleccionCandidatosController : Controller
    {
        private ERP_GMEDINAEntities db = new ERP_GMEDINAEntities();

        public ActionResult Index()
        {
            ViewBag.fare_Id = new SelectList(db.tbFasesReclutamiento, "fare_Id", "fare_Descripcion");
            ViewBag.per_Id = new SelectList(db.tbPersonas, "per_Id", "per_Identidad");
            //ViewBag.rper_Id = new SelectList(db.tbRequisiciones, "rper_Id", "req_Descripcion");
            Session["Usuario"] = new tbUsuario { usu_Id = 1 };
            List<tbSeleccionCandidatos> tbSeleccionCandidatos = new List<tbSeleccionCandidatos> { };
            return View(tbSeleccionCandidatos);
        }

        public ActionResult llenarTabla()
        {
            try
            {
                using (db = new ERP_GMEDINAEntities())
                {
                    var candidatos = db.V_SeleccionCandidatos
                        .Select(
                        t => new
                        {
                            Identidad = t.Identidad,
                            Nombre = t.Nombre,
                            Fase = t.Fase,
                            Plaza_Solicitada = t.Plaza_Solicitada,
                            Fecha = t.Fecha
                        }).ToList();
                    return Json(candidatos, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json("-2", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ChildRowData(int? id)
        {
            //declaramos la variable de coneccion solo para recuperar los datos necesarios.
            //posteriormente es destruida.
            List<V_SeleccionCandidatos> lista = new List<V_SeleccionCandidatos> { };
            using (db = new ERP_GMEDINAEntities())
            {
                try
                {
                    lista = db.V_SeleccionCandidatos.Where(x => x.Id == id).ToList();
                }
                catch
                {
                }
            }
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Create(tbSeleccionCandidatos tbSeleccionCandidatos)
        {
            string msj = "";
            try
            {
                var list = db.UDP_RRHH_tbSeleccionCandidatos_Insert(tbSeleccionCandidatos.per_Id,
                                                                        tbSeleccionCandidatos.fare_Id,
                                                                        tbSeleccionCandidatos.scan_Fecha,
                                                                        1,
                                                                        1,
                                                                        DateTime.Now);
                foreach (UDP_RRHH_tbSeleccionCandidatos_Insert_Result item in list)
                {
                    msj = item.MensajeError + " ";
                }
            }
            catch (Exception ex)
            {
                msj = "-2";
                ex.Message.ToString();
            }
            return Json(msj.Substring(0, 2), JsonRequestBehavior.AllowGet);
        }


        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    List<tbSeleccionCandidatos> tbSeleccionCandidatos = null;
        //    try
        //    {
        //        tbSeleccionCandidatos = new List<Models.tbSeleccionCandidatos> { };
        //        tbSeleccionCandidatos = db.tbSeleccionCandidatos.Where(x => x.scan_Estado).Include(t => t.tbFaseSeleccion).Include(t => t.tbFasesReclutamiento).Include(t => t.tbPersonas).Include(t => t.tbRequisiciones).Include(t => t.tbUsuario).Include(t => t.tbUsuario1).ToList();

        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Message.ToString();
        //        return HttpNotFound();
        //    }
        //    Session["id"] = id;
        //    var list = new tbSeleccionCandidatos();
        //    foreach (var item in tbSeleccionCandidatos)
        //    {
        //        list = new tbSeleccionCandidatos
        //        {
        //            hamo_AmonestacionAnterior = item.hamo_AmonestacionAnterior,
        //            hamo_Observacion = item.hamo_Observacion,
        //            tbTipoAmonestaciones = item.tbTipoAmonestaciones,
        //            tbUsuario = item.tbUsuario,
        //            hamo_FechaCrea = item.hamo_FechaCrea,
        //            tbUsuario1 = item.tbUsuario1,
        //            hamo_FechaModifica = item.hamo_FechaModifica
        //        };

        //    }
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

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
