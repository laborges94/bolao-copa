using Microsoft.EntityFrameworkCore;
using Bolao.Models;

namespace Bolao.Data
{
    public class BolaoDbContext : DbContext
    {
        public BolaoDbContext(DbContextOptions<BolaoDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Models.Bolao> Boloes { get; set; } = null!;
        public DbSet<Participante> Participantes { get; set; } = null!;
        public DbSet<Selecao> Selecoes { get; set; } = null!;
        public DbSet<Fase> Fases { get; set; } = null!;
        public DbSet<Partida> Partidas { get; set; } = null!;
        public DbSet<Palpite> Palpites { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure uniqueness
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Models.Bolao>()
                .HasIndex(b => b.CodigoConvite)
                .IsUnique();

            // Unique participant per sweepstake (bolao)
            modelBuilder.Entity<Participante>()
                .HasIndex(p => new { p.BolaoId, p.UsuarioId })
                .IsUnique();

            // Unique guess per match per participant
            modelBuilder.Entity<Palpite>()
                .HasIndex(p => new { p.PartidaId, p.ParticipanteId })
                .IsUnique();

            // Configure relationships for Partida to avoid multiple cascade paths on Selecao
            modelBuilder.Entity<Partida>()
                .HasOne(p => p.TimeCasa)
                .WithMany()
                .HasForeignKey(p => p.TimeCasaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Partida>()
                .HasOne(p => p.TimeVisitante)
                .WithMany()
                .HasForeignKey(p => p.TimeVisitanteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
