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
            var tbDeduccionAFP = db.tbDeduccionAFP.Where(t => t.dafp_Activo == true).Include(t => t.tbAFP).Include(t => t.tbEmpleados);
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
                                           per_Nombres = t.tbEmpleados.tbPersonas.per_Nombres,
                                           per_Apellidos = t.tbEmpleados.tbPersonas.per_Apellidos,
                                           emp_CuentaBancaria = t.tbEmpleados.emp_CuentaBancaria,
                                           dafp_AporteLps = t.dafp_AporteLps,
                                           afp_Id = t.afp_Id,
                                           afp_Descripcion = t.tbAFP.afp_Descripcion,
                                           emp_Id = t.emp_Id,
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

        // GET: DeduccionAFP/Create
        public ActionResult Create()
        {
            ViewBag.afp_Id = new SelectList(db.tbAFP, "afp_Id", "afp_Descripcion");
            ViewBag.emp_Id = new SelectList(db.tbPersonas, "emp_Id", "per_Nombres" + ' ' + "per_Apellidos", db.tbEmpleados.Include(d => d.emp_Id));
            return View();
        }

        // POST: DeduccionAFP/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "dafp_AporteLps,afp_Id,emp_Id,dafp_UsuarioCrea,dafp_FechaCrea")] tbDeduccionAFP tbDeduccionAFP)
        {
            //LLENAR LA DATA DE AUDITORIA, DE NO HACERLO EL MODELO NO SERÍA VÁLIDO Y SIEMPRE CAERÍA EN EL CATCH
            tbDeduccionAFP.dafp_UsuarioCrea = 1;
            tbDeduccionAFP.dafp_FechaCrea = DateTime.Now;
            //VARIABLE PARA ALMACENAR EL RESULTADO DEL PROCESO Y ENVIARLO AL LADO DEL CLIENTE
            string response = String.Empty;
            IEnumerable<object> listDeduccionAFP = null;
            string MensajeError = "";
            //VALIDAR SI EL MODELO ES VÁLIDO
            if (ModelState.IsValid)
            {
                try
                {
                    //EJECUTAR PROCEDIMIENTO ALMACENADO
                    listDeduccionAFP = db.UDP_Plani_tbDeduccionAFP_Insert(tbDeduccionAFP.dafp_AporteLps,
                                                                          tbDeduccionAFP.afp_Id,
                                                                          tbDeduccionAFP.emp_Id,
                                                                          tbDeduccionAFP.dafp_UsuarioCrea,
                                                                          tbDeduccionAFP.dafp_FechaCrea);
                    //RECORRER EL TIPO COMPLEJO DEL PROCEDIMIENTO ALMACENADO PARA EVALUAR EL RESULTADO DEL SP
                    foreach (UDP_Plani_tbDeduccionAFP_Insert_Result Resultado in listDeduccionAFP)
                        MensajeError = Resultado.MensajeError;

                    if (MensajeError.StartsWith("-1"))
                    {
                        //EN CASO DE OCURRIR UN ERROR, IGUALAMOS LA VARIABLE "RESPONSE" A ERROR PARA VALIDARLO EN EL CLIENTE
                        ModelState.AddModelError("", "No se pudo ingresar el registro, contacte al administrador");
                        response = "error";
                    }

                }
                catch (Exception Ex)
                {
                    //EN CASO DE CAER EN EL CATCH, IGUALAMOS LA VARIABLE "RESPONSE" A ERROR PARA VALIDARLO EN EL CLIENTE
                    response = Ex.Message.ToString();
                }
                //SI LA EJECUCIÓN LLEGA A ESTE PUNTO SIGNIFICA QUE NO OCURRIÓ NINGÚN ERROR Y EL PROCESO FUE EXITOSO
                //IGUALAMOS LA VARIABLE "RESPONSE" A "BIEN" PARA VALIDARLO EN EL CLIENTE
                response = "bien";
            }
            else
            {
                //SI EL MODELO NO ES VÁLIDO, IGUALAMOS LA VARIABLE "RESPONSE" A ERROR PARA VALIDARLO EN EL CLIENTE
                response = "error";
            }

            //RETORNAMOS LA VARIABLE RESPONSE AL CLIENTE PARA EVALUARLA

            ViewBag.afp_Id = new SelectList(db.tbAFP, "afp_Id", "afp_Descripcion", db.tbAFP.Include(d => d.afp_Id));
            ViewBag.emp_Id = new SelectList(db.tbPersonas, "emp_Id", "per_Nombres" + ' ' + "per_Apellidos", db.tbEmpleados.Include(d => d.emp_Id));

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        //FUNCIÓN: OBETENER LA DATA PARA LLENAR LOS DROPDOWNLIST DE EDICIÓN Y CREACIÓN
        public JsonResult EditGetEmpleadoDDL()
        {
            //OBTENER LA DATA QUE NECESITAMOS, HACIENDOLO DE ESTA FORMA SE EVITA LA EXCEPCION POR "REFERENCIAS CIRCULARES"
            var DDL =
            from DeduAFP in db.tbDeduccionAFP
            join Emp in db.tbEmpleados on DeduAFP.emp_Id equals Emp.emp_Id
            join Per in db.tbPersonas on Emp.per_Id equals Per.per_Id
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
            join AFP in db.tbAFP on DeduAFP.afp_Id equals AFP.afp_Id
            select new { Id = AFP.afp_Id, Descripcion = AFP.afp_Descripcion };
            //RETORNAR LA DATA EN FORMATO JSON AL CLIENTE 
            return Json(DDL, JsonRequestBehavior.AllowGet);
        }


        // GET: DeduccionAFP/Edit/5
        public JsonResult Edit(int? ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            tbDeduccionAFP tbDeduccionAFPJSON = db.tbDeduccionAFP.Find(ID);
            return Json(tbDeduccionAFPJSON, JsonRequestBehavior.AllowGet);
        }

        // POST: DeduccionAFP/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "dafp_Id,dafp_AporteLps,afp_Id,emp_Id,dafp_UsuarioModifica,dafp_FechaModifica")] tbDeduccionAFP tbDeduccionAFP)
        {
            //DATA DE AUDIOTIRIA DE CREACIÓN, PUESTA UNICAMENTE PARA QUE NO CAIGA EN EL CATCH
            //EN EL PROCEDIMIENTO ALMACENADO, ESTOS DOS CAMPOS NO SE DEBEN MODIFICAR
            /*
            tbDeduccionAFP.cde_UsuarioCrea = 1;
            tbDeduccionAFP.cde_FechaCrea = DateTime.Now;
            */
            //LLENAR DATA DE AUDITORIA
            tbDeduccionAFP.dafp_UsuarioModifica = 1;
            tbDeduccionAFP.dafp_FechaModifica = DateTime.Now;
            //VARIABLE DONDE SE ALMACENARA EL RESULTADO DEL PROCESO
            string response = String.Empty;
            IEnumerable<object> listDeduccionAFP = null;
            string MensajeError = "";
            //VALIDAR SI EL MODELO ES VÁLIDO
            if (ModelState.IsValid)
            {
                try
                {
                    //EJECUTAR PROCEDIMIENTO ALMACENADO
                    listDeduccionAFP = db.UDP_Plani_tbDeduccionAFP_Update(tbDeduccionAFP.dafp_Id,
                                                                          tbDeduccionAFP.dafp_AporteLps,
                                                                          tbDeduccionAFP.afp_Id,
                                                                          tbDeduccionAFP.emp_Id,
                                                                          tbDeduccionAFP.dafp_UsuarioModifica,
                                                                          tbDeduccionAFP.dafp_FechaModifica);
                    //RECORRER EL TIPO COMPLEJO DEL PROCEDIMIENTO ALMACENADO PARA EVALUAR EL RESULTADO DEL SP
                    foreach (UDP_Plani_tbCatalogoDeDeducciones_Update_Result Resultado in listDeduccionAFP)
                        MensajeError = Resultado.MensajeError;

                    if (MensajeError.StartsWith("-1"))
                    {
                        //EN CASO DE OCURRIR UN ERROR, IGUALAMOS LA VARIABLE "RESPONSE" A ERROR PARA VALIDARLO EN EL CLIENTE
                        ModelState.AddModelError("", "No se pudo ingresar el registro, contacte al administrador");
                        response = "error";
                    }

                }
                catch (Exception Ex)
                {
                    //EN CASO DE CAER EN EL CATCH, IGUALAMOS LA VARIABLE "RESPONSE" A ERROR PARA VALIDARLO EN EL CLIENTE
                    response = Ex.Message.ToString();
                }
                //SI LA EJECUCIÓN LLEGA A ESTE PUNTO SIGNIFICA QUE NO OCURRIÓ NINGÚN ERROR Y EL PROCESO FUE EXITOSO
                //IGUALAMOS LA VARIABLE "RESPONSE" A "BIEN" PARA VALIDARLO EN EL CLIENTE
                response = "bien";
            }
            else
            {
                // SI EL MODELO NO ES CORRECTO, RETORNAR ERROR
                ModelState.AddModelError("", "No se pudo modificar el registro, contacte al administrador.");
                response = "error";
            }

            //RETORNAR MENSAJE AL LADO DEL CLIENTE
            
            ViewBag.afp_Id = new SelectList(db.tbAFP, "afp_Id", "afp_Descripcion", db.tbAFP.Include(d => d.afp_Id));
            ViewBag.emp_Id = new SelectList(db.tbPersonas, "emp_Id", "per_Nombres" + ' ' + "per_Apellidos", db.tbEmpleados.Include(d => d.emp_Id));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        // GET: DeduccionAFP/Details/5
        public JsonResult Details(int? ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            tbDeduccionAFP tbDeduccionAFPJSON = db.tbDeduccionAFP.Find(ID);
            return Json(tbDeduccionAFPJSON, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Inactivar(int? ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            tbDeduccionAFP tbDeduccionAFPJSON = db.tbDeduccionAFP.Find(ID);
            return Json(tbDeduccionAFPJSON, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inactivar([Bind(Include = "cde_IdDeducciones,cde_UsuarioModifica,cde_FechaModifica")] tbDeduccionAFP tbDeduccionAFP)
        {
            //DATA DE AUDIOTIRIA DE CREACIÓN, PUESTA UNICAMENTE PARA QUE NO CAIGA EN EL CATCH
            //EN EL PROCEDIMIENTO ALMACENADO, ESTOS DOS CAMPOS NO SE DEBEN MODIFICAR
            //tbCatalogoDeDeducciones.cde_UsuarioCrea = 1;
            //tbCatalogoDeDeducciones.cde_FechaCrea = DateTime.Now;


            //LLENAR DATA DE AUDITORIA
            tbDeduccionAFP.dafp_UsuarioModifica = 1;
            tbDeduccionAFP.dafp_FechaModifica = DateTime.Now;
            //VARIABLE DONDE SE ALMACENARA EL RESULTADO DEL PROCESO
            string response = String.Empty;
            IEnumerable<object> listDeduccionAFP = null;
            string MensajeError = "";
            //VALIDAR SI EL MODELO ES VÁLIDO
            if (ModelState.IsValid)
            {
                try
                {
                    //EJECUTAR PROCEDIMIENTO ALMACENADO
                    listDeduccionAFP = db.UDP_Plani_tbDeduccionAFP_Inactivar(tbDeduccionAFP.dafp_Id,
                                                                                               tbDeduccionAFP.dafp_UsuarioModifica,
                                                                                               tbDeduccionAFP.dafp_FechaModifica);
                    //RECORRER EL TIPO COMPLEJO DEL PROCEDIMIENTO ALMACENADO PARA EVALUAR EL RESULTADO DEL SP
                    foreach (UDP_Plani_tbDeduccionAFP_Inactivar_Result Resultado in listDeduccionAFP)
                        MensajeError = Resultado.MensajeError;

                    if (MensajeError.StartsWith("-1"))
                    {
                        //EN CASO DE OCURRIR UN ERROR, IGUALAMOS LA VARIABLE "RESPONSE" A ERROR PARA VALIDARLO EN EL CLIENTE
                        ModelState.AddModelError("", "No se pudo inactivar el registro, contacte al administrador");
                        response = "error";
                    }

                }
                catch (Exception Ex)
                {
                    //EN CASO DE CAER EN EL CATCH, IGUALAMOS LA VARIABLE "RESPONSE" A ERROR PARA VALIDARLO EN EL CLIENTE
                    response = Ex.Message.ToString();
                }
                //SI LA EJECUCIÓN LLEGA A ESTE PUNTO SIGNIFICA QUE NO OCURRIÓ NINGÚN ERROR Y EL PROCESO FUE EXITOSO
                //IGUALAMOS LA VARIABLE "RESPONSE" A "BIEN" PARA VALIDARLO EN EL CLIENTE
                response = "bien";
            }
            else
            {
                // SI EL MODELO NO ES CORRECTO, RETORNAR ERROR
                ModelState.AddModelError("", "No se pudo inactivar el registro, contacte al administrador.");
                response = "error";
            }
            //ViewBag.tde_IdTipoDedu = new SelectList(db.tbTipoDeduccion, "tde_IdTipoDedu", "tde_Descripcion", tbCatalogoDeDeducciones.tde_IdTipoDedu);

            //RETORNAR MENSAJE AL LADO DEL CLIENTE
            return Json(response, JsonRequestBehavior.AllowGet);
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
