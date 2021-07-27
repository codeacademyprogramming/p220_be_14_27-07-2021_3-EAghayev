using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pustok.DAL;
using Pustok.Models;
using Pustok.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeViewModel homeVM = new HomeViewModel
            {
                Sliders = _context.Sliders.OrderBy(x => x.Order).ToList(),
                Features = _context.Features.OrderBy(x => x.Order).ToList(),
                FeaturedBooks = _context.Books.Include(x => x.Author).Include(x => x.BookImages).Where(x => x.IsFeatured).Take(20).ToList(),
                AvailableBooks = _context.Books.Include(x => x.Author).Include(x => x.BookImages).Where(x => x.IsAvailable).Take(20).ToList(),
                NewBooks = _context.Books.Include(x => x.Author).Include(x => x.BookImages).Where(x => x.IsNew).Take(20).ToList(),
                Setting = _context.Settings.FirstOrDefault()
            };

            //var sliders = _context.Sliders.Where(x => x.Order > 5).Skip(10).Take(5).ToList();
            //var slide = _context.Sliders.FirstOrDefault(x => x.Order == 5);
            //var isExist = _context.Sliders.Any(x => x.Order == 5);
            //var groupedSliders = _context.Sliders.GroupBy(x => x.Order);

            return View(homeVM);
        }
    }
}
