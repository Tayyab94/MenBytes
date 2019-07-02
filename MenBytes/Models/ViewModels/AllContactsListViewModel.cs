using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MenBytes.Models.MenBytesModels;

namespace MenBytes.Models.ViewModels
{
    public class AllContactsListViewModel
    {
        public List<ContactUs> ContactUsList
        {
            get; set;

        }
        public string SearchItem { get; set; }

        public Pager Pager { get; set; }
    }
}