using LaptoManiaOficial.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LaptoManiaOficial.Contexto
{
    public class MiContext : DbContext

    {
        public MiContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Secretaria> Secretarias { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Equipo>? Equipos { get; set; }
        public DbSet<Cliente>? Clientes { get; set; }

        public IEnumerable<object> Usuarioss { get; internal set; }

    }
}
