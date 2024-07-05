using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class CriandoTabelaPatrimonio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Patrimonios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    FK_Categoria = table.Column<int>(type: "int", nullable: false),
                    FK_Marca = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patrimonios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patrimonios_Categorias_FK_Categoria",
                        column: x => x.FK_Categoria,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patrimonios_Marcas_FK_Marca",
                        column: x => x.FK_Marca,
                        principalTable: "Marcas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patrimonios_FK_Categoria",
                table: "Patrimonios",
                column: "FK_Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_Patrimonios_FK_Marca",
                table: "Patrimonios",
                column: "FK_Marca");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Patrimonios");
        }
    }
}
