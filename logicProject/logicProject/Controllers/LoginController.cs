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
                return RedirectToAction("Dashboard", "Store");
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
            return RedirectToAction("Dashboard", "Store");
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
            return RedirectToAction("Dashboard", "Departments");
        }
        public ActionResult Logout(string type)
        {
              
            if (type == "d")
            {
                string session = (string)Session["DeptSession"];
                DepartmentStaff ds = db.DepartmentStaff.Where(x => x.SessionId == session).SingleOrDefault();
                ds.SessionId = null;
                Session.Remove("DeptSession");
                db.SaveChanges();
                return RedirectToAction("DepartmentLogin");
            }
            else
            {
                string session = (string)Session["StoreSession"];
                StoreStaff ss = db.StoreStaff.Where(x => x.SessionId == session).SingleOrDefault();
                ss.SessionId = null;
                Session.Remove("StoreSession");
                db.SaveChanges();
                return RedirectToAction("StoreLogin");
            }
            
        }
    }
}