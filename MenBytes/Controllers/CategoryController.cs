using MenBytes.Models.MenBytesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using MenBytes.Models;
using MenBytes.Models.Context;
using MenBytes.Models.MenBytesHandlers;
using MenBytes.Models.ViewModels;

namespace MenBytes.Controllers
{
    [Authorize]
    [Authorize(Roles = MB.Admin)]
    public class CategoryController : Controller
    {
        // GET: Category
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CategoryTable(string search, int? pageNo)
        {
            //ApplicationDbContext _context= new ApplicationDbContext();
            SearchCategoryViewModel model= new SearchCategoryViewModel();
            CategoriesHandler categoriesHandler= new CategoriesHandler();
            model.SearchItem = search;
              
            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;

            var totalRecord = categoriesHandler.GetCategoryCount(search);
            model.Categories = categoriesHandler.GetAllCategories(search, pageNo.Value);

            //model.Categories = _context.Categories.ToList();

            if (model.Categories != null)
            {
                model.Pager=new Pager(totalRecord,pageNo,3);
                return PartialView("_categoryTable", model);
            }
            return PartialView("_categoryTable",model);
        }

        //[HttpGet]
        //public ActionResult Create()
        //{
        //    NewCategoryViewModel model= new NewCategoryViewModel();
        //    return PartialView(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Category obj)
        //{
        //    ApplicationDbContext _context =new ApplicationDbContext();
        //    if (!ModelState.IsValid)
        //    {
        //        return RedirectToAction("Index", "Category");
        //    }
        //    else
        //    {
        //        _context.Categories.Add(obj);
        //        _context.SaveChanges();
        //    }

        //    return RedirectToAction("CategoryTable");
        //}



        [HttpGet]
        public ActionResult CreateNew()
        {
            //Category model = new Category();
            //return View(model);

            return View();
        }

        [HttpPost]
        public ActionResult CreateNew(NewCategoryViewModel  obj)
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("","Please fill the mendatory field");
                return View(obj);
            }
            else
            {
                Category model = new Category();
                model.Name = obj.Name;
                //model.IsActive = obj.IsActive;
                model.IsActive = true;


                //_context.Categories.Add(model);
                //_context.SaveChanges();

                //CategoriesHandler categoriesHandler = new CategoriesHandler();
                //categoriesHandler.AddCagtegory(model);
                //_context.Categories.Add(obj);
                //_context.SaveChanges();

                //               List<Category> CategoriesList=_context.Categories.ToList()
                //;


                List<object> list = new List<object>();
                list.Add(model.Name);
                list.Add(model.IsActive);

                object[] allitems = list.ToArray();

                int output = _context.Database.ExecuteSqlCommand("Insert into Categories (name,isActive) values(@p0,@p1)", allitems);
                if(output>0)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }


                return Json(true, JsonRequestBehavior.AllowGet);
            }

            //return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ApplicationDbContext _context= new ApplicationDbContext();
            // var Category = _context.Categories.SingleOrDefault(x => x.Id.Equals(id));

            //var Category = _context.Categories.SqlQuery("Select * from Categories where Id=@P0", id).SingleOrDefault();


            var Category = _context.Categories.SqlQuery("Select * from Categories where Id={0}",id).SingleOrDefault();


            if (Category == null)
                return HttpNotFound();

            EditCategoryViewModel model=new EditCategoryViewModel();
            model.Id = Category.Id;
            model.Name = Category.Name;
            model.IsActive = Category.IsActive;

            return View(model);

        }

        [HttpPost]
        public ActionResult Edit(EditCategoryViewModel model)
        {
            bool msg = true;
            ApplicationDbContext _context= new ApplicationDbContext();

            var Category = _context.Categories.FirstOrDefault(s => s.Id == model.Id);
            if (Category == null)
            {
                msg = false;
            }
            else
            {
                Category.Name = model.Name;
                Category.IsActive = model.IsActive;

                _context.Entry(Category).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();

                return Json(msg, JsonRequestBehavior.AllowGet);
            }
         
            //ApplicationDbContext _context = new ApplicationDbContext();
            //var Category = _context.Categories.SingleOrDefault(x => x.Id.Equals(id));
            //if (Category == null)
            //    return HttpNotFound();

            //EditCategoryViewModel model = new EditCategoryViewModel();
            //model.Id = Category.Id;
            //model.Name = Category.Name;
            //model.IsActive = Category.IsActive;

            return View(model);

        }



        [HttpPost]
        public ActionResult Delete(int id)
        {
            //CategoriesHandler categoriesHandler= new CategoriesHandler();
            //categoriesHandler.DeleteCategory(id);

            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                Category model = _context.Categories.Where(x => x.Id == id).SingleOrDefault();


                if (model != null)
                {
                    if (model.IsActive == true)
                    {
                        model.IsActive = false;

                    }
                    else
                    {
                        model.IsActive = true;
                    }


                    _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("CategoryTable");
        }


    }
}