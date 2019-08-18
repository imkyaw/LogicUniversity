using logicProject.Models.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using logicProject.Models.EF;
using logicProject.Models.ViewModel;
using System.Data.Entity;
using System.Net;
using logicProject.Models.DAO;
using logicProject.Utility;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace logicProject.Controllers
{
    public class InventoryController : Controller
    {
        private LogicEntities db = new LogicEntities();
        private PurchaseOrderDAO purchaseOrderDAO = new PurchaseOrderDAO();
        private SupplierDAO supplierDAO = new SupplierDAO();
        private SupplierProductDAO supplierProductDAO = new SupplierProductDAO();
        private StockTransactionDAO stockTransactionDAO = new StockTransactionDAO();
        private ProductDAO productDAO = new ProductDAO();
        private DepartmentDAO departmentDAO = new DepartmentDAO();
        private DisbursementDAO disbursementDAO = new DisbursementDAO();
        private StoreStaffDAO storestaffDAO = new StoreStaffDAO();
        private DepartmentStaffDAO departmentStaffDAO = new DepartmentStaffDAO();
        private CollectionPointDAO collectionPointDAO = new CollectionPointDAO();
        // GET: Inventory
        public ActionResult Index()
        {
            return View();
        }
        //Antonio's inventory catalogue controller methods should appear here
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

        public ActionResult StockCardDetails(string id, string startDate, string endDate)
        {
            Product product = db.Product.Find(id);

            List<SupplierProduct> supplierproduct = db.SupplierProduct.OrderBy(pro => pro.Price).Where(pro => pro.ProductId == id).ToList();
            ViewData["supplierproduct"] = supplierproduct;

            if(startDate!= null && endDate!=null)
            {
                DateTime start = DateTime.Parse(startDate);
                DateTime end = DateTime.Parse(endDate);
                List<StockTransaction> stockCardDetails = db.StockTransaction.Where(p => p.ProductId == id && p.TranDate > start && p.TranDate < end).ToList();
                ViewData["stockcarddetails"] = stockCardDetails;
            }
            else
            {
                List<StockTransaction> stockCardDetails = db.StockTransaction.Where(p => p.ProductId == id).ToList();
                ViewData["stockcarddetails"] = stockCardDetails;
            }

            return View(product);
        }

        public ActionResult CreateProduct()
        {

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProduct([Bind(Include = "ProductId,Category,Description,Bin,Unit,Qty,ReorderLevel,ReorderQty")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("ProductDetails", new { category = product.Category });
            }
            return View(product);
        }


        // GET: Products/Edit/5
        public ActionResult EditProduct(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct([Bind(Include = "ProductId,Category,Description,Bin,Unit,Qty,ReorderLevel,ReorderQty")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ProductDetails", new { category = product.Category });
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult DeleteProduct(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedProduct(string id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("ProductDetails", new { category = product.Category });
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

        public ActionResult EditDepartmentDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Department.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.CollectionPt = new SelectList(db.CollectionPoint, "CollectionPtId", "CollectionPt", department.CollectionPt);

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDepartmentDetails([Bind(Include = "DeptId,DeptName,ContactName,PhNo,FaxNo,EmailAddr,HeadId,CollectionPt,RepId")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DepartmentDetails");
            }
            ViewBag.CollectionPt = new SelectList(db.CollectionPoint, "CollectionPtId", "CollectionPt", department.CollectionPt);
            return View(department);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //Code from Antonio ends here
        //Code from Prasanth begins

        public ActionResult OrderStatus()
        {
            var request = db.Request.Include(d => d.Department);
            return View(request.ToList());
        }
        public ActionResult RequestForm()
        {
            return View();
        }

        public ActionResult RequestsList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RequestsList(FormCollection values)
        {
            string fromdate = values["FromDate"];
            string todate = values["Todate"];

            DateTime fromDate = DateTime.Parse(fromdate);
            DateTime toDate = DateTime.Parse(todate);

            ViewBag.Fromdate = fromdate;
            ViewBag.Todate = todate;


            if (fromDate > toDate)
            {
                ViewBag.msg = "Please select the Correct Date";
                return View();
            }

            else
            {
                List<RequestListViewModel> list = new List<RequestListViewModel>();
                //var requests = db.Request.Where(r => r.Status == "Outstanding").Include(d => d.Department);
                var request = (from r in db.Request join d in db.Department on r.DeptId equals d.DeptId join ds in db.DepartmentStaff on r.StaffId equals ds.StaffId join cp in db.CollectionPoint on d.CollectionPt equals cp.CollectionPtId where (r.ReqDate >= fromDate && r.ReqDate <= toDate) || r.Status == "Outstanding" select new { department = d, requests = r, staff = ds, collectiopoint = cp }).ToList();
                foreach (var a in request)
                {
                    list.Add(new RequestListViewModel()
                    {
                        department = a.department,
                        staff = a.staff,
                        requests = a.requests,
                        collectionPoint = a.collectiopoint
                    });
                }
                return View(list);
            }
        }

        public ActionResult Requestitems(int id)
        {

            ViewBag.Id = id;
            List<RequestItemsViewModel> list = new List<RequestItemsViewModel>();
            var requestitems = (from rd in db.RequestDetail join p in db.Product on rd.ProductId equals p.ProductId where rd.RequestId == id select new { requestDetail = rd, product = p }).ToList();
            foreach (var b in requestitems)
            {
                list.Add(new RequestItemsViewModel()
                {
                    requestDetail = b.requestDetail,
                    product = b.product

                });
            }

            return View(list);
        }
        //Code from Prasanth ends here
        //Code from Udaya
        public ActionResult ViewInventory()
        {
            List<Product> items;

            using (LogicEntities db = new LogicEntities())
            {
                items = db.Product.ToList();
            }
            ViewData["items"] = items;

            return View();
        }
        public ActionResult ViewLowStock()
        {
            List<Product> items;

            using (LogicEntities db = new LogicEntities())
            {
                items = db.Product.Where(item => item.Qty < item.ReorderLevel).ToList();
                db.SaveChanges();

            }
            ViewData["items"] = items;

            return View();
        }
        public ActionResult create_form()
        {
            StoreStaff user = new StoreStaff();

            user = (StoreStaff)Session["StoreStaff"];
            using (LogicEntities db = new LogicEntities())
            {
                db.Adjustment.RemoveRange(db.Adjustment.Where(x => x.Status == "temp").ToList());
                db.SaveChanges();
            }
            int form_number = 1;
            using (LogicEntities db = new LogicEntities())
            {
                if (db.Adjustment.AsEnumerable().ToList().Count != 0)
                {
                    form_number = db.Adjustment.AsEnumerable().Max(p => p.AdjustmentId);
                    //Debug.WriteLine("***************** before" + form_number);
                    form_number = form_number + 1;
                    //Debug.WriteLine("***************** after" + form_number);
                }
                else
                {
                    form_number = 1;
                }
            }



            //Debug.WriteLine("*****************" + form_number);
            using (LogicEntities db = new LogicEntities())
            {
                db.Adjustment.Add(new Adjustment { AdjustmentId = form_number, Remark = "--", AdjustedDate = DateTime.Now.Date, StaffId = user.StaffId, Status = "temp" });
                db.SaveChanges();
            }

            List<ProductViewModel> items;

            int num_new;
            using (LogicEntities db = new LogicEntities())
            {

                //items = db.Product.Select(p => new Product
                //{
                //    ProductId = p.ProductId,
                //    Category = p.Category,
                //    Description = p.Description,
                //    Qty = p.Qty
                //}).ToList();

                //items = db.Product.ToList();
                items = getProducts(db);
                num_new = db.Adjustment.AsEnumerable().Max(p => p.AdjustmentId);

            }

            ViewData["form_number"] = num_new;
            ViewData["items"] = items;
            ViewData["msg"] = num_new.ToString();

            return View();

        }

        private List<ProductViewModel> getProducts(LogicEntities db)
        {
            List<ProductViewModel> items = (from p in db.Product
                                            join pod in db.PurchaseOrderDetail on p.ProductId equals pod.ProductId
                                            join po in db.PurchaseOrder on pod.OrderId equals po.OrderId
                                            join spd in db.SupplierProduct on po.SupplierId equals spd.SupplierId
                                            where spd.ProductId == pod.ProductId
                                            select new ProductViewModel
                                            {
                                                ProductId = p.ProductId,
                                                Category = p.Category,
                                                Description = p.Description,
                                                Qty = p.Qty,
                                                UnitPrice = spd.Price
                                            }).ToList<ProductViewModel>();

            items.Insert(0, new ProductViewModel
            {
                Description = "None",
                UnitPrice = 0
            });

            return items;
        }

        public ActionResult addnew(AdjustmentDetail form_detail_obj)
        {
            if (string.IsNullOrEmpty(form_detail_obj.ProductId))
            {
                return RedirectToAction("addcart");
            }

            using (LogicEntities db = new LogicEntities())
            {
                List<AdjustmentDetail> temp = db.AdjustmentDetail.ToList();

                int form_details_num = (db.AdjustmentDetail.AsEnumerable().ToList().Count == 0) ? 1 : (db.AdjustmentDetail.AsEnumerable().Max(p => p.AdjustmentDetailId)) + 1;

                AdjustmentDetail old_rec_to_update = (from f in db.AdjustmentDetail
                                                      where f.ProductId == form_detail_obj.ProductId && f.AdjustmentId == form_detail_obj.AdjustmentId
                                                      select f).FirstOrDefault();
                if (old_rec_to_update != null)
                {
                    old_rec_to_update.Qty += form_detail_obj.Qty;
                }
                else
                {
                    //Debug.WriteLine("____________" + form_details_num + "##" + form_detail_obj.AdjustmentId);
                    db.AdjustmentDetail.Add(new AdjustmentDetail { AdjustmentDetailId = form_details_num, AdjustmentId = form_detail_obj.AdjustmentId, ProductId = form_detail_obj.ProductId, Qty = Convert.ToInt32(form_detail_obj.Qty), UnitPrice = form_detail_obj.UnitPrice, TotalPrice = form_detail_obj.TotalPrice, reason = form_detail_obj.reason });
                }

                db.SaveChanges();
            }

            return RedirectToAction("addcart");
        }

        public ActionResult addcart()
        {
            StoreStaff user = (StoreStaff)Session["user"];
            int form_number;
            List<AdjustmentDetail> cart_list;
            List<ProductViewModel> items;
            using (LogicEntities db = new LogicEntities())
            {
                //items = db.Product.ToList();
                items = getProducts(db);
                form_number = db.Adjustment.AsEnumerable().Max(p => p.AdjustmentId);
                cart_list = db.AdjustmentDetail.Include(ad => ad.Product).Where(x => x.AdjustmentId == form_number).ToList();
            }

            ViewData["cart_list"] = cart_list;
            ViewData["form_number"] = form_number;
            ViewData["items"] = items;
            return View("create_form");
        }

        public ActionResult submitrequest()
        {
            int emp_id = int.Parse(Request.Form["emp_id"]);
            int form_number = int.Parse(Request.Form["form_number"]);

            using (LogicEntities db = new LogicEntities())
            {
                Adjustment old_rec_to_update = (from f in db.Adjustment
                                                where f.StaffId == emp_id && f.AdjustmentId == form_number && f.Status == "temp"
                                                select f).FirstOrDefault();
                if (old_rec_to_update != null)
                {

                    old_rec_to_update.Status = "Pending";

                }
                db.SaveChanges();
            }
            //Send email to either manager/supervisor 
            return View("index");
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
        //Code from Udaya ends here
        //Code Below onwards is written by Zhen Xiang
        [Route("createpo")]
        public ActionResult CreatePO()
        {
            ViewData["SupplierList"] = supplierDAO.GetSuppliers();
            return View();
        }

        [Route("getsupplieritemdetails")]
        public ActionResult GetSupplierItemDetails(string supplierId)
        {
            string detailsJson = productDAO.GetItemDetails(supplierId);

            return Json(detailsJson, JsonRequestBehavior.AllowGet);
        }

        [Route("getsupplieritemprices")]
        public ActionResult GetSupplierItemPrices(string supplierId)
        {
            string pricesJson = supplierProductDAO.GetSupplierItemPrices(supplierId);

            return Json(pricesJson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SavePO(string supplierId)
        {
            StoreStaff storeStaff = (StoreStaff)Session["StoreStaff"];
            StoreStaff supervisor = storestaffDAO.GetStoreStaffbyRole("Supervisor");
            string[] productIds = Request.Form.GetValues("itemCode");
            string[] requiredQty = Request.Form.GetValues("quantity");

            int id = storeStaff.StaffId;

            int poNum = purchaseOrderDAO.SaveNewPO(supplierId, productIds, requiredQty, id);
            List<string> emails = new List<string>();
            emails.Add(supervisor.StaffEmail);
            string subj = "PO for Approval";
            string msg = "Dear " + supervisor.StaffName + ",\n"+ 
                storeStaff.StaffName + " has created PO: " + poNum.ToString() + " for your approval." ;

            EmailService.SendEmail(emails, subj, msg);

            return RedirectToAction("FindPO");
        }

        public ActionResult FindPO(string startDate, string endDate, string status)
        {
            List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();
            List<StoreStaff> creators = new List<StoreStaff>();

            //if (PONumber != null && PONumber != "")
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

        [Route("detailedpo/{id:regex(\\d)?}")]
        public ActionResult DetailedPO(int id)
        {
            PurchaseOrder po = new PurchaseOrder();
            StoreStaff creator = new StoreStaff();
            Supplier supplier = new Supplier();
            List<PurchaseOrderDetail> poDetails = new List<PurchaseOrderDetail>();
            List<Product> poProducts = new List<Product>();
            List<SupplierProduct> priceList = new List<SupplierProduct>();
            //method cannot be put in a seperate class (DAO), otherwise the other fields cannot be populated
            using (db)
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

                //Not sure why, but this section of code prevents an exception from happening
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
        public ActionResult InventorySubMenu()
        {
            return View();
        }
        public ActionResult UpdateInventory()   //Not used
        {
            string itemCode = Request.Form.Get("itemCode");
            string purpose = Request.Form.Get("purpose");
            string transactionDetails = Request.Form.Get("transactionDetails");
            int qty = Convert.ToInt32(Request.Form.Get("quantity"));
            DateTime date = DateTime.Parse(Request.Form.Get("date"));

            if (itemCode != null)
            {
                Product p = productDAO.FindProductById(itemCode);
                if(-qty > p.Qty )   //Logic condition to check if enough items to withdraw
                {
                    //Error: Not that many items to withdraw
                }
                else
                {
                    productDAO.UpdateItemQty(itemCode, qty);
                    stockTransactionDAO.UpdateQuantityOneItem(itemCode, purpose, qty, date,transactionDetails);
                }
                
            }
            return View();
        }

        public ActionResult ReceiveInventory()
        {
            List<Supplier> suppliers = supplierDAO.GetSuppliers();
            ViewData["suppliers"] = suppliers;
            return View();
        }

        public ActionResult AddManyItems()
        {
            string[] itemCodes = Request.Form.GetValues("itemCode");
            string[] receivedQtys = Request.Form.GetValues("ReceivedQty");
            DateTime date = DateTime.Parse(Request.Form.Get("date"));
            string supplierId = Request.Form.Get("supplierId");
            string comments = Request.Form.Get("comments");

            stockTransactionDAO.AddManyItems(itemCodes, receivedQtys, date, supplierId, comments);
            productDAO.UpdateItemQty(itemCodes, receivedQtys, true);

            return RedirectToAction("InventorySubMenu");
        }
        public ActionResult CreateRetrievalForm()
        {
            DateTime cutoff = DateTime.Now.Date;
            List<RetrievalViewModel> retrievalList = new List<RetrievalViewModel>();
            using (db = new LogicEntities())
            {
                var resultSet = from rd in db.RequestDetail
                                from r in db.Request
                                where (r.Status.Contains("New") || r.Status.Contains("Outstanding"))
                                && r.RequestId == rd.RequestId && r.ReqDate < cutoff //Ensures only item requests before the day of order are withdrawn
                                group rd by rd.Product into g
                                select new RetrievalViewModel
                                {
                                    Product = g.Key,
                                    Quantity = g.Sum(x => x.ReqQty)
                                };
                retrievalList = resultSet.ToList();
            }
            foreach(RetrievalViewModel rvm in retrievalList)
            {
                rvm.MaxQuantity = productDAO.GetItemMaxQty(rvm.Product.ProductId);
            }
            ViewData["Retrieval"] = retrievalList;
            return View();
        }

        public ActionResult WithdrawItems()
        {
            string[] itemCodes = Request.Form.GetValues("itemCode");
            string[] retrievedQtys = Request.Form.GetValues("qtyRetrieved");

            stockTransactionDAO.WithdrawManyItems(itemCodes, retrievedQtys);
            productDAO.UpdateItemQty(itemCodes, retrievedQtys, false);

            return RedirectToAction("GenerateDisbursementLists");
        }

        //public ActionResult OutstandingDisbursementList()
        //{
        //    DateTime cutoff = DateTime.Now.Date;
        //    List<Department> departments = new List<Department>();
        //    departments = departmentDAO.GetDepartmentsWithOutstandingOrders(cutoff);

        //    ViewData["departments"] = departments;

        //    return View();
        //}

        public ActionResult GenerateDisbursementLists()
        {
            DateTime cutoff = DateTime.Now.Date;    //Ensures that orders only up to yesterday 2359 is taken into consideration
            List<Department> departments = departmentDAO.GetDepartmentsWithOutstandingOrders(cutoff);
            List<DisbursementDetailViewModel> consolidatedOrders = new List<DisbursementDetailViewModel>();

            foreach(Department d in departments)
            {
                DepartmentStaff rep = departmentStaffDAO.getDeptRep(d.DeptId);
                using (db = new LogicEntities())
                {                    
                    var resultSet = from rd in db.RequestDetail
                                    from r in db.Request
                                    where (r.Status.Contains("New") || r.Status.Contains("Outstanding"))
                                    && r.RequestId == rd.RequestId && r.DeptId == d.DeptId
                                    && r.ReqDate < cutoff
                                    group rd by new { rd.ProductId, r.DeptId } into g
                                    select new DisbursementDetailViewModel//Note to self: Groupby only works with enums or primitive data types
                                    {
                                        ProductId = g.Key.ProductId,
                                        DepartmentId = g.Key.DeptId,
                                        Quantity = g.Sum(x => x.ReqQty)
                                    };
                    consolidatedOrders = resultSet.ToList();

                    Disbursement dis = new Disbursement
                    {
                        DisDate = DateTime.Now,
                        Status = "Pending",
                        DeptId = d.DeptId,
                        StoreStaffId = null,
                        ReceiveStaffId = rep.StaffId,
                        CollectionPointId = d.CollectionPt
                    };
                    List<DisbursementDetail> disbursementdetails = new List<DisbursementDetail>();
                    foreach (DisbursementDetailViewModel p in consolidatedOrders)
                    {
                        DisbursementDetail dd = new DisbursementDetail
                        {
                            ProductId = p.ProductId,
                            RequiredQty = p.Quantity,
                            ReceivedQty = 0,
                            DisId = dis.DisId
                        };
                        disbursementdetails.Add(dd);
                    }
                    db.Disbursement.Add(dis);
                    db.DisbursementDetail.AddRange(disbursementdetails);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("FindDisbursementList");
        }


        //public ActionResult GenerateDisbursementList(string deptId)
        //{
        //    DateTime cutoff = DateTime.Now.Date;    //Ensures that orders only up to yesterday 2359 is taken into consideration
        //    List<DisbursementDetail> disbursementdetails = new List<DisbursementDetail>();
        //    Disbursement dis = new Disbursement();

        //    using (db)
        //    {
        //        var resultSet = from rd in db.RequestDetail
        //                        from r in db.Request
        //                        where (r.Status.Contains("New") || r.Status.Contains("Outstanding"))
        //                        && r.RequestId == rd.RequestId && r.DeptId == deptId
        //                        && r.ReqDate < cutoff
        //                        group rd by new { rd.ProductId, r.DeptId } into g
        //                        select new DisbursementDetailViewModel//Note to self: Groupby only works with enums or primitive data types
        //                        {
        //                            ProductId = g.Key.ProductId,
        //                            DepartmentId = g.Key.DeptId,
        //                            Quantity = g.Sum(x => x.ReqQty)
        //                        };
        //        List<DisbursementDetailViewModel> consolidatedOrders = resultSet.ToList();
        //        dis = new Disbursement
        //        {
        //            DisDate = DateTime.Now,
        //            Status = "Pending"
        //            //Field for department appears to be missing
        //            //dis.StoreStaff = null;
        //        };
        //        foreach (DisbursementDetailViewModel p in consolidatedOrders)
        //        {
        //            DisbursementDetail dd = new DisbursementDetail
        //            {
        //                ProductId = p.ProductId,
        //                //RequiredQty = p.Quantity
        //                ReceivedQty = 0,   //Should set to zero
        //                DisId = dis.DisId
        //            };
        //            disbursementdetails.Add(dd);
        //        }
        //        db.Disbursement.Add(dis);
        //        db.DisbursementDetail.AddRange(disbursementdetails);
        //        db.SaveChanges();
        //    }
                
        //    return RedirectToAction("DetailedDisbursementList", new { id = dis.DisId });
        //}
        public ActionResult FindDisbursementList(string deptId, string startDate, string endDate)
        {
            List<Department> departments = departmentDAO.GetDepartments();
            List<Disbursement> disbursements = new List<Disbursement>();

            if (deptId != null)
            {
             disbursements = disbursementDAO.GetDisbursements(deptId);
            }
            if(startDate != null && endDate != null)
            {
                disbursements = disbursementDAO.GetDisbursements(DateTime.Parse(startDate), DateTime.Parse(endDate));
            }
            
            ViewData["departments"] = departments;
            ViewData["disbursements"] = disbursements;

            return View();
        }

        [Route("detaileddisbursementlist/{id:regex(\\d)?}")]
        public ActionResult DetailedDisbursementList(int id)
        {
            Disbursement disbursement = new Disbursement();
            Department department = new Department();
            List <DisbursementDetail> disbursementDetails = new List<DisbursementDetail>();
            List<Product> disbursedProducts = new List<Product>();
            StoreStaff storeStaff = new StoreStaff();   //To create via the session object
            DepartmentStaff receiver = new DepartmentStaff();
            CollectionPoint cp = new CollectionPoint();

            using (db = new LogicEntities())
            {
                var result = from d in db.Disbursement
                             where d.DisId == id
                             select d;
                disbursement = result.First();
                department = disbursement.Department;
                disbursementDetails = disbursement.DisbursementDetails.ToList();
                foreach(DisbursementDetail dd in disbursementDetails)
                {
                    disbursedProducts.Add(dd.Product);
                }
            }
            if (disbursement.StoreStaffId != null)
            {
                storeStaff = storestaffDAO.GetStaffbyId(disbursement.StoreStaffId.Value);
            }
            else
            {
                storeStaff = (StoreStaff)Session["StoreStaff"];//StoreStaff from session
            }
            receiver = departmentStaffDAO.GetStaffById(disbursement.ReceiveStaffId);
            cp = collectionPointDAO.GetCollectionPoint(disbursement.CollectionPointId);
            ViewData["receiver"] = receiver;
            ViewData["storestaff"] = storeStaff;
            ViewData["disbursement"] = disbursement;
            ViewData["disbursementdetails"] = disbursementDetails;
            ViewData["CollectionPoint"] = cp;
            return View();
        }

        public ActionResult SetDisbursementList(int id, string status, string deptId)
        {
            string[] productIds = Request.Form.GetValues("productId");
            string[] requestedQtys = Request.Form.GetValues("requestedQty");
            string[] receivedQtys = Request.Form.GetValues("receivedQty");
            string[] remarks = Request.Form.GetValues("remarks");
            int storeStaffId = Convert.ToInt32(Request.Form.Get("creatorId"));
            //Update the item quantities && Set Disburesment status to delivered
            disbursementDAO.SaveDisbursement(id, status, receivedQtys, remarks, storeStaffId);
            DepartmentStaff rep = departmentStaffDAO.getDeptRep(deptId);
            //Set all the orders status to Delivered. Not DAO-ed
            using (db = new LogicEntities())
            {
                var resultSet = from r in db.Request
                                where r.DeptId == deptId
                                select r;
                List<Request> requests = resultSet.ToList();
                foreach(Request req in requests)
                {
                    req.Status = "Delivered";
                    db.Entry(req).State = EntityState.Modified;
                    db.SaveChanges();
                }
            //if Qty received < Qty Requested, Generate outstanding order in requests

                Request outstanding = new Request
                {
                    DeptId = deptId,
                    ReqDate = DateTime.Now,
                    Remark = "Outstanding Order, " + DateTime.Now.Date.ToString("d"),
                    Status = "Outstanding",
                    StaffId = rep.StaffId
                };
                List<RequestDetail> outstandingDetails = new List<RequestDetail>();
                for (int x = 0; x < requestedQtys.Length; x++)
                {
                    if(requestedQtys[x] != receivedQtys[x])
                    {
                        RequestDetail rd = new RequestDetail
                        {
                            ProductId = productIds[x],
                            RequestId = outstanding.RequestId,
                            ReqQty = Convert.ToInt32(requestedQtys[x]) - Convert.ToInt32(receivedQtys[x]),
                            ReceivedQty = 0, //Shouldn't be needed...
                        };
                        outstandingDetails.Add(rd);
                    }
                }
                if(outstandingDetails.Count != 0)
                {
                    db.Request.Add(outstanding);
                    db.RequestDetail.AddRange(outstandingDetails);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("FindDisbursementList");
        }
    }
}