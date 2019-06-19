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
    [Table("partes_lineas")]
    public class ParteLinea : IComparable
    {
        #region Members
        int _id;
        int _id_parte;
        int _id_tecnico;
        int _id_vehiculo;
        DateTime _fecha = DateTime.Now;
        int _hora_inicio;
        int _minutos_inicio;
        int _hora_fin;
        int _minutos_fin;
        int _km;
        string _tipo;       
        string _descripcion;
        string _usuario_creacion;
        DateTime _fecha_creacion = DateTime.Now;

        string _inicio = "00:00";
        string _fin = "00:00";

        string _tipoSeleccionado;     
        List<string> _tecnicosSeleccionados;
        List<string> _vehiculosSeleccionados;

        private DataBaseContext db = new DataBaseContext();
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

        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        [Column("fecha")]
        public DateTime Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }
        [Display(Name = "Hora de Inicio")]
        [Column("hora_inicio")]
        public int HoraInicio
        {
            get { return _hora_inicio; }
            set { _hora_inicio = value; }
        }

        [Display(Name = "Minutos de Inicio")]
        [Column("minutos_inicio")]
        public int MinutosInicio
        {
            get { return _minutos_inicio; }
            set { _minutos_inicio = value; }
        }

        [Display(Name = "Hora de Finalización")]
        [Column("hora_fin")]
        public int HoraFin
        {
            get { return _hora_fin; }
            set { _hora_fin = value; }
        }

        [Display(Name = "Minutos de Finalización")]
        [Column("minutos_fin")]
        public int MinutosFin
        {
            get { return _minutos_fin; }
            set { _minutos_fin = value; }
        }

        [Display(Name = "Kilómetros")]
        [Required(ErrorMessage = "Kilómetros no válidos")]
        [Column("km")]
        public int KM
        {
            get { return _km; }
            set { _km = value; }
        }

        [Display(Name = "Tipo")]
        [Column("tipo")]
        public string Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
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

        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Descripción no válida")]
        [Column("descripcion")]
        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        [Display(Name = "Usuario que Crea la Linea del Parte")]
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

        [NotMapped]
        public List<SelectListItem> Tecnicos { get; set; }

        [NotMapped]
        [Display(Name = "Tecnicos")]
        [Required(ErrorMessage = "Técnico no válido")]
        public List<string> TecnicosSeleccionados
        {
            get { return _tecnicosSeleccionados; }
            set { _tecnicosSeleccionados = value; }
        }

        [NotMapped]
        public List<SelectListItem> Vehiculos { get; set; }

        [NotMapped]
        [Display(Name = "Vehiculos")]
        public List<string> VehiculosSeleccionados
        {
            get { return _vehiculosSeleccionados; }
            set { _vehiculosSeleccionados = value; }
        }

        [ForeignKey("IDParte")]
        public virtual Parte Parte { get; set; }

        [InverseProperty("ParteLinea")]
        virtual public List<HistoricoParteLinea> HistoricosPartesLineas { get; set; }

        [InverseProperty("ParteLinea")]
        virtual public List<ParteLineaTecnico> PartesLineasTecnicos { get; set; }

        [InverseProperty("ParteLinea")]
        virtual public List<ParteLineaVehiculo> PartesLineasVehiculos { get; set; }

         
        [NotMapped]
        [Display(Name = "Hora de Inicio")]
        public string VerHoraInicio
        {
            get
            {
                string hora = "";
                if (HoraInicio < 10) hora = "0" + HoraInicio + ":"; 
                else hora = HoraInicio + ":";
                if (MinutosInicio < 10) hora += "0" + MinutosInicio;
                else hora += MinutosInicio;
                return hora;
            }
        }      

        [NotMapped]
        [Display(Name = "Hora de Inicio")]
        [Required(ErrorMessage = "Hora no válida")]
        public string Inicio
        {
            get { return _inicio; }
            set { _inicio = value; }
        }

         
        [NotMapped]
        [Display(Name = "Hora de Finalicazión")]
        public string VerHoraFin
        {
            get
            {
                string hora = "";
                if (HoraFin < 10) hora = "0" + HoraFin + ":"; 
                else hora = HoraFin + ":";
                if (MinutosFin < 10) hora += "0" + MinutosFin;
                else hora += MinutosFin;
                return hora;
            }
        }

        [NotMapped]
        [Display(Name = "Hora de Finalicazión")]
        [Required(ErrorMessage = "Hora no válida")]
        public string Fin
        {
            get { return _fin; }
            set { _fin = value; }
        }

        [NotMapped]
        public string HorasString
        {
            get
            {
                TimeSpan ts = (new TimeSpan(HoraFin, MinutosFin, 0)) - (new TimeSpan(HoraInicio, MinutosInicio, 0));
                return ts.TotalHours.ToString("N2");
            }
        }

        [NotMapped]
        public TimeSpan Horas
        {
            get
            {
                Parte parte = db.Partes.Find(IDParte);
                if (parte!= null && parte.Tipo == "Por Horas")
                {
                    int horaCalc = ((HoraFin * 60) + MinutosFin) - ((HoraInicio * 60) + MinutosInicio);
                    int hora = 0;
                    int minutos = 0;                 
                    
                    if (Tipo == "In Situ")
                    {
                        if (horaCalc < 60)
                        {
                            hora = 1;
                        }
                        else
                        {
                            hora = horaCalc / 60;
                            horaCalc %= 60;
                            if (horaCalc <= 30)
                            {
                                minutos = 30;
                            }
                            else
                            {
                                hora++;
                            }
                        }
                    }
                    else
                    {
                        hora = horaCalc / 60;
                        horaCalc %= 60;
                        if (horaCalc <= 30)
                        {
                            minutos = 30;
                        }
                        else
                        {
                            hora++;
                        }
                    }
                    return new TimeSpan(hora, minutos, 0);
                }



                return (new TimeSpan(HoraFin, MinutosFin, 0)) - (new TimeSpan(HoraInicio, MinutosInicio, 0));
            }
        }

        [NotMapped]
        [Display(Name = "Tecnicos")]
        public string ResumenTecnicos
        {
            get
            {
                string result = "";
                if (PartesLineasTecnicos != null)
                {
                    foreach (ParteLineaTecnico tecnico in PartesLineasTecnicos)
                    {
                        if (result != "") result += ", ";
                        result += tecnico.Tecnico.Nombre;
                    }
                }
                return result;
            }
        }

        [NotMapped]
        [Display(Name = "Vehiculos")]
        public string ResumenVehiculos
        {
            get
            {
                string result = "";
                if (PartesLineasVehiculos != null)
                {
                    foreach (ParteLineaVehiculo tecnico in PartesLineasVehiculos)
                    {
                        if (result != "") result += ", ";
                        result += tecnico.Vehiculo.Descripcion;
                    }
                }
                return result;
            }
        }

        #endregion

        #region Constructor
        public ParteLinea() { }
        #endregion

        #region IComparable Members
        public int CompareTo(Object obj)
        {
            if (obj == null) return 1;

            ParteLinea otroElemento = obj as ParteLinea;
            if (otroElemento != null) return this.Id.CompareTo(otroElemento.Id);
            else throw new ArgumentException("Object is not ParteLinea");
        }
        #endregion
    }
}
