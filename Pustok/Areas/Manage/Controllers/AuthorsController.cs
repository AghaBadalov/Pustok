using Microsoft.AspNetCore.Mvc;
using Pustok.Models;

namespace Pustok.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AuthorsController : Controller
    {
        private readonly PustokContext _pustokContext;

        public AuthorsController(PustokContext pustokContext)
        {
            _pustokContext = pustokContext;
        }

        public IActionResult Index()
        {
            ViewBag.Authors = _pustokContext.Authors.ToList();
            return View();
        }
        public IActionResult Create()
        {
            ViewBag.Authors = _pustokContext.Authors.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Author author)
        {
            //Author Author1 = PustokContext.Authors.Find(Author.Id);
            //if (Author1 is null) return View();
            if (!ModelState.IsValid) return View();

            _pustokContext.Authors.Add(author);
            _pustokContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Author Author = _pustokContext.Authors.FirstOrDefault(x => x.Id == id);
            if (Author is null) return View("Error");

            _pustokContext.Authors.Remove(Author);
            _pustokContext.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Update(int id)
        {
            Author Author = _pustokContext.Authors.Find(id);
            if (Author is null) return View("Error");

            return View(Author);
        }

        [HttpPost]
        public IActionResult Update(Author newAut)
        {
            Author exAuthor = _pustokContext.Authors.Find(newAut.Id);
            if (exAuthor is null) return View("Error");

            exAuthor.Fullname = newAut.Fullname;
            _pustokContext.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
