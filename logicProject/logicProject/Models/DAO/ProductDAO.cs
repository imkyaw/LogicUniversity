using logicProject.Models.DBContext;
using logicProject.Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
//Written by Zhen Xiang
namespace logicProject.Models.DAO
{
    public class ProductDAO
    {
        private LogicEntities db;
        private JavaScriptSerializer jsonSerialiser = new JavaScriptSerializer();

        public Product FindProductById(string id)
        {
            using (db = new LogicEntities())
            {
                Product p = db.Product.Find(id);
                return p;
            }
        }

        public string GetItemDetails(string supplierId)
        {
            string detailsJson = "";
            using (db = new LogicEntities())
            {
                var resultSet = from p in db.Product
                                from sp in db.SupplierProduct
                                where sp.SupplierId == supplierId && p.ProductId == sp.ProductId
                                select new { ProductId = p.ProductId, Description = p.Description, Unit = p.Unit };

                var itemDetails = resultSet.ToList();
                detailsJson = jsonSerialiser.Serialize(itemDetails);
            }
            return detailsJson;
        }

        public void UpdateItemQty(string productId, int qty)
        {
            using (db = new LogicEntities())
            {
                Product p = db.Product.Find(productId);
                p.Qty = p.Qty + qty;
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void UpdateItemQty(string[] productIds, string[] qtys, bool isAdd)
        {
            using (db = new LogicEntities())
            {
                for(int x = 0; x < productIds.Length; x++)
                {
                    Product p = db.Product.Find(productIds[x]);
                    if (isAdd)
                    {
                        p.Qty = p.Qty + Convert.ToInt32(qtys[x]);
                    }
                    else
                    {
                        p.Qty = p.Qty - Convert.ToInt32(qtys[x]);
                    }
                    db.Entry(p).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
        }

        public int GetItemMaxQty(string id)
        {
            using (db = new LogicEntities())
            {
                return db.Product.Find(id).Qty;
            }
        }
    }
}