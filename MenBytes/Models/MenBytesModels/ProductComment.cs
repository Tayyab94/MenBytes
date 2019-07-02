using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MenBytes.Models.MenBytesModels
{
    public class ProductComment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter your Name")]
        public String Name { get; set; }

        [Required(ErrorMessage = "Please write your feedback")]
        public string Comment { get; set; }

        public DateTime PostedDate { get; set; }

        public int ProductID { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product  Product { get; set; }

    }
}