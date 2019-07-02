using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MenBytes.Models.ViewModels
{
    public class ShippingVM
    {

        public string firstName { get; set; }

        public string lastName  { get; set; }


        public string address { get; set; }

        public string city { get; set; }

        public string country { get; set; }


        public string email { get; set; }

        public string phone { get; set; }

        public string paymentType { get; set; }
    }
}