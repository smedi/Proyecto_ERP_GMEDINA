﻿using System;
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
    public class HistorialVacacionesController : Controller
    {
        private ERP_GMEDINAEntities db = new ERP_GMEDINAEntities();

        // GET: HistorialAmonestaciones
        public ActionResult Index()
        {
            ViewBag.hvac_Id = new SelectList(db.tbHistorialVacaciones, "hvac_Id", "hvac_Descripcion");
            var tbHistorialVacaciones = db.tbHistorialVacaciones.Include(t => t.tbEmpleados).Include(t => t.tbUsuario).Include(t => t.tbUsuario1);
            return View(tbHistorialVacaciones);
        }
        public ActionResult llenarTabla()
        {
            try
            {
                using (db = new ERP_GMEDINAEntities())
                {
                    var Empleados = db.V_HVacacionesEmpleados.Where(t => t.emp_Estado == true)
                        .Select(
                        t => new
                        {
                            emp_Id = t.emp_Id,
                            Empleado = t.emp_NombreCompleto,
                            Cargo = t.car_Descripcion,
                            Departamento = t.depto_Descripcion
                        }).ToList();
                    return Json(Empleados, JsonRequestBehavior.AllowGet);
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
            List<V_Historialvacaciones> lista = new List<V_Historialvacaciones> { };
            using (db = new ERP_GMEDINAEntities())
            {
                try
                {
                    lista = db.V_Historialvacaciones.Where(x => x.emp_Id == id).ToList();
                }
                catch
                {
                }
            }
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public ActionResult llenarDropDowlist()
        {
            var TipoAmonestacion = new List<object> { };
            using (db = new ERP_GMEDINAEntities())
            {
                try
                {
                    TipoAmonestacion.Add(new
                    {
                        Id = 0,
                        Descripcion = "**Seleccione una opción**"
                    });
                    TipoAmonestacion.AddRange(db.tbTipoAmonestaciones
                        .Select(tabla => new { id = tabla.tamo_Id, Descripcion = tabla.tamo_Descripcion })
                        .ToList());

                }
                catch
                {
                    return Json("-2", 0);
                }
            }
            var result = new Dictionary<string, object>();
            result.Add("TipoAmonestacion", TipoAmonestacion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        // GET: HistorialAmonestaciones/Details/5
        //Modal de Detalle 
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<tbHistorialAmonestaciones> tbHistorialAmonestaciones = null;
            try
            {
                tbHistorialAmonestaciones = new List<Models.tbHistorialAmonestaciones> { };
                tbHistorialAmonestaciones = db.tbHistorialAmonestaciones.Where(x => x.emp_Id == id).Include(t => t.tbTipoAmonestaciones).Include(t => t.tbUsuario).Include(t => t.tbUsuario1).ToList();

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return HttpNotFound();
            }
            Session["id"] = id;
            var amonestaciones = new tbHistorialAmonestaciones();
            foreach (var item in tbHistorialAmonestaciones)
            {
                amonestaciones = new tbHistorialAmonestaciones
                {
                    hamo_AmonestacionAnterior = item.hamo_AmonestacionAnterior,
                    hamo_Observacion = item.hamo_Observacion,
                    tbTipoAmonestaciones = item.tbTipoAmonestaciones,
                    tbUsuario = item.tbUsuario,
                    hamo_FechaCrea = item.hamo_FechaCrea,
                    tbUsuario1 = item.tbUsuario1,
                    hamo_FechaModifica = item.hamo_FechaModifica
                };

            }
            return Json(amonestaciones, JsonRequestBehavior.AllowGet);
        }

        // GET: HistorialAmonestaciones/Create
        public ActionResult Create()
        {
            ViewBag.hamo_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario");
            ViewBag.hamo_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario");
            ViewBag.emp_Id = new SelectList(db.tbEmpleados, "emp_Id", "emp_CuentaBancaria");
            ViewBag.hamo_AmonestacionAnterior = new SelectList(db.tbHistorialAmonestaciones, "hamo_Id", "hamo_Observacion");
            ViewBag.tamo_Id = new SelectList(db.tbTipoAmonestaciones, "tamo_Id", "tamo_Descripcion");
            return View();
        }

        // POST: HistorialAmonestaciones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "hamo_Id,emp_Id,tamo_Id,hamo_Fecha,hamo_AmonestacionAnterior,hamo_Observacion,hamo_Estado,hamo_RazonInactivo,hamo_UsuarioCrea,hamo_FechaCrea,hamo_UsuarioModifica,hamo_FechaModifica")] tbHistorialAmonestaciones tbHistorialAmonestaciones)
        {
            if (ModelState.IsValid)
            {
                db.tbHistorialAmonestaciones.Add(tbHistorialAmonestaciones);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.hamo_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbHistorialAmonestaciones.hamo_UsuarioCrea);
            ViewBag.hamo_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbHistorialAmonestaciones.hamo_UsuarioModifica);
            ViewBag.emp_Id = new SelectList(db.tbEmpleados, "emp_Id", "emp_CuentaBancaria", tbHistorialAmonestaciones.emp_Id);
            ViewBag.hamo_AmonestacionAnterior = new SelectList(db.tbHistorialAmonestaciones, "hamo_Id", "hamo_Observacion", tbHistorialAmonestaciones.hamo_AmonestacionAnterior);
            ViewBag.tamo_Id = new SelectList(db.tbTipoAmonestaciones, "tamo_Id", "tamo_Descripcion", tbHistorialAmonestaciones.tamo_Id);
            return View(tbHistorialAmonestaciones);
        }

        // GET: HistorialAmonestaciones/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tbHistorialAmonestaciones tbHistorialAmonestaciones = db.tbHistorialAmonestaciones.Find(id);
        //    if (tbHistorialAmonestaciones == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.hamo_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbHistorialAmonestaciones.hamo_UsuarioCrea);
        //    ViewBag.hamo_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbHistorialAmonestaciones.hamo_UsuarioModifica);
        //    ViewBag.emp_Id = new SelectList(db.tbEmpleados, "emp_Id", "emp_CuentaBancaria", tbHistorialAmonestaciones.emp_Id);
        //    ViewBag.hamo_AmonestacionAnterior = new SelectList(db.tbHistorialAmonestaciones, "hamo_Id", "hamo_Observacion", tbHistorialAmonestaciones.hamo_AmonestacionAnterior);
        //    ViewBag.tamo_Id = new SelectList(db.tbTipoAmonestaciones, "tamo_Id", "tamo_Descripcion", tbHistorialAmonestaciones.tamo_Id);
        //    return View(tbHistorialAmonestaciones);
        //}

        // POST: HistorialAmonestaciones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "hamo_Id,emp_Id,tamo_Id,hamo_Fecha,hamo_AmonestacionAnterior,hamo_Observacion,hamo_Estado,hamo_RazonInactivo,hamo_UsuarioCrea,hamo_FechaCrea,hamo_UsuarioModifica,hamo_FechaModifica")] tbHistorialAmonestaciones tbHistorialAmonestaciones)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbHistorialAmonestaciones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.hamo_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbHistorialAmonestaciones.hamo_UsuarioCrea);
            ViewBag.hamo_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbHistorialAmonestaciones.hamo_UsuarioModifica);
            ViewBag.emp_Id = new SelectList(db.tbEmpleados, "emp_Id", "emp_CuentaBancaria", tbHistorialAmonestaciones.emp_Id);
            ViewBag.hamo_AmonestacionAnterior = new SelectList(db.tbHistorialAmonestaciones, "hamo_Id", "hamo_Observacion", tbHistorialAmonestaciones.hamo_AmonestacionAnterior);
            ViewBag.tamo_Id = new SelectList(db.tbTipoAmonestaciones, "tamo_Id", "tamo_Descripcion", tbHistorialAmonestaciones.tamo_Id);
            return View(tbHistorialAmonestaciones);
        }

        // GET: HistorialAmonestaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbHistorialAmonestaciones tbHistorialAmonestaciones = db.tbHistorialAmonestaciones.Find(id);
            if (tbHistorialAmonestaciones == null)
            {
                return HttpNotFound();
            }
            return View(tbHistorialAmonestaciones);
        }

        // POST: HistorialAmonestaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbHistorialAmonestaciones tbHistorialAmonestaciones = db.tbHistorialAmonestaciones.Find(id);
            db.tbHistorialAmonestaciones.Remove(tbHistorialAmonestaciones);
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