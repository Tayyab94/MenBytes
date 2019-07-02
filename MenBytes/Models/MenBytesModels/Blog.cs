using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MenBytes.Models.MenBytesModels
{
    public class Blog
    {

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string postedBy { get; set; }

        [Required]
        [AllowHtml]
        public string blogMessage { get; set; }

        public DateTime postedDate { get; set; }

        public string blogImage { get; set; }
        
        public virtual List<BlogsComment> BlogComments { get; set; }
    }
}