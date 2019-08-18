using logicProject.Models.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace logicProject.Models.DAO
{
    public class SupplierProductDAO
    {
        private LogicEntities db = new LogicEntities();
        private JavaScriptSerializer jsonSerialiser = new JavaScriptSerializer();

        public string GetSupplierItemPrices(string supplierId)
        {
            string pricesJson = null;
            using (db)
            {
                var result = from sp in db.SupplierProduct
                             where sp.SupplierId.Contains(supplierId)
                             select new { ProductId = sp.ProductId, Price = sp.Price };
                var prices = result.ToList();

                pricesJson = jsonSerialiser.Serialize(prices);
            }
            return pricesJson;
        }
    }
}