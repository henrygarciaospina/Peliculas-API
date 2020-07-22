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

        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
    }
}
