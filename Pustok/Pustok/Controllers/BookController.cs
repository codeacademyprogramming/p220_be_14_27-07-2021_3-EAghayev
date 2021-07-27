using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok.DAL;
using Pustok.Models;
using Pustok.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Controllers
{
    public class BookController : Controller
    {
        private readonly AppDbContext _context;

        public BookController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? genreId, int page=1)
        {
            var query = _context.Books.AsQueryable();

            if(genreId != null)
            {
                query = query.Where(x => x.GenreId == genreId);
            }

            var totalPage = query.Count() / 3d;
            ViewBag.TotalPage = Math.Ceiling(totalPage);
            ViewBag.GenreId = genreId;

            ViewBag.SelectedPage = page;
            List<Book> books = query.Include(x=>x.Author).Include(x=>x.BookImages).Skip((page-1)*3).Take(3).ToList();
            return View(books);
        }


        public IActionResult Detail(int id)
        {
            Book book = _context.Books
                .Include(x=>x.Author)
                .Include(x=>x.Genre)
                .Include(x=>x.BookImages)
                .Include(x=>x.BookTags).ThenInclude(x=>x.Tag)
                .FirstOrDefault(x => x.Id == id);


            if (book == null) return NotFound();

            List<Book> relatedBooks = _context.Books
                .Include(x=>x.Author)
                .Include(x=>x.BookImages)
                .Where(x => x.GenreId == book.GenreId && x.Id!=book.Id).ToList();
            ViewBag.RelatedBooks = relatedBooks;

            return View(book);
        }

        public IActionResult GetDetailedBook(int id)
        {
            Book book = _context.Books.Include(x => x.Author).Include(x => x.BookImages).FirstOrDefault(x => x.Id == id);

            return PartialView("_BookDetail", book);
            //return Json(book);
        }

        public IActionResult SetSession(int id)
        {
            Book book = _context.Books.FirstOrDefault(x => x.Id == id);

            var bookStr = JsonConvert.SerializeObject(book);

            HttpContext.Session.SetString("Book", bookStr);

            return RedirectToAction("index", "home");
        }

        public IActionResult ShowSession()
        {
            var sessionValue = HttpContext.Session.GetString("Book");
            Book book = JsonConvert.DeserializeObject<Book>(sessionValue);

            //return Json(book);

            return Content(sessionValue);
        }

        public IActionResult SetCookie(int id)
        {
            Book book = _context.Books.FirstOrDefault(x => x.Id == id);

            var cookieBooks = HttpContext.Request.Cookies["BookList"];
           
            if(cookieBooks == null)
            {
                List<Book> books = new List<Book>();
                books.Add(book);
                var bookListStr = JsonConvert.SerializeObject(books);
                HttpContext.Response.Cookies.Append("BookList", bookListStr);
            }
            else
            {
                List<Book> books = JsonConvert.DeserializeObject<List<Book>>(cookieBooks);
                books.Add(book);
                var bookListStr = JsonConvert.SerializeObject(books);
                HttpContext.Response.Cookies.Append("BookList", bookListStr);
            }

            return RedirectToAction("index", "home");
        }

        public IActionResult ShowCookie()
        {
            var bookStr = HttpContext.Request.Cookies["BookList"];

            return Content(bookStr);
        }

        public IActionResult DeleteCookie(string key)
        {
            HttpContext.Response.Cookies.Delete(key);
            return RedirectToAction("index", "home");
        }

        public IActionResult AddToBasket(int id)
        {
            Book book = _context.Books.FirstOrDefault(x => x.Id == id);


            var basket = HttpContext.Request.Cookies["Basket"];
            List<BasketCookieItemViewModel> basketItems;

            if (basket == null)
            {
                basketItems = new List<BasketCookieItemViewModel>();
                basketItems.Add(new BasketCookieItemViewModel
                {
                    Id = book.Id,
                    Count = 1
                });

                var basketStr = JsonConvert.SerializeObject(basketItems);
                HttpContext.Response.Cookies.Append("Basket", basketStr);
            }
            else
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketCookieItemViewModel>>(basket);

                BasketCookieItemViewModel basketItem = basketItems.FirstOrDefault(x => x.Id == book.Id);

                if(basketItem == null)
                {
                    basketItem = new BasketCookieItemViewModel
                    {
                        Id = book.Id,
                        Count = 1
                    };
                    basketItems.Add(basketItem);
                }
                else
                {
                    basketItem.Count++;
                }
                
                var basketStr = JsonConvert.SerializeObject(basketItems);
                HttpContext.Response.Cookies.Append("Basket", basketStr);
            }

            BasketViewModel basketData = new BasketViewModel
            {
                BasketItems = new List<BasketItemViewModel>(),
                TotalPrice = 0
            };

            foreach (var item in basketItems)
            {
                Book existBook = _context.Books.Include(x=>x.BookImages).FirstOrDefault(x => x.Id == item.Id);

                if (existBook != null)
                {
                    BasketItemViewModel basketItemVM = new BasketItemViewModel
                    {
                        Book = existBook,
                        Count = item.Count
                    };

                    basketData.TotalPrice += basketItemVM.Book.DiscountedPrice * item.Count;
                    basketData.BasketItems.Add(basketItemVM);
                    basketData.Count++;
                }

                //var json = JsonConvert.SerializeObject(basketData,settings: new JsonSerializerSettings
                //{
                //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                //});
            }

            return Json(basketData);

        }

        public IActionResult ShowBasket()
        {
            var basket = HttpContext.Request.Cookies["Basket"];

            return Content(basket);
        }

    }
}
