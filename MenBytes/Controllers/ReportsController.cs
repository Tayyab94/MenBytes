using MenBytes.Models.Context;
using MenBytes.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using  System.Data.Entity;
using MenBytes.Models.MenBytesHandlers;
using MenBytes.Models;

namespace MenBytes.Controllers
{
    public class ReportsController : Controller
    {
        // GET: PurchaseReports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StockReport()
        {
            return View();
        }

        public JsonResult showProductsAl(DataTablesParam param, string f_Date,string to_Date)
        {
            ApplicationDbContext _context = new ApplicationDbContext();


            List<showAllProducts> orderList = new List<showAllProducts>();


            int totalCount = 0;
            int pageNo = 1;

            if (param.iDisplayStart >= param.iDisplayLength)
                pageNo = (param.iDisplayStart / param.iDisplayLength) + 1;


            if (param.sSearch != null)
            {
                totalCount = _context.Products.Include(x => x.ProductStockList).Include(x => x.productOrderDetailList)
                            .Include(x => x.Category).Include(x => x.Companies)
                        .Where(x => (x.ProductName.ToLower().Contains(param.sSearch.ToLower()) || x.Companies.CompanyName.ToString().Contains(param.sSearch) || x.Category.Name.ToLower().Contains(param.sSearch.ToLower())) && x.isDeleted == false).Count();
                orderList = _context.Products.Include(x => x.ProductStockList).Include(x => x.productOrderDetailList).Include(x => x.Category).Include(x => x.Companies)

                    .OrderBy(x => x.ID).Skip((pageNo - 1) * param.iDisplayLength).Take(param.iDisplayLength)
                    .Where(x => (x.ProductName.ToLower().Contains(param.sSearch.ToLower()) || x.Companies.CompanyName.ToString().Contains(param.sSearch) || x.Category.Name.ToLower().Contains(param.sSearch.ToLower()) || x.IsFeatured.ToString().ToLower().Contains(param.sSearch.ToLower()) || x.isDeleted.ToString().ToLower().Contains(param.sSearch.ToLower())) && x.isDeleted == false)
                    .Select(s => new showAllProducts
                    {
                        ID = s.ID,
                        ProductName = s.ProductName,
                        CategoryName = s.Category.Name,
                        CompanyName = s.Companies.CompanyName,
                        IsFeatured = s.IsFeatured,
                        IsDeleted = s.isDeleted,
                        Quantity = ((s.ProductStockList.Select(x => x.productUpdatedQuantity).Sum(x => (int?)x) ?? 0) - (s.productOrderDetailList.Select(x => x.Quantity).Sum(x => (int?)x) ?? 0)),



                        productImage = s.ProductImages.Select(x => x.ImagePath).FirstOrDefault()
                        //ProductImages = s.ProductImages.Select(x => x.ImagePath).ToList()
                    }).ToList();
            }
            else
            {
                totalCount = _context.Products.Include(x => x.ProductStockList).Include(x => x.productOrderDetailList).Include(x => x.Category).Include(x => x.Companies).Where(x => x.isDeleted == false).Count();
                orderList = _context.Products.Include(x => x.ProductStockList).Include(x => x.productOrderDetailList).Include(x => x.Category).Include(x => x.Companies)
                    .OrderBy(x => x.ID).Skip((pageNo - 1) * param.iDisplayLength).Take(param.iDisplayLength)

                    .Where(x => x.isDeleted == false)
                    .Select(s => new showAllProducts
                    {
                        ID = s.ID,
                        ProductName = s.ProductName,
                        CategoryName = s.Category.Name,
                        CompanyName = s.Companies.CompanyName,
                        IsFeatured = s.IsFeatured,
                        IsDeleted = s.isDeleted,
                        Quantity = ((s.ProductStockList.Select(x => x.productUpdatedQuantity).Sum(x => (int?)x) ?? 0) - (s.productOrderDetailList.Select(x => x.Quantity).Sum(x => (int?)x) ?? 0)),

                        productImage = s.ProductImages.Select(x => x.ImagePath).FirstOrDefault()

                    }).ToList();

            }




            return Json(new
            {
                aaData = orderList,
                sEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = totalCount
            }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult ProductStrock()
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            PurchaseStockViewModel model = new PurchaseStockViewModel();

            model.CategoriesList =_context.Categories.ToList();
            return View(model);
        }


        ////THis function show the product whose quantity is less 15
        public ActionResult FilterStock(string search, string toDate, string fromDate,int? categoryID, int? pageNo)
        {
            PurchaseStockViewModel model = new PurchaseStockViewModel();
            ProductHandler handler = new ProductHandler();
            model.SearchItem = search;
            model.toDate = toDate;
            model.frmDate = fromDate;

            model.CategoryId = categoryID.Value > 0 ? categoryID.Value : 0;

            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;
            //model.productList = handler.GetAllStockProduct(search,toDate,fromDate,pageNo.Value);

            var totalRecord = handler.GetAllStockProductCount(search, toDate, fromDate,categoryID.Value);

            model.productList = handler.GetAllStockProduct1(search, toDate, fromDate, categoryID.Value,pageNo.Value);

            if (model.productList != null)
            {
                model.Pager = new Pager(totalRecord, pageNo, 6);
                return PartialView("_FilterStock", model);
            }

            return PartialView("_FilterStock", model);

        }


        [HttpGet]
        public ActionResult SaleStock()
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            SaleStockViewModel model = new SaleStockViewModel();
            model.orderStatusList = _context.OrderStatuses.ToList();

            return View(model); 


        }




        public ActionResult FilterSaleStock(string payment, string toDate, string fromDate, int? statusID, int? pageNo)
        {
            SaleStockViewModel model = new SaleStockViewModel();
            OrderShippingHanlder handler = new OrderShippingHanlder();
            model.SearchItem = payment;
      
            model.toDate = toDate;
            model.frmDate = fromDate;

            model.statusId = statusID.Value > 0 ? statusID.Value : 0;

            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;

            var totalRecord = handler.GetAllSaleProductCount(payment, toDate, fromDate, statusID.Value);

            model.OrderRecord = handler.GetAllSaleProduct(payment, toDate, fromDate, statusID.Value, pageNo.Value);


            double bill = 0;
            foreach (var b in model.OrderRecord)
            {
                bill += b.totalBill;
            }

            ViewBag.totalBill = bill;

            if (model.OrderRecord != null)
            {
                model.Pager = new Pager(totalRecord, pageNo, 10);
                return PartialView("_FilterSaleStock", model);
            }

            return PartialView("_FilterSaleStock", model);

        }



        //public ActionResult FilterSaleStock(string payment, string toDate, string fromDate, int? statusID, int? pageNo)
        //{
        //    SaleStockViewModel model = new SaleStockViewModel();
        //    OrderShippingHanlder handler = new OrderShippingHanlder();
        //    model.SearchItem = payment;
        //    model.toDate = toDate;
        //    model.frmDate = fromDate;

        //    //model.statusId = statusID.Value > 0 ? statusID.Value : 0;

        //    model.statusId =statusID.HasValue ? statusID.Value > 0 ? statusID.Value : 0:0;

        //    pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;

        //    var totalRecord = handler.GetAllSaleProductCount(payment, toDate, fromDate, statusID.Value);

        //    model.OrderRecord = handler.GetAllSaleProduct(payment, toDate, fromDate, statusID.Value, pageNo.Value);

        //    if (model.OrderRecord != null)
        //    {
        //        model.Pager = new Pager(totalRecord, pageNo, 3);
        //        return PartialView("_FilterSaleStock", model);
        //    }

        //    return PartialView("_FilterSaleStock", model);

        //}




    }
}