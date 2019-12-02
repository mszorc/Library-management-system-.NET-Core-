﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SBDlibrary.Models;
using SBDlibrary.ViewModel;
using SBDlibrary.ViewModels.AccountViewModels;

namespace SBDlibrary.Controllers
{
    public class KontaController : Controller
    {
        private readonly LibraryDbContext _context;
        private readonly UserManager<Uzytkownicy> _userManager;
        private readonly SignInManager<Uzytkownicy> _signInManager;
        private readonly IEmailSender _emailSender;

        [TempData]
        public string StatusMessage { get; set; }


        private IHttpContextAccessor _accessor;

       
        public KontaController(LibraryDbContext libraryDbContext, 
            UserManager<Uzytkownicy> userManager, SignInManager<Uzytkownicy> signInManager,
            IEmailSender emailSender, IHttpContextAccessor accessor)
        {
            _context = libraryDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _accessor = accessor;
        }

        public IActionResult Index()
        {
            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ZnajdzUzytkownika(string email)
        {
            var uzytkownicy = from m in _context.Uzytkownicy
                          select m;

            if (!String.IsNullOrEmpty(email))
            {
                uzytkownicy = uzytkownicy.Where(s => s.email.Contains(email));
            }

            return View(uzytkownicy);
        }


        [HttpGet]
        [Authorize]
        public IActionResult ZmienHaslo()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ZmienHaslo([Bind("Password,NewPassword,ConfirmNewPassword")] InputModelChangePassword model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Zmiana hasła nie przebiegła pomyślnie");
                return View();
            }
            model.StatusMessage = "Zmiana hasła przebiegła pomyślnie";
            await _signInManager.RefreshSignInAsync(user);
            return View(model);

        }

        [HttpGet]
        public IActionResult ZapomnianoHasla()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ZapomnianoHasla([Bind("Email")] InputModelForgotPassword model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (ModelState.IsValid)
            {
                if (user == null)
                {
                    return View();
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var callbackUrl = Url.Action(
                    "ResetujHaslo",
                    "Konta",
                    new { code = code },
                    protocol: HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(model.Email, "Reset password",
                    $"Zresetuj swoje hasło <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>klikając tutaj</a>.");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Zaloguj()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Zaloguj([Bind("Email,Password")] InputModelLogin model)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByEmailAsync(model.Email);

                Logi log = new Logi();
                log.Uzytkownicy = user;
                var ip = _accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                log.ip_urzadzenia = ip;

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Logowanie przebiegło niepomyślnie.");
                    log.Uzytkownicy = await _context.Uzytkownicy.FirstOrDefaultAsync(m => m.email == "niezidentyfikowany");
                    log.komunikat = "logowanie niepomyślne, nieodnaleziono uzytkownika";
                    _context.Logi.Add(log);
                    _context.SaveChanges();
                    return View();
                }

                var result = await _signInManager.PasswordSignInAsync(user.id_uzytkownika.ToString(), model.Password, false, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    log.komunikat = "logowanie pomyslne";
                    _context.Logi.Add(log);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    log.komunikat = "logowanie niepomyślne, błędne hasło";
                    _context.Logi.Add(log);
                    _context.SaveChanges();
                    ModelState.AddModelError(string.Empty, "Logowanie przebiegło niepomyślnie");
                }
            }         

            return View();
        }

        [Authorize]
        public IActionResult Wyloguj()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Zarzadzaj()
        {

            var user = await _userManager.GetUserAsync(User);
            var Input = new InputModelChangePersonalData
            {
                FirstName = user.imie,
                LastName = user.nazwisko,
                Address = user.adres_zamieszkania
            };
            Input.StatusMessage = null;

            return View(Input);
        }
        [HttpPost]
        public async Task<IActionResult> Zarzadzaj([Bind("FirstName,LastName,Address")]InputModelChangePersonalData model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                user.imie = model.FirstName;
                user.nazwisko = model.LastName;
                user.adres_zamieszkania = model.Address;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    model.StatusMessage = (string)"Profil został zaktualizowany";
                    return View(model);
                }

            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ZmienDaneOsobowe(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            var Input = new InputModelChangePersonalData
            {
                FirstName = user.imie,
                LastName = user.nazwisko,
                Address = user.adres_zamieszkania
            };
            Input.StatusMessage = null;

            return View(Input);
        }

        [HttpPost]
        public async Task<IActionResult> ZmienDaneOsobowe(int id, [Bind("FirstName,LastName,Address")]InputModelChangePersonalData model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id.ToString());

                if (user == null)
                {
                    return NotFound();
                }

                user.imie = model.FirstName;
                user.nazwisko = model.LastName;
                user.adres_zamieszkania = model.Address;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    model.StatusMessage = (string)"Profil został zaktualizowany";
                    return View(model);
                }

            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Bibliotekarz,Admin")]
        public IActionResult Zarejestruj()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Bibliotekarz,Admin")]
        public async Task<IActionResult> Zarejestruj([Bind("Email,FirstName,LastName,Address")]InputModelRegister model)
        {
            if (ModelState.IsValid)
            {
                var check = await _userManager.FindByEmailAsync(model.Email);
                if (check != null)
                {
                    ModelState.AddModelError(string.Empty, "Użytkownik z podanym adresem email już istnieje.");
                    return View();
                }

                var user = new Uzytkownicy
                {
                    email = model.Email,
                    imie = model.FirstName,
                    nazwisko = model.LastName,
                    adres_zamieszkania = model.Address
                };
                var password = RandomPassword();
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    model.StatusMessage = "Rejestracja przebiegła pomyślnie";
                    user = await _userManager.FindByEmailAsync(model.Email);
                    await _userManager.AddToRoleAsync(user, "Klient");
                    await _emailSender.SendEmailAsync(model.Email, 
                        "Twoje konto w aplikacji SBDlibrary zostało utworzone. ", "Login: " + model.Email + " Hasło: " + password);
                    return View(model);
                }

            }
            return View();
        }

