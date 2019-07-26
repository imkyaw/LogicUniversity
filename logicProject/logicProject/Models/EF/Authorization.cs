using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace logicProject.Models.EF
{
    public class Authorization
    {
        [Key]
        public int AuthNo { get; set; }
        public int StaffId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}