using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using logicProject.Models.DBContext;
using logicProject.Models.EF;

namespace logicProject.Controllers
{
    public class DepartmentsController : Controller
    {
        private LogicEntities db = new LogicEntities();

        // GET: Departments
        public ActionResult Index()
        {
            var department = db.Department.Include(d => d.CollectionPoint);
            return View(department.ToList());
        }

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Department.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Departments/Create
        //public ActionResult Create()
        //{
        //    ViewBag.CollectionPt = new SelectList(db.CollectionPoint, "CollectionPtId", "CollectionPt");
        //    return View();
        //}
        
        //// POST: Departments/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "DeptId,DeptName,ContactName,PhNo,FaxNo,EmailAddr,HeadId,CollectionPt,RepId")] Department department)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Department.Add(department);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CollectionPt = new SelectList(db.CollectionPoint, "CollectionPtId", "CollectionPt", department.CollectionPt);
        //    return View(department);
        //}

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Department.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.CollectionPt = new SelectList(db.CollectionPoint, "CollectionPtId", "CollectionPt", department.CollectionPt);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DeptId,DeptName,ContactName,PhNo,FaxNo,EmailAddr,HeadId,CollectionPt,RepId")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CollectionPt = new SelectList(db.CollectionPoint, "CollectionPtId", "CollectionPt", department.CollectionPt);
            return View(department);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Department.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Department department = db.Department.Find(id);
            db.Department.Remove(department);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
