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
using System.Net.Mail;
using Rotativa;
using System.IO;
using System.Net.Mime;
using System.Xml.Linq;
using System.Text;

namespace OSCPartes.Controllers
{
    public class PartesController : Controller
    {

        private DataBaseContext db = new DataBaseContext();


        // GET: Partes
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, bool? archivadosFilter, bool? currentArchivadosFilter, int? page)
        {
            if (sortOrder==null || sortOrder == "") sortOrder = "FechaAlta";
            ViewBag.CurrentSort = sortOrder;

            ViewBag.TecnicoSortParm = sortOrder == "Tecnico" ? "Tecnico_desc" : "Tecnico";
            ViewBag.FechaFinSortParm = sortOrder == "FechaFin" ? "FechaFin_desc" : "FechaFin";
            ViewBag.FechaAltaSortParm = sortOrder == "FechaAlta" ? "FechaAlta_desc" : "FechaAlta";
            ViewBag.TipoSortParm = sortOrder == "Tipo" ? "Tipo_desc" : "Tipo";
            ViewBag.DescripcionSortParm = sortOrder == "Descripcion" ? "Descripcion_desc" : "Descripcion";
            ViewBag.FacturadoSortParm = sortOrder == "Facturado" ? "Facturado_desc" : "Facturado";
            ViewBag.EnviadoSortParm = sortOrder == "Enviado" ? "Enviado_desc" : "Enviado";
            ViewBag.EstadoSortParm = sortOrder == "Estado" ? "Estado_desc" : "Estado";
            ViewBag.UsuarioCreacionSortParm = sortOrder == "UsuarioCreacion" ? "UsuarioCreacion_desc" : "UsuarioCreacion";
            ViewBag.FechaCreacionSortParm = sortOrder == "FechaCreacion" ? "FechaCreacion_desc" : "FechaCreacion";

            ViewBag.ClienteCodigoSortParm = sortOrder == "ClienteCodigo" ? "ClienteCodigo_desc" : "ClienteCodigo";
            ViewBag.ClienteNIFSortParm = sortOrder == "ClienteNIF" ? "ClienteNIF_desc" : "ClienteNIF";
            ViewBag.ClienteNombreFiscalSortParm = sortOrder == "ClienteNombreFiscal" ? "ClienteNombreFiscal_desc" : "ClienteNombreFiscal";
            ViewBag.ClienteNombreComercialSortParm = sortOrder == "ClienteNombreComercial" ? "ClienteNombreComercial_desc" : "ClienteNombreComercial";
            ViewBag.ClienteDomicilioSortParm = sortOrder == "ClienteDomicilio" ? "ClienteDomicilio_desc" : "ClienteDomicilio";
            ViewBag.ClienteCPSortParm = sortOrder == "ClienteCP" ? "ClienteCP_desc" : "ClienteCP";
            ViewBag.ClientePoblacionSortParm = sortOrder == "ClientePoblacion" ? "ClientePoblacion_desc" : "ClientePoblacion";
            ViewBag.ClienteProvinciaSortParm = sortOrder == "ClienteProvencia" ? "ClienteProvincia_desc" : "ClienteProvincia";
            ViewBag.ClientePaisSortParm = sortOrder == "ClientePais" ? "ClientePais_desc" : "ClientePais";
            ViewBag.ClienteContactoPersonaSortParm = sortOrder == "ClienteContactoPersona" ? "ClienteContactoPersona_desc" : "ClienteContactoPersona";
            ViewBag.ClienteContactoTelefonoSortParm = sortOrder == "ClienteContactoTelefono" ? "ClienteContactoTelefono_desc" : "ClienteContactoTelefono";
            ViewBag.ClienteContactoEmailSortParm = sortOrder == "ClienteContactoEmail" ? "ClienteContactoEmail_desc" : "ClienteContactoEmail";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (currentArchivadosFilter == null && searchString != null)
            {
                archivadosFilter = true;
            }
            if (archivadosFilter == null) archivadosFilter = currentArchivadosFilter;

            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentArchivadosFilter = archivadosFilter;

            var partes = from p in db.Partes select p;

            int? id = null;

            try{
                id = Int32.Parse(searchString);
            }
            catch
            {
                id = null;
            }

            if (archivadosFilter != null && archivadosFilter.Value)
            {
                if(id != null)
                {
                    partes = partes.Where(p =>  p.Id == id ||
                                                p.Tecnico.Nombre.Contains(searchString) ||
                                                p.FechaAlta.ToString().Contains(searchString) ||
                                                p.FechaFin.ToString().Contains(searchString) ||
                                                p.Tipo.Contains(searchString) ||
                                                p.Estado.Contains(searchString) ||
                                                p.Descripcion.Contains(searchString) ||
                                                p.UsuarioCreacion.Contains(searchString) ||
                                                p.FechaCreacion.ToString().Contains(searchString) ||
                                                p.ClienteCodigo == id ||
                                                p.ClienteNIF.Contains(searchString) ||
                                                p.ClienteNombreFiscal.Contains(searchString) ||
                                                p.ClienteNombreComercial.Contains(searchString) ||
                                                p.ClienteDomicilio.Contains(searchString) ||
                                                p.ClienteCP.Contains(searchString) ||
                                                p.ClientePoblacion.Contains(searchString) ||
                                                p.ClienteProvincia.Contains(searchString) ||
                                                p.ClientePais.Contains(searchString) ||
                                                p.ClienteContactoPersona.Contains(searchString) ||
                                                p.ClienteContactoEmail.Contains(searchString) ||
                                                p.ClienteContactoTelefono.Contains(searchString)
                                               
                            );
                }
                else if (!String.IsNullOrEmpty(searchString))
                {
                    partes = partes.Where(p =>  p.Tecnico.Nombre.Contains(searchString) ||
                                                p.FechaAlta.ToString().Contains(searchString) ||
                                                p.FechaFin.ToString().Contains(searchString) ||
                                                p.Tipo.Contains(searchString) ||
                                                p.Estado.Contains(searchString) ||
                                                p.Descripcion.Contains(searchString) ||
                                                p.UsuarioCreacion.Contains(searchString) ||
                                                p.FechaCreacion.ToString().Contains(searchString) ||                                              
                                                p.ClienteNIF.Contains(searchString) ||
                                                p.ClienteNombreFiscal.Contains(searchString) ||
                                                p.ClienteNombreComercial.Contains(searchString) ||
                                                p.ClienteDomicilio.Contains(searchString) ||
                                                p.ClienteCP.Contains(searchString) ||
                                                p.ClientePoblacion.Contains(searchString) ||
                                                p.ClienteProvincia.Contains(searchString) ||
                                                p.ClientePais.Contains(searchString) ||
                                                p.ClienteContactoPersona.Contains(searchString) ||
                                                p.ClienteContactoEmail.Contains(searchString) ||
                                                p.ClienteContactoTelefono.Contains(searchString)
                                                 );
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    
                    partes = partes.Where(p =>  (p.Id == int.Parse(searchString) ||
                                                p.Tecnico.Nombre.Contains(searchString) ||
                                                p.FechaAlta.ToString().Contains(searchString) ||
                                                p.FechaFin.ToString().Contains(searchString) ||
                                                p.Tipo.Contains(searchString) ||
                                                p.Estado.Contains(searchString) ||
                                                p.Descripcion.Contains(searchString) ||
                                                p.UsuarioCreacion.Contains(searchString) ||
                                                p.FechaCreacion.ToString().Contains(searchString) ||
                                                p.ClienteCodigo == int.Parse(searchString) ||
                                                p.ClienteNIF.Contains(searchString) ||
                                                p.ClienteNombreFiscal.Contains(searchString) ||
                                                p.ClienteNombreComercial.Contains(searchString) ||
                                                p.ClienteDomicilio.Contains(searchString) ||
                                                p.ClienteCP.Contains(searchString) ||
                                                p.ClientePoblacion.Contains(searchString) ||
                                                p.ClienteProvincia.Contains(searchString) ||
                                                p.ClientePais.Contains(searchString) ||
                                                p.ClienteContactoPersona.Contains(searchString) ||
                                                p.ClienteContactoEmail.Contains(searchString) ||
                                                p.ClienteContactoTelefono.Contains(searchString)) &&
                                                !p.Estado.Contains("Finalizado")
                                                );
                }
                else
                {
                    if (User.Identity.Name == "OSC\\asanchez")
                    {
                        partes = partes.Where(p => p.Tecnico.Nombre.Contains("Ángel") && (!p.Estado.Contains("Finalizado") || (p.Tipo.Contains("Por Horas") && p.Facturado == false && p.Estado.Contains("Finalizado"))));
                    }
                    else if (User.Identity.Name == "OSC\\aoria")
                    {
                        partes = partes.Where(p => p.Tecnico.Nombre.Contains("Alejandro") && (!p.Estado.Contains("Finalizado") || (p.Tipo.Contains("Por Horas") && p.Facturado == false && p.Estado.Contains("Finalizado"))));
                    }
                    else if (User.Identity.Name == "OSC\\mmoreno")
                    {
                        partes = partes.Where(p => (p.Tecnico.Nombre.Contains("Mariló") && !p.Estado.Contains("Finalizado")) || (p.Tipo.Contains("Por Horas") && p.Facturado == false ));
                    }
                    else
                    {
                        partes = partes.Where(p => !p.Estado.Contains("Finalizado") || (p.Tipo.Contains("Por Horas") && p.Facturado == false && p.Estado.Contains("Finalizado") ));
                    }
                }
            }

            switch (sortOrder)
            {
                case "Tecnico":
                    partes = partes.OrderBy(p => p.IDTecnico);
                    break;
                case "Tecnico_desc":
                    partes = partes.OrderByDescending(p => p.IDTecnico);
                    break;
                case "FechaAlta":
                    partes = partes.OrderBy(p => p.FechaAlta);
                    break;
                case "FechaAlta_desc":
                    partes = partes.OrderByDescending(p => p.FechaAlta);
                    break;
                case "FechaFin":
                    partes = partes.OrderBy(p => p.FechaFin);
                    break;
                case "FechaFin_desc":
                    partes = partes.OrderByDescending(p => p.FechaFin);
                    break;
                case "Tipo":
                    partes = partes.OrderBy(p => p.Tipo);
                    break;
                case "Tipo_desc":
                    partes = partes.OrderByDescending(p => p.Tipo);
                    break;
                case "Descripcion":
                    partes = partes.OrderBy(p => p.Descripcion);
                    break;
                case "Descripcion_desc":
                    partes = partes.OrderByDescending(p => p.Descripcion);
                    break;
                case "Facturado":
                    partes = partes.OrderBy(p => p.Facturado);
                    break;
                case "Facturado_desc":
                    partes = partes.OrderByDescending(p => p.Facturado);
                    break;
                case "Enviado":
                    partes = partes.OrderBy(p => p.Enviado);
                    break;
                case "Enviado_desc":
                    partes = partes.OrderByDescending(p => p.Enviado);
                    break;
                case "Estado":
                    partes = partes.OrderBy(p => p.Estado);
                    break;
                case "Estado_desc":
                    partes = partes.OrderByDescending(p => p.Estado);
                    break;
                case "UsuarioCreacion":
                    partes = partes.OrderBy(p => p.UsuarioCreacion);
                    break;
                case "UsuarioCreacion_desc":
                    partes = partes.OrderByDescending(p => p.UsuarioCreacion);
                    break;
                case "FechaCreacion":
                    partes = partes.OrderBy(p => p.FechaCreacion);
                    break;
                case "FechaCreacion_desc":
                    partes = partes.OrderByDescending(p => p.FechaCreacion);
                    break;
                case "ClienteCodigo":
                    partes = partes.OrderBy(p => p.ClienteCodigo);
                    break;
                case "ClienteCodigo_desc":
                    partes = partes.OrderByDescending(p => p.ClienteCodigo);
                    break;
                case "ClienteNIF":
                    partes = partes.OrderBy(p => p.ClienteNIF);
                    break;
                case "ClienteNIF_desc":
                    partes = partes.OrderByDescending(p => p.ClienteNIF);
                    break;
                case "ClienteNombreFiscal":
                    partes = partes.OrderBy(p => p.ClienteNombreFiscal);
                    break;
                case "ClienteNombreFiscal_desc":
                    partes = partes.OrderByDescending(p => p.ClienteNombreFiscal);
                    break;
                case "ClienteNombreComercial":
                    partes = partes.OrderBy(p => p.ClienteNombreComercial);
                    break;
                case "ClienteNombreComercial_desc":
                    partes = partes.OrderByDescending(p => p.ClienteNombreComercial);
                    break;
                case "ClienteDocmicilio":
                    partes = partes.OrderBy(p => p.ClienteDomicilio);
                    break;
                case "ClienteDomicilio_desc":
                    partes = partes.OrderByDescending(p => p.ClienteDomicilio);
                    break;
                case "ClienteCP":
                    partes = partes.OrderBy(p => p.ClienteCP);
                    break;
                case "ClienteCP_desc":
                    partes = partes.OrderByDescending(p => p.ClienteCP);
                    break;
                case "ClienteProvincia":
                    partes = partes.OrderBy(p => p.ClienteProvincia);
                    break;
                case "ClienteProvincia_desc":
                    partes = partes.OrderByDescending(p => p.ClienteProvincia);
                    break;
                case "ClientePoblacion":
                    partes = partes.OrderBy(p => p.ClientePoblacion);
                    break;
                case "ClientePoblacion_desc":
                    partes = partes.OrderByDescending(p => p.ClientePoblacion);
                    break;
                case "ClientePais":
                    partes = partes.OrderBy(p => p.ClientePais);
                    break;
                case "ClientePais_desc":
                    partes = partes.OrderByDescending(p => p.ClientePais);
                    break;
                case "ClienteContactoPersona":
                    partes = partes.OrderBy(p => p.ClienteContactoPersona);
                    break;
                case "ClienteContactoPersona_desc":
                    partes = partes.OrderByDescending(p => p.ClienteContactoPersona);
                    break;
                case "ClienteContactoTelefono":
                    partes = partes.OrderBy(p => p.ClienteContactoTelefono);
                    break;
                case "ClienteContactoTelefono_desc":
                    partes = partes.OrderByDescending(p => p.ClienteContactoTelefono);
                    break;
                case "ClienteContactoEmail":
                    partes = partes.OrderBy(p => p.ClienteContactoEmail);
                    break;
                case "ClienteContactoEmail_desc":
                    partes = partes.OrderByDescending(p => p.ClienteContactoEmail);
                    break;
                default:
                    partes = partes.OrderBy(p => p.Id);
                    break;
            }
            
            int pageNumber = (page ?? 1);

            return View(partes.ToPagedList(pageNumber,Constantes.PAGE_SIZE));
        }

