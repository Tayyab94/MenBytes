using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MenBytes.Models.MenBytesModels;

namespace MenBytes.Models.ViewModels
{
    public class PhotoGalleryViewModel
    {
        


        public int MaximumPrice { get; set; }

        public string searchITem { get; set; }
        //public List<showAllProducts> ProductList { get; set; }

        public List<Product> ProductList { get; set; }

        public int? SortBy { get; set; }

        public int? categoryId { get; set; }
        public List<Category> FeaturedCatagories { get; set; }

        public int? companyId { get; set; }
        public List<Companies> FeaturedCompanieses { get; set; }

        public Pager pager { get; set; }
    }
}