using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MenBytes.Models.MenBytesModels
{
    public class ProductStock
    {
        public int  Id { get; set; }
        public int  productId { get; set; }

        [ForeignKey("productId")]
        public virtual  Product Product { get; set; }

        public int productPreviourQuantity { get; set; }
        public int productUpdatedQuantity { get; set; }

        public DateTime createdDate { get; set; }

        public string stockType
        {
            get; set;

        }
    }
}