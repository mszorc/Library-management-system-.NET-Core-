using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Projekt2.ViewModels.EntryVM
{
    public class EditVM
    {   
        [Key]
        public int ID { get; set; }
        [Display(Name = "Film")]
        [Required(ErrorMessage = "Film name is required")]
        public string Film { get; set; }

        [Display(Name = "Developer")]
        [Required(ErrorMessage = "Developer name is required")]
        public string Developer { get; set; }

        [Display(Name = "Dillution")]
        [Required(ErrorMessage = "Dillution is required")]
        public string Dillution { get; set; }

        [Display(Name = "ISO")]
        [Required]
        [Range(1, 12800, ErrorMessage = "ISO has to be from range 1- 12 800")]
        public int ISO { get; set; }

        [Display(Name = "Time")]
        [Required(ErrorMessage = "Time is required")]
        [Range(1, 10000, ErrorMessage = "Time has to be bigger or equal 1")]
        public double Time { get; set; }

        [Display(Name = "Temperature")]
        [Required(ErrorMessage = "Temperature is required")]
        [Range(1, 100, ErrorMessage = "Temperature has to be from range 1- 100")]
        public int Temperature { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        public string User { get; set; }
    }
}