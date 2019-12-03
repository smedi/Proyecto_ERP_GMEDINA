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
    public class DeduccionAFPController : Controller
    {
        private ERP_GMEDINAEntities db = new ERP_GMEDINAEntities();

        // GET: DeduccionAFP
        public ActionResult Index()
        {
            var tbDeduccionAFP = db.tbDeduccionAFP.Where(t => t.dafp_Activo == true).Include(t => t.tbAFP).Include(t => t.tbCatalogoDeDeducciones).Include(t => t.tbEmpleados);
            return View(tbDeduccionAFP.ToList());
        }

        // GET: OBTENER LA DATA Y ENVIARLA A LA VISTA EN FORMATO JSON
        public ActionResult GetData()
        {
            //SI SE LLEGA A DAR PROBLEMAS DE "REFERENCIAS CIRCULARES", OBTENER LA DATA DE ESTA FORMA
            //SELECCIONANDO UNO POR UNO LOS CAMPOS QUE NECESITAREMOS
            //DE LO CONTRARIO, HACERLO DE LA FORMA CONVENCIONAL (EJEMPLO: db.tbCatalogoDeDeducciones.ToList(); )
            var tbDeduccionAFP1 = db.tbDeduccionAFP
                        .Select(t => new { dafp_Id = t.dafp_Id,
                                           per_Nombres = t.tbPersonas.per_Nombres,
                                           per_Apellidos = t.tbPersonas.per_Apellidos,
                                           emp_CuentaBancaria = t.tbEmpleados.emp_CuentaBancaria,
                                           dafp_AporteLps = t.dafp_AporteLps,
                                           dafp_AporteDol = t.dafp_AporteDol,
                                           afp_Descripcion = t.tbAFP.afp_Descripcion,
                                           cde_DescripcionDeduccion = t.tbCatalogoDeDeducciones.cde_DescripcionDeduccion,
                                           afp_Id = t.afp_Id,
                                           emp_Id = t.emp_Id,
                                           cde_IdDeducciones = t.cde_IdDeducciones,
                                           dafp_UsuarioCrea = t.dafp_UsuarioCrea,
                                           dafp_UsuarioModifica = t.dafp_UsuarioModifica,
                                           dafp_FechaCrea = t.dafp_FechaCrea,
                                           dafp_FechaModifica = t.dafp_FechaModifica,
                                           dafp_Activo = t.dafp_Activo
                                         })
                                           .Where(t => t.dafp_Activo == true)
                        .ToList();
            //RETORNAR JSON AL LADO DEL CLIENTE
            return new JsonResult { Data = tbDeduccionAFP1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // GET: DeduccionAFP/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbDeduccionAFP tbDeduccionAFP = db.tbDeduccionAFP.Find(id);
            if (tbDeduccionAFP == null)
            {
                return HttpNotFound();
            }
            return View(tbDeduccionAFP);
        }

        // GET: DeduccionAFP/Create
        public ActionResult Create()
        {
            /*ViewBag.afp_Id = new SelectList(db.tbAFP, "afp_Id", "afp_Descripcion");
            ViewBag.cde_IdDeducciones = new SelectList(db.tbCatalogoDeDeducciones, "cde_IdDeducciones", "cde_DescripcionDeduccion");
            ViewBag.emp_Id = new SelectList(db.tbEmpleados, "emp_Id", "emp_CuentaBancaria");*/
            return View();
        }

        // POST: DeduccionAFP/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "dafp_Id,dafp_AporteLps,dafp_AporteDol,afp_Id,cde_IdDeducciones,emp_Id,dafp_UsuarioCrea,dafp_FechaCrea,dafp_UsuarioModifica,dafp_FechaModifica,dafp_Activo")] tbDeduccionAFP tbDeduccionAFP)
        {
            if (ModelState.IsValid)
            {
                db.tbDeduccionAFP.Add(tbDeduccionAFP);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

           /* 
            ViewBag.afp_Id = new SelectList(db.tbAFP, "afp_Id", "afp_Descripcion", tbDeduccionAFP.afp_Id);
            ViewBag.cde_IdDeducciones = new SelectList(db.tbCatalogoDeDeducciones, "cde_IdDeducciones", "cde_DescripcionDeduccion", tbDeduccionAFP.cde_IdDeducciones);
            ViewBag.emp_Id = new SelectList(db.tbEmpleados, "emp_Id", "emp_CuentaBancaria", tbDeduccionAFP.emp_Id);
           */
            return View(tbDeduccionAFP);
        }


        //FUNCIÓN: OBETENER LA DATA PARA LLENAR LOS DROPDOWNLIST DE EDICIÓN Y CREACIÓN
        public JsonResult EditGetEmpleadoDDL()
        {
            //OBTENER LA DATA QUE NECESITAMOS, HACIENDOLO DE ESTA FORMA SE EVITA LA EXCEPCION POR "REFERENCIAS CIRCULARES"
            var DDL =
            from DeduAFP in db.tbDeduccionAFP
            join Emp in db.tbEmpleados on Emp.emp_Id equals DeduAFP.emp_Id into prodGroup
            join Per in db.tbPersonas on Per.per_Id equals Emp.per_Id into prodsGroup
            select new { Id = DeduAFP.emp_Id, Descripcion = Per.per_Nombres + ' ' + Per.per_Apellidos };
            //RETORNAR LA DATA EN FORMATO JSON AL CLIENTE 
            return Json(DDL, JsonRequestBehavior.AllowGet);
        }

        //FUNCIÓN: OBETENER LA DATA PARA LLENAR LOS DROPDOWNLIST DE EDICIÓN Y CREACIÓN
        public JsonResult EditGetAFPDDL()
        {
            //OBTENER LA DATA QUE NECESITAMOS, HACIENDOLO DE ESTA FORMA SE EVITA LA EXCEPCION POR "REFERENCIAS CIRCULARES"
            var DDL =
            from DeduAFP in db.tbDeduccionAFP
            join AFP in db.tbAFP on DeduAFP.afp_Id equals AFP.afp_Id into prodGroup
            select new { Id = AFP.afp_Id, Descripcion = AFP.afp_Descripcion };
            //RETORNAR LA DATA EN FORMATO JSON AL CLIENTE 
            return Json(DDL, JsonRequestBehavior.AllowGet);
        }

        //FUNCIÓN: OBETENER LA DATA PARA LLENAR LOS DROPDOWNLIST DE EDICIÓN Y CREACIÓN
        public JsonResult EditGetDeduccionesDDL()
        {
            //OBTENER LA DATA QUE NECESITAMOS, HACIENDOLO DE ESTA FORMA SE EVITA LA EXCEPCION POR "REFERENCIAS CIRCULARES"
            var DDL =
            from DeduAFP in db.tbDeduccionAFP
            join CatDeduc in db.tbCatalogoDeDeducciones on DeduAFP.cde_IdDeducciones equals CatDeduc.cde_IdDeducciones into prodGroup
            select new { Id = CatDeduc.cde_IdDeducciones, Descripcion = CatDeduc.cde_DescripcionDeduccion };
            //RETORNAR LA DATA EN FORMATO JSON AL CLIENTE 
            return Json(DDL, JsonRequestBehavior.AllowGet);
        }


        // GET: DeduccionAFP/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbDeduccionAFP tbDeduccionAFP = db.tbDeduccionAFP.Find(id);
            if (tbDeduccionAFP == null)
            {
                return HttpNotFound();
            }

            /*
            ViewBag.afp_Id = new SelectList(db.tbAFP, "afp_Id", "afp_Descripcion", tbDeduccionAFP.afp_Id);
            ViewBag.cde_IdDeducciones = new SelectList(db.tbCatalogoDeDeducciones, "cde_IdDeducciones", "cde_DescripcionDeduccion", tbDeduccionAFP.cde_IdDeducciones);
            ViewBag.emp_Id = new SelectList(db.tbEmpleados, "emp_Id", "emp_CuentaBancaria", tbDeduccionAFP.emp_Id);
            */
            return View(tbDeduccionAFP);
        }

        // POST: DeduccionAFP/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "dafp_Id,dafp_AporteLps,dafp_AporteDol,afp_Id,cde_IdDeducciones,emp_Id,dafp_UsuarioCrea,dafp_FechaCrea,dafp_UsuarioModifica,dafp_FechaModifica,dafp_Activo")] tbDeduccionAFP tbDeduccionAFP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbDeduccionAFP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            /*
            ViewBag.afp_Id = new SelectList(db.tbAFP, "afp_Id", "afp_Descripcion", tbDeduccionAFP.afp_Id);
            ViewBag.cde_IdDeducciones = new SelectList(db.tbCatalogoDeDeducciones, "cde_IdDeducciones", "cde_DescripcionDeduccion", tbDeduccionAFP.cde_IdDeducciones);
            ViewBag.emp_Id = new SelectList(db.tbEmpleados, "emp_Id", "emp_CuentaBancaria", tbDeduccionAFP.emp_Id);
            */
            return View(tbDeduccionAFP);
        }


        /*
        // GET: DeduccionAFP/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbDeduccionAFP tbDeduccionAFP = db.tbDeduccionAFP.Find(id);
            if (tbDeduccionAFP == null)
            {
                return HttpNotFound();
            }
            return View(tbDeduccionAFP);
        }

        // POST: DeduccionAFP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbDeduccionAFP tbDeduccionAFP = db.tbDeduccionAFP.Find(id);
            db.tbDeduccionAFP.Remove(tbDeduccionAFP);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        */

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
