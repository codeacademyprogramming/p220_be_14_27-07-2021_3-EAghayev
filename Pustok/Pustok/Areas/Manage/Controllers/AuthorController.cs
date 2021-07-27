using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    public class AuthorController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AuthorController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page=1)
        {
            ViewBag.SelectedPage = page;
            ViewBag.TotalPage = Math.Ceiling(_context.Authors.Count() / 2d);

            var model = _context.Authors.Include(x=>x.Books)
                .Skip((page-1)*2).Take(2)
                .ToList();


            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Author author)
        {
            if (!ModelState.IsValid) return View();

            if(author.ImageFile != null)
            {
                if(author.ImageFile.ContentType != "image/jpeg" && author.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "Fayl   .jpg ve ya   .png ola biler!");
                    return View();
                }

                if(author.ImageFile.Length> 2097152)
                {
                    ModelState.AddModelError("ImageFile", "Fayl olcusu 2mb-dan boyuk ola bilmez!");
                    return View();
                }

                author.Image = FileManager.Save(_env.WebRootPath, "uploads/authors", author.ImageFile);
            }

            _context.Authors.Add(author);
            _context.SaveChanges();

            return RedirectToAction("index");
        }


        public IActionResult Edit(int id)
        {
            Author author = _context.Authors.FirstOrDefault(x => x.Id == id);

            if (author == null) return NotFound();

            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Author author)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Author existAuthor = _context.Authors.FirstOrDefault(x => x.Id == author.Id);

            if (existAuthor == null) return NotFound();

            existAuthor.FullName = author.FullName;
            existAuthor.Desc = author.Desc;

            if(author.ImageFile != null)
            {
                if (author.ImageFile.ContentType != "image/jpeg" && author.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "Fayl   .jpg ve ya   .png ola biler!");
                    return View();
                }

                if (author.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "Fayl olcusu 2mb-dan boyuk ola bilmez!");
                    return View();
                }


                string newFileName = FileManager.Save(_env.WebRootPath, "uploads/authors", author.ImageFile);

                if (!string.IsNullOrWhiteSpace(existAuthor.Image))
                {
                    string oldFilePath = Path.Combine(_env.WebRootPath, "uploads/authors", existAuthor.Image);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
               

                existAuthor.Image = newFileName;
            }
            else if (string.IsNullOrWhiteSpace(author.Image) && !string.IsNullOrWhiteSpace(existAuthor.Image))
            {
                FileManager.Delete(_env.WebRootPath, "uploads/authors", existAuthor.Image);

                existAuthor.Image = null;
            }


            _context.SaveChanges();

            return RedirectToAction("index");
        }


    }
}
