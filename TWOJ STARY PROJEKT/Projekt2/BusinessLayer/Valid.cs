using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projekt2.DAL;
using Projekt2.Models;
using System.Web.Security;

namespace Projekt2.BusinessLayer
{
    public class Valid
    {
        public bool IsValid(UserAccount user)
        {
            DBA database = new DBA();
            foreach(UserAccount USER in database.GetUsers())
            {
                string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(user.Password, "sha1");
                if (USER.Username == user.Username && USER.Password == hash) return true;
            }
            return false;
        }
    }
}