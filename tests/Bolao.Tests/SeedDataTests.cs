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
        public void Initialize_ShouldSeedAllPhasesAndMatches()
        {
            // Arrange
            using var context = new BolaoDbContext(_contextOptions);
            
            // Act
            SeedData.Initialize(context);

            // Assert
            var fases = context.Fases.OrderBy(f => f.Ordem).ToList();
            Assert.Equal(4, fases.Count);
            
            var quartas = fases[0];
            Assert.Equal("Quartas de final", quartas.Nome);
            Assert.Equal(2, quartas.Ordem);

            var semifinal = fases[1];
            Assert.Equal("Semifinal", semifinal.Nome);
            Assert.Equal(3, semifinal.Ordem);

            var terceiro = fases[2];
            Assert.Equal("Disputa de 3º lugar", terceiro.Nome);
            Assert.Equal(4, terceiro.Ordem);

            var final = fases[3];
            Assert.Equal("Final", final.Nome);
            Assert.Equal(5, final.Ordem);

            var partidas = context.Partidas.Include(p => p.TimeCasa).Include(p => p.TimeVisitante).OrderBy(p => p.Numero).ToList();
            Assert.Equal(8, partidas.Count);

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

            // Match 7: França vs Inglaterra
            var match7 = partidas[6];
            Assert.Equal(7, match7.Numero);
            Assert.Equal("FRA", match7.TimeCasa?.Sigla);
            Assert.Equal("ENG", match7.TimeVisitante?.Sigla);
            Assert.Equal(terceiro.Id, match7.FaseId);
            Assert.False(match7.Finalizada);

            // Match 8: Espanha vs Argentina
            var match8 = partidas[7];
            Assert.Equal(8, match8.Numero);
            Assert.Equal("ESP", match8.TimeCasa?.Sigla);
            Assert.Equal("ARG", match8.TimeVisitante?.Sigla);
            Assert.Equal(final.Id, match8.FaseId);
            Assert.False(match8.Finalizada);
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

            // 2. All 8 matches should still be seeded
            var countPartidas = context.Partidas.Count();
            Assert.Equal(8, countPartidas);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
