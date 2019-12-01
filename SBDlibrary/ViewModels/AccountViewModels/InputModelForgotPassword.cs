using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.ViewModel
{
    public class InputModelForgotPassword
    {
        [Required(ErrorMessage = "Pole jest wymagane.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
