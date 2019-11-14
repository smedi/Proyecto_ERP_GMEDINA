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
    public class DeduccionesExtraordinariasController : Controller
    {
        private ERP_GMEDINAEntities db = new ERP_GMEDINAEntities();

        // GET: DeduccionesExtraordinarias
        public ActionResult Index()
        {
            var tbDeduccionesExtraordinarias = db.tbDeduccionesExtraordinarias.Include(t => t.tbUsuario).Include(t => t.tbUsuario1).Include(t => t.tbCatalogoDeDeducciones).Include(t => t.tbEquipoEmpleados);
            return View(tbDeduccionesExtraordinarias.ToList());
        }

        // GET: DeduccionesExtraordinarias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbDeduccionesExtraordinarias tbDeduccionesExtraordinarias = db.tbDeduccionesExtraordinarias.Find(id);
            if (tbDeduccionesExtraordinarias == null)
            {
                return HttpNotFound();
            }
            return View(tbDeduccionesExtraordinarias);
        }

        // GET: DeduccionesExtraordinarias/Create
        public ActionResult Create()
        {
            ViewBag.dex_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario");
            ViewBag.dex_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario");
            ViewBag.cde_IdDeducciones = new SelectList(db.tbCatalogoDeDeducciones, "cde_IdDeducciones", "cde_DescripcionDeduccion");
            ViewBag.eqem_Id = new SelectList(db.tbEquipoEmpleados, "eqem_Id", "eqem_RazonInactivo");
            return View();
        }

        // POST: DeduccionesExtraordinarias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "dex_IdDeduccionesExtra,eqem_Id,dex_MontoInicial,dex_MontoRestante,dex_ObservacionesComentarios,cde_IdDeducciones,dex_Cuota,dex_UsuarioCrea,dex_FechaCrea,dex_UsuarioModifica,dex_FechaModifica,dex_Activo")] tbDeduccionesExtraordinarias tbDeduccionesExtraordinarias)
        {
            if (ModelState.IsValid)
            {
                db.tbDeduccionesExtraordinarias.Add(tbDeduccionesExtraordinarias);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.dex_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbDeduccionesExtraordinarias.dex_UsuarioCrea);
            ViewBag.dex_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbDeduccionesExtraordinarias.dex_UsuarioModifica);
            ViewBag.cde_IdDeducciones = new SelectList(db.tbCatalogoDeDeducciones, "cde_IdDeducciones", "cde_DescripcionDeduccion", tbDeduccionesExtraordinarias.cde_IdDeducciones);
            ViewBag.eqem_Id = new SelectList(db.tbEquipoEmpleados, "eqem_Id", "eqem_RazonInactivo", tbDeduccionesExtraordinarias.eqem_Id);
            return View(tbDeduccionesExtraordinarias);
        }

        // GET: DeduccionesExtraordinarias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbDeduccionesExtraordinarias tbDeduccionesExtraordinarias = db.tbDeduccionesExtraordinarias.Find(id);
            if (tbDeduccionesExtraordinarias == null)
            {
                return HttpNotFound();
            }
            ViewBag.dex_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbDeduccionesExtraordinarias.dex_UsuarioCrea);
            ViewBag.dex_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbDeduccionesExtraordinarias.dex_UsuarioModifica);
            ViewBag.cde_IdDeducciones = new SelectList(db.tbCatalogoDeDeducciones, "cde_IdDeducciones", "cde_DescripcionDeduccion", tbDeduccionesExtraordinarias.cde_IdDeducciones);
            ViewBag.eqem_Id = new SelectList(db.tbEquipoEmpleados, "eqem_Id", "eqem_RazonInactivo", tbDeduccionesExtraordinarias.eqem_Id);
            return View(tbDeduccionesExtraordinarias);
        }

        // POST: DeduccionesExtraordinarias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "dex_IdDeduccionesExtra,eqem_Id,dex_MontoInicial,dex_MontoRestante,dex_ObservacionesComentarios,cde_IdDeducciones,dex_Cuota,dex_UsuarioCrea,dex_FechaCrea,dex_UsuarioModifica,dex_FechaModifica,dex_Activo")] tbDeduccionesExtraordinarias tbDeduccionesExtraordinarias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbDeduccionesExtraordinarias).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.dex_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbDeduccionesExtraordinarias.dex_UsuarioCrea);
            ViewBag.dex_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbDeduccionesExtraordinarias.dex_UsuarioModifica);
            ViewBag.cde_IdDeducciones = new SelectList(db.tbCatalogoDeDeducciones, "cde_IdDeducciones", "cde_DescripcionDeduccion", tbDeduccionesExtraordinarias.cde_IdDeducciones);
            ViewBag.eqem_Id = new SelectList(db.tbEquipoEmpleados, "eqem_Id", "eqem_RazonInactivo", tbDeduccionesExtraordinarias.eqem_Id);
            return View(tbDeduccionesExtraordinarias);
        }

        // GET: DeduccionesExtraordinarias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbDeduccionesExtraordinarias tbDeduccionesExtraordinarias = db.tbDeduccionesExtraordinarias.Find(id);
            if (tbDeduccionesExtraordinarias == null)
            {
                return HttpNotFound();
            }
            return View(tbDeduccionesExtraordinarias);
        }

        // POST: DeduccionesExtraordinarias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbDeduccionesExtraordinarias tbDeduccionesExtraordinarias = db.tbDeduccionesExtraordinarias.Find(id);
            db.tbDeduccionesExtraordinarias.Remove(tbDeduccionesExtraordinarias);
            db.SaveChanges();
            return RedirectToAction("Index");
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
