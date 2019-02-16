using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ispit.Data.EntityModels;

namespace Ispit.Web.ViewModels {
    public class DogadjajiVm {
        public List<NeoznacenDogadjaj> NeoznaceniDogadjaji { get; set; }
        public List<OznaceniDogadjaj> OznaceniDogadjaji { get; set; }

        public class NeoznacenDogadjaj {
            public int ID { get; set; }
            public string Opis { get; set; }
            public DateTime DatumOdrzavanja { get; set; }
            public string Nastavnik { get; set; }
            public int BrojObaveza { get; set; }

        }

        public class OznaceniDogadjaj {
            public int ID { get; set; }
            public string Opis { get; set; }
            public DateTime DatumOdrzavanja { get; set; }
            public string Nastavnik { get; set; }
            public int RealizovanoObaveza { get; set; }
        }

        public List<StanjeObaveze> StanjaObaveza { get; set; }

        public List<PoslataNotifikacija> Notifikacije { get; set; }

        public List<Dogadjaj> NastavnikDogadjaji { get; set; }

    }
}
