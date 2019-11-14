using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Data.Entity;
using Projekt2.Models;
using System.IO;
using System.Data.Entity.Migrations;

namespace Projekt2.DBAL
{
    public class DataBase : DbContext
    {
        public int id = 1;
        public DataBase() : base("DevBase")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entry>().ToTable("EntriesTab");
            modelBuilder.Entity<UserAccount>().ToTable("UsersTab");
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UserAccount> UserDB { get; set; }
        public DbSet<Entry> EntriesDB { get; set; }

        public List<UserAccount> GetUserList()
        {
            List<UserAccount> UserList = new List<UserAccount>();
            UserList = UserDB.ToList();
            return UserList;
        }

        public void Register(UserAccount user)
        {
            user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(user.Password, "sha1");
            UserDB.Add(user);
            SaveChanges();
        }
        

        public List<Entry> GetEntriesList()
        {
            List<Entry> EntriesList = new List<Entry>();
            EntriesList = EntriesDB.ToList();
            return EntriesList;
        }

        public void AddEntry (Entry entry)
        {
            entry.ID = id;
            EntriesDB.Add(entry);
            SaveChanges();
            id++;
        }
        
        public void Delete(Entry entry)
        {
            var itemtodelete = EntriesDB.SingleOrDefault(x => x.ID == entry.ID);
            EntriesDB.Remove(itemtodelete);
            SaveChanges();
        }
        public void Edit(Entry entry)
        {
            EntriesDB.AddOrUpdate(entry);
            SaveChanges();
        }
    }
}