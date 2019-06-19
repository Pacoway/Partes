using OSCPartes.Configuracion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace OSCPartes.Models
{
    [Table("historicos_partes")]
    public class HistoricoParte : IComparable
    {
        #region Members
        int _id;
        int _id_parte;
        string _descripcion;
        DateTime _fecha = DateTime.Now;
        string _usuario;
        #endregion

        #region Properties
        [Key,Column("id")]
        public int Id
        {
            get { return _id; }
            set { _id = value; }        
        }

        [Column("id_parte")]
        public int IDParte
        {
            get { return _id_parte; }
            set { _id_parte = value; }
        }

        [Display(Name = "Descripción")]
        [Column("descripcion")]
        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        [Display(Name = "Fecha de Actualización")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        [Column("fecha")]
        public DateTime Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }

        [Display(Name = "Usuario")]
        [Column("usuario")]
        public string Usuario
        {
            get { return _usuario; }
            set { _usuario = value; }
        }

        [ForeignKey("IDParte")]
        public virtual Parte Parte { get; set; }

        #endregion

        #region Constructor
        public HistoricoParte() { }
        #endregion

        #region IComparable Members
        public int CompareTo(Object obj)
        {
            if (obj == null) return 1;

            HistoricoParte otroElemento = obj as HistoricoParte;
            if (otroElemento != null) return this.Id.CompareTo(otroElemento.Id);
            else throw new ArgumentException("Object is not HistoricoParte");
        }
        #endregion
    }
}
