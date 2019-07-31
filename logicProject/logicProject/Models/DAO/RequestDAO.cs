using logicProject.Models.DBContext;
using logicProject.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace logicProject.Models.DAO
{
    public class RequestDAO
    {
        enum status
        {
            Pending,
            Approved,
            Outstanding,
            Complete
        }
        public static void AddRequest(string products,string qty,int id)
        {
            string[] productArr = products.Split(',').ToArray();
            int[] qtyArr = qty.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            DateTime reqTime = DateTime.Now;
            
            using(LogicEntities db = new LogicEntities())
            {
                string DeptId = db.DepartmentStaff.Where(x => x.StaffId == id).Select(x => x.DeptId).FirstOrDefault();
                Request request = new Request();
                request.StaffId = id;
                request.DeptId = DeptId;
                request.RequestFormId = DeptId + "/";
                request.ReqDate = reqTime;
                request.Status = status.Pending.ToString();
                db.Request.Add(request);
                db.SaveChanges();
                for(int i = 0; i < productArr.Length; i++)
                {
                    string des = productArr[i];
                    string productId = db.Product.Where(x => x.Description == des).Select(x=>x.ProductId).SingleOrDefault();
                    RequestDetail rd = new RequestDetail();
                    rd.ProductId = productId;
                    rd.ReqQty = qtyArr[i];
                    rd.RequestId = request.RequestId;
                    db.RequestDetail.Add(rd);   
                }
                db.SaveChanges();
            }
        }
    }
}