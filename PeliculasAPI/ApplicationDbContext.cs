using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PeliculasAPI.Entidades;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace PeliculasAPI
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {   
        } 
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<PeliculasActores>()
                .HasKey(pa => new { pa.ActorId, pa.PeliculaId });

            modelBuilder.Entity<PeliculasGeneros>()
                .HasKey(pg => new { pg.GeneroId, pg.PeliculaId });

            modelBuilder.Entity<PeliculasSalasDeCine>()
                .HasKey(ps => new { ps.PeliculaId, ps.SalaDeCineId });



            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            //Generamos un rolAdminId y un usuarioIdAdmin por defecto y aleatorio con permisos de Admin con Online GUIDE Generator 
            var rolAdminId = "1fdd5e62-121c-45f7-8c44-8238637954d3";
            var usuarioAdminId = "24e30796-ed46-4943-aa9a-2215c16a57c3";


            var rolAdmin = new IdentityRole()
            {
                Id = rolAdminId,
                Name = "Admin",
                NormalizedName = "Admin"
            };

            var passwordHasher = new PasswordHasher<IdentityUser>();

            var username = "project.siglo@gmail.com";

            var usuarioAdmin = new IdentityUser()
            {
                Id = usuarioAdminId,
                UserName = username,
                NormalizedUserName = username,
                Email = username,
                NormalizedEmail = username,
                PasswordHash = passwordHasher.HashPassword(null, "Aa123456!")
            };

            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid:4326);

            /*Primero sse debe correr la migración:
             * Add-Migration TablasIdentity --> para crear las tablas Identity
             * Update-Database --> para confirmar la creación
             * por lo cual el siguiente código se debe comentar ya que las tablas aún no existen, 
             * una vez se corra la migarción se descomenta el código y corremos la  migración:
             * Add-Migration AdminData
             * Update-Database
             */

            ////Inicio comentario
            //modelBuilder.Entity<IdentityUser>()
            //    .HasData(usuarioAdmin);

            //modelBuilder.Entity<IdentityRole>()
            //    .HasData(rolAdmin);

            //modelBuilder.Entity<IdentityUserClaim<string>>()
            //    .HasData(new IdentityUserClaim<string>()
            //    {
            //        Id = 1,
            //        ClaimType = ClaimTypes.Role,
            //        UserId = usuarioAdminId,
            //        ClaimValue = "Admin"
            //    });
            ////Fin comentario

            modelBuilder.Entity<SalaDeCine>()
               .HasData(new List<SalaDeCine>
               {
                    new SalaDeCine{Id = 1003, Nombre = "Agora", Ubicacion = geometryFactory.CreatePoint(new Coordinate(-69.9388777, 18.4839233))},
                    new SalaDeCine{Id = 1004, Nombre = "Sambil", Ubicacion = geometryFactory.CreatePoint(new Coordinate(-69.9118804, 18.4826214))},
                    new SalaDeCine{Id = 1005, Nombre = "Megacentro", Ubicacion = geometryFactory.CreatePoint(new Coordinate(-69.856427, 18.506934))},
                    new SalaDeCine{Id = 1006, Nombre = "Village East Cinema", Ubicacion = geometryFactory.CreatePoint(new Coordinate(-73.986227, 40.730898))}
               });


            //Registros para poblar tabla Actores
            var jimCarrey = new Actor() 
            {
                Id = 3, Nombre = "Jim Carrey", 
                FechaNacimiento = new DateTime(1962, 01, 17) 
            };
            
            var robertDowney = new Actor() 
            { 
                Id = 4, 
                Nombre = "Robert Downey Jr.", 
                FechaNacimiento = new DateTime(1965, 4, 4) 
            };
            
            var chrisEvans = new Actor() 
            { 
                Id = 5, Nombre = "Chris Evans", 
                FechaNacimiento = new DateTime(1981, 06, 13) };

            modelBuilder.Entity<Actor>()
                .HasData(new List<Actor>
                {
                    jimCarrey, robertDowney, chrisEvans
                });

            //Registros para poblar tabla Peliculas
            var endgame = new Pelicula()
            {
                Id = 3,
                Titulo = "Avengers: Endgame",
                EnCines = true,
                FechaEstreno = new DateTime(2019, 04, 26)
            };

            var iw = new Pelicula()
            {
                Id = 4,
                Titulo = "Avengers: Infinity Wars",
                EnCines = false,
                FechaEstreno = new DateTime(2019, 04, 26)
            };

            var sonic = new Pelicula()
            {
                Id = 5,
                Titulo = "Sonic the Hedgehog",
                EnCines = false,
                FechaEstreno = new DateTime(2020, 02, 28)
            };
            var emma = new Pelicula()
            {
                Id = 6,
                Titulo = "Emma",
                EnCines = false,
                FechaEstreno = new DateTime(2020, 02, 21)
            };
            var wonderwoman = new Pelicula()
            {
                Id = 7,
                Titulo = "Wonder Woman 1984",
                EnCines = false,
                FechaEstreno = new DateTime(2020, 08, 14)
            };

            modelBuilder.Entity<Pelicula>()
                .HasData(new List<Pelicula>
                {
                    endgame, iw, sonic, emma, wonderwoman
                });
        }


        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }

        public DbSet<PeliculasActores> PeliculasActores { get; set; }
        public DbSet<PeliculasGeneros> PeliculasGeneros { get; set; }
        public DbSet<SalaDeCine> SalasDeCine { get; set; }
        public DbSet<PeliculasSalasDeCine> PeliculasSalasDeCines { get; set; }

    }
}
