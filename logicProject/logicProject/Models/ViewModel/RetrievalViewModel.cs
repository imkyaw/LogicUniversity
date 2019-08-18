using logicProject.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace logicProject.Models.ViewModel
{
    public class RetrievalViewModel
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public int MaxQuantity { get; set; }
    }
}