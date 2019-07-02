using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MenBytes.Models.MenBytesModels;

namespace MenBytes.Models.ViewModels
{
    public class StockViewMdoel
    {
    }

    public class PurchaseStockViewModel
    {

        public List<stockProduct> productList { get; set; }
        public  string toDate { get; set; }

        public string frmDate { get; set; }
        public string SearchItem { get; set; }

        public int CategoryId
        {
            get;
            set;
        }

        public  List<Category> CategoriesList { get; set; }
        public Pager Pager { get; set; }
    }

    public class stockProduct
    {
        public int id { get; set; }

        public string ProductName { get; set; }

        public string Category { get; set; }
        public int openingQty { get; set; }
        public int inQty { get; set; }

        public int outQty { get; set; }

        public int totalQty { get; set; }

        public DateTime C_Date { get; set; }


        public int preQ { get; set; }

        public  int updateCount { get; set; }

      public  string stockType { get; set; }
    }

    
}