using logicProject.Models.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using logicProject.Models.ViewModel;
using logicProject.Models.DAO;

namespace logicProject.Controllers
{
    public class RequestController : Controller
    {
        private LogicEntities db = new LogicEntities();
        // GET: Request
        public ActionResult OrderStatus()
        {
            var request = db.Request.Include(d => d.Department);
            return View(request.ToList());
        }
        public ActionResult RequestForm()
        {
            ViewBag.ProductCategory = new SelectList(db.Product, "Category", "Category");
            ViewBag.Products = new SelectList(db.Product, "Description", "Description");
            return View();
        }
        public JsonResult SearchProduct(string category)
        {
            var products = db.Product.Where(s => s.Category == category).Select(x => x.Description).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
            // return Json(c, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchCategory()
        {
            var products = db.Product.Select(x=>x.Category).ToList().Distinct();
            var modifiedData = products.Select(x => new
            {
                id = x,
                text = x
            });
           
            return Json(modifiedData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateRequest(string products,string qty)
        {
            
            int id = 6;
            if (products != "" && ModelState.IsValid)
            {
                RequestDAO.AddRequest(products, qty, id);
                return RedirectToAction("OrderStatus","Request");
            }
            return RedirectToAction("OrderStatus", "Request");
        }
           
    }
}