using logicProject.Models.DBContext;
using logicProject.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace logicProject.Models.DAO
{
    public class FavOrderDAO
    {
        public static void AddRequest(string products, string qty, int id, string nameId)
        {
            string[] productArr = products.Split(',').ToArray();
            int[] qtyArr = qty.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            using (LogicEntities db = new LogicEntities())
            {
                string DeptId = db.DepartmentStaff.Where(x => x.StaffId == id).Select(x => x.DeptId).FirstOrDefault();
                string DeptName = db.Department.Where(x => x.DeptId == DeptId).Select(x => x.DeptName).FirstOrDefault();
                FavOrder request = new FavOrder();
                request.StaffId = id;
                request.DeptId = DeptId;
                
                request.FavOrderName = nameId;
                db.FavOrder.Add(request);
                db.SaveChanges();
                for (int i = 0; i < productArr.Length; i++)
                {
                    string des = productArr[i];
                    string productId = db.Product.Where(x => x.Description == des).Select(x => x.ProductId).FirstOrDefault();
                    FavOrderDetails rd = new FavOrderDetails();
                    rd.ProductId = productId;
                    rd.FavQty = qtyArr[i];
                    rd.FavOrderId = request.FavOrderId;
                    db.FavOrderDetails.Add(rd);
                }
                db.SaveChanges();
                FavOrder r = db.FavOrder.Find(request.FavOrderId);
                r.FavFormId = DeptId + "/" + "F0" + r.FavOrderId + "-" + DeptName;
                db.SaveChanges();
            }
        }
    }

}