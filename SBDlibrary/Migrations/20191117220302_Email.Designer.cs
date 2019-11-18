﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SBDlibrary.Models;

namespace SBDlibrary.Migrations
{
    [DbContext(typeof(LibraryDbContext))]
    [Migration("20191117220302_Email")]
    partial class Email
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SBDlibrary.Models.Autor", b =>
                {
                    b.Property<int>("id_autor")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("imie")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("nazwisko")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("id_autor");

                    b.ToTable("Autor");
                });

            modelBuilder.Entity("SBDlibrary.Models.Autorzy_Ksiazki", b =>
                {
                    b.Property<int>("id_autora");

                    b.Property<int>("id_ksiazki");

                    b.HasKey("id_autora", "id_ksiazki");

                    b.HasIndex("id_ksiazki");

                    b.ToTable("Autorzy_Ksiazki");
                });

            modelBuilder.Entity("SBDlibrary.Models.Dostawcy", b =>
                {
                    b.Property<int>("id_dostawcy")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("adres")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("nazwa")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("id_dostawcy");

                    b.ToTable("Dostawcy");
                });

            modelBuilder.Entity("SBDlibrary.Models.Egzemplarze", b =>
                {
                    b.Property<int>("id_egzemplarza")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("id_ksiazki");

                    b.HasKey("id_egzemplarza");

                    b.HasIndex("id_ksiazki");

                    b.ToTable("Egzemplarze");
                });

            modelBuilder.Entity("SBDlibrary.Models.Kategorie", b =>
                {
                    b.Property<int>("id_kategorii")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("nazwa")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("id_kategorii");

                    b.ToTable("Kategorie");
                });

            modelBuilder.Entity("SBDlibrary.Models.Kategorie_Ksiazki", b =>
                {
                    b.Property<int>("id_kategorii");

                    b.Property<int>("id_ksiazki");

                    b.HasKey("id_kategorii", "id_ksiazki");

                    b.HasIndex("id_ksiazki");

                    b.ToTable("Kategorie_Ksiazki");
                });

            modelBuilder.Entity("SBDlibrary.Models.Ksiazki", b =>
                {
                    b.Property<int>("id_ksiazki")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Wydawnictwaid_wydawnictwa");

                    b.Property<DateTime>("data_wydania");

                    b.Property<string>("tytuł")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("id_ksiazki");

                    b.HasIndex("Wydawnictwaid_wydawnictwa");

                    b.ToTable("Ksiazki");
                });

            modelBuilder.Entity("SBDlibrary.Models.Logi", b =>
                {
                    b.Property<int>("id_logu")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("id_uzytkownika");

                    b.Property<string>("ip_urzadzenia")
                        .IsRequired()
                        .HasMaxLength(15);

                    b.Property<string>("komunikat")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("id_logu");

                    b.HasIndex("id_uzytkownika");

                    b.ToTable("Logi");
                });

            modelBuilder.Entity("SBDlibrary.Models.Rezerwacje", b =>
                {
                    b.Property<int>("id_rezerwacji")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("data_rezerwacji");

                    b.Property<int>("id_egzemplarza1");

                    b.Property<int>("id_uzytkownika1");

                    b.Property<int>("status_rezerwacji");

                    b.HasKey("id_rezerwacji");

                    b.HasIndex("id_egzemplarza1");

                    b.HasIndex("id_uzytkownika1");

                    b.ToTable("Rezerwacje");
                });

            modelBuilder.Entity("SBDlibrary.Models.Role", b =>
                {
                    b.Property<int>("id_roli")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("nazwa")
                        .IsRequired()
                        .HasMaxLength(12);

                    b.HasKey("id_roli");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("SBDlibrary.Models.Uzytkownicy", b =>
                {
                    b.Property<int>("id_uzytkownika")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("adres_zamieszkania")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("email")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("haslo")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("imie")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("nazwisko")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("id_uzytkownika");

                    b.ToTable("Uzytkownicy");
                });

            modelBuilder.Entity("SBDlibrary.Models.Uzytkownicy_role", b =>
                {
                    b.Property<int>("id_uzytkownika");

                    b.Property<int>("id_roli");

                    b.HasKey("id_uzytkownika", "id_roli");

                    b.HasAlternateKey("id_roli", "id_uzytkownika");

                    b.ToTable("Uzytkownicy_role");
                });

            modelBuilder.Entity("SBDlibrary.Models.Wydawnictwa", b =>
                {
                    b.Property<int>("id_wydawnictwa")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("nazwa")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("id_wydawnictwa");

                    b.ToTable("Wydawnictwa");
                });

            modelBuilder.Entity("SBDlibrary.Models.Wypozyczenia", b =>
                {
                    b.Property<int>("id_wypozyczenia")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("data_wypozyczenia");

                    b.Property<DateTime>("data_zwrotu");

                    b.Property<int>("id_egzemplarza1");

                    b.Property<int>("id_uzytkownika1");

                    b.HasKey("id_wypozyczenia");

                    b.HasIndex("id_egzemplarza1");

                    b.HasIndex("id_uzytkownika1");

                    b.ToTable("Wypozyczenia");
                });

            modelBuilder.Entity("SBDlibrary.Models.Zamowienia", b =>
                {
                    b.Property<int>("id_zamowienia")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("data_zamowienia");

                    b.Property<int?>("dostawcyid_dostawcy");

                    b.Property<int>("status_zamowienia");

                    b.HasKey("id_zamowienia");

                    b.HasIndex("dostawcyid_dostawcy");

                    b.ToTable("Zamowienia");
                });

            modelBuilder.Entity("SBDlibrary.Models.Zamowienie_ksiazki", b =>
                {
                    b.Property<int>("id_zamowienia");

                    b.Property<int>("id_ksiazki");

                    b.Property<int>("ilosc");

                    b.HasKey("id_zamowienia", "id_ksiazki");

                    b.HasAlternateKey("id_ksiazki", "id_zamowienia");

                    b.ToTable("Zamowienie_ksiazki");
                });

            modelBuilder.Entity("SBDlibrary.Models.Zwroty", b =>
                {
                    b.Property<int>("id_zwrotu")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("data_zwrotu");

                    b.Property<int?>("id_wypozyczenia1");

                    b.Property<float>("kara");

                    b.HasKey("id_zwrotu");

                    b.HasIndex("id_wypozyczenia1");

                    b.ToTable("Zwroty");
                });

            modelBuilder.Entity("SBDlibrary.Models.Autorzy_Ksiazki", b =>
                {
                    b.HasOne("SBDlibrary.Models.Autor", "Autor")
                        .WithMany()
                        .HasForeignKey("id_autora")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SBDlibrary.Models.Ksiazki", "Ksiazki")
                        .WithMany()
                        .HasForeignKey("id_ksiazki")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SBDlibrary.Models.Egzemplarze", b =>
                {
                    b.HasOne("SBDlibrary.Models.Ksiazki", "Ksiazki")
                        .WithMany()
                        .HasForeignKey("id_ksiazki")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SBDlibrary.Models.Kategorie_Ksiazki", b =>
                {
                    b.HasOne("SBDlibrary.Models.Ksiazki", "Ksiazki")
                        .WithMany()
                        .HasForeignKey("id_kategorii")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SBDlibrary.Models.Kategorie", "Kategorie")
                        .WithMany()
                        .HasForeignKey("id_ksiazki")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SBDlibrary.Models.Ksiazki", b =>
                {
                    b.HasOne("SBDlibrary.Models.Wydawnictwa", "Wydawnictwa")
                        .WithMany()
                        .HasForeignKey("Wydawnictwaid_wydawnictwa")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SBDlibrary.Models.Logi", b =>
                {
                    b.HasOne("SBDlibrary.Models.Uzytkownicy", "Uzytkownicy")
                        .WithMany("Logi")
                        .HasForeignKey("id_uzytkownika");
                });

            modelBuilder.Entity("SBDlibrary.Models.Rezerwacje", b =>
                {
                    b.HasOne("SBDlibrary.Models.Egzemplarze", "id_egzemplarza")
                        .WithMany("Rezerwacje")
                        .HasForeignKey("id_egzemplarza1")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SBDlibrary.Models.Uzytkownicy", "id_uzytkownika")
                        .WithMany()
                        .HasForeignKey("id_uzytkownika1")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SBDlibrary.Models.Uzytkownicy_role", b =>
                {
                    b.HasOne("SBDlibrary.Models.Role", "Role")
                        .WithMany("Uzytkownicy_role")
                        .HasForeignKey("id_roli")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SBDlibrary.Models.Uzytkownicy", "Uzytkownicy")
                        .WithMany("Uzytkownicy_role")
                        .HasForeignKey("id_uzytkownika")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SBDlibrary.Models.Wypozyczenia", b =>
                {
                    b.HasOne("SBDlibrary.Models.Egzemplarze", "id_egzemplarza")
                        .WithMany("Wypozyczenia")
                        .HasForeignKey("id_egzemplarza1")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SBDlibrary.Models.Uzytkownicy", "id_uzytkownika")
                        .WithMany()
                        .HasForeignKey("id_uzytkownika1")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SBDlibrary.Models.Zamowienia", b =>
                {
                    b.HasOne("SBDlibrary.Models.Dostawcy", "dostawcy")
                        .WithMany("Zamowienia")
                        .HasForeignKey("dostawcyid_dostawcy");
                });

            modelBuilder.Entity("SBDlibrary.Models.Zamowienie_ksiazki", b =>
                {
                    b.HasOne("SBDlibrary.Models.Ksiazki", "Ksiazki")
                        .WithMany()
                        .HasForeignKey("id_ksiazki")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SBDlibrary.Models.Zamowienia", "Zamowienia")
                        .WithMany("Zamowienie_ksiazki")
                        .HasForeignKey("id_zamowienia")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SBDlibrary.Models.Zwroty", b =>
                {
                    b.HasOne("SBDlibrary.Models.Wypozyczenia", "id_wypozyczenia")
                        .WithMany()
                        .HasForeignKey("id_wypozyczenia1");
                });
#pragma warning restore 612, 618
        }
    }
}
