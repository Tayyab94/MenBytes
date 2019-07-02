using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MenBytes.Models.MenBytesModels
{
    public class OrderDetail
    {
        public int Id { get; set; }


        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        public int ProductID { get; set; }

        [ForeignKey("ProductID")]
        public virtual  Product Product { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public float Price { get; set; }

        public float Total { get; set; }
    }
}