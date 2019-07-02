using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using  System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace MenBytes.Models.MenBytesModels
{
    public class Subscribe
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Please Enter your Email ID")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}