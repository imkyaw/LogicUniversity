using logicProject.Models.DBContext;
using logicProject.Models.EF;
using logicProject.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace logicProject.Models.DAO
{
    //Written By Zhen Xiang
    public class DisbursementDAO
    {
        private LogicEntities db;
        public List<Disbursement> GetDisbursements(string deptId)
        {
            using (db = new LogicEntities())
            {
                return db.Disbursement.Where(x => x.DeptId == deptId).ToList();
            }   
        }
        public List<Disbursement> GetDisbursements(DateTime startDate, DateTime endDate)
        {
            using (db = new LogicEntities())
            {
                var resultSet = from d in db.Disbursement
                                where d.DisDate >= startDate && d.DisDate <= endDate
                                select d;
                return resultSet.ToList();
            }
        }
        public List<DisbursementDetailViewModel> GetConsolidatedRequestsByDepartment(string deptId, DateTime cutoff)
        {  //Ensures the orders before today only are considered
            using (db = new LogicEntities())
            {
                var resultSet = from rd in db.RequestDetail
                                from r in db.Request
                                where (r.Status.Contains("New") || r.Status.Contains("Outstanding"))
                                && r.RequestId == rd.RequestId && r.Department.DeptId == deptId
                                && r.ReqDate < cutoff
                                group rd by new { rd.ProductId, r.DeptId } into g
                                select new DisbursementDetailViewModel//Note to self: Groupby only works with enums or primitive data types
                                {
                                    ProductId = g.Key.ProductId,
                                    DepartmentId = g.Key.DeptId,
                                    Quantity = g.Sum(x => x.ReqQty)
                                };
                List<DisbursementDetailViewModel> consolidatedOrders = resultSet.ToList();
                return consolidatedOrders;
            }
        }

        public void CreateDisbursement(Disbursement d, List<DisbursementDetail> dd)
        {
            using (db = new LogicEntities())
            {
                db.Disbursement.Add(d);
                db.DisbursementDetail.AddRange(dd);
                db.SaveChanges();
            }
        }
        public void CreateDisbursements(List<Disbursement> d, List<DisbursementDetail> dd)
        {
            using (db = new LogicEntities())
            {
                db.Disbursement.AddRange(d);
                db.DisbursementDetail.AddRange(dd);
                db.SaveChanges();
            }
        }

        public void SaveDisbursement(int id, string status, string[]receivedQtys, string[]remarks, int storestaffId)
        {
            using (db = new LogicEntities())
            {
                var result = from d in db.Disbursement
                             where d.DisId == id
                             select d;
                Disbursement dis = result.First();
                dis.Status = status;
                dis.StoreStaffId = storestaffId;

                var resultSet = from dd in db.DisbursementDetail
                                where dd.DisId == id
                                select dd;

                List<DisbursementDetail> disbursementDetails = resultSet.ToList();
                int x = 0;
                foreach(DisbursementDetail disd in disbursementDetails)
                {
                    disd.ReceivedQty = Convert.ToInt32(receivedQtys[x]);
                    disd.Remarks = remarks[x];
                    x++;
                }
                dis.DisbursementDetails = disbursementDetails;
                db.Entry(dis).State = EntityState.Modified;
                db.SaveChanges();

            }
        }
    }
}