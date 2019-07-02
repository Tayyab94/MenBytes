using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MenBytes.Models.ViewModels
{
    public class AdminHomeViewModel
    {
        public int TotalWebSubscribe { get; set; }
        public int TotalProducts  { get; set; }

        public int TotalCategories { get; set; }
        public int TotalCompanies { get; set; }
        public int TotalContactsUSer { get; set; }

        public  int TotalCountAllProductComments { get; set; }
        
    }
}