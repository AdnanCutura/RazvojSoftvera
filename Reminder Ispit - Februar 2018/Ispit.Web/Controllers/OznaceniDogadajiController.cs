using eUniverzitet.Web.Helper;
using Ispit.Data;
using Ispit.Data.EntityModels;
using Ispit.Web.Helper;
using Ispit.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Dogadjaj = Ispit.Data.EntityModels.Dogadjaj;

namespace Ispit.Web.Controllers {
    [Autorizacija]
    public class OznaceniDogadajiController : Controller {
        private readonly MyContext _dbContext;

        public OznaceniDogadajiController(MyContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public IActionResult Index()
        {

            var sviDogadjaji = _dbContext.Dogadjaj
                .Include(sd => sd.Nastavnik)
                .ToList();

            var nalog = ControllerContext.HttpContext.GetLogiraniKorisnik();


            var student = _dbContext.Student.FirstOrDefault(s => s.KorisnickiNalogId == nalog.Id);
            var razrednik = _dbContext.Nastavnik.FirstOrDefault(s => s.KorisnickiNalogId == nalog.Id);
            var returnModel = new DogadjajiVm();
            if (student != null)
            {
                var finalOznaceni = new List<Dogadjaj>();
                var finalNeoznaceni = new List<Dogadjaj>();

                var oznaceniTabela = _dbContext.OznacenDogadjaj.ToList();

                foreach (var item in oznaceniTabela)
                {
                    var pronadjeniDogadjaj = _dbContext.Dogadjaj.Find(item.DogadjajID);
                    if (student.ID == item.StudentID)
                        finalOznaceni.Add(pronadjeniDogadjaj);
                }

                foreach (var dogadjaj in sviDogadjaji)
                {
                    var flag = 0;
                    foreach (var oznacen in finalOznaceni)
                        if (dogadjaj.ID == oznacen.ID)
                            flag = 1;

                    if (flag == 0)
                        finalNeoznaceni.Add(dogadjaj);

                }

                var neoznaceniDogadjaji = finalNeoznaceni
                    .Select(d => new DogadjajiVm.NeoznacenDogadjaj
                    {
                        ID = d.ID,
                        Nastavnik = d.Nastavnik.ImePrezime,
                        DatumOdrzavanja = d.DatumOdrzavanja,
                        Opis = d.Opis,
                        BrojObaveza = _dbContext.Obaveza.Count(o => o.DogadjajID == d.ID)
                    })
                    .ToList();


                var oznaceniDogadjaji = finalOznaceni
                    .Select(d => new DogadjajiVm.OznaceniDogadjaj
                    {
                        ID = d.ID,
                        Nastavnik = d.Nastavnik.ImePrezime,
                        DatumOdrzavanja = d.DatumOdrzavanja,
                        Opis = d.Opis,
                        RealizovanoObaveza = (int)_dbContext.StanjeObaveze.Where(so => so.OznacenDogadjajID == d.ID).Sum(so => so.IzvrsenoProcentualno) / _dbContext.Obaveza.Count(o => o.DogadjajID == d.ID)
                    })
                    .ToList();

                var statusiObaveza = _dbContext.StanjeObaveze
                    .Include(so => so.Obaveza)
                    .ToList();

                var notifikacije = _dbContext.PoslataNotifikacija.ToList();

                returnModel = new DogadjajiVm
                {
                    NeoznaceniDogadjaji = neoznaceniDogadjaji,
                    OznaceniDogadjaji = oznaceniDogadjaji,
                    StanjaObaveza = statusiObaveza,
                    Notifikacije = notifikacije
                };
            }
            else
            {
                var dogadjaji = _dbContext.Dogadjaj.Where(d => d.NastavnikID == razrednik.ID).ToList();
                returnModel = new DogadjajiVm
                {
                    NastavnikDogadjaji = dogadjaji
                };
            }
            return View(returnModel);
        }

        public IActionResult Dodaj(int id)
        {

            var dogadjaj = _dbContext.Dogadjaj.Find(id);
            var nalog = ControllerContext.HttpContext.GetLogiraniKorisnik();

            var student = _dbContext.Student.First(s => s.KorisnickiNalogId == nalog.Id);

            var oznacenDogadjaj = new OznacenDogadjaj
            {
                StudentID = student.ID,
                DogadjajID = dogadjaj.ID,
                DatumDodavanja = DateTime.Now
            };
            _dbContext.OznacenDogadjaj.Add(oznacenDogadjaj);
            _dbContext.SaveChanges();

            var obaveze = _dbContext.Obaveza.Where(o => o.DogadjajID == dogadjaj.ID).ToList();
            foreach (var obaveza in obaveze)
            {
                var stanjeObaveze = new StanjeObaveze
                {
                    OznacenDogadjajID = oznacenDogadjaj.ID,
                    IsZavrseno = false,
                    ObavezaID = obaveza.ID,
                    NotifikacijaDanaPrije = obaveza.NotifikacijaDanaPrijeDefault,
                    NotifikacijeRekurizivno = obaveza.NotifikacijeRekurizivnoDefault,
                    IzvrsenoProcentualno = 0
                };
                _dbContext.StanjeObaveze.Add(stanjeObaveze);
            }


            _dbContext.SaveChanges();
            _dbContext.Dispose();

            return RedirectToAction("Index");
        }

        public IActionResult Detalji(int id)
        {
            var dogadjaj = _dbContext.OznacenDogadjaj
                .Include(d => d.Dogadjaj)
                .ThenInclude(i => i.Nastavnik)
                .First(d => d.ID == id);

            return View(dogadjaj);
        }

        public IActionResult ProcitanaNotifikacija(int id)
        {
            var poslataNofit = new PoslataNotifikacija
            {
                StanjeObavezeID = id,
                DatumSlanja = DateTime.Now
            };

            _dbContext.PoslataNotifikacija.Add(poslataNofit);
            _dbContext.SaveChanges();
            //_dbContext.Dispose();

            return RedirectToAction("Index");
        }
    }
}
