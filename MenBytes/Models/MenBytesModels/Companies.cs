using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MenBytes.Models.MenBytesModels
{
    public class Companies
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50,ErrorMessage = "Maximum length is 50")]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }


        public bool isDeleted { get; set; }

        public  virtual  List<Product> ProductsList { get; set; }
    }
}