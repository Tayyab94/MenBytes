using System.Data.Entity;
using MenBytes.Models.MenBytesModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MenBytes.Models.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("MenBytesConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Category> Categories { get; set; }

        public  DbSet<Companies> Companieses { get; set; }

        public DbSet<Product> Products { get; set; }

        public  DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<Subscribe> Subscribes { get; set; }

        public DbSet<ContactUs> ContactUse { get; set; }

        public  DbSet<ProductComment> ProductComments { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        public  DbSet<OrderStatus> OrderStatuses { get; set; }

        public DbSet<OrderShipping> OrderShippings { get; set; }

        public DbSet<Order> Orders { get;set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<BlogsComment> BlogsComments { get; set; }

        //public System.Data.Entity.DbSet<MenBytes.Models.ViewModels.OrderViewModel> OrderViewModels { get; set; }


        public DbSet<ProductStock> ProductStocks { get; set; }

        //public System.Data.Entity.DbSet<MenBytes.Models.ViewModels.showAllProducts> showAllProducts { get; set; }


        public DbSet<UserWishList> UserWishlist { get; set; }
    }
}