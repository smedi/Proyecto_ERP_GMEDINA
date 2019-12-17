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
    public class HistorialVacacionesController : Controller
    {
        private ERP_GMEDINAEntities db = new ERP_GMEDINAEntities();

        // GET: HistorialVacaciones
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



        // GET: HistorialVacaciones/Details/5
        //Modal de Detalle 

        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    List<tbHistorialVacaciones> tbHistorialVacaciones = null;
        //    try
        //    {
        //        tbHistorialVacaciones = new List<Models.tbHistorialVacaciones> { };
        //        tbHistorialVacaciones = db.tbHistorialVacaciones.Where(x => x.emp_Id == id).Include(t => t.tbTipoAmonestaciones).Include(t => t.tbUsuario).Include(t => t.tbUsuario1).ToList();

        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Message.ToString();
        //        return HttpNotFound();
        //    }
        //    Session["id"] = id;
        //    var amonestaciones = new tbHistorialVacaciones();
        //    foreach (var item in tbHistorialVacaciones)
        //    {
        //        amonestaciones = new tbHistorialVacaciones
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
        //    return Json(amonestaciones, JsonRequestBehavior.AllowGet);
        //}


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<tbHistorialVacaciones> tbHistorialVacaciones = null;
            try
            {
                tbHistorialVacaciones = new List<Models.tbHistorialVacaciones> { };
                tbHistorialVacaciones = db.tbHistorialVacaciones.Where(x => x.emp_Id == id).Include(t => t.tbUsuario).Include(t => t.tbUsuario1).ToList();

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return HttpNotFound();
            }
            Session["id"] = id;
            var vacaciones = new tbHistorialVacaciones();
            foreach (var item in tbHistorialVacaciones)
            {
                vacaciones = new tbHistorialVacaciones
                {
                    hvac_Id = item.hvac_Id,
                    hvac_FechaInicio = item.hvac_FechaInicio,
                    hvac_FechaFin = item.hvac_FechaFin,
                    hvac_CantDias = item.hvac_CantDias,
                    hvac_DiasPagados = item.hvac_DiasPagados,
                    hvac_MesVacaciones = item.hvac_MesVacaciones,
                    hvac_AnioVacaciones = item.hvac_AnioVacaciones,
                    hvac_Estado = item.hvac_Estado,
                    hvac_RazonInactivo = item.hvac_RazonInactivo,
                    hvac_UsuarioCrea = item.hvac_UsuarioCrea,
                    hvac_FechaCrea = item.hvac_FechaCrea,
                    hvac_FechaModifica = item.hvac_FechaModifica,
                    //tbUsuario = item.tbUsuario,
                    //tbUsuario1 = item.tbUsuario1
                };

            }
            return Json(vacaciones, JsonRequestBehavior.AllowGet);
        }

        //GET: HistorialVacaciones/Create
        public JsonResult Create(tbHistorialVacaciones HistorialVacaciones)
        {
            string msj = "";
            try
            {
                var list = db.UDP_RRHH_tbHistorialVacaciones_Insert(HistorialVacaciones.emp_Id,
                                                                        HistorialVacaciones.hvac_FechaInicio,
                                                                        HistorialVacaciones.hvac_FechaFin,
                                                                        1,
                                                                        DateTime.Now);
                foreach (UDP_RRHH_tbHistorialVacaciones_Insert_Result item in list)
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
        [HttpPost]
        public ActionResult Delete(tbHistorialVacaciones tbHistorialVacaciones)
        {
            string msj = "";
            if (tbHistorialVacaciones.hvac_Id != 0 && tbHistorialVacaciones.hvac_RazonInactivo != "")
            {
                var Usuario = (tbUsuario)Session["Usuario"];
                try
                {
                    var list = db.UDP_RRHH_tbHistorialVacaciones_Delete(tbHistorialVacaciones.hvac_Id, tbHistorialVacaciones.hvac_RazonInactivo, 1, DateTime.Now);
                    foreach (UDP_RRHH_tbHistorialVacaciones_Delete_Result item in list)
                    {
                        msj = item.MensajeError + " ";
                    }
                }
                catch (Exception ex)
                {
                    msj = "-2";
                    ex.Message.ToString();
                }
                Session.Remove("id");
            }
            else
            {
                msj = "-3";
            }
            return Json(msj.Substring(0, 2), JsonRequestBehavior.AllowGet);
        }



        //GET: HistorialVacaciones/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tbHistorialVacaciones tbHistorialVacaciones = db.tbHistorialVacaciones.Find(id);
        //    if (tbHistorialVacaciones == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.hamo_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbHistorialVacaciones.hamo_UsuarioCrea);
        //    ViewBag.hamo_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbHistorialVacaciones.hamo_UsuarioModifica);
        //    ViewBag.emp_Id = new SelectList(db.tbEmpleados, "emp_Id", "emp_CuentaBancaria", tbHistorialVacaciones.emp_Id);
        //    ViewBag.hamo_AmonestacionAnterior = new SelectList(db.tbHistorialVacaciones, "hamo_Id", "hamo_Observacion", tbHistorialVacaciones.hamo_AmonestacionAnterior);
        //    ViewBag.tamo_Id = new SelectList(db.tbTipoAmonestaciones, "tamo_Id", "tamo_Descripcion", tbHistorialVacaciones.tamo_Id);
        //    return View(tbHistorialVacaciones);
        //}

        // POST: HistorialVacaciones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "hamo_Id,emp_Id,tamo_Id,hamo_Fecha,hamo_AmonestacionAnterior,hamo_Observacion,hamo_Estado,hamo_RazonInactivo,hamo_UsuarioCrea,hamo_FechaCrea,hamo_UsuarioModifica,hamo_FechaModifica")] tbHistorialVacaciones tbHistorialVacaciones)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(tbHistorialVacaciones).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.hamo_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbHistorialVacaciones.hamo_UsuarioCrea);
        //    ViewBag.hamo_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbHistorialVacaciones.hamo_UsuarioModifica);
        //    ViewBag.emp_Id = new SelectList(db.tbEmpleados, "emp_Id", "emp_CuentaBancaria", tbHistorialVacaciones.emp_Id);
        //    ViewBag.hamo_AmonestacionAnterior = new SelectList(db.tbHistorialVacaciones, "hamo_Id", "hamo_Observacion", tbHistorialVacaciones.hamo_AmonestacionAnterior);
        //    ViewBag.tamo_Id = new SelectList(db.tbTipoAmonestaciones, "tamo_Id", "tamo_Descripcion", tbHistorialVacaciones.tamo_Id);
        //    return View(tbHistorialVacaciones);
        //}

        // GET: HistorialVacaciones/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tbHistorialVacaciones tbHistorialVacaciones = db.tbHistorialVacaciones.Find(id);
        //    if (tbHistorialVacaciones == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tbHistorialVacaciones);
        //}

        // POST: HistorialVacaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbHistorialVacaciones tbHistorialVacaciones = db.tbHistorialVacaciones.Find(id);
            db.tbHistorialVacaciones.Remove(tbHistorialVacaciones);
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
