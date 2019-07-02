using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MenBytes.Models.ViewModels
{
    public class UserWishlistVIewModel
    {
        public int ID { get; set; }
        public int productId { get; set; }

        public string productName
        {
            get; set; 

        }

        public string Image { get; set; }
    }
}