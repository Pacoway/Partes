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
    public class PartesLineasController : Controller
    {
        private DataBaseContext db = new DataBaseContext();

        // GET: ParteLineas Print
        public ActionResult Print()
        {

            var partesLineas = from p in db.PartesLineas select p;

            partesLineas = partesLineas.OrderBy(p => p.Id);

            return View(partesLineas);
        }

        // GET: ParteLineas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParteLinea PartesLineas = db.PartesLineas.Find(id);
            if (PartesLineas == null)
            {
                return HttpNotFound();
            }
            return View(PartesLineas);
        }

        // GET: ParteLineas/Create
        public ActionResult Create(int id)
        {
            ParteLinea parteLinea = new ParteLinea();
            parteLinea.IDParte = id;

            parteLinea.Tipos = cargarTipos();
            parteLinea.Tecnicos = cargarTecnicos();
            parteLinea.Vehiculos = cargarVehiculos();

            return View(parteLinea);
        }

        // POST: ParteLineas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IDParte,Fecha,HoraInicio,MinutosInicio,HoraFin,MinutosFin,KM,Tipo,Descripcion,UsuarioCreacion,FechaCreacion,PartesLineasTecnicos,PartesLineasVehiculos,TecnicosSeleccionados,VehiculosSeleccionados,TipoSeleccionado,Inicio,Fin")] ParteLinea parteLinea)
        {
            parteLinea.IDParte = parteLinea.Id;
            parteLinea.Id = 0;
            
            if (ModelState.IsValid)
            {
                if (!DataValidation.ValidateHora(parteLinea.Inicio) && !DataValidation.ValidateHora(parteLinea.Fin))
                { 
                    if (!DataValidation.ValidateHora(parteLinea.Inicio))
                    {
                        ModelState.AddModelError("Inicio", "Hora no Válida [00:00]");
                    }

                    if (!DataValidation.ValidateHora(parteLinea.Fin))
                    {
                        ModelState.AddModelError("Fin", "Hora no Válida [00:00]");
                    }
                        parteLinea.Tipos = cargarTipos();
                        parteLinea.Tecnicos = cargarTecnicos();
                        parteLinea.Vehiculos = cargarVehiculos();
                        return View(parteLinea);
                }

                if (parteLinea.TipoSeleccionado != null && parteLinea.TipoSeleccionado != "")
                {
                    parteLinea.Tipo = parteLinea.TipoSeleccionado;
                }

                parteLinea.HoraInicio = Int32.Parse(parteLinea.Inicio.Substring(0, 2));
                parteLinea.MinutosInicio = Int32.Parse(parteLinea.Inicio.Substring(3, 2));
                parteLinea.HoraFin = Int32.Parse(parteLinea.Fin.Substring(0, 2));
                parteLinea.MinutosFin = Int32.Parse(parteLinea.Fin.Substring(3, 2));

                if (((parteLinea.HoraInicio * 100) + parteLinea.MinutosInicio) > ((parteLinea.HoraFin * 100) + parteLinea.MinutosFin))
                {
                    ModelState.AddModelError("Inicio", "Hora de Inicio es mayor que la de Finalización");
                    ModelState.AddModelError("Fin", "Hora de Finalización es menor que la de Inicio");

                    parteLinea.Tipos = cargarTipos();
                    parteLinea.Tecnicos = cargarTecnicos();
                    parteLinea.Vehiculos = cargarVehiculos();

                    return View(parteLinea);
                }

                //// Check sizes
                if (parteLinea.Tipo != null && parteLinea.Tipo.Length > 50) parteLinea.Tipo = parteLinea.Tipo.Substring(0, 50);
                if (parteLinea.Descripcion != null && parteLinea.Descripcion.Length > 1000) parteLinea.Descripcion = parteLinea.Descripcion.Substring(0, 1000);
                if (parteLinea.UsuarioCreacion != null && parteLinea.UsuarioCreacion.Length > 50) parteLinea.UsuarioCreacion = parteLinea.UsuarioCreacion.Substring(0, 50);

                HistoricoParteLinea historico = new HistoricoParteLinea();
                historico.Descripcion = "Creación";
                historico.ParteLinea = parteLinea;
                historico.Usuario = /*User.Identity.Name*/ "Francisco";

                db.HistoricosPartesLineas.Add(historico);

                //PartelineaTecnico
                foreach (string id in parteLinea.TecnicosSeleccionados)
                {
                    ParteLineaTecnico parteLineaTecnico = new ParteLineaTecnico();
                    parteLineaTecnico.IDParteLinea = parteLinea.Id;
                    parteLineaTecnico.IDTecnico = int.Parse(id);

                    if (parteLinea.PartesLineasTecnicos == null) parteLinea.PartesLineasTecnicos = new List<ParteLineaTecnico>();
                    parteLinea.PartesLineasTecnicos.Add(parteLineaTecnico);
                }
                //ParteLineaVehiculo
                if(parteLinea.VehiculosSeleccionados != null)
                {
                    foreach (string id in parteLinea.VehiculosSeleccionados)
                    {
                        ParteLineaVehiculo parteLineaVehiculo = new ParteLineaVehiculo();
                        parteLineaVehiculo.IDParteLinea = parteLinea.Id;
                        parteLineaVehiculo.IDVehiculo = int.Parse(id);

                        if (parteLinea.PartesLineasVehiculos == null) parteLinea.PartesLineasVehiculos = new List<ParteLineaVehiculo>();
                        parteLinea.PartesLineasVehiculos.Add(parteLineaVehiculo);
                    }
                }

                db.PartesLineas.Add(parteLinea);

                db.SaveChanges();

                return RedirectToAction("../Partes/Details/"+parteLinea.IDParte);
            }

            parteLinea.Tipos = cargarTipos();
            parteLinea.Tecnicos = cargarTecnicos();
            parteLinea.Vehiculos = cargarVehiculos();

            return View(parteLinea);
        }

        // GET: ParteLineas/Edit/5
        public ActionResult Edit(int? id, int idParte)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParteLinea parteLinea = db.PartesLineas.Find(id);
            parteLinea.IDParte = idParte;

            if (parteLinea == null)
            {
                return HttpNotFound();
            }
    
            parteLinea.Inicio = parteLinea.VerHoraInicio;
            parteLinea.Fin = parteLinea.VerHoraFin;

            parteLinea.Tipos = cargarTiposEdicion(parteLinea.Tipo);
            parteLinea.Tecnicos = cargarTecnicosEdicion(parteLinea.PartesLineasTecnicos);
            parteLinea.Vehiculos = cargarVehiculosEdicion(parteLinea.PartesLineasVehiculos);

            return View(parteLinea);
        }

        // POST: ParteLineas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IDParte,Fecha,HoraInicio,MinutosInicio,HoraFin,MinutosFin,KM,Tipo,Descripcion,UsuarioCreacion,FechaCreacion,PartesLineasTecnicos,PartesLineasVehiculos,TecnicosSeleccionados,VehiculosSeleccionados,TipoSeleccionado,Inicio,Fin")] ParteLinea parteLinea)
        {
            if (ModelState.IsValid)
            {               

                ParteLinea oldParteLinea = db.PartesLineas.AsNoTracking().Where(p=>p.Id == parteLinea.Id).FirstOrDefault();

                db.Entry(parteLinea).State = EntityState.Modified;

                parteLinea.UsuarioCreacion = oldParteLinea.UsuarioCreacion;
                parteLinea.FechaCreacion = oldParteLinea.FechaCreacion;

                if (!DataValidation.ValidateHora(parteLinea.Inicio) && !DataValidation.ValidateHora(parteLinea.Fin))
                {
                    if (!DataValidation.ValidateHora(parteLinea.Inicio))
                    {
                        ModelState.AddModelError("Inicio", "Hora no Válida [00:00]");
                    }

                    if (!DataValidation.ValidateHora(parteLinea.Fin))
                    {
                        ModelState.AddModelError("Fin", "Hora no Válida [00:00]");
                    }

                    parteLinea.Tipos = cargarTiposEdicion(parteLinea.Tipo);
                    parteLinea.Tecnicos = cargarTecnicosEdicion(parteLinea.PartesLineasTecnicos);
                    parteLinea.Vehiculos = cargarVehiculosEdicion(parteLinea.PartesLineasVehiculos);
                    return View(parteLinea);
                }

                if (parteLinea.TipoSeleccionado != null && parteLinea.TipoSeleccionado != "")
                {
                    parteLinea.Tipo = parteLinea.TipoSeleccionado;
                }

                parteLinea.HoraInicio = Int32.Parse(parteLinea.Inicio.Substring(0, 2));
                parteLinea.MinutosInicio = Int32.Parse(parteLinea.Inicio.Substring(3, 2));
                parteLinea.HoraFin = Int32.Parse(parteLinea.Fin.Substring(0, 2));
                parteLinea.MinutosFin = Int32.Parse(parteLinea.Fin.Substring(3, 2));

                if (((parteLinea.HoraInicio * 100) + parteLinea.MinutosInicio) > ((parteLinea.HoraFin * 100) + parteLinea.MinutosFin))
                {
                    ModelState.AddModelError("Inicio", "Hora de Inicio es mayor que la de Finalización");
                    ModelState.AddModelError("Fin", "Hora de Finalización es menor que la de Inicio");

                    parteLinea.Tipos = cargarTiposEdicion(parteLinea.Tipo);
                    parteLinea.Tecnicos = cargarTecnicosEdicion(parteLinea.PartesLineasTecnicos);
                    parteLinea.Vehiculos = cargarVehiculosEdicion(parteLinea.PartesLineasVehiculos);

                    return View(parteLinea);
                }

                //// Check sizes
                if (parteLinea.Tipo != null && parteLinea.Tipo.Length > 50) parteLinea.Tipo = parteLinea.Tipo.Substring(0, 50);
                if (parteLinea.Descripcion != null && parteLinea.Descripcion.Length > 1000) parteLinea.Descripcion = parteLinea.Descripcion.Substring(0, 1000);
                if (parteLinea.UsuarioCreacion != null && parteLinea.UsuarioCreacion.Length > 50) parteLinea.UsuarioCreacion = parteLinea.UsuarioCreacion.Substring(0, 50);

                HistoricoParteLinea historico = new HistoricoParteLinea();
                historico.Descripcion = "Editado: ";

                //Tecnicos seleccionados para el historico
                string tecnicosNuevos = "";
                foreach (string tecnicoid in parteLinea.TecnicosSeleccionados)
                {
                    Tecnico tecnico = db.Tecnicos.Find(Int32.Parse(tecnicoid));
                    if (tecnicosNuevos != "") tecnicosNuevos += ", ";
                    tecnicosNuevos += tecnico.Nombre;
                }

                //Vehiculos seleccionados para el historico
                string vehiculosNuevos = "";
                if(parteLinea.VehiculosSeleccionados != null)
                {
                    foreach (string vehiculoid in parteLinea.VehiculosSeleccionados)
                    {
                        Vehiculo vehiculo = db.Vehiculos.Find(Int32.Parse(vehiculoid));
                        if (vehiculosNuevos != "") vehiculosNuevos += ", ";
                        vehiculosNuevos += vehiculo.Descripcion;
                    }
                }

                if (oldParteLinea != null)
                {

                    if (oldParteLinea.IDParte != parteLinea.IDParte) historico.Descripcion += "[IDParte:" + oldParteLinea.IDParte + ">>" + parteLinea.IDParte + "]";
                    if (oldParteLinea.ResumenTecnicos != tecnicosNuevos) historico.Descripcion += "[Tecnicos:" + oldParteLinea.ResumenTecnicos + ">>" + tecnicosNuevos + "]";
                    if (oldParteLinea.ResumenVehiculos != vehiculosNuevos) historico.Descripcion += "[Vehiculos:" + oldParteLinea.ResumenVehiculos + ">>" + vehiculosNuevos + "]";
                    if (oldParteLinea.Fecha != parteLinea.Fecha) historico.Descripcion += "[Fecha:" + oldParteLinea.Fecha + ">>" + parteLinea.Fecha + "]";
                    if (oldParteLinea.HoraInicio != parteLinea.HoraInicio) historico.Descripcion += "[HoraInicio:" + oldParteLinea.HoraInicio + ">>" + parteLinea.HoraInicio + "]";
                    if (oldParteLinea.MinutosInicio != parteLinea.MinutosInicio) historico.Descripcion += "[MinutosInicio:" + oldParteLinea.MinutosInicio + ">>" + parteLinea.MinutosInicio + "]";
                    if (oldParteLinea.HoraFin != parteLinea.HoraFin) historico.Descripcion += "[HoraFin:" + oldParteLinea.HoraFin + ">>" + parteLinea.HoraFin + "]";
                    if (oldParteLinea.MinutosFin != parteLinea.MinutosFin) historico.Descripcion += "[MinutosFin:" + oldParteLinea.MinutosFin + ">>" + parteLinea.MinutosFin + "]";
                    if (oldParteLinea.KM != parteLinea.KM) historico.Descripcion += "[KM:" + oldParteLinea.KM + ">>" + parteLinea.KM + "]";
                    if (oldParteLinea.Tipo != parteLinea.Tipo) historico.Descripcion += "[Tipo:" + oldParteLinea.Tipo + ">>" + parteLinea.Tipo + "]";
                    if (oldParteLinea.Descripcion != parteLinea.Descripcion) historico.Descripcion += "[Descripcion:" + oldParteLinea.Descripcion + ">>" + parteLinea.Descripcion + "]";
                    if (oldParteLinea.UsuarioCreacion != parteLinea.UsuarioCreacion) historico.Descripcion += "[UsuarioCreacion:" + oldParteLinea.UsuarioCreacion + ">>" + parteLinea.UsuarioCreacion + "]";
                    if (oldParteLinea.FechaCreacion != parteLinea.FechaCreacion) historico.Descripcion += "[FechaCreacion:" + oldParteLinea.FechaCreacion + ">>" + parteLinea.FechaCreacion + "]";
                }

                if (historico.Descripcion != null && historico.Descripcion.Length > 500) historico.Descripcion = historico.Descripcion.Substring(0, 500);
                if (historico.Descripcion == "Editado: ") historico.Descripcion += " Sin cambios";

                historico.ParteLinea = parteLinea;
                historico.Usuario = /*User.Identity.Name*/ "Francisco";

                db.HistoricosPartesLineas.Add(historico);

                //Borrar PartesLineasVehiculos
                    foreach (ParteLineaVehiculo parteLineaVehiculo in db.PartesLineasVehiculos.Where(p => p.IDParteLinea == parteLinea.Id))
                    {
                        db.PartesLineasVehiculos.Remove(parteLineaVehiculo);
                    }

                //Borrar PartesLineasTecnicos               
                    foreach (ParteLineaTecnico parteLineaTecnico in db.PartesLineasTecnicos.Where(p => p.IDParteLinea == parteLinea.Id))
                    {
                        db.PartesLineasTecnicos.Remove(parteLineaTecnico);
                    }

                //Crear PartelineaTecnico
                foreach (string id in parteLinea.TecnicosSeleccionados)
                {
                    ParteLineaTecnico parteLineaTecnico = new ParteLineaTecnico();
                    parteLineaTecnico.IDParteLinea = parteLinea.Id;
                    parteLineaTecnico.IDTecnico = int.Parse(id);

                    if (parteLinea.PartesLineasTecnicos == null) parteLinea.PartesLineasTecnicos = new List<ParteLineaTecnico>();
                    parteLinea.PartesLineasTecnicos.Add(parteLineaTecnico);
                }
                //Crear ParteLineaVehiculo

                if (parteLinea.VehiculosSeleccionados != null)
                {
                    foreach (string id in parteLinea.VehiculosSeleccionados)
                    {
                        ParteLineaVehiculo parteLineaVehiculo = new ParteLineaVehiculo();
                        parteLineaVehiculo.IDParteLinea = parteLinea.Id;
                        parteLineaVehiculo.IDVehiculo = int.Parse(id);

                        if (parteLinea.PartesLineasVehiculos == null) parteLinea.PartesLineasVehiculos = new List<ParteLineaVehiculo>();
                        parteLinea.PartesLineasVehiculos.Add(parteLineaVehiculo);
                    }
                }              

                db.SaveChanges();
                return RedirectToAction("../Partes/Details/" + parteLinea.IDParte);
            }

            parteLinea.Tipos = cargarTiposEdicion(parteLinea.Tipo);
            parteLinea.Tecnicos = cargarTecnicosEdicion(parteLinea.PartesLineasTecnicos);
            parteLinea.Vehiculos = cargarVehiculosEdicion(parteLinea.PartesLineasVehiculos);

            return View(parteLinea);
        }

        // GET: PArtes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParteLinea parteLinea = db.PartesLineas.Find(id);
            if (parteLinea == null)
            {
                return HttpNotFound();
            }
            return View(parteLinea);
        }

        // POST: Partes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParteLinea parteLinea = db.PartesLineas.Find(id);

            List<int> IdsPartesLineas = new List<int>();

            foreach (HistoricoParteLinea historico in parteLinea.HistoricosPartesLineas) IdsPartesLineas.Add(historico.Id);

            foreach (int item in IdsPartesLineas)
            {
                HistoricoParteLinea historico = db.HistoricosPartesLineas.Find(item);
                db.HistoricosPartesLineas.Remove(historico);
            }

            //PartesLineasVehiculos
            if (parteLinea.PartesLineasVehiculos != null)
            {
                foreach (ParteLineaVehiculo parteLineaVehiculo in db.PartesLineasVehiculos.Where(p => p.IDParteLinea == parteLinea.Id))
                {
                    db.PartesLineasVehiculos.Remove(parteLineaVehiculo);
                }
            }

            //PartesLineasTecnicos
            if (parteLinea.PartesLineasTecnicos != null)
            {
                foreach (ParteLineaTecnico parteLineaTecnico in db.PartesLineasTecnicos.Where(p => p.IDParteLinea == parteLinea.Id))
                {
                    db.PartesLineasTecnicos.Remove(parteLineaTecnico);
                }
            }

            db.PartesLineas.Remove(parteLinea);

            db.SaveChanges();
            return RedirectToAction("../Partes/Details/" + parteLinea.IDParte);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
        private List<SelectListItem> cargarTipos()
        {
            List<SelectListItem> tipos = new List<SelectListItem>();

            SelectListItem item = new SelectListItem();
            item.Text = "In Situ";
            item.Selected = true;
            tipos.Add(item);

            item = new SelectListItem();
            item.Text = "Remoto";
            item.Selected = false;
            tipos.Add(item);

            item = new SelectListItem();
            item.Text = "Taller";
            item.Selected = false;
            tipos.Add(item);

            return tipos;
        }

        private List<SelectListItem> cargarTiposEdicion(string tipo)
        {
            List<SelectListItem> tipos = new List<SelectListItem>();

            SelectListItem item = new SelectListItem();
            item.Text = "In Situ";
            if(tipo == item.Text) item.Selected = true;
            else item.Selected = false;
            tipos.Add(item);

            item = new SelectListItem();
            item.Text = "Remoto";
            if (tipo == item.Text) item.Selected = true;
            else item.Selected = false;
            tipos.Add(item);

            item = new SelectListItem();
            item.Text = "Taller";
            if (tipo == item.Text) item.Selected = true;
            else item.Selected = false;
            tipos.Add(item);


            return tipos;
        }
        private List<SelectListItem> cargarTecnicos()
        {
            List<SelectListItem> tecnicos = new List<SelectListItem>();

            foreach (Tecnico tecnico in db.Tecnicos)
            {
                SelectListItem item = new SelectListItem();
                item.Text = tecnico.Nombre;
                item.Value = tecnico.Id.ToString();
                item.Selected = false;
                tecnicos.Add(item);
            }
            return tecnicos;
        }

        private List<SelectListItem> cargarTecnicosEdicion(List<ParteLineaTecnico> partesLineasTecnicosActuales)
        {
            List<SelectListItem> tecnicos = new List<SelectListItem>();

            foreach (Tecnico tecnico in db.Tecnicos)
            {
                SelectListItem item = new SelectListItem();
                item.Text = tecnico.Nombre;
                item.Value = tecnico.Id.ToString();
                foreach(ParteLineaTecnico parteLineaTecnicoActual in partesLineasTecnicosActuales)
                {
                    if (parteLineaTecnicoActual.Tecnico.Nombre == tecnico.Nombre) item.Selected = true;
                    else item.Selected = false;
                }
                tecnicos.Add(item);
            }
            return tecnicos;
        }
        private List<SelectListItem> cargarVehiculos()
        {
            List<SelectListItem> vehiculos = new List<SelectListItem>();

            foreach (Vehiculo vehiculo in db.Vehiculos)
            {
                SelectListItem item = new SelectListItem();
                item.Text = vehiculo.Descripcion;
                item.Value = vehiculo.Id.ToString();
                item.Selected = false;
                vehiculos.Add(item);
            }
            return vehiculos;
        }

        private List<SelectListItem> cargarVehiculosEdicion(List<ParteLineaVehiculo> partesLineasVehiculosActuales)
        {
            List<SelectListItem> vehiculos = new List<SelectListItem>();

            foreach (Vehiculo vehiculo in db.Vehiculos)
            {
                SelectListItem item = new SelectListItem();
                item.Text = vehiculo.Descripcion;
                item.Value = vehiculo.Id.ToString();
                foreach (ParteLineaVehiculo parteLineaVehiculoActual in partesLineasVehiculosActuales)
                {
                    if (parteLineaVehiculoActual.Vehiculo.Descripcion == vehiculo.Descripcion) item.Selected = true;
                    else item.Selected = false;
                }
                vehiculos.Add(item);
            }
            return vehiculos;
        }
    }
}
