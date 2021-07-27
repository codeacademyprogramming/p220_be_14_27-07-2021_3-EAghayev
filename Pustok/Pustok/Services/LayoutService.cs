using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok.DAL;
using Pustok.Models;
using Pustok.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LayoutService(AppDbContext  context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public List<Genre> GetGenres()
        {
            return _context.Genres.ToList();
        }

        public Setting GetSetting()
        {
            return _context.Settings.FirstOrDefault();
        }

        public BasketViewModel GetBasket()
        {
            var basket = _httpContextAccessor.HttpContext.Request.Cookies["Basket"];

            BasketViewModel basketData = new BasketViewModel
            {
                BasketItems = new List<BasketItemViewModel>(),
                TotalPrice = 0
            };

            if(basket != null)
            {
                List<BasketCookieItemViewModel> cookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemViewModel>>(basket);

                foreach (var item in cookieItems)
                {
                   Book book = _context.Books.Include(x=>x.BookImages).FirstOrDefault(x => x.Id == item.Id);

                    if (book != null)
                    {
                        BasketItemViewModel basketItemVM = new BasketItemViewModel
                        {
                            Book = book,
                            Count = item.Count
                        };

                        basketData.TotalPrice += basketItemVM.Book.DiscountedPrice * item.Count;
                        basketData.BasketItems.Add(basketItemVM);
                        basketData.Count++;
                    }

                }
            }
          

            return basketData;
        }
    }
}
