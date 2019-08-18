using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace logicProject.Models.ViewModel
{
    public class ProductViewModel
    {   //Written by Udaya
        public string ProductId { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public int Qty { get; set; }
    }
}