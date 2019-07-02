using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MenBytes.Models.Context;
using System.Data.Entity;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc.Async;
using System.Web.Mvc.Filters;
using MenBytes.Models.MenBytesModels;
using MenBytes.Models.ViewModels;

namespace MenBytes.Models.MenBytesHandlers
{
    public class ProductHandler
    {
        public int AddProduct(NewProductViewModel obj, FormCollection data)
        {
            int id = 0;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                Product modelProduct = new Product();
                modelProduct.ProductName = obj.ProductName;
                //   modelProduct.Quantity = obj.Quantity;
                modelProduct.salePrice = obj.salePrice;
                modelProduct.purchasePrice = obj.purchasePrice;
                modelProduct.CategoryId = obj.CategoryId;
                modelProduct.CompanyId = obj.CompanyId;


                //long uno = DateTime.Now.Ticks;
                //int count = 0;
                //foreach (string file_Name in Request.Files)
                //{
                //    HttpPostedFileBase file = Request.Files[file_Name];
                //    if (!string.IsNullOrWhiteSpace(file.FileName))
                //    {
                //        string url = "/Images/PostAdvImages/" + $"{uno}_{++count}" + file.FileName.Substring(file.FileName.LastIndexOf("."));
                //        string path = Server.MapPath(url);
                //        file.SaveAs(path);
                //        objAdv.Images.Add(new AdvertisementImages { Url = url, Priority = count });
                //    }
                //}


                modelProduct.ProductImages.Add(new ProductImage { ImagePath = "as" });
                _context.Products.Add(modelProduct);
                _context.SaveChanges();
                id = modelProduct.ID;
            }

            return id;
        }

