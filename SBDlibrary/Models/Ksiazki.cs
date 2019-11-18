﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.Models
{
    public class Ksiazki
    {
        [Key]
        [Column(Order = 0)]
        public int id_ksiazki { get; set; }
        [Required]
        [Column(Order = 1)]
        public virtual Wydawnictwa Wydawnictwa { get; set; }
        [Required]
        [StringLength(50)]
        [Column(Order = 2)]
        public string tytuł { get; set; }
        [Required]
        [Column(Order = 3)]
        public DateTime data_wydania { get; set; }
    }
}