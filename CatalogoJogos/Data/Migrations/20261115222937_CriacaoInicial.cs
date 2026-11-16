using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogoJogos.Data.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administrador",
                columns: table => new
                {
                    IdAdmin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    SenhaHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrador", x => x.IdAdmin);
                });

            migrationBuilder.CreateTable(
                name: "Jogador",
                columns: table => new
                {
                    IdJogador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Apelido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    SenhaHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FotoPerfil = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SaldoCarteira = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0m),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jogador", x => x.IdJogador);
                    table.CheckConstraint("CK_Jogador_SaldoCarteira", "[SaldoCarteira] >= 0");
                });

            migrationBuilder.CreateTable(
                name: "Jogo",
                columns: table => new
                {
                    IdJogo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    CaminhoFotoCapa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IdAdminCadastro = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jogo", x => x.IdJogo);
                    table.CheckConstraint("CK_Jogo_Preco", "[Preco] >= 0 AND [Preco] <= 9999.99");
                    table.ForeignKey(
                        name: "FK_Jogo_Administrador_IdAdminCadastro",
                        column: x => x.IdAdminCadastro,
                        principalTable: "Administrador",
                        principalColumn: "IdAdmin",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Avaliacao",
                columns: table => new
                {
                    IdAvaliacao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nota = table.Column<int>(type: "int", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DataAvaliacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdJogador = table.Column<int>(type: "int", nullable: false),
                    IdJogo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avaliacao", x => x.IdAvaliacao);
                    table.CheckConstraint("CK_Avaliacao_Nota", "[Nota] BETWEEN 1 AND 5");
                    table.ForeignKey(
                        name: "FK_Avaliacao_Jogador_IdJogador",
                        column: x => x.IdJogador,
                        principalTable: "Jogador",
                        principalColumn: "IdJogador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Avaliacao_Jogo_IdJogo",
                        column: x => x.IdJogo,
                        principalTable: "Jogo",
                        principalColumn: "IdJogo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Compra",
                columns: table => new
                {
                    IdCompra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataAquisicao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValorDebitado = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    IdJogador = table.Column<int>(type: "int", nullable: false),
                    IdJogo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compra", x => x.IdCompra);
                    table.CheckConstraint("CK_Compra_ValorDebitado", "[ValorDebitado] >= 0");
                    table.ForeignKey(
                        name: "FK_Compra_Jogador_IdJogador",
                        column: x => x.IdJogador,
                        principalTable: "Jogador",
                        principalColumn: "IdJogador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compra_Jogo_IdJogo",
                        column: x => x.IdJogo,
                        principalTable: "Jogo",
                        principalColumn: "IdJogo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Administrador_Email",
                table: "Administrador",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacao_IdJogador_IdJogo",
                table: "Avaliacao",
                columns: new[] { "IdJogador", "IdJogo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacao_IdJogo",
                table: "Avaliacao",
                column: "IdJogo");

            migrationBuilder.CreateIndex(
                name: "IX_Compra_IdJogador_IdJogo",
                table: "Compra",
                columns: new[] { "IdJogador", "IdJogo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Compra_IdJogo",
                table: "Compra",
                column: "IdJogo");

            migrationBuilder.CreateIndex(
                name: "IX_Jogador_Apelido",
                table: "Jogador",
                column: "Apelido",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jogador_Email",
                table: "Jogador",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jogo_Categoria",
                table: "Jogo",
                column: "Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_Jogo_IdAdminCadastro",
                table: "Jogo",
                column: "IdAdminCadastro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avaliacao");

            migrationBuilder.DropTable(
                name: "Compra");

            migrationBuilder.DropTable(
                name: "Jogador");

            migrationBuilder.DropTable(
                name: "Jogo");

            migrationBuilder.DropTable(
                name: "Administrador");
        }
    }
}
