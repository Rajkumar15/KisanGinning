using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KisanWeighining.Models;
using DataLayer;
using DataLayer.KisanModel;
using System.Linq.Dynamic;
using System.Data;
using KisanWeighining.Models.KisanModel;



namespace KisanWeighining.Controllers
{
    public class SystemMasterController : Controller
    {
        // Start Dependencies Object
        #region
        public readonly Irepository<tbl_UserDetails> _userdetail;
        public readonly Irepository<tbl_truckMaster> _truckmaster;
        public readonly Irepository<tbl_FarmerMaster> _farmermaster;
        public readonly Irepository<tbl_productMaster> _productmaster;
        public readonly Irepository<tbl_DealerMaster> _dealermaster;
        public readonly Irepository<tbl_BrokerMaster> _brokermaster;
        public readonly Irepository<tbl_TransportMaster> _transportmaster;
        public readonly Irepository<tbl_TaxMaster> _taxmaster;
        public readonly Irepository<tbl_ProductTaxEntry> _producttaxmaster;
        public readonly Irepository<tbl_ExpensesPurposeMaster> _ExpensesPurpose;
        public SystemMasterController(Irepository<tbl_UserDetails> _detail, Irepository<tbl_truckMaster> truckmaster, Irepository<tbl_FarmerMaster> farmermaster,
            Irepository<tbl_productMaster> productmaster, Irepository<tbl_DealerMaster> dealermaster, Irepository<tbl_BrokerMaster> brokermaster,
            Irepository<tbl_TransportMaster> transportmaster, Irepository<tbl_TaxMaster> taxmaster, Irepository<tbl_ProductTaxEntry> producttaxmaster,
            Irepository<tbl_ExpensesPurposeMaster> expensePurpose)
        {
            _userdetail = _detail;
            _truckmaster = truckmaster;
            _farmermaster = farmermaster;
            _productmaster = productmaster;
            _dealermaster = dealermaster;
            _brokermaster = brokermaster;
            _transportmaster = transportmaster;
            _taxmaster = taxmaster;
            _producttaxmaster = producttaxmaster;
            _ExpensesPurpose = expensePurpose;
        }
        #endregion
        // GET: SystemMaster

        [AllowAnonymous]
        public ActionResult InfoRegister(int userid)
        {
           
            try
            {
                ViewBag.Title = TempData["userid"].ToString();
            }
            catch { }
            try
            {
                ViewBag.Email = TempData["Username"].ToString();
            }
            catch { }
            var model = _userdetail.GetAll().Where(x => x.pkid == userid).FirstOrDefault();
            if (model != null)
            {
                tbl_UserDetailss abc = new tbl_UserDetailss();
                abc.pkid = model.pkid;
                abc.firstname = model.firstname;
                abc.middlename = model.middlename;
                abc.lastname = model.lastname;
                abc.Address1 = model.Address1;
                abc.Address2 = model.Address2;
                abc.mobileno = model.mobileno;
                abc.emailid = model.emailid;
                abc.User_fkid = model.User_fkid;
                return View(abc);
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult InfoRegister(tbl_UserDetailss model)
        {
            if (model.pkid == 0)
            {
               tbl_UserDetails abc = new tbl_UserDetails();
                abc.firstname = model.firstname;
                abc.middlename = model.middlename;
                abc.lastname = model.lastname;
                abc.Address1 = model.Address1;
                abc.Address2 = model.Address2;
                abc.mobileno = model.mobileno;
                abc.emailid = model.emailid;
                abc.User_fkid = model.User_fkid;
                _userdetail.Add(abc);
            }
            else{
              tbl_UserDetails abc = _userdetail.Get(model.pkid);
                abc.pkid=model.pkid;
                 abc.firstname = model.firstname;
                abc.middlename = model.middlename;
                abc.lastname = model.lastname;
                abc.Address1 = model.Address1;
                abc.Address2 = model.Address2;
                abc.mobileno = model.mobileno;
                abc.emailid = model.emailid;
                abc.User_fkid = model.User_fkid;
                _userdetail.Update(abc);
            }
            return RedirectToAction("Register","Account");
        }
        [HttpGet]
        public ActionResult UserList()
        {
            return View();
        }
        public ActionResult GetUserList()
        {
            var search = Request.Form.GetValues("search[value]")[0];
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Find Order Column
            string sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            //dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            try
            {
                var v = (from a in _userdetail.GetAll()
                         select new
                         {
                             pkid = a.pkid,
                             fullname = a.firstname + "" + a.lastname,
                             add = a.Address1,
                             mob = a.mobileno,
                             emailid = a.emailid
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.fullname.ToLower().Contains(search.ToLower()) || x.add.ToLower().Contains(search.ToLower()) || x.mob.Contains(search) || x.emailid.ToLower().Contains(search.ToLower())) select b);
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    v = v.OrderBy(sortColumn, sortColumnDir);
                }
                recordsTotal = v.Count();
                var data = v.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);

            }
        }