        public Product GetProductById(int id)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                return _context.Products.Include(s => s.Category).Include(c => c.Companies)
                    .Include(i => i.ProductImages).Include(x => x.ProductComments)
                    .Where(x => x.ID.Equals(id)).SingleOrDefault();
            }
        }

        public List<showAllProducts> GetAllProducts(string search, int page)
        { /*AllProductViewModel model= new AllProductViewModel();*/
            int pageSize = 6;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {

                if (!string.IsNullOrEmpty(search))
                {
                    return _context.Products.Include(x => x.Category).Include(x => x.Companies)
                        .Include(x => x.ProductImages)
                        .Where(x => x.ProductName.ToLower().Contains(search.ToLower()) && x.ProductName != null && x.isDeleted == false && (x.ProductStockList.Select(sq => sq.productUpdatedQuantity).Sum(s => (int?)s) ?? 0) - (x.productOrderDetailList.Select(xy => xy.Quantity).Sum(s => (int?)s) ?? 0) <= 30)
                        .Select(s => new showAllProducts
                        {
                            ID = s.ID,
                            ProductName = s.ProductName,
                            CategoryName = s.Category.Name,
                            CompanyName = s.Companies.CompanyName,
                            IsFeatured = s.IsFeatured,
                            Quantity = s.ProductStockList.Select(x => x.productUpdatedQuantity).Sum(x => (int?)x) ?? 0 - s.productOrderDetailList.Select(x => x.Quantity).Sum(x => (int?)x) ?? 0,

                            ProductImages = s.ProductImages.Select(x => x.ImagePath).ToList()
                        }).OrderBy(x => x.ID).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    return _context.Products
                        .Where(x => x.isDeleted == false && (x.ProductStockList.Select(xt => xt.productUpdatedQuantity).Sum(s => (int?)s) ?? 0) - (x.productOrderDetailList.Select(xy => xy.Quantity).Sum(s => (int?)s) ?? 0) <= 30)
                        .Include(x => x.Category).Include(x => x.Companies)
                        .Include(x => x.ProductImages).Include(x => x.productOrderDetailList).Include(x => x.ProductStockList)
                        .Select(s => new showAllProducts
                        {
                            ID = s.ID,
                            ProductName = s.ProductName,
                            CategoryName = s.Category.Name,
                            CompanyName = s.Companies.CompanyName,
                            IsFeatured = s.IsFeatured,
                            Quantity = ((s.ProductStockList.Select(x => x.productUpdatedQuantity).Sum(x => (int?)x) ?? 0) - (s.productOrderDetailList.Select(x => x.Quantity).Sum(x => (int?)x) ?? 0)),

                            // Quantity =s.ProductStockList.Select(x=>x.productUpdatedQuantity).Sum(x=>x.productUpdatedQuantity) -s.productOrderDetailList.Where(x=>x.ProductID==s.ID).Sum(x=>(int?)x.Quantity??0),
                            //  Quantity =int.Parse(_context.ProductStocks.Where(x => x.productId == s.ID).Sum(x => x.productUpdatedQuantity).ToString()) -int.Parse(_context.OrderDetails.Where(x => x.ProductID == s.ID).Sum(x => x.Quantity).ToString()),

                            ProductImages = s.ProductImages.Select(x => x.ImagePath).ToList()
                        }).OrderBy(x => x.ID).Skip((page - 1) * pageSize).Take(pageSize).ToList();




                    //return  _context.Products.Where(x=> x.isDeleted == false).Include(x => x.Category).Include(x => x.Companies)
                    //    .Include(x => x.ProductImages).Include(x=>x.productOrderDetailList).Include(x=>x.ProductStockList)
                    //    .Select(s => new showAllProducts
                    //    {
                    //        ID = s.ID,
                    //        ProductName = s.ProductName,
                    //        CategoryName = s.Category.Name,
                    //        CompanyName = s.Companies.CompanyName,
                    //        IsFeatured = s.IsFeatured,

                    //        Quantity = ((s.ProductStockList.Select(x=>x.productUpdatedQuantity).Sum(x=>(int?)x)??0) -(s.productOrderDetailList.Select(x=>x.Quantity).Sum(x=>(int?)x)??0)),

                    //        // Quantity =s.ProductStockList.Select(x=>x.productUpdatedQuantity).Sum(x=>x.productUpdatedQuantity) -s.productOrderDetailList.Where(x=>x.ProductID==s.ID).Sum(x=>(int?)x.Quantity??0),
                    //        //  Quantity =int.Parse(_context.ProductStocks.Where(x => x.productId == s.ID).Sum(x => x.productUpdatedQuantity).ToString()) -int.Parse(_context.OrderDetails.Where(x => x.ProductID == s.ID).Sum(x => x.Quantity).ToString()),

                    //        ProductImages = s.ProductImages.Select(x => x.ImagePath).ToList()
                    //    }).OrderBy(x => x.ID).Skip((page - 1) * pageSize).Take(pageSize).ToList();


                }


            }
        }

        public int GetProductCount(string Search)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                if (!string.IsNullOrEmpty(Search))
                {
                    return _context.Products.Where(x =>
                        x.ProductName.ToLower().Contains(Search.ToLower()) && x.ProductName != null && x.isDeleted == false).Count();
                }
                else
                {
                    return _context.Products.Where(x => x.isDeleted == false).Count();
                }
            }
        }


        //this function is show in shop Controller
        public List<Product> GetShowAllProducts(string sreachProduct, int? minimumPrice, int? maximumPrice, int? cataId, int? SortBy, int pageNo, int pageSize, int? compId)
        {
            using (var _context = new ApplicationDbContext())
            {

                List<Product> productsList = _context.Products.Include(x => x.Category).Include(x=>x.Companies).Include(c => c.ProductImages).Where(x => x.isDeleted != true && x.Companies.isDeleted == false && x.Category.IsActive==true).ToList();

                if (cataId.HasValue)
                {
                    productsList = productsList.Where(x => x.CategoryId == cataId.Value).ToList();
                }

                if (compId.HasValue)
                {
                    productsList = productsList.Where(x => x.CompanyId == compId.Value).ToList();
                }
                if (!string.IsNullOrEmpty(sreachProduct))
                {
                    productsList = productsList.Where(x => x.ProductName.ToLower().Contains(sreachProduct.ToLower())).ToList();
                }

                if (minimumPrice.HasValue)
                {
                    productsList = productsList.Where(x => x.salePrice >= minimumPrice).ToList();
                }
                if (maximumPrice.HasValue)
                {
                    productsList = productsList.Where(x => x.salePrice <= maximumPrice).ToList();
                }

                if (SortBy.HasValue)
                {
                    switch ((SortByEnum)SortBy.Value)
                    {

                        case SortByEnum.Popularity:

                            break;
                        case SortByEnum.lowToHigh:
                            productsList = productsList.OrderBy(x => x.salePrice).ToList();
                            break;
                        case SortByEnum.highToLow:
                            productsList = productsList.OrderByDescending(x => x.salePrice).ToList();
                            break;
                        default:
                            productsList = productsList.ToList();
                            break;
                    }
                }

                return productsList.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }

        }



        //this function is Returnt the Total product Count
        public int GetAllProductsCount(string sreachProduct, int? minimumPrice, int? maximumPrice, int? cataId, int? SortBy, int? compId)
        {
            using (var _context = new ApplicationDbContext())
            {

                List<Product> productsList = _context.Products.Include(x=>x.Companies).Where(x => x.isDeleted != true && x.Companies.isDeleted==false).ToList();

                if (cataId.HasValue)
                {
                    productsList = productsList.Where(x => x.CategoryId == cataId.Value).ToList();
                }
                if (compId.HasValue)
                {
                    productsList = productsList.Where(x => x.CompanyId == compId.Value).ToList();
                }

                if (!string.IsNullOrEmpty(sreachProduct))
                {
                    productsList = productsList.Where(x => x.ProductName.ToLower().Contains(sreachProduct.ToLower())).ToList();
                }

                if (minimumPrice.HasValue)
                {
                    productsList = productsList.Where(x => x.salePrice >= minimumPrice).ToList();
                }
                if (maximumPrice.HasValue)
                {
                    productsList = productsList.Where(x => x.salePrice <= maximumPrice).ToList();
                }

                if (SortBy.HasValue)
                {
                    switch ((SortByEnum)SortBy.Value)
                    {

                        case SortByEnum.Popularity:

                            break;
                        case SortByEnum.lowToHigh:
                            productsList = productsList.OrderBy(x => x.salePrice).ToList();
                            break;
                        case SortByEnum.highToLow:
                            productsList = productsList.OrderByDescending(x => x.salePrice).ToList();
                            break;
                        default:
                            productsList = productsList.ToList();
                            break;
                    }
                }

                return productsList.Count;
            }

        }



        public List<stockProduct> GetAllStockProduct(string search, string toDate, string fromDate, int page)
        { /*AllProductViewModel model= new AllProductViewModel();*/
            int pageSize = 6;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                List<stockProduct> model = new List<stockProduct>();

                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    model = _context.ProductStocks.Include(x => x.Product).Include(x => x.Product.productOrderDetailList)

                        .Where(x => x.createdDate.Date >= Convert.ToDateTime(toDate) && x.createdDate.Date <= Convert.ToDateTime(fromDate))
                        .Select(s => new stockProduct
                        {
                            id = s.productId,
                            ProductName = s.Product.ProductName,
                            inQty = s.productUpdatedQuantity,
                            outQty = s.Product.productOrderDetailList.Sum(x => x.Quantity),

                            //    s.ProductStockList.Select(x => x.productUpdatedQuantity).Sum(x => (int?)x) ?? 0 - s.productOrderDetailList.Select(x => x.Quantity).Sum(x => (int?)x) ?? 0,

                            //ProductImages = s.ProductImages.Select(x => x.ImagePath).ToList()
                        }).OrderBy(x => x.id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }
               


                return model;

            }
        }



        public List<stockProduct> GetAllStockProduct1(string search, string toDate, string fromDate, int catId, int page)
        { /*AllProductViewModel model= new AllProductViewModel();*/
            int pageSize = 6;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {

                List<stockProduct> model = new List<stockProduct>();
                DateTime form = Convert.ToDateTime(fromDate).Date.AddDays(1);
                DateTime to = Convert.ToDateTime(toDate).Date;


                if (string.IsNullOrEmpty(search) && catId == 0)
                {
                    model = _context.ProductStocks.Include(x => x.Product).Include(x => x.Product.productOrderDetailList).Include(x => x.Product.Category)
                        .Where(x => (x.createdDate <= form && x.createdDate >= to))
                        .GroupBy(x => new { ID = x.productId,Date=x.createdDate,pName =x.Product.ProductName , categoryName = x.Product.Category.Name })
                        .Select(x => new stockProduct()
                        {ProductName = x.Key.pName,
                            id = x.Key.ID,
                            inQty = x.Sum(y => y.productUpdatedQuantity),
                            Category = x.Key.categoryName,
                            outQty = _context.OrderDetails
                                         .Where(xp => xp.ProductID == x.Key.ID && (xp.Order.createAt <= form && xp.Order.createAt >= to))
                                         .Sum(eq => (int?)eq.Quantity) ?? 0,
                        C_Date = x.Key.Date,
                            updateCount = x.Count(),
                            stockType = search,
                            preQ = (_context.ProductStocks
                                        .Where(xp => xp.productId == x.Key.ID && (xp.createdDate < to))
                                        .Sum(eq => (int?)eq.productUpdatedQuantity) ?? 0) - (_context.OrderDetails
                                                                                                 .Where(xp => xp.ProductID == x.Key.ID && (xp.Order.createAt < to))
                                                                                                 .Sum(eq => (int?)eq.Quantity) ?? 0),
                        }).OrderBy(x => x.id).Skip((page - 1) * pageSize).Take(pageSize)
                        .ToList();
                }
                else if (!string.IsNullOrEmpty(search) && catId == 0)
                {
                    model = _context.ProductStocks.Include(x => x.Product).Include(x => x.Product.productOrderDetailList).Include(x => x.Product.Category)
                        .Where(x => (x.createdDate <= form && x.createdDate >= to) && x.stockType.ToLower() == search.ToLower())
                        .GroupBy(x => new { ID = x.productId, Date=x.createdDate, pName = x.Product.ProductName,categoryName = x.Product.Category.Name })
                        .Select(x => new stockProduct()
                        {
                            ProductName = x.Key.pName,
                            id = x.Key.ID,
                            inQty = x.Sum(y => y.productUpdatedQuantity),

                            Category = x.Key.categoryName,
                            outQty = _context.OrderDetails
                                         .Where(xp => xp.ProductID == x.Key.ID && (xp.Order.createAt <= form && xp.Order.createAt >= to))
                                         .Sum(eq => (int?)eq.Quantity) ?? 0,
                            updateCount = x.Count(),
                            preQ = (_context.ProductStocks
                                        .Where(xp => xp.productId == x.Key.ID && (xp.createdDate < to))
                                        .Sum(eq => (int?)eq.productUpdatedQuantity) ?? 0) - (_context.OrderDetails
                                        .Where(xp => xp.ProductID == x.Key.ID && (xp.Order.createAt < to))
                                        .Sum(eq => (int?)eq.Quantity) ?? 0),
                            stockType = search,
                            C_Date = x.Key.Date
                        }).OrderBy(x => x.id).Skip((page - 1) * pageSize).Take(pageSize)
                        .ToList();
                }
                else if (string.IsNullOrEmpty(search) && catId != 0)
                {
                    model = _context.ProductStocks.Include(x => x.Product).Include(x => x.Product.productOrderDetailList).Include(x => x.Product.Category)
                        .Where(x => (x.createdDate <= form && x.createdDate >= to) && x.Product.CategoryId == catId)
                        .GroupBy(x => new { ID = x.productId, Date = x.createdDate, pName = x.Product.ProductName, categoryName = x.Product.Category.Name })
                        .Select(x => new stockProduct()
                        {
                            ProductName = x.Key.pName,
                            id = x.Key.ID,
                            inQty = x.Sum(y => y.productUpdatedQuantity),

                            Category = x.Key.categoryName,
                            outQty = _context.OrderDetails
                                         .Where(xp => xp.ProductID == x.Key.ID && (xp.Order.createAt <= form && xp.Order.createAt >= to))
                                         .Sum(eq => (int?)eq.Quantity) ?? 0,
                            updateCount = x.Count(),
                            C_Date = x.Key.Date,
                            stockType = search,
                            preQ = (_context.ProductStocks
                                        .Where(xp => xp.productId == x.Key.ID && (xp.createdDate < to))
                                        .Sum(eq => (int?)eq.productUpdatedQuantity) ?? 0) - (_context.OrderDetails
                                                                                                 .Where(xp => xp.ProductID == x.Key.ID && (xp.Order.createAt < to))
                                                                                                 .Sum(eq => (int?)eq.Quantity) ?? 0),
                        }).OrderBy(x => x.id).Skip((page - 1) * pageSize).Take(pageSize)
                        .ToList();
                }
                else
                {
                    model = _context.ProductStocks.Include(x => x.Product).Include(x => x.Product.productOrderDetailList).Include(x => x.Product.Category)
                        .Where(x => (x.createdDate <= form && x.createdDate >= to) && x.stockType.ToLower() == search.ToLower() && x.Product.CategoryId == catId)
                        .GroupBy(x => new { ID = x.productId, Date = x.createdDate, pName = x.Product.ProductName, categoryName = x.Product.Category.Name })
                        .Select(x => new stockProduct()
                        {
                            ProductName = x.Key.pName,
                            id = x.Key.ID,
                            inQty = x.Sum(y => y.productUpdatedQuantity),
                            Category = x.Key.categoryName,
                            outQty = _context.OrderDetails
                                         .Where(xp => xp.ProductID == x.Key.ID && (xp.Order.createAt <= form && xp.Order.createAt >= to))
                                         .Sum(eq => (int?)eq.Quantity) ?? 0,
                            updateCount = x.Count(),
                            C_Date = x.Key.Date,
                            stockType = search,
                            preQ = (_context.ProductStocks
                                        .Where(xp => xp.productId == x.Key.ID && (xp.createdDate < to))
                                        .Sum(eq => (int?)eq.productUpdatedQuantity) ?? 0) - (_context.OrderDetails
                                                                                                 .Where(xp => xp.ProductID == x.Key.ID && (xp.Order.createAt < to))
                                                                                                 .Sum(eq => (int?)eq.Quantity) ?? 0),
                        }).OrderBy(x => x.id).Skip((page - 1) * pageSize).Take(pageSize)
                        .ToList();
                }




                

                return model;

            }
        }

        public int GetAllStockProductCount(string search, string toDate, string fromDate, int category)
        {
            DateTime form = Convert.ToDateTime(fromDate).Date.AddDays(1);
            DateTime to = Convert.ToDateTime(toDate).Date;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                if (string.IsNullOrEmpty(search) && category == 0)
                {
                    return _context.ProductStocks.Include(x => x.Product).Include(x => x.Product.productOrderDetailList).Include(x => x.Product.Category)
                        .Where(x => (x.createdDate <= form && x.createdDate >= to))
                        .GroupBy(x => new { ID = x.productId, categoryName = x.Product.Category.Name })
                        .Count();
                }
                else if (!string.IsNullOrEmpty(search) && category == 0)
                {
                    return _context.ProductStocks.Include(x => x.Product).Include(x => x.Product.productOrderDetailList).Include(x => x.Product.Category)
                        .Where(x => (x.createdDate <= form && x.createdDate >= to) && x.stockType.ToLower() == search.ToLower())
                        .GroupBy(x => new { ID = x.productId, categoryName = x.Product.Category.Name })
                        .Count();
                }
                else if (string.IsNullOrEmpty(search) && category != 0)
                {
                    return _context.ProductStocks.Include(x => x.Product).Include(x => x.Product.productOrderDetailList).Include(x => x.Product.Category)
                        .Where(x => (x.createdDate <= form && x.createdDate >= to) && x.Product.CategoryId == category)
                        .GroupBy(x => new { ID = x.productId, categoryName = x.Product.Category.Name })
                        .Count();
                }
                else
                {
                    return _context.ProductStocks.Include(x => x.Product).Include(x => x.Product.productOrderDetailList).Include(x => x.Product.Category)
                            .Where(x => (x.createdDate <= form && x.createdDate >= to) && x.stockType.ToLower() == search.ToLower() && x.Product.CategoryId == category)
                        .GroupBy(x => new { ID = x.productId, categoryName = x.Product.Category.Name })
                       .Count();
                }


            }
        }
 


    }
}