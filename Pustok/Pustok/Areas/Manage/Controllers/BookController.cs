using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Helpers;
using Pustok.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Areas.Manage.Controllers
{
    [Area("manage")]
    public class BookController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BookController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page=1)
        {
            ViewBag.SelectedPage = page;
            ViewBag.TotalPage = Math.Ceiling(_context.Books.Count() / 4d);

            List<Book> model = _context.Books
                .Include(x=>x.Author).Include(x=>x.Genre)
                .Skip((page - 1) * 4).Take(4).ToList();

            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Publishers = _context.Publishers.ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Publishers = _context.Publishers.ToList();

            if (!ModelState.IsValid) return View();

            #region Checking

            if (!_context.Genres.Any(x => x.Id == book.GenreId))
            {
                ModelState.AddModelError("GenreId", "Genre mövcud deyil!");
                return View();
            }

            if (!_context.Authors.Any(x => x.Id == book.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Author mövcud deyil!");
                return View();
            }

            if (!_context.Publishers.Any(x => x.Id == book.PublisherId))
            {
                ModelState.AddModelError("PublisherId", "Publisher mövcud deyil!");
                return View();
            }

            if (_context.Books.Any(x => x.Code == book.Code))
            {
                ModelState.AddModelError("Code", "Code tekrar ola bilmez!");
                return View();
            }

            if(book.PosterImage == null)
            {
                ModelState.AddModelError("PosterImage", "Deyer mecburidir!");
                return View();
            }

            if (book.HoverPosterImage == null)
            {
                ModelState.AddModelError("HoverPosterImage", "Deyer mecburidir!");
                return View();
            }

            if (book.PosterImage.ContentType != "image/jpeg" && book.PosterImage.ContentType != "image/png")
            {
                ModelState.AddModelError("PosterImage", "Fayl   .jpg ve ya   .png ola biler!");
                return View();
            }

            if (book.PosterImage.Length > 2097152)
            {
                ModelState.AddModelError("PosterImage", "Fayl olcusu 2mb-dan boyuk ola bilmez!");
                return View();
            }

            if (book.HoverPosterImage.ContentType != "image/jpeg" && book.HoverPosterImage.ContentType != "image/png")
            {
                ModelState.AddModelError("HoverPosterImage", "Fayl   .jpg ve ya   .png ola biler!");
                return View();
            }

            if (book.HoverPosterImage.Length > 2097152)
            {
                ModelState.AddModelError("HoverPosterImage", "Fayl olcusu 2mb-dan boyuk ola bilmez!");
                return View();
            }

            if (book.Images != null)
            {
                foreach (var item in book.Images)
                {
                    if (item.ContentType != "image/jpeg" && item.ContentType != "image/png")
                    {
                        ModelState.AddModelError("Images", "Fayl   .jpg ve ya   .png ola biler!");
                        return View();
                    }

                    if (item.Length > 2097152)
                    {
                        ModelState.AddModelError("Images", "Fayl olcusu 2mb-dan boyuk ola bilmez!");
                        return View();
                    }

                }
            }
            #endregion

            book.BookImages = new List<BookImage>();

            BookImage posterImage = new BookImage
            {
                PosterStatus = true,
                Image = FileManager.Save(_env.WebRootPath, "uploads/books", book.PosterImage)
            };
            book.BookImages.Add(posterImage);


            BookImage hoverPosterImage = new BookImage
            {
                PosterStatus = false,
                Image = FileManager.Save(_env.WebRootPath, "uploads/books", book.HoverPosterImage)
            };
            book.BookImages.Add(hoverPosterImage);

            foreach (var item in book.Images)
            {
                BookImage bookImage = new BookImage
                {
                    PosterStatus = null,
                    Image = FileManager.Save(_env.WebRootPath, "uploads/books", item)
                };
                book.BookImages.Add(bookImage);
            }

            _context.Books.Add(book);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Book book = _context.Books.Include(x=>x.BookImages).FirstOrDefault(x => x.Id == id);

            if (book == null) return NotFound();

            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Publishers = _context.Publishers.ToList();

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book book)
        {
            #region Checking
            if (!ModelState.IsValid)
            {
                return View();
            }

            Book existBook = _context.Books.Include(x=>x.BookImages).FirstOrDefault(x => x.Id == book.Id);

            if (existBook == null) return NotFound();

            if (!_context.Genres.Any(x => x.Id == book.GenreId))
            {
                ModelState.AddModelError("GenreId", "Genre mövcud deyil!");
                return View();
            }

            if (!_context.Authors.Any(x => x.Id == book.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Author mövcud deyil!");
                return View();
            }

            if (!_context.Publishers.Any(x => x.Id == book.PublisherId))
            {
                ModelState.AddModelError("PublisherId", "Publisher mövcud deyil!");
                return View();
            }

            if(book.PosterImage != null)
            {
                if (book.PosterImage.ContentType != "image/jpeg" && book.PosterImage.ContentType != "image/png")
                {
                    ModelState.AddModelError("PosterImage", "Fayl   .jpg ve ya   .png ola biler!");
                    return View();
                }

                if (book.PosterImage.Length > 2097152)
                {
                    ModelState.AddModelError("PosterImage", "Fayl olcusu 2mb-dan boyuk ola bilmez!");
                    return View();
                }
            }

            if (book.HoverPosterImage != null)
            {
                if (book.HoverPosterImage.ContentType != "image/jpeg" && book.HoverPosterImage.ContentType != "image/png")
                {
                    ModelState.AddModelError("HoverPosterImage", "Fayl   .jpg ve ya   .png ola biler!");
                    return View();
                }

                if (book.HoverPosterImage.Length > 2097152)
                {
                    ModelState.AddModelError("HoverPosterImage", "Fayl olcusu 2mb-dan boyuk ola bilmez!");
                    return View();
                }
            }

            if (book.Images != null)
            {
                foreach (var item in book.Images)
                {
                    if (item.ContentType != "image/jpeg" && item.ContentType != "image/png")
                    {
                        ModelState.AddModelError("Images", "Fayl   .jpg ve ya   .png ola biler!");
                        return View();
                    }

                    if (item.Length > 2097152)
                    {
                        ModelState.AddModelError("Images", "Fayl olcusu 2mb-dan boyuk ola bilmez!");
                        return View();
                    }

                }
            }
            #endregion

            existBook.Name = book.Name;
            existBook.Price = book.Price;
            existBook.DiscountedPrice = book.DiscountedPrice;
            existBook.Code = book.Code;
            existBook.CostPrice = book.CostPrice;
            existBook.Desc = book.Desc;
            existBook.GenreId = book.GenreId;
            existBook.AuthorId = book.AuthorId;
            existBook.PublisherId = book.PublisherId;
            existBook.IsAvailable = book.IsAvailable;
            existBook.IsNew = book.IsNew;
            existBook.IsFeatured = book.IsFeatured;


            if(book.PosterImage != null)
            {
                string filename = FileManager.Save(_env.WebRootPath, "uploads/books", book.PosterImage);

                BookImage oldPoster = existBook.BookImages.FirstOrDefault(x => x.PosterStatus == true);

                if(oldPoster != null)
                {
                    FileManager.Delete(_env.WebRootPath, "uploads/books", oldPoster.Image);
                }

                oldPoster.Image = filename;
            }

            if (book.HoverPosterImage != null)
            {
                string filename = FileManager.Save(_env.WebRootPath, "uploads/books", book.HoverPosterImage);

                BookImage oldPoster = existBook.BookImages.FirstOrDefault(x => x.PosterStatus == false);

                if (oldPoster != null)
                {
                    FileManager.Delete(_env.WebRootPath, "uploads/books", oldPoster.Image);
                }

                oldPoster.Image = filename;
            }



            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
