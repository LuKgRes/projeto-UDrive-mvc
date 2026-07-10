using Microsoft.EntityFrameworkCore;
using Proyecto_Programacion_III.Models.Entidades;
using Proyecto_Programacion_III.Models;


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
        public DbSet<AgendamentoPersonalizado> AgendamentoPersonalizados { get; set; }
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
                .HasOne(a => a.Cliente)
                .WithMany(u => u.Agendamentos)
                .HasForeignKey(a => a.ClienteId);

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

            modelBuilder.Entity<Servicos>().HasData(
         new Servicos { ServicosId = 1, Nome = "Troca de óleo + filtros", Descricao = "Troca de óleo do motor", Valor = 235.00m, Personalizado = false },
         new Servicos { ServicosId = 2, Nome = "Troca de pastilhas", Descricao = "Troca das pastilhas de freio para manter a eficiência e a segurança da frenagem", Valor = 320.00m, Personalizado = false },
         new Servicos { ServicosId = 3, Nome = "Alinhamento",Descricao = "Ajuste da direção para manter as rodas alinhadas e evitar desgaste irregular", Valor = 140.00m, Personalizado = false },
         new Servicos { ServicosId = 4, Nome = "Balanceamento", Descricao = "Balanceamento das rodas para reduzir vibrações e aumentar a estabilidade", Valor = 100.00m, Personalizado = false },
         new Servicos { ServicosId = 5, Nome = "Geral", Descricao = "Diagnóstico e reparo de componentes mecânicos do veículo", Valor = 320.00m, Personalizado = false }
            );

        }
    }
}
