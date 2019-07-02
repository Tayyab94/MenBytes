using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MenBytes.Models.Context;
using MenBytes.Models.ViewModels;
using System.Data.Entity;
using MenBytes.Models.MenBytesModels;

namespace MenBytes.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ShowAllOrders(DataTablesParam param)
        {
            ApplicationDbContext _context = new ApplicationDbContext();


            List<OrderViewModel> orderList = new List<OrderViewModel>();


            int totalCount = 0;
            int pageNo = 1;

            if (param.iDisplayStart >= param.iDisplayLength)
                pageNo = (param.iDisplayStart / param.iDisplayLength) + 1;


            if (param.sSearch != null)
            {
                totalCount = _context.Orders.Include(x => x.OrderStatus).Include(x => x.user).Where(x => x.user.user_Name.ToLower().Contains(param.sSearch.ToLower()) || x.createAt.ToString().Contains(param.sSearch) || x.OrderStatus.statusName.ToLower().Contains(param.sSearch.ToLower())).Count();
                orderList = _context.Orders.Include(x => x.OrderStatus).Include(x => x.user)
                    .OrderBy(x => x.Id).Skip((pageNo - 1) * param.iDisplayLength).Take(param.iDisplayLength)
                    .Where(x => x.user.user_Name.ToLower().Contains(param.sSearch.ToLower()) || x.createAt.ToString().Contains(param.sSearch) || x.Id.ToString() == param.sSearch || x.OrderStatus.statusName.ToLower().Contains(param.sSearch.ToLower()))
                    .Select(x => new OrderViewModel
                    {
                        Id = x.Id,
                        userName = x.user.user_Name,
                        orderCreated = x.createAt,
                        orderStatud = x.OrderStatus.statusName,
                        totalBill = x.totalBill

                    }).ToList();
            }
            else
            {
                totalCount = _context.Orders.Count();
                orderList = _context.Orders.Include(x => x.OrderStatus).Include(x => x.user)
                    .OrderBy(x => x.Id).Skip((pageNo - 1) * param.iDisplayLength).Take(param.iDisplayLength)
                    .Select(x => new OrderViewModel
                    {
                        Id = x.Id,
                        userName = x.user.user_Name,
                        orderCreated = x.createAt,
                        orderStatud = x.OrderStatus.statusName,
                        totalBill = x.totalBill
                    }
                    ).ToList();

            }




            return Json(new
            {
                aaData = orderList,
                sEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = totalCount
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DetailOrder(int id)
        {

            ApplicationDbContext _context = new ApplicationDbContext();

            var detail = _context.Orders.Include(a => a.user).Include(x => x.OrderDetail).Include(x => x.OrderShipping)
                .Include(x => x.OrderStatus).Where(x => x.Id == id).SingleOrDefault();


            return View(detail);
        }

        public bool deleteOrder(int id)
        {

            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                var order = _context.Orders.Include(x => x.OrderDetail).Where(x => x.Id == id).SingleOrDefault();
                if (order == null)
                {
                    return false;
                }

                var ship = _context.OrderShippings.Where(x => x.Id == order.OrderShippingID).SingleOrDefault();

                _context.OrderShippings.Remove(ship);


                order.OrderDetail.RemoveAll(c => c.OrderId == order.Id);


                _context.Orders.Remove(order);

                _context.SaveChanges();
            }

            return true;

        }

        public int TotalOrderCount()
        {
            using (ApplicationDbContext _contect = new ApplicationDbContext())
            {

                int total = _contect.Orders.Where(x => x.orderStatusID.Equals(MB.OrderPendding)).Count();

                return total;
            }
        }


        public ActionResult EditOrderStatus(int id)
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            var order = _context.Orders.Include(x => x.OrderStatus).Where(x => x.Id == id).SingleOrDefault();

            ViewBag.StatusList =
                new SelectList(_context.OrderStatuses.ToList(), "Id", "statusName", order.orderStatusID);

            return PartialView("_ChangeStatus", order);
            //return PartialView("_PartialPage1");
            //return PartialView("/Views/Order/_ChangeStatus.cshtml",order);

        }

        public void ChangeOrderStatus(int orderID, int statusValue)
        {

            ApplicationDbContext _context = new ApplicationDbContext();

            var order = _context.Orders.Where(x => x.Id == orderID).SingleOrDefault();

            if (statusValue == MB.OrderComplete)
            {
                order.DeliveryDate = DateTime.Now;

                order.orderStatusID = statusValue;
            }
            else
            {
                order.DeliveryDate = DateTime.Now;

                order.orderStatusID = statusValue;
            }


            _context.SaveChanges();
        }
    }
}