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
    public class FormaPagoController : Controller
    {
        private ERP_GMEDINAEntities db = new ERP_GMEDINAEntities();

        // GET: FormaPago
        public ActionResult Index()
        {
            var tbFormaPago = db.tbFormaPago.Include(t => t.tbUsuario).Include(t => t.tbUsuario1);
            return View(tbFormaPago.ToList());
        }

        // GET: FormaPago/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbFormaPago tbFormaPago = db.tbFormaPago.Find(id);
            if (tbFormaPago == null)
            {
                return HttpNotFound();
            }
            return View(tbFormaPago);
        }

        // GET: FormaPago/Create
        public ActionResult Create()
        {
            ViewBag.fpa_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario");
            ViewBag.fpa_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario");
            return View();
        }

        // POST: FormaPago/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "fpa_IdFormaPago,fpa_Descripcion,fpa_UsuarioCrea,fpa_FechaCrea,fpa_UsuarioModifica,fpa_FechaModifica,fpa_Activo")] tbFormaPago tbFormaPago)
        {
            if (ModelState.IsValid)
            {
                db.tbFormaPago.Add(tbFormaPago);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.fpa_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbFormaPago.fpa_UsuarioCrea);
            ViewBag.fpa_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbFormaPago.fpa_UsuarioModifica);
            return View(tbFormaPago);
        }

        // GET: FormaPago/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbFormaPago tbFormaPago = db.tbFormaPago.Find(id);
            if (tbFormaPago == null)
            {
                return HttpNotFound();
            }
            ViewBag.fpa_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbFormaPago.fpa_UsuarioCrea);
            ViewBag.fpa_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbFormaPago.fpa_UsuarioModifica);
            return View(tbFormaPago);
        }

        // POST: FormaPago/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "fpa_IdFormaPago,fpa_Descripcion,fpa_UsuarioCrea,fpa_FechaCrea,fpa_UsuarioModifica,fpa_FechaModifica,fpa_Activo")] tbFormaPago tbFormaPago)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbFormaPago).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.fpa_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbFormaPago.fpa_UsuarioCrea);
            ViewBag.fpa_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbFormaPago.fpa_UsuarioModifica);
            return View(tbFormaPago);
        }

        // GET: FormaPago/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbFormaPago tbFormaPago = db.tbFormaPago.Find(id);
            if (tbFormaPago == null)
            {
                return HttpNotFound();
            }
            return View(tbFormaPago);
        }

        // POST: FormaPago/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbFormaPago tbFormaPago = db.tbFormaPago.Find(id);
            db.tbFormaPago.Remove(tbFormaPago);
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
