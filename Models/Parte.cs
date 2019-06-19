using Foolproof;
using OSCPartes.App_Data;
using OSCPartes.Configuracion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace OSCPartes.Models
{
    [Table("partes")]
    public class Parte : IComparable
    {
        #region Members
        int _id;
        int _id_cliente;
        int _cliente_codigo;
        string _cliente_nif;
        string _cliente_nombre_fiscal;
        string _cliente_nombre_comercial;
        string _cliente_domicilio;
        string _cliente_cp;
        string _cliente_poblacion;
        string _cliente_provincia;
        string _cliente_pais;
        string _cliente_contacto_persona;
        string _cliente_contacto_telefono;
        string _cliente_contacto_email;
        int _id_tecnico;
        DateTime _fecha_alta = DateTime.Now;
        DateTime? _fecha_fin;
        string _tipo;
        string _horasFacturables;
        string _descripcion;
        bool _facturado = false;
        bool _enviado = false;
        string _estado;
        string _usuario_creacion;
        DateTime _fecha_creacion = DateTime.Now;

        string _clienteSeleccionado;
        string _tecnicoSeleccionado;
        string _tipoSeleccionado;
        string _estadoSeleccionado;

        string _emailTextInfo;

        private DataBaseContext db = new DataBaseContext();
        #endregion

        #region Properties
        [Display(Name = "Nº Parte: ")]
        [Key,Column("id")]
        public int Id
        {
            get { return _id; }
            set { _id = value; }        
        }

        [Display(Name = "Cliente")]
        [Column("id_cliente")]
        public int IDCliente
        {
            get { return _id_cliente; }
            set { _id_cliente = value; }
        }

        [Display(Name = "Código Cliente")]       
        [Column("cliente_codigo")]
        public int ClienteCodigo
        {
            get { return _cliente_codigo; }
            set { _cliente_codigo = value; }
        }

        [Display(Name = "NIF Cliente")]
        [Column("cliente_nif")]
        public string ClienteNIF
        {
            get { return _cliente_nif; }
            set { _cliente_nif = value; }
        }

        [Display(Name = "Nombre Fiscal")]
        [Column("cliente_nombre_fiscal")]
        public string ClienteNombreFiscal
        {
            get { return _cliente_nombre_fiscal; }
            set { _cliente_nombre_fiscal = value; }
        }

        [Display(Name = "Nombre Comercial Cliente")]
        [Column("cliente_nombre_comercial")]
        public string ClienteNombreComercial
        {
            get { return _cliente_nombre_comercial; }
            set { _cliente_nombre_comercial = value; }
        }

        [Display(Name = "Domicilio Cliente")]
        [Column("cliente_domicilio")]
        public string ClienteDomicilio
        {
            get { return _cliente_domicilio; }
            set { _cliente_domicilio = value; }
        }

        [Display(Name = "Código Postal Cliente")]
        [Column("cliente_cp")]
        public string ClienteCP
        {
            get { return _cliente_cp; }
            set { _cliente_cp = value; }
        }

        [Display(Name = "Población Cliente")]
        [Column("cliente_poblacion")]
        public string ClientePoblacion
        {
            get { return _cliente_poblacion; }
            set { _cliente_poblacion = value; }
        }

        [Display(Name = "Provincia Cliente")]
        [Column("cliente_provincia")]
        public string ClienteProvincia
        {
            get { return _cliente_provincia; }
            set { _cliente_provincia = value; }
        }

        [Display(Name = "País Cliente")]
        [Column("cliente_pais")]
        public string ClientePais
        {
            get { return _cliente_pais; }
            set { _cliente_pais = value; }
        }

        [Display(Name = "Persona Contacto")]
        [Column("cliente_contacto_persona")]
        public string ClienteContactoPersona
        {
            get { return _cliente_contacto_persona; }
            set { _cliente_contacto_persona = value; }
        }

        [Display(Name = "Teléfono")]
        [Column("cliente_contacto_telefono")]
        public string ClienteContactoTelefono
        {
            get { return _cliente_contacto_telefono; }
            set { _cliente_contacto_telefono = value; }
        }

        [Display(Name = "Email")]
        [Column("cliente_contacto_email")]
        public string ClienteContactoEmail
        {
            get { return _cliente_contacto_email; }
            set { _cliente_contacto_email = value; }
        }

        [Display(Name = "Técnico")]
        [Column("id_tecnico")]
        public int IDTecnico
        {
            get { return _id_tecnico; }
            set { _id_tecnico = value; }
        }

        [Display(Name = "Fecha de Alta")]
        [Required(ErrorMessage = "Fecha no válida")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        [Column("fecha_alta")]
        public DateTime FechaAlta
        {
            get { return _fecha_alta; }
            set { _fecha_alta = value; }
        }


        [Display(Name = "Fecha de Finalización")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        [Column("fecha_fin")]
        public DateTime? FechaFin
        {
            get { return _fecha_fin; }
            set { _fecha_fin = value; }
        }

        [Display(Name = "Tipo")]
        [Column("tipo")]
        public string Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }

        [Display(Name = "Horas Facturables")]
        [Column("horas_facturables")]
        public string HorasFacturables
        {
            get { return _horasFacturables; }
            set { _horasFacturables = value; }
        }

        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Descripción no válida")]
        [Column("descripcion")]
        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        [Display(Name = "Facturado")]
        [Column("facturado")]
        public bool Facturado
        {
            get { return _facturado; }
            set { _facturado = value; }
        }

        [Display(Name = "Enviado")]
        [Column("enviado")]
        public bool Enviado
        {
            get { return _enviado; }
            set { _enviado = value; }
        }

        [NotMapped]
        public List<SelectListItem> Estados { get; set; }

        [Display(Name = "Estado del Parte")]        
        [Column("estado")]
        public string Estado
        {
            get { return _estado; }
            set { _estado = value; }
        }

        [NotMapped]
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Estado no válido")]
        public string EstadoSeleccionado
        {
            get { return _estadoSeleccionado; }
            set { _estadoSeleccionado = value; }
        }

        [Display(Name = "Persona que Crea el Parte")]
        [Required(ErrorMessage = "Usuario no válido")]
        [Column("usuario_creacion")]
        public string UsuarioCreacion
        {
            get { return _usuario_creacion; }
            set { _usuario_creacion = value; }
        }

        [Display(Name = "Fecha de Creación")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        [Column("fecha_creacion")]
        public DateTime FechaCreacion
        {
            get { return _fecha_creacion; }
            set { _fecha_creacion = value; }
        }

        [NotMapped]
        public List<SelectListItem> Clientes { get; set; }

        [NotMapped]
        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "Cliente no válido")]
        public string ClienteSeleccionado
        {
            get { return _clienteSeleccionado; }
            set { _clienteSeleccionado = value; }
        }

        [NotMapped]
        public List<SelectListItem> Tecnicos { get; set; }

        [NotMapped]
        [Display(Name = "Tecnico")]
        [Required(ErrorMessage = "Tecnico no válido")]
        public string TecnicoSeleccionado
        {
            get { return _tecnicoSeleccionado; }
            set { _tecnicoSeleccionado = value; }
        }

        [NotMapped]
        public List<SelectListItem> Tipos { get; set; }

        [NotMapped]
        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "Tipo no válido")]
        public string TipoSeleccionado
        {
            get { return _tipoSeleccionado; }
            set { _tipoSeleccionado = value; }
        }

        [ForeignKey("IDCliente")]
        public virtual Cliente Cliente { get; set; }

        [ForeignKey("IDTecnico")]
        public virtual Tecnico Tecnico { get; set; }

        [InverseProperty("Parte")]
        virtual public List<HistoricoParte> HistoricosPartes { get; set; }

        [InverseProperty("Parte")]
        virtual public List<ParteLinea> PartesLineas { get; set; }

        [NotMapped]
        [Display(Name = "Horas Totales")]
        public string HorasString
        {
            get
            {
                TimeSpan ts = new TimeSpan(0, 0, 0);
                foreach (ParteLinea linea in PartesLineas)
                {
                    ts += linea.Horas;
                }
                return ts.TotalHours.ToString("N2");
            }
        }

        [NotMapped]
        [Display(Name = "Horas Facturables")]
        public string HorasFacturablesString
        {
            get
            {
                if (HorasFacturables == null || HorasFacturables == "")
                {
                    TimeSpan ts = new TimeSpan(0, 0, 0);
                    return ts.TotalHours.ToString("N2");
                }
                else
                {
                    TimeSpan ts = new TimeSpan(Int32.Parse(HorasFacturables.Substring(0, 2)), Int32.Parse(HorasFacturables.Substring(3, 2)), 0);
                    return ts.TotalHours.ToString("N2");
                }
            }
        }

        [NotMapped]
        [Display(Name = "Kms Totales")]
        public string KmsString
        {
            get
            {
                int km = 0;
                foreach (ParteLinea linea in PartesLineas)
                {
                    km += linea.KM;
                }
                return km.ToString();
            }
        }

        [NotMapped]
        [Display(Name = "EmailTextInfo")]
        public string EmailTextInfo
        {
            get { return _emailTextInfo; }
            set { _emailTextInfo = value; }
        }


        #endregion

        #region Constructor
        public Parte() { }
        #endregion

        #region IComparable Members
        public int CompareTo(Object obj)
        {
            if (obj == null) return 1;

            Parte otroElemento = obj as Parte;
            if (otroElemento != null) return this.Id.CompareTo(otroElemento.Id);
            else throw new ArgumentException("Object is not Parte");
        }
        #endregion
    }
}