         // Start product master
        [HttpGet]
        public ActionResult ProductMaster(int _id = 0)
        {
            if (_id > 0)
            {
                var data = _productmaster.Get(_id);
                tbl_productMasterss abc = new tbl_productMasterss();
                abc.pkid = data.pkid;
                abc.productname = data.productname;
                abc.productdescription = data.productdescription;        
                return View(abc);
            }
            else
            {
                return View();
            }          
        }
        [HttpPost]
        public ActionResult ProductMaster(tbl_productMasterss model)
        {         
                if (model.pkid == 0)
                {
                    tbl_productMaster abc = new tbl_productMaster();
                    abc.productname = model.productname;
                    abc.productdescription = model.productdescription;
                    _productmaster.Add(abc);
                }
                else
                {
                    tbl_productMaster abc = _productmaster.Get(model.pkid);
                    abc.productname = model.productname;
                    abc.productdescription = model.productdescription;
                    _productmaster.Update(abc);
                }
                return RedirectToAction("ProductMaster", "SystemMaster");           
        }
        public ActionResult ProductList()
        {
            var search = Request.Form.GetValues("search[value]")[0];
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Find Order Column
            string sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            //dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            try
            {
                var v = _productmaster.GetAll();
                //SORT
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.productname.ToLower().Contains(search.ToLower()) || x.productdescription.ToLower().Contains(search.ToLower())) select b);
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    v = v.OrderBy(sortColumn, sortColumnDir);
                    //v = v.OrderBy(x=>x.orderfkid);
                }
                recordsTotal = v.Count();
                var data = v.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);

            }
        }      


        // start truck master
        [HttpGet]
        public ActionResult TruckMaster(int _id = 0)
        {
            if (_id > 0)
            {
                var data = _truckmaster.Get(_id);
                tbl_truckMasterss abc = new tbl_truckMasterss();
                abc.pkid = data.pkid;
                abc.truckno = data.truckno;
                abc.tairweight = data.tairweight;
                abc.capcity = data.capcity;
                abc.trucktype = data.trucktype;       
                return View(abc);
            }
            else
            {
                return View();
            }         
        }
        [HttpPost]
        public ActionResult TruckMaster(tbl_truckMasterss model)
        {
            if (model.pkid == 0)
            {
                tbl_truckMaster abc = new tbl_truckMaster();
                var abcoperation = _truckmaster.GetAll();
                 int maxid = 1;
                try
                {
                    maxid = abcoperation.Max(u => u.pkid);
                    maxid++;
                }               
               catch{                      
                }
                abc.RFIDnumber = "00" + maxid;
                abc.truckno = model.truckno;
                abc.tairweight = model.tairweight;
                abc.capcity = model.capcity;
                abc.trucktype = model.trucktype;              
                _truckmaster.Add(abc);
            }
            else
            {
                tbl_truckMaster abc = _truckmaster.Get(model.pkid);
                abc.truckno = model.truckno;
                abc.tairweight = model.tairweight;
                abc.capcity = model.capcity;
                abc.trucktype = model.trucktype;  
                _truckmaster.Update(abc);
            }
            return RedirectToAction("TruckMaster", "SystemMaster");
        }
        public ActionResult truckList()
       {
            var search = Request.Form.GetValues("search[value]")[0];
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Find Order Column
            string sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            //dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            try
            {
                var v = _truckmaster.GetAll();
                var asd = v.Count();
                //SORT
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.RFIDnumber.Contains(search) || x.truckno.Contains(search)) select b);
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    v = v.OrderBy(sortColumn, sortColumnDir);
                    //v = v.OrderBy(x=>x.orderfkid);
                }
               recordsTotal = v.Count();
               var psd = v.Count();
                var data = v.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);

            }
        }      
        //farmer entry start
        [HttpGet]
        public ActionResult farmer(int _id = 0)
        {
            if (_id > 0)
            {
                var data = _farmermaster.Get(_id);
                tbl_FarmerMasterss abc = new tbl_FarmerMasterss();
                abc.pkid = data.pkid;
                abc.fullname = data.fullname;
                abc.address1 = data.address1;
                abc.address2 = data.address2;
                abc.mobileno = data.mobileno;
                abc.villagename = data.villagename;
                return View(abc);
            }
            else
            {
                return View();
            }         
        }
        [HttpPost]
        public ActionResult farmer(tbl_FarmerMasterss model)
        {
            if (model.pkid == 0)
            {
                tbl_FarmerMaster abc = new tbl_FarmerMaster();
                abc.fullname = model.fullname;
                abc.address1 = model.address1;
                abc.address2 = model.address2;
                abc.mobileno = model.mobileno;
                abc.villagename = model.villagename;
                _farmermaster.Add(abc);
            }
            else {
                tbl_FarmerMaster abc = _farmermaster.Get(model.pkid);
                abc.pkid = model.pkid;
                abc.fullname = model.fullname;
                abc.address1 = model.address1;
                abc.address2 = model.address2;
                abc.mobileno = model.mobileno;
                abc.villagename = model.villagename;
                _farmermaster.Update(abc);
            }
            return RedirectToAction("farmer", "SystemMaster");
        }
        public ActionResult FarmerList()
        {
            var search = Request.Form.GetValues("search[value]")[0];
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Find Order Column
            string sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            //dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            try
            {
                var v = _farmermaster.GetAll();
                //SORT
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.fullname.ToLower().Contains(search.ToLower())) select b);
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    v = v.OrderBy(sortColumn, sortColumnDir);
                    //v = v.OrderBy(x=>x.orderfkid);
                }
                recordsTotal = v.Count();
                var data = v.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);

            }
        }       
        //Dealer entry start
        [HttpGet]
        public ActionResult Dealer(int _id = 0)
        {
            if (_id > 0)
            {
                var data = _dealermaster.Get(_id);
                tbl_DealerMasterss abc = new tbl_DealerMasterss();
                abc.pkid = data.pkid;
                abc.fullname = data.fullname;
                abc.address1 = data.address1;
                abc.address2 = data.address2;
                abc.mobileno = data.mobileno;
                abc.emailid = data.emailid;
                return View(abc);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Dealer(tbl_DealerMasterss model)
        {
            if (model.pkid == 0)
            {
                tbl_DealerMaster abc = new tbl_DealerMaster();
                abc.fullname = model.fullname;
                abc.address1 = model.address1;
                abc.address2 = model.address2;
                abc.mobileno = model.mobileno;
                abc.emailid = model.emailid;
                _dealermaster.Add(abc);
            }
            else
            {
                tbl_DealerMaster abc = _dealermaster.Get(model.pkid);
                abc.pkid = model.pkid;
                abc.fullname = model.fullname;
                abc.address1 = model.address1;
                abc.address2 = model.address2;
                abc.mobileno = model.mobileno;
                abc.emailid = model.emailid;
                _dealermaster.Update(abc);
            }
            return RedirectToAction("Dealer", "SystemMaster");
        }
        public ActionResult DealerList()
        {
            var search = Request.Form.GetValues("search[value]")[0];
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Find Order Column
            string sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            //dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            try
            {
                var v = _dealermaster.GetAll();
                //SORT
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.fullname.ToLower().Contains(search.ToLower()) || x.address1.ToLower().Contains(search.ToLower()) || x.address2.ToLower().Contains(search.ToLower()) || x.mobileno.ToLower().Contains(search.ToLower()) || x.emailid.ToLower().Contains(search.ToLower())) select b);
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    v = v.OrderBy(sortColumn, sortColumnDir);
                    //v = v.OrderBy(x=>x.orderfkid);
                }
                recordsTotal = v.Count();
                var data = v.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);

            }
        }
        // Broker entry start
        [HttpGet]
        public ActionResult BrokerMaster(int _id = 0)
        {
            if (_id > 0)
            {
                var data = _brokermaster.Get(_id);
                tbl_BrokerMasterss abc = new tbl_BrokerMasterss();
                abc.pkid = data.pkid;
                abc.Brokername = data.Brokername;
                abc.address1 = data.address1;
                abc.address2 = data.address2;
                abc.mobile = data.mobile;
                abc.emailid = data.emailid;
                return View(abc);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult BrokerMaster(tbl_BrokerMasterss model)
        {
            if (model.pkid == 0)
            {
                tbl_BrokerMaster abc = new tbl_BrokerMaster();
                abc.Brokername = model.Brokername;
                abc.address1 = model.address1;
                abc.address2 = model.address2;
                abc.mobile = model.mobile;
                abc.emailid = model.emailid;
                _brokermaster.Add(abc);
            }
            else
            {
                tbl_BrokerMaster abc = _brokermaster.Get(model.pkid);
                abc.pkid = model.pkid;
                abc.Brokername = model.Brokername;
                abc.address1 = model.address1;
                abc.address2 = model.address2;
                abc.mobile = model.mobile;
                abc.emailid = model.emailid;
                _brokermaster.Update(abc);
            }
            return RedirectToAction("BrokerMaster", "SystemMaster");
        }
        public ActionResult BrokerList()
        {
            var search = Request.Form.GetValues("search[value]")[0];
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Find Order Column
            string sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            //dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            try
            {
                var v = _brokermaster.GetAll();
                //SORT
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.Brokername.ToLower().Contains(search.ToLower()) || x.address1.ToLower().Contains(search.ToLower()) || x.address2.ToLower().Contains(search.ToLower()) || x.mobile.Contains(search) || x.emailid.ToLower().Contains(search.ToLower())) select b);
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    v = v.OrderBy(sortColumn, sortColumnDir);
                    //v = v.OrderBy(x=>x.orderfkid);
                }
                recordsTotal = v.Count();
                var data = v.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);

            }
        }

        // Transport entry start
        [HttpGet]
        public ActionResult TransportMaster(int _id = 0)
        {
            if (_id > 0)
            {
                var data = _transportmaster.Get(_id);
                tbl_TransportMasterss abc = new tbl_TransportMasterss();
                abc.pkid = data.pkid;
                abc.transportname = data.transportname;
                abc.address1 = data.address1;
                abc.address2 = data.address2;
                abc.mobileno = data.mobileno;
                abc.emailid = data.emailid;
                abc.ownername = data.ownername;
                abc.ownermobileno = data.ownermobileno;
                return View(abc);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult TransportMaster(tbl_TransportMasterss model)
        {
            if (model.pkid == 0)
            {
                tbl_TransportMaster abc = new tbl_TransportMaster();
                abc.pkid = model.pkid;
                abc.transportname = model.transportname;
                abc.address1 = model.address1;
                abc.address2 = model.address2;
                abc.mobileno = model.mobileno;
                abc.emailid = model.emailid;
                abc.ownername = model.ownername;
                abc.ownermobileno = model.ownermobileno;
                _transportmaster.Add(abc);
            }
            else
            {
                tbl_TransportMaster abc = _transportmaster.Get(model.pkid);
                abc.pkid = model.pkid;
                abc.transportname = model.transportname;
                abc.address1 = model.address1;
                abc.address2 = model.address2;
                abc.mobileno = model.mobileno;
                abc.emailid = model.emailid;
                abc.ownername = model.ownername;
                abc.ownermobileno = model.ownermobileno;
                _transportmaster.Update(abc);
            }
            return RedirectToAction("TransportMaster", "SystemMaster");
        }
        public ActionResult TransportList()
        {
            var search = Request.Form.GetValues("search[value]")[0];
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Find Order Column
            string sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            //dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            try
            {
                var v = _transportmaster.GetAll();
                //SORT
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.transportname.ToLower().Contains(search.ToLower()) || x.address1.ToLower().Contains(search.ToLower()) || x.address2.ToLower().Contains(search.ToLower()) || x.mobileno.Contains(search) || x.emailid.ToLower().Contains(search.ToLower()) || x.ownername.ToLower().Contains(search.ToLower()) || x.ownermobileno.Contains(search)) select b);
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    v = v.OrderBy(sortColumn, sortColumnDir);
                    //v = v.OrderBy(x=>x.orderfkid);
                }
                recordsTotal = v.Count();
                var data = v.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);

            }
        }

        // tax master entry
        [HttpGet]
        public ActionResult taxmaster(int _id = 0)
        {
            if (_id > 0)
            {
                var data = _taxmaster.Get(_id);
                tbl_TaxMasterss abc = new tbl_TaxMasterss();
                abc.pkid = data.pkid;
                abc.Saletype = data.Saletype;
                abc.taxper = data.taxper;
                abc.CGST = data.CGST;
                abc.SGST = data.SGST;
                abc.IGST = data.IGST;               
                return View(abc);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult taxmaster(tbl_TaxMasterss model)
        {
            if (model.pkid == 0)
            {
                tbl_TaxMaster abc = new tbl_TaxMaster();
                abc.pkid = model.pkid;
                abc.Saletype = model.Saletype;
                abc.taxper = model.taxper;
                abc.CGST = model.CGST;
                abc.SGST = model.SGST;
                abc.IGST = model.IGST;     
                _taxmaster.Add(abc);
            }
            else
            {
                tbl_TaxMaster abc = _taxmaster.Get(model.pkid);
                abc.pkid = model.pkid;
                abc.Saletype = model.Saletype;
                abc.taxper = model.taxper;
                abc.CGST = model.CGST;
                abc.SGST = model.SGST;
                abc.IGST = model.IGST;    
                _taxmaster.Update(abc);
            }
            return RedirectToAction("taxmaster", "SystemMaster");
        }
        public ActionResult taxList()
        {
            var search = Request.Form.GetValues("search[value]")[0];
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Find Order Column
            string sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            //dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            try
            {
                var v = _taxmaster.GetAll();
                //SORT
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.Saletype.ToLower().Contains(search.ToLower())) select b);
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    v = v.OrderBy(sortColumn, sortColumnDir);
                    //v = v.OrderBy(x=>x.orderfkid);
                }
                recordsTotal = v.Count();
                var data = v.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);

            }
        }

        //product tax Master
        [HttpGet]
        public ActionResult Prodtaxmaster(int _id = 0)
        {
            ViewBag.pro = new SelectList(_productmaster.GetAll(), "pkid", "productname");
            ViewBag.tax = new SelectList(_taxmaster.GetAll(), "pkid", "Saletype");
            if (_id > 0)
            {
                var data = _producttaxmaster.Get(_id);
                tbl_ProductTaxEntryss abc = new tbl_ProductTaxEntryss();
                abc.pkid = data.pkid;
                abc.Product_fkid = data.Product_fkid;
                abc.HSNno = data.HSNno;
                abc.tax_fkid = data.tax_fkid;             
                return View(abc);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Prodtaxmaster(tbl_ProductTaxEntryss model)
        {
            ViewBag.pro = new SelectList(_productmaster.GetAll(), "pkid", "productname");
            ViewBag.tax = new SelectList(_taxmaster.GetAll(), "pkid", "Saletype");
            if (model.pkid == 0)
            {
                tbl_ProductTaxEntry abc = new tbl_ProductTaxEntry();
                abc.pkid = model.pkid;
                abc.pkid = model.pkid;
                abc.Product_fkid = model.Product_fkid;
                abc.HSNno = model.HSNno;
                abc.tax_fkid = model.tax_fkid;      
                _producttaxmaster.Add(abc);
            }
            else
            {
                tbl_ProductTaxEntry abc = _producttaxmaster.Get(model.pkid);
                abc.pkid = model.pkid;
                abc.pkid = model.pkid;
                abc.Product_fkid = model.Product_fkid;
                abc.HSNno = model.HSNno;
                abc.tax_fkid = model.tax_fkid;
                _producttaxmaster.Update(abc);
            }
            return RedirectToAction("Prodtaxmaster", "SystemMaster");
        }
        public ActionResult prodtaxList()
        {
            var search = Request.Form.GetValues("search[value]")[0];
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Find Order Column
            string sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            //dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            try
            {
                var v =  (from a in _producttaxmaster.GetAll()
                         join b in _productmaster.GetAll() on a.Product_fkid equals b.pkid
                          join c in _taxmaster.GetAll() on a.tax_fkid equals c.pkid
                         select new
                         {
                             pkid = a.pkid,
                           produname = b.productname,
                           HSNCODE = a.HSNno,
                           taxtype = c.Saletype
                         });
                //SORT
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.produname.ToLower().Contains(search.ToLower())) select b);
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    v = v.OrderBy(sortColumn, sortColumnDir);
                    //v = v.OrderBy(x=>x.orderfkid);
                }
                recordsTotal = v.Count();
                var data = v.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpGet]
        public ActionResult ExpensesPurpose(int _id = 0)
        {
            if (_id > 0)
            {
                var data = _ExpensesPurpose.Get(_id);
                tbl_ExpensesPurposeMasterss abc = new tbl_ExpensesPurposeMasterss();
                abc.pkid = data.pkid;
                abc.purposename = data.purposename;              
                return View(abc);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult ExpensesPurpose(tbl_ExpensesPurposeMaster model)
        {
            if (model.pkid == 0)
            {
                tbl_ExpensesPurposeMaster abc = new tbl_ExpensesPurposeMaster();
                abc.purposename = model.purposename;
                abc.mdate = DateTime.Now;
                _ExpensesPurpose.Add(abc);
            }
            else
            {
                tbl_ExpensesPurposeMaster abc = _ExpensesPurpose.Get(model.pkid);
                abc.purposename = model.purposename;
                abc.mdate = DateTime.Now;
                _ExpensesPurpose.Update(abc);
            }
            return RedirectToAction("ExpensesPurpose", "SystemMaster");
        }
        public ActionResult expenseslist()
        {
            var search = Request.Form.GetValues("search[value]")[0];
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Find Order Column
            string sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            //dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            try
            {
                var v = (from a in _ExpensesPurpose.GetAll()
                        
                         select new
                         {
                             pkid = a.pkid,
                             purname = a.purposename,
                             dat = a.mdate,                            
                         });
                //SORT
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.purname.ToLower().Contains(search.ToLower())) select b);
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    v = v.OrderBy(sortColumn, sortColumnDir);
                    //v = v.OrderBy(x=>x.orderfkid);
                }
                recordsTotal = v.Count();
                var data = v.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}