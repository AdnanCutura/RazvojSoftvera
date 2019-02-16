using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using RS1_Ispit_asp.net_core.Controllers;

namespace RS1_Ispit_asp.net_core.ViewModel {
    public class DodajMaturskiIspitVm {
        public int NastavnikId { get; set; }
        public string Nastavnik { get; set; }
        public List<SelectListItem> Skole { get; set; }

        [Required(ErrorMessage = "Odabir škole je obavezno polje")]
        [Display(Name = "Škola")]
        public int SkolaId { get; set; }
        public List<SelectListItem> Predmeti { get; set; }

        [Required(ErrorMessage = "Odabir predmeta je obavezno polje")]
        [Display(Name = "Predmet")]
        public int PredmetId { get; set; }
        public string SkolskaGodina { get; set; }

        [Required(ErrorMessage = "Odabir datuma ispita je obavezno polje")]
        [Range(typeof(DateTime), "1/1/2019", "1/1/2030", ErrorMessage = "Datum ne smije biti ispod 2019. godine")]
        [Display(Name = "Datum ispita")]
        public DateTime DatumIspita { get; set; }
    }
}
