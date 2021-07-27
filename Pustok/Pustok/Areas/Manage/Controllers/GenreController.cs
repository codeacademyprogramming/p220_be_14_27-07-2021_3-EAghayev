using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Areas.Manage.Controllers
{
    [Area("manage")]
    public class GenreController : Controller
    {
        private readonly AppDbContext _context;

        public GenreController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Genre> genres = _context.Genres.Include(x=>x.Books).ToList();

            return View(genres);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Genre genre)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            //Genre genre = new Genre
            //{
            //    Name = name
            //};

            _context.Genres.Add(genre);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Genre genre = _context.Genres.FirstOrDefault(x => x.Id == id);

            if (genre == null) return NotFound();

            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Genre genre)
        {
            Genre existGenre = _context.Genres.FirstOrDefault(x => x.Id == genre.Id);

            if (existGenre == null) return NotFound();

            existGenre.Name = genre.Name;
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Genre existGenre = _context.Genres.FirstOrDefault(x => x.Id == id);
            if (existGenre == null)
            {
                return Json(new { status = 404 });
            }

            _context.Genres.Remove(existGenre);
            _context.SaveChanges();

            return Json(new { status = 200 });
        }

    }
}
