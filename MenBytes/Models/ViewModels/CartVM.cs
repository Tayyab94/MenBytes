using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MenBytes.Models.ViewModels
{
    public class CartVM
    {
        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public float Price { get; set; }

        public float Total { get { return Quantity * Price; } }
        
        public string ProductImage { get; set; }
    }
}