using MenBytes.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MenBytes.Models.MenBytesModels;
using  System.Data.Entity;

namespace MenBytes.Models.MenBytesHandlers
{
    public class BlogHandler
    {


        public Blog GetBlogById(int id)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                return _context.Blogs.Include(x => x.BlogComments).Where(x => x.Id == id).SingleOrDefault();
            }
        }

        public List<Blog> GetAllBlogs(string search, int page)
        { /*AllProductViewModel model= new AllProductViewModel();*/
            int pageSize = 6;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {

                if (!string.IsNullOrEmpty(search))
                {
                    return _context.Blogs
                        .Where(x =>(x.Title.ToLower().Contains(search.ToLower()) || x.postedBy.ToLower().Contains(search.ToLower())) && x.Title != null)
                        .OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    return _context.Blogs.OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }


            }
        }

        public int GetBlogsCount(string Search)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                if (!string.IsNullOrEmpty(Search))
                {
                    return _context.Blogs.Where(x =>(x.Title.ToLower().Contains(Search.ToLower()) || x.postedBy.ToLower().Contains(Search.ToLower())) && x.Title != null).Count();
                }
                else
                {
                    return _context.Blogs.Count();
                }
            }
        }
    }
}