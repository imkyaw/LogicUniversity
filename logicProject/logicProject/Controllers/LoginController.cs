using logicProject.Models.DAO;
using logicProject.Models.DBContext;
using logicProject.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace logicProject.Controllers
{
   [RoutePrefix("Logic")]
    public class LoginController : Controller
    {
        // GET: Login
        private LogicEntities db = new LogicEntities();
        [Route("Store")]
        public ActionResult StoreLogin(string username,string password)
        {
            string session = (string)Session["StoreSession"];
            if (session != null)
            {
                //return RedirectToAction("Index", "Inventory");
            }
            if (username == null || password == null)
                return View();
            StoreStaff user = db.StoreStaff.Where(x => x.Username == username && x.Password == password).SingleOrDefault();
            if (user == null)
            {
                return View();
            }
            string sessionId = Guid.NewGuid().ToString();
            if (user != null)
            {
                user.SessionId = sessionId;
                db.SaveChanges();
            }
            Session["StoreSession"] = sessionId;
            Session["StoreStaff"] = user;
            if (user.StaffType == "Clerk")
            {
                return RedirectToAction("Index", "Inventory");
            }
            else if (user.StaffType == "Supervisor")
            {
                return RedirectToAction("Index", "Supervisor");
            }
            else if (user.StaffType == "Manager")
            {
                return RedirectToAction("Index", "Manager");
            }
            else
            {
                return View();
            }
        }
        [Route("Department")]
        public ActionResult DepartmentLogin(string username, string password)
        {
            string session = (string)Session["DeptSession"];
            if (session != null)
            {
                return RedirectToAction("Dashboard", "Departments");
            }
            if (username == null || password == null)
                return View();
            DepartmentStaff user = db.DepartmentStaff.Where(x => x.Username == username && x.Password == password).SingleOrDefault();
            DateTime today = DateTime.Now;
            int deptId = int.Parse(user.DeptId);
            Authorization authorization = db.Authorization.Where(x => x.StaffId == user.StaffId && x.StartDate<=today && x.EndDate>=today).FirstOrDefault();
            Authorization checkHead = db.Authorization.Where(x=>x.DeptId==deptId && x.StartDate <= today && x.EndDate >= today).FirstOrDefault();
            string sessionId = Guid.NewGuid().ToString();
            if (user == null)
            {
                return View();
            }
            if (user != null)
            {
                user.SessionId = sessionId;
                db.SaveChanges();
            }
            Session["DeptSession"] = sessionId;
            Session["DeptStaff"] = user;
            Session["TempHead"] = false;
            Session["CheckHead"] = false;
            if (checkHead != null)
                Session["CheckHead"] = true;
            if (authorization != null)
            {
                Session["TempHead"] = true;
            }
            return RedirectToAction("Dashboard", "Departments");
        }

        public JsonResult DepartmentLoginApi(string username, string password)
        {
            if (username == null || password == null)
            {
                return Json(new { isok = false, message = "Login Unsuccessful" });
            }
            DepartmentStaff user = db.DepartmentStaff.Where(x => x.Username == username && x.Password == password).SingleOrDefault();
            if (user == null)
            {
                return Json(new { isok = false, message = "Login Unsuccessful" });
            }
            return Json(new { isok = true, message = "Login Successful",Id=user.StaffId });
        }

        public ActionResult Logout(string type)
        { 
            if (type == "d")
            {
                string session = (string)Session["DeptSession"];
                DepartmentStaff ds = db.DepartmentStaff.Where(x => x.SessionId == session).SingleOrDefault();
                ds.SessionId = null;
                Session.Remove("DeptSession");
                Session.Remove("DeptStaff");
                Session.Remove("TempHead");
                db.SaveChanges();
                return RedirectToAction("DepartmentLogin");
            }
            else
            {
                string session = (string)Session["StoreSession"];
                StoreStaff ss = db.StoreStaff.Where(x => x.SessionId == session).SingleOrDefault();
                ss.SessionId = null;
                Session.Remove("StoreSession");
                Session.Remove("StoreStaff");
                db.SaveChanges();
                return RedirectToAction("StoreLogin");
            }
            
        }
        [HttpGet,Route]
        public ActionResult StartPage()
        {
            return View();
        }
    }
}