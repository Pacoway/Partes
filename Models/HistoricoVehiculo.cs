using OSCPartes.Configuracion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace OSCPartes.Models
{
    [Table("historicos_vehiculos")]
    public class HistoricoVehiculo : IComparable
    {
        #region Members
        int _id;
        int _id_vehiculo;
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

        [Column("id_vehiculo")]
        public int IDVehiculo
        {
            get { return _id_vehiculo; }
            set { _id_vehiculo = value; }
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

        [ForeignKey("IDVehiculo")]
        public virtual Vehiculo Vehiculo { get; set; }

        #endregion

        #region Constructor
        public HistoricoVehiculo() { }
        #endregion

        #region IComparable Members
        public int CompareTo(Object obj)
        {
            if (obj == null) return 1;

            HistoricoVehiculo otroElemento = obj as HistoricoVehiculo;
            if (otroElemento != null) return this.Id.CompareTo(otroElemento.Id);
            else throw new ArgumentException("Object is not HistoricoVehiculo");
        }
        #endregion
    }
}
