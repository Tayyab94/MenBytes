using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MenBytes.Models.Context;
using MenBytes.Models.MenBytesModels;

namespace MenBytes.Models.MenBytesHandlers
{
    public class CompaniesHandler
    {

        public List<Companies> GetAllCompanieses()
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                return _context.Companieses.Where(c=>c.isDeleted==false).ToList();
            }
        }

        public List<Companies> GetAllCompanies(string search, int page)
        {
            int pageSize = 3;

            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return _context.Companieses.Where(x => x.CompanyName.ToLower().Contains(search.ToLower()) && x.CompanyName != null)
                        .OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    return _context.Companieses.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }
            }
        }

        //Return the Total Count of the Categories
        public int GetCompaniesCount(string search)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return _context.Companieses.Where(x => x.CompanyName.ToLower().Contains(search.ToLower()) && x.CompanyName != null)
                        .Count();
                }
                else
                {
                    return _context.Companieses.Count();
                }
            }
        }


        public void IsDeletedCompany(int id)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                Companies model = _context.Companieses.Where(x => x.Id == id).SingleOrDefault();


                if (model != null)
                {
                    if (model.isDeleted == true)
                    {
                        model.isDeleted = false;

                    }
                    else
                    {
                        model.isDeleted = true;
                    }


                    _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
        }
    }
}