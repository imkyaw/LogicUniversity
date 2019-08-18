using logicProject.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace logicProject.Models.ViewModel
{
    public class RequestListViewModel
    {
        public Department department { get; set; }
        public DepartmentStaff staff { get; set; }
        public Request requests { get; set; }

        public CollectionPoint collectionPoint { get; set; }

        public string requestDate
        {
            get { return this.requests.ReqDate.ToString("MM/dd/yyyy"); }
            set { this.requestDate = this.requests.ReqDate.ToString("MM/dd/yyyy"); }
        }
    }
}