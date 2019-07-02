using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MenBytes.Models.MenBytesModels;

namespace MenBytes.Models.ViewModels
{
    public class CategoryViewModel
    {
    }
    public class SearchCategoryViewModel
    {
       public  List<Category> Categories { get; set; }

       public string SearchItem { get; set; }

       public  Pager Pager { get; set; }
    }
    public class NewCategoryViewModel
    {
            
        [Required]
        [StringLength(50,ErrorMessage = "Maximum string length is 50")]
        public string Name { get; set; }

        public bool IsActive { get; set; }
    }

    public class EditCategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Maximum string length is 50")]
        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}