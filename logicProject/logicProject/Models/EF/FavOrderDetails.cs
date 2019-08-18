using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace logicProject.Models.EF
{
    public class FavOrderDetails
    {
        [Key]
        public int FavOrderDetailsId { get; set; }
        public string ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public int FavOrderId { get; set; }
        [ForeignKey("FavOrderId")]
        public virtual FavOrder FavOrder { get; set; }
        public int FavQty { get; set; }
    }

}