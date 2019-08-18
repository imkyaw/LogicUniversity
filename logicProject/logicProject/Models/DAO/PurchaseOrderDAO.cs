using logicProject.Models.DBContext;
using logicProject.Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace logicProject.Models.DAO
{
    public class PurchaseOrderDAO   //Written By Zhen Xiang
    {
        private LogicEntities db;
        public int SaveNewPO(string supplierId, string[] productIds, string[] requiredQty, int id)
        {
            List<PurchaseOrderDetail> purchaseOrderDetails = new List<PurchaseOrderDetail>();

            PurchaseOrder po = new PurchaseOrder
            {
                OrderDate = DateTime.Now,
                ApprovedDate = null,
                StaffId = id,    //To get this information from the session object
                Status = "Pending",
                SupplierId = supplierId,
            };

            for (int x = 0; x < productIds.Length; x++)
            {
                PurchaseOrderDetail pod = new PurchaseOrderDetail
                {
                    OrderId = po.OrderId,
                    ProductId = productIds[x],
                    ReqQty = Convert.ToInt32(requiredQty[x]),
                };
                purchaseOrderDetails.Add(pod);
            }
            using (db = new LogicEntities())
            {
                db.PurchaseOrder.Add(po);
                db.PurchaseOrderDetail.AddRange(purchaseOrderDetails);
                db.SaveChanges();
            }
            return po.OrderId;
        }
        public PurchaseOrder GetPurchaseOrder(int no)
        {
            PurchaseOrder purchaseOrder = new PurchaseOrder();
            using (db = new LogicEntities())
            {
                var resultsSet = from o in db.PurchaseOrder
                                 where o.OrderId == no
                                 select o;
                purchaseOrder = resultsSet.FirstOrDefault();
            }
            return purchaseOrder;
        }

        public List<PurchaseOrder> GetPurchaseOrders(DateTime start, DateTime end)
        {
            List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();

            using (db = new LogicEntities())
            {
                var resultsSet = from o in db.PurchaseOrder
                                 where o.OrderDate >= start && o.OrderDate <= end
                                 select o;
                purchaseOrders = resultsSet.ToList();
            }

            return purchaseOrders;
        }

        public List<PurchaseOrder> GetPurchaseOrders(string status)
        {
            List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();

            using (db = new LogicEntities())
            {
                var resultsSet = from o in db.PurchaseOrder
                                 where o.Status.Contains(status)
                                 select o;
                purchaseOrders = resultsSet.ToList();
            }

            return purchaseOrders;
        }
        public void SetPOStatus(string status, string remarks, int id)
        {
            using (db = new LogicEntities())
            {
                var result = from po in db.PurchaseOrder
                             where po.OrderId == id
                             select po;

                PurchaseOrder p = result.First();
                p.Status = status;
                p.ApprovedDate = DateTime.Now;
                p.Remarks = remarks;  //Additional remarks column for po needed, must be able to be null
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}