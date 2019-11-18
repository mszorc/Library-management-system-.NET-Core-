using System;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SBDlibrary.Models;

namespace SBDlibrary.Controllers
{
    public class AccountController : Controller
    {
        private readonly LibraryDbContext _context;
        private readonly UserManager<Uzytkownicy> _userManager;
        private readonly SignInManager<Uzytkownicy> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(LibraryDbContext libraryDbContext, 
            UserManager<Uzytkownicy> userManager, SignInManager<Uzytkownicy> signInManager,
            IEmailSender emailSender)
        {
            _context = libraryDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([Bind("Email")] InputModelForgotPassword model)
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
                    "ResetPassword",
                    "Account",
                    new { code = code },
                    protocol: HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(model.Email, "Reset password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn([Bind("Email,Password")] InputModelLogin model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (ModelState.IsValid)
            {
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }

                var result = await _signInManager.PasswordSignInAsync(user.id_uzytkownika.ToString(), model.Password, false, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }         

            return View();
        }

        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return View();
        }

        public IActionResult Manage()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Bibliotekarz")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([Bind("Email,FirstName,LastName,Address")]InputModelRegister model)
        {
            if (ModelState.IsValid)
            {
                var check = await _userManager.FindByEmailAsync(model.Email);
                if (check != null)
                {
                    ModelState.AddModelError(string.Empty, "User with given email address already exists");
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
                    user = await _userManager.FindByEmailAsync(model.Email);
                    await _userManager.AddToRoleAsync(user, "Klient");
                    await _emailSender.SendEmailAsync(model.Email, 
                        "Your SBDlibrary account has been created.", "Login: " + model.Email + "Password: " + password);
                    return RedirectToAction("Index", "Home");
                }

            }
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string code)
        {
            TempData["Token"] = (string)code;
            /*if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                TempData["Model"] = new InputModelResetPassword
                {
                    Code = code
                };
            }*/
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([Bind("Email,Password,ConfirmPassword")] InputModelResetPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return View();
                }
                var result = await _userManager.ResetPasswordAsync(user, (string)TempData["Token"], model.Password);
                TempData["Model"] = null;
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

            }

            return View();
        }

        public class InputModelLogin
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
        }

        public class InputModelRegister
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email*")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
            [Display(Name = "First name*")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
            [Display(Name = "Last name*")]
            public string LastName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
            [Display(Name = "Address*")]
            public string Address { get; set; }/*

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password*")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password*")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }*/
        }

        public class InputModelForgotPassword
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
        }

        public class InputModelResetPassword
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password*")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password*")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
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