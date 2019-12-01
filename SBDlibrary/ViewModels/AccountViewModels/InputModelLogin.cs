using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.ViewModel
{
    public class InputModelLogin
    {
        [Required(ErrorMessage = "Pole 'Email' jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole 'Hasło' jest wymagane.")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
    }
}
