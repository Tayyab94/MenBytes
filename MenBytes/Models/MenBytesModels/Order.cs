using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MenBytes.Models.MenBytesModels
{
    public class Order
    {
        public int Id { get; set; }

        public string userID { get; set; }

        [ForeignKey("userID")]
        public virtual ApplicationUser user { get; set; }

        public DateTime createAt { get; set; }

        public double totalBill { get; set; }

        public string paymentType { get; set; }

        public int orderStatusID { get; set; }

        [ForeignKey("orderStatusID")]
        public virtual OrderStatus OrderStatus { get; set; }

        public int OrderShippingID { get; set; }


        public  DateTime? DeliveryDate { get; set; }


        [ForeignKey("OrderShippingID")]
        public virtual OrderShipping OrderShipping { get; set; }

        public virtual List<OrderDetail> OrderDetail { get; set; }
    }
}