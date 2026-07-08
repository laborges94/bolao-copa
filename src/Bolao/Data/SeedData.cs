using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Bolao.Models;
using Bolao.Helpers;

namespace Bolao.Data
{
    public static class SeedData
    {
        public static void Initialize(BolaoDbContext context)
        {
            // Apply migrations for SQLite local development, or ensure schema is created for PostgreSQL
            if (context.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                context.Database.Migrate();
            }
            else
            {
                context.Database.EnsureCreated();
            }

            // Check if Fase "Quartas de final" is already seeded
            if (context.Fases.Any(f => f.Nome == "Quartas de final"))
            {
                // Ensure admin exists with the stronger password even if already seeded
                EnsureAdminUser(context);
                return; // Already seeded
            }

            // 1. Clear database tables to ensure a clean environment
            context.Palpites.RemoveRange(context.Palpites);
            context.Partidas.RemoveRange(context.Partidas);
            context.Fases.RemoveRange(context.Fases);
            context.Selecoes.RemoveRange(context.Selecoes);
            context.Participantes.RemoveRange(context.Participantes);
            context.Boloes.RemoveRange(context.Boloes);
            context.Usuarios.RemoveRange(context.Usuarios);
            context.SaveChanges();

            // 2. Create Default Admin User with stronger password
            EnsureAdminUser(context);

            // 3. Create Fase "Quartas de final"
            var fase = new Fase
            {
                Nome = "Quartas de final",
                Ordem = 2
            };
            context.Fases.Add(fase);
            context.SaveChanges(); // Persist to get ID

            // 4. Create 8 Selecoes (with Switzerland as the missing one)
            var selecoes = new List<Selecao>
            {
                new() { Nome = "França", Sigla = "FRA", Bandeira = "🇫🇷" },
                new() { Nome = "Marrocos", Sigla = "MAR", Bandeira = "🇲🇦" },
                new() { Nome = "Espanha", Sigla = "ESP", Bandeira = "🇪🇸" },
                new() { Nome = "Bélgica", Sigla = "BEL", Bandeira = "🇧🇪" },
                new() { Nome = "Noruega", Sigla = "NOR", Bandeira = "🇳🇴" },
                new() { Nome = "Inglaterra", Sigla = "ENG", Bandeira = "🏴" },
                new() { Nome = "Argentina", Sigla = "ARG", Bandeira = "🇦🇷" },
                new() { Nome = "Suíça", Sigla = "SUI", Bandeira = "🇨🇭" }
            };
            context.Selecoes.AddRange(selecoes);
            context.SaveChanges(); // Get IDs

            // Helper to get Selecao by Sigla
            var getSelecao = new Func<string, Selecao>(sigla => selecoes.First(s => s.Sigla == sigla));

            // 5. Create 4 Quarterfinal Matches
            // França x Marrocos (09/07/2026 17:00 local -> 20:00 UTC)
            // Espanha x Bélgica (10/07/2026 16:00 local -> 19:00 UTC)
            // Noruega x Inglaterra (11/07/2026 18:00 local -> 21:00 UTC)
            // Argentina x Suíça (11/07/2026 22:00 local -> 12/07/2026 01:00 UTC)
            var partidas = new List<Partida>
            {
                new()
                {
                    Numero = 1,
                    DataHora = new DateTime(2026, 7, 9, 20, 0, 0, DateTimeKind.Utc),
                    TimeCasaId = getSelecao("FRA").Id,
                    TimeVisitanteId = getSelecao("MAR").Id,
                    FaseId = fase.Id,
                    Finalizada = false
                },
                new()
                {
                    Numero = 2,
                    DataHora = new DateTime(2026, 7, 10, 19, 0, 0, DateTimeKind.Utc),
                    TimeCasaId = getSelecao("ESP").Id,
                    TimeVisitanteId = getSelecao("BEL").Id,
                    FaseId = fase.Id,
                    Finalizada = false
                },
                new()
                {
                    Numero = 3,
                    DataHora = new DateTime(2026, 7, 11, 21, 0, 0, DateTimeKind.Utc),
                    TimeCasaId = getSelecao("NOR").Id,
                    TimeVisitanteId = getSelecao("ENG").Id,
                    FaseId = fase.Id,
                    Finalizada = false
                },
                new()
                {
                    Numero = 4,
                    DataHora = new DateTime(2026, 7, 12, 1, 0, 0, DateTimeKind.Utc),
                    TimeCasaId = getSelecao("ARG").Id,
                    TimeVisitanteId = getSelecao("SUI").Id,
                    FaseId = fase.Id,
                    Finalizada = false
                }
            };
            context.Partidas.AddRange(partidas);
            context.SaveChanges();
        }

        private static void EnsureAdminUser(BolaoDbContext context)
        {
            var admin = context.Usuarios.FirstOrDefault(u => u.Email == "admin@bolao.com");
            if (admin == null)
            {
                admin = new Usuario
                {
                    Nome = "Administrador",
                    Email = "admin@bolao.com",
                    Senha = PasswordHasher.Hash("AdminBolao2026!"),
                    IsAdmin = true
                };
                context.Usuarios.Add(admin);
            }
            else
            {
                admin.Senha = PasswordHasher.Hash("AdminBolao2026!");
                context.Entry(admin).State = EntityState.Modified;
            }
            context.SaveChanges();
        }
    }
}
