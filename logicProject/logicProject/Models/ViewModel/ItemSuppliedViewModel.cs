using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using logicProject.Models.EF;

namespace logicProject.Models.ViewModel
{
    //Coded by Antonio
    public class ItemSuppliedViewModel
    {
        public Product product { get; set; }
        public Supplier supplier { get; set; }
        public SupplierProduct supplierproduct { get; set; }
    }
}
