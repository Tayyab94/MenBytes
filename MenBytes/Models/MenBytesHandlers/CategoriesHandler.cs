using System.Collections.Generic;
using System.Linq;
using MenBytes.Models.Context;
using MenBytes.Models.MenBytesModels;
using System.Data.Entity;

namespace MenBytes.Models.MenBytesHandlers
{
    public class CategoriesHandler
    {
        public void AddCagtegory(Category obj)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _context.Categories.Add(obj);
                _context.SaveChanges();
            }
        }


        public void DeleteCategory(int id)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                var category = _context.Categories.SingleOrDefault(a => a.Id == id);
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }

        public List<Category> GetAllCategoriesForShop()
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                return _context.Categories.Include(x=>x.ProductsList).Where(x => x.IsActive != false).ToList();
            }
        }

        public List<Category> GetAllCategories()
        {
            using (ApplicationDbContext _context= new ApplicationDbContext())
            {
                return _context.Categories.ToList();
            }
        }
        public List<Category> GetAllCategories(string search, int page)
        {
            int pageSize = 3;

            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return _context.Categories.Where(x => x.Name.ToLower().Contains(search.ToLower()) && x.Name != null)
                        .OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    return _context.Categories.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }
            }
        }


        //Return the Total Count of the Categories
        public int GetCategoryCount(string search)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return _context.Categories.Where(x => x.Name.ToLower().Contains(search.ToLower()) && x.Name != null)
                        .Count();
                }
                else
                {
                    return _context.Categories.Count();
                }
            }
        }
    }
}