﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.Models
{
    public class Egzemplarze
    {
        public enum Status
        {
            Niedostepny, Dostępny, Zarezerwowany, Wypozyczony
        }

        [Key]
        [Column(Order = 0)] 
        public int id_egzemplarza { get; set; }
        [Column(Order = 1)]
        public int id_ksiazki { get; set; }  
        [Required]
        [Column(Order = 2)]
        [ForeignKey("id_ksiazki")]
        public virtual Ksiazki Ksiazki { get; set; }
        //DefaultValue("dostepny")]
        public Status? status { get; set; }
        public ICollection<Wypozyczenia> Wypozyczenia { get; set; }
        public ICollection<Rezerwacje> Rezerwacje { get; set; }
    }

    public class SimpleCreateModel
    {
       
        [Display(Name = "id_ksiazki")]
        public int id_ksiazki;
    }
}
