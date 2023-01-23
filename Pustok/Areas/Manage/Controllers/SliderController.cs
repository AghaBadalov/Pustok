using Microsoft.AspNetCore.Mvc;
using Pustok.Helpers;
using Pustok.Models;
using System.IO;
using System.Reflection;

namespace Pustok.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        private readonly PustokContext _pustokContext;
        private readonly IWebHostEnvironment _env;

        public SliderController(PustokContext pustokContext,IWebHostEnvironment env)
        {
            _pustokContext = pustokContext;

            _env = env;
        }

        public IActionResult Index()
        {
            ViewBag.Sliders = _pustokContext.Sliders.ToList();
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.Sliders = _pustokContext.Sliders.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Slider Slider)
        {
            if (Slider.Imagefile.ContentType != "image/png" && Slider.Imagefile.ContentType != "image/jpeg")
            {
                ModelState.AddModelError("Imagefile", "You can upload only jpeg , jpg and png type files ");
                return View();
            }

            if (Slider.Imagefile.Length> 2097152)
            {
                ModelState.AddModelError("Imagefile", "You can only upload files smaller than 2mb.");
                return View();
            }
            

            //string filename = Slider.Imagefile.FileName;


            //if (filename.Length > 64)
            //{
            //    filename=filename.Substring(filename.Length - 64,64);
            //}
            //filename=Guid.NewGuid().ToString()+filename;

            //string path = "C:\\Users\\lenovo\\OneDrive\\Desktop\\pustoooooooooooooooook\\Pustok\\wwwroot\\uploads\\slider\\"+filename;
            //using (FileStream filestream = new FileStream(path, FileMode.Create)) 
            //{
            //    Slider.Imagefile.CopyTo(filestream);
            //}
            Slider.Img = Slider.Imagefile.SaveFile(_env.WebRootPath,"uploads/slider");

            if (!ModelState.IsValid) return View();
            

            _pustokContext.Sliders.Add(Slider);
            _pustokContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Slider Slider = _pustokContext.Sliders.FirstOrDefault(x => x.Id == id);
            if (Slider is null) return View("Error");
            string path = Path.Combine(_env.WebRootPath, "uploads/slider", Slider.Img);
            if (System.IO.File.Exists(path))
            {

                System.IO.File.Delete(path);
            }
            _pustokContext.Sliders.Remove(Slider);

            _pustokContext.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Update(int id)
        {
            Slider Slider = _pustokContext.Sliders.Find(id);
            if (Slider is null) return View("Error");


            return View(Slider);
        }

        [HttpPost]
        public IActionResult Update(Slider slider)
        {
            Slider exstSlider = _pustokContext.Sliders.Find(slider.Id);
            if(slider.Img != null)
            {

                if (slider.Imagefile.ContentType != "image/png" && slider.Imagefile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("Imagefile", "You can upload only jpeg , jpg and png type files ");
                    return View();
                }
                if (slider.Imagefile.Length > 2097152)
                {
                    ModelState.AddModelError("Imagefile", "You can only upload files smaller than 2mb.");
                    return View();
                }
                
                string path = Path.Combine(_env.WebRootPath, "uploads/slider", exstSlider.Img);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                
            }

             exstSlider.Img = slider.Imagefile.SaveFile(_env.WebRootPath, "uploads/slider");

            if (exstSlider == null) return NotFound();

            exstSlider.Title = slider.Title;
            exstSlider.RedirectUrl = slider.RedirectUrl;
            exstSlider.Desc = slider.Desc;
            exstSlider.Price = slider.Price;
            exstSlider.AuthorName = slider.AuthorName;
            
            exstSlider.Order = slider.Order;
            _pustokContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
