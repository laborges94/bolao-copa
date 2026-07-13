using System;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Bolao.Data;
using Bolao.Models;
using Xunit;

namespace Bolao.Tests
{
    public class SeedDataTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<BolaoDbContext> _contextOptions;

        public SeedDataTests()
        {
            // Create and open a connection to a one-time in-memory database
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // Construct DbContextOptions for SQLite using the open connection
            _contextOptions = new DbContextOptionsBuilder<BolaoDbContext>()
                .UseSqlite(_connection)
                .Options;
        }

        [Fact]
        public void Initialize_ShouldSeedQuartasAndSemifinais()
        {
            // Arrange
            using var context = new BolaoDbContext(_contextOptions);
            
            // Act
            SeedData.Initialize(context);

            // Assert
            var fases = context.Fases.OrderBy(f => f.Ordem).ToList();
            Assert.Equal(2, fases.Count);
            
            var quartas = fases[0];
            Assert.Equal("Quartas de final", quartas.Nome);
            Assert.Equal(2, quartas.Ordem);

            var semifinal = fases[1];
            Assert.Equal("Semifinal", semifinal.Nome);
            Assert.Equal(3, semifinal.Ordem);

            var partidas = context.Partidas.Include(p => p.TimeCasa).Include(p => p.TimeVisitante).OrderBy(p => p.Numero).ToList();
            Assert.Equal(6, partidas.Count);

            // Match 5: França vs Espanha
            var match5 = partidas[4];
            Assert.Equal(5, match5.Numero);
            Assert.Equal("FRA", match5.TimeCasa?.Sigla);
            Assert.Equal("ESP", match5.TimeVisitante?.Sigla);
            Assert.Equal(semifinal.Id, match5.FaseId);
            Assert.False(match5.Finalizada);

            // Match 6: Inglaterra vs Argentina
            var match6 = partidas[5];
            Assert.Equal(6, match6.Numero);
            Assert.Equal("ENG", match6.TimeCasa?.Sigla);
            Assert.Equal("ARG", match6.TimeVisitante?.Sigla);
            Assert.Equal(semifinal.Id, match6.FaseId);
            Assert.False(match6.Finalizada);
        }

        [Fact]
        public void Initialize_IncrementalSeeding_ShouldNotOverwriteExistingData()
        {
            // Arrange
            using var context = new BolaoDbContext(_contextOptions);
            SeedData.Initialize(context);

            // Let's simulate existing user data: create a bolao, a participant, and a guess for match 1
            var usuario = new Usuario
            {
                Nome = "Usuário Teste",
                Email = "teste@teste.com",
                Senha = "password"
            };
            context.Usuarios.Add(usuario);
            context.SaveChanges();

            var bolao = new Models.Bolao
            {
                Nome = "Bolão de Teste",
                CodigoConvite = "TEST-1234",
                CriadoEm = DateTime.UtcNow,
                CriadoPorId = usuario.Id
            };
            context.Boloes.Add(bolao);
            context.SaveChanges();

            var participante = new Participante
            {
                BolaoId = bolao.Id,
                UsuarioId = usuario.Id,
                NomeExibicao = "Participante Teste",
                EntrouEm = DateTime.UtcNow
            };
            context.Participantes.Add(participante);
            context.SaveChanges();

            var palpite = new Palpite
            {
                PartidaId = 1, // França x Marrocos
                ParticipanteId = participante.Id,
                GolsCasa = 2,
                GolsVisitante = 1
            };
            context.Palpites.Add(palpite);
            context.SaveChanges();

            // Clear context trackings
            context.ChangeTracker.Clear();

            // Act - Run Initialize again simulating restart of application with Semifinal updates
            SeedData.Initialize(context);

            // Assert
            // 1. Data should still exist (no deletions/resets because "Quartas de final" is already seeded)
            var countBoloes = context.Boloes.Count();
            Assert.Equal(1, countBoloes);

            var countPalpites = context.Palpites.Count();
            Assert.Equal(1, countPalpites);

            var savedPalpite = context.Palpites.First();
            Assert.Equal(2, savedPalpite.GolsCasa);
            Assert.Equal(1, savedPalpite.GolsVisitante);

            // 2. Semifinal matches should still be seeded
            var countPartidas = context.Partidas.Count();
            Assert.Equal(6, countPartidas);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
