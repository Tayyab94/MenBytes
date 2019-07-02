using MenBytes.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MenBytes.Models.Context;
using MenBytes.Models.ViewModels;
using MenBytes.Models.MenBytesHandlers;
using System.Net.Mail;
using System.Text;
using System.Net;
using System.IO;
using MenBytes.Models.MenBytesModels;

namespace MenBytes.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        //THis function show the product whose quantity is less 15
        public ActionResult ContactTableList(string search, int? pageNo)
        {
           AllContactsListViewModel model = new AllContactsListViewModel();
           ContactHanlder handler = new ContactHanlder();
            model.SearchItem = search;
            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;

            var totalRecord = handler.GetContactMessageCount(search);
            model.ContactUsList = handler.GetAllContactMessage(search, pageNo.Value);

            if (model.ContactUsList != null)
            {
                model.Pager = new Pager(totalRecord, pageNo, 6);
                return PartialView("_ContactTableList", model);
            }

            return PartialView("_ContactTableList", model);

        }


        [HttpGet]
        public ActionResult DetailContact(int id)
        {
           ApplicationDbContext _context = new ApplicationDbContext();

           var ContactObj = _context.ContactUse.Where(x => x.ID == id && x.IsRead == false).FirstOrDefault();

           return PartialView("_DetailContact",ContactObj);
        }

        [HttpGet]
        public PartialViewResult GetEmailData(int id)
        {
            ApplicationDbContext _context= new ApplicationDbContext();

            var email = _context.ContactUse.Where(x => x.ID == id).Select(x=>new Email
            {ID = x.ID,
               Subject = x.Subject,
               To = x.Email
            }).SingleOrDefault();

               return PartialView("_GetEmailData", email);
          
        }


        public ActionResult SendEmailToCient(Email obj, HttpPostedFileBase fileUploader)
        {

            ApplicationDbContext _context= new ApplicationDbContext();
            string senderEmail = System.Configuration.ConfigurationManager.AppSettings["senderEmail"].ToString();

               string senderPassword =System.Configuration.ConfigurationManager.AppSettings["sendPassword"].ToString();
        //    sendEmailFunction(obj.To,obj.Subject,obj.Message);


            using (MailMessage mail = new MailMessage(senderEmail, obj.To))
            {
                mail.Subject = obj.Subject;
                mail.Body = obj.Message;
                if (fileUploader != null)
                {
                    string fileName = Path.GetFileName(fileUploader.FileName);
                    mail.Attachments.Add(new Attachment(fileUploader.InputStream, fileName));
                    
                }

                try
                {
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential networkCredential = new NetworkCredential(senderEmail, senderPassword);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = networkCredential;
                    smtp.Port = 587;
                    smtp.Send(mail);
                    ViewBag.Message = "Sent";

                    var contactus = _context.ContactUse.Where(x => x.ID == obj.ID).SingleOrDefault();
                    contactus.IsRead = true;
                    _context.Entry(contactus).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                    TempData["mailError"] = "asd";
                    ViewBag.mailErrorMessage = "asda";
                    return RedirectToAction("Index");
                }
             
                return RedirectToAction("Index");
            }


            ////try
            ////{
            ////    string senderEmail = System.Configuration.ConfigurationManager.AppSettings["senderEmail"].ToString();

            ////    string senderPassword =
            ////        System.Configuration.ConfigurationManager.AppSettings["sendPassword"].ToString();

            ////    SmtpClient client = new SmtpClient("smtp.gmail.com", 587);

            ////    client.EnableSsl = true;
            ////    client.Timeout = 100000;
            ////    client.DeliveryMethod = SmtpDeliveryMethod.Network;
            ////    client.UseDefaultCredentials = false;
            ////    client.Credentials = new NetworkCredential(senderEmail, senderPassword);

            ////    MailMessage mailMessage = new MailMessage(senderEmail, obj.To, obj.Subject, obj.Message);
            ////    mailMessage.IsBodyHtml = true;
            ////    mailMessage.BodyEncoding = Encoding.UTF8;

            ////    client.Send(mailMessage);
            ////}
            ////catch (Exception e)
            ////{
            ////    Console.WriteLine(e);
            ////    throw;
            ////}
            //return View();


        }

       

        public void sendEmailFunction(string toEmail, string subjct, string emailBody)
        {
            try
            {
                string senderEmail = System.Configuration.ConfigurationManager.AppSettings["senderEmail"].ToString();

                string senderPassword =
                    System.Configuration.ConfigurationManager.AppSettings["sendPassword"].ToString();

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);

                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                MailMessage mailMessage = new MailMessage(senderEmail, toEmail, subjct, emailBody);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = Encoding.UTF8;

                client.Send(mailMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public ActionResult AllContactMessages()
        {
            return View();
        }



        //This Show all the Contact messages
        public JsonResult showContactsAl(DataTablesParam param)
        {
            ApplicationDbContext _context = new ApplicationDbContext();


            List<ContactUs> orderList = new List<ContactUs>();


            int totalCount = 0;
            int pageNo = 1;

            if (param.iDisplayStart >= param.iDisplayLength)
                pageNo = (param.iDisplayStart / param.iDisplayLength) + 1;


            if (param.sSearch != null)
            {
                totalCount = _context.ContactUse.Where(x => x.fullName.ToLower().Contains(param.sSearch.ToLower()) || x.Phone.ToString().Contains(param.sSearch) || x.Subject.ToLower().Contains(param.sSearch.ToLower()) || x.IsRead.ToString().Contains(param.sSearch) || x.Email.ToLower().Contains(param.sSearch.ToLower()) || x.ContactDate.ToString().ToLower().Contains(param.sSearch.ToLower())).Count();
                orderList = _context.ContactUse
                    .OrderBy(x => x.ID).Skip((pageNo - 1) * param.iDisplayLength).Take(param.iDisplayLength)
                    .Where(x => x.fullName.ToLower().Contains(param.sSearch.ToLower()) || x.Phone.ToString().Contains(param.sSearch) || x.Subject.ToLower().Contains(param.sSearch.ToLower()) || x.IsRead.ToString().Contains(param.sSearch) || x.Email.ToLower().Contains(param.sSearch.ToLower()))
                    .ToList();
            }
            else
            {
                totalCount = _context.ContactUse.Count();
                orderList = _context.ContactUse
                    .OrderBy(x => x.ID).Skip((pageNo - 1) * param.iDisplayLength).Take(param.iDisplayLength)
                  .ToList();

            }
            
            return Json(new
            {
                aaData = orderList,
                sEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = totalCount
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DeleteContactMessage(int id)
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            var con = _context.ContactUse.SingleOrDefault(x => x.ID == id);

            _context.ContactUse.Remove(con);
            _context.SaveChanges();

            return Json(new {status = true}, JsonRequestBehavior.AllowGet);
        }
    }
}