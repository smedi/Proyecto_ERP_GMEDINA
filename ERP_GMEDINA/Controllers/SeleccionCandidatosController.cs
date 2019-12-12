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
                            Plaza_Disponible = t.Plaza_Disponible,
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
