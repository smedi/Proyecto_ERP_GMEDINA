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
    public class CatalogoDeIngresosController : Controller
    {
        private ERP_GMEDINAEntities db = new ERP_GMEDINAEntities();


        // GET: tbCatalogoDeIngresos
        public ActionResult Index()
        {
            var tbCatalogoDeIngresos = db.tbCatalogoDeIngresos.Include(t => t.tbUsuario).Include(t => t.tbUsuario1).Where(x => x.cin_Activo == true);
            return View(tbCatalogoDeIngresos.ToList());
        }

        public ActionResult GetData()
        {
            var tbCatalogoDeIngresos1 = db.tbCatalogoDeIngresos               
                        .Select(c => new {
                                           cin_IdIngresos = c.cin_IdIngreso,
                                           cin_DescripcionIngreso = c.cin_DescripcionIngreso,
                                           cin_Activo = c.cin_Activo,
                                           cin_UsuarioCrea = c.cin_UsuarioCrea,
                                           cin_FechaCrea = c.cin_FechaCrea,
                                           cin_UsuarioModifica= c.cin_UsuarioModifica,
                                           cin_FechaModifica = c.cin_FechaModifica})
                                           .Where(x => x.cin_Activo == true).ToList();
            //RETORNAR JSON AL LADO DEL CLIENTE
            return new JsonResult { Data = tbCatalogoDeIngresos1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cin_IdIngreso,cin_DescripcionIngreso,cin_UsuarioCrea,cin_FechaCrea,cin_UsuarioModifica,cin_FechaModifica,cin_Activo")] tbCatalogoDeIngresos tbCatalogoDeIngresos)
        {

            //LLENAR LA DATA DE AUDITORIA, DE NO HACERLO EL MODELO NO SERÍA VÁLIDO Y SIEMPRE CAERÍA EN EL CATCH
            tbCatalogoDeIngresos.cin_UsuarioCrea = 1;
            tbCatalogoDeIngresos.cin_FechaCrea = DateTime.Now;
            //VARIABLE PARA ALMACENAR EL RESULTADO DEL PROCESO Y ENVIARLO AL LADO DEL CLIENTE
            string response = String.Empty;
            IEnumerable<object> listCatalogoDeIngresos = null;
            string MensajeError = "";
            //VALIDAR SI EL MODELO ES VÁLIDO
            if (ModelState.IsValid)
            {
                try
                {
                    //EJECUTAR PROCEDIMIENTO ALMACENADO
                    listCatalogoDeIngresos = db.UDP_Plani_tbCatalogoDeIngresos_Insert(tbCatalogoDeIngresos.cin_DescripcionIngreso,
                                                                                         tbCatalogoDeIngresos.cin_UsuarioCrea,
                                                                                         tbCatalogoDeIngresos.cin_FechaCrea);
                    //RECORRER EL TIPO COMPLEJO DEL PROCEDIMIENTO ALMACENADO PARA EVALUAR EL RESULTADO DEL SP
                    foreach (UDP_Plani_tbCatalogoDeDeducciones_Insert_Result Resultado in listCatalogoDeIngresos)
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

            ViewBag.cin_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbCatalogoDeIngresos.cin_UsuarioCrea);
            ViewBag.cin_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbCatalogoDeIngresos.cin_UsuarioModifica);
            return View(tbCatalogoDeIngresos);
        }

        // GET: CatalogoDeIngresos/Edit/5
        public JsonResult Edit(int? ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            tbCatalogoDeIngresos tbCatalogoDeIngresosJSON = db.tbCatalogoDeIngresos.Find(ID);
            return Json(tbCatalogoDeIngresosJSON, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Details(int? ID)
        {
            var tbCatalogoDeIngresosJSON = from tbCatIngreso in db.tbCatalogoDeIngresos
                                           where tbCatIngreso.cin_Activo == true && tbCatIngreso.cin_IdIngreso == ID
                                           select new
                                           {
                                               tbCatIngreso.cin_IdIngreso,
                                               tbCatIngreso.cin_DescripcionIngreso,
                                               tbCatIngreso.cin_Activo,
                                               tbCatIngreso.cin_UsuarioCrea,
                                               UsuCrea= tbCatIngreso.tbUsuario.usu_NombreUsuario,
                                               tbCatIngreso.cin_FechaCrea,
                                               tbCatIngreso.cin_UsuarioModifica,
                                               UsuModifica= tbCatIngreso.tbUsuario1.usu_NombreUsuario,
                                               tbCatIngreso.cin_FechaModifica
                                           };


            db.Configuration.ProxyCreationEnabled = false;
            //tbCatalogoDeIngresos tbCatalogoDeIngresosJSON = db.tbCatalogoDeIngresos.Include(t => t.tbUsuario).Include(t => t.tbUsuario1).Find(ID);
            return Json(tbCatalogoDeIngresosJSON, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cin_IdIngreso,cin_DescripcionIngreso,cin_UsuarioModifica,cin_FechaModifica,cin_Activo")] tbCatalogoDeIngresos tbCatalogoDeIngresos)
        {
            //LLENAR DATA DE AUDITORIA
            tbCatalogoDeIngresos.cin_UsuarioModifica = 1;
            tbCatalogoDeIngresos.cin_FechaModifica = DateTime.Now;
            string response = String.Empty;
            IEnumerable<object> listCatalogoDeIngresos = null;
            string MensajeError = "";
            if (ModelState.IsValid)
            {
                try
                {
                    //EJECUTAR PROCEDIMIENTO ALMACENADO
                    listCatalogoDeIngresos = db.UDP_Plani_tbCatalogoDeIngresos_Update(tbCatalogoDeIngresos.cin_IdIngreso,
                                                                                            tbCatalogoDeIngresos.cin_DescripcionIngreso,
                                                                                            tbCatalogoDeIngresos.cin_UsuarioModifica,
                                                                                            tbCatalogoDeIngresos.cin_FechaModifica
                                                                                            );
                    //RECORRER EL TIPO COMPLEJO DEL PROCEDIMIENTO ALMACENADO PARA EVALUAR EL RESULTADO DEL SP
                    foreach (UDP_Plani_tbCatalogoDeIngresos_Update_Result Resultado in listCatalogoDeIngresos)
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
                    ModelState.AddModelError("", "No se pudo modificar el registro, contacte al administrador.");
                    response = "error";
                }
            }
            else
            {
                // SI EL MODELO NO ES CORRECTO, RETORNAR ERROR
                ModelState.AddModelError("", "No se pudo modificar el registro, contacte al administrador.");
                response = "error";
            }
            //RETORNAR MENSAJE AL LADO DEL CLIENTE
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Inactivar(int? ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            tbCatalogoDeIngresos tbCatalogoDeIngresosJSON = db.tbCatalogoDeIngresos.Find(ID);
            return Json(tbCatalogoDeIngresosJSON, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inactivar(int id)
        {
            IEnumerable<object> listCatalogoDeIngresos = null;
            string MensajeError = "";
            //VARIABLE DONDE SE ALMACENARA EL RESULTADO DEL PROCESO
            string response = String.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    listCatalogoDeIngresos = db.UDP_Plani_tbCatalogoDeIngresos_Inactivar(id,
                                                                                         1,
                                                                                         DateTime.Now
                                                                                            );

                    foreach (UDP_Plani_tbCatalogoDeIngresos_Inactivar_Result Resultado in listCatalogoDeIngresos)
                        MensajeError = Resultado.MensajeError;

                    if (MensajeError.StartsWith("-1"))
                    {
                        //EN CASO DE OCURRIR UN ERROR, IGUALAMOS LA VARIABLE "RESPONSE" A ERROR PARA VALIDARLO EN EL CLIENTE
                        ModelState.AddModelError("", "No se pudo actualizar el registro. Contacte al administrador.");
                        response = "error";
                    }
                }
                catch (Exception Ex)
                {
                    response = "error";
                }
                //SI LA EJECUCIÓN LLEGA A ESTE PUNTO SIGNIFICA QUE NO OCURRIÓ NINGÚN ERROR Y EL PROCESO FUE EXITOSO
                //IGUALAMOS LA VARIABLE "RESPONSE" A "BIEN" PARA VALIDARLO EN EL CLIENTE
                response = "bien";
            }
            else
            {
                //Se devuelve un mensaje de error en caso de que el modelo no sea válido
                response = "error";
            }

            return Json(JsonRequestBehavior.AllowGet);

           
        }        

        // GET: CatalogoDeDeducciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbCatalogoDeIngresos tbCatalogoDeIngresos = db.tbCatalogoDeIngresos.Find(id);
            if (tbCatalogoDeIngresos == null)
            {
                return HttpNotFound();
            }
            return View(tbCatalogoDeIngresos);
        }

        // POST: CatalogoDeDeducciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbCatalogoDeIngresos tbCatalogoDeIngresos = db.tbCatalogoDeIngresos.Find(id);
            db.tbCatalogoDeIngresos.Remove(tbCatalogoDeIngresos);
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
