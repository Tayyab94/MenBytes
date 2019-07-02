using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MenBytes.Models.MenBytesModels
{
    public class OrderShipping
    {
        public int Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        public string company_name { get; set; }

        public string address { get; set; }

        public string City { get; set; }
        public string Country { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
      
    }
}