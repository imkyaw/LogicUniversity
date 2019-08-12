using logicProject.Models.DBContext;
using logicProject.Models.EF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace logicProject.Controllers
{
    public class DeptHeadController : Controller
    {
        private LogicEntities db = new LogicEntities();

        //Get the list of staff - Hui Chin
        public JsonResult GetStaffList(string searchTerm)
        {
            var staffList = db.DepartmentStaff.ToList();

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
    
        public ActionResult AppointRepresentative()
        {
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
            }
            return Json(new { isok = true, message = "Appointment Successful" });
        }

        public ActionResult AuthorizeStaff()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AppointStaff(string staffname, DateTime getStartDate, DateTime getEndDate)
        {
            var result = db.DepartmentStaff.SingleOrDefault(b => b.StaffName == staffname);
            var resultStaffId = result.StaffId;
            var resultDeptId = int.Parse(result.DeptId);
            using (LogicEntities db = new LogicEntities())
            {
                var authorization = db.Set<Authorization>();
                authorization.Add(new Authorization { DeptId = resultDeptId, StaffId = resultStaffId, StartDate = getStartDate, EndDate = getEndDate });
                db.SaveChanges();
            }
            return Json(new { isok = true, message = "Authorization Successful" });
        }
        [HttpPost]
        public ActionResult AppointStaffApi(string staffname, string getStartDate, string getEndDate)
        {
            var result = db.DepartmentStaff.SingleOrDefault(b => b.StaffName == staffname);
            var resultStaffId = result.StaffId;
            var resultDeptId = int.Parse(result.DeptId);
            DateTime fromDate = DateTime.ParseExact(getStartDate, "MM/dd/yyyy HH:mm:ss",
                                           CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(getEndDate, "MM/dd/yyyy HH:mm:ss",
                                           CultureInfo.InvariantCulture);
            using (LogicEntities db = new LogicEntities())
            {
                var authorization = db.Set<Authorization>();
                authorization.Add(new Authorization { DeptId = resultDeptId, StaffId = resultStaffId, StartDate = fromDate, EndDate = toDate });
                db.SaveChanges();
            }
            return Json(new { isok = true, message = "Authorization Successful" });
        }

        //Hui Chin Part finished
    }
}