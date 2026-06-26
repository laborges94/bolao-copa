using System;
using System.Collections.Generic;

namespace Bolao.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public bool IsAdmin { get; set; } = false;

        // Relacionamentos
        public ICollection<Participante> Participantes { get; set; } = new List<Participante>();
    }

    public class Bolao
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CodigoConvite { get; set; } = string.Empty;
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
        public int CriadoPorId { get; set; }
        public bool Encerrado { get; set; } = false;

        // Relacionamentos
        public ICollection<Participante> Participantes { get; set; } = new List<Participante>();
    }

    public class Participante
    {
        public int Id { get; set; }
        public int BolaoId { get; set; }
        public int UsuarioId { get; set; }
        public string NomeExibicao { get; set; } = string.Empty;
        public DateTime EntrouEm { get; set; } = DateTime.UtcNow;

        // Relacionamentos
        public Bolao? Bolao { get; set; }
        public Usuario? Usuario { get; set; }
    }

    public class Selecao
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
        public string Bandeira { get; set; } = string.Empty; // Emoji ou string
    }

    public class Fase
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Ordem { get; set; }

        // Relacionamentos
        public ICollection<Partida> Partidas { get; set; } = new List<Partida>();
    }

    public class Partida
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public DateTime DataHora { get; set; }
        public int TimeCasaId { get; set; }
        public int TimeVisitanteId { get; set; }
        public int? GolsCasa { get; set; }
        public int? GolsVisitante { get; set; }
        public bool Finalizada { get; set; } = false;
        public int FaseId { get; set; }

        // Relacionamentos
        public Fase? Fase { get; set; }
        public Selecao? TimeCasa { get; set; }
        public Selecao? TimeVisitante { get; set; }
    }

    public class Palpite
    {
        public int Id { get; set; }
        public int PartidaId { get; set; }
        public int ParticipanteId { get; set; }
        public int GolsCasa { get; set; }
        public int GolsVisitante { get; set; }
        public int? Pontos { get; set; }

        // Relacionamentos
        public Partida? Partida { get; set; }
        public Participante? Participante { get; set; }
    }
}
