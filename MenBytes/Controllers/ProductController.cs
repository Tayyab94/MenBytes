using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MenBytes.Models.Context;
using MenBytes.Models.MenBytesHandlers;
using MenBytes.Models.MenBytesModels;
using MenBytes.Models.ViewModels;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Microsoft.Ajax.Utilities;
using System.IO;
using System.Net.Configuration;
using MenBytes.Models;
using Microsoft.AspNet.Identity;

namespace MenBytes.Controllers
{
    public class ProductController : Controller
    {

        //Useing  // GET: Product
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult ShowPro()
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {

                var GetAllProducts = _context.Products.Include(s => s.Category).Include(s => s.Companies)
                    .Include(x => x.ProductImages)
                    .Select(x => new showAllProducts
                    {
                        ID = x.ID,
                        ProductName = x.ProductName,
                        CategoryName = x.Category.Name,
                        CompanyName = x.Companies.CompanyName,
                        IsFeatured = x.IsFeatured,
                        ProductImages = x.ProductImages.Select(c => c.ImagePath).ToList()

                    }).ToList();

                return View(GetAllProducts);
            }

        }

        //THis function show the product whose quantity is less 15
        public ActionResult ProductTable(string search, int? pageNo)
        {
            AllProductViewModel model = new AllProductViewModel();
            ProductHandler handler = new ProductHandler();
            model.SearchItem = search;
            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;

            var totalRecord = handler.GetProductCount(search);
            model.productList = handler.GetAllProducts(search, pageNo.Value);

            if (model.productList != null)
            {
                model.Pager = new Pager(totalRecord, pageNo, 6);
                return PartialView("_ProductTable", model);
            }

            return PartialView("_ProductTable", model);

        }


        // Using This  function is use to create the Product

        [HttpGet]
        [Authorize(Roles = MB.Admin)]
        public ActionResult CreatProduct()
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            NewProductViewModel model = new NewProductViewModel();

