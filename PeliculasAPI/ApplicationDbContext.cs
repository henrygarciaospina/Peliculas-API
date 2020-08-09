using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace PeliculasAPI
{
    public class ApplicationDbContext : DbContext
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

            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
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
    }
}
