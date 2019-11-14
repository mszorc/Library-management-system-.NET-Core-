using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Projekt2.ViewModels.EntryVM
{
    public class SearchVM
    {
        [Required(ErrorMessage ="Film is required")]
        public string Film { get; set; }
        [Required(ErrorMessage ="Developer is required")]
        public string Developer { get; set; }
    }
}