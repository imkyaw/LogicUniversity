using logicProject.Models.DBContext;
using logicProject.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace logicProject.Models.DAO
{
    public class StockTransactionDAO
    {//Written By Zhen Xiang
        private LogicEntities db;

        public void UpdateQuantityOneItem(string itemCode, string purpose, int qty, DateTime date,string transactionDetails)
        {
            if (purpose == "Withdrawal")
            {
                qty = -qty;
            }
            using (db = new LogicEntities())
            {
                Product p = db.Product.Find(itemCode);

                StockTransaction st = new StockTransaction
                {
                    ProductId = itemCode,
                    TranDate = date,
                    Qty = qty,
                    TotalBalance = p.Qty + qty,
                    Remarks = purpose + " " + transactionDetails //Need an additional remarks field
                };
                db.StockTransaction.Add(st);
                db.SaveChanges();
            }
        }
        
        public void WithdrawManyItems(string[] itemCodes, string[] retrievedQtys)
        {
            List<StockTransaction> stockTransactions = new List<StockTransaction>();
            using (db = new LogicEntities())
            {
                for (int x = 0; x < itemCodes.Length; x++)
                {
                    Product p = db.Product.Find(itemCodes[x]);
                    if (-1 * Convert.ToInt32(retrievedQtys[x]) > p.Qty)
                    {
                        //Throw an error    //Not used due to modification made to view validation
                    }
                    else
                    {
                        StockTransaction stockTransaction = new StockTransaction
                        {
                            ProductId = itemCodes[x],
                            Qty = -1 * Convert.ToInt32(retrievedQtys[x]),
                            TranDate = DateTime.Now,
                            Remarks = "Inventory Withdrawal, Date: " +DateTime.Now.Date.ToString("d"),
                            TotalBalance = p.Qty - Convert.ToInt32(retrievedQtys[x])
                        };
                        stockTransactions.Add(stockTransaction);
                    }
                }
                db.StockTransaction.AddRange(stockTransactions);
                db.SaveChanges();
            }
        }
        public void AddManyItems(string[] itemCodes, string[] receivedQtys, DateTime date, string supplierId, string comments)
        {
            List<StockTransaction> stockTransactions = new List<StockTransaction>();

            using (db = new LogicEntities())
            {
                for (int x = 0; x < itemCodes.Length; x++)
                {

                    Product p = db.Product.Find(itemCodes[x]);
                    StockTransaction stockTransaction = new StockTransaction
                    {
                        ProductId = itemCodes[x],
                        Qty = Convert.ToInt32(receivedQtys[x]),
                        TranDate = date,
                        Remarks = "Inventory Received, Date: " +date.Date.ToString("d") + " " + supplierId + " " + comments,
                        TotalBalance = p.Qty + Convert.ToInt32(receivedQtys[x])
                    };
                    stockTransactions.Add(stockTransaction);
                }
                db.StockTransaction.AddRange(stockTransactions);
                db.SaveChanges();
            }
        }
    }
}