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
            var tbDeduccionesExtraordinarias = db.tbDeduccionesExtraordinarias.Where(t => t.dex_Activo == true).Include(t => t.tbUsuario).Include(t => t.tbUsuario1).Include(t => t.tbCatalogoDeDeducciones).Include(t => t.tbEquipoEmpleados);
            return View(tbDeduccionesExtraordinarias.ToList());
        }

        // GET: OBTENER LA DATA Y ENVIARLA A LA VISTA EN FORMATO JSON
        public ActionResult GetData()
        {
            //SI SE LLEGA A DAR PROBLEMAS DE "REFERENCIAS CIRCULARES", OBTENER LA DATA DE ESTA FORMA
            //SELECCIONANDO UNO POR UNO LOS CAMPOS QUE NECESITAREMOS
            //DE LO CONTRARIO, HACERLO DE LA FORMA CONVENCIONAL (EJEMPLO: db.tbCatalogoDeDeducciones.ToList(); )

            var tbDeduccionesExtraordinariasD = db.tbDeduccionesExtraordinarias
                .Select(d => new
                {
                    dex_IdDeduccionesExtra = d.dex_IdDeduccionesExtra,
                    eqem_Id = d.eqem_Id,
                    cde_IdDeducciones = d.cde_IdDeducciones,
                    dex_MontoInicial = d.dex_MontoInicial,
                    dex_MontoRestante = d.dex_MontoRestante,
                    dex_ObservacionesComentarios = d.dex_ObservacionesComentarios,
                    cde_DescripcionDeduccion = d.tbCatalogoDeDeducciones.cde_DescripcionDeduccion,
                    dex_Cuota = d.dex_Cuota,
                    dex_Activo = d.dex_Activo,
                    dex_UsuarioCrea = d.dex_UsuarioCrea,
                    dex_FechaCrea = d.dex_FechaCrea,
                    dex_UsuarioModifica = d.dex_UsuarioModifica,
                    dex_FechaModifica = d.dex_FechaModifica
                }).Where(d => d.dex_Activo == true)
                .ToList();
            //RETORNAR JSON AL LADO DEL CLIENTE
            return new JsonResult { Data = tbDeduccionesExtraordinariasD, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // GET: DeduccionesExtraordinarias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            V_DeduccionesExtraordinarias_Detalles oDeduccionesExtraordinarias_Detalles = db.V_DeduccionesExtraordinarias_Detalles.Where(x => x.dex_IdDeduccionesExtra == id).FirstOrDefault();
            if (oDeduccionesExtraordinarias_Detalles == null)
            {
                return HttpNotFound();
            }
            return View(oDeduccionesExtraordinarias_Detalles);
        }

        [HttpPost]
        public ActionResult DetalleDE(int? id)
        {
            if (ModelState.IsValid)
            {

                try
                {

                    var ConsultaDetalle = from pde in db.tbDeduccionesExtraordinarias
                                          join pcd in db.tbCatalogoDeDeducciones on pde.cde_IdDeducciones equals pcd.cde_IdDeducciones
                                          join rhee in db.tbEquipoEmpleados on pde.eqem_Id equals rhee.eqem_Id
                                          join rhet in db.tbEquipoTrabajo on rhee.eqtra_Id equals rhet.eqtra_Id
                                          join rhe in db.tbEmpleados on rhee.emp_Id equals rhe.emp_Id
                                          join rhp in db.tbPersonas on rhe.per_Id equals rhp.per_Id
                                          join rhc in db.tbCargos on rhe.car_Id equals rhc.car_Id
                                          join rhd in db.tbDepartamentos on rhc.car_Id equals rhd.car_Id
                                          join rha in db.tbAreas on rhd.area_Id equals rha.area_Id

                                          where
                                          pde.dex_Activo == true &&
                                          pde.dex_IdDeduccionesExtra == id

                                          select new ViewModelDeduccionesExtraordinarias
                                          {
                                              dex_IdDeduccionesExtra = pde.dex_IdDeduccionesExtra,
                                              eqem_Id = pde.eqem_Id,
                                              per_Empleado = rhp.per_Nombres + ' ' + rhp.per_Apellidos,
                                              car_Cargo = rhc.car_Descripcion,
                                              depto_Departamento = rhd.depto_Descripcion,
                                              area_Area = rha.area_Descripcion,
                                              dex_ObservacionesComentarios = pde.dex_ObservacionesComentarios,
                                              eqtra_Id = rhet.eqtra_Id,
                                              eqtra_Codigo = rhet.eqtra_Codigo,
                                              eqtra_Descripcion = rhet.eqtra_Descripcion,
                                              dex_MontoInicial = pde.dex_MontoInicial,
                                              dex_MontoRestante = pde.dex_MontoRestante,
                                              dex_Cuota = pde.dex_Cuota,
                                              cde_IdDeducciones = pde.cde_IdDeducciones,
                                              cde_DescripcionDeduccion = pcd.cde_DescripcionDeduccion,
                                              dex_UsuarioCrea = pde.dex_UsuarioCrea,
                                              dex_FechaCrea = pde.dex_FechaCrea,
                                              dex_UsuarioModifica = pde.dex_UsuarioModifica,
                                              dex_FechaModifica = pde.dex_FechaModifica,
                                              dex_Activo = pde.dex_Activo
                                          };

                    ViewBag.ConsultasDetalles = ConsultaDetalle.ToList();
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }

            }
            return View();
        }

        // GET: DeduccionesExtraordinarias/Create
        public ActionResult Create()
        {

            ViewBag.cde_IdDeducciones = new SelectList(db.tbCatalogoDeDeducciones, "cde_IdDeducciones", "cde_DescripcionDeduccion");
            ViewBag.eqem_Id = new SelectList(db.V_DeduccionesExtraordinarias_EquipoEmpleado, "eqem_Id", "per_Empleado");
            return View();
        }

        // POST: DeduccionesExtraordinarias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "eqem_Id,dex_MontoInicial,dex_MontoRestante,dex_ObservacionesComentarios,cde_IdDeducciones,dex_Cuota,dex_UsuarioCrea,dex_FechaCrea")] tbDeduccionesExtraordinarias tbDeduccionesExtraordinarias)
        {
            //Para llenar los campos de auditoria
            tbDeduccionesExtraordinarias.dex_UsuarioCrea = 1;
            tbDeduccionesExtraordinarias.dex_FechaCrea = DateTime.Now;
            //Variable para enviarla al lado del Cliente
            string Response = String.Empty;
            IEnumerable<object> listDeduccionesExtraordinarias = null;
            string MensajeError = "";
            if (ModelState.IsValid)
            {
                try
                {
                    //Ejecutar Procedimiento Almacenado
                    listDeduccionesExtraordinarias = db.UDP_Plani_tbDeduccionesExtraordinarias_Insert(tbDeduccionesExtraordinarias.eqem_Id,
                                                                                                      tbDeduccionesExtraordinarias.dex_MontoInicial,
                                                                                                      tbDeduccionesExtraordinarias.dex_MontoRestante,
                                                                                                      tbDeduccionesExtraordinarias.dex_ObservacionesComentarios,
                                                                                                      tbDeduccionesExtraordinarias.cde_IdDeducciones,
                                                                                                      tbDeduccionesExtraordinarias.dex_Cuota,
                                                                                                      tbDeduccionesExtraordinarias.dex_UsuarioCrea,
                                                                                                      tbDeduccionesExtraordinarias.dex_FechaCrea);
                    //El tipo complejo del Procedimiento Almacenado
                    foreach (UDP_Plani_tbDeduccionesExtraordinarias_Insert_Result Resultado in listDeduccionesExtraordinarias)
                    {
                        MensajeError = Resultado.MensajeError;
                    }

                    if (MensajeError.StartsWith("-1"))
                    {
                        //En caso de un error igualamos la variable Response a "Error" para validar en el lado del Cliente
                        ModelState.AddModelError("", "No se pudo Registrar. Contacte al Administrador!");
                        Response = "Error";
                    }
                }
                catch (Exception Ex)
                {
                    Response = Ex.Message.ToString();
                }
                //Si llega aqui significa que todo salio correctamente. Solo igualamos Response a "Exito" para validar en el lado del Cliente
                Response = "Exito";
                return RedirectToAction("Index");

            }
            else
            {
                //Si el modelo no es valido. Igualamos Response a "Error" para validar en el lado del Cliente
                Response = "Error";
            }

            ViewBag.cde_IdDeducciones = new SelectList(db.tbCatalogoDeDeducciones, "cde_IdDeducciones", "cde_DescripcionDeduccion", tbDeduccionesExtraordinarias.cde_IdDeducciones);
            ViewBag.eqem_Id = new SelectList(db.tbEquipoEmpleados, "eqem_Id", "per_Empleado", db.V_DeduccionesExtraordinarias_EquipoEmpleado.Include(d => d.per_Empleado));
            return Json(Response, JsonRequestBehavior.AllowGet);

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

            ViewBag.cde_IdDeducciones = new SelectList(db.tbCatalogoDeDeducciones, "cde_IdDeducciones", "cde_DescripcionDeduccion", tbDeduccionesExtraordinarias.cde_IdDeducciones);

            //Aqui iria la Vista donde trae al empleado según su Id
            ViewBag.eqem_Id = new SelectList(db.V_DeduccionesExtraordinarias_EquipoEmpleado, "eqem_Id", "per_Empleado");
            return View(tbDeduccionesExtraordinarias);
        }

        // POST: DeduccionesExtraordinarias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "dex_IdDeduccionesExtra,eqem_Id,dex_MontoInicial,dex_MontoRestante,dex_ObservacionesComentarios,cde_IdDeducciones,dex_Cuota,dex_UsuarioModifica,dex_FechaModifica")] tbDeduccionesExtraordinarias tbDeduccionesExtraordinarias)
        {
            //Para llenar los campos de auditoria
            tbDeduccionesExtraordinarias.dex_UsuarioModifica = 1;
            tbDeduccionesExtraordinarias.dex_FechaModifica = DateTime.Now;
            //Variable para enviarla al lado del Cliente
            string Response = String.Empty;
            IEnumerable<object> listDeduccionesExtraordinarias = null;
            string MensajeError = "";
            if (ModelState.IsValid)
            {
                try
                {
                    //Ejecutar Procedimiento Almacenado
                    listDeduccionesExtraordinarias = db.UDP_Plani_tbDeduccionesExtraordinarias_Update(tbDeduccionesExtraordinarias.dex_IdDeduccionesExtra,
                                                                                                      tbDeduccionesExtraordinarias.eqem_Id,
                                                                                                      tbDeduccionesExtraordinarias.dex_MontoInicial,
                                                                                                      tbDeduccionesExtraordinarias.dex_MontoRestante,
                                                                                                      tbDeduccionesExtraordinarias.dex_ObservacionesComentarios,
                                                                                                      tbDeduccionesExtraordinarias.cde_IdDeducciones,
                                                                                                      tbDeduccionesExtraordinarias.dex_Cuota,
                                                                                                      tbDeduccionesExtraordinarias.dex_UsuarioModifica,
                                                                                                      tbDeduccionesExtraordinarias.dex_FechaModifica);

                    //El tipo complejo del Procedimiento Almacenado
                    foreach (UDP_Plani_tbDeduccionesExtraordinarias_Update_Result Resultado in listDeduccionesExtraordinarias)
                    {
                        MensajeError = Resultado.MensajeError;
                    }

                    if (MensajeError.StartsWith("-1"))
                    {
                        //En caso de un error igualamos la variable Response a "Error" para validar en el lado del Cliente
                        ModelState.AddModelError("", "No se pudo Actualizar. Contacte al Administrador!");
                        Response = "Error";
                    }
                }
                catch (Exception Ex)
                {
                    Response = Ex.Message.ToString();
                }
                //Si llega aqui significa que todo salio correctamente. Solo igualamos Response a "Exito" para validar en el lado del Cliente
                Response = "Exito";
                return RedirectToAction("Index");
            }
            else
            {
                //Si el modelo no es valido. Igualamos Response a "Error" para validar en el lado del Cliente
                Response = "Error";
            }

            ViewBag.cde_IdDeducciones = new SelectList(db.tbCatalogoDeDeducciones, "cde_IdDeducciones", "cde_DescripcionDeduccion", tbDeduccionesExtraordinarias.cde_IdDeducciones);
            ViewBag.eqem_Id = new SelectList(db.tbEquipoEmpleados, "eqem_Id", "per_Nombres", db.V_DeduccionesExtraordinarias_EquipoEmpleado.Include(d => d.per_Empleado));
            return Json(Response, JsonRequestBehavior.AllowGet);

        }

        //FUNCIÓN: OBETENER LA DATA PARA LLENAR LOS DROPDOWNLIST DE EDICIÓN Y CREACIÓN
        public JsonResult EditGetDDL()
        {
            //OBTENER LA DATA QUE NECESITAMOS, HACIENDOLO DE ESTA FORMA SE EVITA LA EXCEPCION POR "REFERENCIAS CIRCULARES"
            var DDL =
            from EqEm in db.tbEquipoEmpleados
            join Empl in db.tbEmpleados on EqEm.emp_Id equals Empl.emp_Id
            join Pers in db.tbPersonas on Empl.per_Id equals Pers.per_Id
            select new { Id = EqEm.eqem_Id , Nombre = Pers.per_Nombres };
            //RETORNAR LA DATA EN FORMATO JSON AL CLIENTE 
            return Json(DDL, JsonRequestBehavior.AllowGet);
        }


        //GET: DeduccionesExtraordinarias/Inactivar
        public ActionResult Inactivar(int? ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            tbDeduccionesExtraordinarias tbDeduccionesExtraordinariasJSON = db.tbDeduccionesExtraordinarias.Find(ID);
            return Json(tbDeduccionesExtraordinariasJSON, JsonRequestBehavior.AllowGet);
        }


        //POST: DeduccionesExtraordinarias/Inactivar
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inactivar(int dex_IdDeduccionesExtra)
        {
            //Para llenar los campos de auditoria
            //tbDeduccionesExtraordinarias.dex_UsuarioModifica = 1;
            //tbDeduccionesExtraordinarias.dex_FechaModifica = DateTime.Now;
            //Variable para enviarla al lado del Cliente
            string Response = String.Empty;
            IEnumerable<object> listDeduccionesExtraordinarias = null;
            string MensajeError = "";
            if (ModelState.IsValid)
            {
                try
                {
                    //Ejecutar Procedimiento Almacenado
                    listDeduccionesExtraordinarias = db.UDP_Plani_tbDeduccionesExtraordinarias_Inactivar(dex_IdDeduccionesExtra,
                                                                                                         1,
                                                                                                         DateTime.Now);

                    //El tipo complejo del Procedimiento Almacenado
                    foreach (UDP_Plani_tbDeduccionesExtraordinarias_Inactivar_Result Resultado in listDeduccionesExtraordinarias)
                    {
                        MensajeError = Resultado.MensajeError;
                    }

                    if (MensajeError.StartsWith("-1"))
                    {
                        //En caso de un error igualamos la variable Response a "Error" para validar en el lado del Cliente
                        ModelState.AddModelError("", "No se pudo Inactivar. Contacte al Administrador!");
                        Response = "Error";
                    }
                }
                catch (Exception Ex)
                {
                    Response = Ex.Message.ToString();
                }
                //Si llega aqui significa que todo salio correctamente. Solo igualamos Response a "Exito" para validar en el lado del Cliente
                Response = "Exito";
            }
            else
            {
                //Si el modelo no es valido. Igualamos Response a "Error" para validar en el lado del Cliente
                Response = "Error";
            }

            return Json(Response, JsonRequestBehavior.AllowGet);

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
