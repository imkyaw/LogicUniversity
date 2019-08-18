using logicProject.Models.DBContext;
using logicProject.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace logicProject.Models.DAO
{
    public class StoreStaffDAO
    {
        private LogicEntities db;

        public StoreStaff GetStaffbyId(int id)
        {
            using (db = new LogicEntities())
            {
                return db.StoreStaff.FirstOrDefault(x => x.StaffId == id);
            }
        }
        public StoreStaff GetStoreStaffbyPONumber(int id)
        {
            using (db = new LogicEntities())
            {
                return db.StoreStaff.FirstOrDefault(x => x.StaffId == id);
            }
        }
        public StoreStaff GetStoreStaffbyRole(string role)
        {
            using (db = new LogicEntities())
            {
                return db.StoreStaff.FirstOrDefault(x => x.StaffType == role);
            }
        }
    }
}