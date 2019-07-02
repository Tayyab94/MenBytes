using MenBytes.Models.MenBytesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MenBytes.Models.ViewModels
{
    public class BlogViewModel
    {

    }


    public class DetailBlogViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string postedBy { get; set; }
        
        [AllowHtml]
        public string blogMessage { get; set; }

        public DateTime postedDate { get; set; }

        public string blogImage { get; set; }

        public List<Blog> RecentBlogs { get; set; }

        public List<BlogsComment> BlogComments
        {
            get;
            set;
        }
      
    }

    public class AllBlogViewModel
    {
        public List<Blog> BlogsList { get; set; }
        public string SearchItem { get; set; }

        public Pager Pager { get; set; }
    }
}