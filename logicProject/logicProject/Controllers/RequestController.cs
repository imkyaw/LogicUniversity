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
        [Custom(Roles = "All")]
        public ActionResult OrderStatus()
        {
            DepartmentStaff a = Session["DeptStaff"] as DepartmentStaff;
            String temp = Convert.ToBoolean(Session["TempHead"]).ToString();
            String check= Convert.ToBoolean(Session["CheckHead"]).ToString();
            ViewData["ApproveOrNot"] = null;
            var request = db.Request.Include(d => d.Department);
            if (a.StaffType == "head")
            {
                ViewData["ApproveOrNot"] = true;
            }else if(a.StaffType!="head" && temp=="True")
            {
                ViewData["ApproveOrNot"] = true;
            }
            else
            {
                request = request.Where(x => x.StaffId == a.StaffId).Include(d=>d.Department);
            }
            if (a.StaffType == "head" && check == "True")
            {
                ViewData["ApproveOrNot"] = null;
            }
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
        public ActionResult CreateRequest(string products,string qty,string save)
        {
            DepartmentStaff a = Session["DeptStaff"] as DepartmentStaff;
            if (products != "" && ModelState.IsValid)
            {
                RequestDAO.AddRequest(products, qty, a.StaffId,save);
                return RedirectToAction("OrderStatus","Request");
            }
            return RedirectToAction("OrderStatus", "Request");
        }

        //Save Request
        public ActionResult ViewFavRequest()
        {
            DepartmentStaff a = Session["DeptStaff"] as DepartmentStaff;
            List<Request> list = db.Request.Where(x => x.FavRequest == true).ToList();
            return View();
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
            String name = db.DepartmentStaff.Where(x => x.StaffId == req.StaffId).Select(x => x.StaffName).FirstOrDefault();
            ViewData["staff"] = name;
            ViewData["request"] = req;
            return View(request);
        }

        [HttpGet]
        [Custom(Roles = "All")]
        public ActionResult GetLocation()
        {
            DepartmentStaff a = Session["DeptStaff"] as DepartmentStaff;
            var c = db.Department.Where(x => x.DeptId == a.DeptId).FirstOrDefault();
            ViewData["collectionPoint"] = c.CollectionPt;
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
        public ActionResult GetLocationAsync(int locationId)
        {
            DepartmentStaff ds = Session["DeptStaff"] as DepartmentStaff;
            string DeptId = ds.DeptId;
            Department d = db.Department.Where(x => x.DeptId == DeptId).SingleOrDefault();
            d.CollectionPt = locationId;
            db.SaveChanges();
            List<String> list = db.StoreStaff.Select(x=>x.StaffEmail).ToList();
            string message = Utility.EmailBody.CollectionSubject + "\n" + "By " + d.DeptName;
            Utility.EmailService.SendEmail(list,Utility.EmailBody.CollectionSubject,message);
            //await e.SendEmailAsync("kyawsithungalay@gmail.com", "hello", "das");
            
            return Json(new { isok = true, message = "Collection Point is set.",redirect="/Departments/Dashboard",locationId=locationId });
        }
        [HttpPost]
        public ActionResult PostLocationApi(int locationId,int staffId)
        {
            //DepartmentStaff ds = Session["DeptStaff"] as DepartmentStaff;
            DepartmentStaff ds = db.DepartmentStaff.Where(x => x.StaffId == staffId).FirstOrDefault();
            string DeptId = ds.DeptId;
            Department d = db.Department.Where(x => x.DeptId == DeptId).SingleOrDefault();
            d.CollectionPt = locationId;
            db.SaveChanges();
            List<String> list = db.StoreStaff.Select(x => x.StaffEmail).ToList();
            string message = Utility.EmailBody.CollectionBody + "\n" + "By " + d.DeptName;
            Utility.EmailService.SendEmail(list, Utility.EmailBody.CollectionSubject, message);
            
            return Json(new { isok = true, message = "Collection Point is set." });
        }
        //Wei Sheng Part end here
        //Aprrove Or Reject - Harbinder Part
        [HttpGet]
        public ActionResult RequestApproval(int requestId)
        {
            List<RequestDetail> list = db.RequestDetail.Where(a => a.Request.RequestId.Equals(requestId)).ToList();
            Request req = db.Request.Where(x => x.RequestId == requestId).SingleOrDefault();
            String name = db.DepartmentStaff.Where(x => x.StaffId == req.StaffId).Select(x => x.StaffName).FirstOrDefault();
            ViewData["staff"] = name;
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
