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
    public class PersonasController : Controller
    {
        private ERP_GMEDINAEntities db = null;

        // GET: Areas
        public ActionResult Index()
        {
            
            var tbPersonas = new List<tbPersonas> { };
            return View(tbPersonas);
        }
        public ActionResult llenarTabla()
        {
            try
            {
                //declaramos la variable de coneccion solo para recuperar los datos necesarios.
                //posteriormente es destruida.
                using (db = new ERP_GMEDINAEntities())
                {
                    var tbPersonas = db.tbPersonas
                        .Select(
                        p => new
                        {
                            Id = p.per_Id,
                            Identidad = p.per_Identidad,
                            Nombre = p.per_Nombres + " " + p.per_Apellidos,
                            CorreoElectronico = p.per_CorreoElectronico
                        })
                        .ToList();
                    return Json(tbPersonas, JsonRequestBehavior.AllowGet);
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
            
            List<V_tbPersonas> lista = new List<V_tbPersonas> { };
            using (db = new ERP_GMEDINAEntities())
            {
                try
                {
                    lista = db.V_tbPersonas.Where(x => x.per_Id == id).ToList();
                    if(lista.Count == 0)
                    {
                        lista.Add(new V_tbPersonas { per_Id = id,Relacion_Id =0,Descripcion ="",Relacion = "" });
                    }
                }
                catch(Exception ex)
                {
                    ex.Message.ToString();
                }
            }
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        public ActionResult llenarDropDowlist()
        {
            var Sucursales = new List<object> { };
            using (db = new ERP_GMEDINAEntities())
            {
                try
                {
                    Sucursales.Add(new
                    {
                        Id = 0,
                        Descripcion = "**Seleccione una opción**"
                    });
                    Sucursales.AddRange(db.tbSucursales
                    .Select(tabla => new { Id = tabla.suc_Id, Descripcion = tabla.suc_Descripcion })
                    .ToList());
                }
                catch
                {
                    return Json("-2", 0);
                }

            }
            var result = new Dictionary<string, object>();
            result.Add("Sucursales", Sucursales);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: Areas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //declaramos la variable de coneccion solo para recuperar los datos necesarios.
            //posteriormente es destruida.
            tbAreas tbAreas = null;
            using (db = new ERP_GMEDINAEntities())
            {
                try
                {
                    tbAreas = db.tbAreas.Find(id);
                }
                catch
                {

                }
            }
            if (tbAreas == null)
            {
                return HttpNotFound();
            }
            return View(tbAreas);
        }
        // GET: Areas/Create
        public ActionResult Create()
        {
            //List<tbNacionalidades> list = new List<tbNacionalidades>();

            //for(int i =0; i<3; i++)
            //{

            //    tbNacionalidades Nacio = new tbNacionalidades();
            //    Nacio.nac_Id = 1+i;
            //    Nacio.nac_Descripcion = "Hola"+i;
            //    list.Add(Nacio);
            //}
            //var lista = db.tbNacionalidades.Select(x => new
            //{
            //    nac_Id = x.nac_Id,
            //    nac_Descripcion = x.nac_Descripcion
            //}).ToList(); ; 

            //declaramos la variable de coneccion solo para recuperar los datos necesarios.
            //posteriormente es destruida.
            //ViewBag.nac_Id = new SelectList(db.tbNacionalidades, "nac_Id", "nac_Descripcion");
            List<tbCompetencias> Competencias = new List<tbCompetencias> { };
            ViewBag.comp_Id = new SelectList(Competencias, "comp_Id", "comp_Descripcion");

            List<tbHabilidades> Habilidades = new List<tbHabilidades> { };
            ViewBag.habi = new SelectList(Habilidades, "habi_Id", "habi_Descripcion");

            //List<tbIdiomas> Idiomas = new List<tbIdiomas> { };
            //ViewBag.idi_Id = new SelectList(Idiomas, "idi_Id", "idi_Descripcion");

            //List<tbTitulos> Titulod = new List<tbTitulos> { };
            //ViewBag.titu_Id = new SelectList(Titulod, "titu_Id", "titu_Descripcion");

            //List<tbRequerimientosEspeciales> RequerimientosEspeciales = new List<tbRequerimientosEspeciales> { };
            //ViewBag.resp_Id = new SelectList(RequerimientosEspeciales, "resp_Id", "resp_Descripcion");
            //List<tbSucursales> Sucursales = new List<tbSucursales> { };
            //ViewBag.suc_Id = new SelectList(Sucursales, "suc_Id", "suc_Descripcion");
            return View();
        }
        // POST: Areas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(tbAreas tbAreas, tbDepartamentos[] tbDepartamentos)
        {
            //declaramos la variable de coneccion solo para recuperar los datos necesarios.
            //posteriormente es destruida.
            string result = "";
            using (db = new ERP_GMEDINAEntities())
            {
                //en esta area ingresamos el registro con el procedimiento almacenado
                try
                {

                }
                catch
                {
                    result = "-2";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: Areas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tbPersonas tbPersonas = null;
            try
            {
                tbPersonas = db.tbPersonas.Find(id);
                if (tbPersonas == null || !tbPersonas.per_Estado)
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
            var Personas = new tbPersonas
            {
                per_Id = tbPersonas.per_Id,
                per_Identidad = tbPersonas.per_Identidad,
                per_Nombres = tbPersonas.per_Nombres,
                per_Apellidos = tbPersonas.per_Apellidos,
                per_Sexo = tbPersonas.per_Sexo,
                per_Edad = tbPersonas.per_Edad,
                per_Direccion = tbPersonas.per_Direccion,
                per_Estado = tbPersonas.per_Estado,
                per_RazonInactivo = tbPersonas.per_RazonInactivo,
                per_UsuarioCrea = tbPersonas.per_UsuarioCrea,
                per_FechaCrea = tbPersonas.per_FechaCrea,
                per_UsuarioModifica = tbPersonas.per_UsuarioModifica,
                per_FechaModifica = tbPersonas.per_FechaModifica,
                tbUsuario = new tbUsuario { usu_NombreUsuario = IsNull(tbPersonas.tbUsuario).usu_NombreUsuario },
                tbUsuario1 = new tbUsuario { usu_NombreUsuario = IsNull(tbPersonas.tbUsuario1).usu_NombreUsuario }
            };
            return Json(Personas, JsonRequestBehavior.AllowGet);
        }
        // POST: Areas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "area_Id,car_Id,suc_Id,area_Descripcion,area_Estado,area_Razoninactivo,area_Usuariocrea,area_Fechacrea,area_Usuariomodifica,area_Fechamodifica")] tbAreas tbAreas)
        {
            //declaramos la variable de coneccion solo para recuperar los datos necesarios.
            //posteriormente es destruida.
            string result = "";
            using (db = new ERP_GMEDINAEntities())
            {
                try
                {
                    //en esta area actualizamos el registro con el procedimiento almacenado
                }
                catch
                {
                    result = "-2";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // POST: Areas/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            //declaramos la variable de coneccion solo para recuperar los datos necesarios.
            //posteriormente es destruida.
            string result = "";
            using (db = new ERP_GMEDINAEntities())
            {
                try
                {
                    //en esta area Inavilitamos el registro con el procedimiento almacenado
                }
                catch
                {
                    result = "-2";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected tbUsuario IsNull(tbUsuario valor)
        {
            if (valor != null)
            {
                return valor;
            }
            else
            {
                return new tbUsuario { usu_NombreUsuario = "" };
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && db != null)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
