using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModel {
    public class AjaxVm {

        public List<UcenikModel> MaturskiStavke { get; set; }

        public class UcenikModel {
            public int MaturskiStavkaId { get; set; }
            public string Ucenik { get; set; }
            public double? ProsjekOcjena { get; set; }
            public bool PristupioIspitu { get; set; }

            [Range(0, 100, ErrorMessage = "Bodovi moraju biti između 0 i 100")]
            [Display(Name = "Osvojeni bodovi")]
            public int RezultattIspita { get; set; }
        }
    }
}
