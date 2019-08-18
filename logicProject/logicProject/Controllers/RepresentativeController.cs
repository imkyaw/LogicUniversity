using logicProject.Filter;
using logicProject.Models.DBContext;
using logicProject.Models.EF;
using logicProject.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace logicProject.Controllers
{
    public class RepresentativeController : Controller
    {
        private LogicEntities db = new LogicEntities();
        // GET: Representative
        public ActionResult Index()
        {
            return View();
        }
        [Filter.DeptAuthorize]
        [Custom(Roles = "staff")]
        public ActionResult DisbursementList()
        {
            List<DisbursementViewModel> list = (from d in db.Disbursement
                        join s in db.StoreStaff on d.StoreStaffId equals s.StaffId
                        select new DisbursementViewModel
                        {
                            Disbur = d,
                            StaffName=s.StaffName
                        }).ToList();
            ViewData["dislist"] = list;            
            return View();
        }
        [Filter.DeptAuthorize]
        [Custom(Roles = "staff")]
        public ActionResult DisbursementDetail(int id)
        {
            DisbursementViewModel dis =( from d in db.Disbursement
                               join c in db.CollectionPoint on d.CollectionPointId equals c.CollectionPtId
                               where d.DisId==id
                               select new DisbursementViewModel
                               {
                                   Disbur=d,
                                   CollectionPoint=c.CollectionPt,
                                   Detail=db.DisbursementDetail.Where(x=>x.DisId==id).ToList(),
                               }).FirstOrDefault();
            ViewData["dislist"] = dis;
            return View();
        }
    }
}