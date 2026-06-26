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
            // Apply any pending migrations automatically (or ensure database is created)
            context.Database.Migrate();

            // Check if Fase "16 avos de final" is already seeded
            if (context.Fases.Any(f => f.Nome == "16 avos de final"))
            {
                return; // Already seeded
            }

            // 1. Create Fase
            var fase = new Fase
            {
                Nome = "16 avos de final",
                Ordem = 1
            };
            context.Fases.Add(fase);
            context.SaveChanges(); // Persist to get ID

            // 2. Create 32 Selecoes
            var selecoes = new List<Selecao>
            {
                new() { Nome = "Brasil", Sigla = "BRA", Bandeira = "🇧🇷" },
                new() { Nome = "Argentina", Sigla = "ARG", Bandeira = "🇦🇷" },
                new() { Nome = "França", Sigla = "FRA", Bandeira = "🇫🇷" },
                new() { Nome = "Alemanha", Sigla = "GER", Bandeira = "🇩🇪" },
                new() { Nome = "Espanha", Sigla = "ESP", Bandeira = "🇪🇸" },
                new() { Nome = "Inglaterra", Sigla = "ENG", Bandeira = "🏴" },
                new() { Nome = "Itália", Sigla = "ITA", Bandeira = "🇮🇹" },
                new() { Nome = "Uruguai", Sigla = "URU", Bandeira = "🇺🇾" },
                new() { Nome = "Holanda", Sigla = "NED", Bandeira = "🇳🇱" },
                new() { Nome = "Bélgica", Sigla = "BEL", Bandeira = "🇧🇪" },
                new() { Nome = "Portugal", Sigla = "POR", Bandeira = "🇵🇹" },
                new() { Nome = "Croácia", Sigla = "CRO", Bandeira = "🇭🇷" },
                new() { Nome = "Senegal", Sigla = "SEN", Bandeira = "🇸🇳" },
                new() { Nome = "Japão", Sigla = "JPN", Bandeira = "🇯🇵" },
                new() { Nome = "Marrocos", Sigla = "MAR", Bandeira = "🇲🇦" },
                new() { Nome = "Estados Unidos", Sigla = "USA", Bandeira = "🇺🇸" },
                new() { Nome = "México", Sigla = "MEX", Bandeira = "🇲🇽" },
                new() { Nome = "Canadá", Sigla = "CAN", Bandeira = "🇨🇦" },
                new() { Nome = "Suíça", Sigla = "SUI", Bandeira = "🇨🇭" },
                new() { Nome = "Dinamarca", Sigla = "DEN", Bandeira = "🇩🇰" },
                new() { Nome = "Polônia", Sigla = "POL", Bandeira = "🇵🇱" },
                new() { Nome = "Suécia", Sigla = "SWE", Bandeira = "🇸🇪" },
                new() { Nome = "Colômbia", Sigla = "COL", Bandeira = "🇨🇴" },
                new() { Nome = "Chile", Sigla = "CHI", Bandeira = "🇨🇱" },
                new() { Nome = "Equador", Sigla = "ECU", Bandeira = "🇪🇨" },
                new() { Nome = "Coreia do Sul", Sigla = "KOR", Bandeira = "🇰🇷" },
                new() { Nome = "Camarões", Sigla = "CMR", Bandeira = "🇨🇲" },
                new() { Nome = "Gana", Sigla = "GHA", Bandeira = "🇬🇭" },
                new() { Nome = "Tunísia", Sigla = "TUN", Bandeira = "🇹🇳" },
                new() { Nome = "Arábia Saudita", Sigla = "KSA", Bandeira = "🇸🇦" },
                new() { Nome = "Austrália", Sigla = "AUS", Bandeira = "🇦🇺" },
                new() { Nome = "Irã", Sigla = "IRN", Bandeira = "🇮🇷" }
            };
            context.Selecoes.AddRange(selecoes);
            context.SaveChanges(); // Get IDs

            // 3. Create 16 Matches (Partidas)
            // Schedule matches in the future (e.g., starting tomorrow 10 AM, spaced 4 hours)
            var baseDate = DateTime.Today.AddDays(1).AddHours(10);
            var partidas = new List<Partida>();

            for (int i = 0; i < 16; i++)
            {
                var timeCasa = selecoes[i * 2];
                var timeVisitante = selecoes[i * 2 + 1];

                partidas.Add(new Partida
                {
                    Numero = i + 1,
                    DataHora = baseDate.AddDays(i / 2).AddHours((i % 2) * 4), // 2 matches per day (10:00, 14:00)
                    TimeCasaId = timeCasa.Id,
                    TimeVisitanteId = timeVisitante.Id,
                    FaseId = fase.Id,
                    Finalizada = false
                });
            }
            context.Partidas.AddRange(partidas);

            // 4. Create Default Admin User
            if (!context.Usuarios.Any(u => u.Email == "admin@bolao.com"))
            {
                var admin = new Usuario
                {
                    Nome = "Administrador",
                    Email = "admin@bolao.com",
                    Senha = PasswordHasher.Hash("admin123"),
                    IsAdmin = true
                };
                context.Usuarios.Add(admin);
            }

            context.SaveChanges();
        }
    }
}
