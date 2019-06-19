using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OSCPartes.App_Data;
using OSCPartes.Models;
using System.Data.Entity.Infrastructure;
using PagedList;
using OSCPartes.Configuracion;
using OSCPartes.Validation;

namespace OSCPartes.Controllers
{
    public class VehiculosController : Controller
    {
        private DataBaseContext db = new DataBaseContext();

        // GET: Vehiculos
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (sortOrder==null || sortOrder == "") sortOrder = "Matricula";
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DescripcionSortParm = sortOrder == "Descripcion" ? "Descripcion_desc" : "Descripcion";
            ViewBag.MatriculaSortParm = sortOrder == "Matricula" ? "Matricula_desc" : "Matricula";
            ViewBag.UsuarioCreacionSortParm = sortOrder == "UsuarioCreacion" ? "UsuarioCreacion_desc" : "UsuarioCreacion";
            ViewBag.FechaCreacionSortParm = sortOrder == "FechaCreacion" ? "FechaCreacion_desc" : "FechaCreacion";
            
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var vehiculos = from p in db.Vehiculos select p;

                if (!String.IsNullOrEmpty(searchString))
                {

                    vehiculos = vehiculos.Where(p => p.Descripcion.Contains(searchString) ||
                                               p.Matricula.Contains(searchString) ||
                                               p.UsuarioCreacion.Contains(searchString) ||
                                               p.FechaCreacion.ToString().Contains(searchString));

                }        

            switch (sortOrder)
            {               
                case "Descripcion":
                    vehiculos = vehiculos.OrderBy(p => p.Descripcion);
                    break;
                case "Descripcion_desc":
                    vehiculos = vehiculos.OrderByDescending(p => p.Descripcion);
                    break;
                case "Matricula":
                    vehiculos = vehiculos.OrderBy(p => p.Matricula);
                    break;
                case "Matricula_desc":
                    vehiculos = vehiculos.OrderByDescending(p => p.Matricula);
                    break;
                case "UsuarioCreacion":
                    vehiculos = vehiculos.OrderBy(p => p.UsuarioCreacion);
                    break;
                case "UsuarioCreacion_desc":
                    vehiculos = vehiculos.OrderByDescending(p => p.UsuarioCreacion);
                    break;              
                case "FechaCreacion":
                    vehiculos = vehiculos.OrderBy(p => p.FechaCreacion);
                    break;
                case "FechaCreacion_desc":
                    vehiculos = vehiculos.OrderByDescending(p => p.FechaCreacion);
                    break;
                default:
                    vehiculos = vehiculos.OrderBy(p => p.Id);
                    break;
            }
            
            int pageNumber = (page ?? 1);

            return View(vehiculos.ToPagedList(pageNumber,Constantes.PAGE_SIZE));
        }
    
        // GET: Vehiculos Print
        public ActionResult Print()
        {
            var vehiculos = from p in db.Vehiculos select p;
            return View(vehiculos);
        }
      
