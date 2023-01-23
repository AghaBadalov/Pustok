using Microsoft.AspNetCore.Mvc;
using Pustok.Models;
using System;

namespace Pustok.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CategorieController : Controller
    {
        private readonly PustokContext _pustokContext;

        public CategorieController(PustokContext pustokContext)
        {
            _pustokContext = pustokContext;
        }
        public IActionResult Index()
        {
            ViewBag.Categories = _pustokContext.Categories.ToList();
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _pustokContext.Categories.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            
            if (!ModelState.IsValid) return View();

            _pustokContext.Categories.Add(category);
            _pustokContext.SaveChanges();

            return RedirectToAction("Index");   
        }

        public IActionResult Delete(int id)
        {
            Category category = _pustokContext.Categories.Find(id);
            if (category is null) return View("Error");

            _pustokContext.Categories.Remove(category);
            _pustokContext.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Update(int id)
        {
            Category category = _pustokContext.Categories.Find(id);
            if (category is null) return View("Error");


            return View(category);
        }

        [HttpPost]
        public IActionResult Update(Category Cat)
        {

            Category existCat = _pustokContext.Categories.Find(Cat.Id);

            if (existCat == null) return NotFound();

            existCat.Name = Cat.Name;
            _pustokContext.SaveChanges();

            return RedirectToAction("Index");

            
        }


    }
}
