using System.Data.Common;
using CSharp_Mascotas2.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CSharp_Mascotas2
{
    public class AppDbContext : DbContext
    {

        public DbSet<Usuario> Usuarios2 { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

    }
}