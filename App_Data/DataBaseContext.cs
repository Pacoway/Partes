using OSCPartes.Configuracion;
using OSCPartes.Models;
using System;
using System.Data.Entity;

namespace OSCPartes.App_Data
{

    public class DataBaseContext: DbContext
    {
        public DataBaseContext() : base("Data Source=workstation id = OSCPartes.mssql.somee.com; packet size = 4096; user id = Pacoway_SQLLogin_1; pwd=ex8pwxtgvl;data source = OSCPartes.mssql.somee.com; persist security info=False;initial catalog = OSCPartes;MultipleActiveResultSets=True") { }



        public DbSet<Parte> Partes { get; set; }
        public DbSet<HistoricoParte> HistoricosPartes { get; set; }
        public DbSet<Tecnico> Tecnicos { get; set; }
        public DbSet<HistoricoTecnico> HistoricosTecnicos { get; set; }
        public DbSet<ParteLinea> PartesLineas { get; set; }
        public DbSet<HistoricoParteLinea> HistoricosPartesLineas { get; set; }
        public DbSet<ParteLineaTecnico> PartesLineasTecnicos { get; set; }
        public DbSet<ParteLineaVehiculo> PartesLineasVehiculos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<HistoricoCliente> HistoricosClientes { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<HistoricoVehiculo> HistoricosVehiculos { get; set; }

        // General
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }

}