        [HttpGet]
        public IActionResult ResetujHaslo(string code)
        {
            TempData["Token"] = (string)code;
            if (TempData["Error"] != null)
            {
                ModelState.AddModelError(string.Empty, (string)TempData["Error"]);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetujHaslo([Bind("Email,Password,ConfirmPassword")] InputModelResetPassword model)
        {
            var token = (string)TempData["Token"];
            if (ModelState.IsValid)
            {
                
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction("ResetPassword", "Account", new { code = token });
                }
                var passwordValidator = new PasswordValidator<Uzytkownicy>();
                var result = await passwordValidator.ValidateAsync(_userManager, null, model.Password);
                if (!result.Succeeded)
                {
                    TempData["Error"] = "Hasło musi zawierać małe i duże litery, cyfry oraz znaki specjalne.";
                    return RedirectToAction("ResetPassword", "Account", new { code = token });
                }
                result = await _userManager.ResetPasswordAsync(user, token, model.Password);
                //TempData["Token"] = null;
                if (result.Succeeded)
                {
                    Logi log = new Logi();
                    var ip = _accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                    log.Uzytkownicy = user;
                    log.ip_urzadzenia = ip;
                    log.komunikat = "zmieniono hasło";
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("ResetPassword", "Account", new { code = token });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UstawRole(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            var Input = new InputModelSetRoles
            {
                Id = (int) id,
                Email = user.email,
                StatusMessage = null
            };

            if (await _userManager.IsInRoleAsync(user, "Klient"))
            {
                ViewBag.Klient = "checked";
            }
            if (await _userManager.IsInRoleAsync(user, "Bibliotekarz"))
            {
                ViewBag.Bibliotekarz = "checked";
            }
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                ViewBag.Admin = "checked";
            }

            return View(Input);
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> UstawRole(int id, IFormCollection formCollection)
        {
            bool KlientCheckbox = false, BibliotekarzCheckbox = false, AdminCheckbox = false;
            string KlientCheckboxValue = "";
            string BibliotekarzCheckboxValue = "";
            string AdminCheckboxValue = "";
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(formCollection["KlientCheckbox"])) { KlientCheckbox = true; }
            if (!string.IsNullOrEmpty(formCollection["BibliotekarzCheckbox"])) { BibliotekarzCheckbox = true; }
            if (!string.IsNullOrEmpty(formCollection["AdminCheckbox"])) { AdminCheckbox = true; }

            if (KlientCheckbox) { KlientCheckboxValue = formCollection["KlientCheckbox"]; }
            if (BibliotekarzCheckbox) { BibliotekarzCheckboxValue = formCollection["BibliotekarzCheckbox"]; }
            if (AdminCheckbox) { AdminCheckboxValue = formCollection["AdminCheckbox"]; }

            bool IsKlient = await _userManager.IsInRoleAsync(user, "Klient");
            bool IsBibliotekarz = await _userManager.IsInRoleAsync(user, "Bibliotekarz");
            bool IsAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            var KlientRoleId = await _context.Role.FirstOrDefaultAsync(m => m.nazwa == "Klient");
            var BibliotekarzRoleId = await _context.Role.FirstOrDefaultAsync(m => m.nazwa == "Bibliotekarz");
            var AdminRoleId = await _context.Role.FirstOrDefaultAsync(m => m.nazwa == "Admin");


            if (KlientCheckbox != IsKlient)
            {
                if (IsKlient)
                {
                    var user_role = await _context.Uzytkownicy_role.FirstOrDefaultAsync(m => m.id_uzytkownika == user.id_uzytkownika
                                                                                    && m.id_roli == KlientRoleId.id_roli);
                    _context.Remove(user_role);
                    await _context.SaveChangesAsync();
                    ViewBag.Klient = "";
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "Klient");
                    await _context.SaveChangesAsync();
                    ViewBag.Klient = "checked";
                }
            }
            else
            {
                if (IsKlient) ViewBag.Klient = "checked";
                else ViewBag.Klient = "";
            }

            if (BibliotekarzCheckbox != IsBibliotekarz)
            {
                if (IsBibliotekarz)
                {
                    var user_role = await _context.Uzytkownicy_role.FirstOrDefaultAsync(m => m.id_uzytkownika == user.id_uzytkownika
                                                                                    && m.id_roli == BibliotekarzRoleId.id_roli);
                    _context.Remove(user_role);
                    await _context.SaveChangesAsync();
                    ViewBag.Bibliotekarz = "";
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "Bibliotekarz");
                    await _context.SaveChangesAsync();
                    ViewBag.Bibliotekarz = "checked";
                }
            }
            else
            {
                if (IsBibliotekarz) ViewBag.Bibliotekarz = "checked";
                else ViewBag.Bibliotekarz = "";
            }

            if (AdminCheckbox != IsAdmin)
            {
                if (IsAdmin)
                {
                    var user_role = await _context.Uzytkownicy_role.FirstOrDefaultAsync(m => m.id_uzytkownika == user.id_uzytkownika
                                                                                    && m.id_roli == AdminRoleId.id_roli);
                    _context.Remove(user_role);
                    await _context.SaveChangesAsync();
                    ViewBag.Admin = "";
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                    await _context.SaveChangesAsync();
                    ViewBag.Admin = "checked";
                }
            }
            else
            {
                if (IsAdmin) ViewBag.Admin = "checked";
                else ViewBag.Admin = "";
            }

            var Input = new InputModelSetRoles
            {
                Id = (int)id,
                Email = user.email,
                StatusMessage = "Zmiana ról przebiegła pomyślnie"
            };

            return View(Input);
        }
        
        [Authorize(Roles = "Klient")]
        public async Task<IActionResult> KsiazkiKlienta (int ?id)
        {
          // if(id==0)
            var user = await _userManager.GetUserAsync(User);

            var wypozyczenia =  _context.Wypozyczenia.Where(m => m.id_uzytkownika == user.id_uzytkownika);
            var egzemplarze = from b in wypozyczenia from c in _context.Egzemplarze.Where(c => b.id_egzemplarza == c.id_egzemplarza) select c;
            var Ksiazki = from c in egzemplarze from d in _context.Ksiazki.Where(d => c.id_ksiazki == d.id_ksiazki) select d;
            return View(Ksiazki);
        }

        [Authorize(Roles = "Klient")]
        public async Task<IActionResult> RezerwacjeKlienta(int? id)
        {
            // if(id==0)
            var user = await _userManager.GetUserAsync(User);

            var rezerwacje = _context.Rezerwacje.Where(m => m.id_uzytkownika == user.id_uzytkownika);
           foreach(Rezerwacje x in rezerwacje)
            {
                x.Egzemplarze = await _context.Egzemplarze.FirstOrDefaultAsync(m => m.id_egzemplarza == x.id_egzemplarza);
                x.Uzytkownicy= await _context.Uzytkownicy.FirstOrDefaultAsync(m => m.id_uzytkownika == x.id_uzytkownika);
            }
            return View(rezerwacje);
        }


        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }


        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        private string RandomSpecialCharacter(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(14 * random.NextDouble() + 33)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        private string RandomPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            builder.Append(RandomSpecialCharacter(2));
            return builder.ToString();
        }
    }
}