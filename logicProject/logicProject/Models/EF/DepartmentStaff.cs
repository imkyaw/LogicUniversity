using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace logicProject.Models.EF
{
    public class DepartmentStaff
    {
        [Key]
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public string StaffEmail { get; set; }
        public string StaffType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SessionId { get; set; }
        public string DeptId { get; set; }
        [ForeignKey("DeptId")]
        public virtual Department Department { get; set; }
    }
}