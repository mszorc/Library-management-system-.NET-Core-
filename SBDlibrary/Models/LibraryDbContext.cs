using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.Models
{
    public class LibraryDbContext: DbContext
    {

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
           : base(options)
        {
        }

        public virtual DbSet<Dostawcy> Dostawcy { get; set; }
        public virtual DbSet<Zamowienia> Zamowienia { get; set; }
        public virtual DbSet<Zamowienie_ksiazki> Zamowienie_ksiazki { get; set; }
        public virtual DbSet<Egzemplarze> Egzemplarze { get; set; }
        public virtual DbSet<Logi> Logi { get; set; }
        public virtual DbSet<Uzytkownicy> Uzytkownicy { get; set; }
        public virtual DbSet<Uzytkownicy_role> Uzytkownicy_role { get; set; }
        public virtual DbSet<Role> Role { get; set; }

        public virtual DbSet<Autor> Autor { get; set; }
        public virtual DbSet<Ksiazki> Ksiazki { get; set; }
        public virtual DbSet<Wydawnictwa> Wydawnictwa { get; set; }
        public virtual DbSet<Kategorie> Kategorie { get; set; }
        public virtual DbSet<Kategorie_Ksiazki> Kategorie_Ksiazki { get; set; }
        public virtual DbSet<Autorzy_Ksiazki> Autorzy_Ksiazki { get; set; }

        public virtual DbSet<Rezerwacje> Rezerwacje { get; set; }
        public virtual DbSet<Wypozyczenia> Wypozyczenia { get; set; }
        public virtual DbSet<Zwroty> Zwroty { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dostawcy>().ToTable("Dostawcy");
            modelBuilder.Entity<Zamowienia>().ToTable("Zamowienia");
            //modelBuilder.Entity<Zamowienie_ksiazki>().ToTable("Zamowienie_ksiazki");
            modelBuilder.Entity<Zamowienie_ksiazki>().HasKey(c => new { c.id_zamowienia, c.id_ksiazki });
            modelBuilder.Entity<Egzemplarze>().ToTable("Egzemplarze");
            modelBuilder.Entity<Logi>().ToTable("Logi");
            modelBuilder.Entity<Uzytkownicy>().ToTable("Uzytkownicy");
            //modelBuilder.Entity<Uzytkownicy_role>().ToTable("Uzytkownicy_role");
            modelBuilder.Entity<Uzytkownicy_role>().HasKey(c => new { c.id_uzytkownika, c.id_roli });
            modelBuilder.Entity<Role>().ToTable("Role");

            modelBuilder.Entity<Autor>().ToTable("Autor");
            modelBuilder.Entity<Ksiazki>().ToTable("Ksiazki");
            modelBuilder.Entity<Autorzy_Ksiazki>().HasKey(d => new { d.id_autora, d.id_ksiazki});
            modelBuilder.Entity<Wydawnictwa>().ToTable("Wydawnictwa");
            modelBuilder.Entity<Kategorie>().ToTable("Kategorie");
            modelBuilder.Entity<Kategorie_Ksiazki>().HasKey(c => new { c.id_kategorii, c.id_ksiazki });

            modelBuilder.Entity<Rezerwacje>().ToTable("Rezerwacje");
            modelBuilder.Entity<Wypozyczenia>().ToTable("Wypozyczenia");
            modelBuilder.Entity<Zwroty>().ToTable("Zwroty");

        }


    }
}
