using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.ViewModel;
using System.Linq;

namespace RS1_Ispit_asp.net_core.Controllers {
    public class AjaxController : Controller {
        private readonly MojContext _dbContext;

        public AjaxController(MojContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index(int id)
        {

            var returnModel = new AjaxVm
            {
                MaturskiStavke = _dbContext.MaturskiIspitStavke
                    .Where(mi => mi.MaturskiIspitId == id)
                    .Select(s => new AjaxVm.UcenikModel
                    {
                        RezultattIspita = s.RezultatMaturskog ?? 0,
                        Ucenik = s.OdjeljenjeStavka.Ucenik.ImePrezime,
                        PristupioIspitu = s.PristupIspitu,
                        MaturskiStavkaId = s.Id,
                        ProsjekOcjena = _dbContext.DodjeljenPredmet.Where(dp => dp.OdjeljenjeStavka.UcenikId == s.OdjeljenjeStavka.UcenikId).Average(a => (double?)a.ZakljucnoKrajGodine)
                    })
                    .ToList()
            };

            return PartialView(returnModel);
        }

        public IActionResult Uredi(int id)
        {
            var stavka = _dbContext.MaturskiIspitStavke
                .Include(mis => mis.OdjeljenjeStavka.Ucenik)
                .First(mis => mis.Id == id);

            return PartialView(stavka);
        }

        public IActionResult Snimi(MaturskiIspitStavke model)
        {

            if (!ModelState.IsValid)
            {
                var stavka = new MaturskiIspitStavke
                {
                    RezultatMaturskog = model.RezultatMaturskog,
                    OdjeljenjeStavkaId = model.OdjeljenjeStavkaId,
                    MaturskiIspitId = model.MaturskiIspitId,
                    PristupIspitu = model.PristupIspitu,
                    ProsjekOcjena = model.ProsjekOcjena
                };
                return PartialView("Uredi", stavka);
            }
            _dbContext.MaturskiIspitStavke.Find(model.Id).RezultatMaturskog = model.RezultatMaturskog;
            _dbContext.SaveChanges();
            _dbContext.Dispose();

            return Redirect("/Ajax/Index/" + model.MaturskiIspitId);
        }

        [HttpPost]
        public IActionResult SnimiRezultat(int id, int bodovi)
        {
            if (!ModelState.IsValid)
            {
                var stavkaOld = new MaturskiIspitStavke
                {
                    OdjeljenjeStavkaId = id,
                    RezultatMaturskog = bodovi
                };
                return PartialView("Uredi", stavkaOld);
            }

            var stavka = _dbContext.MaturskiIspitStavke.Find(id);
            stavka.RezultatMaturskog = bodovi;
            _dbContext.SaveChanges();
            _dbContext.Dispose();

            return Redirect("/Ajax/Index/" + stavka.MaturskiIspitId);
        }
        public IActionResult Pristupio(int id)
        {
            var stavka = _dbContext.MaturskiIspitStavke
                .First(mis => mis.Id == id);
            stavka.PristupIspitu = !stavka.PristupIspitu;
            _dbContext.SaveChanges();
            _dbContext.Dispose();
            return Redirect("/Ajax/Index/" + stavka.MaturskiIspitId);
        }
    }
}