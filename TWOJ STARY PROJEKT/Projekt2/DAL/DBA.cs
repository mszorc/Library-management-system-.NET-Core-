using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projekt2.Models;
using Projekt2.DBAL;


namespace Projekt2.DAL
{
    public class DBA
    {
        

        public List<UserAccount> GetUsers()
        {
            DataBase database = new DataBase();
            return database.GetUserList();
        }
        
        public void RegisterUser(UserAccount user)
        {
            DataBase database = new DataBase();
            database.Register(user);
        }

        public List<Entry> GetEntries()
        {
            DataBase database = new DataBase();
            return database.GetEntriesList();
        }

        public void AddEntry(Entry entry)
        {
            DataBase database = new DataBase();
            database.AddEntry(entry);
        }

        public void Delete(Entry entry)
        {
            DataBase database = new DataBase();
            database.Delete(entry);
        }
        public void Edit(Entry entry)
        {
            DataBase database = new DataBase();
            database.Edit(entry);
        }
    }
}