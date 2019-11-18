using Microsoft.EntityFrameworkCore.Migrations;

namespace SBDlibrary.Migrations
{
    public partial class Email : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<string>(
                name: "nazwisko",
                table: "Uzytkownicy",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "imie",
                table: "Uzytkownicy",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "adres_zamieszkania",
                table: "Uzytkownicy",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezerwacje_Uzytkownicy_id_uzytkownika1",
                table: "Rezerwacje");

            migrationBuilder.DropForeignKey(
                name: "FK_Wypozyczenia_Uzytkownicy_id_uzytkownika1",
                table: "Wypozyczenia");

            migrationBuilder.RenameColumn(
                name: "id_uzytkownika1",
                table: "Wypozyczenia",
                newName: "id_uzytkownikaId");

            migrationBuilder.RenameIndex(
                name: "IX_Wypozyczenia_id_uzytkownika1",
                table: "Wypozyczenia",
                newName: "IX_Wypozyczenia_id_uzytkownikaId");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Uzytkownicy",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "id_uzytkownika",
                table: "Uzytkownicy",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id_uzytkownika1",
                table: "Rezerwacje",
                newName: "id_uzytkownikaId");

            migrationBuilder.RenameIndex(
                name: "IX_Rezerwacje_id_uzytkownika1",
                table: "Rezerwacje",
                newName: "IX_Rezerwacje_id_uzytkownikaId");

            migrationBuilder.AlterColumn<string>(
                name: "nazwisko",
                table: "Uzytkownicy",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "imie",
                table: "Uzytkownicy",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "adres_zamieszkania",
                table: "Uzytkownicy",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AddForeignKey(
                name: "FK_Rezerwacje_Uzytkownicy_id_uzytkownikaId",
                table: "Rezerwacje",
                column: "id_uzytkownikaId",
                principalTable: "Uzytkownicy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wypozyczenia_Uzytkownicy_id_uzytkownikaId",
                table: "Wypozyczenia",
                column: "id_uzytkownikaId",
                principalTable: "Uzytkownicy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
