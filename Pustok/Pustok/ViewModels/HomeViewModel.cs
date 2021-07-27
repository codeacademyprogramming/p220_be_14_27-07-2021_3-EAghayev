﻿using Pustok.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.ViewModels
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; }
        public List<Feature> Features { get; set; }
        public List<Book> FeaturedBooks { get; set; }
        public List<Book> NewBooks { get; set; }
        public List<Book> AvailableBooks { get; set; }
        public Setting Setting { get; set; }
    }
}
