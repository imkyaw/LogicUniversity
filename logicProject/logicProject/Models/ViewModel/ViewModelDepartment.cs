using logicProject.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace logicProject.Models.ViewModel
{
    public class ViewModelDepartment
    {
        public CollectionPoint collectionpoint { get; set; }
        public Department department { get; set; }
        public string headname { get; set; }
        public string Repname { get; set; }

    }
}