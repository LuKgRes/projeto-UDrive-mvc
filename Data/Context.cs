using Microsoft.EntityFrameworkCore;
using Proyecto_Programacion_III.Models.Entidades;

namespace Proyecto_Programacion_III.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }


        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Servicos> Servicos { get; set; }
        public DbSet<Agendamentos> Agendamentos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cliente
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.Identificacion)
                .IsUnique();

            // Usuario
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Agendamento -> Usuario (1 usuário tem N Agendamentos)
            modelBuilder.Entity<Agendamentos>()
                .HasOne(a => a.Usuario)
                .WithMany(u => u.Agendamentos)
                .HasForeignKey(a => a.UsuarioId);

            // Agendamento -> Cliente (1 cliente tem N Agendamentos)
            modelBuilder.Entity<Agendamentos>()
                .HasOne(a => a.Cliente)
                .WithMany(c => c.Agendamentos)
                .HasForeignKey(a => a.ClienteId);

            // Agendamento -> Servico (1 serviço tem N Agendamentos)
            modelBuilder.Entity<Agendamentos>()
                .HasOne(a => a.Servicos)
                .WithMany(s => s.Agendamentos)
                .HasForeignKey(a => a.ServicosId);
        }
    }
}
