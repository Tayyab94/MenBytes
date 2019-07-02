using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MenBytes.Models.Context;
using MenBytes.Models.ViewModels;

using  System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace MenBytes.Controllers
{
    [Authorize(Roles = MB.Admin)]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            AdminHomeViewModel model = new AdminHomeViewModel();

            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                model.TotalCategories = _context.Categories.ToList().Count();
                model.TotalCompanies = _context.Companieses.ToList().Count();
                model.TotalWebSubscribe = _context.Subscribes.ToList().Count();
                model.TotalContactsUSer = _context.ContactUse.Where(x=>x.IsRead==false).ToList().Count();
                model.TotalProducts = _context.Products.ToList().Count();
                model.TotalCountAllProductComments = _context.ProductComments.ToList().Count();
            }
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult GetData()
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            //var query = _context.OrderDetails.Include(x => x.Product)
            //    .GroupBy(x =>x.Product.ProductName)
            //    .Select(x => new {name = x.Key, count = x.Key.Length}).ToList();

            // var p = _context.Products.ToList();

            //List<abs> mm=new List<abs>();
            // abs obj=new abs();

            // foreach (var product in p)
            // {
            //     obj.name = product.ProductName;
            //     var c = _context.OrderDetails.Where(x => x.ProductID == product.ID).Sum(x =>(int?)x.Quantity) ??0;
            //     mm.Add(obj);

            // }
            var dateCritaria = DateTime.Now.Date.AddDays(-15);

            //var q = _context.Orders.Where(x => x.createAt > dateCritaria).GroupBy(c => c.createAt).ToList();

            //var query = _context.Orders.Include(x => x.OrderDetail)
            //    .GroupBy(x =>x.createAt)
            //    .Select(x => new { name = x.Key, count = _context.Orders.Count() }).ToList();

            //List<bb> objbb= new List<bb>();

            //bb bobj=new bb();
            //foreach (var aa in q)
            //{
            //    bobj.dd = aa.Key;

            //}

            var q = _context.Orders.Where(x => x.createAt > dateCritaria)
                .GroupBy(c => new {date=EntityFunctions.TruncateTime(c.createAt)})
                .Select(xy=>new totalBill()
                {
                    bill = xy.Sum(x=>x.totalBill),
                    dTime = xy.Key.date.Value
                }).OrderBy(o=>o.dTime).ToList();


            //List<bb> objbb = new List<bb>();

            //bb bobj = new bb();
            //foreach (var aa in q)
            //{
            //    bobj.dd = aa.Key.date;

            //    bobj.billl = aa.Key.billTotal;

            //    objbb.Add(bobj);

            //}

            return Json(q,JsonRequestBehavior.AllowGet);


        }



        public ActionResult GetBrandDataForChart()
        {
           ApplicationDbContext _context = new ApplicationDbContext();

           //var company1 = _context.Products.Where(x => x.CompanyId == 1).Count();

           //var company2 = _context.Products.Where(x => x.CompanyId == 2).Count();


           var query4 = _context.Products.Include(x => x.Companies).Where(x=>x.Companies.isDeleted!=true)
               .GroupBy(c =>c.Companies.CompanyName).Select(x =>
                   new Companiesratio
                   {
                       Name = x.Key,
                       Qty = x.Count()
                   }).ToList();

           //CompanyRatio  companyRatio= new CompanyRatio();
           //companyRatio.company1_R = company1;
           //companyRatio.company2_R = company2;


           return Json(query4, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetOrderDataForChart()
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            var dateCritaria = DateTime.Now.Date.AddDays(-7);

            var query4 = _context.Orders.Include(x=>x.OrderStatus).Where(x => x.createAt >= dateCritaria)
                .GroupBy(c => c.OrderStatus.statusName).Select(x =>
                    new orderRatio
                    {
                        OrderCount = x.Count(),
                        orderName = x.Key
                    }).ToList();

            //CompanyRatio  companyRatio= new CompanyRatio();
            //companyRatio.company1_R = company1;
            //companyRatio.company2_R = company2;


            return Json(query4, JsonRequestBehavior.AllowGet);

        }



        public ActionResult GetDataForChartOftopBrand()
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            var dateCritaria = DateTime.Now.Date.AddDays(-9);

            var q=_context.OrderDetails.Include(c=>c.Product.Companies).Include(c=>c.Order).Where(x => x.Order.createAt >= dateCritaria)
                .GroupBy(c => c.Product.Companies.CompanyName).Select(x =>
                    new BrandRatio
                    {
                        OrderCount = x.Count(),
                        orderName = x.Key
                    }).ToList();

            //var query4 = _context.Orders.Include(x => x.OrderStatus).Include(c=>c.OrderDetail.ToList()).Where(x => x.createAt >= dateCritaria)
            //    .GroupBy(c => c.).Select(x =>
            //        new orderRatio
            //        {
            //            OrderCount = x.Count(),
            //            orderName = x.Key
            //        }).ToList();

            //CompanyRatio  companyRatio= new CompanyRatio();
            //companyRatio.company1_R = company1;
            //companyRatio.company2_R = company2;


            return Json(q, JsonRequestBehavior.AllowGet);

        }

    }

    public class totalBill
    {
        public DateTime dTime { get; set; }
        public double bill { get; set; }
    }

    public class bb
    {
        public DateTime dd { get; set; }

        public double billl { get; set; }
    }

    public class Companiesratio
    {
        public string Name { get; set; }
        public int Qty { get; set; }
    }

    public class abs
    {
        public string name { get; set; }

        public List<int> cout { get; set; }
    }


    public class CompanyRatio
    {
        public int company1_R { get; set; }

        public int  company2_R { get; set; }
    }


    public class orderRatio
    {
        public string  orderName { get; set; }
        public int OrderCount { get; set; }
    }

    public class BrandRatio
    {
        public string orderName { get; set; }
        public int OrderCount { get; set; }
    }
}