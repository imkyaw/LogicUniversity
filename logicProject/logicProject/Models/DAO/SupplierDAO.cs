using logicProject.Models.DBContext;
using logicProject.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace logicProject.Models.DAO
{
    public class SupplierDAO
    {
        private LogicEntities db = new LogicEntities();
        public List<Supplier> GetSuppliers()
        {
            using (db)
            {
                return db.Supplier.ToList();
            }
        }
    }
}