using Ispit.Data;
using Ispit.Data.EntityModels;
using Ispit.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Ispit.Web.Controllers {
    public class DetaljiAjaxController : Controller {
        private readonly MyContext _dbContext;

        public DetaljiAjaxController(MyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index(int id)
        {
            var stanjeObaveze = _dbContext.StanjeObaveze
                .Include(o => o.Obaveza)
                .Where(o => o.OznacenDogadjaj.ID == id)
                .ToList();

            var returnModel = new DetaljiAjaxVm
            {
                StanjeObaveze = stanjeObaveze
            };

            return PartialView(returnModel);
        }

        public IActionResult Uredi(int id)
        {
            var obaveza = _dbContext.StanjeObaveze
                .Include(so => so.Obaveza)
                .First(so => so.Id == id);

            return PartialView(obaveza);
        }

        public IActionResult Snimi(StanjeObaveze model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("Uredi", model);
            }

            var editObaveza = _dbContext.StanjeObaveze.Find(model.Id);
            editObaveza.IzvrsenoProcentualno = model.IzvrsenoProcentualno;

            _dbContext.SaveChanges();
            _dbContext.Dispose();

            return Redirect("/DetaljiAjax/Index/" + model.OznacenDogadjajID);
        }
    }
}