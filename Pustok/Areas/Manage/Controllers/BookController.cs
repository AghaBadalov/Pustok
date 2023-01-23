using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Helpers;
using Pustok.Models;


namespace Pustok.Areas.Manage.Controllers
{
    [Authorize]
    [Area("manage")]
    public class BookController : Controller
    {
        private readonly PustokContext _pustokContext;
        private readonly IWebHostEnvironment _env;

        public BookController(PustokContext pustokContext,IWebHostEnvironment env)
        {
            _pustokContext = pustokContext;
            _env = env;
        }

        public IActionResult Index()
        {
            return View(_pustokContext.Books.ToList());
        }

        public IActionResult Create()
        {
            ViewBag.Authors = _pustokContext.Authors.ToList();
            ViewBag.Categories = _pustokContext.Categories.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            ViewBag.Authors = _pustokContext.Authors.ToList();
            ViewBag.Categories = _pustokContext.Categories.ToList();
            if (book.PosterImage != null)
            {
                if (book.PosterImage.ContentType != "image/png" && book.PosterImage.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("PosterImage", "You can upload only jpeg , jpg and png type files ");
                    return View();
                }

                if (book.PosterImage.Length > 2097152)
                {
                    ModelState.AddModelError("PosterImage", "You can only upload files smaller than 2mb.");
                    return View();
                }
                BookImage bookImage = new BookImage
                {

                    Book = book,
                    ImageUrl = book.PosterImage.SaveFile(_env.WebRootPath, "uploads/book"),
                    Isposter=true

                };
                _pustokContext.BookImages.Add(bookImage);

            }
            if (book.HoverImage != null)
            {
                if (book.HoverImage.ContentType != "image/png" && book.HoverImage.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("HoverImage", "You can upload only jpeg , jpg and png type files ");
                    return View();
                }

                if (book.HoverImage.Length > 2097152)
                {
                    ModelState.AddModelError("HoverImage", "You can only upload files smaller than 2mb.");
                    return View();
                }
                BookImage bookImage = new BookImage
                {

                    Book = book,
                    ImageUrl = book.HoverImage.SaveFile(_env.WebRootPath, "uploads/book"),
                    Isposter = false

                };
                _pustokContext.BookImages.Add(bookImage);

            }


            if (book.ImageFiles != null)
            {
                foreach (IFormFile image in book.ImageFiles)
                {
                    if (image.ContentType != "image/png" && image.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("Imagefile", "You can upload only jpeg , jpg and png type files ");
                        return View();
                    }

                    if (image.Length > 2097152)
                    {
                        ModelState.AddModelError("Imagefile", "You can only upload files smaller than 2mb.");
                        return View();
                    }


                    BookImage bookImage = new BookImage
                    {
                        Book = book,
                        ImageUrl = image.SaveFile(_env.WebRootPath, "uploads/book"),
                        Isposter = null
                    };
                    _pustokContext.BookImages.Add(bookImage);


                }
            }



            if (!ModelState.IsValid) return View();

            _pustokContext.Books.Add(book);
            _pustokContext.SaveChanges();

            return RedirectToAction("Index");


        }

        public IActionResult Update(int id)
        {
            ViewBag.Authors = _pustokContext.Authors.ToList();
            ViewBag.Categories = _pustokContext.Categories.ToList();
            Book book = _pustokContext.Books.FirstOrDefault(x=> x.Id == id);

            if (book is null) return View("Error");

            return View(book);
        }

        [HttpPost]
        public IActionResult Update(Book book)
        {
            
            Book existBook = _pustokContext.Books.FirstOrDefault(x => x.Id == book.Id);
            if (book.PosterImage != null)
            {
                if (book.PosterImage.ContentType != "image/png" && book.PosterImage.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("PosterImage", "You can upload only jpeg , jpg and png type files ");
                    return View();
                }

                if (book.PosterImage.Length > 2097152)
                {
                    ModelState.AddModelError("PosterImage", "You can only upload files smaller than 2mb.");
                    return View();
                }
                BookImage image=_pustokContext.BookImages.Where(b=>b.BookId== book.Id).FirstOrDefault(x=>x.Isposter==true);
                _pustokContext.BookImages.Remove(image);

                string path = Path.Combine(_env.WebRootPath, "uploads/book", image.ImageUrl);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }




                BookImage bookImage = new BookImage
                {

                    BookId = book.Id,
                    ImageUrl = book.PosterImage.SaveFile(_env.WebRootPath, "uploads/book"),
                    Isposter = true

                };
                _pustokContext.BookImages.Add(bookImage);
                _pustokContext.SaveChanges();

            }
            if (book.HoverImage != null)
            {
                if (book.HoverImage.ContentType != "image/png" && book.HoverImage.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("HoverImage", "You can upload only jpeg , jpg and png type files ");
                    return View();
                }

                if (book.HoverImage.Length > 2097152)
                {
                    ModelState.AddModelError("HoverImage", "You can only upload files smaller than 2mb.");
                    return View();
                }
                BookImage image = _pustokContext.BookImages.Where(b => b.BookId == book.Id).FirstOrDefault(x => x.Isposter == false);
                _pustokContext.BookImages.Remove(image);

                string path = Path.Combine(_env.WebRootPath, "uploads/book", image.ImageUrl);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                BookImage bookImage = new BookImage
                {

                    Book = book,
                    ImageUrl = book.HoverImage.SaveFile(_env.WebRootPath, "uploads/book"),
                    Isposter = false

                };
                _pustokContext.BookImages.Add(bookImage);
                _pustokContext.SaveChanges();
            }


            if (book.ImageFiles != null)
            {
                foreach (IFormFile image in book.ImageFiles)
                {
                    if (image.ContentType != "image/png" && image.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("Imagefile", "You can upload only jpeg , jpg and png type files ");
                        return View();
                    }

                    if (image.Length > 2097152)
                    {
                        ModelState.AddModelError("Imagefile", "You can only upload files smaller than 2mb.");
                        return View();
                    }


                    BookImage bookImage = new BookImage
                    {
                        Book = book,
                        ImageUrl = image.SaveFile(_env.WebRootPath, "uploads/book"),
                        Isposter = null
                    };
                    _pustokContext.BookImages.Add(bookImage);


                }
            }
               
            if (existBook is null) return View("Error");




            existBook.SalePrice = book.SalePrice;
            existBook.AuthorId = book.AuthorId;
            existBook.Name = book.Name;
            existBook.DiscountPrice = book.DiscountPrice;
            existBook.CategoryId = book.CategoryId;
            existBook.Costprice = book.Costprice;
            existBook.IsAvailable = book.IsAvailable;
            existBook.IsFeatured = book.IsFeatured;
            existBook.IsNew = book.IsNew;
            existBook.Desc = book.Desc;
            existBook.Code = book.Code;
            

            _pustokContext.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            Book book = _pustokContext.Books.Include(x=>x.BookImages).FirstOrDefault(x => x.Id == id);
            //if(book is null) return View("Error");
            foreach (var item in book.BookImages)
            {
                string path =  Path.Combine(_env.WebRootPath, "uploads/book", item.ImageUrl);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            
            _pustokContext.Books.Remove(book);
            _pustokContext.SaveChanges();


            return Ok();
        }
    }
}