        // GET: Vehiculos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehiculo vehiculos = db.Vehiculos.Find(id);
            if (vehiculos == null)
            {
                return HttpNotFound();
            }
            return View(vehiculos);
        }

        // GET: Vehiculos/Create
        public ActionResult Create()
        {
            Vehiculo vehiculo = new Vehiculo();       
            return View(vehiculo);
        }

        // POST: Vehiculos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Matricula,UsuarioCreacion,FechaCreacion")] Vehiculo vehiculo)
        {
            if (ModelState.IsValid)
            {
                if (!DataValidation.ValidateMatricula(vehiculo.Matricula))
                {
                    ModelState.AddModelError("Matricula", "Matrícula no Válida [1234XXX]");
                    return View(vehiculo);
                }
                //// Check sizes               
                if (vehiculo.Descripcion != null && vehiculo.Descripcion.Length > 50) vehiculo.Descripcion = vehiculo.Descripcion.Substring(0, 50);
                if (vehiculo.Matricula != null && vehiculo.Matricula.Length > 50) vehiculo.Matricula = vehiculo.Matricula.Substring(0, 50);
                if (vehiculo.UsuarioCreacion != null && vehiculo.UsuarioCreacion.Length > 50) vehiculo.UsuarioCreacion = vehiculo.UsuarioCreacion.Substring(0, 50);              

                HistoricoVehiculo historicoVehiculo = new HistoricoVehiculo();
                historicoVehiculo.Descripcion = "Creación";
                historicoVehiculo.Vehiculo = vehiculo;
                historicoVehiculo.Usuario = /*User.Identity.Name*/ "Francisco";

                db.HistoricosVehiculos.Add(historicoVehiculo);
                db.Vehiculos.Add(vehiculo);

                db.SaveChanges();

                return RedirectToAction("Index");

            }

            return View(vehiculo);
        }

        // GET: Vehiculos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehiculo vehiculo = db.Vehiculos.Find(id);
            if (vehiculo == null)
            {
                return HttpNotFound();
            }                      
            return View(vehiculo);
        }

        // POST: Vehiculos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Matricula,UsuarioCreacion,FechaCreacion")] Vehiculo vehiculo)
        {
            if (ModelState.IsValid)
            {
                if (!DataValidation.ValidateMatricula(vehiculo.Matricula))
                {
                    ModelState.AddModelError("Matricula", "Matrícula no Válida [1234XXX]");
                    return View(vehiculo);
                }
                Vehiculo oldVehiculo = db.Vehiculos.AsNoTracking().Where(p=>p.Id == vehiculo.Id).FirstOrDefault();

                db.Entry(vehiculo).State = EntityState.Modified;

                vehiculo.UsuarioCreacion = oldVehiculo.UsuarioCreacion;
                vehiculo.FechaCreacion = oldVehiculo.FechaCreacion;

                //// Check sizes
                if (vehiculo.Descripcion != null && vehiculo.Descripcion.Length > 50) vehiculo.Descripcion = vehiculo.Descripcion.Substring(0, 50);
                if (vehiculo.Matricula != null && vehiculo.Matricula.Length > 50) vehiculo.Matricula = vehiculo.Matricula.Substring(0, 50);
            
                HistoricoVehiculo historicoVehiculo = new HistoricoVehiculo();
                historicoVehiculo.Descripcion = "Editado: ";

                if (oldVehiculo != null)
                {
                    if (oldVehiculo.Descripcion != vehiculo.Descripcion) historicoVehiculo.Descripcion += "[Descripción:" + oldVehiculo.Descripcion + ">>" + vehiculo.Descripcion + "]";
                    if (oldVehiculo.Matricula != vehiculo.Matricula) historicoVehiculo.Descripcion += "[Matricula:" + oldVehiculo.Matricula + ">>" + vehiculo.Matricula + "]";                
                }

                if (historicoVehiculo.Descripcion != null && historicoVehiculo.Descripcion.Length > 500) historicoVehiculo.Descripcion = historicoVehiculo.Descripcion.Substring(0, 500);
                if (historicoVehiculo.Descripcion == "Editado: ") historicoVehiculo.Descripcion += " Sin cambios";

                historicoVehiculo.Vehiculo = vehiculo;
                historicoVehiculo.Usuario = /*User.Identity.Name*/ "Francisco";

                db.HistoricosVehiculos.Add(historicoVehiculo);                

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vehiculo);
        }

        // GET: Vehiculos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehiculo vehiculo = db.Vehiculos.Find(id);
            if (vehiculo == null)
            {
                return HttpNotFound();
            }
            return View(vehiculo);
        }

        // POST: Vehiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehiculo vehiculo = db.Vehiculos.Find(id);

            List<int> Ids = new List<int>();
            foreach (HistoricoVehiculo historicoVehiculo in vehiculo.HistoricosVehiculos) Ids.Add(historicoVehiculo.Id);

            foreach(int item in Ids)
            {
                HistoricoVehiculo historicoVehiculo = db.HistoricosVehiculos.Find(item);
                db.HistoricosVehiculos.Remove(historicoVehiculo);
            }
            db.Vehiculos.Remove(vehiculo);

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
