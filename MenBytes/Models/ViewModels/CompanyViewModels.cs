using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MenBytes.Models.MenBytesModels;

namespace MenBytes.Models.ViewModels
{
    public class CompanyViewModels
    {
    }


    public class SearchCompanyViewModel
    {
        public List<Companies> CompaniesList { get; set; }

        public string SearchItem { get; set; }

        public Pager Pager { get; set; }
    }
}