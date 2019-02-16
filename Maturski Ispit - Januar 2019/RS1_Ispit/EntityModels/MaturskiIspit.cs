using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using RS1_Ispit_asp.net_core.Controllers;

namespace RS1_Ispit_asp.net_core.EntityModels {
    public class MaturskiIspit {
        public int Id { get; set; }
        
        public int SkolaId { get; set; }
        public Skola Skola { get; set; }

        public int NastavnikId { get; set; }
        public Nastavnik Nastavnik { get; set; }

        public int SkolskaGodinaId { get; set; }
        public SkolskaGodina SkolskaGodina { get; set; }
       
        public int PredmetId { get; set; }
        public Predmet Predmet { get; set; }
        
        public DateTime DatumIspita { get; set; }
        public string Napomena { get; set; }

    }
}
