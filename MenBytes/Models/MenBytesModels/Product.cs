using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;

namespace MenBytes.Models.MenBytesModels
{
    public class Product
    {
        public int ID { get; set; }
        [StringLength(150,MinimumLength = 3,ErrorMessage = "Name should be between 3 to 50")]
        public string ProductName { get; set; }
        

        public float salePrice { get; set; }

        public float purchasePrice { get; set; }
        
        public int CategoryId { get; set; }


        [DisplayName("Product Discription")]
        public string ProductDescription { get; set; }

        [ForeignKey("CategoryId")]
        public virtual  Category Category { get; set; }

        public int CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Companies Companies { get; set; }

        public bool IsFeatured { get; set; }
        public virtual  List<ProductImage> ProductImages { get; set; }

        public virtual List<ProductComment> ProductComments { get; set; }

        public  bool isDeleted { get; set; }

        public  virtual List<OrderDetail> productOrderDetailList { get; set; }


        public  virtual List<ProductStock> ProductStockList { get; set; }


        public  virtual  List<UserWishList> UserWishList { get; set; }
   
    }
}