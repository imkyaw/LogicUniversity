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
        [DeptAuthorize]
        [Custom(Roles = "All")]
        public ActionResult OrderStatus()
        {
            DepartmentStaff a = Session["DeptStaff"] as DepartmentStaff;
            String temp = Convert.ToBoolean(Session["TempHead"]).ToString();
            String check= Convert.ToBoolean(Session["CheckHead"]).ToString();
            ViewData["ApproveOrNot"] = null;
            var request = db.Request.Where(x=>x.DeptId==a.DeptId).Include(d => d.Department);
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
        [DeptAuthorize]
        [Custom(Roles = "staff")]
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
            DepartmentStaff a = Session["DeptStaff"] as DepartmentStaff;
            if (products != "" && ModelState.IsValid)
            {
                List<string> head = new List<string>();
                RequestDAO.AddRequest(products, qty, a.StaffId);
                string headname = db.DepartmentStaff.Where(x => x.DeptId == a.DeptId && x.StaffType == "head").Select(x=>x.StaffEmail).FirstOrDefault();
                head.Add(headname);
                string message = Utility.EmailBody.RequestBody + a.StaffName + Utility.EmailBody.RequestBody2;
                Utility.EmailService.SendEmail(head, Utility.EmailBody.RequestSubject, message);
                return RedirectToAction("OrderStatus","Request");
            }
            return RedirectToAction("OrderStatus", "Request");
        }

        //Save Request
        
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
        [Custom(Roles = "head")]
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
        [Custom(Roles = "head")]
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
            DepartmentStaff ds = Session["DeptStaff"] as DepartmentStaff;
            Request rd = db.Request.Find(id);
            List<string> emails = new List<string>();
            string email = db.DepartmentStaff.Where(x => x.StaffId == rd.StaffId).Select(x => x.StaffEmail).FirstOrDefault();
            emails.Add(email);
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
            string message = Utility.EmailBody.ApproveBody + approve + ". Check for the reason  http://127.0.0.1:64451/Request/OrderStatus";
            Utility.EmailService.SendEmail(emails, Utility.EmailBody.ApproveSubject + approve, message);
            return RedirectToAction("OrderStatus");
        }


        //Save Favourite 
        [HttpGet]
        public ActionResult ViewFaveOrder()
        {

            var res = (from t in ((from r in db.Request
                                   join rd in db.RequestDetail on r.RequestId equals rd.RequestId
                                   where rd.ProductId == "P001"
                                   where r.DeptId == "6"
                                   select new
                                   {
                                       rdate = r.ReqDate,
                                       qty = rd.ReqQty
                                   }).ToList())
                       group t by new { rdate = new DateTime(t.rdate.Year, t.rdate.Month, 1) } into g
                       select new
                       {
                           date = g.Key.rdate,
                           qty = g.Sum(x => x.qty)
                       }).ToList();
            List<FavOrder> list = new List<FavOrder>();
            list = db.FavOrder.ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult FavOrderDetails(int id)
        {
            List<FavOrderDetails> fv = db.FavOrderDetails.Where(x => x.FavOrderId.Equals(id)).ToList();
            return View(fv);
        }

        [HttpGet]
        public ActionResult CreateFavOrder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateFavOrder(string products, string qty, string nameId)
        {
            DepartmentStaff ds = Session["DeptStaff"] as DepartmentStaff;

            if (products != "" && ModelState.IsValid)
            {
                FavOrderDAO.AddRequest(products, qty, ds.StaffId, nameId);
                return RedirectToAction("ViewFaveOrder");
            }

            return RedirectToAction("ViewFaveOrder");
        }


        [HttpGet]
        public ActionResult DeleteFavOrder(int id)
        {
            List<FavOrderDetails> fv = db.FavOrderDetails.Where(a => a.FavOrderId.Equals(id)).ToList();
            return View(fv);
        }

        [HttpPost]
        public ActionResult DeleteFavOrder(string delete, int id)
        {
            FavOrder fv = db.FavOrder.Find(id);
            db.FavOrder.Remove(fv);
            db.SaveChanges();
            return RedirectToAction("ViewFaveOrder");
        }


        [HttpGet]
        public ActionResult SendForOrder(int Id)
        {
             List < FavOrderDetails > fv = db.FavOrderDetails.Where(a => a.FavOrderId.Equals(Id)).ToList();
            return View(fv);
        }

        [HttpPost]
        public ActionResult SendforOrder(int Id)
        {
            DateTime reqTime = DateTime.Now;
            List<FavOrderDetails> fav = new List<FavOrderDetails>();
            FavOrder fv = db.FavOrder.Find(Id);
            fav = db.FavOrderDetails.Where(x => x.FavOrderId == fv.FavOrderId).ToList();
            Request request = new Request();
            request.DeptId = fv.DeptId;
            request.RequestFormId = fv.FavFormId;
            request.StaffId = fv.StaffId;
            request.Status = "Pending";
            request.ReqDate = reqTime;
            db.Request.Add(request);
            db.SaveChanges();
            foreach(FavOrderDetails f in fav) 
            {
                RequestDetail rd = new RequestDetail();
                rd.ProductId = f.ProductId;
                rd.ReqQty = f.FavQty;
                rd.RequestId = request.RequestId;
                db.RequestDetail.Add(rd);
            }
            db.SaveChanges();
            Request r = db.Request.Find(request.RequestId);
            r.RequestFormId = fv.DeptId + "/" + "R0" + r.RequestId + "-" + fv.Department.DeptName;
            db.SaveChanges();
            return RedirectToAction("OrderStatus", "Request");
        }
    }
    
}
