using MenBytes.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MenBytes.Models.Context;
using MenBytes.Models.MenBytesModels;
using  System.Data.Entity;

namespace MenBytes.Models.MenBytesHandlers
{
    public class OrderShippingHanlder
    {
        public int AddShiping(ShippingVM obj)
        {
            OrderShipping model = new OrderShipping();
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                model.firstName = obj.firstName;
                model.lastName = obj.lastName;
                model.address = obj.address;
                model.email = obj.email;
                model.phone = obj.phone;
                model.City = obj.city;
                model.Country = obj.country;
                _context.OrderShippings.Add(model);
                _context.SaveChanges();
            }

            return model.Id;
        }

        public List<OrderRecordViewModel> GetAllSaleProduct(string paymenttype, string toDate, string fromDate, int statusId, int page)
        { /*AllProductViewModel model= new AllProductViewModel();*/
            int pageSize = 10;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {

                List<OrderRecordViewModel> model = new List<OrderRecordViewModel>();
                DateTime form = Convert.ToDateTime(fromDate).Date.AddDays(1);
                DateTime to = Convert.ToDateTime(toDate).Date;


                if (string.IsNullOrEmpty(paymenttype) && statusId == 0)
                {
                    model = _context.Orders.Include(s => s.OrderStatus)
                        .Where(x => (x.createAt <= form && x.createAt >= to))
                        .Select(x => new OrderRecordViewModel
                        {
                            Id = x.Id,
                            orderStatud = x.OrderStatus.statusName,
                            paymentType = x.paymentType,
                            totalBill = x.totalBill,
                            userName = x.user.user_Name,
                            C_Date = x.createAt
                        }).OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();

                }
                else if (!string.IsNullOrEmpty(paymenttype) && statusId == 0)
                {
                    model = _context.Orders.Include(s => s.OrderStatus)
                        .Where(x => (x.createAt <= form && x.createAt >= to) && x.paymentType.Equals(paymenttype))
                        .Select(x => new OrderRecordViewModel
                        {
                            Id = x.Id,
                            orderStatud = x.OrderStatus.statusName,
                            paymentType = x.paymentType,
                            totalBill = x.totalBill,
                            userName = x.user.user_Name,
                            C_Date = x.createAt
                        }).OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }
                else if (string.IsNullOrEmpty(paymenttype) && statusId != 0)
                {
                    if (statusId == 3)
                    {
                    
                        model = _context.Orders.Include(s => s.OrderStatus).Where(x => (x.DeliveryDate <= form && x.DeliveryDate >= to) && x.orderStatusID.Equals(statusId))
                            .Select(x => new OrderRecordViewModel
                            {
                                Id = x.Id,
                                orderStatud = x.OrderStatus.statusName,
                                paymentType = x.paymentType,
                                totalBill = x.totalBill,
                                userName = x.user.user_Name,
                                C_Date = x.DeliveryDate.Value
                            }).OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();


                    }
                    else
                    {
                        model = _context.Orders.Include(s => s.OrderStatus)
                            .Where(x => (x.createAt <= form && x.createAt >= to) && x.orderStatusID.Equals(statusId))
                            .Select(x => new OrderRecordViewModel
                            {
                                Id = x.Id,
                                orderStatud = x.OrderStatus.statusName,
                                paymentType = x.paymentType,
                                totalBill = x.totalBill,
                                userName = x.user.user_Name,
                                C_Date = x.createAt
                            }).OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    }


                }
                else
                {

                    if (statusId == 3)
                    {
                        model = _context.Orders.Include(s => s.OrderStatus).Where(x => (x.DeliveryDate <= form && x.DeliveryDate >= to) && x.orderStatusID.Equals(statusId) && x.paymentType.Equals(paymenttype))
                            .Select(x => new OrderRecordViewModel
                            {
                                Id = x.Id,
                                orderStatud = x.OrderStatus.statusName,
                                paymentType = x.paymentType,
                                totalBill = x.totalBill,
                                userName = x.user.user_Name,
                                C_Date = x.DeliveryDate.Value
                            }).OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    }
                    else
                    {
                        model = _context.Orders.Include(s => s.OrderStatus)
                            .Where(x => (x.createAt <= form && x.createAt >= to) && x.orderStatusID.Equals(statusId) && x.paymentType.Equals(paymenttype))
                            .Select(x => new OrderRecordViewModel
                            {
                                Id = x.Id,
                                orderStatud = x.OrderStatus.statusName,
                                paymentType = x.paymentType,
                                totalBill = x.totalBill,
                                userName = x.user.user_Name,
                                C_Date = x.createAt
                            }).OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();

                    }
                    
                }



                return model;

            }
        }

