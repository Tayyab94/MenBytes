using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MenBytes.Models.Context;
using MenBytes.Models.MenBytesHandlers;
using MenBytes.Models.MenBytesModels;
using MenBytes.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace MenBytes.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            //Init the Cart list
            var cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();


            //check if the cart is empty
            if (cart.Count == 0 && Session["cart"] == null)
            {
                ViewBag.Message = "Your cart is empty.";
                return View();

            }

            // Calculate the total and sve it to the ViewBag 


            decimal total = 0m;
            foreach (var item in cart)
            {
                total +=Convert.ToDecimal(item.Total);
            }
            ViewBag.GrandTotal = total;

            //return a cart_List
            return View(cart);
        }

        [HttpGet]
        public ActionResult MiniCartList()
        {
            //Init the Cart list
            var cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();


            //check if the cart is empty
            if (cart.Count == 0 && Session["cart"] == null)
            {
                ViewBag.Message = "Your cart is empty.";
                return PartialView();

            }

            // Calculate the total and sve it to the ViewBag 


            decimal total = 0m;
            foreach (var item in cart)
            {
                total += Convert.ToDecimal(item.Total);
            }
            ViewBag.GrandTotal = total;

            //return a cart_List
            return PartialView(cart);
        }

        [HttpGet]
        public ActionResult MiniCartListPartial()
        {
            //Init the Cart list
            var cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();


            //check if the cart is empty
            if (cart.Count == 0 && Session["cart"] == null)
            {
                ViewBag.Message = "Your cart is empty.";
                return PartialView();

            }

            // Calculate the total and sve it to the ViewBag 


            decimal total = 0m;
            foreach (var item in cart)
            {
                total += Convert.ToDecimal(item.Total);
            }
            ViewBag.GrandTotal = total;

            //return a cart_List
            return PartialView(cart);
        }

        [HttpGet]
        public ActionResult MiniCartListPartial1()
        {
            //Init the Cart list
            var cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();


            //check if the cart is empty
            if (cart.Count == 0 && Session["cart"] == null)
            {
                ViewBag.Message = "Your cart is empty.";
                return PartialView();

            }

            // Calculate the total and sve it to the ViewBag 


            decimal total = 0m;
            foreach (var item in cart)
            {
                total += Convert.ToDecimal(item.Total);
            }
            ViewBag.GrandTotal = total;

            //return a cart_List
            return Json(cart,JsonRequestBehavior.AllowGet);
        }
        public ActionResult CartPartial()
        {
            //Init CartVM
            CartVM modalCart = new CartVM();

            //Init Qty
            int qty = 0;
            float price = 0;


            //Check for the Cart Session

            if (Session["cart"] != null)
            {

                //Get the Total Qty and Price
                var list = (List<CartVM>)Session["cart"];

                foreach (var item in list)
                {
                    qty += item.Quantity;
                    price += item.Quantity * item.Price;

                }

                modalCart.Quantity = qty;
                modalCart.Price = price;


            }
            else
            {
                //or set the quantity And Price 0

                modalCart.Price = 0;
                modalCart.Quantity = 0;
            }

            //Return PartialView With Modal
            return PartialView(modalCart);
        }

        public ActionResult AddToCartPartial(int id)
        {
            List<CartVM> cartList = Session["cart"] as List<CartVM> ?? new List<CartVM>();

            CartVM model =new CartVM();

            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                Product objProduct = _context.Products.Include(x => x.ProductImages).Where(s => s.ID == id)
                    .SingleOrDefault();


                var productInCart = cartList.FirstOrDefault(s => s.ProductID == id);


                int inStock = _context.ProductStocks.Where(s => s.productId == objProduct.ID).ToList().Sum(x => x.productUpdatedQuantity);
                int outStock = _context.OrderDetails.Where(x => x.ProductID == objProduct.ID).ToList().Sum(x => x.Quantity);

                int totalStock = inStock - outStock;

                //if Not, Add New
                if (productInCart == null)
                {
                    if (totalStock > 0)
                    {
                        cartList.Add(new CartVM()
                        {
                            ProductID = objProduct.ID,
                            ProductName = objProduct.ProductName,
                            Quantity = 1,
                            Price = objProduct.salePrice,

                            ProductImage = objProduct.ProductImages[0].ImagePath
                        });
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                   
                }
                else
                {
                    int q = productInCart.Quantity+1;
                    if (totalStock >= q)
                    {
                        productInCart.Quantity++;
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                   

                }
                //Get Total Quantity And the Price 
                int quntity = 0;
                float pric = 0;

                //Loop THroug the Cart

                foreach (var item in cartList)
                {
                    quntity += item.Quantity;
                    pric += item.Quantity * item.Price;
                }
                //Set the quantity in to the Mdoel
                model.Quantity = quntity;
                model.Price = pric;

                //Save back to Session

                Session["cart"] = cartList;

                //return PartialView With the model 
                return PartialView("/Views/Cart/_addTocartPartial.cshtml", model);
            }
        }

        public void RemoveFormCart(int id)
        {
            List<CartVM> cart=Session["cart"] as List<CartVM>;

            CartVM obj = cart.FirstOrDefault(s => s.ProductID == id);
            cart.Remove(obj);

        }

        public JsonResult GetTotalCartValue(int id,int currentValue, bool increaseOrDecrease)
        {
            ApplicationDbContext _context= new ApplicationDbContext();
            bool trueReturn = false;
            List<CartVM> Items = (List<CartVM>)Session["cart"];
            CartVM c = Items.Where(x => x.ProductID == id).FirstOrDefault();

            var pro = new ApplicationDbContext().Products.Where(x => x.ID == c.ProductID).SingleOrDefault();
            var total = 0;
            string NotificationMessage = null;
            decimal tol = 0m;

            int t_quantities=0;
            int t_Items = 0;

            int inStock = _context.ProductStocks.Where(s => s.productId == c.ProductID).ToList().Sum(x => x.productUpdatedQuantity);
            int outStock = _context.OrderDetails.Where(x => x.ProductID == c.ProductID).ToList().Sum(x => x.Quantity);

            int totalStock = inStock - outStock;
            if (increaseOrDecrease)
            {
                //  if (pro.Quantity >= currentValue)
                if (totalStock >= currentValue)
                {
                    //c.Quantity++;

                    c.Quantity = currentValue;
                    trueReturn = true;


                   // decimal tol = 0m;
                    foreach (var itm in Items)
                    {
                        tol += Convert.ToDecimal(itm.Total);
                    }

                    t_quantities = (Items.Sum(q => q.Quantity) > 0 ? Items.Sum(q => q.Quantity) : 0);
                    t_Items = Items.Count();
                    // Store Needed Data


                    NotificationMessage = "Cart is Updated Successfult!";
                    var result1 = new { qty = c.Quantity, price = c.Price, grandtotal = tol, totalQ = t_quantities, totalItms = t_Items, msg = NotificationMessage, trueReturnMSg = trueReturn };


                    //Return the Json with the Data
                    return Json(result1, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    c.Quantity = c.Quantity;
                    trueReturn = false;

                    foreach (var itm in Items)
                    {
                        tol += Convert.ToDecimal(itm.Total);
                    }

                    t_quantities = (Items.Sum(q => q.Quantity) > 0 ? Items.Sum(q => q.Quantity) : 0);
                    t_Items = Items.Count();
                    // Store Needed Data


                    NotificationMessage = $"You Can not add more Quantity than {currentValue}";
                    var result1 = new { qty = c.Quantity, price = c.Price, grandtotal = tol, totalQ = t_quantities, totalItms = t_Items, msg = NotificationMessage, trueReturnMSg = trueReturn };


                    //Return the Json with the Data
                    return Json(result1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //Increment the Quantity
                if (c.Quantity > 1)
                {
                    //c.Quantity--;
                    c.Quantity = currentValue;
                    

                    NotificationMessage = "Cart is Updated Successfult!";
                    trueReturn = true;
                  
                    foreach (var itm in Items)
                    {
                        tol += Convert.ToDecimal(itm.Total);
                    }

                    t_quantities = (Items.Sum(q => q.Quantity) > 0 ? Items.Sum(q => q.Quantity) : 0);
                    t_Items      = Items.Count();
                    // Store Needed Data

                    var result2 = new { qty = c.Quantity, price = c.Price, grandtotal = tol, totalQ = t_quantities, totalItms = t_Items, msg = NotificationMessage, trueReturnMSg = trueReturn };


                    //Return the Json with the Data
                    return Json(result2, JsonRequestBehavior.AllowGet);
                }
                
            }


        
            foreach (var itm in Items)
            {
                tol += Convert.ToDecimal(itm.Total);
            }

           t_quantities = (Items.Sum(q => q.Quantity) > 0 ? Items.Sum(q => q.Quantity) : 0);
           t_Items = Items.Count();
            // Store Needed Data


            NotificationMessage = "!";
            var result = new { qty = c.Quantity, price = c.Price, grandtotal = tol, totalQ = t_quantities, totalItms = t_Items, msg = NotificationMessage, trueReturnMSg = false };


            //Return the Json with the Data
            return Json(result, JsonRequestBehavior.AllowGet);
            
        }

        [Authorize]
        public async Task<ActionResult> CheckOut()
        {

            decimal usdRate = new decimal();


            try
            {
                string apiUrl = "http://www.apilayer.net/api/live?access_key=aa04833cd9e906575fe7dac2549e65f9";

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        CurrencyRate currencyRate = Newtonsoft.Json.JsonConvert.
                            DeserializeObject<CurrencyRate>(data);
                        usdRate = currencyRate.quotes.USDPKR;
                    }


                }
            }
            catch (Exception e)
            {
                usdRate = 156.15m;
            }
          


            ViewBag.DolRate = usdRate;

            CheckOutViewModel modelObj = new CheckOutViewModel();

           List<CartVM> ListCart = Session["cart"] as List<CartVM>;
            //Init the Cart list
            modelObj.cartList = Session["cart"] as List<CartVM>;
            
            
            //decimal total = 0m;
            //foreach (var item in ListCart)
            //{
            //    total += Convert.ToDecimal(item.Total);
            //}

            modelObj.CartGrandBill = ListCart.Sum(x => x.Total);
            modelObj.CartTotalItems = ListCart.Count();
            modelObj.cartTotalQuantities = ListCart.Sum(s => s.Quantity);
        
            return View(modelObj);
        }

        public ActionResult StoreBillingInfo(string fName,string lName,string address,string city,string country, string email,string phone, string paymentType)
        {

            ShippingVM model= new ShippingVM();

            model.firstName = fName;
            model.lastName = lName;
            model.address = address;
            model.city = city;
            model.country = country;

            model.email = email;
            model.phone = phone;

            model.paymentType = paymentType;

            Session["shippingInfo"] = model;

         
                return Json(paymentType, JsonRequestBehavior.AllowGet);
           

          //  return RedirectToAction("Thankyou","Cart");

        }

      
        [Authorize]
        public ActionResult Thankyou()
        {
            ApplicationDbContext _applicationDbContext= new ApplicationDbContext();
            List<CartVM> cartList = Session["cart"] as List<CartVM>;
            if (cartList == null)
            {

                TempData["cartEmptyMessage"] = "Your Cart is Empty Now";
                return RedirectToAction("Index", "ClientSide");
            }
            Order orderObj = new Order();
            int orderID = 0;
            ShippingVM shipping=Session["shippingInfo"] as ShippingVM;


            var userId = User.Identity.GetUserId();



            
           var userAccount=new ApplicationDbContext().Users.Where(x=>x.Id==userId).SingleOrDefault();
            OrderShippingHanlder orderShippingHanlder= new OrderShippingHanlder();
            int orderShipId = orderShippingHanlder.AddShiping(shipping);

            using (ApplicationDbContext _context =new ApplicationDbContext())
            {
               

                orderObj.orderStatusID = 1;
                orderObj.OrderShippingID = orderShipId;
               orderObj.createAt=DateTime.Now;
               orderObj.paymentType = shipping.paymentType;
               orderObj.userID = userId;
            
               orderObj.totalBill =Convert.ToDouble(cartList.Sum(x => x.Total));

               _context.Orders.Add(orderObj);
               _context.SaveChanges();
               orderID = orderObj.Id;

               OrderDetail objDetail = new OrderDetail();

               foreach (var item in cartList)
               {
                   objDetail.OrderId = orderID;
                   objDetail.ProductID = item.ProductID;
                   objDetail.ProductName = item.ProductName;
                   objDetail.Price = item.Price;
                   objDetail.Quantity = item.Quantity;
                   objDetail.Total = item.Total;

                   _context.OrderDetails.Add(objDetail);
                   _context.SaveChanges();
               }
            }



            ViewBag.ShoppingCartList = cartList.ToList();

            ViewBag.grandTotal = orderObj.totalBill;


            string strBody = "Dear User " + userAccount.user_Name + ",<br> Thank you so much for shopping  its Worth is ."
                             + "<br/><br/>" +orderObj.totalBill + "<br/><br/><br/><b> Best Regards & Thanks</b>: MensBytes <br/>" ;

            MB.sendEmailFunction(userAccount.Email,"Thankyou for shopping",strBody);

            Session["shippingInfo"] = null;
            Session["cart"] = null;
            return View();
        }

        [Authorize]
        [HttpGet]
        
        
        public ActionResult WishList()
        {
            
            return View();
        }


        public JsonResult ShowAllWishlist(DataTablesParam param)
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            
            List<UserWishlistVIewModel> orderList = new List<UserWishlistVIewModel>();
            string userID = User.Identity.GetUserId();


            int totalCount = 0;
            int pageNo = 1;

            if (param.iDisplayStart >= param.iDisplayLength)
                pageNo = (param.iDisplayStart / param.iDisplayLength) + 1;


            if (param.sSearch != null)
            {
                totalCount = _context.UserWishlist.Include(x => x.Users).Include(x=>x.Products).Where(x=>x.user_id==userID && x.Products.ProductName.ToLower().Contains(param.sSearch.ToLower())).Count();
                orderList = _context.UserWishlist.Include(x => x.Users).Include(x => x.Products).Include("Products.ProductImages")
                    .OrderByDescending(x => x.Id).Skip((pageNo - 1) * param.iDisplayLength).Take(param.iDisplayLength)
                    .Where(x => x.user_id == userID && (x.Products.ProductName.ToLower().Contains(param.sSearch.ToLower()))&& x.Products.isDeleted==false)
                    .Select(x => new UserWishlistVIewModel
                    {
                        ID = x.Id,
                        productId = x.product_id,
                        productName = x.Products.ProductName,
                        Image = x.Products.ProductImages.Select(xp => xp.ImagePath).FirstOrDefault()
                    }).ToList();
            }
            else
            {
                totalCount = _context.UserWishlist.Where(x=>x.user_id==userID).Count();
                orderList = _context.UserWishlist.Include(x => x.Users).Include(x => x.Products).Include("Products.ProductImages")
                    .OrderByDescending(x => x.Id).Skip((pageNo - 1) * param.iDisplayLength).Take(param.iDisplayLength)
                    .Where(x => x.user_id == userID && x.Products.isDeleted==false)
                    .Select(x => new UserWishlistVIewModel
                    {
                        ID = x.Id,
                     productId = x.product_id,
                     productName = x.Products.ProductName,
                     Image = x.Products.ProductImages.Select(xp=>xp.ImagePath).FirstOrDefault()
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

    }

    public class CurrencyRate
    {
        public CurrencyType quotes { get; set; }
    }

    public class CurrencyType
    {
        public decimal USDPKR { get; set; }
    }

}