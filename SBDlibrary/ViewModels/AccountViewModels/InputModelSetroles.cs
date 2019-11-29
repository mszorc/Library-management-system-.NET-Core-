using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.ViewModels.AccountViewModels
{
    public class InputModelSetRoles
    {
        [Required(ErrorMessage = "Pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
    }
}
