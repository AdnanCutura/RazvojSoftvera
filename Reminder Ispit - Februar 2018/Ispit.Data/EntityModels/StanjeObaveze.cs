using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ispit.Data.EntityModels {

    public class StanjeObaveze {
        public int Id { get; set; }
        public DateTime DatumIzvrsenja { get; set; }

        [Range(0, 100, ErrorMessage = "Opseg mora biti između 0 i 100.")]
        [Display(Name = "Izršeno procentualno")]
        public float IzvrsenoProcentualno { get; set; }
        public int NotifikacijaDanaPrije { get; set; }
        public bool NotifikacijeRekurizivno { get; set; }
        public bool IsZavrseno { get; set; }


        [ForeignKey(nameof(Obaveza))]
        public int ObavezaID { get; set; }
        public Obaveza Obaveza { get; set; }




        [ForeignKey(nameof(OznacenDogadjaj))]
        public int OznacenDogadjajID { get; set; }
        public OznacenDogadjaj OznacenDogadjaj { get; set; }




    }
}
