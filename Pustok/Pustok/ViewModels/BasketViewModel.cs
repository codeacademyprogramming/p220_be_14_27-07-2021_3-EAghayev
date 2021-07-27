using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.ViewModels
{
    public class BasketViewModel
    {
        public List<BasketItemViewModel> BasketItems { get; set; }
        public int Count { get; set; }
        public double TotalPrice { get; set; }
    }
}
