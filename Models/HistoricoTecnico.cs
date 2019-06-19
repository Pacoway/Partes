using OSCPartes.Configuracion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace OSCPartes.Models
{
    [Table("historicos_tecnicos")]
    public class HistoricoTecnico : IComparable
    {
        #region Members
        int _id;
        int _id_tecnico;
        string _descripcion;
        string _usuario;
        DateTime _fecha = DateTime.Now;
        #endregion

        #region Properties
        [Key,Column("id")]
        public int Id
        {
            get { return _id; }
            set { _id = value; }        
        }

        [Column("id_tecnico")]
        public int IDTecnico
        {
            get { return _id_tecnico; }
            set { _id_tecnico = value; }
        }

        [Display(Name = "Descripción")]
        [Column("descripcion")]
        [Required(ErrorMessage = "Descripcion no válida")]
        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        [Display(Name = "Fecha de Actualización")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        [Column("fecha")]
        [Required(ErrorMessage = "Fecha no válida")]
        public DateTime Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }

        [Display(Name = "Usuario")]
        [Column("usuario")]
        [Required(ErrorMessage = "Usuario no válido")]
        public string Usuario
        {
            get { return _usuario; }
            set { _usuario = value; }
        }

        [ForeignKey("IDTecnico")]
        public virtual Tecnico Tecnico { get; set; }

        #endregion

        #region Constructor
        public HistoricoTecnico() { }
        #endregion

        #region IComparable Members
        public int CompareTo(Object obj)
        {
            if (obj == null) return 1;

            HistoricoTecnico otroElemento = obj as HistoricoTecnico;
            if (otroElemento != null) return this.Id.CompareTo(otroElemento.Id);
            else throw new ArgumentException("Object is not HistoricoTecnico");
        }
        #endregion
    }
}
