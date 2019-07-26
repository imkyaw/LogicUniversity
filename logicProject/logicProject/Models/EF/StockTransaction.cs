using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace logicProject.Models.EF
{
    public class StockTransaction
    {
        [Key]
        public int StockTranId { get; set; }
        public DateTime TranDate { get; set; }
        public int Qty { get; set; }
        public int TotalBalance { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}