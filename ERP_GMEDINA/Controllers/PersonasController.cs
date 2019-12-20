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
            Session["Usuario"] = new tbUsuario { usu_Id = 1 };
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
        public ActionResult llenarDropDowlistNacionalidades()
        {
            var Nacionalidades = new List<object> { };
            using (db = new ERP_GMEDINAEntities())
            {
                try
                {
                    Nacionalidades.Add(new
                    {
                        Id = 0,
                        Descripcion = "**Seleccione una opción**"
                    });
                    Nacionalidades.AddRange(db.tbNacionalidades
                    .Select(tabla => new { id = tabla.nac_Id, Descripcion = tabla.nac_Descripcion })
                    .ToList());
                }
                catch
                {
                    return Json("-2", 0);
                }

            }
            var result = new Dictionary<string, object>();
            result.Add("nac_Id", Nacionalidades);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Competencias
        public ActionResult llenarDropDowlistCompetencias()
        {
            var Competencias = new List<object> { };
            using (db = new ERP_GMEDINAEntities())
            {
                try
                {
                    Competencias.Add(new
                    {
                        Id = 0,
                        Descripcion = "**Seleccione una opción**"
                    });
                    Competencias.AddRange(db.tbCompetencias
                    .Select(tabla => new { Id = tabla.comp_Id, Descripcion = tabla.comp_Descripcion})
                    .ToList());
                }
                catch
                {
                    return Json("-2", 0);
                }

            }
            var result = new Dictionary<string, object>();
            result.Add("comp_Id", Competencias);
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
            Session["Usuario"] = new tbUsuario { usu_Id = 1 };
            //Ddl Sexo
            var Sexo = new List<object> { };
                    Sexo.Add(new
                    {
                        Id = 0,
                        Descripcion = "**Seleccione una opción**"
                    });
                    Sexo.Add(new
                    {
                        Id = "F",
                        Descripcion = "Femenino"
                    });
                    Sexo.Add(new
                    {
                        Id = "M",
                        Descripcion = "Masculino"
                    });
            ViewBag.sexo = new SelectList(Sexo, "Id", "Descripcion");
            //Ddl EstadoCivil
            var EstadoCivil = new List<object> { };
            EstadoCivil.Add(new
            {  Id = 0,   Descripcion = "**Seleccione una opción**",
            });
            EstadoCivil.Add(new
            {  Id = "S", Descripcion = "Soltero"
            });
            EstadoCivil.Add(new
            {  Id = "C", Descripcion = "Casado"
            });
            ViewBag.EstadoCivil = new SelectList(EstadoCivil, "Id", "Descripcion");
            //ddl TipoSangre
            var TipoSangre = new List<object> { };
            TipoSangre.Add(new
            {Id = 0, Descripcion = "**Seleccione una opción**",
            });
            TipoSangre.Add(new
            { Id = "O+", Descripcion = "O+"
            });
            TipoSangre.Add(new
            { Id = "O-",Descripcion = "O-"
            });
            TipoSangre.Add(new
            {Id = "A",Descripcion = "A+"
            });
            TipoSangre.Add(new
            {Id = "A-",Descripcion = "A-"
            });
            ViewBag.TipoSangre = new SelectList(TipoSangre, "Id", "Descripcion");
            
            List<V_tbNacionalidades> Nacionalidades = new List<V_tbNacionalidades> { };
            ViewBag.nac_Id = new SelectList(Nacionalidades, "nac_Id", "nac_Descripcion");

            List<tbCompetencias> Competencias = new List<tbCompetencias> { };
            ViewBag.comp_Id = new SelectList(Competencias, "comp_Id", "comp_Descripcion");
            return View();
        }
        // POST: Areas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(tbPersonas tbPersonas)
        {
            //declaramos la variable de coneccion solo para recuperar los datos necesarios.
            //posteriormente es destruida.
            string result = "";
            
            using (db = new ERP_GMEDINAEntities())
            {
                var id = 1;
                var Usuario = (tbUsuario)Session["Usuario"];
                //en esta area ingresamos el registro con el procedimiento almacenado
                try
                {
                    var lista = db.rrhh_tbPersonas_Insert(tbPersonas.per_Identidad,tbPersonas.per_Nombres, tbPersonas.per_Apellidos, tbPersonas.per_FechaNacimiento, tbPersonas.per_Sexo, tbPersonas.per_Edad, tbPersonas.nac_Id, tbPersonas.per_Direccion, tbPersonas.per_Telefono, tbPersonas.per_CorreoElectronico, tbPersonas.per_EstadoCivil, tbPersonas.per_TipoSangre,id,DateTime.Now);
                    Response.Redirect("Index");
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

            List<tbPersonas> tbPersonas = null;
            try
            {
                tbPersonas = new List<Models.tbPersonas> { };
                tbPersonas = db.tbPersonas.Where(x => x.per_Id == id).Include(t => t.tbUsuario).Include(t => t.tbUsuario1).ToList();

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return HttpNotFound();
            }
            Session["id"] = id;
            var Personas = new tbPersonas();
            foreach (var item in tbPersonas)
            {
                Personas = new tbPersonas()
                {
                    per_Id = item.per_Id,
                    per_Identidad = item.per_Identidad
                    //per_Nombres = tbPersonas.per_Nombres,
                    //per_Apellidos = tbPersonas.per_Apellidos,
                    //per_Sexo = tbPersonas.per_Sexo,
                    //per_Edad = tbPersonas.per_Edad,
                    //per_Direccion = tbPersonas.per_Direccion,
                    //per_Estado = tbPersonas.per_Estado,
                    //per_RazonInactivo = tbPersonas.per_RazonInactivo,
                    //per_UsuarioCrea = tbPersonas.per_UsuarioCrea,
                    //per_FechaCrea = tbPersonas.per_FechaCrea,
                    //per_UsuarioModifica = tbPersonas.per_UsuarioModifica,
                    //per_FechaModifica = tbPersonas.per_FechaModifica,
                    //tbUsuario = new tbUsuario { usu_NombreUsuario = IsNull(tbPersonas.tbUsuario).usu_NombreUsuario },
                    //tbUsuario1 = new tbUsuario { usu_NombreUsuario = IsNull(tbPersonas.tbUsuario1).usu_NombreUsuario }
                };

            }
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
