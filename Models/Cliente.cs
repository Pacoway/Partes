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
    [Table("clientes")]
    public class Cliente : IComparable
    {
        #region Members
        int _id;
        int _codigo;
        string _nif;
        string _nombre_fiscal;
        string _nombre_comercial;
        string _domicilio;
        string _cp;
        string _poblacion;
        string _provincia;
        string _pais;
        string _contacto_persona;
        string _contacto_telefono;
        string _contacto_email;
        string _usuario_creacion;
        DateTime _fecha_creacion = DateTime.Now;
         
        private DataBaseContext db = new DataBaseContext();
        #endregion

        #region Properties
        [Key,Column("id")]
        public int Id
        {
            get { return _id; }
            set { _id = value; }        
        }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "Código no válido")]
        [Column("codigo")]
        public int Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        [Display(Name = "NIF")]
        [Required(ErrorMessage = "NIF no válido")]
        [Column("nif")]
        public string NIF
        {
            get { return _nif; }
            set { _nif = value; }
        }

        [Display(Name = "Nombre Fiscal")]
        [Required(ErrorMessage = "Nombre Fiscal no válido")]
        [Column("nombre_fiscal")]
        public string NombreFiscal
        {
            get { return _nombre_fiscal; }
            set { _nombre_fiscal = value; }
        }

        [Display(Name = "Nombre Comercial")]
        [Column("nombre_comercial")]
        public string NombreComercial
        {
            get { return _nombre_comercial; }
            set { _nombre_comercial = value; }
        }

        [Display(Name = "Domicilio")]
        [Column("domicilio")]
        public string Domicilio
        {
            get { return _domicilio; }
            set { _domicilio = value; }
        }

        [Display(Name = "CP")]
        [Column("cp")]
        public string CP
        {
            get { return _cp; }
            set { _cp = value; }
        }

        [Display(Name = "Poblacion")]
        [Column("poblacion")]
        public string Poblacion
        {
            get { return _poblacion; }
            set { _poblacion = value; }
        }

        [Display(Name = "Provincia")]
        [Column("provincia")]
        public string Provincia
        {
            get { return _provincia; }
            set { _provincia = value; }
        }

        [Display(Name = "País")]
        [Column("pais")]
        public string Pais
        {
            get { return _pais; }
            set { _pais = value; }
        }

        [Display(Name = "Persona de Contacto")]
        [Column("contacto_persona")]
        public string ContactoPersona
        {
            get { return _contacto_persona; }
            set { _contacto_persona = value; }
        }

        [Display(Name = "Teléfono de Contacto")]
        [Column("contacto_telefono")]
        public string ContactoTelefono
        {
            get { return _contacto_telefono; }
            set { _contacto_telefono = value; }
        }

        [Display(Name = "Email de Contacto")]
        [Column("contacto_email")]
        public string ContactoEmail
        {
            get { return _contacto_email; }
            set { _contacto_email = value; }
        }

        [Display(Name = "Persona que Crea el Cliente")]
        [Required(ErrorMessage ="Usuario no válido")]
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

        [InverseProperty("Cliente")]
        virtual public List<HistoricoCliente> HistoricosClientes { get; set; }

        [InverseProperty("Cliente")]
        virtual public List<Parte> Partes { get; set; }
        #endregion

        #region Constructor
        public Cliente() { }
        #endregion

        #region IComparable Members
        public int CompareTo(Object obj)
        {
            if (obj == null) return 1;

            Cliente otroElemento = obj as Cliente;
            if (otroElemento != null) return this.Id.CompareTo(otroElemento.Id);
            else throw new ArgumentException("Object is not Cliente");
        }
        #endregion
    }
}
