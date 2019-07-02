using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MenBytes.Models.MenBytesModels;

namespace MenBytes.Models.ViewModels
{
    public class OrderViewModel
    {
        public int  Id { get; set; }
        public string userName { get; set; }

        public DateTime orderCreated { get; set; }


        public  string orderDate { get; set; }
        public string paymentType { get; set; }

        public string orderStatud { get; set; }

        public double totalBill { get; set; }
    }


    public class OrderRecordViewModel
    {
        public int Id { get; set; }
        public string userName { get; set; }

        public string paymentType { get; set; }

        public string orderStatud { get; set; }

        public double totalBill { get; set; }

        public DateTime C_Date { get; set; }
    }

    public class SaleStockViewModel
    {

        public List<OrderRecordViewModel> OrderRecord { get; set; }
        public string toDate { get; set; }

        public string frmDate { get; set; }
        public string SearchItem { get; set; }

        public int statusId
        {
            get;
            set;
        }


        public List<OrderStatus> orderStatusList { get; set; }
        public Pager Pager { get; set; }
    }


}