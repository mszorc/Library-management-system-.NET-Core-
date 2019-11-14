using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Projekt2.ViewModels.AuthenticationVM
{
    public class LoginVM
    {
        [Display(Name = "Username")]
        [Required]
        public string username { get; set; }
        [Display(Name = "Password")]
        [Required]
        public string password { get; set; }
    }
}