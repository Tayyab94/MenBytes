using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MenBytes.Models.Context
{
    public class Email
    {

        public int  ID { get; set; }
        public string To { get; set; }

        public string Subject { get; set; }
        [AllowHtml]
        public string Message { get; set; }
    }
}