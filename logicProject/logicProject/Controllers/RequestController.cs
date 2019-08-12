using logicProject.Models.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using logicProject.Models.ViewModel;
using logicProject.Models.DAO;
using logicProject.Models.EF;
using System.Web.Helpers;
using System.Web.Security;
using logicProject.Filter;

namespace logicProject.Controllers
{
    public class RequestController : Controller
    {
        private LogicEntities db = new LogicEntities();
        // GET: Request
        [Custom(Roles = "head")]
        public ActionResult OrderStatus()
        {
            string a = Session["StoreSession"] as string;
            var request = db.Request.Include(d => d.Department);
            ViewData["header"] = "Order Status";
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

        //View Request Details - Wei Sheng part
        public ActionResult ViewRequestDetails(int requestId)
        {

            var request = from rd in db.RequestDetail
                          join r in db.Request on rd.RequestId equals r.RequestId
                          join p in db.Product on rd.ProductId equals p.ProductId
                          where rd.RequestId==requestId
                          select new RequestViewModel
                          {
                              requestDetails = rd,
                              requests = r,
                              products = p
                          };
            Request req = db.Request.Where(x=>x.RequestId==requestId).SingleOrDefault();
            ViewData["request"] = req;
            return View(request);


        }
        [HttpGet]
        [Custom(Roles = "All")]
        public ActionResult GetLocation()
        {
            return View(db.CollectionPoint);
        }
        [HttpGet]
        public JsonResult GetLocationApi()
        {
            var list = db.CollectionPoint.ToList();
            var modifiedData = list.Select(x => new
            {
                id=x.CollectionPtId,
                name = x.CollectionPt,
                text = x.Description
            });
            return Json(modifiedData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> GetLocationAsync(int locationId)
        {
            //DepartmentStaff ds = Session["DeptStaff"] as DepartmentStaff;
            int StaffId = 6;
            string DeptId = db.DepartmentStaff.Where(x => x.StaffId == StaffId).Select(x => x.DeptId).SingleOrDefault();
            Department d = db.Department.Where(x => x.DeptId == DeptId).SingleOrDefault();
            d.CollectionPt = locationId;
            db.SaveChanges();
            Utility.EmailService e = new Utility.EmailService();
            await e.SendEmailAsync("kyawsithungalay@gmail.com", "hello", "das");

            return Json(new { isok = true, message = "Collection Point is set.",redirect="/Departments/Dashboard" });
        }

        //Wei Sheng Part end here
        //Aprrove Or Reject - Harbinder Part
        [HttpGet]
        public ActionResult RequestApproval(int requestId)
        {
            List<RequestDetail> list = db.RequestDetail.Where(a => a.Request.RequestId.Equals(requestId)).ToList();
            Request req = db.Request.Where(x => x.RequestId == requestId).SingleOrDefault();
            ViewData["request"] = req;
            return View(list);
        }

        [HttpPost]
        public ActionResult RequestApproval(string approve, string reject, string remarks, int id)
        {
            Request rd = db.Request.Find(id);

            if (approve == "Approve")
            {
                rd.Status = approve;
                rd.Remark = remarks;
                db.SaveChanges();
            }
            else if (reject == "Reject")
            {
                rd.Status = reject;
                rd.Remark = remarks;
                db.SaveChanges();
            }
            return RedirectToAction("OrderStatus");
        }

    }


}
