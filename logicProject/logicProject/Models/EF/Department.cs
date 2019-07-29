using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace logicProject.Models.EF
{
    public class Department
    {
        [Key]
        public string DeptId { get; set; }
        public string DeptName { get; set; }
        public string ContactName { get; set; }
        public string PhNo { get; set; }
        public string FaxNo { get; set; }
        public string EmailAddr { get; set; }
        public int HeadId { get; set; }
        public int CollectionPt { get; set; }
        [ForeignKey("CollectionPt")]
        public virtual CollectionPoint CollectionPoint { get; set; }
        public int RepId { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<DepartmentStaff> DepartmentStaffs { get; set; }
    }
}