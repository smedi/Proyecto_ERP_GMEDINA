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
    public class InstitucionesFinancierasController : Controller
    {
        private ERP_GMEDINAEntities db = new ERP_GMEDINAEntities();

        // GET: InstitucionesFinancieras
        public ActionResult Index()
        {
            var tbInstitucionesFinancieras = db.tbInstitucionesFinancieras.Include(t => t.tbUsuario).Include(t => t.tbUsuario1);
            return View(tbInstitucionesFinancieras.ToList());
        }

        // GET: InstitucionesFinancieras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbInstitucionesFinancieras tbInstitucionesFinancieras = db.tbInstitucionesFinancieras.Find(id);
            if (tbInstitucionesFinancieras == null)
            {
                return HttpNotFound();
            }
            return View(tbInstitucionesFinancieras);
        }

        // GET: InstitucionesFinancieras/Create
        public ActionResult Create()
        {
            ViewBag.insf_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario");
            ViewBag.insf_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario");
            return View();
        }

        // POST: InstitucionesFinancieras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "insf_IdInstitucionFinanciera,insf_DescInstitucionFinanc,insf_Contacto,insf_Telefono,insf_Correo,insf_UsuarioCrea,insf_FechaCrea,insf_UsuarioModifica,insf_FechaModifica,insf_Activo")] tbInstitucionesFinancieras tbInstitucionesFinancieras)
        {
            //LLENAR LA DATA DE AUDITORIA, DE NO HACERLO EL MODELO NO SERÍA VÁLIDO Y SIEMPRE CAERÍA EN EL CATCH
            tbInstitucionesFinancieras.insf_UsuarioCrea = 1;
            tbInstitucionesFinancieras.insf_FechaCrea = DateTime.Now;
            tbInstitucionesFinancieras.insf_Activo = true;
            //VARIABLE PARA ALMACENAR EL RESULTADO DEL PROCESO Y ENVIARLO AL LADO DEL CLIENTE
            string response = String.Empty;
            IEnumerable<object> listInstitucionesFinancieras = null;
            string MensajeError = "";
            //VALIDAR SI EL MODELO ES VÁLIDO
            if (ModelState.IsValid)
            {
                try
                {
                    //EJECUTAR PROCEDIMIENTO ALMACENADO
                    listInstitucionesFinancieras = db.UDP_Plani_tbInstitucionesFinancieras_Insert(tbInstitucionesFinancieras.insf_DescInstitucionFinanc,
                                                                                            tbInstitucionesFinancieras.insf_Contacto, 
                                                                                            tbInstitucionesFinancieras.insf_Telefono,
                                                                                            tbInstitucionesFinancieras.insf_Correo,
                                                                                            tbInstitucionesFinancieras.insf_UsuarioCrea,
                                                                                            tbInstitucionesFinancieras.insf_FechaCrea,
                                                                                            tbInstitucionesFinancieras.insf_Activo);
                    //RECORRER EL TIPO COMPLEJO DEL PROCEDIMIENTO ALMACENADO PARA EVALUAR EL RESULTADO DEL SP
                    foreach (UDP_Plani_tbInstitucionesFinancieras_Insert1_Result Resultado in listInstitucionesFinancieras)
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
                return RedirectToAction("Index");

            }
            else
            {
                //SI EL MODELO NO ES VÁLIDO, IGUALAMOS LA VARIABLE "RESPONSE" A ERROR PARA VALIDARLO EN EL CLIENTE
                response = "error";
            }

            // ViewBag.insf_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbInstitucionesFinancieras.insf_UsuarioCrea);
            // ViewBag.insf_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbInstitucionesFinancieras.insf_UsuarioModifica);
            // return View(tbInstitucionesFinancieras);
            return View(tbInstitucionesFinancieras);
        }

        // GET: InstitucionesFinancieras/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbInstitucionesFinancieras tbInstitucionesFinancieras = db.tbInstitucionesFinancieras.Find(id);
            if (tbInstitucionesFinancieras == null)
            {
                return HttpNotFound();
            }
            ViewBag.insf_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbInstitucionesFinancieras.insf_UsuarioCrea);
            ViewBag.insf_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbInstitucionesFinancieras.insf_UsuarioModifica);
            return View(tbInstitucionesFinancieras);
        }

        // POST: InstitucionesFinancieras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "insf_IdInstitucionFinanciera,insf_DescInstitucionFinanc,insf_Contacto,insf_Telefono,insf_Correo,insf_UsuarioCrea,insf_FechaCrea,insf_UsuarioModifica,insf_FechaModifica,insf_Activo")] tbInstitucionesFinancieras tbInstitucionesFinancieras)
        {
            tbInstitucionesFinancieras.insf_UsuarioModifica = 1;
            tbInstitucionesFinancieras.insf_FechaModifica = DateTime.Now;
            //--listInstitucionesFinancieras = listInFs
            IEnumerable<object> listInFs = null;
            string MensajeError = "";
            if (ModelState.IsValid)
            {
                try
                {
                    listInFs = db.UDP_Plani_tbInstitucionesFinancieras_Update(
                                                        tbInstitucionesFinancieras.insf_IdInstitucionFinanciera,
                                                        tbInstitucionesFinancieras.insf_DescInstitucionFinanc,
                                                        tbInstitucionesFinancieras.insf_Contacto,
                                                        tbInstitucionesFinancieras.insf_Telefono,
                                                        tbInstitucionesFinancieras.insf_Correo,
                                                        tbInstitucionesFinancieras.insf_UsuarioModifica,
                                                        tbInstitucionesFinancieras.insf_FechaModifica,
                                                        tbInstitucionesFinancieras.insf_Activo);

                    foreach (UDP_Plani_tbInstitucionesFinancieras_Update1_Result Resultado in listInFs)
                        MensajeError = Resultado.MensajeError;

                    if (MensajeError.StartsWith("-1"))
                    {
                        ModelState.AddModelError("", "No se pudo mdoficar el registro, contacte al administrador");
                        return View(tbInstitucionesFinancieras);
                    }
                }
                catch (Exception Ex)
                {
                    Ex.Message.ToString();
                }
                return RedirectToAction("Index");
            }
            //ViewBag.insf_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbInstitucionesFinancieras.insf_UsuarioCrea);
            //ViewBag.insf_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbInstitucionesFinancieras.insf_UsuarioModifica);
            return View(tbInstitucionesFinancieras);
        }

        // GET: InstitucionesFinancieras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbInstitucionesFinancieras tbInstitucionesFinancieras = db.tbInstitucionesFinancieras.Find(id);
            if (tbInstitucionesFinancieras == null)
            {
                return HttpNotFound();
            }
            return View(tbInstitucionesFinancieras);
        }

        // POST: InstitucionesFinancieras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbInstitucionesFinancieras tbInstitucionesFinancieras = db.tbInstitucionesFinancieras.Find(id);
            db.tbInstitucionesFinancieras.Remove(tbInstitucionesFinancieras);
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
