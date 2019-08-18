using logicProject.Models.DBContext;
using logicProject.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace logicProject.Models.DAO
{
    public class DepartmentStaffDAO
    {
        private LogicEntities db;

        public DepartmentStaff getDeptRep(string deptId)
        {
            using (db = new LogicEntities())
            {
                return db.DepartmentStaff.FirstOrDefault(x => x.DeptId == deptId && x.StaffType == "Rep");
            }
        }
        public DepartmentStaff GetStaffById(int id)
        {
            using (db = new LogicEntities())
            {
                return db.DepartmentStaff.Find(id);
            }
        }
    }
}