            model.Category = _context.Categories.Where(x=>x.IsActive==true).ToList();
            model.Companies = _context.Companieses.Where(x=>x.isDeleted!=true).ToList();
            return View(model);

        }

        //Useing//Useing  //This  function is use to create the Product
        [HttpPost]
        [Authorize(Roles = MB.Admin)]
        public ActionResult CreatProduct(NewProductViewModel obj, FormCollection data)
        {

            ProductStock productStockObj = new ProductStock();
            //long uno;
            //int count;
            ProductHandler productHandler = new ProductHandler();

            Product model = new Product();
            if (!ModelState.IsValid)
            {
                ApplicationDbContext _context = new ApplicationDbContext();


                //obj.Category = _context.Categories.ToList();
                //obj.Companies = _context.Companieses.ToList();


                obj.Category = _context.Categories.Where(x => x.IsActive == true).ToList();
                obj.Companies = _context.Companieses.Where(x => x.isDeleted != true).ToList();

                return View(obj);
            }
            else
            {
                int id = 0;
                using (ApplicationDbContext _context = new ApplicationDbContext())
                {
                    Product modelProduct = new Product();
                    modelProduct.ProductName = obj.ProductName;
                  //  modelProduct.Quantity = obj.Quantity;
                    modelProduct.salePrice = obj.salePrice;
                    modelProduct.purchasePrice = obj.purchasePrice;
                    modelProduct.CategoryId = obj.CategoryId;
                    modelProduct.CompanyId = obj.CompanyId;
                    modelProduct.ProductDescription = obj.ProductDescription;
                    modelProduct.IsFeatured = obj.IsFeatured;
                    modelProduct.isDeleted = false;
                    _context.Products.Add(modelProduct);
                    _context.SaveChanges();
                    id = modelProduct.ID;
                    long uno = DateTime.Now.Ticks;
                    int count = 0;

                    HttpFileCollectionBase files = Request.Files;

                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i];

                        string fName = file.FileName;
                        if (!string.IsNullOrWhiteSpace(file.FileName))
                        {
                            string url = "/Images/ProductImages/" + $"{uno}_{++count}" +
                                         file.FileName.Substring(file.FileName.LastIndexOf("."));
                            string path = Server.MapPath(url);
                            file.SaveAs(path);


                            _context.ProductImages.Add(new ProductImage { ImagePath = url, ProductId = id });


                        }
                        _context.SaveChanges();
                    }

                    int pid = id;

                    productStockObj.productId = pid;
                    productStockObj.productPreviourQuantity = 0;
                    productStockObj.productUpdatedQuantity = obj.Quantity;
                    productStockObj.createdDate = DateTime.Now;
                    productStockObj.stockType = MB.newStock;

                    _context.ProductStocks.Add(productStockObj);
                    _context.SaveChanges();


                }


                return RedirectToAction("showProducts", "Product");

            }

        }
        
        public ActionResult EditUpdatedPrductQuantity(int id)
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            var product = _context.ProductStocks.Where(x => x.productId == id).OrderByDescending(x => x.Id)
                .Select(x => new EditProductQunatiyUPViewModel
                {
                    Id = x.Id,  
                    pro_Id = x.productId,
                    quantity = x.productUpdatedQuantity
                }).FirstOrDefault();


            return PartialView("_EditChangeQunatityProduct", product);
        }


        //This function is update the product Quantiy at first time
        public ActionResult changeProductQuantity(int id)
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            var product = _context.Products.Where(x => x.ID == id).Select(x => new ProductQunatiyUPViewModel
            {
                Id = x.ID
            }).FirstOrDefault();

            //var product = _context.ProductStocks.Where(x => x.productId == id).Select(x => new ProductQunatiyUPViewModel
            //{
            //    Id = x.Id,
            //       quantity = x.productPreviourQuantity
            //}).FirstOrDefault();


            return PartialView("_changeQunatityProduct", product);

        }

        public ActionResult ProductUpdateQty(int id, int qty,bool EditnewQty,int? KeyId)
        {
            bool newstatus =false;bool updateStatus= false;

            int data = 0;
            Product objProduct = new Product();

            ProductStock objProductStock = new ProductStock();

            ApplicationDbContext _context = new ApplicationDbContext();

            objProduct = _context.Products.Where(x => x.ID == id).SingleOrDefault();

            if (objProduct != null)
            {
                if (EditnewQty != true)
                {
                    objProductStock.productId = id;

                    objProductStock.productPreviourQuantity =
                        _context.ProductStocks.Where(x => x.productId == objProduct.ID)
                            .ToList().Sum(x => x.productUpdatedQuantity) - _context.OrderDetails
                            .Where(x => x.ProductID == objProduct.ID).ToList().Sum(x => x.Quantity);
                    objProductStock.productUpdatedQuantity = qty;
                    objProductStock.createdDate = DateTime.Now;
                    objProductStock.stockType = MB.updateStock;

                    _context.ProductStocks.Add(objProductStock);

                  //  objProduct.Quantity += qty;

                  //  _context.Entry(objProduct).State = System.Data.Entity.EntityState.Modified;

                    _context.SaveChanges();

                    newstatus = true;

                    updateStatus = false;

                    data = Convert.ToInt32(objProductStock.productPreviourQuantity + objProductStock.productUpdatedQuantity);
                }
                else
                {
                    int pq = 0;
                     objProductStock = _context.ProductStocks.Where(x => x.Id == KeyId && x.productId == id)
                        .SingleOrDefault();
                     pq = objProductStock.productUpdatedQuantity;
                     objProductStock.productPreviourQuantity =
                        _context.ProductStocks.Where(x => x.productId == objProduct.ID)
                            .ToList().Sum(x => x.productUpdatedQuantity) - _context.OrderDetails
                            .Where(x => x.ProductID == objProduct.ID).ToList().Sum(x => x.Quantity);
                     objProductStock.productUpdatedQuantity = qty;

                    _context.SaveChanges();

                    newstatus = true;
                    if (qty > objProductStock.productUpdatedQuantity)
                    {

                        data = Convert.ToInt32(objProductStock.productPreviourQuantity + objProductStock.productUpdatedQuantity);
                    }
                    else
                    {

                        data = Convert.ToInt32(objProductStock.productPreviourQuantity - (pq-qty));
                    }
                }

            }

            if (newstatus == true )
            {
            
                return Json(new { id = objProduct.ID, quantity = data, Status = newstatus }, JsonRequestBehavior.AllowGet);
            }
            //else if (newstatus == false && updateStatus == true)
            //{

            //    return Json(new { id = objProduct.ID, quantity = objProductStock.productPreviourQuantity, Status = updateStatus }, JsonRequestBehavior.AllowGet);
            //}
            else
            {

                return Json(new { Status = false }, JsonRequestBehavior.AllowGet);
            }

        }


        //Useing
        [Authorize(Roles = MB.Admin)]

        //Edit Product GetFunction

        public ActionResult Edit(int id)
        {
            EditProductViewModel model = new EditProductViewModel();
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                var product = _context.Products.Include(x => x.Category).Include(x => x.Companies)
                    .Include(x=>x.ProductStockList).Include(x=>x.productOrderDetailList)
                    .Include(x => x.ProductImages).Where(x => x.ID == id)
                    .SingleOrDefault();

                model.ID = product.ID;
                model.ProductName = product.ProductName;
                model.IsFeatured = product.IsFeatured;
                model.ProductDescription = product.ProductDescription;
                model.CategoryId = product.Category != null ? product.Category.Id : 0;
                model.Category = _context.Categories.Where(x=>x.IsActive==true).ToList();
                model.CompanyId = product.Companies != null ? product.Companies.Id : 0;
                model.Companies = _context.Companieses.Where(x=>x.isDeleted!=true).ToList();
                model.salePrice = product.salePrice;
                model.purchasePrice = product.purchasePrice;
                model.Quantity = product.ProductStockList.Sum(x => x.productUpdatedQuantity) -
                                 product.productOrderDetailList.Sum(s => s.Quantity);
                model.ProductImages = product.ProductImages;

            }
            return View(model);
        }

        //Useing Edit Product Post Function
        [HttpPost]
        [Authorize(Roles = MB.Admin)]
        public ActionResult Edit(EditProductViewModel obj)
        {
            bool msg = false;
            string[] imgp = new string[3];

            if (ModelState.IsValid)
            {
                try
                {
                    int proId = 0;
                    using (ApplicationDbContext _context = new ApplicationDbContext())
                    {
                        Product model = _context.Products.Include(x => x.Category).Include(x => x.Companies)
                            .Include(x => x.ProductImages)
                            .Where(x => x.ID == obj.ID).SingleOrDefault();
                        proId = model.ID;
                        model.ProductName = obj.ProductName;
                        model.ProductDescription = obj.ProductDescription;
                        model.IsFeatured = obj.IsFeatured;
                        model.CategoryId = obj.CategoryId;
                        model.CompanyId = obj.CompanyId;
                        //   model.Quantity = model.Quantity + obj.Quantity;
                        model.salePrice = obj
                            .salePrice;
                        model.purchasePrice = obj.purchasePrice;

                        _context.Entry(model).State = System.Data.Entity.EntityState.Modified;

                        _context.SaveChanges();


                        var Imagelist = _context.ProductImages.Where(x => x.ProductId == proId).ToList();
                        long uno = DateTime.Now.Ticks;
                        int count = 0;

                        HttpFileCollectionBase files = Request.Files;

                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFileBase file = Request.Files[i];

                            string fName = file.FileName;
                            if (!string.IsNullOrWhiteSpace(file.FileName))
                            {
                                string url = "/Images/ProductImages/" + $"{uno}_{++count}" +
                                             file.FileName.Substring(file.FileName.LastIndexOf("."));
                                string path = Server.MapPath(url);
                                imgp[i] = url;
                                file.SaveAs(path);
                            }

                        }

                        for (int i = 0; i < Imagelist.Count; i++)
                        {
                            //var pImage = _context.ProductImages.Where(x => x.ProductId == proId).SingleOrDefault();

                            string pth = Server.MapPath("~/" + Imagelist[i].ImagePath.ToString());


                            if (pth != null || Imagelist[i].ImagePath.Length != 0)
                            {
                                if (imgp[i] != null)
                                {
                                    if (System.IO.File.Exists(pth))
                                    {
                                        System.IO.File.Delete(pth);
                                    }
                                    Imagelist[i].ImagePath = imgp[i].ToString();
                                }
                                else
                                {
                                    Imagelist[i].ImagePath = Imagelist[i].ImagePath;
                                }
                                _context.Entry(Imagelist[i]).State = System.Data.Entity.EntityState.Modified;
                                _context.SaveChanges();
                            }
                        }

                        msg = true;

                        ViewBag.alertMsg = "you have Successfuly Update your Product";

                        return RedirectToAction("Index", "Product");
                    }

                }
                catch (Exception e)
                {
                    msg = false;
                    return Json(msg, JsonRequestBehavior.AllowGet);

                }
            }
            else
            {
                return RedirectToAction("Edit", "Product", new {id = @obj.ID});
            }
         
            //return View();


        }

        //Useing
        public ActionResult ProductDetail(int id)
        {
            ProductDetailViewModel model = new ProductDetailViewModel();
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                var product = _context.Products.Include(x => x.Category).Include(x => x.Companies)
                    .Include(x => x.ProductImages).Where(x => x.ID == id).Include(x => x.ProductComments).Select(s => new ProductDetailViewModel
                    {
                        ID = s.ID,
                        ProductName = s.ProductName,
                        ProductDescription = s.ProductDescription,
                        Category = s.Category.Name,
                        Companies = s.Companies.CompanyName,
                        salePrice = s.salePrice,
                    //    Quantity = s.Quantity,
                        IsFeatured = s.IsFeatured,
                        ProductImages = s.ProductImages.Select(x => x.ImagePath).ToList(),
                        ProductComments = s.ProductComments,
                    })
                    .SingleOrDefault();
                if (product != null)
                {
                    model.ID = product.ID;
                    model.ProductName = product.ProductName;
                    model.IsFeatured = product.IsFeatured;
                    model.ProductDescription = product.ProductDescription;
                    model.Category = product.Category;
                    model.Companies = product.Companies;
                    model.salePrice = product.salePrice;
                    model.Quantity = product.Quantity;
                    model.ProductImages = product.ProductImages;
                    model.ProductComments = product.ProductComments;

                    model.CategoryWiseProducts = _context.Products.Include(x => x.Category)
                        .Where(s => s.Category.Name.ToLower().Contains(model.Category.ToLower())).Select(s => new showAllProducts
                        {
                            ID = s.ID,
                            ProductName = s.ProductName,
                            ProductDescription = s.ProductDescription,
                            salePrice = s.salePrice,
                            ProductImages = s.ProductImages.Select(i => i.ImagePath).ToList()
                        }).ToList();
                }

                return View(model);
            }
            //return View(product);
        }
        
        //Useing
        [HttpPost]
        [Authorize(Roles = MB.Admin)]
        public ActionResult Delete(int id)
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            Product productObj = new ProductHandler().GetProductById(id);

            foreach (var child in productObj.ProductImages.ToList())
            {
                string path = Request.MapPath("~/" + child.ImagePath);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                //if (path != null || child.ProductId != 0)
                //{
                //    _context.Entry(child).State = System.Data.Entity.EntityState.Deleted;
                //  //  _context.SaveChanges();
                //}
            }

            productObj.ProductImages.RemoveAll(x => x.ProductId == productObj.ID);

            productObj.ProductComments.RemoveAll(x => x.ProductID == productObj.ID);




            //foreach (var child in productObj.ProductComments)
            //{
            //    _context.Entry(child).State = EntityState.Deleted;
            //   // _context.SaveChanges();
            //}

            _context.Entry(productObj).State = System.Data.Entity.EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction("ProductTable");
        }


        [HttpPost]
        public ActionResult productHide(int id)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                var product = _context.Products.Where(x => x.ID.Equals(id)).SingleOrDefault();

                product.isDeleted = true;

                _context.SaveChanges();
            }

            return RedirectToAction("ProductTable", "Product");
        }


        public ActionResult showHideProducts()
        {
            return View();
        }

        public JsonResult ShowAllHideProducts(DataTablesParam param)
        {
            ApplicationDbContext _context = new ApplicationDbContext();


            List<showAllProducts> orderList = new List<showAllProducts>();


            int totalCount = 0;
            int pageNo = 1;

            if (param.iDisplayStart >= param.iDisplayLength)
                pageNo = (param.iDisplayStart / param.iDisplayLength) + 1;


            if (param.sSearch != null)
            {
                totalCount = _context.Products.Include(x => x.ProductStockList).Include(x => x.productOrderDetailList).Include(x => x.Category).Include(x => x.Companies).Where(x =>( x.ProductName.ToLower().Contains(param.sSearch.ToLower()) || x.Companies.CompanyName.ToString().Contains(param.sSearch) || x.Category.Name.ToLower().Contains(param.sSearch.ToLower())) && x.isDeleted==true).Count();
                orderList = _context.Products.Include(x => x.ProductStockList).Include(x => x.productOrderDetailList).Include(x => x.Category).Include(x => x.Companies)
                    
                    .Where(x => (x.ProductName.ToLower().Contains(param.sSearch.ToLower()) || x.Companies.CompanyName.ToString().Contains(param.sSearch) || x.Category.Name.ToLower().Contains(param.sSearch.ToLower())) && x.isDeleted == true)
                    .Select(s => new showAllProducts
                    {
                        ID = s.ID,
                        ProductName = s.ProductName,
                        CategoryName = s.Category.Name,
                        CompanyName = s.Companies.CompanyName,
                        IsFeatured = s.IsFeatured,
                        IsDeleted = s.isDeleted,
                        Quantity = ((s.ProductStockList.Select(x => x.productUpdatedQuantity).Sum(x => (int?)x) ?? 0) - (s.productOrderDetailList.Select(x => x.Quantity).Sum(x => (int?)x) ?? 0))

                        
                        //ProductImages = s.ProductImages.Select(x => x.ImagePath).ToList()
                    })
                    .OrderBy(x => x.ID).Skip((pageNo - 1) * param.iDisplayLength).Take(param.iDisplayLength).ToList();
            }
            else
            {
                totalCount = _context.Products.Include(x => x.ProductStockList).Include(x => x.productOrderDetailList).Include(x => x.Category).Include(x => x.Companies).Where(x => x.isDeleted == true).Count();
                orderList = _context.Products.Include(x => x.ProductStockList).Include(x => x.productOrderDetailList).Include(x => x.Category).Include(x => x.Companies)
                   
                    .Where(x => x.isDeleted == true)

                    .Select(s => new showAllProducts
                    {
                        ID = s.ID,
                        ProductName = s.ProductName,
                        CategoryName = s.Category.Name,
                        CompanyName = s.Companies.CompanyName,
                        IsFeatured = s.IsFeatured,
                        IsDeleted = s.isDeleted,
                        Quantity = ((s.ProductStockList.Select(x => x.productUpdatedQuantity).Sum(x => (int?)x) ?? 0) - (s.productOrderDetailList.Select(x => x.Quantity).Sum(x => (int?)x) ?? 0))


                        //ProductImages = s.ProductImages.Select(x => x.ImagePath).ToList()
                    }).OrderBy(x => x.ID).Skip((pageNo - 1) * param.iDisplayLength).Take(param.iDisplayLength).ToList();

            }




            return Json(new
            {
                aaData = orderList,
                sEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        //This function just do isDeleted=false (Active the product)
        public ActionResult hideToShow(int id)
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            var prduct = _context.Products.Where(x => x.ID == id).SingleOrDefault();

            prduct.isDeleted = false;

            _context.Entry(prduct).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);

        }

        //This gives us All Products including these are hide, less Quantity etc etc
        public ActionResult showProducts()
        {
            
            return View();
        }

        public JsonResult showProductsAl(DataTablesParam param)
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
                    .Where(x => (x.ProductName.ToLower().Contains(param.sSearch.ToLower()) || x.Companies.CompanyName.ToString().Contains(param.sSearch) || x.Category.Name.ToLower().Contains(param.sSearch.ToLower()) || x.IsFeatured.ToString().ToLower().Contains(param.sSearch.ToLower()) || x.isDeleted.ToString().ToLower().Contains(param.sSearch.ToLower())) && x.isDeleted==false)
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
                   
                    .Where(x=>x.isDeleted==false)
                    .Select(s => new showAllProducts
                    {
                        ID = s.ID,
                        ProductName = s.ProductName,
                        CategoryName = s.Category.Name,
                        CompanyName = s.Companies.CompanyName,
                        IsFeatured = s.IsFeatured,
                        IsDeleted = s.isDeleted,
                        Quantity = ((s.ProductStockList.Select(x => x.productUpdatedQuantity).Sum(x => (int?)x) ?? 0) - (s.productOrderDetailList.Select(x => x.Quantity).Sum(x => (int?)x) ?? 0)),

                        productImage=s.ProductImages.Select(x=>x.ImagePath).FirstOrDefault()
                 
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



        public ActionResult AddToWish(int id)
        {
            UserWishList objUserWishList= new UserWishList();
            ApplicationDbContext _context = new ApplicationDbContext();
            if (User.Identity.IsAuthenticated)
            {
                string userID = User.Identity.GetUserId();
                int productID = _context.Products.Where(s => s.ID == id).SingleOrDefault().ID;

                if (_context.UserWishlist.Any(x => x.product_id == productID))
                {
                    return Json(new {status = false, msg = ":) Produt is aleardy Added into Your Wish-List"}, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    UserWishList obj = new UserWishList();

                    obj.user_id = userID;
                    obj.product_id = productID;
                    _context.UserWishlist.Add(obj);
                    _context.SaveChanges();

                    return Json(new { status = true, msg = "Yahooo, Produt is Successfuly Added..!" }, JsonRequestBehavior.AllowGet);
                }
               
            }
            else
            {
                return Json(new { status = false, msg = ":(, Please login into your Account..!" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RemoveToWishlist(int id)
        {
            var st = false;
           ApplicationDbContext _context =new ApplicationDbContext();
           var whishlist = _context.UserWishlist.Where(s => s.Id == id).SingleOrDefault();

           if (whishlist != null)
           {
               _context.UserWishlist.Remove(whishlist);
               _context.SaveChanges();
               st = true;
           }
           
         
            return Json(st,JsonRequestBehavior.AllowGet);
        }

        public ActionResult productDetailAdmin(int id)
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            var product = _context.Products.Include(x => x.ProductStockList).Include(x => x.productOrderDetailList).Include(x=>x.ProductImages)
                .Where(x => x.ID == id).Select(s => new showAllProducts
                {
                    ID = s.ID,
                    ProductName = s.ProductName,
                    CategoryName = s.Category.Name,
                    CompanyName = s.Companies.CompanyName,
                    IsFeatured = s.IsFeatured,
                    IsDeleted = s.isDeleted,
                    Quantity = ((s.ProductStockList.Select(x => x.productUpdatedQuantity).Sum(x => (int?)x) ?? 0) - (s.productOrderDetailList.Select(x => x.Quantity).Sum(x => (int?)x) ?? 0)),

                    ProductImages = s.ProductImages.Select(x => x.ImagePath).ToList(),
                    ProductDescription = s.ProductDescription,
                    salePrice = s.salePrice,
                    purchasePrice = s.purchasePrice

                }).SingleOrDefault();
            return PartialView("_productDetailAdmin",product);
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
                    ProductComment model = new ProductComment();
                    model.ProductID = id;
                    model.Name = userName;
                    model.Comment = comment;
                    model.PostedDate = DateTime.Now;
                    _context.ProductComments.Add(model);
                    _context.SaveChanges();

                    ProductComment obj = _context.ProductComments.OrderBy(x => x.Id).Take(1).SingleOrDefault();
                    int cout = _context.ProductComments.Where(x => x.ProductID == id).Count();
                    return Json(new
                    {
                        model.Name,
                        model.Comment,
                        model.PostedDate,
                        cout
                    }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);


                    return Json(new { ProID, name, productComment }, JsonRequestBehavior.AllowGet);
                }
            }


        }
        
        public ActionResult ProductAdd()
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            NewProductViewModel model = new NewProductViewModel();
            model.Category = _context.Categories.ToList();
            model.Companies = _context.Companieses.ToList();

            return View(model);

        }

        [HttpPost]
        public ActionResult ProductAdd(NewProductViewModel model)
        {
            for (int i = 0; i < model.Images.Length; i++)
            {
                string[] img = model.Images[i].Split(',');
            }
            return View();
        }


        [HttpPost]
        public ActionResult fileUpload()
        {

            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;

                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];

                        string fname;

                        //Check for internet Explorer

                        if (Request.Browser.Browser.ToUpper() == "IE" ||
                            Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });

                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        //GEt the Complete Folder Path Wher you want to Store the Image

                        fname = Path.Combine(Server.MapPath("~/Uploaded/"), fname);

                        file.SaveAs(fname);

                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Json("Error occurred. Error details: " + e.Message);

                }
            }
            else
            {
                return Json("File Uploaded Successfully!", JsonRequestBehavior.AllowGet);
            }

            return View();

        }

    }
}

