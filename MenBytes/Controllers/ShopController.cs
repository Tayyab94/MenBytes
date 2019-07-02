using MenBytes.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MenBytes.Models.MenBytesHandlers;
using MenBytes.Models;

namespace MenBytes.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index(string sreachProduct, int? MinimumPrice, int? MaximumPrice,int? compID ,int? CataId, int? SortBy, int? pageNo)
        {
            CategoriesHandler categoriesHandler= new CategoriesHandler();
            ProductHandler productHandler =new ProductHandler();
            CompaniesHandler companiesHandler= new CompaniesHandler();
            var pageSize = 9;

            PhotoGalleryViewModel model = new PhotoGalleryViewModel();

            model.FeaturedCatagories = categoriesHandler.GetAllCategoriesForShop();
            model.FeaturedCompanieses = companiesHandler.GetAllCompanieses();

            model.searchITem = sreachProduct;
            if (SortBy.HasValue)
            {
                model.SortBy = SortBy.Value;
            }

            if (CataId.HasValue)
            {
                model.categoryId = CataId.Value;
            }

            if (compID.HasValue)
            {
                model.companyId = compID.Value;
            }

           // model.MaximumPrice = ProductsService.Instance.GetMaximumPrice();

            pageNo = pageNo.HasValue ? pageNo.Value > 1 ? pageNo.Value : 1 : 1;
            model.ProductList = productHandler.GetShowAllProducts(sreachProduct, MinimumPrice, MaximumPrice, CataId, SortBy, pageNo.Value, pageSize, compID);

            int TotalItemCount = productHandler.GetAllProductsCount(sreachProduct, MinimumPrice, MaximumPrice, CataId, SortBy, compID);


            model.pager = new Pager(TotalItemCount, pageNo, pageSize);

            return View(model);
        }
    }
}