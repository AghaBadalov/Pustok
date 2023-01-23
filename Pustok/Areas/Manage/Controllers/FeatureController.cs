using Microsoft.AspNetCore.Mvc;
using Pustok.Models;

namespace Pustok.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class FeatureController : Controller
    {
        private readonly PustokContext _pustokContext;

        public FeatureController(PustokContext pustokContext)
        {
            _pustokContext = pustokContext;
        }

        
        public IActionResult Index()
        {
            ViewBag.Features = _pustokContext.Features.ToList();
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.Features = _pustokContext.Features.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Feature Feature)
        {
            if (!ModelState.IsValid) return View();

            _pustokContext.Features.Add(Feature);
            _pustokContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Feature Feature = _pustokContext.Features.FirstOrDefault(x => x.Id == id);
            if (Feature is null) return View("Error");

            _pustokContext.Features.Remove(Feature);
            _pustokContext.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Update(int id)
        {
            Feature Feature = _pustokContext.Features.FirstOrDefault(x => x.Id == id);
            if (Feature is null) return View("Error");


            return View(Feature);
        }

        [HttpPost]
        public IActionResult Update(Feature newFeature)
        {

            Feature existFeature = _pustokContext.Features.Find(newFeature.Id);

            if (existFeature == null) return NotFound();

            existFeature.Title1 = newFeature.Title1;
            existFeature.Title2 = newFeature.Title2;
            existFeature.Icon = newFeature.Icon;

            _pustokContext.SaveChanges();

            return RedirectToAction("Index");

        }
    }
}
