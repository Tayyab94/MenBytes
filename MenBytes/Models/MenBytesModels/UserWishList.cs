using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MenBytes.Models.MenBytesModels
{
    public class UserWishList
    {
        public int Id { get; set; }
        public string user_id { get; set; }

        [ForeignKey("user_id")]
        public virtual  ApplicationUser Users { get; set; }

        public int  product_id { get; set; }

        [ForeignKey("product_id")]
        public virtual Product Products { get; set; }
    }
}