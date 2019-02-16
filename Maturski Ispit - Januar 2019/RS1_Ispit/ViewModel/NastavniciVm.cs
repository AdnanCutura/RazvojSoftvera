using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RS1_Ispit_asp.net_core.EntityModels;

namespace RS1_Ispit_asp.net_core.ViewModel {
    public class NastavniciVm {
        public List<NastavnikModel> Nastavnici { get; set; }
        public class NastavnikModel {
            public string ImePrezime { get; set; }
            public int NastavnikId { get; set; }
            public List<string> Skole { get; set; }
        }


    }
}
