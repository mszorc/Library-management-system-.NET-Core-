using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Projekt2.Models;
using Projekt2.DAL;
using Projekt2.BusinessLayer;

namespace Projekt2.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserAccount user)
        {
            if (ModelState.IsValid)
            {
                DBA database = new DBA();
                database.RegisterUser(user);

                return RedirectToAction("Login");
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserAccount user)
        {
            if (user.Username != null && user.Password != null)
            {
            Valid valid = new Valid();
            if (valid.IsValid(user))
            {
                FormsAuthentication.SetAuthCookie(user.Username, true);
                TempData["LoginName"] = user.Username;
                return RedirectToAction("Index", "Entry");
            }
            else
            {
                ModelState.AddModelError("CredentialError", "Username od Password wrong");
            }
            }
            return View();
        }
        

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            TempData["LoginName"] = null;
            return RedirectToAction("Index","Entry");
        }
    }
}