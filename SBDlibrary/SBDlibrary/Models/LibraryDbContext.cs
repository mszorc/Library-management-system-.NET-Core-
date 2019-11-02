using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.Models
{
    public class LibraryDbContext: DbContext
    {
        public LibraryDbContext(): base()
        {

        }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
           : base(options)
        {
        }

        public virtual DbSet<Dostawcy> Dostawcy { get; set; }
        public virtual DbSet<Zamowienia> Zamowienia { get; set; }
        public virtual DbSet<Zamowienie_ksiazki> Zamowienie_ksiazki { get; set; }
        public virtual DbSet<Egzemplarze> Egzemplarze { get; set; }


    }
}
