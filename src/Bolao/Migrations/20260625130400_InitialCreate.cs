using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bolao.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boloes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    CodigoConvite = table.Column<string>(type: "TEXT", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CriadoPorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Encerrado = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boloes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Ordem = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Selecoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Sigla = table.Column<string>(type: "TEXT", nullable: false),
                    Bandeira = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Selecoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Senha = table.Column<string>(type: "TEXT", nullable: false),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Partidas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Numero = table.Column<int>(type: "INTEGER", nullable: false),
                    DataHora = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TimeCasaId = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeVisitanteId = table.Column<int>(type: "INTEGER", nullable: false),
                    GolsCasa = table.Column<int>(type: "INTEGER", nullable: true),
                    GolsVisitante = table.Column<int>(type: "INTEGER", nullable: true),
                    Finalizada = table.Column<bool>(type: "INTEGER", nullable: false),
                    FaseId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partidas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Partidas_Fases_FaseId",
                        column: x => x.FaseId,
                        principalTable: "Fases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Partidas_Selecoes_TimeCasaId",
                        column: x => x.TimeCasaId,
                        principalTable: "Selecoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partidas_Selecoes_TimeVisitanteId",
                        column: x => x.TimeVisitanteId,
                        principalTable: "Selecoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Participantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BolaoId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    NomeExibicao = table.Column<string>(type: "TEXT", nullable: false),
                    EntrouEm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participantes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participantes_Boloes_BolaoId",
                        column: x => x.BolaoId,
                        principalTable: "Boloes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participantes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Palpites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PartidaId = table.Column<int>(type: "INTEGER", nullable: false),
                    ParticipanteId = table.Column<int>(type: "INTEGER", nullable: false),
                    GolsCasa = table.Column<int>(type: "INTEGER", nullable: false),
                    GolsVisitante = table.Column<int>(type: "INTEGER", nullable: false),
                    Pontos = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Palpites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Palpites_Participantes_ParticipanteId",
                        column: x => x.ParticipanteId,
                        principalTable: "Participantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Palpites_Partidas_PartidaId",
                        column: x => x.PartidaId,
                        principalTable: "Partidas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boloes_CodigoConvite",
                table: "Boloes",
                column: "CodigoConvite",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Palpites_ParticipanteId",
                table: "Palpites",
                column: "ParticipanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Palpites_PartidaId_ParticipanteId",
                table: "Palpites",
                columns: new[] { "PartidaId", "ParticipanteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participantes_BolaoId_UsuarioId",
                table: "Participantes",
                columns: new[] { "BolaoId", "UsuarioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participantes_UsuarioId",
                table: "Participantes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Partidas_FaseId",
                table: "Partidas",
                column: "FaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Partidas_TimeCasaId",
                table: "Partidas",
                column: "TimeCasaId");

            migrationBuilder.CreateIndex(
                name: "IX_Partidas_TimeVisitanteId",
                table: "Partidas",
                column: "TimeVisitanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Palpites");

            migrationBuilder.DropTable(
                name: "Participantes");

            migrationBuilder.DropTable(
                name: "Partidas");

            migrationBuilder.DropTable(
                name: "Boloes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Fases");

            migrationBuilder.DropTable(
                name: "Selecoes");
        }
    }
}
