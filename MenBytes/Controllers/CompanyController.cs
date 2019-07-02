using MenBytes.Models.Context;
using MenBytes.Models.MenBytesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MenBytes.Models.MenBytesHandlers;
using MenBytes.Models.ViewModels;
using MenBytes.Models;

namespace MenBytes.Controllers
{
    public class CompanyController : Controller
    {
        // GET: Company
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult CompanyTable(string search, int? pageNo)
        {
            //ApplicationDbContext _context= new ApplicationDbContext();
            SearchCompanyViewModel model = new SearchCompanyViewModel();
            CompaniesHandler companiesHandler = new CompaniesHandler();
            model.SearchItem = search;

            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;

            var totalRecord = companiesHandler.GetCompaniesCount(search);
            model.CompaniesList = companiesHandler.GetAllCompanies(search, pageNo.Value);

            
            if (model.CompaniesList != null)
            {
                model.Pager = new Pager(totalRecord, pageNo, 3);
                return PartialView("_CompanyTable", model);
            }
            return PartialView("_CompanyTable", model);
        }





        [HttpGet]
        public ActionResult CreateCompany()
        {
            Companies model = new Companies();

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateCompany(Companies obj)
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            if (!ModelState.IsValid)
            {
                return View(obj);
            }
            else
            {
                obj.isDeleted = false;
                _context.Companieses.Add(obj);
                _context.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            Companies model = _context.Companieses.Where(x => x.Id.Equals(id)).SingleOrDefault();
            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(Companies model)
        {
            ApplicationDbContext _context= new ApplicationDbContext();

            if (ModelState.IsValid)
            {
                _context.Entry(model).State = System.Data.Entity.EntityState.Modified;

                _context.SaveChanges();

                ViewBag.statusMessage = "Company has been Edit";
                
                //return RedirectToAction("Edit", "Company", new { id = model.Id });

                return View(model);

            }
            else
            {

                return View(model);
            }

        }

        public ActionResult Delete(int id)
        {
            CompaniesHandler companiesHandler = new CompaniesHandler();

            companiesHandler.IsDeletedCompany(id);

            return RedirectToAction("CompanyTable", "Company");
        }
    }
}