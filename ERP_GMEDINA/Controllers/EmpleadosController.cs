﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERP_GMEDINA.Models;
using OfficeOpenXml;
using LinqToExcel;

//using Excel = Microsoft.Office.Interop.Excel;


namespace ERP_GMEDINA.Controllers
{
    public class EmpleadosController : Controller
    {
        private ERP_GMEDINAEntities db = new ERP_GMEDINAEntities();

        // GET: Empleados
        public ActionResult Index()
        {
            var tbEmpleados = new List < tbEmpleados >{ };
            return View(tbEmpleados);
        }
        public ActionResult llenarTabla()
        {
            try
            {
                //declaramos la variable de coneccion solo para recuperar los datos necesarios.
                //posteriormente es destruida.
                using (db = new ERP_GMEDINAEntities())
                {
                    var tbEmpleados = db.tbEmpleados
                        .Select(x=>new
                        {
                            Id=x.emp_Id,
                            per_Identidad = x.tbPersonas.per_Identidad,
                            Nombre = x.tbPersonas.per_Nombres+" "+x.tbPersonas.per_Apellidos,
                            depto_Descripcion = x.tbDepartamentos.depto_Descripcion,
                            per_Sexo = x.tbPersonas.per_Sexo,
                            per_Edad = x.tbPersonas.per_Edad,
                            per_Telefono = x.tbPersonas.per_Telefono,
                            per_CorreoElectronico = x.tbPersonas.per_CorreoElectronico
                        })
                        .ToList();
                    return Json(tbEmpleados, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json("-2", JsonRequestBehavior.AllowGet);
            }            
        }
        public ActionResult ChildRowData(int id)
        {
            List<V_Datos_Empleado> lista = new List<V_Datos_Empleado> { };
            using (db = new ERP_GMEDINAEntities())
            {
                try
                {
                    lista = db.V_Datos_Empleado.Where(x => x.emp_Id == id).ToList();
                }
                catch
                {
                }
            }
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DescargarArchivo()
        {
            string carpeta = AppDomain.CurrentDomain.BaseDirectory + "Downloadable files/";
            byte[] ArchivoBytes = System.IO.File.ReadAllBytes(carpeta + "AgregarEmpleados.xlsx");
            string NombreArchivo = "AgregarEmpleados.xlsx";
            return File(ArchivoBytes, System.Net.Mime.MediaTypeNames.Application.Octet, NombreArchivo);
        }

        public void ArchivoEmpleados()
        {
            List<ExcelEmpleados> ExcelEmpleados = new List<ExcelEmpleados>();
            ExcelEmpleados.Add(new ExcelEmpleados() { per_Identidad = "", per_Nombres = "", per_Apellidos = "", per_FechaNacimiento = "", per_Sexo = "", nac_Id = "", per_Direccion = "", per_Telefono = "", per_CorreoElectronico = "", per_EstadoCivil = "", per_TipoSangre = "", Cargo = db.UDP_RRHH_tbCargos_tbEmpleados_Select().ToList(), area_Id = "", depto_Id = "", jor_Id = "", cpla_IdPlanilla="", fpa_IdFormaPago="" });
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("ArchivoEmpleados");
            Sheet.Cells["A1:O3"].Merge = true;
            Sheet.Cells["A1:O3"].Style.Font.Size = 16;
            Sheet.Cells["A1:O3"].Style.Font.Color.SetColor(System.Drawing.Color.Red);
            Sheet.Cells["A1:O3"].Value = "FAVOR LLENAR UNICAMENTE LA INFORMACION SOLICITADA, NO CAMBIAR NINGUNA CONFIGURACION DE ESTE DOCUMENTO";
            Sheet.Cells["A1:O3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            Sheet.Cells["A1:O3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            Sheet.Cells["A1:O3"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            Sheet.Cells["A1:O3"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            Sheet.Cells["A1:O3"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            Sheet.Cells["A1:O3"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            Sheet.Cells["A1:O3"].Style.Border.Top.Color.SetColor(System.Drawing.Color.Red);
            Sheet.Cells["A1:O3"].Style.Border.Left.Color.SetColor(System.Drawing.Color.Red);
            Sheet.Cells["A1:O3"].Style.Border.Right.Color.SetColor(System.Drawing.Color.Red);
            Sheet.Cells["A1:O3"].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Red);
            Sheet.Cells["A1:O3"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            Sheet.Cells["A1:O3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#f0f3f5"));
            Sheet.Cells["A4"].Value = "Identidad";
            Sheet.Cells["B4"].Value = "Nombres";
            Sheet.Cells["C4"].Value = "Apellidos";
            Sheet.Cells["D4"].Value = "Fecha Nacimiento";
            Sheet.Cells["E4"].Value = "Sexo";
            Sheet.Cells["F4"].Value = "Nacionalidad";
            Sheet.Cells["G4"].Value = "Direccion";
            Sheet.Cells["H4"].Value = "Telefono";
            Sheet.Cells["I4"].Value = "Correo Electronico";
            Sheet.Cells["J4"].Value = "Estado Civil";
            Sheet.Cells["K4"].Value = "Tipo de Sangre";
            Sheet.Cells["L4"].Value = "Cargo";
            Sheet.Cells["M4"].Value = "Area";
            Sheet.Cells["N4"].Value = "Departamentos";
            Sheet.Cells["O4"].Value = "Jornadas";
            Sheet.Cells["P4"].Value = "Planilla";
            Sheet.Cells["Q4"].Value = "FormadePago";



            int row = 2;
            foreach (var item in ExcelEmpleados)
            {
                Sheet.Cells[string.Format("A{0}", row)].Value = item.per_Identidad;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.per_Nombres;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.per_Apellidos;
                Sheet.Cells[string.Format("D{0}", row)].Value = item.per_FechaNacimiento;
                Sheet.Cells["D5"].Style.Numberformat.Format = "yyyy-mm-dd";
                //Sheet.Cells["D5"].Formula = "=DATE(2014,10,5)";

                var per_Sexo = Sheet.DataValidations.AddListValidation("E5");
                per_Sexo.Formula.Values.Add("F");
                per_Sexo.Formula.Values.Add("M");
                //Sheet.Cells[string.Format("E{0}", row)].Value = item.per_Sexo;

                var Nacionalidades = db.tbNacionalidades
                    .Select(tabla => tabla.nac_Descripcion)
                    .ToArray();
                Sheet.Cells["MB1"].LoadFromCollection<string>(Nacionalidades.ToList<string>());
                var Nac_val = Sheet.DataValidations.AddListValidation("F5");
                Nac_val.Formula.ExcelFormula = "$MB$1:$MB$" + Nacionalidades.Length;
                Sheet.Column(340).Style.Font.Color.SetColor(System.Drawing.Color.White);

                Sheet.Cells[string.Format("G{0}", row)].Value = item.per_Direccion;
                Sheet.Cells[string.Format("H{0}", row)].Value = item.per_Telefono;
                Sheet.Cells[string.Format("I{0}", row)].Value = item.per_CorreoElectronico;

                var per_EstadoCivil = Sheet.DataValidations.AddListValidation("J5");
                per_EstadoCivil.Formula.Values.Add("S");
                per_EstadoCivil.Formula.Values.Add("C");
                per_EstadoCivil.Formula.Values.Add("D");
                per_EstadoCivil.Formula.Values.Add("V");
                //Sheet.Cells[string.Format("J{0}", row)].Value = item.per_EstadoCivil;
                Sheet.Cells[string.Format("K{0}", row)].Value = item.per_TipoSangre;

                var Cargos = db.tbCargos
                    .Select(tabla=>tabla.car_Descripcion)
                    .ToArray();
                Sheet.Cells["MA1"].LoadFromCollection<string>(Cargos.ToList<string>());
                var Car_val = Sheet.DataValidations.AddListValidation("L5");
                Car_val.Formula.ExcelFormula = "$MA$1:$MA$"+ Cargos.Length;
                Sheet.Column(339).Style.Font.Color.SetColor(System.Drawing.Color.White);
            
                //agregar parte de :"FAVOR LLENAR UNICAMENTE LA INFORMACION SOLICITADA, NO CAMBIAR NINGUNA CONFIGURACION DE ESTE DOCUMENTO"

                var Areas = db.tbAreas
                .Select(tabla => tabla.area_Descripcion)
                .ToArray();
                Sheet.Cells["MC1"].LoadFromCollection<string>(Areas.ToList<string>());
                var area_val = Sheet.DataValidations.AddListValidation("M5");
                area_val.Formula.ExcelFormula = "$MC$1:$MC$" + Areas.Length;
                Sheet.Column(341).Style.Font.Color.SetColor(System.Drawing.Color.White);


                var Dpto = db.tbDepartamentos
                .Select(tabla => tabla.depto_Descripcion)
                .ToArray();
                Sheet.Cells["MD1"].LoadFromCollection<string>(Dpto.ToList<string>());
                var Dpto_val = Sheet.DataValidations.AddListValidation("N5");
                Dpto_val.Formula.ExcelFormula = "$MD$1:$MD$" + Dpto.Length;
                Sheet.Column(342).Style.Font.Color.SetColor(System.Drawing.Color.White);

                var Jor = db.tbJornadas
               .Select(tabla => tabla.jor_Descripcion)
               .ToArray();
                Sheet.Cells["ME1"].LoadFromCollection<string>(Jor.ToList<string>());
                var jor_val = Sheet.DataValidations.AddListValidation("O5");
                jor_val.Formula.ExcelFormula = "$ME$1:$ME$" + Jor.Length;
                Sheet.Column(342).Style.Font.Color.SetColor(System.Drawing.Color.White);

                var Plani = db.tbCatalogoDePlanillas
               .Select(tabla => tabla.cpla_DescripcionPlanilla)
               .ToArray();
                Sheet.Cells["MF1"].LoadFromCollection<string>(Plani.ToList<string>());
                var Plani_val = Sheet.DataValidations.AddListValidation("P5");
                Plani_val.Formula.ExcelFormula = "$MF$1:$MF$" + Plani.Length;
                Sheet.Column(342).Style.Font.Color.SetColor(System.Drawing.Color.White);

                var Fpago = db.tbFormaPago
           .Select(tabla => tabla.fpa_Descripcion)
           .ToArray();
                Sheet.Cells["MG1"].LoadFromCollection<string>(Fpago.ToList<string>());
                var Fpago_val = Sheet.DataValidations.AddListValidation("Q5");
                Fpago_val.Formula.ExcelFormula = "$MQ$1:$MQ$" + Fpago.Length;
                Sheet.Column(342).Style.Font.Color.SetColor(System.Drawing.Color.White);


                row++;
            }
            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + $"ArchivoEmpleados_{DateTime.Now.Ticks.ToString()}.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();
        }

        [HttpPost]
        public ActionResult UploadEmpleados(tbPersonas personas,tbEmpleados empleados, HttpPostedFileBase FileUpload)
        {

            ERP_GMEDINAEntities objEntity = new ERP_GMEDINAEntities();
            string data = "";
            if (FileUpload != null)
            {
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string filename = FileUpload.FileName;

                    if (filename.EndsWith(".xlsx"))
                    {
                        string targetpath = Server.MapPath("~/Downloadable files/");
                        FileUpload.SaveAs(targetpath + filename);
                        string pathToExcelFile = targetpath + filename;

                        string sheetName = "ArchivoEmpleados";

                        var excelFile = new ExcelQueryFactory(pathToExcelFile);
                        var PERDetails = from a in excelFile.Worksheet<tbPersonas>(sheetName) select a;
                        //var empDetails = from ba in excelFile.Worksheet<tbEmpleados>(sheetName) select ba;
                        foreach (var a in PERDetails)
                        {
                            if (a.per_Nombres != null)
                            {

                                DateTime? myBirthdate = null;


                                if (a.per_Identidad.Length > 12)
                                {  
                                    data = "Numero de identidad mayor a 12";
                                    ViewBag.Message = data;

                                }
                                var nac_id = objEntity.tbNacionalidades.Where(x => x.nac_Descripcion == a.tbNacionalidades.nac_Descripcion).Select(x => x.nac_Descripcion);

                                myBirthdate = Convert.ToDateTime(a.per_FechaNacimiento);


                                //var resullt = PostExcelData(a.per_Identidad, a.per_Nombres, a.per_Apellidos, myBirthdate, a.per_Sexo, a.nac_Id, a.per_Direccion, a.per_Telefono,a.per_CorreoElectronico,a.per_EstadoCivil,a.per_TipoSangre,a.tbEmpleados);
                                if (resullt <= 0)
                                {
                                    data = "Hello User, Found some duplicate values! Only unique employee number has inserted and duplicate values(s) are not inserted";
                                    ViewBag.Message = data;
                                    continue;

                                }
                                else
                                {
                                    data = "Successful upload records";
                                    ViewBag.Message = data;
                                }
                            }

                            else
                            {
                                data = a.per_Nombres + "Some fields are null, Please check your excel sheet";
                                ViewBag.Message = data;
                                return View("ExcelUpload");
                            }

                        }
                    }

                    else
                    {
                        data = "This file is not valid format";
                        ViewBag.Message = data;
                    }
                    return View("ExcelUpload");
                }
                else
                {

                    data = "Only Excel file format is allowed";

                    ViewBag.Message = data;
                    return View("ExcelUpload");

                }

            }
            else
            {

                if (FileUpload == null)
                {
                    data = "Please choose Excel file";
                }
                ViewBag.Message = data;
                return View("ExcelUpload");
            }
        }


        //public int PostExcelData(string Identidad, string Nombres, string Apellidos, DateTime? FechaNacimiento, string Sexo,int edad, int Nacionalidad, string Direccion, string Telefono,string CorreoElectronico,string EstadoCivil,string TipodeSangre,int Cargo,int Area,int Departamentos,int Jornadas,int Planillas,int FormadePago)
        //{
        //    ERP_GMEDINAEntities DbEntity = new ERP_GMEDINAEntities();
        //    //aqui es donde se le dice al PA que es lo que va a insertar y en que campos.
        //    var InsertExcelData = DbEntity.UDP_RRHH_tbEmpleados_Insert(Identidad,Nombres,Apellidos,FechaNacimiento,Sexo,edad,Nacionalidad,Direccion,Telefono,CorreoElectronico,EstadoCivil,TipodeSangre,Cargo,Area,Departamentos,Jornadas,Planillas,FormadePago);

        //     return InsertExcelData;
            
        //}


        // GET: Empleados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbEmpleados tbEmpleados = db.tbEmpleados.Find(id);
            if (tbEmpleados == null)
            {
                return HttpNotFound();
            }
            return View(tbEmpleados);
        }

        // GET: Empleados/Create
        public ActionResult Create()
        {
            ViewBag.emp_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario");
            ViewBag.emp_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario");
            ViewBag.area_Id = new SelectList(db.tbAreas, "area_Id", "area_Descripcion");
            ViewBag.car_Id = new SelectList(db.tbCargos, "car_Id", "car_Descripcion");
            ViewBag.per_Id = new SelectList(db.tbPersonas, "per_Id", "per_Identidad");
            return View();
        }

        // POST: Empleados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "emp_Id,per_Id,car_Id,area_Id,depto_Id,jor_Id,cpla_IdPlanilla,fpa_IdFormaPago,emp_CuentaBancaria,emp_Reingreso,emp_Fechaingreso,emp_RazonSalida,emp_CargoAnterior,emp_FechaDeSalida,emp_Estado,emp_RazonInactivo,emp_UsuarioCrea,emp_FechaCrea,emp_UsuarioModifica,emp_FechaModifica")] tbEmpleados tbEmpleados)
        {
            if (ModelState.IsValid)
            {
                db.tbEmpleados.Add(tbEmpleados);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.emp_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbEmpleados.emp_UsuarioCrea);
            ViewBag.emp_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbEmpleados.emp_UsuarioModifica);
            ViewBag.area_Id = new SelectList(db.tbAreas, "area_Id", "area_Descripcion", tbEmpleados.area_Id);
            ViewBag.car_Id = new SelectList(db.tbCargos, "car_Id", "car_Descripcion", tbEmpleados.car_Id);
            ViewBag.per_Id = new SelectList(db.tbPersonas, "per_Id", "per_Identidad", tbEmpleados.per_Id);
            return View(tbEmpleados);
        }

        // GET: Empleados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbEmpleados tbEmpleados = db.tbEmpleados.Find(id);
            if (tbEmpleados == null)
            {
                return HttpNotFound();
            }
            ViewBag.emp_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbEmpleados.emp_UsuarioCrea);
            ViewBag.emp_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbEmpleados.emp_UsuarioModifica);
            ViewBag.area_Id = new SelectList(db.tbAreas, "area_Id", "area_Descripcion", tbEmpleados.area_Id);
            ViewBag.car_Id = new SelectList(db.tbCargos, "car_Id", "car_Descripcion", tbEmpleados.car_Id);
            ViewBag.per_Id = new SelectList(db.tbPersonas, "per_Id", "per_Identidad", tbEmpleados.per_Id);
            return View(tbEmpleados);
        }

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "emp_Id,per_Id,car_Id,area_Id,depto_Id,jor_Id,cpla_IdPlanilla,fpa_IdFormaPago,emp_CuentaBancaria,emp_Reingreso,emp_Fechaingreso,emp_RazonSalida,emp_CargoAnterior,emp_FechaDeSalida,emp_Estado,emp_RazonInactivo,emp_UsuarioCrea,emp_FechaCrea,emp_UsuarioModifica,emp_FechaModifica")] tbEmpleados tbEmpleados)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbEmpleados).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.emp_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbEmpleados.emp_UsuarioCrea);
            ViewBag.emp_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbEmpleados.emp_UsuarioModifica);
            ViewBag.area_Id = new SelectList(db.tbAreas, "area_Id", "area_Descripcion", tbEmpleados.area_Id);
            ViewBag.car_Id = new SelectList(db.tbCargos, "car_Id", "car_Descripcion", tbEmpleados.car_Id);
            ViewBag.per_Id = new SelectList(db.tbPersonas, "per_Id", "per_Identidad", tbEmpleados.per_Id);
            return View(tbEmpleados);
        }

        // GET: Empleados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbEmpleados tbEmpleados = db.tbEmpleados.Find(id);
            if (tbEmpleados == null)
            {
                return HttpNotFound();
            }
            return View(tbEmpleados);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbEmpleados tbEmpleados = db.tbEmpleados.Find(id);
            db.tbEmpleados.Remove(tbEmpleados);
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