        public int GetAllSaleProductCount(string paymenttype, string toDate, string fromDate, int statusId)
        {
            DateTime form = Convert.ToDateTime(fromDate).Date.AddDays(1);
            DateTime to = Convert.ToDateTime(toDate).Date;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                if (string.IsNullOrEmpty(paymenttype) && statusId == 0)
                {
                    return _context.Orders.Include(s => s.OrderStatus)
                        .Where(x => (x.createAt <= form && x.createAt >= to))
                        .Count();
                }
                else if (!string.IsNullOrEmpty(paymenttype) && statusId == 0)
                {
                    return _context.Orders.Include(s => s.OrderStatus)
                        .Where(x => (x.createAt <= form && x.createAt >= to) && x.paymentType.Equals(paymenttype))
                        .Count();
                }
                else if (string.IsNullOrEmpty(paymenttype) && statusId != 0)
                {
                    if (statusId == 3)
                    {
                        return _context.Orders.Include(s => s.OrderStatus).Where(x =>
                                (x.DeliveryDate <= form && x.DeliveryDate >= to) && x.orderStatusID.Equals(statusId))
                            .Count();
                    }
                    else
                    {
                        return _context.Orders.Include(s => s.OrderStatus)
                            .Where(x => (x.createAt <= form && x.createAt >= to) && x.orderStatusID.Equals(statusId))
                            .Count();
                    }



                }
                else
                {
                    if (statusId == 3)
                    {
                        return _context.Orders.Include(s => s.OrderStatus).Where(x =>
                                (x.DeliveryDate <= form && x.DeliveryDate >= to) && x.orderStatusID.Equals(statusId) &&
                                x.paymentType.Equals(paymenttype))
                            .Count();
                    }
                    else
                    {
                        return _context.Orders.Include(s => s.OrderStatus)
                            .Where(x => (x.createAt <= form && x.createAt >= to) && x.orderStatusID.Equals(statusId) &&
                                        x.paymentType.Equals(paymenttype))
                            .Count();
                    }


                }


            }
        }

        //public List<OrderRecordViewModel> GetAllSaleProduct(string paymenttype, string toDate, string fromDate, int statusId, int page)
        //{ /*AllProductViewModel model= new AllProductViewModel();*/
        //    int pageSize = 3;
        //    using (ApplicationDbContext _context = new ApplicationDbContext())
        //    {

        //        List<OrderRecordViewModel> model = new List<OrderRecordViewModel>();
        //        DateTime form = Convert.ToDateTime(fromDate).Date.AddDays(1);
        //        DateTime to = Convert.ToDateTime(toDate).Date;


        //        if (string.IsNullOrEmpty(paymenttype) && statusId == 0)
        //        {
        //            model = _context.Orders.Include(s => s.OrderStatus)
        //                .Where(x => (x.createAt <= form && x.createAt >= to))
        //                .Select(x => new OrderRecordViewModel
        //                {
        //                    Id = x.Id,
        //                    orderStatud = x.OrderStatus.statusName,
        //                    paymentType = x.paymentType,
        //                    totalBill = x.totalBill,
        //                    userName = x.user.user_Name
        //                }).OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                 
        //        }
        //        else if (!string.IsNullOrEmpty(paymenttype) && statusId == 0)
        //        {
        //            model = _context.Orders.Include(s => s.OrderStatus)
        //                .Where(x => (x.createAt <= form && x.createAt >= to) && x.paymentType.Equals(paymenttype))
        //                .Select(x => new OrderRecordViewModel
        //                {
        //                    Id = x.Id,
        //                    orderStatud = x.OrderStatus.statusName,
        //                    paymentType = x.paymentType,
        //                    totalBill = x.totalBill,
        //                    userName = x.user.user_Name
        //                }).OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //        }
        //        else if (string.IsNullOrEmpty(paymenttype) && statusId != 0)
        //        {
        //            if (statusId == 1)
        //            {
        //                model = _context.Orders.Include(s=>s.OrderStatus).Where(x => (x.createAt <= form && x.createAt >= to) && x.orderStatusID.Equals(statusId))
        //                    .Select(x => new OrderRecordViewModel
        //                    {
        //                        Id = x.Id,
        //                        orderStatud = x.OrderStatus.statusName,
        //                        paymentType = x.paymentType,
        //                        totalBill = x.totalBill,
        //                        userName = x.user.user_Name
        //                    }).OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //            }
        //            else
        //            {
        //                model = _context.Orders.Include(s => s.OrderStatus)
        //                    .Where(x => (x.DeliveryDate <= form && x.DeliveryDate >= to) && x.orderStatusID.Equals(statusId))
        //                    .Select(x => new OrderRecordViewModel
        //                    {
        //                        Id = x.Id,
        //                        orderStatud = x.OrderStatus.statusName,
        //                        paymentType = x.paymentType,
        //                        totalBill = x.totalBill,
        //                        userName = x.user.user_Name
        //                    }).OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //            }

                   
        //        }
        //        else
        //        {

        //            if (statusId == 1)
        //            {
        //                model = _context.Orders.Include(s => s.OrderStatus).Where(x => (x.createAt <= form && x.createAt >= to) && x.orderStatusID.Equals(statusId) && x.paymentType.Equals(paymenttype))
        //                    .Select(x => new OrderRecordViewModel
        //                    {
        //                        Id = x.Id,
        //                        orderStatud = x.OrderStatus.statusName,
        //                        paymentType = x.paymentType,
        //                        totalBill = x.totalBill,
        //                        userName = x.user.user_Name
        //                    }).OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //            }
        //            else
        //            {
        //                model = _context.Orders.Include(s => s.OrderStatus)
        //                    .Where(x => (x.DeliveryDate <= form && x.DeliveryDate >= to) && x.orderStatusID.Equals(statusId) && x.paymentType.Equals(paymenttype))
        //                    .Select(x => new OrderRecordViewModel
        //                    {
        //                        Id = x.Id,
        //                        orderStatud = x.OrderStatus.statusName,
        //                        paymentType = x.paymentType,
        //                        totalBill = x.totalBill,
        //                        userName = x.user.user_Name
        //                    }).OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //            }


        //            //model = _context.Orders.Include(c=>c.OrderStatus)
        //            //    .Where(x => (x.DeliveryDate <= form && x.DeliveryDate >= to) && x.orderStatusID.Equals(statusId) && x.paymentType.Equals(paymenttype))
        //            //    .Select(x => new OrderRecordViewModel
        //            //    {
        //            //        Id = x.Id,
        //            //        orderStatud = x.OrderStatus.statusName,
        //            //        paymentType = x.paymentType,
        //            //        totalBill = x.totalBill,
        //            //        userName = x.user.user_Name
        //            //    }).OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //        }



        //        return model;

        //    }
        //}

        //public int GetAllSaleProductCount(string paymenttype, string toDate, string fromDate, int statusId)
        //{
        //    DateTime form = Convert.ToDateTime(fromDate).Date.AddDays(1);
        //    DateTime to = Convert.ToDateTime(toDate).Date;
        //    using (ApplicationDbContext _context = new ApplicationDbContext())
        //    {
        //        if (string.IsNullOrEmpty(paymenttype) && statusId == 0)
        //        {
        //            return _context.Orders.Include(s => s.OrderStatus)
        //                .Where(x => (x.createAt <= form && x.createAt >= to))
        //                .Count();
        //        }
        //        else if (!string.IsNullOrEmpty(paymenttype) && statusId == 0)
        //        {
        //            return _context.Orders.Include(s => s.OrderStatus)
        //                .Where(x => (x.createAt <= form && x.createAt >= to) && x.paymentType.Equals(paymenttype))
        //                .Count();
        //        }
        //        else if (string.IsNullOrEmpty(paymenttype) && statusId != 0)
        //        {
        //            if (statusId == 1)
        //            {
        //                return _context.Orders.Include(s => s.OrderStatus).Where(x =>
        //                        (x.DeliveryDate <= form && x.DeliveryDate >= to) && x.orderStatusID.Equals(statusId))
        //                    .Count();
        //            }
        //            else
        //            {
        //                return _context.Orders.Include(s => s.OrderStatus)
        //                    .Where(x => (x.createAt <= form && x.createAt >= to) && x.orderStatusID.Equals(statusId))
        //                    .Count();
        //            }



        //            //if (statusId == 1)
        //            //{
        //            //    return _context.Orders.Include(s => s.OrderStatus).Where(x =>
        //            //            (x.createAt <= form && x.createAt >= to) && x.orderStatusID.Equals(statusId))
        //            //        .Count();
        //            //}
        //            //else
        //            //{
        //            //    return _context.Orders.Include(s => s.OrderStatus).Where(x =>
        //            //            (x.DeliveryDate <= form && x.DeliveryDate >= to) && x.orderStatusID.Equals(statusId))
        //            //        .Count();
        //            //}
        //        }
        //        else
        //        {
        //            if (statusId == 1)
        //            {
        //                return _context.Orders.Include(s => s.OrderStatus).Where(x =>
        //                        (x.DeliveryDate <= form && x.DeliveryDate >= to) && x.orderStatusID.Equals(statusId) &&
        //                        x.paymentType.Equals(paymenttype))
        //                    .Count();
        //            }
        //            else
        //            {
        //                return _context.Orders.Include(s => s.OrderStatus)
        //                    .Where(x => (x.createAt <= form && x.createAt >= to) && x.orderStatusID.Equals(statusId) &&
        //                                x.paymentType.Equals(paymenttype))
        //                    .Count();
        //            }

                 
        //        }


        //    }
        //}
    }
}