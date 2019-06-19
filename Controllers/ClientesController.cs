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
    public class ClientesController : Controller
    {
        private DataBaseContext db = new DataBaseContext();

        // GET: Clientes
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (sortOrder==null || sortOrder == "") sortOrder = "Codigo";
            ViewBag.CurrentSort = sortOrder;

            ViewBag.CodigoSortParm = sortOrder == "Codigo" ? "Codigo_desc" : "Codigo";
            ViewBag.NIFSortParm = sortOrder == "NIF" ? "NIF_desc" : "NIF";
            ViewBag.NombreFiscalSortParm = sortOrder == "NombreFiscal" ? "NombreFiscal_desc" : "NombreFiscal";
            ViewBag.NombreComercialSortParm = sortOrder == "NombreComercial" ? "NombreComercial_desc" : "NombreComercial";
            ViewBag.DomicilioSortParm = sortOrder == "Domicilio" ? "Domicilio_desc" : "Domicilio";
            ViewBag.CPSortParm = sortOrder == "CP" ? "CP_desc" : "CP";
            ViewBag.PoblacionSortParm = sortOrder == "Poblacion" ? "Poblacion_desc" : "Poblacion";
            ViewBag.ProvinciaSortParm = sortOrder == "Provincia" ? "Provincia_desc" : "Provincia";
            ViewBag.PaisSortParm = sortOrder == "Pais" ? "Pais_desc" : "Pais";
            ViewBag.PersonaContactoSortParm = sortOrder == "PersonaContacto" ? "PersonaContacto_desc" : "PersonaContacto";
            ViewBag.TelefonoContactoSortParm = sortOrder == "TelefonoContacto" ? "TelefonoContacto_desc" : "TelefonoContacto";
            ViewBag.EmailContactoSortParm = sortOrder == "EmailContacto" ? "EmailContacto_desc" : "EmailContacto";
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

            ViewBag.CurrentFilter = searchString;

            var clientes = from c in db.Clientes select c;

            int? id = null;

            try
            {
                id = Int32.Parse(searchString);
            }
            catch
            {
                id = null;
            }

            if (!String.IsNullOrEmpty(searchString))
            {

                if (id != null)
                {
                clientes = clientes.Where(p =>  p.Codigo == id ||
                                                p.NIF.Contains(searchString) ||
                                                p.NombreFiscal.Contains(searchString) ||
                                                p.NombreComercial.Contains(searchString) ||
                                                p.Domicilio.Contains(searchString) ||
                                                p.CP.Contains(searchString) ||
                                                p.Poblacion.Contains(searchString) ||
                                                p.Provincia.Contains(searchString) ||
                                                p.Pais.Contains(searchString) ||
                                                p.ContactoPersona.Contains(searchString) ||
                                                p.ContactoTelefono.Contains(searchString) ||
                                                p.ContactoEmail.Contains(searchString) ||
                                                p.UsuarioCreacion.Contains(searchString) ||
                                                p.FechaCreacion.ToString().Contains(searchString));                                            
                }
                else
                {
                    clientes = clientes.Where(p =>  p.NIF.Contains(searchString) ||
                                                    p.NombreFiscal.Contains(searchString) ||
                                                    p.NombreComercial.Contains(searchString) ||
                                                    p.Domicilio.Contains(searchString) ||
                                                    p.CP.Contains(searchString) ||
                                                    p.Poblacion.Contains(searchString) ||
                                                    p.Provincia.Contains(searchString) ||
                                                    p.Pais.Contains(searchString) ||
                                                    p.ContactoPersona.Contains(searchString) ||
                                                    p.ContactoTelefono.Contains(searchString) ||
                                                    p.ContactoEmail.Contains(searchString) ||
                                                    p.UsuarioCreacion.Contains(searchString) ||
                                                    p.FechaCreacion.ToString().Contains(searchString));
                }
            }

            switch (sortOrder)
            {
                case "Codigo":
                    clientes = clientes.OrderBy(p => p.Codigo);
                    break;
                case "Codigo_desc":
                    clientes = clientes.OrderByDescending(p => p.Codigo);
                    break;
                case "NIF":
                    clientes = clientes.OrderBy(p => p.NIF);
                    break;
                case "NIF_desc":
                    clientes = clientes.OrderByDescending(p => p.NIF);
                    break;
                case "NombreFiscal":
                    clientes = clientes.OrderBy(p => p.NombreFiscal);
                    break;
                case "NombreFiscal_desc":
                    clientes = clientes.OrderByDescending(p => p.NombreFiscal);
                    break;
                case "NombreComercial":
                    clientes = clientes.OrderBy(p => p.NombreComercial);
                    break;
                case "NombreComercial_desc":
                    clientes = clientes.OrderByDescending(p => p.NombreComercial);
                    break;
                case "Domicilio":
                    clientes = clientes.OrderBy(p => p.Domicilio);
                    break;
                case "Domicilio_desc":
                    clientes = clientes.OrderByDescending(p => p.Domicilio);
                    break;
                case "CP":
                    clientes = clientes.OrderBy(p => p.CP);
                    break;
                case "CP_desc":
                    clientes = clientes.OrderByDescending(p => p.CP);
                    break;
                case "Poblacion":
                    clientes = clientes.OrderBy(p => p.Poblacion);
                    break;
                case "Poblacion_desc":
                    clientes = clientes.OrderByDescending(p => p.Poblacion);
                    break;
                case "Provincia":
                    clientes = clientes.OrderBy(p => p.Provincia);
                    break;
                case "Provincia_desc":
                    clientes = clientes.OrderByDescending(p => p.Provincia);
                    break;
                case "Pais":
                    clientes = clientes.OrderBy(p => p.Pais);
                    break;
                case "Pais_desc":
                    clientes = clientes.OrderByDescending(p => p.Pais);
                    break;
                case "ContactoPersona":
                    clientes = clientes.OrderBy(p => p.ContactoPersona);
                    break;
                case "ContactoPersona_desc":
                    clientes = clientes.OrderByDescending(p => p.ContactoPersona);
                    break;
                case "ContactoTelefono":
                    clientes = clientes.OrderBy(p => p.ContactoTelefono);
                    break;
                case "ContactoTelefono_desc":
                    clientes = clientes.OrderByDescending(p => p.ContactoTelefono);
                    break;
                case "ContactoEmail":
                    clientes = clientes.OrderBy(p => p.ContactoEmail);
                    break;
                case "ContactoEmail_desc":
                    clientes = clientes.OrderByDescending(p => p.ContactoEmail);
                    break;
                case "UsuarioCreacion":
                    clientes = clientes.OrderBy(p => p.UsuarioCreacion);
                    break;
                case "UsuarioCreacion_desc":
                    clientes = clientes.OrderByDescending(p => p.UsuarioCreacion);
                    break;
                case "FechaCreacion":
                    clientes = clientes.OrderBy(p => p.FechaCreacion);
                    break;
                case "FechaCreacion_desc":
                    clientes = clientes.OrderByDescending(p => p.FechaCreacion);
                    break;
                default:
                    clientes = clientes.OrderBy(p => p.Codigo);
                    break;
            }
            
            int pageNumber = (page ?? 1);

            return View(clientes.ToPagedList(pageNumber,Constantes.PAGE_SIZE));
        }

        // GET: Clientes Print
        public ActionResult Print()
        {

            var clientes = from c in db.Clientes select c;

            clientes = clientes.OrderBy(p => p.Codigo);

            return View(clientes);
        }

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            Cliente cliente = new Cliente();

            return View(cliente);
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Codigo,NIF,NombreFiscal,NombreComercial,Domicilio,CP,Poblacion,Provincia,Pais,ContactoPersona,ContactoTelefono,ContactoEmail,FechaCreacion,UsuarioCreacion")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                if (!DataValidation.ValidateCIF_NIF_DNI_NIE(cliente.NIF))
                {
                    ModelState.AddModelError("NIF", "NIF no válido [A1234567] [A1234567A] [12345678A]");
                    return View(cliente);
                }               

                cliente.ContactoTelefono = DataValidation.FormatTLF(cliente.ContactoTelefono);

                //// Check sizes
                if (cliente.NIF!= null && cliente.NIF.Length > 50) cliente.NIF = cliente.NIF.Substring(0, 50);
                if (cliente.NombreFiscal != null && cliente.NombreFiscal.Length > 100) cliente.NombreFiscal = cliente.NombreFiscal.Substring(0, 100);
                if (cliente.NombreComercial != null && cliente.NombreComercial.Length > 100) cliente.NombreComercial = cliente.NombreComercial.Substring(0, 100);
                if (cliente.Domicilio != null && cliente.Domicilio.Length > 500) cliente.Domicilio = cliente.Domicilio.Substring(0, 500);
                if (cliente.CP != null && cliente.CP.Length > 50) cliente.CP = cliente.CP.Substring(0, 50);
                if (cliente.Poblacion != null && cliente.Poblacion.Length > 50) cliente.Poblacion = cliente.Poblacion.Substring(0, 50);
                if (cliente.Provincia != null && cliente.Provincia.Length > 50) cliente.Provincia = cliente.Provincia.Substring(0, 50);
                if (cliente.Pais != null && cliente.Pais.Length > 50) cliente.Pais = cliente.Pais.Substring(0, 50);
                if (cliente.ContactoPersona != null && cliente.ContactoPersona.Length > 500) cliente.ContactoPersona = cliente.ContactoPersona.Substring(0, 500);
                if (cliente.ContactoTelefono != null && cliente.ContactoTelefono.Length > 50) cliente.ContactoTelefono = cliente.ContactoTelefono.Substring(0, 50);
                if (cliente.ContactoEmail != null && cliente.ContactoEmail.Length > 50) cliente.ContactoEmail = cliente.ContactoEmail.Substring(0, 50);
                if (cliente.UsuarioCreacion != null && cliente.UsuarioCreacion.Length > 50) cliente.UsuarioCreacion = cliente.UsuarioCreacion.Substring(0, 50);

                HistoricoCliente historico = new HistoricoCliente();
                historico.Descripcion = "Creación";
                historico.Cliente = cliente;
                historico.Usuario = /*User.Identity.Name*/ "Francisco";

                db.HistoricosClientes.Add(historico);
                db.Clientes.Add(cliente);

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Codigo,NIF,NombreFiscal,NombreComercial,Domicilio,CP,Poblacion,Provincia,Pais,ContactoPersona,ContactoTelefono,ContactoEmail,FechaCreacion,UsuarioCreacion")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                if (!DataValidation.ValidateCIF_NIF_DNI_NIE(cliente.NIF))
                {
                    ModelState.AddModelError("NIF", "NIF no válido [A1234567] [A1234567A] [12345678A]");
                    return View(cliente);
                }
               
                cliente.ContactoTelefono = DataValidation.FormatTLF(cliente.ContactoTelefono);

                Cliente oldCliente = db.Clientes.AsNoTracking().Where(p=>p.Id == cliente.Id).FirstOrDefault();

                db.Entry(cliente).State = EntityState.Modified;

                cliente.UsuarioCreacion = oldCliente.UsuarioCreacion;
                cliente.FechaCreacion = oldCliente.FechaCreacion;

                //// Check sizes
                if (cliente.NIF != null && cliente.NIF.Length > 50) cliente.NIF = cliente.NIF.Substring(0, 50);
                if (cliente.NombreFiscal != null && cliente.NombreFiscal.Length > 100) cliente.NombreFiscal = cliente.NombreFiscal.Substring(0, 100);
                if (cliente.NombreComercial != null && cliente.NombreComercial.Length > 100) cliente.NombreComercial = cliente.NombreComercial.Substring(0, 100);
                if (cliente.Domicilio != null && cliente.Domicilio.Length > 500) cliente.Domicilio = cliente.Domicilio.Substring(0, 500);
                if (cliente.CP != null && cliente.CP.Length > 50) cliente.CP = cliente.CP.Substring(0, 50);
                if (cliente.Poblacion != null && cliente.Poblacion.Length > 50) cliente.Poblacion = cliente.Poblacion.Substring(0, 50);
                if (cliente.Provincia != null && cliente.Provincia.Length > 50) cliente.Provincia = cliente.Provincia.Substring(0, 50);
                if (cliente.Pais != null && cliente.Pais.Length > 50) cliente.Pais = cliente.Pais.Substring(0, 50);
                if (cliente.ContactoPersona != null && cliente.ContactoPersona.Length > 500) cliente.ContactoPersona = cliente.ContactoPersona.Substring(0, 500);
                if (cliente.ContactoTelefono != null && cliente.ContactoTelefono.Length > 50) cliente.ContactoTelefono = cliente.ContactoTelefono.Substring(0, 50);
                if (cliente.ContactoEmail != null && cliente.ContactoEmail.Length > 50) cliente.ContactoEmail = cliente.ContactoEmail.Substring(0, 50);
                if (cliente.UsuarioCreacion != null && cliente.UsuarioCreacion.Length > 50) cliente.UsuarioCreacion = cliente.UsuarioCreacion.Substring(0, 50);

                HistoricoCliente historico = new HistoricoCliente();
                historico.Descripcion = "Editado: ";

                if (oldCliente != null)
                {
                    if (oldCliente.Codigo != cliente.Codigo) historico.Descripcion += "[Código:" + oldCliente.Codigo + ">>" + cliente.Codigo+ "]";
                    if (oldCliente.NIF != cliente.NIF) historico.Descripcion += "[NIF:" + oldCliente.NIF + ">>" + cliente.NIF + "]";
                    if (oldCliente.NombreFiscal != cliente.NombreFiscal) historico.Descripcion += "[Nombre Fiscal:" + oldCliente.NombreFiscal + ">>" + cliente.NombreFiscal + "]";
                    if (oldCliente.NombreComercial != cliente.NombreComercial) historico.Descripcion += "[Nombre Comercial:" + oldCliente.NombreComercial + ">>" + cliente.NombreComercial + "]";
                    if (oldCliente.Domicilio != cliente.Domicilio) historico.Descripcion += "[Domicilio:" + oldCliente.Domicilio + ">>" + cliente.Domicilio + "]";
                    if (oldCliente.CP != cliente.CP) historico.Descripcion += "[CP:" + oldCliente.CP + ">>" + cliente.CP + "]";
                    if (oldCliente.Poblacion != cliente.Poblacion) historico.Descripcion += "[Población:" + oldCliente.Poblacion + ">>" + cliente.Poblacion + "]";
                    if (oldCliente.Provincia != cliente.Provincia) historico.Descripcion += "[Provincia:" + oldCliente.Provincia + ">>" + cliente.Provincia + "]";
                    if (oldCliente.Pais != cliente.Pais) historico.Descripcion += "[País:" + oldCliente.Pais + ">>" + cliente.Pais + "]";
                    if (oldCliente.ContactoPersona != cliente.ContactoPersona) historico.Descripcion += "[Contacto:" + oldCliente.ContactoPersona + ">>" + cliente.ContactoPersona + "]";
                    if (oldCliente.ContactoTelefono != cliente.ContactoTelefono) historico.Descripcion += "[Teléfono:" + oldCliente.ContactoTelefono + ">>" + cliente.ContactoTelefono + "]";
                    if (oldCliente.ContactoEmail != cliente.ContactoEmail) historico.Descripcion += "[Email:" + oldCliente.ContactoEmail + ">>" + cliente.ContactoEmail + "]";
                }

                if (historico.Descripcion != null && historico.Descripcion.Length > 500) historico.Descripcion = historico.Descripcion.Substring(0, 500);
                if (historico.Descripcion == "Editado: ") historico.Descripcion += " Sin cambios";

                historico.Cliente = cliente;
                historico.Usuario = /*User.Identity.Name*/ "Francisco";

                db.HistoricosClientes.Add(historico);
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Clientes.Find(id);

            List<int> Ids = new List<int>();
            foreach (HistoricoCliente historico in cliente.HistoricosClientes) Ids.Add(historico.Id);

            foreach(int item in Ids)
            {
                HistoricoCliente historico = db.HistoricosClientes.Find(item);
                db.HistoricosClientes.Remove(historico);
            }
            db.Clientes.Remove(cliente);

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