        // GET: Partes/Print
        public ActionResult Print(int id)
        {
            Parte parte = db.Partes.Find(id) ;

            return View(parte);
        }

        public ActionResult generarPDF(int id)
        {
            Parte parte = db.Partes.Find(id);
            return new PartialViewAsPdf("PrintPDF", parte);
        }

        // Enviar pdf por correo
        public ActionResult SendMail(int id)
        {
            Parte parte = db.Partes.Find(id);

            var cookies = Request.Cookies.AllKeys.ToDictionary(k => k, k => Request.Cookies[k].Value);

            var mailpdft = new ViewAsPdf()
            {
                ViewName = "PrintPDF",
                Model = parte,
                PageSize = Rotativa.Options.Size.A4,
                //FormsAuthenticationCookieName = System.Web.Security.FormsAuthentication.FormsCookieName,
                Cookies = cookies,
                PageWidth = 210,
                PageHeight = 297
            };
            
            Byte[] PdfData = mailpdft.BuildPdf(ControllerContext);

            MemoryStream stream = new MemoryStream(PdfData);
            Attachment att = new Attachment(stream, "Parte.pdf", "application/pdf");            

            string to = parte.ClienteContactoEmail;
            string from = "francisco.morina.lm@gmail.com";
            string subject = "Parte O.S. Consulting nº " + parte.Id;
            string body =   "<html><body>"+
                                "<p>Estimados señores de "+ parte.ClienteNombreFiscal +"</p>"+
                                "<p>Les adjuntamos el parte Nº "+ parte.Id +" con fecha "+ parte.FechaAlta.ToString().Substring(0,10) + "<br/>" +
                                "<p>Rogamos revisen y devuelvan firmado y sellado a francisco.morina.lm@gmail.com</p>" +
                                "<p>Pueden contactar con "+ parte.Tecnico.Nombre + " en el telefono XXX XX XX XX para cualquier incidencia </p><br/>"+
                                "<p>Saludos</p><br/>" +
                            "";

            MailMessage mail_message = new MailMessage(from, to, subject, body);

            LinkedResource LinkedImage = new LinkedResource(Server.MapPath("../../logoprint.png"));
            LinkedImage.ContentId = "MyPic";            
            LinkedImage.ContentType = new ContentType(MediaTypeNames.Image.Jpeg);

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
              body+ " <img style='width: 45%' src=cid:MyPic></body></html>",
              null, "text/html");

