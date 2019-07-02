using MenBytes.Models.MenBytesModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MenBytes.Models.ViewModels
{
    public class ProductViewModel
    {
    }

    public class NewProductViewModel
    {
        public int ID { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Name should be between 3 to 50")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Quantity is Requiread")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Sale Price is Required")]
        public float salePrice { get; set; }


        [Required(ErrorMessage = "Purchase Price is Required")]
        public float purchasePrice { get; set; }

        [Required]
        [DisplayName("Product Description")]
        public string ProductDescription { get; set; }

        public int CategoryId { get; set; }
        
        public  List<Category> Category { get; set; }

        public int CompanyId { get; set; }
        
        public  List<Companies> Companies { get; set; }


        public bool IsFeatured { get; set; }

        public string[] Images { get; set; }
        public  List<ProductImage> ProductImages { get; set; }
    }

    public class EditProductViewModel
    {
        public int ID { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Name should be between 3 to 50")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Quantity is Requiread")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Sale Price is Required")]
        public float salePrice { get; set; }


        [Required(ErrorMessage = "Purchase Price is Required")]
        public float purchasePrice { get; set; }

        [Required]
        [DisplayName("Product Description")]
        public string ProductDescription { get; set; }

        public int CategoryId { get; set; }

        public List<Category> Category { get; set; }

        public int CompanyId { get; set; }

        public List<Companies> Companies { get; set; }


        public bool IsFeatured { get; set; }
        
        public List<ProductImage> ProductImages { get; set; }
    }
    public class showAllProducts
    {
        public int ID { get; set; }
        
        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public float salePrice { get; set; }

        public float purchasePrice { get; set; }
        
        public string ProductDescription { get; set; }

        public string CategoryName { get; set; }
        

        public string  CompanyName { get; set; }
        

        public bool IsFeatured { get; set; }

        public   bool IsDeleted { get; set; }
        public List<string> ProductImages { get; set; }

        public string productImage { get; set; }
    }

    public class AllProducts
    {

        public  List<showAllProducts> pro { get; set; }
        public List<Product> productList { get; set; }
    }
    
    //This Class is used in AdminSide for Show all products and in ProductTableAction
    public class AllProductViewModel
    {
        public List<showAllProducts> productList { get; set; }

        public string SearchItem { get; set; }

        public Pager Pager { get; set; }
    }
    
    //This Class is used for the Detail of the Product, in DetailProductView
    public class ProductDetailViewModel
    {
        public int ID { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public float salePrice { get; set; }
        
        public string ProductDescription { get; set; }

        
        public string Category { get; set; }


        public string Companies { get; set; }


        public bool IsFeatured { get; set; }

        public List<string> ProductImages { get; set; }

        public  List<ProductComment> ProductComments { get; set; }

        public  List<showAllProducts> CategoryWiseProducts { get; set; }

    }


    public class showAllProducts_ClientHome
    {
        public int ID { get; set; }

        public string ProductName { get; set; }

       
        public float salePrice { get; set; }
        
     
        public string ProductImages { get; set; }
    }


    public class productDetailAsminViewModel
    {
        public int ID { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public float salePrice { get; set; }

        public float purchasePrice { get; set; }

        public string ProductDescription { get; set; }

        public string CategoryName { get; set; }


        public string CompanyName { get; set; }


        public bool IsFeatured { get; set; }

        public bool IsDeleted { get; set; }
        public List<string> ProductImages { get; set; }

    }
}