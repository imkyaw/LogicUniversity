using logicProject.Models.DBContext;
using logicProject.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using PagedList;
namespace logicProject.Controllers
{
    public class ProductController : Controller
    {
        private LogicEntities db = new LogicEntities();
        // GET: Product
        public ActionResult GetProductList()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            db.Configuration.LazyLoadingEnabled = false;
            var products = db.Product.ToList();
            int totalrows = products.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                products = products.Where(x => x.Category.ToLower().Contains(searchValue.ToLower()) || x.Description.ToLower().Contains(searchValue.ToLower()) || x.Unit.ToLower().Contains(searchValue.ToLower())|| x.ProductId.ToLower().Contains(searchValue.ToLower())).ToList();
            }
            int totalrowsafterfiltering = products.Count;

            return Json(new { data = products, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
    	}
        public ActionResult ProductCatalog()
        {
            return View();
        }
	}
}