using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace logicProject.Models.EF
{
    public class FavOrder
    {
        [Key]
        public int FavOrderId { get; set; }
        public string FavOrderName { get; set; }
        public string FavFormId { get; set; }
        public int StaffId { get; set; }
        public string DeptId { get; set; }
        [ForeignKey("DeptId")]
        public virtual Department Department { get; set; }
    }

}