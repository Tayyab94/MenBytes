using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MenBytes.Models.ViewModels
{
    public class CheckOutViewModel
    {
        public List<CartVM> cartList { get; set; }

        public  float CartGrandBill { get; set; }


        public int CartTotalItems { get; set; }

        public int cartTotalQuantities { get; set; }
    }
}