using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.ViewModel;
using System;
using System.Linq;

namespace RS1_Ispit_asp.net_core.Controllers {
    public class OdrzanaNastavaController : Controller {

        private readonly MojContext _dbContext;

        public OdrzanaNastavaController(MojContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var model = new NastavniciVm
            {
                Nastavnici = _dbContext.Nastavnik
                    .Select(n => new NastavniciVm.NastavnikModel
                    {
                        ImePrezime = n.Ime + " " + n.Prezime,
                        NastavnikId = n.Id,
                        Skole = _dbContext.PredajePredmet
                            .Where(o => o.NastavnikID == n.Id)
                            .GroupBy(g => g.Odjeljenje.Skola.Naziv)
                            .Select(q => q.Key)
                            .ToList()

                    })
                    .ToList()
            };
            return View(model);
        }

        public IActionResult Odaberi(int id)
        {

            var model = new MaturskiIspitiVm
            {
                NastavnikId = id,
                MaturskiIspiti = _dbContext.MaturskiIspit
                    .Where(m => m.NastavnikId == id)
                    .Select(m => new MaturskiIspitiVm.MaturskiModel
                    {
                        Id = m.Id,
                        DatumIspita = m.DatumIspita.ToString("dd.MM.yyyy."),
                        Predmet = m.Predmet.Naziv,
                        SkolskaGodina = m.SkolskaGodina.ToString(),
                        Skola = m.Skola.Naziv,
                        Odsutni = _dbContext.MaturskiIspitStavke
                            .Where(w => w.MaturskiIspitId == m.Id && !w.PristupIspitu)
                            .Select(q => q.OdjeljenjeStavka.Ucenik.ImePrezime)
                            .ToList()
                    })
                    .ToList()

            };

            return View(model);
        }

        public IActionResult Dodaj(int id)
        {
            var nastavnik = _dbContext.Nastavnik.Find(id);
            var model = PripremaMaturskogIspita(id, nastavnik);

            return View(model);
        }

        private DodajMaturskiIspitVm PripremaMaturskogIspita(int id, Nastavnik nastavnik)
        {
            var model = new DodajMaturskiIspitVm
            {
                NastavnikId = id,
                Nastavnik = nastavnik.Ime + " " + nastavnik.Prezime,
                SkolskaGodina = _dbContext.SkolskaGodina
                    .Where(sk => sk.Aktuelna)
                    .Select(s => s.Naziv)
                    .FirstOrDefault(),
                Skole = _dbContext.PredajePredmet
                    .Where(o => o.NastavnikID == id)
                    .GroupBy(g => g.Odjeljenje.Skola.Naziv)
                    .Select(s => new SelectListItem
                    {
                        Text = s.Key,
                        Value = s.Select(q => q.Odjeljenje.Skola.Id).FirstOrDefault().ToString()
                    })
                    .ToList(),
                Predmeti = _dbContext.PredajePredmet
                    .Where(p => p.NastavnikID == id)
                    .GroupBy(p => p.Predmet.Naziv)
                    .Select(
                        q => new SelectListItem
                        {
                            Text = q.Key,
                            Value = q.Select(x => x.Predmet.Id).FirstOrDefault().ToString()
                        }
                    )
                    .ToList(),
                DatumIspita = DateTime.Now
            };
            return model;
        }

        [HttpPost]
        public IActionResult Snimi(DodajMaturskiIspitVm model)
        {
            if (!ModelState.IsValid)
            {
                var nastavnik = _dbContext.Nastavnik.Find(model.NastavnikId);
                var model1 = PripremaMaturskogIspita(nastavnik.Id, nastavnik);
                return View("Dodaj", model1);
            }

            var maturskiIspit = new MaturskiIspit
            {
                DatumIspita = model.DatumIspita,
                SkolaId = model.SkolaId,
                NastavnikId = model.NastavnikId,
                PredmetId = model.PredmetId,
                SkolskaGodina = _dbContext.SkolskaGodina.FirstOrDefault(sg => sg.Naziv == model.SkolskaGodina)
            };

            _dbContext.MaturskiIspit.Add(maturskiIspit);
            _dbContext.SaveChanges();

            _dbContext.MaturskiIspitStavke
                .AddRange(
                    _dbContext.DodjeljenPredmet
                        .GroupBy(dp => dp.OdjeljenjeStavkaId)
                        .Where(w =>
                            w.Select(q => q.OdjeljenjeStavka.Odjeljenje.Razred).FirstOrDefault() == 4 &&
                            w.Select(q => q.OdjeljenjeStavka.Odjeljenje.SkolaID).First() == model.SkolaId &&
                            w.Count(a => a.ZakljucnoKrajGodine == 1) == 0
                        )
                        .Select(s => new MaturskiIspitStavke
                        {
                            RezultatMaturskog = 0,
                            MaturskiIspitId = maturskiIspit.Id,
                            PristupIspitu = true,
                            OdjeljenjeStavkaId = s.Select(q => q.OdjeljenjeStavka.Id)
                                        .FirstOrDefault()
                        })
                          .ToList()
                    );


            TempData["message-key"] = "Maturski ispit je uspješno dodan";

            _dbContext.SaveChanges();
            _dbContext.Dispose();

            return Redirect("/OdrzanaNastava/Odaberi/" + model.NastavnikId);
        }

        public IActionResult Uredi(int id)
        {
            var maturski = _dbContext.MaturskiIspit
                .Include(m => m.Predmet)
                .Include(m => m.Nastavnik)
                .First(m => m.Id == id);

            return View(maturski);
        }

        [HttpPost]
        public IActionResult SnimiMaturski(MaturskiIspit model)
        {
            var maturski = _dbContext.MaturskiIspit.Find(model.Id);
            maturski.Napomena = model.Napomena;

            _dbContext.SaveChanges();
            _dbContext.Dispose();

            return Redirect("/OdrzanaNastava/Uredi/" + model.Id);
        }

        public IActionResult ObrisiMaturski(int id)
        {
            var maturskiIspit = _dbContext.MaturskiIspit.Find(id);
            var nastavnikId = maturskiIspit.NastavnikId;

            _dbContext.Remove(maturskiIspit);
            _dbContext.RemoveRange(
                _dbContext.MaturskiIspitStavke.Where(x => x.MaturskiIspitId == id)
                );

            TempData["message-key"] = "Maturski ispit je uspješno izbrisan";
            _dbContext.SaveChanges();
            _dbContext.Dispose();

            return Redirect("/OdrzanaNastava/Odaberi/" + nastavnikId);
        }


    }
}