using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Projekt2.Models
{
    public class Entry
    {
        [Key]
        public int ID { get; set; }
        public string Film { get; set; }
        public string Developer { get; set; }
        public string Dillution { get; set; }
        public int ISO { get; set; }
        public double Time { get; set; }
        public int Temperature { get; set; }
        public string Notes { get; set; }
        public string User { get; set; }
    }
}