//public ActionResult ProductTable()
//{
//    AllProducts model = new AllProducts();
//    using (ApplicationDbContext _context = new ApplicationDbContext())
//    {

//        model.pro = _context.Products.Include(x => x.Category).Include(x => x.Companies)
//            .Include(x => x.ProductImages)
//            .Select(s => new showAllProducts
//            {
//                ID = s.ID,
//                ProductName = s.ProductName,
//                CategoryName = s.Category.Name,
//                CompanyName = s.Companies.CompanyName,
//                IsFeatured = s.IsFeatured,
//                Quantity = s.Quantity,
//                ProductImages = s.ProductImages.Select(x => x.ImagePath).ToList()
//            }).OrderBy(x => x.ID).ToList();
//        //model.productList= _context.Products.Include(s => s.Category).Include(s => s.Companies)
//        //    .Include(x => x.ProductImages)
//        //    .Select(x => new Product
//        //    {
//        //        ID = x.ID,
//        //         ProductName = x.ProductName,
//        //          Category = new Category()
//        //          {
//        //               Name = x.Category.Name
//        //          },
//        //          Companies = new Companies()
//        //          {
//        //               CompanyName = x.Companies.CompanyName
//        //          },
//        //          IsFeatured = x.IsFeatured,
//        //          ProductImages = x.ProductImages.ToList()


