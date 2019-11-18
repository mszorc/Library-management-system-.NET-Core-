using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
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

        

        public AccountController(LibraryDbContext libraryDbContext, 
            UserManager<Uzytkownicy> userManager, SignInManager<Uzytkownicy> signInManager)
        {
            _context = libraryDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([Bind("Email")] InputModelForgotPassword model)
        {
            return RedirectToAction("Home", "Index");
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn([Bind("Id,Password")] InputModelLogin model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
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
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([Bind("Email,FirstName,LastName,Address,Password,ConfirmPassword")]InputModelRegister model)
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

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    user = await _userManager.FindByEmailAsync(model.Email);
                    await _userManager.AddToRoleAsync(user, "Klient");


                  
                    return RedirectToAction("Index", "Home");
                }

            }
            return View();
        }






        public class InputModelLogin
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Id")]
            public int Id { get; set; }

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
            public string Address { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password*")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password*")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public class InputModelForgotPassword
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
        }
    }
}