using logicProject.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace logicProject.Models.ViewModel
{
    public class AuthorizationViewModel
    {
        public string staffName { get; set; }
        public Authorization auth { get; set; }
    }
}