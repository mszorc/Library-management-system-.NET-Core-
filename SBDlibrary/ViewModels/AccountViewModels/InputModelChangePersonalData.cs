using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.ViewModel
{
    public class InputModelChangePersonalData
    {
        [Required(ErrorMessage = "Pole 'Imię' jest wymagane.")]
        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "Imię musi mieć co najmniej 2 oraz maksymalnie 20 znaków długości.", MinimumLength = 2)]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Pole 'Nazwisko' jest wymagane.")]
        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "Nazwisko musi mieć co najmniej 2 oraz maksymalnie 20 znaków długości.", MinimumLength = 2)]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Pole 'Adres zamieszkania' jest wymagane.")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Adres zamieszkania musi mieć co najmniej 2 oraz maksymalnie 20 znaków długości.", MinimumLength = 2)]
        [Display(Name = "Adres zamieszkania")]
        public string Address { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

    }
}
