using logicProject.Models.DAO;
using logicProject.Models.DBContext;
using logicProject.Models.EF;
using logicProject.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using logicProject.Utility;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace logicProject.Controllers
{
    public class SupervisorController : Controller
    {
        LogicEntities db = new LogicEntities();
        private PurchaseOrderDAO purchaseOrderDAO = new PurchaseOrderDAO();
        private StoreStaffDAO storestaffDAO = new StoreStaffDAO();
        // GET: Supervisor
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ProductCategory()
        {
            var categories = db.Product.Select(p => p.Category).Distinct().ToList();
            ViewData["category"] = categories;
            return View();
        }

        public ActionResult ProductDetails(string category)
        {
            List<Product> products = db.Product.Where(pro => pro.Category == category).ToList();
            ViewData["productdetails"] = products;
            ViewData["category"] = category;
            return View();
        }

        public ActionResult StockCardDetails(string id)
        {
            Product product = db.Product.Find(id);

            List<SupplierProduct> supplierproduct = db.SupplierProduct.OrderBy(pro => pro.Price).Where(pro => pro.ProductId == id).ToList();
            ViewData["supplierproduct"] = supplierproduct;

            List<StockTransaction> stockCardDetails = db.StockTransaction.Where(p => p.ProductId == id).ToList();
            ViewData["stockcarddetails"] = stockCardDetails;

            return View(product);
        }

        public ActionResult SupplierList()
        {
            return View(db.Supplier.ToList());
        }

        public ActionResult ItemSupplied(string supplierid)
        {
            //SELECT s.ProductId,p.Category,p.[Description],s.Price
            //FROM SupplierProducts s, Suppliers sp,Products p
            //WHERE s.SupplierId = 'ALPA' AND p.ProductId = s.ProductId AND s.SupplierId = sp.SupplierId;

            SupplierProduct suppro = db.SupplierProduct.Where(pro => pro.SupplierId == supplierid).First();

            List<Product> products = db.Product.ToList();
            List<Supplier> suppliers = db.Supplier.ToList();
            List<SupplierProduct> supplierproducts = db.SupplierProduct.Where(sup => sup.SupplierId == supplierid).ToList();

            if (suppro == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewData["supplierproducts"] = suppro;
            }

            List<ItemSuppliedViewModel> data = (from sp in supplierproducts
                                                join s in suppliers on sp.SupplierId equals s.SupplierId
                                                join p in products on sp.ProductId equals p.ProductId
                                                select new ItemSuppliedViewModel { product = p, supplier = s, supplierproduct = sp }).ToList();

            return View(data);
        }
        public ActionResult DepartmentDetails()
        {

            List<ViewModelDepartment> dep = db.Department
                .Select(x => new ViewModelDepartment { collectionpoint = x.CollectionPoint, department = x, headname = db.DepartmentStaff.Where(y => y.DeptId == x.DeptId && x.HeadId == y.StaffId).Select(y => y.StaffName).FirstOrDefault(), Repname = db.DepartmentStaff.Where(y => y.DeptId == x.DeptId && x.RepId == y.StaffId).Select(y => y.StaffName).FirstOrDefault() }).ToList();

            return View(dep);
        }

        //public ActionResult EditDepartmentDetails(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Department department = db.Department.Find(id);
        //    if (department == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CollectionPt = new SelectList(db.CollectionPoint, "CollectionPtId", "CollectionPt", department.CollectionPt);

        //    return View(department);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult EditDepartmentDetails([Bind(Include = "DeptId,DeptName,ContactName,PhNo,FaxNo,EmailAddr,HeadId,CollectionPt,RepId")] Department department)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(department).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("DepartmentDetails");
        //    }
        //    ViewBag.CollectionPt = new SelectList(db.CollectionPoint, "CollectionPtId", "CollectionPt", department.CollectionPt);
        //    return View(department);
        //}
        //Code from Antonio ends here
        //Code from Zhen Xiang begins below
        public ActionResult FindPO(string startDate, string endDate, string status)
        {
            List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();
            List<StoreStaff> creators = new List<StoreStaff>();

            //if (PONumber != null)
            //{
            //    int number = Convert.ToInt32(PONumber);
            //    purchaseOrders = purchaseOrderDAO.GetPurchaseOrders(number);
            //}
            if (startDate != null && endDate != null)
            {
                DateTime start = DateTime.Parse(startDate);
                DateTime end = DateTime.Parse(endDate);
                purchaseOrders = purchaseOrderDAO.GetPurchaseOrders(start, end);
            }
            else if (status != "")
            {
                purchaseOrders = purchaseOrderDAO.GetPurchaseOrders(status);
            }

            if (purchaseOrders.Count != 0)
            {
                foreach (PurchaseOrder po in purchaseOrders)
                {
                    StoreStaff storeStaff = storestaffDAO.GetStoreStaffbyPONumber(po.StaffId);
                    creators.Add(storeStaff);
                }
            }

            ViewData["creators"] = creators;
            ViewData["purchaseOrders"] = purchaseOrders;

            return View();
        }

        public ActionResult DetailedPO(int id)
        {
            PurchaseOrder po = new PurchaseOrder();
            StoreStaff creator = new StoreStaff();
            Supplier supplier = new Supplier();
            List<PurchaseOrderDetail> poDetails = new List<PurchaseOrderDetail>();
            List<Product> poProducts = new List<Product>();
            List<SupplierProduct> priceList = new List<SupplierProduct>();
            //method cannot be put in a seperate class (DAO), otherwise the other fields cannot be populated
            using (db = new LogicEntities())
            {
                var result = from o in db.PurchaseOrder
                             where o.OrderId == id
                             select o;
                po = result.First();
                var result2 = from ss in db.StoreStaff
                              where ss.StaffId == po.StaffId
                              select ss;
                creator = result2.First();
                supplier = po.Supplier;
                priceList = po.Supplier.SupplierProduct.ToList();
                poDetails = po.RequestDetails.ToList();

                foreach (PurchaseOrderDetail pod in poDetails)
                {
                    poProducts.Add(pod.Product);
                }
            }

            ViewData["PO"] = po;
            ViewData["creator"] = creator;
            ViewData["supplier"] = supplier;
            ViewData["poDetails"] = poDetails;
            ViewData["priceList"] = priceList;

            return View();
        }

        public ActionResult ApprovePO(string status, string remarks, int id)
        {
            purchaseOrderDAO.SetPOStatus(status, remarks, id);
            PurchaseOrder purchaseOrder = purchaseOrderDAO.GetPurchaseOrder(id);
            StoreStaff storeStaff = storestaffDAO.GetStaffbyId(purchaseOrder.StaffId);
            List<string> emails = new List<string>();
            emails.Add(storeStaff.StaffEmail);
            string subj = "Status of PO: " + purchaseOrder.OrderId.ToString();
            string msg = "Dear " + storeStaff.StaffName + ",\n Your PO: " + purchaseOrder.OrderId.ToString() + " has been " + purchaseOrder.Status + ".";
            EmailService.SendEmail(emails, subj, msg);
            return RedirectToAction("FindPO");
        }
        //Code Written By Zhen Xiang ends here
        //Code Below Written By Udaya
        public ActionResult ApproveForm(int id)
        {

            List<AdjustmentDetail> cart_list;

            using (LogicEntities db = new LogicEntities())
            {
                cart_list = db.AdjustmentDetail.Where(item => item.AdjustmentId == id).ToList();
                Adjustment req = db.Adjustment.Where(x => x.AdjustmentId == id).SingleOrDefault();

                ViewData["cart_list"] = cart_list;
                ViewData["adjustment"] = req;
                db.SaveChanges();
            }



            return View();
        }

        [HttpPost]
        public ActionResult ApproveForm(string approve, string reject, string remarks, int id)
        {

            using (LogicEntities db = new LogicEntities())
            {
                Adjustment rd = db.Adjustment.Find(id);
                StoreStaff staff = db.StoreStaff.Find(rd.StaffId);
                List<string> emails = new List<string>();
                emails.Add(staff.StaffEmail);
                string subj = "Status - Adjustment Form: " + rd.AdjustmentId;

                if (approve == "Approve")
                {
                    rd.Status = approve;
                    rd.Remark = remarks;

                    db.SaveChanges();

                    List<AdjustmentDetail> adjustmentDetails = db.AdjustmentDetail.Where(x => x.AdjustmentId == id).ToList();
                    foreach (var detail in adjustmentDetails)
                    {
                        var res = db.Product.SingleOrDefault(p => p.ProductId == detail.ProductId);
                        var res2 = db.StockTransaction.SingleOrDefault(p => p.ProductId == detail.ProductId);

                        if (res != null)
                        {
                            res.Qty += detail.Qty;
                            db.SaveChanges();
                        }

                        if (res2 != null)
                        {
                            res2.Qty += detail.Qty;
                            db.SaveChanges();
                        }
                    }
                    //Send email to notify that adjustment voucher has been approved
                    string msg = "Dear " + staff.StaffName + ",\n Your Adjustment voucher has been approved.";
                    EmailService.SendEmail(emails, subj, msg);
                }
                else if (reject == "Reject")
                {
                    rd.Status = reject;
                    rd.Remark = remarks;
                    db.SaveChanges();
                    //Send email to notify that adjustment has been rejected
                    string msg = "Dear " + staff.StaffName + ",\n Your Adjustment voucher has been rejected.";
                    EmailService.SendEmail(emails, subj, msg);
                }
            }
            return RedirectToAction("ViewVoucherList");
        }

        public ActionResult ChangeStatus()
        {
            Adjustment adjustment = new Adjustment();


            return View();
        }

        public ActionResult ViewVoucherList()
        {
            List<Adjustment> cart_list;

            using (LogicEntities db = new LogicEntities())
            {

                cart_list = db.Database.SqlQuery<Adjustment>("select distinct a.* from Adjustments a join AdjustmentDetails ad on a.AdjustmentId=ad.AdjustmentId where (select sum(x.TotalPrice) from AdjustmentDetails x where x.AdjustmentId=a.AdjustmentId group by x.AdjustmentId) < 250").ToList();

                db.SaveChanges();
            }
            ViewData["cart_list"] = cart_list;

            return View();
        }

        public static async Task<string> demo(int item, int mont)
        {
            string content = "";
            using (var httpClient = new HttpClient())
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://127.0.0.1:3000");

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.
                HttpResponseMessage response = client.GetAsync("?id=" + item + "&mon=" + mont).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    content = await response.Content.ReadAsStringAsync();
                    //dataObjects = Convert.ToString(response.Content);  //Make sure to add a reference to System.Net.Http.Formatting.dll
                    //Debug.WriteLine(content);
                }
                else
                {
                    //Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            return content;
        }

        public ActionResult Chart(string MONID, string ITEMID)
        {
            List<string> values_ = new List<string>();
            List<string> keys_ = new List<string>();
            Dictionary<string, string> dict_ = new Dictionary<string, string>();
            ViewBag.QTYS = values_;
            ViewBag.MONS = keys_;
            ViewBag.dict = dict_;
            ViewBag.monthid = (MONID == null) ? "1" : MONID;
            ViewBag.itemid = (ITEMID == null) ? "1" : ITEMID;
            ViewBag.status = false;
            return View();
        }

        public ActionResult GenChart(string ITEMID, string MONID)
        {

            List<string> values_ = new List<string>();
            List<string> keys_ = new List<string>();
            Dictionary<string, string> dict_ = new Dictionary<string, string>();

            int cat = int.Parse(ITEMID);
            int mon = int.Parse(MONID);
            if (cat != 0 && mon != 0)
            {
                JObject obj = JObject.Parse(demo(cat, mon).Result);

                JObject result = (JObject)obj["result"];

                foreach (var i in result)
                {
                    //Debug.WriteLine(i);
                    values_.Add((string)i.Value);
                    //Debug.WriteLine(((string)i.Value));
                    keys_.Add((string)i.Key);
                    //Debug.WriteLine(((string)i.Key));

                    dict_[(string)i.Key] = (string)i.Value;

                }
            }

            //ViewBag.QTYS = values_;
            //ViewBag.MONS = keys_;
            ViewBag.dict = dict_;
            ViewBag.monthid = MONID;
            ViewBag.itemid = ITEMID;
            ViewBag.status = true;

            return View("Chart", new { MONID = MONID, ITEMID = ITEMID });

        }
    }
}