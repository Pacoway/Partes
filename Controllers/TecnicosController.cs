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
    public class TecnicosController : Controller
    {
        private DataBaseContext db = new DataBaseContext();

        // GET: Tecnicos
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (sortOrder==null || sortOrder == "") sortOrder = "Nombre";
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NombreSortParm = sortOrder == "Nombre" ? "Nombre_desc" : "Nombre";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "Email_desc" : "Email";
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

            var tecnicos = from p in db.Tecnicos select p;

                if (!String.IsNullOrEmpty(searchString))
                {

                    tecnicos = tecnicos.Where(p => p.Nombre.Contains(searchString) ||
                                               p.Email.Contains(searchString) ||
                                               p.UsuarioCreacion.Contains(searchString) ||
                                               p.FechaCreacion.ToString().Contains(searchString));

                }        

            switch (sortOrder)
            {               
                case "Nombre":
                    tecnicos = tecnicos.OrderBy(p => p.Nombre);
                    break;
                case "Nombre_desc":
                    tecnicos = tecnicos.OrderByDescending(p => p.Nombre);
                    break;
                case "Email":
                    tecnicos = tecnicos.OrderBy(p => p.Email);
                    break;
                case "Email_desc":
                    tecnicos = tecnicos.OrderByDescending(p => p.Email);
                    break;
                case "UsuarioCreacion":
                    tecnicos = tecnicos.OrderBy(p => p.UsuarioCreacion);
                    break;
                case "UsuarioCreacion_desc":
                    tecnicos = tecnicos.OrderByDescending(p => p.UsuarioCreacion);
                    break;              
                case "FechaCreacion":
                    tecnicos = tecnicos.OrderBy(p => p.FechaCreacion);
                    break;
                case "FechaCreacion_desc":
                    tecnicos = tecnicos.OrderByDescending(p => p.FechaCreacion);
                    break;
                default:
                    tecnicos = tecnicos.OrderBy(p => p.Id);
                    break;
            }
            
            int pageNumber = (page ?? 1);

            return View(tecnicos.ToPagedList(pageNumber,Constantes.PAGE_SIZE));
        }
    
        // GET: Tecnicos Print
        public ActionResult Print()
        {
            var tecnicos = from p in db.Tecnicos select p;
            return View(tecnicos);
        }
      
        // GET: Tecnicos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tecnico tecnicos = db.Tecnicos.Find(id);
            if (tecnicos == null)
            {
                return HttpNotFound();
            }
            return View(tecnicos);
        }

        // GET: Tecnicos/Create
        public ActionResult Create()
        {
            Tecnico tecnico = new Tecnico();       
            return View(tecnico);
        }

        // POST: Tecnicos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Email,UsuarioCreacion,FechaCreacion")] Tecnico tecnico)
        {
            if (ModelState.IsValid)
            {
                if (!DataValidation.ValidateEmail(tecnico.Email))
                {
                    ModelState.AddModelError("Email", "Email no Válido");
                    return View(tecnico);
                }
                //// Check sizes               
                if (tecnico.Nombre != null && tecnico.Nombre.Length > 50) tecnico.Nombre = tecnico.Nombre.Substring(0, 50);
                if (tecnico.Email != null && tecnico.Email.Length > 50) tecnico.Email = tecnico.Email.Substring(0, 50);
                if (tecnico.UsuarioCreacion != null && tecnico.UsuarioCreacion.Length > 50) tecnico.UsuarioCreacion = tecnico.UsuarioCreacion.Substring(0, 50);              

                HistoricoTecnico historicoTecnico = new HistoricoTecnico();
                historicoTecnico.Descripcion = "Creación";
                historicoTecnico.Tecnico = tecnico;
                historicoTecnico.Usuario = /*User.Identity.Name*/ "Francisco";

                db.HistoricosTecnicos.Add(historicoTecnico);
                db.Tecnicos.Add(tecnico);

                db.SaveChanges();

                return RedirectToAction("Index");

            }

            return View(tecnico);
        }

        // GET: Tecnicos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tecnico tecnico = db.Tecnicos.Find(id);
            if (tecnico == null)
            {
                return HttpNotFound();
            }                      
            return View(tecnico);
        }

        // POST: Tecnicos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Email,UsuarioCreacion,FechaCreacion")] Tecnico tecnico)
        {
            if (ModelState.IsValid)
            {
                if (!DataValidation.ValidateEmail(tecnico.Email))
                {
                    ModelState.AddModelError("Email", "Email no Válido");
                    return View(tecnico);
                }

                Tecnico oldTecnico = db.Tecnicos.AsNoTracking().Where(p=>p.Id == tecnico.Id).FirstOrDefault();

                db.Entry(tecnico).State = EntityState.Modified;

                tecnico.UsuarioCreacion = oldTecnico.UsuarioCreacion;
                tecnico.FechaCreacion = oldTecnico.FechaCreacion;

                //// Check sizes
                if (tecnico.Nombre != null && tecnico.Nombre.Length > 50) tecnico.Nombre = tecnico.Nombre.Substring(0, 50);
                if (tecnico.Email != null && tecnico.Email.Length > 50) tecnico.Email = tecnico.Email.Substring(0, 50);
            
                HistoricoTecnico historicoTecnico = new HistoricoTecnico();
                historicoTecnico.Descripcion = "Editado: ";

                if (oldTecnico != null)
                {
                    if (oldTecnico.Nombre != tecnico.Nombre) historicoTecnico.Descripcion += "[Descripción:" + oldTecnico.Nombre + ">>" + tecnico.Nombre + "]";
                    if (oldTecnico.Email != tecnico.Email) historicoTecnico.Descripcion += "[Email:" + oldTecnico.Email + ">>" + tecnico.Email + "]";                
                }

                if (historicoTecnico.Descripcion != null && historicoTecnico.Descripcion.Length > 500) historicoTecnico.Descripcion = historicoTecnico.Descripcion.Substring(0, 500);
                if (historicoTecnico.Descripcion == "Editado: ") historicoTecnico.Descripcion += " Sin cambios";

                historicoTecnico.Tecnico = tecnico;
                historicoTecnico.Usuario = /*User.Identity.Name*/ "Francisco";

                db.HistoricosTecnicos.Add(historicoTecnico);                

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tecnico);
        }

        // GET: Tecnicos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tecnico tecnico = db.Tecnicos.Find(id);
            if (tecnico == null)
            {
                return HttpNotFound();
            }
            return View(tecnico);
        }

        // POST: Tecnicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tecnico tecnico = db.Tecnicos.Find(id);

            List<int> Ids = new List<int>();
            foreach (HistoricoTecnico historicoTecnico in tecnico.HistoricosTecnicos) Ids.Add(historicoTecnico.Id);

            foreach(int item in Ids)
            {
                HistoricoTecnico historicoTecnico = db.HistoricosTecnicos.Find(item);
                db.HistoricosTecnicos.Remove(historicoTecnico);
            }
            db.Tecnicos.Remove(tecnico);

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
