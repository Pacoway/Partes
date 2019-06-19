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
    [Table("partes_lineas_vehiculos")]
    public class ParteLineaVehiculo : IComparable
    {
        #region Members
        int _id;
        int _id_parte_linea;
        int _id_vehiculo;  

        private DataBaseContext db = new DataBaseContext();
        #endregion

        #region Properties
        [Key,Column("id")]
        public int Id
        {
            get { return _id; }
            set { _id = value; }        
        }

        [Column("id_parte_linea")]
        public int IDParteLinea
        {
            get { return _id_parte_linea; }
            set { _id_parte_linea = value; }
        }

        [Column("id_vehiculo")]
        public int IDVehiculo
        {
            get { return _id_vehiculo; }
            set { _id_vehiculo = value; }
        }

        [ForeignKey("IDParteLinea")]
        public virtual ParteLinea ParteLinea { get; set; }

        [ForeignKey("IDVehiculo")]
        public virtual Vehiculo Vehiculo { get; set; }
        #endregion

        #region Constructor
        public ParteLineaVehiculo() { }
        #endregion

        #region IComparable Members
        public int CompareTo(Object obj)
        {
            if (obj == null) return 1;

            ParteLineaVehiculo otroElemento = obj as ParteLineaVehiculo;
            if (otroElemento != null) return this.Id.CompareTo(otroElemento.Id);
            else throw new ArgumentException("Object is not ParteLineaVehiculo");
        }
        #endregion
    }
}
