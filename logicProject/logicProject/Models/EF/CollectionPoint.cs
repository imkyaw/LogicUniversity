using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace logicProject.Models.EF
{
    public class CollectionPoint
    {
        [Key]
        public int CollectionPtId { get; set; }
        public string CollectionPt { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
    }
}