using MenBytes.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MenBytes.Models.MenBytesModels;

namespace MenBytes.Models.MenBytesHandlers
{
    public class ContactHanlder
    {
        public List<ContactUs> GetAllContactMessage(string search, int page)
        { /*AllProductViewModel model= new AllProductViewModel();*/
            int pageSize = 6;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {

                if (!string.IsNullOrEmpty(search))
                {
                    return _context.ContactUse
                        .Where(x => ( x.fullName.ToLower().Contains(search.ToLower()) || x.Subject.ToLower().Contains(search.ToLower()) || x.Phone.ToLower().Contains(search.ToLower()) || x.Email.ToLower().Contains(search.ToLower()))  && x.IsRead == false)
                        //.Select(s => new ContactUs
                        //{
                        //    ID = s.ID,
                        //    fullName = s.fullName,
                        //    Email = s.Email,
                        //    IsRead = s.IsRead,
                        //    Message = s.Message,
                        //    Phone = s.Phone,
                        //    Subject = s.Subject
                        //})
                        .OrderBy(x => x.ID).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    return _context.ContactUse
                        .Where(x => x.IsRead == false)
                   
                        .OrderBy(x => x.ID).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    
                }


            }
        }

        public int GetContactMessageCount(string search)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return _context.ContactUse.Where(x => (x.fullName.ToLower().Contains(search.ToLower()) || x.Subject.ToLower().Contains(search.ToLower()) || x.Phone.ToLower().Contains(search.ToLower()) || x.Email.ToLower().Contains(search.ToLower())) && x.IsRead == false)
                        .Count();
                }
                else
                {
                    return _context.ContactUse.Where(x => x.IsRead == false).Count();
                }
            }
        }
    }
}