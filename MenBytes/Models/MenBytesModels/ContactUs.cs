using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MenBytes.Models.MenBytesModels
{
    public class ContactUs
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "User Name is Required")]
        [StringLength(20, ErrorMessage = "Maximum lenght is 20")]
        public string fullName { get; set; }

        [Required(ErrorMessage = "Phone No is Required")]
        public string Phone { get; set; }

        public string Subject { get; set; }
        [Required(ErrorMessage = "Please Type your Message")]
        public string Message { get; set; }


        [Required(ErrorMessage = "Email Id is Required")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        public DateTime ContactDate { get; set; }
        public bool IsRead { get; set; }
    }
}