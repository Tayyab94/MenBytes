using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using System.Web;

namespace MenBytes.Models.MenBytesModels
{
    public class ProductImage
    {
        public int ID { get; set; }
        public string ImagePath { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public  virtual Product Product { get; set; }
    }
}