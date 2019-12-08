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
    public class HistorialIncapacidadesController : Controller
    {
        private ERP_GMEDINAEntities db = new ERP_GMEDINAEntities();

        // GET: HistorialIncapacidades
        public ActionResult Index()
        {
            var tbHistorialIncapacidades = db.tbHistorialIncapacidades.Include(t => t.tbUsuario).Include(t => t.tbUsuario1).Include(t => t.tbEmpleados).Include(t => t.tbTipoIncapacidades);
            return View(tbHistorialIncapacidades.ToList());
        }

        // GET: HistorialIncapacidades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbHistorialIncapacidades tbHistorialIncapacidades = db.tbHistorialIncapacidades.Find(id);
            if (tbHistorialIncapacidades == null)
            {
                return HttpNotFound();
            }
            return View(tbHistorialIncapacidades);
        }

        // GET: HistorialIncapacidades/Create
        public ActionResult Create()
        {
            ViewBag.hinc_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario");
            ViewBag.hinc_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario");
            ViewBag.emp_Id = new SelectList(db.tbEmpleados, "emp_Id", "emp_CuentaBancaria");
            ViewBag.ticn_Id = new SelectList(db.tbTipoIncapacidades, "ticn_Id", "ticn_Descripcion");
            return View();
        }

        // POST: HistorialIncapacidades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "hinc_Id,emp_Id,ticn_Id,hinc_Dias,hinc_CentroMedico,hinc_Doctor,hinc_Diagnostico,hinc_FechaInicio,hinc_FechaFin,hinc_Estado,hinc_RazonInactivo,hinc_UsuarioCrea,hinc_FechaCrea,hinc_UsuarioModifica,hinc_FechaModifica")] tbHistorialIncapacidades tbHistorialIncapacidades)
        {
            if (ModelState.IsValid)
            {
                db.tbHistorialIncapacidades.Add(tbHistorialIncapacidades);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.hinc_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbHistorialIncapacidades.hinc_UsuarioCrea);
            ViewBag.hinc_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbHistorialIncapacidades.hinc_UsuarioModifica);
            ViewBag.emp_Id = new SelectList(db.tbEmpleados, "emp_Id", "emp_CuentaBancaria", tbHistorialIncapacidades.emp_Id);
            ViewBag.ticn_Id = new SelectList(db.tbTipoIncapacidades, "ticn_Id", "ticn_Descripcion", tbHistorialIncapacidades.ticn_Id);
            return View(tbHistorialIncapacidades);
        }

        // GET: HistorialIncapacidades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbHistorialIncapacidades tbHistorialIncapacidades = db.tbHistorialIncapacidades.Find(id);
            if (tbHistorialIncapacidades == null)
            {
                return HttpNotFound();
            }
            ViewBag.hinc_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbHistorialIncapacidades.hinc_UsuarioCrea);
            ViewBag.hinc_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbHistorialIncapacidades.hinc_UsuarioModifica);
            ViewBag.emp_Id = new SelectList(db.tbEmpleados, "emp_Id", "emp_CuentaBancaria", tbHistorialIncapacidades.emp_Id);
            ViewBag.ticn_Id = new SelectList(db.tbTipoIncapacidades, "ticn_Id", "ticn_Descripcion", tbHistorialIncapacidades.ticn_Id);
            return View(tbHistorialIncapacidades);
        }

        // POST: HistorialIncapacidades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "hinc_Id,emp_Id,ticn_Id,hinc_Dias,hinc_CentroMedico,hinc_Doctor,hinc_Diagnostico,hinc_FechaInicio,hinc_FechaFin,hinc_Estado,hinc_RazonInactivo,hinc_UsuarioCrea,hinc_FechaCrea,hinc_UsuarioModifica,hinc_FechaModifica")] tbHistorialIncapacidades tbHistorialIncapacidades)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbHistorialIncapacidades).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.hinc_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbHistorialIncapacidades.hinc_UsuarioCrea);
            ViewBag.hinc_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbHistorialIncapacidades.hinc_UsuarioModifica);
            ViewBag.emp_Id = new SelectList(db.tbEmpleados, "emp_Id", "emp_CuentaBancaria", tbHistorialIncapacidades.emp_Id);
            ViewBag.ticn_Id = new SelectList(db.tbTipoIncapacidades, "ticn_Id", "ticn_Descripcion", tbHistorialIncapacidades.ticn_Id);
            return View(tbHistorialIncapacidades);
        }

        // GET: HistorialIncapacidades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbHistorialIncapacidades tbHistorialIncapacidades = db.tbHistorialIncapacidades.Find(id);
            if (tbHistorialIncapacidades == null)
            {
                return HttpNotFound();
            }
            return View(tbHistorialIncapacidades);
        }

        // POST: HistorialIncapacidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbHistorialIncapacidades tbHistorialIncapacidades = db.tbHistorialIncapacidades.Find(id);
            db.tbHistorialIncapacidades.Remove(tbHistorialIncapacidades);
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
