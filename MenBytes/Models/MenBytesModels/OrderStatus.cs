using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MenBytes.Models.MenBytesModels
{
    public class OrderStatus
    {
        public int Id { get; set; }
        [MaxLength(25)]
        public string statusName { get; set; }

        public virtual List<Order> Orders { get; set; }
    }
}