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
    [Table("tecnicos")]
    public class Tecnico : IComparable
    {
        #region Members
        int _id;
        string _nombre;
        string _email;       
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

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Nombre no válido")]
        [Column("nombre")]
        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email no válido")]
        [Column("email")]
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        [Display(Name = "Usuario que Crea el Tecnico")]
        [Required(ErrorMessage = "Usuario no válido")]
        [Column("usuario_creacion")]
        public string UsuarioCreacion
        {
            get { return _usuario_creacion; }
            set { _usuario_creacion = value; }
        }

        [Display(Name = "Fecha de Alta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        [Column("fecha_creacion")]
        public DateTime FechaCreacion
        {
            get { return _fecha_creacion; }
            set { _fecha_creacion = value; }
        }      
     
        [InverseProperty("Tecnico")]
        virtual public List<HistoricoTecnico> HistoricosTecnicos { get; set; }

        [InverseProperty("Tecnico")]
        virtual public List<Parte> Partes { get; set; }

        [InverseProperty("Tecnico")]
        virtual public List<ParteLineaTecnico> PartesLineasTecnicos { get; set; }

        #endregion

        #region Constructor
        public Tecnico() { }
        #endregion

        #region IComparable Members
        public int CompareTo(Object obj)
        {
            if (obj == null) return 1;

            Tecnico otroElemento = obj as Tecnico;
            if (otroElemento != null) return this.Id.CompareTo(otroElemento.Id);
            else throw new ArgumentException("Object is not Tecnico");
        }
        #endregion
    }
}
