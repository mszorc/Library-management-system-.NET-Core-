using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SBDlibrary.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Autor",
                columns: table => new
                {
                    id_autor = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    imie = table.Column<string>(maxLength: 20, nullable: false),
                    nazwisko = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autor", x => x.id_autor);
                });

            migrationBuilder.CreateTable(
                name: "Dostawcy",
                columns: table => new
                {
                    id_dostawcy = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nazwa = table.Column<string>(maxLength: 20, nullable: false),
                    adres = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dostawcy", x => x.id_dostawcy);
                });

            migrationBuilder.CreateTable(
                name: "Kategorie",
                columns: table => new
                {
                    id_kategorii = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nazwa = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorie", x => x.id_kategorii);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id_roli = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nazwa = table.Column<string>(maxLength: 12, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.id_roli);
                });

            migrationBuilder.CreateTable(
                name: "Uzytkownicy",
                columns: table => new
                {
                    id_uzytkownika = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    haslo = table.Column<string>(maxLength: 20, nullable: false),
                    imie = table.Column<string>(maxLength: 20, nullable: false),
                    nazwisko = table.Column<string>(maxLength: 20, nullable: false),
                    adres_zamieszkania = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uzytkownicy", x => x.id_uzytkownika);
                });

            migrationBuilder.CreateTable(
                name: "Wydawnictwa",
                columns: table => new
                {
                    id_wydawnictwa = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nazwa = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wydawnictwa", x => x.id_wydawnictwa);
                });

            migrationBuilder.CreateTable(
                name: "Zamowienia",
                columns: table => new
                {
                    id_zamowienia = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    data_zamowienia = table.Column<DateTime>(nullable: false),
                    status_zamowienia = table.Column<int>(nullable: false),
                    id_dostawcy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zamowienia", x => x.id_zamowienia);
                    table.ForeignKey(
                        name: "FK_Zamowienia_Dostawcy",
                        column: x => x.id_dostawcy,
                        principalTable: "Dostawcy",
                        principalColumn: "id_dostawcy",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Logi",
                columns: table => new
                {
                    id_logu = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ip_urzadzenia = table.Column<string>(maxLength: 15, nullable: false),
                    komunikat = table.Column<string>(maxLength: 50, nullable: false),
                    id_uzytkownika = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logi", x => x.id_logu);
                    table.ForeignKey(
                        name: "FK_Logi_Uzytkownicy",
                        column: x => x.id_uzytkownika,
                        principalTable: "Uzytkownicy",
                        principalColumn: "id_uzytkownika",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Uzytkownicy_role",
                columns: table => new
                {
                    id_roli = table.Column<int>(nullable: false),
                    id_uzytkownika = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uzytkownicy_role", x => new { x.id_uzytkownika, x.id_roli });
                    table.UniqueConstraint("AK_Uzytkownicy_role_id_roli_id_uzytkownika", x => new { x.id_roli, x.id_uzytkownika });
                    table.ForeignKey(
                        name: "FK_Uzytkownicy_Role",
                        column: x => x.id_roli,
                        principalTable: "Role",
                        principalColumn: "id_roli",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Uzytkownicy_Role_2",
                        column: x => x.id_uzytkownika,
                        principalTable: "Uzytkownicy",
                        principalColumn: "id_uzytkownika",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ksiazki",
                columns: table => new
                {
                    id_ksiazki = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    id_wydawnictwa = table.Column<int>(nullable: false),
                    tytuł = table.Column<string>(maxLength: 50, nullable: false),
                    data_wydania = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ksiazki", x => x.id_ksiazki);
                    table.ForeignKey(
                        name: "FK_Ksiazki_Wydawnictwa_Wydawnictwaid_wydawnictwa",
                        column: x => x.id_wydawnictwa,
                        principalTable: "Wydawnictwa",
                        principalColumn: "id_wydawnictwa",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Autorzy_Ksiazki",
                columns: table => new
                {
                    id_autora = table.Column<int>(nullable: false),
                    id_ksiazki = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autorzy_Ksiazki", x => new { x.id_autora, x.id_ksiazki });
                    table.ForeignKey(
                        name: "FK_Autorzy_Ksiazki",
                        column: x => x.id_autora,
                        principalTable: "Autor",
                        principalColumn: "id_autor",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Autorzy_Ksiazki_2",
                        column: x => x.id_ksiazki,
                        principalTable: "Ksiazki",
                        principalColumn: "id_ksiazki",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Egzemplarze",
                columns: table => new
                {
                    id_egzemplarza = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    id_ksiazki = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Egzemplarze", x => x.id_egzemplarza);
                    table.ForeignKey(
                        name: "FK_Egzemplarze_Ksiazki",
                        column: x => x.id_ksiazki,
                        principalTable: "Ksiazki",
                        principalColumn: "id_ksiazki",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kategorie_Ksiazki",
                columns: table => new
                {
                    id_kategorii = table.Column<int>(nullable: false),
                    id_ksiazki = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorie_Ksiazki", x => new { x.id_kategorii, x.id_ksiazki });
                    table.ForeignKey(
                        name: "FK_Kategorie_Ksiazki",
                        column: x => x.id_kategorii,
                        principalTable: "Ksiazki",
                        principalColumn: "id_ksiazki",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Kategorie_Ksiazki_2",
                        column: x => x.id_ksiazki,
                        principalTable: "Kategorie",
                        principalColumn: "id_kategorii",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Zamowienie_ksiazki",
                columns: table => new
                {
                    id_zamowienia = table.Column<int>(nullable: false),
                    id_ksiazki = table.Column<int>(nullable: false),
                    ilosc = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zamowienie_ksiazki", x => new { x.id_zamowienia, x.id_ksiazki });
                    table.UniqueConstraint("AK_Zamowienie_ksiazki_id_ksiazki_id_zamowienia", x => new { x.id_ksiazki, x.id_zamowienia });
                    table.ForeignKey(
                        name: "FK_Zamowienie_ksiazki",
                        column: x => x.id_ksiazki,
                        principalTable: "Ksiazki",
                        principalColumn: "id_ksiazki",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Zamowienie_ksiazki_2",
                        column: x => x.id_zamowienia,
                        principalTable: "Zamowienia",
                        principalColumn: "id_zamowienia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rezerwacje",
                columns: table => new
                {
                    id_rezerwacji = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    data_rezerwacji = table.Column<DateTime>(nullable: false),
                    status_rezerwacji = table.Column<int>(nullable: false),
                    id_uzytkownika = table.Column<int>(nullable: false),
                    id_egzemplarza = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezerwacje", x => x.id_rezerwacji);
                    table.ForeignKey(
                        name: "FK_Rezerwacje_Egzemplarze",
                        column: x => x.id_egzemplarza,
                        principalTable: "Egzemplarze",
                        principalColumn: "id_egzemplarza",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rezerwacje_Uzytkownicy_2",
                        column: x => x.id_uzytkownika,
                        principalTable: "Uzytkownicy",
                        principalColumn: "id_uzytkownika",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wypozyczenia",
                columns: table => new
                {
                    id_wypozyczenia = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    data_wypozyczenia = table.Column<DateTime>(nullable: false),
                    data_zwrotu = table.Column<DateTime>(nullable: false),
                    id_uzytkownika = table.Column<int>(nullable: false),
                    id_egzemplarza = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wypozyczenia", x => x.id_wypozyczenia);
                    table.ForeignKey(
                        name: "FK_Wypozyczenia_Egzemplarze",
                        column: x => x.id_egzemplarza,
                        principalTable: "Egzemplarze",
                        principalColumn: "id_egzemplarza",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wypozyczenia_Uzytkownicy_2",
                        column: x => x.id_uzytkownika,
                        principalTable: "Uzytkownicy",
                        principalColumn: "id_uzytkownika",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Zwroty",
                columns: table => new
                {
                    id_zwrotu = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    data_zwrotu = table.Column<DateTime>(nullable: false),
                    kara = table.Column<float>(nullable: false),
                    id_wypozyczenia = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zwroty", x => x.id_zwrotu);
                    table.ForeignKey(
                        name: "FK_Zwroty_Wypozyczenia",
                        column: x => x.id_wypozyczenia,
                        principalTable: "Wypozyczenia",
                        principalColumn: "id_wypozyczenia",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Autorzy_Ksiazki_id_ksiazki",
                table: "Autorzy_Ksiazki",
                column: "id_ksiazki");

            migrationBuilder.CreateIndex(
                name: "IX_Egzemplarze_id_ksiazki",
                table: "Egzemplarze",
                column: "id_ksiazki");

            migrationBuilder.CreateIndex(
                name: "IX_Kategorie_Ksiazki_id_ksiazki",
                table: "Kategorie_Ksiazki",
                column: "id_ksiazki");

            migrationBuilder.CreateIndex(
                name: "IX_Ksiazki_Wydawnictwaid_wydawnictwa",
                table: "Ksiazki",
                column: "id_wydawnictwa");

            migrationBuilder.CreateIndex(
                name: "IX_Logi_id_uzytkownika",
                table: "Logi",
                column: "id_uzytkownika");

            migrationBuilder.CreateIndex(
                name: "IX_Rezerwacje_id_egzemplarza1",
                table: "Rezerwacje",
                column: "id_egzemplarza");

            migrationBuilder.CreateIndex(
                name: "IX_Rezerwacje_id_uzytkownika1",
                table: "Rezerwacje",
                column: "id_uzytkownika");

            migrationBuilder.CreateIndex(
                name: "IX_Wypozyczenia_id_egzemplarza1",
                table: "Wypozyczenia",
                column: "id_egzemplarza");

            migrationBuilder.CreateIndex(
                name: "IX_Wypozyczenia_id_uzytkownika1",
                table: "Wypozyczenia",
                column: "id_uzytkownika");

            migrationBuilder.CreateIndex(
                name: "IX_Zamowienia_dostawcyid_dostawcy",
                table: "Zamowienia",
                column: "id_dostawcy");

            migrationBuilder.CreateIndex(
                name: "IX_Zwroty_id_wypozyczenia1",
                table: "Zwroty",
                column: "id_wypozyczenia");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Autorzy_Ksiazki");

            migrationBuilder.DropTable(
                name: "Kategorie_Ksiazki");

            migrationBuilder.DropTable(
                name: "Logi");

            migrationBuilder.DropTable(
                name: "Rezerwacje");

            migrationBuilder.DropTable(
                name: "Uzytkownicy_role");

            migrationBuilder.DropTable(
                name: "Zamowienie_ksiazki");

            migrationBuilder.DropTable(
                name: "Zwroty");

            migrationBuilder.DropTable(
                name: "Autor");

            migrationBuilder.DropTable(
                name: "Kategorie");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Zamowienia");

            migrationBuilder.DropTable(
                name: "Wypozyczenia");

            migrationBuilder.DropTable(
                name: "Dostawcy");

            migrationBuilder.DropTable(
                name: "Egzemplarze");

            migrationBuilder.DropTable(
                name: "Uzytkownicy");

            migrationBuilder.DropTable(
                name: "Ksiazki");

            migrationBuilder.DropTable(
                name: "Wydawnictwa");
        }
    }
}
