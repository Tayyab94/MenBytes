using MenBytes.Models.MenBytesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MenBytes.Models.Context;
using System.IO;
using MenBytes.Models.ViewModels;
using MenBytes.Models;
using  System.Data.Entity;
using MenBytes.Models.MenBytesHandlers;

namespace MenBytes.Controllers
{
    public class SiteBlogController : Controller
    {
        // GET: SiteBlog
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult BlogTable(string search, int? pageNo)
        {
            AllBlogViewModel model = new AllBlogViewModel();
            BlogHandler handler = new BlogHandler();
            model.SearchItem = search;
            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;

            var totalRecord = handler.GetBlogsCount(search);
            model.BlogsList = handler.GetAllBlogs(search, pageNo.Value);

            if (model.BlogsList != null)
            {
                model.Pager = new Pager(totalRecord, pageNo, 6);
                return PartialView("_BlogTable", model);
            }
            return PartialView("_BlogTable", model);
        }
        [HttpGet]
        public ActionResult AddBlog()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddBlog(Blog model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (ApplicationDbContext _context= new ApplicationDbContext())
            {
                var file = Request.Files[0];
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/BlogImages/"),fileName);
                file.SaveAs(path);
                var blogImageUrl = string.Format("/Images/BlogImages/{0}", fileName);
                model.blogImage = blogImageUrl;
                model.postedDate = DateTime.Now;
                _context.Blogs.Add(model);
                _context.SaveChanges();
            }
            return View();
        }

        public ActionResult BlogDetail(int id)
        {
            ApplicationDbContext _context= new ApplicationDbContext();

            DetailBlogViewModel model= new DetailBlogViewModel();

            var blog = _context.Blogs.Include(t=>t.BlogComments).Where(s => s.Id == id).SingleOrDefault();
            model.Id = blog.Id;
            model.blogImage = blog.blogImage;
            model.Title = blog.Title;
            model.postedBy = blog.postedBy;
            model.postedDate = blog.postedDate;
            model.blogMessage = blog.blogMessage;
            model.RecentBlogs = _context.Blogs.Take(5).OrderByDescending(x=>x.Id).ToList();
            model.BlogComments = blog.BlogComments;
            return View(model);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            using ( ApplicationDbContext _context=new ApplicationDbContext())
            {
                Blog blog = _context.Blogs.Where(s => s.Id.Equals(id)).SingleOrDefault();

                return View(blog);
            }

        }

        [HttpPost]
        public ActionResult Edit(Blog obj)
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            var model = _context.Blogs.Where(x => x.Id.Equals(obj.Id)).SingleOrDefault();
            model.Title = obj.Title;
            model.postedBy = obj.postedBy;
            model.postedDate=DateTime.Now;
            model.blogMessage = obj.blogMessage;

            var file = Request.Files[0];

            if (!string.IsNullOrEmpty(file.FileName))
            {
                var path1 = Request.MapPath("~/" + model.blogImage);
                if (System.IO.File.Exists(path1))
                {
                   System.IO.File.Delete(path1);
                }
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/BlogImages/"), fileName);
                file.SaveAs(path);
                var blogImageUrl = string.Format("/Images/BlogImages/{0}", fileName);
                model.blogImage = blogImageUrl;
            }
            else
            {
                model.blogImage = model.blogImage;
            }

            _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("Index","SiteBlog");
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            BlogHandler blogHandler= new BlogHandler(); 
          //  var blog = _context.Blogs.Include(x=>x.BlogComments).Where(s => s.Id == id).SingleOrDefault();

           // blog.BlogComments.RemoveAll(x => x.BlogID == blog.Id);

            //_context.Blogs.Remove(blog);


            //foreach (var child in blog.BlogComments)
            //{
            //    _context.Entry(child).State = System.Data.Entity.EntityState.Deleted;
            //}

            Blog blog = blogHandler.GetBlogById(id);

            blog.BlogComments.RemoveAll(x => x.BlogID == id);

            _context.Entry(blog).State = System.Data.Entity.EntityState.Deleted;

         //  _context.Blogs.Remove(blog);
            _context.SaveChanges();

            return RedirectToAction("BlogTable");

        }

        
        public ActionResult BloglistPage(string search,int? pageNo)
        {
            AllBlogViewModel model = new AllBlogViewModel();
            BlogHandler handler = new BlogHandler();
            model.SearchItem = search;
            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;

            var totalRecord = handler.GetBlogsCount(search);
            model.BlogsList = handler.GetAllBlogs(search, pageNo.Value);

            if (model.BlogsList != null)
            {
                model.Pager = new Pager(totalRecord, pageNo, 6);
                return View(model);
            }

            return View(model);
        }

        //Useing
        [HttpPost]
        public ActionResult CommentAdding(int id, string userName, string comment)
        {
            int ProID = id;
            string name = userName;
            string productComment = comment;

            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                try
                {
                    BlogsComment model = new BlogsComment();
                    model.BlogID = id;
                    model.Name = userName;
                    model.Comment = comment;
                    model.PostedDate = DateTime.Now;
                    _context.BlogsComments.Add(model);
                    _context.SaveChanges();

                    var countBlogs = _context.BlogsComments.Where(x => x.BlogID == model.BlogID).Count();

                    return Json(new
                    {
                        model.Name,
                        model.Comment,
                        model.PostedDate
                        ,count= countBlogs
                    }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);


                    return Json(new { ProID, name, productComment }, JsonRequestBehavior.AllowGet);
                }
            }


        }

    }
}