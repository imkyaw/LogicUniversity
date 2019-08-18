using logicProject.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace logicProject.Models.ViewModel
{
    public class RequestItemsViewModel
    {
        public RequestDetail requestDetail { get; set; }

        public Product product { get; set; }

        //public SupplierProduct supplierProduct { get; set; }
    }
}