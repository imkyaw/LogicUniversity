using logicProject.Models.DBContext;

using logicProject.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using logicProject.Models.EF;
using logicProject.Filter;

namespace logicProject.Controllers
{
    public class DeptHeadController : Controller
    {
        private LogicEntities db = new LogicEntities();

        //Get the list of staff - Hui Chin
        public JsonResult GetStaffList(string searchTerm)
        {
            DepartmentStaff a = Session["DeptStaff"] as DepartmentStaff;
            var staffList = db.DepartmentStaff.Where(x=>x.StaffType!="head" && x.DeptId==a.DeptId).ToList();

            if (searchTerm != null)
            {
                staffList = db.DepartmentStaff.Where(x => x.StaffName.Contains(searchTerm)).ToList();

            }

            var modifiedData = staffList.Select(x => new
            {
                id = x.StaffId,
                text = x.StaffName
            });
            return Json(modifiedData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStaffListApi(string searchTerm,int staffId)
        {
            string deptId = db.DepartmentStaff.Where(x => x.StaffId == staffId).Select(x => x.DeptId).FirstOrDefault();
            var staffList = db.DepartmentStaff.Where(x => x.StaffType != "head" && x.DeptId==deptId).ToList();

            if (searchTerm != null)
            {
                staffList = db.DepartmentStaff.Where(x => x.StaffName.Contains(searchTerm)).ToList();
            }

            var modifiedData = staffList.Select(x => new
            {
                id = x.StaffId,
                text = x.StaffName
            });
            return Json(modifiedData, JsonRequestBehavior.AllowGet);
        }
        [Filter.DeptAuthorize]
        [Custom(Roles = "head")]
        public ActionResult AppointRepresentative()
        {
            DepartmentStaff a = Session["DeptStaff"] as DepartmentStaff;
            Department d = db.Department.Where(x => x.DeptId == a.DeptId).FirstOrDefault();
            List<DepartmentStaff> list = db.DepartmentStaff.ToList();
            String name = list.Where(x => x.StaffId == d.RepId).Select(x => x.StaffName).FirstOrDefault();
            ViewData["CurrentRep"] = name;
            return View();
        }

        [HttpPost]
        public ActionResult AppointRep(string staffname)
        {
            
            var result = db.DepartmentStaff.SingleOrDefault(x => x.StaffName == staffname);

            if (result != null)
            {
                var resultId = result.StaffId;
                var deptId = result.DeptId;
                Department d = db.Department.Where(x => x.DeptId == deptId).SingleOrDefault();
                d.RepId = result.StaffId;
                db.SaveChanges();
                List<string> email = new List<string>();
                email.Add(result.StaffEmail);
                Utility.EmailService.SendEmail(email, Utility.EmailBody.RepresentativeSubject, Utility.EmailBody.RepresentatvieBody);
                return Json(new { isok = true, message = "Appointment Successful.", redirect = "/DeptHead/AppointRepresentative" });
            }
            return Json(new { isok = false, message = "Appointment Unsuccessful.", redirect = "/DeptHead/AppointRepresentative" });
        }
        [Filter.DeptAuthorize]
        [Custom(Roles = "head")]
        public ActionResult AuthorizeStaff()
        {
            DepartmentStaff a = Session["DeptStaff"] as DepartmentStaff;
            string deptId = a.DeptId;
            ViewData["tempHead"] = false;
            if (a.StaffId == a.Department.HeadId)
            {
                ViewData["tempHead"] = true;
            }
            DateTime today = DateTime.Now;
            var authorization = (from auth in db.Authorization
                                join s in db.DepartmentStaff on auth.StaffId equals s.StaffId
                                where auth.DeptId == deptId
                                where auth.EndDate>=today
                                select new AuthorizationViewModel
                                {
                                    staffName = s.StaffName,
                                    auth = auth
                                }).ToList();
            //var authorization = db.Authorization.Where(x=>x.DeptId==deptId && x.StartDate>=today).ToList();
            ViewData["AuthList"] = authorization;
            return View();
        }

        [HttpPost]
        public ActionResult AppointStaff(string staffname, DateTime getStartDate, DateTime getEndDate)
        {
            var result = db.DepartmentStaff.SingleOrDefault(b => b.StaffName == staffname);
            var resultStaffId = result.StaffId;
            string resultDeptId = result.DeptId;
            var list = db.Authorization.Where(x => x.DeptId == resultDeptId && x.EndDate >= getStartDate && x.StartDate <= getEndDate).ToList();
            if (list.Count>0)
            {
                return Json(new { isok = false, message = "Authorization Unsuccessful" });
            }
            using (LogicEntities db = new LogicEntities())
            {
                var authorization = db.Set<logicProject.Models.EF.Authorization>();
                authorization.Add(new logicProject.Models.EF.Authorization { DeptId = resultDeptId, StaffId = resultStaffId, StartDate = getStartDate, EndDate = getEndDate });
                db.SaveChanges();
            }
            List<string> email = new List<string>();
            email.Add(result.StaffEmail);
            string message = Utility.EmailBody.HeadBody + getStartDate.ToString("dddd, dd MMMM yyyy")+" and will expired on "+getEndDate.ToString("dddd, dd MMMM yyyy");
            Utility.EmailService.SendEmail(email, Utility.EmailBody.HeadSubject, message);
            return Json(new { isok = true, message = "Authorization Successful", redirect = "/DeptHead/AuthorizeStaff" });
        }

        [HttpPost]
        public ActionResult AppointStaffApi(string staffname, string getStartDate, string getEndDate)
        {
            var result = db.DepartmentStaff.SingleOrDefault(b => b.StaffName == staffname);
            var resultStaffId = result.StaffId;
            string resultDeptId = result.DeptId;
            DateTime fromDate = DateTime.ParseExact(getStartDate, "MM/dd/yyyy HH:mm:ss",
                                           CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(getEndDate, "MM/dd/yyyy HH:mm:ss",
                                           CultureInfo.InvariantCulture);
            using (LogicEntities db = new LogicEntities())
            {
                var authorization = db.Set<logicProject.Models.EF.Authorization>();
                authorization.Add(new logicProject.Models.EF.Authorization { DeptId = resultDeptId, StaffId = resultStaffId, StartDate = fromDate, EndDate = toDate });
                db.SaveChanges();
            }
            List<string> email = new List<string>();
            email.Add(result.StaffEmail);
            string message = Utility.EmailBody.HeadBody +getStartDate+ " and will expired on "+getEndDate;
            Utility.EmailService.SendEmail(email, Utility.EmailBody.HeadSubject, message);
            return Json(new { isok = true, message = "Authorization Successful" });
        }
        [Custom(Roles = "head")]
        public ActionResult DeleteAuth(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.EF.Authorization a = db.Authorization.Find(id);
            if (a == null)
            {
                return HttpNotFound();
            }
            db.Authorization.Remove(a);
            db.SaveChanges();
            return RedirectToAction("AuthorizeStaff");
        }
        //Hui Chin Part finished
    }
}