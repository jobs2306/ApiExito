using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ApiExito.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;


namespace ApiExito.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        //Constructor de la clase
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Vehiculo> Vehiculo { get; set; }
        public DbSet<ControlVehiculo> ControlVehiculo { get; set; }
    }
}
