using logicProject.Models.DBContext;
using logicProject.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace logicProject.Models.DAO
{
    public class DepartmentDAO
    {   //Written By Zhen Xiang
        private LogicEntities db;
        public List<Department> GetDepartments()
        {
            using (db = new LogicEntities())
            {
                return db.Department.ToList();
            }
        }
        public List<Department> GetDepartmentsWithOutstandingOrders(DateTime cutoff)
        {
            using (db = new LogicEntities())
            {
                var resultSet = from r in db.Request
                                where (r.Status.Contains("New") || r.Status.Contains("Outstanding"))
                                && r.ReqDate < cutoff
                                select r.Department;
                List<Department> departments = resultSet.Distinct().ToList();
                return departments;
            }
        }
    }
}