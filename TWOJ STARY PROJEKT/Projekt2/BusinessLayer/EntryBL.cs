using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projekt2.DAL;
using Projekt2.Models;

namespace Projekt2.BusinessLayer
{
    public class EntryBL
    {
        public List<Entry> List()
        {
            DBA database = new DBA();
            List<Entry> entriesList = database.GetEntries();
            return entriesList;
        }
        public void AddEntry(Entry entry)
        {
            DBA database = new DBA();
            database.AddEntry(entry);
        }
        public void Delete(Entry entry)
        {
            DBA database = new DBA();
            database.Delete(entry);
        }
        public void Edit(Entry entry)
        {
            DBA database = new DBA();
            database.Edit(entry);
        }
    }
}