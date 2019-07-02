using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using MenBytes.Models;
using MenBytes.Models.Context;
using MenBytes.Models.MenBytesModels;
using MenBytes.Models.ViewModels;
using  System.Data.Entity;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace MenBytes.Controllers
{
    public class ClientSideController : Controller
    {
        // GET: ClientSide
        public ActionResult Index()
        {

            ApplicationDbContext _context= new ApplicationDbContext();
            clientHomeViewModel model= new clientHomeViewModel();
            model.BlogsList = _context.Blogs.Include(x=>x.BlogComments).OrderByDescending(x=>x.Id).Take(3).ToList();
            return View(model);
        }

        public ActionResult Contact()
        {ContactUs model = new ContactUs();
            return View(model);
        }

        [HttpPost]
        public ActionResult Contact(ContactUs obj)
        {
            if (ModelState.IsValid)
            {
                ApplicationDbContext _context= new ApplicationDbContext();
        
                obj.IsRead = false;
                obj
                    .ContactDate=DateTime.Now;
                if (string.IsNullOrEmpty(obj.Subject))
                    obj.Subject = "No Subject";
               

                _context.ContactUse.Add(obj);
                try
                {
                    _context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }
           

                return RedirectToAction("Contact", "ClientSide");
            }
            else
            {

                return View(obj);
            }
        }

        public ActionResult AboutUs()
        {
            using ( ApplicationDbContext _context= new ApplicationDbContext())
            {
                string roleid = _context.Roles.Where(x => x.Name == MB.Admin).Select(x => x.Id).SingleOrDefault();
                ViewBag.totalProducts = _context.Products.ToList().Count();

                ViewBag.totalClients = _context.Users.Include(x => x.Roles)
                    .Where(x => !x.Roles.Any(xt=>xt.RoleId==roleid)).ToList().Count();

                ViewBag.totalSale = _context.Orders.Count();
            
                return View();
            }
        }

        [HttpPost]
        public JsonResult subscriptionAction(string emailID)
        {
            string msg = string.Empty;
            if (string.IsNullOrEmpty(emailID))
            {
                msg = "Please Enter you Emmail id";

                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            try
            {
                ApplicationDbContext _context=new ApplicationDbContext();
                if (_context.Subscribes.Any(e=>e.Email.ToLower().Equals(emailID.ToLower())))
                {
                    msg = "You have already Subscribe!";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Subscribe model = new Subscribe();
                    model.Email = emailID.ToLower();
                    _context.Subscribes.Add(model);
                    _context.SaveChanges();

                    msg = "Thank you for your Subscription!";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception e)
            {
                msg = "Something Wrong please Try Again";

                return Json(msg, JsonRequestBehavior.AllowGet);
            
            }
            //return Json(msg, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        public ActionResult LiveChat()
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            string userId = User.Identity.GetUserId();
            string userName = _context.Users.SingleOrDefault(x => x.Id == userId).user_Name;

            ViewBag.userName = userName;
            return View();
            
        }
       
    }
}