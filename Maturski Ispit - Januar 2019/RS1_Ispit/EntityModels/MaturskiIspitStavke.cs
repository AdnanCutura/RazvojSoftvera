using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels {
    public class MaturskiIspitStavke {
        
        public int Id { get; set; }
        public int MaturskiIspitId { get; set; }
        public MaturskiIspit MaturskiIspit { get; set; }

        public int OdjeljenjeStavkaId { get; set; }
        public OdjeljenjeStavka OdjeljenjeStavka { get; set; }

        public double? ProsjekOcjena { get; set; }
        public bool PristupIspitu { get; set; }

        [Range(0, 100, ErrorMessage = "Bodovi moraju biti između 0 i 100")]
        [Display(Name = "Osvojeni bodovi")]
        public int? RezultatMaturskog { get; set; }
    }
}
