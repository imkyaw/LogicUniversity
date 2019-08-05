using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace logicProject.Filter
{
    public class StoreAuthorize : System.Web.Http.Filters.ActionFilterAttribute, System.Web.Mvc.IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext c)
        {
            if (string.IsNullOrEmpty((string)c.HttpContext.Session["StoreSession"]))
                c.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Logic" }, { "action", "Store" } });
        }
    }
    public class DeptAuthorize : ActionFilterAttribute, System.Web.Mvc.IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext c)
        {
            if (string.IsNullOrEmpty((string)c.HttpContext.Session["DeptSession"]))
                c.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Logic" }, { "action", "Department" } });
        }
    }
}