//        //    }).ToList();

//        //model.productList = (from p in _context.Products.Include(x => x.Category).Include(x => x.Companies).Include(x=>x.ProductImages)
//        //    select new Product
//        //    { ProductName = p.ProductName,
//        //        Category = new Category()
//        //        {
//        //            Name = p.Category.Name
//        //        }
//        //        ,
//        //        Companies = new Companies()
//        //        {
//        //            CompanyName=p.Companies.CompanyName
//        //        }
//        //        ,IsFeatured = p.IsFeatured,
//        //      ProductImages = new List<ProductImage>().ToList()

//        //    }).ToList();


//        return PartialView("_ProductTable", model);
//    }


//}


//[HttpPost]
//public ActionResult CreatProduct(NewProductViewModel obj, FormCollection data)
//{
//    //long uno;
//    //int count;
//    ProductHandler productHandler = new ProductHandler();

//    Product model = new Product();
//    if (!ModelState.IsValid)
//    {
//        ApplicationDbContext _context = new ApplicationDbContext();


//        obj.Category = _context.Categories.ToList();
//        obj.Companies = _context.Companieses.ToList();

//        return View(obj);
//    }
//    else
//    {
//        int id = 0;
//        using (ApplicationDbContext _context = new ApplicationDbContext())
//        {
//            Product modelProduct = new Product();
//            modelProduct.ProductName = obj.ProductName;
//            modelProduct.Quantity = obj.Quantity;
//            modelProduct.salePrice = obj.salePrice;
//            modelProduct.purchasePrice = obj.purchasePrice;
//            modelProduct.CategoryId = obj.CategoryId;
//            modelProduct.CompanyId = obj.CompanyId;

//            _context.Products.Add(modelProduct);
//            _context.SaveChanges();
//            id = modelProduct.ID;
//            long uno = DateTime.Now.Ticks;
//            int count = 0;
//            foreach (string file_Name in Request.Files)
//            {
//                HttpPostedFileBase file = Request.Files[file_Name];
//                if (!string.IsNullOrWhiteSpace(file.FileName))
//                {
//                    string url = "/Images/ProductImages/" + $"{uno}_{++count}" +
//                                 file.FileName.Substring(file.FileName.LastIndexOf("."));
//                    string path = Server.MapPath(url);
//                    file.SaveAs(path);

//                    _context.ProductImages.Add(new ProductImage { ImagePath = url, ProductId = id });


//                }
//            }
//            _context.SaveChanges();

//            return View();

//        }

//    }
//}
