using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MenBytes.Models.MenBytesModels;

namespace MenBytes.Models.ViewModels
{
    public class clientHomeViewModel
    {
        public  List<Category> CategoriesList { get; set; }

        public List<Blog> BlogsList { get; set; }
    }
}