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
    [Table("vehiculos")]
    public class Vehiculo : IComparable
    {
        #region Members
        int _id;
        string _descripcion;
        string _matricula;       
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

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Descripción no válida")]
        [Column("descripcion")]
        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        [Display(Name = "Matricula")]
        [Required(ErrorMessage = "Matricula no válida")]
        [Column("matricula")]
        public string Matricula
        {
            get { return _matricula; }
            set { _matricula = value; }
        }

        [Display(Name = "Usuario de Alta")]
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
     
        [InverseProperty("Vehiculo")]
        virtual public List<HistoricoVehiculo> HistoricosVehiculos { get; set; }

        [InverseProperty("Vehiculo")]
        virtual public List<ParteLineaVehiculo> PartesLineasVehiculos { get; set; }

        #endregion

        #region Constructor
        public Vehiculo() { }
        #endregion

        #region IComparable Members
        public int CompareTo(Object obj)
        {
            if (obj == null) return 1;

            Vehiculo otroElemento = obj as Vehiculo;
            if (otroElemento != null) return this.Id.CompareTo(otroElemento.Id);
            else throw new ArgumentException("Object is not Vehiculo");
        }
        #endregion
    }
}
