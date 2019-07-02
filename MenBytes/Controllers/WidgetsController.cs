using MenBytes.Models.Context;
using MenBytes.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;

namespace MenBytes.Controllers
{
    public class WidgetsController : Controller
    {
        // GET: Widgets
        public ActionResult products()
        {
            AllProducts model = new AllProducts();
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {

                model.pro = _context.Products.Include(x => x.Category).Include(x => x.Companies)
                    .Include(x => x.ProductImages).Where(x=>x.isDeleted==false && x.Companies.isDeleted==false)
                    .Select(s => new showAllProducts
                    {
                        ID = s.ID,
                        ProductName = s.ProductName,
                         ProductDescription = s.ProductDescription,
                        CategoryName = s.Category.Name,
                        CompanyName = s.Companies.CompanyName,
                        IsFeatured = s.IsFeatured,
                //        Quantity = s.Quantity,
                        salePrice = s.salePrice,
                        ProductImages = s.ProductImages.Select(x => x.ImagePath).ToList()
                    }).OrderByDescending(x => x.ID).Take(8).ToList();
              


                return PartialView("_products", model);
            }


        }
    }
}