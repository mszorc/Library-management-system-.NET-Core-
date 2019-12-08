using Microsoft.EntityFrameworkCore.Migrations;

namespace SBDlibrary.Migrations
{
    public partial class naprawa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kategorie_Ksiazki_Ksiazki_id_kategorii",
                table: "Kategorie_Ksiazki");

            migrationBuilder.DropForeignKey(
                name: "FK_Kategorie_Ksiazki_Kategorie_id_ksiazki",
                table: "Kategorie_Ksiazki");

            migrationBuilder.AddForeignKey(
                name: "FK_Kategorie_Ksiazki_Kategorie_id_kategorii",
                table: "Kategorie_Ksiazki",
                column: "id_kategorii",
                principalTable: "Kategorie",
                principalColumn: "id_kategorii",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Kategorie_Ksiazki_Ksiazki_id_ksiazki",
                table: "Kategorie_Ksiazki",
                column: "id_ksiazki",
                principalTable: "Ksiazki",
                principalColumn: "id_ksiazki",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kategorie_Ksiazki_Kategorie_id_kategorii",
                table: "Kategorie_Ksiazki");

            migrationBuilder.DropForeignKey(
                name: "FK_Kategorie_Ksiazki_Ksiazki_id_ksiazki",
                table: "Kategorie_Ksiazki");

            migrationBuilder.AddForeignKey(
                name: "FK_Kategorie_Ksiazki_Ksiazki_id_kategorii",
                table: "Kategorie_Ksiazki",
                column: "id_kategorii",
                principalTable: "Ksiazki",
                principalColumn: "id_ksiazki",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Kategorie_Ksiazki_Kategorie_id_ksiazki",
                table: "Kategorie_Ksiazki",
                column: "id_ksiazki",
                principalTable: "Kategorie",
                principalColumn: "id_kategorii",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
