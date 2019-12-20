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
            
       ViewBag.ticn_Id = new SelectList(db.tbTipoIncapacidades, "ticn_Id", "ticn_Descripcion");

        var tbHistorialIncapacidades = db.tbHistorialIncapacidades.Include(t => t.tbUsuario).Include(t => t.tbUsuario1).Include(t => t.tbEmpleados).Include(t => t.tbTipoIncapacidades);
            return View(tbHistorialIncapacidades.ToList());
        }




        public ActionResult llenarTabla()
        {
            try
            {
                using (db = new ERP_GMEDINAEntities())
                {
                    var Empleados = db.V_EmpleadoIncapacidades.Where(t => t.emp_Estado == true)
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
            List<V_HistorialIncapacidades> lista = new List<V_HistorialIncapacidades> { };
            using (db = new ERP_GMEDINAEntities())
            {
                try
                {
                    lista = db.V_HistorialIncapacidades.Where(x => x.emp_Id == id & x.hinc_Estado == true).ToList();

                }
                catch
                {

                }
               
            }
           
                return Json(lista, JsonRequestBehavior.AllowGet);
        }


       

       
        // GET: HistorialIncapacidades/Details/5
        public ActionResult Details(int? id)
        {
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                tbHistorialIncapacidades tbincapacidades = null;
               
                try
                {
                    tbincapacidades = db.tbHistorialIncapacidades.Find(id);
                    if (tbincapacidades == null || !tbincapacidades.hinc_Estado)
                    {
                        return HttpNotFound();
                    }
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                    return HttpNotFound();
                }
                Session["id"] = id;
                var incapacidades = new tbHistorialIncapacidades
              
                {
                    hinc_Id = tbincapacidades.hinc_Id,
                   tbTipoIncapacidades= new tbTipoIncapacidades { ticn_Descripcion = tbincapacidades.tbTipoIncapacidades.ticn_Descripcion },
                    hinc_Dias = tbincapacidades.hinc_Dias,
                    hinc_CentroMedico = tbincapacidades.hinc_CentroMedico,
                    hinc_Doctor = tbincapacidades.hinc_Doctor,
                    hinc_Diagnostico = tbincapacidades.hinc_Diagnostico,
                    hinc_FechaInicio = tbincapacidades.hinc_FechaInicio,
                    hinc_FechaFin = tbincapacidades.hinc_FechaFin,
                    tbUsuario = new tbUsuario { usu_NombreUsuario = tbincapacidades.tbUsuario.usu_NombreUsuario },
                    tbUsuario1 = new tbUsuario { usu_NombreUsuario = tbincapacidades.tbUsuario1.usu_NombreUsuario }
                };

                return Json(incapacidades, JsonRequestBehavior.AllowGet);
            }
        }


        // GET: HistorialIncapacidades/Create
        //public ActionResult Create()
        //{
        //    ViewBag.hinc_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario");
        //    ViewBag.hinc_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario");
        //    ViewBag.emp_Id = new SelectList(db.tbEmpleados, "emp_Id", "emp_CuentaBancaria");
        //    ViewBag.ticn_Id = new SelectList(db.tbTipoIncapacidades, "ticn_Id", "ticn_Descripcion");
        //    return View();
        //}

        // POST: HistorialIncapacidades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public JsonResult Create(tbHistorialIncapacidades tbHistorialIncapacidades)
        {
            string msj = "";
            if (tbHistorialIncapacidades.hinc_Diagnostico != "")
            {
                var Usuario = (tbUsuario)Session["Usuario"];
                try
                {
                    var list = db.UDP_RRHH_tbHistorialIncapacidades_Insert(tbHistorialIncapacidades.emp_Id,
                                                                            tbHistorialIncapacidades.ticn_Id,
                                                                            tbHistorialIncapacidades.hinc_Dias,
                                                                            tbHistorialIncapacidades.hinc_CentroMedico,
                                                                            tbHistorialIncapacidades.hinc_Doctor,
                                                                            tbHistorialIncapacidades.hinc_Diagnostico,
                                                                            tbHistorialIncapacidades.hinc_FechaInicio,
                                                                            tbHistorialIncapacidades.hinc_FechaFin,
                                                                            1,
                                                                            DateTime.Now);
                    foreach (UDP_RRHH_tbHistorialIncapacidades_Insert_Result item in list)
                    {
                        msj = item.MensajeError + " ";
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
            return Json(msj.Substring(0, 2), JsonRequestBehavior.AllowGet);
        }


        // GET: HistorialIncapacidades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<tbHistorialIncapacidades> tbHistorialIncapacidades = null;
            try
            {
                tbHistorialIncapacidades = new List<Models.tbHistorialIncapacidades> { };
                tbHistorialIncapacidades = db.tbHistorialIncapacidades.Where(x => x.emp_Id == id).Include(t => t.tbUsuario).Include(t => t.tbUsuario1).ToList();

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return HttpNotFound();
            }
            Session["id"] = id;
            var vacaciones = new tbHistorialIncapacidades();
            foreach (var item in tbHistorialIncapacidades)
            {
                vacaciones = new tbHistorialIncapacidades
                {
                    hinc_Id = item.hinc_Id,
                    //emp_Id = item.tbEmpleados.tbPersonas.per_Nombres,
                    //ticn_Descripcion = item.tbTipoIncapacidades.ticn_Descripcion,
                    hinc_Dias = item.hinc_Dias,
                    hinc_CentroMedico = item.hinc_CentroMedico,
                    hinc_Diagnostico = item.hinc_Diagnostico,
                    hinc_FechaInicio = item.hinc_FechaInicio,
                    hinc_FechaFin = item.hinc_FechaFin,
                    hinc_UsuarioCrea = item.hinc_UsuarioCrea,
                    hinc_FechaCrea = item.hinc_FechaCrea,
                    hinc_FechaModifica = item.hinc_FechaModifica,
                    //tbUsuario = item.tbUsuario,
                    //tbUsuario1 = item.tbUsuario1
                };

            }
            return Json(vacaciones, JsonRequestBehavior.AllowGet);
        }



        // GET: HistorialIncapacidades/Delete/5
        [HttpPost]
        public ActionResult Delete(tbHistorialIncapacidades tbHistorialIncapacidades)
        {
            string msj = "";
            if (tbHistorialIncapacidades.hinc_Id != 0 && tbHistorialIncapacidades.hinc_RazonInactivo != "")
            {
                var Usuario = (tbUsuario)Session["Usuario"];
                try
                {
                    var list = db.UDP_RRHH_tbHistorialIncapacidades_Delete(tbHistorialIncapacidades.hinc_Id, tbHistorialIncapacidades.hinc_RazonInactivo, 1, DateTime.Now);
                    foreach (UDP_RRHH_tbHistorialIncapacidades_Delete_Result item in list)
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
        // POST: HistorialIncapacidades/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    tbHistorialIncapacidades tbHistorialIncapacidades = db.tbHistorialIncapacidades.Find(id);
        //    db.tbHistorialIncapacidades.Remove(tbHistorialIncapacidades);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
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
