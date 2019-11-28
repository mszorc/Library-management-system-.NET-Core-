using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.ViewModel
{
    public class InputModelChangePassword
    {
        [Required(ErrorMessage = "Pole 'Stare hasło' jest wymagane.")]
        [DataType(DataType.Password)]
        [Display(Name = "Stare hasło")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Pole 'Nowe hasło' jest wymagane.")]
        [DataType(DataType.Password, ErrorMessage = "Hasło musi posiadać duże i małe litery, cyfry oraz znaki specjalne")]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź nowe hasło")]
        [Compare("NewPassword", ErrorMessage = "Pola 'Nowe hasło' i 'Potwierdź nowe hasło' muszą być identyczne")]
        public string ConfirmNewPassword { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

    }
}
