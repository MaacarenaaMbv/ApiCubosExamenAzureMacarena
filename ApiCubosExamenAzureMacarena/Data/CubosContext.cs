using ApiCubosExamenAzureMacarena.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCubosExamenAzureMacarena.Data
{
    public class CubosContext: DbContext
    {
        public CubosContext(DbContextOptions<CubosContext> options) : base(options) { }

        public DbSet<Cubo> Cubos { get; set; }
        public DbSet<CompraCubos> CompraCubos { get;set; }
        public DbSet<UsuarioCubo> UsuarioCubos { get;set ;}
    }
}
