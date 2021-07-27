using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Models
{
    public class Setting
    {
        public int Id { get; set; }
        [StringLength(maximumLength:25)]
        public string SupportPhone { get; set; }
        [StringLength(maximumLength: 25)]
        public string ContactPhone { get; set; }
        [StringLength(maximumLength: 250)]
        public string Address { get; set; }
        [StringLength(maximumLength: 100)]
        public string HeaderLogo { get; set; }
        [StringLength(maximumLength: 100)]
        public string FooterLogo { get; set; }
        [StringLength(maximumLength: 100)]
        public string Email { get; set; }
        [StringLength(maximumLength: 150)]
        public string PromotionTitle { get; set; }
        [StringLength(maximumLength: 250)]
        public string PromotionSubTitle { get; set; }
        [StringLength(maximumLength: 50)]
        public string PromotionBtnText { get; set; }
        [StringLength(maximumLength: 250)]
        public string PromotionBtnUrl { get; set; }
        [StringLength(maximumLength: 100)]
        public string PromotionBgImage { get; set; }
    }
}
