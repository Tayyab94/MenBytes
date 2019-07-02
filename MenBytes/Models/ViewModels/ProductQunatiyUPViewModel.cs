using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MenBytes.Models.ViewModels
{
    public class ProductQunatiyUPViewModel
    {
        public int  Id { get; set; }

        public int quantity { get; set; }
    }

    public class EditProductQunatiyUPViewModel
    {
        public int  Id { get; set; }
        public int pro_Id { get; set; }

        public int quantity { get; set; }
    }
}