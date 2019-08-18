using logicProject.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace logicProject.Models.ViewModel
{
    public class RequestViewModel
    {
        public RequestDetail requestDetails { get; set; }
        public Request requests { get; set; }
        public Product products { get; set; }
        public string requestedDate
        {
            get { return this.requests.ReqDate.ToString("dd MMMM yyyy"); }
            set { this.requests.ReqDate = DateTime.Parse(value); }
        }

    }
}