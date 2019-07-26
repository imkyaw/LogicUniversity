using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    }
}