using Microsoft.EntityFrameworkCore;
using ControlePonto.Models;

namespace ControlePonto.Data
{
    /// <summary>
    /// Contexto do banco de dados para o sistema de controle de ponto
    /// </summary>
    public class ControlePontoContext : DbContext
    {
        public ControlePontoContext(DbContextOptions<ControlePontoContext> options) : base(options)
        {
        }

        // DbSets para as entidades
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<RegistroPonto> RegistrosPonto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade Funcionario
            modelBuilder.Entity<Funcionario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Cargo).IsRequired().HasMaxLength(80);
                
                // Índice único para email
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configuração da entidade RegistroPonto
            modelBuilder.Entity<RegistroPonto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FuncionarioId).IsRequired();
                entity.Property(e => e.DataHoraEntrada).IsRequired();
                entity.Property(e => e.DataHoraSaida);

                // Relacionamento com Funcionario
                entity.HasOne(e => e.Funcionario)
                      .WithMany(f => f.RegistrosPonto)
                      .HasForeignKey(e => e.FuncionarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Dados de seed para desenvolvimento
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Funcionários de exemplo
            modelBuilder.Entity<Funcionario>().HasData(
                new Funcionario
                {
                    Id = 1,
                    Nome = "João Silva",
                    Email = "joao.silva@empresa.com",
                    Cargo = "Desenvolvedor"
                },
                new Funcionario
                {
                    Id = 2,
                    Nome = "Maria Santos",
                    Email = "maria.santos@empresa.com",
                    Cargo = "Analista de Sistemas"
                },
                new Funcionario
                {
                    Id = 3,
                    Nome = "Pedro Oliveira",
                    Email = "pedro.oliveira@empresa.com",
                    Cargo = "Gerente de Projetos"
                }
            );

            // Registros de ponto de exemplo
            modelBuilder.Entity<RegistroPonto>().HasData(
                new RegistroPonto
                {
                    Id = 1,
                    FuncionarioId = 1,
                    DataHoraEntrada = DateTime.Today.AddHours(8),
                    DataHoraSaida = DateTime.Today.AddHours(17)
                },
                new RegistroPonto
                {
                    Id = 2,
                    FuncionarioId = 2,
                    DataHoraEntrada = DateTime.Today.AddHours(9),
                    DataHoraSaida = DateTime.Today.AddHours(18)
                },
                new RegistroPonto
                {
                    Id = 3,
                    FuncionarioId = 1,
                    DataHoraEntrada = DateTime.Today.AddDays(-1).AddHours(8),
                    DataHoraSaida = DateTime.Today.AddDays(-1).AddHours(16)
                }
            );
        }
    }
}
