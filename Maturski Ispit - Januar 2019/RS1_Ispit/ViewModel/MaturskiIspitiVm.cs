using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RS1_Ispit_asp.net_core.EntityModels;

namespace RS1_Ispit_asp.net_core.ViewModel {
    public class MaturskiIspitiVm {
        public List<MaturskiModel> MaturskiIspiti { get; set; }
        public int NastavnikId { get; set; }

        public class MaturskiModel {
            public int Id { get; set; }
            public string Skola { get; set; }
            public string SkolskaGodina { get; set; }
            public string DatumIspita { get; set; }
            public string Predmet { get; set; }
            public List<string> Odsutni { get; set; }
        }
    }
}
