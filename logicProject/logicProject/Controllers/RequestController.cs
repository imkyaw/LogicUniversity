using logicProject.Models.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
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
        //public ActionResult 
    }
}