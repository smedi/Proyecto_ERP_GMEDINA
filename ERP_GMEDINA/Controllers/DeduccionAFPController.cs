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
            var tbDeduccionAFP = db.tbDeduccionAFP.Include(t => t.tbUsuario).Include(t => t.tbUsuario1).Include(t => t.tbAFP).Include(t => t.tbCatalogoDeDeducciones).Include(t => t.tbEmpleados);
            return View(tbDeduccionAFP.ToList());
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
            ViewBag.dafp_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario");
            ViewBag.dafp_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario");
            ViewBag.afp_Id = new SelectList(db.tbAFP, "afp_Id", "afp_Descripcion");
            ViewBag.cde_IdDeducciones = new SelectList(db.tbCatalogoDeDeducciones, "cde_IdDeducciones", "cde_DescripcionDeduccion");
            ViewBag.emp_Id = new SelectList(db.tbEmpleados, "emp_Id", "emp_CuentaBancaria");
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

            ViewBag.dafp_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbDeduccionAFP.dafp_UsuarioCrea);
            ViewBag.dafp_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbDeduccionAFP.dafp_UsuarioModifica);
            ViewBag.afp_Id = new SelectList(db.tbAFP, "afp_Id", "afp_Descripcion", tbDeduccionAFP.afp_Id);
            ViewBag.cde_IdDeducciones = new SelectList(db.tbCatalogoDeDeducciones, "cde_IdDeducciones", "cde_DescripcionDeduccion", tbDeduccionAFP.cde_IdDeducciones);
            ViewBag.emp_Id = new SelectList(db.tbEmpleados, "emp_Id", "emp_CuentaBancaria", tbDeduccionAFP.emp_Id);
            return View(tbDeduccionAFP);
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
            ViewBag.dafp_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbDeduccionAFP.dafp_UsuarioCrea);
            ViewBag.dafp_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbDeduccionAFP.dafp_UsuarioModifica);
            ViewBag.afp_Id = new SelectList(db.tbAFP, "afp_Id", "afp_Descripcion", tbDeduccionAFP.afp_Id);
            ViewBag.cde_IdDeducciones = new SelectList(db.tbCatalogoDeDeducciones, "cde_IdDeducciones", "cde_DescripcionDeduccion", tbDeduccionAFP.cde_IdDeducciones);
            ViewBag.emp_Id = new SelectList(db.tbEmpleados, "emp_Id", "emp_CuentaBancaria", tbDeduccionAFP.emp_Id);
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
            ViewBag.dafp_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbDeduccionAFP.dafp_UsuarioCrea);
            ViewBag.dafp_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbDeduccionAFP.dafp_UsuarioModifica);
            ViewBag.afp_Id = new SelectList(db.tbAFP, "afp_Id", "afp_Descripcion", tbDeduccionAFP.afp_Id);
            ViewBag.cde_IdDeducciones = new SelectList(db.tbCatalogoDeDeducciones, "cde_IdDeducciones", "cde_DescripcionDeduccion", tbDeduccionAFP.cde_IdDeducciones);
            ViewBag.emp_Id = new SelectList(db.tbEmpleados, "emp_Id", "emp_CuentaBancaria", tbDeduccionAFP.emp_Id);
            return View(tbDeduccionAFP);
        }

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