            htmlView.LinkedResources.Add(LinkedImage);
            mail_message.AlternateViews.Add(htmlView);

            mail_message.Attachments.Add(att);
            mail_message.IsBodyHtml = true;

            var client = new SmtpClient("smtp.gmail.com", 587);            
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("francisco.morina.lm@gmail.com", "paco741852963");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Timeout = 5000;

            try
            {
                client.Send(mail_message);
            }
            catch
            {
                parte.EmailTextInfo = "El Email ha sido enviado correctamente";

                parte.Enviado = true;
                parte.ClienteSeleccionado = "4";
                parte.EstadoSeleccionado = "4";
                parte.TecnicoSeleccionado = "4";
                parte.TipoSeleccionado = "4";
                db.SaveChanges();

                return RedirectToAction("Details/", new { id = parte.Id, emailTextInfo = parte.EmailTextInfo });
            }

            parte.EmailTextInfo = "El Email ha sido enviado correctamente";
            
            parte.Enviado = true;
            parte.ClienteSeleccionado = "4";
            parte.EstadoSeleccionado = "4";
            parte.TecnicoSeleccionado = "4";
            parte.TipoSeleccionado = "4";
            db.SaveChanges();
            
            return RedirectToAction("Details/", new { id = parte.Id, emailTextInfo = parte.EmailTextInfo });
        }

        // GET: Partes/Details/5
        public ActionResult Details(int? id, string emailTextInfo, string sortOrder, string currentFilter, string searchString)
            {
                if (sortOrder == null || sortOrder == "") sortOrder = "Fecha";
                ViewBag.CurrentSort = sortOrder;

                ViewBag.FechaSortParm = sortOrder == "Fecha" ? "Fecha_desc" : "Fecha";
                ViewBag.TecnicosSortParm = sortOrder == "Tecnicos" ? "Tecnicos_desc" : "Tecnicos";
                ViewBag.VehiculosSortParm = sortOrder == "Vehiculos" ? "Vehiculos_desc" : "Vehiculos";
                ViewBag.InicioSortParm = sortOrder == "Inicio" ? "Inicio_desc" : "Inicio";
                ViewBag.FinSortParm = sortOrder == "Fin" ? "Fin_desc" : "Fin";
                ViewBag.HorasSortParm = sortOrder == "Horas" ? "Horas_desc" : "Horas";
                ViewBag.KmSortParm = sortOrder == "Km" ? "Km_desc" : "Km";
                ViewBag.TipoSortParm = sortOrder == "Tipo" ? "Tipo_desc" : "Tipo";
                ViewBag.DescripcionSortParm = sortOrder == "Descripcion" ? "Descripcion_desc" : "Descripcion";
            
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Parte parte = db.Partes.Find(id);
                if (parte == null)
                {
                    return HttpNotFound();
                }

                parte.EmailTextInfo = emailTextInfo;

                return View(parte);
            }

        // GET: Partes/Create
        public ActionResult Create(string searchCliente)
        {
            Parte parte = new Parte();

            parte.Tipos = new List<SelectListItem>();

            SelectListItem item = new SelectListItem();
            item.Text = "Presupuesto Cerrado";
            item.Selected = true;
            parte.Tipos.Add(item);

            item = new SelectListItem();
            item.Text = "Mantenimiento";
            item.Selected = false;
            parte.Tipos.Add(item);

            item = new SelectListItem();
            item.Text = "Por Horas";
            item.Selected = false;
            parte.Tipos.Add(item);

            parte.Clientes = new List<SelectListItem>();

            int? search_Codigo = null;

            try
            {
                search_Codigo = Convert.ToInt32(searchCliente);
            }
            catch
            {
                search_Codigo = null;
            }

            if (search_Codigo != null && searchCliente != null && searchCliente != "")
            {
                foreach (Cliente cliente in db.Clientes.Where(p => p.Codigo == search_Codigo || (p.NombreComercial.Contains(searchCliente) || p.NombreFiscal.Contains(searchCliente))))
                {
                    item = new SelectListItem();
                    item.Text = cliente.NombreFiscal;
                    item.Value = cliente.Id.ToString();
                    item.Selected = false;
                    parte.Clientes.Add(item);
                }
            }

            else if (searchCliente != null && searchCliente != "")
            {
                foreach(Cliente cliente in db.Clientes.Where(p=> p.NombreComercial.Contains(searchCliente) || p.NombreFiscal.Contains(searchCliente)))
                {
                    item = new SelectListItem();
                    item.Text = cliente.NombreFiscal;
                    item.Value = cliente.Id.ToString();
                    item.Selected = false;
                    parte.Clientes.Add(item);
                }
            }

            else
            {
                foreach (Cliente cliente in db.Clientes)
                {
                    item = new SelectListItem();
                    item.Text = cliente.NombreFiscal;
                    item.Value = cliente.Id.ToString();
                    item.Selected = false;
                    parte.Clientes.Add(item);
                }
            }

            parte.Tecnicos = new List<SelectListItem>();

            foreach (Tecnico tecnico in db.Tecnicos)
            {
                item = new SelectListItem();
                item.Text = tecnico.Nombre;
                item.Value = tecnico.Id.ToString();
                item.Selected = false;
                parte.Tecnicos.Add(item);
            }

            parte.Estados = new List<SelectListItem>();

            item = new SelectListItem();
            item.Text = "Pendiente";
            item.Selected = true;
            parte.Estados.Add(item);

            item = new SelectListItem();
            item.Text = "En Progreso";
            item.Selected = false;
            parte.Estados.Add(item);

            item = new SelectListItem();
            item.Text = "Finalizado";
            item.Selected = false;
            parte.Estados.Add(item);

            return View(parte);
        }

        // POST: Partes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IDCliente,ClienteCodigo,ClienteNIF,ClienteNombreFiscal,ClienteNombreComercial,ClienteDomicilio,ClienteCP,ClientePoblacion,ClienteProvincia,ClientePais,ClienteContactoPersona,ClienteContactoTelefono,ClienteContactoEmail,IDTecnico,FechaAlta,FechaFin,Tipo,Descripcion,Facturado,Enviado,UsuarioCreacion,FechaCreacion,ClienteSeleccionado,TecnicoSeleccionado,TipoSeleccionado,Estado,EstadoSeleccionado,HorasFacturables")] Parte parte, string searchCliente)
        {
            if (ModelState.IsValid)
            {
                if (parte.ClienteSeleccionado != null && parte.ClienteSeleccionado != "")
                {
                    parte.IDCliente = int.Parse(parte.ClienteSeleccionado);
                    parte.Cliente = db.Clientes.Find(parte.IDCliente);

                    parte.ClienteCodigo = parte.Cliente.Codigo;
                    parte.ClienteNIF = parte.Cliente.NIF;
                    parte.ClienteNombreFiscal = parte.Cliente.NombreFiscal;
                    parte.ClienteNombreComercial = parte.Cliente.NombreComercial;
                    parte.ClienteDomicilio = parte.Cliente.Domicilio;
                    parte.ClienteCP = parte.Cliente.CP;
                    parte.ClientePoblacion = parte.Cliente.Poblacion;
                    parte.ClienteProvincia = parte.Cliente.Provincia;
                    parte.ClientePais = parte.Cliente.Pais;
                    parte.ClienteContactoPersona = parte.Cliente.ContactoPersona;
                    parte.ClienteContactoTelefono = parte.Cliente.ContactoTelefono;
                    parte.ClienteContactoEmail = parte.Cliente.ContactoEmail;
                }

                if (parte.TecnicoSeleccionado != null && parte.TecnicoSeleccionado != "")
                {
                    parte.IDTecnico = int.Parse(parte.TecnicoSeleccionado);
                    parte.Tecnico = db.Tecnicos.Find(parte.IDTecnico);
                }

                if (parte.TipoSeleccionado != null && parte.TipoSeleccionado != "")
                {
                    parte.Tipo = parte.TipoSeleccionado;
                }

                if (parte.EstadoSeleccionado != null && parte.EstadoSeleccionado != "")
                {
                    parte.Estado = parte.EstadoSeleccionado;
                }

                if (parte.HorasFacturables == null && parte.TipoSeleccionado == "Por Horas")
                {
                    parte.HorasFacturables = "";
                }

                //// Check sizes
                if (parte.ClienteNIF != null && parte.ClienteNIF.Length > 50) parte.ClienteNIF = parte.ClienteNIF.Substring(0, 50);
                if (parte.ClienteNombreFiscal != null && parte.ClienteNombreFiscal.Length > 100) parte.ClienteNombreFiscal = parte.ClienteNombreFiscal.Substring(0, 100);
                if (parte.ClienteNombreComercial != null && parte.ClienteNombreComercial.Length > 100) parte.ClienteNombreComercial = parte.ClienteNombreComercial.Substring(0, 100);
                if (parte.ClienteDomicilio != null && parte.ClienteDomicilio.Length > 500) parte.ClienteDomicilio = parte.ClienteDomicilio.Substring(0, 500);
                if (parte.ClienteCP != null && parte.ClienteCP.Length > 50) parte.ClienteCP = parte.ClienteCP.Substring(0, 50);
                if (parte.ClientePoblacion != null && parte.ClientePoblacion.Length > 50) parte.ClientePoblacion = parte.ClientePoblacion.Substring(0, 50);
                if (parte.ClienteProvincia != null && parte.ClienteProvincia.Length > 50) parte.ClienteProvincia = parte.ClienteProvincia.Substring(0, 50);
                if (parte.ClientePais != null && parte.ClientePais.Length > 50) parte.ClientePais = parte.ClientePais.Substring(0, 50);
                if (parte.ClienteContactoPersona != null && parte.ClienteContactoPersona.Length > 500) parte.ClienteContactoPersona = parte.ClienteContactoPersona.Substring(0, 500);
                if (parte.ClienteContactoTelefono != null && parte.ClienteContactoTelefono.Length > 50) parte.ClienteContactoTelefono = parte.ClienteContactoTelefono.Substring(0, 50);
                if (parte.ClienteContactoEmail!= null && parte.ClienteContactoEmail.Length > 50) parte.ClienteContactoEmail = parte.ClienteContactoEmail.Substring(0, 50);
                if (parte.Tipo != null && parte.Tipo.Length > 50) parte.Tipo = parte.Tipo.Substring(0, 50);
                if (parte.Descripcion != null && parte.Descripcion.Length > 1000) parte.Descripcion = parte.Descripcion.Substring(0, 1000);
                if (parte.UsuarioCreacion != null && parte.UsuarioCreacion.Length > 50) parte.UsuarioCreacion = parte.UsuarioCreacion.Substring(0, 50);

                HistoricoParte historico = new HistoricoParte();
                historico.Descripcion = "Creación";
                historico.Parte = parte;
                historico.Usuario = /*User.Identity.Name*/ "Francisco";

                db.HistoricosPartes.Add(historico);
                db.Partes.Add(parte);

                db.SaveChanges();

                return RedirectToAction("Details/"+parte.Id);
            }

            parte.Tipos = new List<SelectListItem>();

            SelectListItem item = new SelectListItem();
            item.Text = "Presupuesto Cerrado";
            item.Selected = true;
            parte.Tipos.Add(item);

            item = new SelectListItem();
            item.Text = "Mantenimiento";
            item.Selected = false;
            parte.Tipos.Add(item);

            item = new SelectListItem();
            item.Text = "Por Horas";
            item.Selected = false;
            parte.Tipos.Add(item);

            parte.Clientes = new List<SelectListItem>();

            int? search_Codigo = null;

            try
            {
                search_Codigo = Convert.ToInt32(searchCliente);
            }
            catch
            {
                search_Codigo = null;
            }

            if (search_Codigo != null && searchCliente != null && searchCliente != "")
            {
                foreach (Cliente cliente in db.Clientes.Where(p => p.Codigo == search_Codigo || (p.NombreComercial.Contains(searchCliente) || p.NombreFiscal.Contains(searchCliente))))
                {
                    item = new SelectListItem();
                    item.Text = cliente.NombreComercial;
                    item.Value = cliente.Id.ToString();
                    item.Selected = false;
                    parte.Clientes.Add(item);
                }
            }

            else if (searchCliente != null && searchCliente != "")
            {
                foreach(Cliente cliente in db.Clientes.Where(p=> p.NombreComercial.Contains(searchCliente) || p.NombreFiscal.Contains(searchCliente)))
                {
                    item = new SelectListItem();
                    item.Text = cliente.NombreComercial;
                    item.Value = cliente.Id.ToString();
                    item.Selected = false;
                    parte.Clientes.Add(item);
                }
            }

            else
            {
                foreach (Cliente cliente in db.Clientes)
                {
                    item = new SelectListItem();
                    item.Text = cliente.NombreComercial;
                    item.Value = cliente.Id.ToString();
                    item.Selected = false;
                    parte.Clientes.Add(item);
                }
            }

            parte.Tecnicos = new List<SelectListItem>();

            foreach (Tecnico tecnico in db.Tecnicos)
            {
                item = new SelectListItem();
                item.Text = tecnico.Nombre;
                item.Value = tecnico.Id.ToString();
                item.Selected = false;
                parte.Tecnicos.Add(item);
            }

            parte.Estados = new List<SelectListItem>();

            item = new SelectListItem();
            item.Text = "Pendiente";
            item.Selected = true;
            parte.Estados.Add(item);

            item = new SelectListItem();
            item.Text = "En Progreso";
            item.Selected = false;
            parte.Estados.Add(item);

            item = new SelectListItem();
            item.Text = "Finalizado";
            item.Selected = false;
            parte.Estados.Add(item);

            return View(parte);
        }

        // GET: Partes/Edit/5
        public ActionResult Edit(int? id, string searchCliente)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parte parte = db.Partes.Find(id);

            if (parte == null)
            {
                return HttpNotFound();
            }

            parte.Tipos = new List<SelectListItem>();

            SelectListItem item = new SelectListItem();
            item.Text = "Presupuesto Cerrado";
            if (parte.Tipo == "Presupuesto Cerrado") item.Selected = true;
            else item.Selected = false;
            parte.Tipos.Add(item);

            item = new SelectListItem();
            item.Text = "Mantenimiento";
            if (parte.Tipo == "Mantenimiento") item.Selected = true;
            else item.Selected = false;
            parte.Tipos.Add(item);

            item = new SelectListItem();
            item.Text = "Por Horas";
            if (parte.Tipo == "Por Horas") item.Selected = true;
            else item.Selected = false;
            parte.Tipos.Add(item);

            parte.Clientes = new List<SelectListItem>();

            int? search_Codigo = null;

            try
            {
                search_Codigo = Convert.ToInt32(searchCliente);
            }
            catch
            {
                search_Codigo = null;
            }

            if (search_Codigo != null && searchCliente != null && searchCliente != "")
            {
                foreach (Cliente cliente in db.Clientes.Where(p => p.Codigo == search_Codigo || (p.NombreComercial.Contains(searchCliente) || p.NombreFiscal.Contains(searchCliente))))
                {
                    item = new SelectListItem();
                    item.Text = cliente.NombreComercial;
                    item.Value = cliente.Id.ToString();                
                    item.Selected = false;
                    parte.Clientes.Add(item);
                }
            }

            else if (searchCliente != null && searchCliente != "")
            {
                foreach (Cliente cliente in db.Clientes.Where(p => p.NombreComercial.Contains(searchCliente) || p.NombreFiscal.Contains(searchCliente)))
                {
                    item = new SelectListItem();
                    item.Text = cliente.NombreComercial;
                    item.Value = cliente.Id.ToString();
                    item.Selected = false;
                    parte.Clientes.Add(item);
                }
            }

            else
            {
                foreach (Cliente cliente in db.Clientes)
                {
                    item = new SelectListItem();
                    item.Text = cliente.NombreComercial;
                    item.Value = cliente.Id.ToString();
                    if (parte.ClienteNombreComercial == cliente.NombreComercial) item.Selected = true;
                    else item.Selected = false;
                    parte.Clientes.Add(item);
                }
            }

            parte.Tecnicos = new List<SelectListItem>();

            foreach (Tecnico tecnico in db.Tecnicos)
            {
                item = new SelectListItem();
                item.Text = tecnico.Nombre;
                item.Value = tecnico.Id.ToString();
                if (parte.Tecnico.Nombre == tecnico.Nombre) item.Selected = true;
                else item.Selected = false;
                parte.Tecnicos.Add(item);
            }

            parte.Estados = new List<SelectListItem>();

            item = new SelectListItem();
            item.Text = "Pendiente";
            if (parte.Estado == "Pendiente") item.Selected = true;
            else item.Selected = false;
            parte.Estados.Add(item);

            item = new SelectListItem();
            item.Text = "En Progreso";
            if (parte.Estado == "En Progreso") item.Selected = true;
            else item.Selected = false;
            parte.Estados.Add(item);

            item = new SelectListItem();
            item.Text = "Finalizado";
            if (parte.Estado == "Finalizado") item.Selected = true;
            else item.Selected = false;
            parte.Estados.Add(item);

            return View(parte);
        }

        // POST: Partes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IDCliente,ClienteCodigo,ClienteNIF,ClienteNombreFiscal,ClienteNombreComercial,ClienteDomicilio,ClienteCP,ClientePoblacion,ClienteProvincia,ClientePais,ClienteContactoPersona,ClienteContactoTelefono,ClienteContactoEmail,IDTecnico,FechaAlta,FechaFin,Tipo,Descripcion,Facturado,Enviado,UsuarioCreacion,FechaCreacion,ClienteSeleccionado,TecnicoSeleccionado,TipoSeleccionado,Estado,EstadoSeleccionado,HorasFacturables")] Parte parte, string searchCliente)
        {
            if (ModelState.IsValid)
            {
                if (parte.ClienteSeleccionado != null && parte.ClienteSeleccionado != "")
                {
                    parte.IDCliente = int.Parse(parte.ClienteSeleccionado);
                    parte.Cliente = db.Clientes.Find(parte.IDCliente);

                    parte.ClienteCodigo = parte.Cliente.Codigo;
                    parte.ClienteNIF = parte.Cliente.NIF;
                    parte.ClienteNombreFiscal = parte.Cliente.NombreFiscal;
                    parte.ClienteNombreComercial = parte.Cliente.NombreComercial;
                    parte.ClienteDomicilio = parte.Cliente.Domicilio;
                    parte.ClienteCP = parte.Cliente.CP;
                    parte.ClientePoblacion = parte.Cliente.Poblacion;
                    parte.ClienteProvincia = parte.Cliente.Provincia;
                    parte.ClientePais = parte.Cliente.Pais;
                    parte.ClienteContactoPersona = parte.Cliente.ContactoPersona;
                    parte.ClienteContactoTelefono = parte.Cliente.ContactoTelefono;
                    parte.ClienteContactoEmail = parte.Cliente.ContactoEmail;
                }

                if (parte.TecnicoSeleccionado != null && parte.TecnicoSeleccionado != "")
                {
                    parte.IDTecnico = int.Parse(parte.TecnicoSeleccionado);
                    parte.Tecnico = db.Tecnicos.Find(parte.IDTecnico);
                }

                if (parte.TipoSeleccionado != null && parte.TipoSeleccionado != "")
                {
                    parte.Tipo = parte.TipoSeleccionado;
                }

                if (parte.EstadoSeleccionado != null && parte.EstadoSeleccionado != "")
                {
                    parte.Estado = parte.EstadoSeleccionado;
                }

                if (parte.HorasFacturables == null && parte.TipoSeleccionado == "Por Horas")
                {
                    parte.HorasFacturables = "";
                }

                Parte oldParte = db.Partes.AsNoTracking().Where(p=>p.Id == parte.Id).FirstOrDefault();

                db.Entry(parte).State = EntityState.Modified;

                parte.UsuarioCreacion = oldParte.UsuarioCreacion;
                parte.FechaCreacion = oldParte.FechaCreacion;

                //// Check sizes
                if (parte.ClienteNIF != null && parte.ClienteNIF.Length > 50) parte.ClienteNIF = parte.ClienteNIF.Substring(0, 50);
                if (parte.ClienteNombreFiscal != null && parte.ClienteNombreFiscal.Length > 100) parte.ClienteNombreFiscal = parte.ClienteNombreFiscal.Substring(0, 100);
                if (parte.ClienteNombreComercial != null && parte.ClienteNombreComercial.Length > 100) parte.ClienteNombreComercial = parte.ClienteNombreComercial.Substring(0, 100);
                if (parte.ClienteDomicilio != null && parte.ClienteDomicilio.Length > 500) parte.ClienteDomicilio = parte.ClienteDomicilio.Substring(0, 500);
                if (parte.ClienteCP != null && parte.ClienteCP.Length > 50) parte.ClienteCP = parte.ClienteCP.Substring(0, 50);
                if (parte.ClientePoblacion != null && parte.ClientePoblacion.Length > 50) parte.ClientePoblacion = parte.ClientePoblacion.Substring(0, 50);
                if (parte.ClienteProvincia != null && parte.ClienteProvincia.Length > 50) parte.ClienteProvincia = parte.ClienteProvincia.Substring(0, 50);
                if (parte.ClientePais != null && parte.ClientePais.Length > 50) parte.ClientePais = parte.ClientePais.Substring(0, 50);
                if (parte.ClienteContactoPersona != null && parte.ClienteContactoPersona.Length > 500) parte.ClienteContactoPersona = parte.ClienteContactoPersona.Substring(0, 500);
                if (parte.ClienteContactoTelefono != null && parte.ClienteContactoTelefono.Length > 50) parte.ClienteContactoTelefono = parte.ClienteContactoTelefono.Substring(0, 50);
                if (parte.ClienteContactoEmail != null && parte.ClienteContactoEmail.Length > 50) parte.ClienteContactoEmail = parte.ClienteContactoEmail.Substring(0, 50);
                if (parte.Tipo != null && parte.Tipo.Length > 50) parte.Tipo = parte.Tipo.Substring(0, 50);
                if (parte.Estado != null && parte.Estado.Length > 50) parte.Estado = parte.Estado.Substring(0, 50);
                if (parte.Descripcion != null && parte.Descripcion.Length > 1000) parte.Descripcion = parte.Descripcion.Substring(0, 1000);
                if (parte.UsuarioCreacion != null && parte.UsuarioCreacion.Length > 50) parte.UsuarioCreacion = parte.UsuarioCreacion.Substring(0, 50);

                HistoricoParte historico = new HistoricoParte();
                historico.Descripcion = "Editado: ";

                if (oldParte != null)
                {
                    if (oldParte.IDCliente!= parte.IDCliente) historico.Descripcion += "[IDCliente:" + oldParte.IDCliente + ">>" + parte.IDCliente + "]";
                    if (oldParte.ClienteCodigo != parte.ClienteCodigo) historico.Descripcion += "[ClienteCodigo:" + oldParte.ClienteCodigo + ">>" + parte.ClienteCodigo + "]";
                    if (oldParte.ClienteNIF != parte.ClienteNIF) historico.Descripcion += "[ClienteNIF:" + oldParte.ClienteNIF + ">>" + parte.ClienteNIF + "]";
                    if (oldParte.ClienteNombreFiscal != parte.ClienteNombreFiscal) historico.Descripcion += "[ClienteNombreFiscal:" + oldParte.ClienteNombreFiscal + ">>" + parte.ClienteNombreFiscal + "]";
                    if (oldParte.ClienteNombreComercial != parte.ClienteNombreComercial) historico.Descripcion += "[ClienteNombreComercial:" + oldParte.ClienteNombreComercial + ">>" + parte.ClienteNombreComercial + "]";
                    if (oldParte.ClienteDomicilio != parte.ClienteDomicilio) historico.Descripcion += "[ClienteDomicilio:" + oldParte.ClienteDomicilio + ">>" + parte.ClienteDomicilio + "]";
                    if (oldParte.ClienteCP != parte.ClienteCP) historico.Descripcion += "[ClienteCP:" + oldParte.ClienteCP + ">>" + parte.ClienteCP + "]";
                    if (oldParte.ClienteProvincia != parte.ClienteProvincia) historico.Descripcion += "[ClienteProvincia:" + oldParte.ClienteProvincia + ">>" + parte.ClienteProvincia + "]";
                    if (oldParte.ClientePoblacion != parte.ClientePoblacion) historico.Descripcion += "[ClientePoblacion:" + oldParte.ClientePoblacion + ">>" + parte.ClientePoblacion + "]";
                    if (oldParte.ClientePais != parte.ClientePais) historico.Descripcion += "[ClientePais:" + oldParte.ClientePais + ">>" + parte.ClientePais + "]";
                    if (oldParte.ClienteContactoPersona != parte.ClienteContactoPersona) historico.Descripcion += "[ClienteContactoPersona:" + oldParte.ClienteContactoPersona + ">>" + parte.ClienteContactoPersona + "]";
                    if (oldParte.ClienteContactoTelefono != parte.ClienteContactoTelefono) historico.Descripcion += "[ClienteContactoTelefono:" + oldParte.ClienteContactoTelefono + ">>" + parte.ClienteContactoTelefono + "]";
                    if (oldParte.ClienteContactoEmail != parte.ClienteContactoEmail) historico.Descripcion += "[ClienteContactoEmail:" + oldParte.ClienteContactoEmail + ">>" + parte.ClienteContactoEmail + "]";
                    if (oldParte.IDTecnico != parte.IDTecnico) historico.Descripcion += "[IDTecnico:" + oldParte.IDTecnico + ">>" + parte.IDTecnico + "]";
                    if (oldParte.FechaAlta != parte.FechaAlta) historico.Descripcion += "[FechaAlta:" + oldParte.FechaAlta + ">>" + parte.FechaAlta + "]";
                    if (oldParte.FechaFin != parte.FechaFin) historico.Descripcion += "[FechaFin:" + oldParte.FechaFin + ">>" + parte.FechaFin + "]";
                    if (oldParte.Tipo != parte.Tipo) historico.Descripcion += "[Tipo:" + oldParte.Tipo + ">>" + parte.Tipo + "]";
                    if (oldParte.Descripcion != parte.Descripcion) historico.Descripcion += "[Descripcion:" + oldParte.Descripcion + ">>" + parte.Descripcion + "]";
                    if (oldParte.Facturado != parte.Facturado) historico.Descripcion += "[Facturado:" + oldParte.Facturado + ">>" + parte.Facturado + "]";
                    if (oldParte.Enviado != parte.Enviado) historico.Descripcion += "[Enviado:" + oldParte.Enviado + ">>" + parte.Enviado + "]";
                    if (oldParte.Estado != parte.Estado) historico.Descripcion += "[Estado:" + oldParte.Estado + ">>" + parte.Estado + "]";
                    if (oldParte.UsuarioCreacion != parte.UsuarioCreacion) historico.Descripcion += "[UsuarioCreacion:" + oldParte.UsuarioCreacion + ">>" + parte.UsuarioCreacion + "]";
                    if (oldParte.FechaCreacion != parte.FechaCreacion) historico.Descripcion += "[FechaCreacion:" + oldParte.FechaCreacion + ">>" + parte.FechaCreacion + "]";
                }

                if (historico.Descripcion != null && historico.Descripcion.Length > 500) historico.Descripcion = historico.Descripcion.Substring(0, 500);
                if (historico.Descripcion == "Editado: ") historico.Descripcion += " Sin cambios";

                historico.Parte = parte;
                historico.Usuario = /*User.Identity.Name*/ "Francisco";

                db.HistoricosPartes.Add(historico);
                
                db.SaveChanges();
                return RedirectToAction("Details/" + parte.Id);
            }

            parte.Tipos = new List<SelectListItem>();

            SelectListItem item = new SelectListItem();
            item.Text = "Presupuesto Cerrado";
            if (parte.Tipo == "Presupuesto Cerrado") item.Selected = true;
            else item.Selected = false;
            parte.Tipos.Add(item);

            item = new SelectListItem();
            item.Text = "Mantenimiento";
            if (parte.Tipo == "Mantenimiento") item.Selected = true;
            else item.Selected = false;
            parte.Tipos.Add(item);

            item = new SelectListItem();
            item.Text = "Por Horas";
            if (parte.Tipo == "Por Horas") item.Selected = true;
            else item.Selected = false;
            parte.Tipos.Add(item);

            parte.Clientes = new List<SelectListItem>();

            int? search_Codigo = null;

            try
            {
                search_Codigo = Convert.ToInt32(searchCliente);
            }
            catch
            {
                search_Codigo = null;
            }

            if (search_Codigo != null && searchCliente != null && searchCliente != "")
            {
                foreach (Cliente cliente in db.Clientes.Where(p => p.Codigo == search_Codigo || (p.NombreComercial.Contains(searchCliente) || p.NombreFiscal.Contains(searchCliente))))
                {
                    item = new SelectListItem();
                    item.Text = cliente.NombreComercial;
                    item.Value = cliente.Id.ToString();
                    item.Selected = false;
                    parte.Clientes.Add(item);
                }
            }

            else if (searchCliente != null && searchCliente != "")
            {
                foreach (Cliente cliente in db.Clientes.Where(p => p.NombreComercial.Contains(searchCliente) || p.NombreFiscal.Contains(searchCliente)))
                {
                    item = new SelectListItem();
                    item.Text = cliente.NombreComercial;
                    item.Value = cliente.Id.ToString();
                    item.Selected = false;
                    parte.Clientes.Add(item);
                }
            }

            else
            {
                foreach (Cliente cliente in db.Clientes)
                {
                    item = new SelectListItem();
                    item.Text = cliente.NombreComercial;
                    item.Value = cliente.Id.ToString();
                    item.Selected = false;
                    parte.Clientes.Add(item);
                }
            }

            parte.Tecnicos = new List<SelectListItem>();

            foreach (Tecnico tecnico in db.Tecnicos)
            {
                item = new SelectListItem();
                item.Text = tecnico.Nombre;
                item.Value = tecnico.Id.ToString();
                if (parte.Tecnico.Nombre == tecnico.Nombre) item.Selected = true;
                else item.Selected = false;
                parte.Tecnicos.Add(item);
            }

            parte.Estados = new List<SelectListItem>();

            item = new SelectListItem();
            item.Text = "Pendiente";
            if (parte.Estado == "Pendiente") item.Selected = true;
            else item.Selected = false;
            parte.Estados.Add(item);

            item = new SelectListItem();
            item.Text = "En Progreso";
            if (parte.Estado == "En Progreso") item.Selected = true;
            else item.Selected = false;
            parte.Estados.Add(item);

            item = new SelectListItem();
            item.Text = "Finalizado";
            if (parte.Estado == "Finalizado") item.Selected = true;
            else item.Selected = false;
            parte.Estados.Add(item);

            return View(parte);
        }

        // GET: PArtes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parte parte = db.Partes.Find(id);
            if (parte == null)
            {
                return HttpNotFound();
            }
            return View(parte);
        }

        // POST: Partes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            db = new DataBaseContext();
            Parte parte = db.Partes.Find(id);

            List<int> Ids = new List<int>();

            foreach (HistoricoParte historico in parte.HistoricosPartes) Ids.Add(historico.Id);

            foreach(int item in Ids)
            {
                HistoricoParte historico = db.HistoricosPartes.Find(item);
                db.HistoricosPartes.Remove(historico);
            }

            //Lineas de partes
            if(parte.PartesLineas != null)
            {
                List<int> IdsPartesLineas = new List<int>();
                
                foreach(ParteLinea parteLinea in db.PartesLineas.Where(p => p.IDParte == parte.Id))
                {
                    if (parteLinea.HistoricosPartesLineas !=null)
                    {
                        foreach (HistoricoParteLinea historico in parteLinea.HistoricosPartesLineas) IdsPartesLineas.Add(historico.Id);

                        foreach (int item in IdsPartesLineas)
                        {
                            HistoricoParteLinea historico = db.HistoricosPartesLineas.Find(item);
                            db.HistoricosPartesLineas.Remove(historico);
                        }
                    }

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

                    db.PartesLineas.Remove(parteLinea);
                }
            }
            db.Partes.Remove(parte);

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


