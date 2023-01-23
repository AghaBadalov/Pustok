using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok.Models;
using Pustok.ViewModels;
using System.Diagnostics;

namespace Pustok.Controllers
{
    public class HomeController : Controller
    {
        private readonly PustokContext _pustokContext;

        public HomeController(PustokContext pustokContext)
        {
            _pustokContext = pustokContext;
        }



        public IActionResult Index()
        {
            HomeViewModel vm = new HomeViewModel {
                Sliders = _pustokContext.Sliders.ToList(),
                Features = _pustokContext.Features.ToList(),
                FeaturedBooks = _pustokContext.Books.Include(x=>x.Author).Include(x=>x.Category).Include(x=>x.BookImages).Where(x=>x.IsFeatured).ToList(),
                NewBooks = _pustokContext.Books.Include(x => x.Author).Include(x => x.Category).Include(x => x.BookImages).Where(x => x.IsNew).ToList(),
                DiscountedBooks = _pustokContext.Books.Include(x => x.Author).Include(x => x.Category).Include(x => x.BookImages).Where(x => x.DiscountPrice>0).ToList()
            };
            
            return View(vm);
        }
        public IActionResult Detail(int id)
        {
            Book book = _pustokContext.Books.Include(x => x.Author).Include(x => x.Category).Include(x => x.BookImages).FirstOrDefault(x=>x.Id==id);
            HomeViewModel vm = new HomeViewModel
            {
                
                FeaturedBooks = _pustokContext.Books.Include(x => x.Author).Include(x => x.Category).Include(x => x.BookImages).Where(x => x.IsFeatured).ToList(),
                NewBooks = _pustokContext.Books.Include(x => x.Author).Include(x => x.Category).Include(x => x.BookImages).Where(x => x.IsNew).ToList(),
                DiscountedBooks = _pustokContext.Books.Include(x => x.Author).Include(x => x.Category).Include(x => x.BookImages).Where(x => x.DiscountPrice > 0).ToList()
            };
            ViewBag.Book1 = book;
            return View(vm);
        }
        public IActionResult SetSession(int id)
        {
            HttpContext.Session.SetString("UserId", id.ToString());

            return Content("UserId");
        }
        public IActionResult GetSession()
        {
            string userid= HttpContext.Session.GetString("UserId");

            return Content(userid);
        }
        public IActionResult RemoveSession()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index");
        }

        public IActionResult SetCookie(string name)
        {
            HttpContext.Response.Cookies.Append("Username", name);

            return Content("Added Cookie");
        }


        public IActionResult GetCookie()
        {
            string name = HttpContext.Request.Cookies["Username"];
            return Content(name); 
        }

        public IActionResult AddToBasket(int bookId)
        {
            if (!_pustokContext.Books.Any(x => x.Id == bookId)) return NotFound();
            List<BasketItemViewModel> basketItems = new List<BasketItemViewModel>();
            BasketItemViewModel basketItem=null ;
            string basketitemsstr = HttpContext.Request.Cookies["BasketItems"];

            if(basketitemsstr != null)
            {
                basketItems=JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketitemsstr);
                basketItem=basketItems.FirstOrDefault(x=>x.BookId== bookId);

                if (basketItem != null) basketItem.Count++;
                else
                {
                    basketItem = new BasketItemViewModel
                    {
                        BookId = bookId,
                        Count = 1
                    };
                    basketItems.Add(basketItem);
                }
            }
            else
            {
                basketItem = new BasketItemViewModel
                {
                    BookId = bookId,
                    Count = 1
                };
                basketItems.Add(basketItem);
            }
            basketitemsstr = JsonConvert.SerializeObject(basketItems);
            HttpContext.Response.Cookies.Append("BasketItems", basketitemsstr);
            return Ok();

        }
        public IActionResult GetBasketItems()
        {
            List<BasketItemViewModel> basketItems = new List<BasketItemViewModel>();
            string basketitemsStr = HttpContext.Request.Cookies["BasketItems"];

            if (basketitemsStr != null)
            {
                basketItems=JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketitemsStr);

            }
            return Json(basketItems);
        }

        public IActionResult CheckOut()
        {
            List<BasketItemViewModel> basketItems = new List<BasketItemViewModel>();

            List<CheckoutItemViewModel> checkoutItems = new List<CheckoutItemViewModel>();
            CheckoutItemViewModel checkoutItem=null; 
            string basketitemsStr = HttpContext.Request.Cookies["BasketItems"];
            if(basketitemsStr != null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketitemsStr);
                foreach (var item in basketItems)
                {
                    checkoutItem = new CheckoutItemViewModel
                    {
                        Book = _pustokContext.Books.FirstOrDefault(x => x.Id == item.BookId),
                        Count = item.Count
                    };
                    checkoutItems.Add(checkoutItem);
                }
            }
            return View(checkoutItems);
        }

    }
}