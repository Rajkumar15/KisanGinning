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
using System.Data.SqlClient;
using System.IO;
using System.Data.OleDb;
using System.Configuration;
using itechDll;
using Newtonsoft.Json;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.Entity.Core.Objects;

namespace KisanWeighining.Controllers
{
    public class KisanSalesController : Controller
    {
        public readonly Irepository<tbl_RemoteConnection> _remote;
        public readonly Irepository<tbl_pool_MachineData_Purchase> _machinedata;
        public readonly Irepository<tbl_FarmerMaster> _farmer;
        public readonly Irepository<tbl_DealerMaster> _dealer;
        public readonly Irepository<tbl_truckMaster> _truck;
        public readonly Irepository<tbl_MenuMaster> _menumaster;
        public readonly Irepository<tbl_MasterMenuName> _Mastermenumaster;
        public readonly Irepository<tbl_PaymentType> _payementtype;
        public readonly Irepository<tbl_SalesEntry_Master> _salesPayment;
        public readonly Irepository<tbl_SalesEntry_Master_Payment> _salesActualPay;
        public readonly Irepository<tbl_productMaster> _productmaster;
        public readonly Irepository<tbl_BankDetailsPerPurchase> _RTGSMASTER;
        public readonly Irepository<tbl_salesPartyRegistartion> _salesparty;
        public readonly Irepository<tbl_TransportMaster> _transport;
        public readonly Irepository<tbl_BrokerMaster> _broker;
        public readonly Irepository<tbl_ProductTaxEntry> _prodtax;
        public readonly Irepository<tbl_TaxMaster> _taxmaster;
        public readonly Irepository<tbl_PurchseStockMaster> _purchasestock;
        public readonly Irepository<tbl_SalesSockMaster> _salesstock;
        public readonly Irepository<tbl_PurchaseEntry_Master> _purchasepayment;
        public readonly Irepository<tbl_ExpensesMaster> _expenses;
        public readonly Irepository<tbl_ExpensesPurposeMaster> _expensesPurpose;
        public readonly Irepository<tbl_SalesOutStanding> _PurchaseOutStanding;
        public readonly Irepository<tbl_CashReceipt> _cashreceipt;
        public readonly Irepository<tbl_CashCreditMaster> _cashcredit;
        public readonly Irepository<tbl_CashMastered> _cashmastereredd;

        public KisanSalesController(Irepository<tbl_RemoteConnection> remote, Irepository<tbl_pool_MachineData_Purchase> machinedata,
            Irepository<tbl_FarmerMaster> farmer, Irepository<tbl_truckMaster> truck, Irepository<tbl_MenuMaster> menuuu, Irepository<tbl_MasterMenuName> Mastermenu,
            Irepository<tbl_PaymentType> paymenttype, Irepository<tbl_SalesEntry_Master> salespayement, Irepository<tbl_SalesEntry_Master_Payment> salesActualPay,
          Irepository<tbl_productMaster> productmaster, Irepository<tbl_BankDetailsPerPurchase> RTGSMASTER, Irepository<tbl_DealerMaster> dealer,
            Irepository<tbl_salesPartyRegistartion> salesparty, Irepository<tbl_TransportMaster> transport, Irepository<tbl_BrokerMaster> broker,
            Irepository<tbl_ProductTaxEntry> prodtax, Irepository<tbl_TaxMaster> taxmaster, Irepository<tbl_PurchaseEntry_Master> purchasepayment,
              Irepository<tbl_PurchseStockMaster> purchasestock, Irepository<tbl_SalesSockMaster> salesstock,
             Irepository<tbl_ExpensesMaster> expenses, Irepository<tbl_ExpensesPurposeMaster> expensespur, Irepository<tbl_SalesOutStanding> purchaseoutstanding,
           Irepository<tbl_CashReceipt> cashreceipt, Irepository<tbl_CashCreditMaster> cashcredit, Irepository<tbl_CashMastered> cashmastereredd)
        {
            _remote = remote;
            _machinedata = machinedata;
            _farmer = farmer;
            _truck = truck;
            _menumaster = menuuu;
            _Mastermenumaster = Mastermenu;
            _payementtype = paymenttype;
            _salesPayment = salespayement;
            _salesActualPay = salesActualPay;
            _productmaster = productmaster;
            _RTGSMASTER = RTGSMASTER;
            _dealer = dealer;
            _salesparty = salesparty;
            _transport = transport;
            _broker = broker;
            _prodtax = prodtax;
            _taxmaster = taxmaster;
            _purchasestock = purchasestock;
            _salesstock = salesstock;
            _purchasepayment = purchasepayment;
            _expenses = expenses;
            _expensesPurpose = expensespur;
            _PurchaseOutStanding = purchaseoutstanding;
            _cashcredit = cashcredit;
            _cashreceipt = cashreceipt;
            _cashmastereredd = cashmastereredd;
        }
        // GET: KisanSales
        [HttpGet]
        public ActionResult Salestrips()
        {
            return View();
        }
        public ActionResult GetSalesList()
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
                var dat = _truck.GetAll();
                var v = (from a in _machinedata.GetAll().Where(x => x.invoicestatus == null && (x.producttypeid == 2 || x.producttypeid == 3)).Take(20)
                         select new
                         {
                             pkid = a.pkid,
                             sleepno = a.TicketNo,
                             farmer = _salesparty.Get(a.farmerid).partyname,
                             truckno = dat.Where(x => x.RFIDnumber == a.truckRFID).FirstOrDefault().truckno,
                             inttime = a.Intime,
                             outtime = a.outtime,
                             grossweight = a.Grossweight,
                             tairweight = a.TairWeight,
                             netweight = a.NetWeight
                         });

                //SORT
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.farmer.ToLower().Contains(search.ToLower()) || x.truckno.ToLower().Contains(search.ToLower()) || x.sleepno.ToLower().Contains(search.ToLower())) select b);
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
        public ActionResult SalesDetails(int id)
        {
            try
            {
                var pdata = _machinedata.Get(id);
                ViewBag.purchaseid = id;
                ViewBag.partyid = pdata.farmerid;
                ViewBag.pro = pdata.producttypeid;
                ViewBag.saleid = pdata.pkid;
                try
                {
                    int taxid = (int)_prodtax.GetAll().Where(x => x.Product_fkid == pdata.producttypeid).FirstOrDefault().tax_fkid;

                    ViewBag.tax = _taxmaster.Get(taxid).SGST;
                }
                catch
                {

                }
                ViewBag.netweighKG = pdata.NetWeight;
                ViewBag.netweighQtl = (pdata.NetWeight / 100);
                ViewBag.partyname = new SelectList(_salesparty.GetAll(), "pkid", "partyname");
                ViewBag.transport = new SelectList(_transport.GetAll(), "pkid", "transportname");
                ViewBag.broker = new SelectList(_broker.GetAll(), "pkid", "Brokername");
                ViewBag.paycon = new SelectList(_payementtype.GetAll(), "pkid", "paymenttypename");
                var model = _salesPayment.GetAll().Where(x => x.Purchase_fkid == id).FirstOrDefault();
                if (model != null)
                {
                    ViewBag.trockno = _truck.Get(model.truckid).truckno;
                    tbl_SalesEntry_Masterss abc = new tbl_SalesEntry_Masterss();
                    abc.pkid = model.pkid;
                    abc.Purchase_fkid = model.Purchase_fkid; abc.ProductType_fkid = model.ProductType_fkid;
                    abc.rateperqtl = model.rateperqtl; abc.ratepercandy = model.ratepercandy; abc.kilogram = model.kilogram; abc.qtl = model.qtl;
                    abc.amount = model.amount; abc.partyid = model.partyid; abc.prno = model.prno; abc.topr = model.topr; abc.pmark = model.pmark;
                    abc.deliveryfromdate = model.deliveryfromdate; abc.brokerid = model.brokerid; abc.transport_fkid = model.transport_fkid; abc.truckid = model.truckid;
                    abc.drivername = model.drivername; abc.saudadate = model.saudadate; abc.paycondition = model.paycondition; abc.FPbales = model.FPbales;
                    abc.InsuranceNo = model.InsuranceNo; abc.Quality = model.Quality; abc.lotno = model.lotno; abc.noofbales = model.noofbales; abc.grossweight = model.grossweight; abc.tareweight = model.tareweight;
                    abc.frightothercharges = model.frightothercharges; abc.advance = model.advance; abc.packagingcgaeges = model.packagingcgaeges; abc.totalamt = model.totalamt;
                    abc.SGST = model.SGST; abc.CGST = model.CGST; abc.IGST = model.IGST; abc.NetPayableAmount = model.NetPayableAmount; abc.amtinword = model.amtinword;
                    return View(abc);
                }
                else
                {
                    return View();
                }

            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/ErrorLog.txt"));
            }
            return View();
        }
        [HttpPost]
        public ActionResult SalesDetails(tbl_SalesEntry_Masterss model)
        {
            int maxid = 0;
            try
            {
                if (_salesPayment.GetAll().Where(x => x.Purchase_fkid == model.Purchase_fkid).Count() > 0)
                {
                    model.pkid = _salesPayment.GetAll().Where(x => x.Purchase_fkid == model.Purchase_fkid).FirstOrDefault().pkid;
                }
                int id = Convert.ToInt32(model.Purchase_fkid);
                var pdata = _machinedata.Get(id);
                ViewBag.pro = pdata.producttypeid;
                ViewBag.saleid = pdata.pkid;
                ViewBag.netweighKG = pdata.NetWeight;
                ViewBag.netweighQtl = (pdata.NetWeight / 100);
                ViewBag.partyname = new SelectList(_salesparty.GetAll(), "pkid", "partyname");
                ViewBag.transport = new SelectList(_transport.GetAll(), "pkid", "transportname");
                ViewBag.broker = new SelectList(_broker.GetAll(), "pkid", "Brokername");
                ViewBag.paycon = new SelectList(_payementtype.GetAll(), "pkid", "paymenttypename");
                var data = _salesPayment.GetAll().Where(x => x.Purchase_fkid == id).ToList();
                var truckno = Request.Form.GetValues("truckno").FirstOrDefault() ?? "";

                if (truckno != "")
                {
                    var tdata = _truck.GetAll().Where(x => x.truckno == truckno).FirstOrDefault();
                    if (tdata != null)
                    {
                        model.truckid = tdata.pkid;
                    }
                    else
                    {
                        tbl_truckMaster abc = new tbl_truckMaster();
                        abc.truckno = truckno;
                        _truck.Add(abc);
                        int _id = _truck.GetAll().Max(x => x.pkid);
                        model.truckid = _id;
                    }
                }
                if (model.pkid == 0)
                {
                    tbl_SalesEntry_Master abc = new tbl_SalesEntry_Master();
                    abc.Purchase_fkid = model.Purchase_fkid; abc.ProductType_fkid = model.ProductType_fkid; abc.Date = DateTime.Now;
                    abc.rateperqtl = model.rateperqtl; abc.ratepercandy = model.ratepercandy; abc.kilogram = model.kilogram; abc.qtl = model.qtl;
                    abc.amount = model.amount; abc.partyid = model.partyid; abc.prno = model.prno; abc.topr = model.topr; abc.pmark = model.pmark;
                    abc.deliveryfromdate = model.deliveryfromdate; abc.brokerid = model.brokerid; abc.transport_fkid = model.transport_fkid; abc.truckid = model.truckid;
                    abc.drivername = model.drivername; abc.saudadate = model.saudadate; abc.paycondition = model.paycondition; abc.FPbales = model.FPbales;
                    abc.InsuranceNo = model.InsuranceNo; abc.Quality = model.Quality; abc.lotno = model.lotno; abc.noofbales = model.noofbales; abc.grossweight = model.grossweight; abc.tareweight = model.tareweight;
                    abc.frightothercharges = model.frightothercharges; abc.advance = model.advance; abc.packagingcgaeges = model.packagingcgaeges; abc.totalamt = model.totalamt;
                    abc.SGST = model.SGST; abc.CGST = model.CGST; abc.IGST = model.IGST; abc.NetPayableAmount = model.NetPayableAmount; abc.amtinword = model.amtinword;
                    _salesPayment.Add(abc);
                    tbl_pool_MachineData_Purchase def = _machinedata.Get(model.Purchase_fkid);
                    def.invoicestatus = 1;
                    _machinedata.Update(def);
                    maxid = _salesPayment.GetAll().OrderByDescending(x => x.pkid).FirstOrDefault().pkid;
                }
                else
                {
                    tbl_SalesEntry_Master abc = _salesPayment.Get(model.pkid);
                    abc.Purchase_fkid = model.Purchase_fkid; abc.ProductType_fkid = model.ProductType_fkid; abc.Date = DateTime.Now;
                    abc.rateperqtl = model.rateperqtl; abc.ratepercandy = model.ratepercandy; abc.kilogram = model.kilogram; abc.qtl = model.qtl;
                    abc.amount = model.amount; abc.partyid = model.partyid; abc.prno = model.prno; abc.topr = model.topr; abc.pmark = model.pmark;
                    abc.deliveryfromdate = model.deliveryfromdate; abc.brokerid = model.brokerid; abc.transport_fkid = model.transport_fkid; abc.truckid = model.truckid;
                    abc.drivername = model.drivername; abc.saudadate = model.saudadate; abc.paycondition = model.paycondition; abc.FPbales = model.FPbales;
                    abc.InsuranceNo = model.InsuranceNo; abc.Quality = model.Quality; abc.lotno = model.lotno; abc.noofbales = model.noofbales; abc.grossweight = model.grossweight; abc.tareweight = model.tareweight;
                    abc.frightothercharges = model.frightothercharges; abc.advance = model.advance; abc.packagingcgaeges = model.packagingcgaeges; abc.totalamt = model.totalamt;
                    abc.SGST = model.SGST; abc.CGST = model.CGST; abc.IGST = model.IGST; abc.NetPayableAmount = model.NetPayableAmount; abc.amtinword = model.amtinword;
                    _salesPayment.Update(abc);
                    maxid = model.pkid;
                }
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/ErrorLog.txt"));
            }
            return RedirectToAction("GetsalesPDF", "KisanSales", new { id = maxid });
        }
        public ActionResult SaveParty(string[] poopupdatasave)
        {
            try
            {
                tbl_salesPartyRegistartion abc = new tbl_salesPartyRegistartion();
                abc.partyname = poopupdatasave[0]; abc.GSTNo = poopupdatasave[1];
                abc.address1 = poopupdatasave[2]; abc.mobileno = poopupdatasave[3];
                abc.city = poopupdatasave[4]; abc.emailid = poopupdatasave[5]; abc.Sale_fkid = Convert.ToInt32(poopupdatasave[6]);
                abc.Partyregdate = DateTime.Now;

                _salesparty.Add(abc);
                int id = _salesparty.GetAll().Max(x => x.pkid);
                var name = _salesparty.Get(id).partyname;
                string da = "<option value=" + id + ">" + name + "</option>";
                return Json(da, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SaveBroker(string[] poopupdatasave)
        {
            try
            {
                tbl_BrokerMaster abc = new tbl_BrokerMaster();
                abc.Brokername = poopupdatasave[0]; abc.address1 = poopupdatasave[1];
                abc.emailid = poopupdatasave[2]; abc.mobile = poopupdatasave[3];

                _broker.Add(abc);
                int id = _broker.GetAll().Max(x => x.pkid);
                var name = _broker.Get(id).Brokername;
                string da = "<option value=" + id + ">" + name + "</option>";
                return Json(da, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SaveTransport(string[] poopupdatasave)
        {
            try
            {
                tbl_TransportMaster abc = new tbl_TransportMaster();
                abc.transportname = poopupdatasave[0]; abc.address1 = poopupdatasave[1];
                abc.emailid = poopupdatasave[2]; abc.mobileno = poopupdatasave[3]; abc.ownername = poopupdatasave[4];

                _transport.Add(abc);
                int id = _transport.GetAll().Max(x => x.pkid);
                var name = _transport.Get(id).transportname;
                string da = "<option value=" + id + ">" + name + "</option>";
                return Json(da, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult PendingPaymentSales()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PendingPaymentSales(tbl_SalesEntry_Master_Paymentss model)
        {
            return View();
        }
        public ActionResult Pendingpaymentlist()
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
                var v = (from a in _salesPayment.GetAll().Where(x => x.PaymentStatus != 1).ToList()
                         join b in _machinedata.GetAll() on a.Purchase_fkid equals b.pkid
                         select new
                         {
                             pkid = a.pkid,
                             purchaseid = b.pkid,
                             invoiceno = "0000" + a.pkid,
                             PartyName = _salesparty.GetAll().Where(x => x.pkid == b.farmerid).FirstOrDefault().partyname,
                             datet = a.Date,
                             netweight = b.NetWeight,
                             payble = (a.totalamt ?? 0),
                             balance = a.NetPayableAmount,
                             GST = (a.SGST) + (a.CGST)
                         });

                //SORT
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.PartyName.ToLower().Contains(search.ToLower())) select b);
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
        public ActionResult SalesDetails_Payment(int id, int Bill_fkid, string pkid)
        {
            int _pkid = Convert.ToInt32(pkid);
            ViewBag.purchasid = id;
            ViewBag.invoiseid = Bill_fkid;
            var data = _salesPayment.Get(Bill_fkid);
            ViewBag.dt = data.Date;
            ViewBag.paytype = new SelectList(_payementtype.GetAll(), "pkid", "paymenttypename");
            ViewBag.holdername = new SelectList(_RTGSMASTER.GetAll().Where(x => x.pkid == 47).ToList(), "pkid", "AccountHolder");
            ViewBag.cheholdername = new SelectList(_RTGSMASTER.GetAll().Where(x => x.pkid == 48).ToList(), "pkid", "Cheq_Partyname");
            ViewBag.netweight = _machinedata.Get(id).NetWeight;
            ViewBag.rate = data.ratepercandy;
            int farmerid = Convert.ToInt32(_machinedata.Get(id).farmerid);
            ViewBag.salePartyname = _salesparty.Get(farmerid).partyname;
            ViewBag.payable = (data.NetPayableAmount);
            ViewBag.advan = data.advance;
            var entry = _salesActualPay.GetAll().Where(x => x.invoice_fkid == Bill_fkid).OrderByDescending(x => x.pkid).FirstOrDefault();
            if (entry != null && pkid == null)
            {
                ViewBag.pendinamt = entry.pendingAmt;
            }
            else
            {
                var entryss = _salesActualPay.GetAll().Where(x => x.invoice_fkid == Bill_fkid).OrderByDescending(x => x.pkid).FirstOrDefault();
                if (entryss == null)
                {
                    ViewBag.pendinamt = data.NetPayableAmount;
                }
                else
                {
                    ViewBag.pendinamt = entryss.pendingAmt;
                }
            }
            if (_pkid == 0)
            {
                return View();
            }
            else
            {
                var model = _salesActualPay.Get(_pkid);
                tbl_SalesEntry_Master_Paymentss abc = new tbl_SalesEntry_Master_Paymentss();
                abc.Purchase_fkid = model.Purchase_fkid; abc.invoice_fkid = model.invoice_fkid; abc.pendingAmt = model.pendingAmt; abc.Currentopeningbalance = model.Currentopeningbalance; abc.datetime = DateTime.Now;
                abc.PaymentMethod = model.PaymentMethod;
                abc.RtgsDetail_fkid = model.RtgsDetail_fkid; abc.RTGSAmtPaying = model.RTGSAmtPaying; abc.ChequeAmtpaying = model.ChequeAmtpaying; abc.chequ_No = model.chequ_No; abc.cheqDetail_fkid = model.cheqDetail_fkid;
                abc.CashPaidTO = model.CashPaidTO; abc.pendingAmt = model.pendingAmt;
                abc.CashAmt = model.CashAmt;
                return View(abc);
            }
        }
        [HttpPost]
        public ActionResult SalesDetails_Payment(tbl_SalesEntry_Master_Paymentss model)
        {
            try
            {
                var previous = _salesActualPay.GetAll().Where(x => x.Purchase_fkid == model.Purchase_fkid).OrderByDescending(x => x.pkid).FirstOrDefault();
                if (previous != null)
                {
                    model.Currentopeningbalance = previous.pendingAmt;
                }
                else
                {
                    var da = _salesPayment.Get(model.invoice_fkid);
                    model.Currentopeningbalance = da.NetPayableAmount;
                }
                if (model.pkid == 0)
                {
                    tbl_SalesEntry_Master_Payment abc = new tbl_SalesEntry_Master_Payment();
                    abc.Purchase_fkid = model.Purchase_fkid; abc.invoice_fkid = model.invoice_fkid; abc.pendingAmt = model.pendingAmt; abc.Currentopeningbalance = model.Currentopeningbalance; abc.datetime = DateTime.Now;
                    abc.PaymentMethod = model.PaymentMethod;
                    abc.RtgsDetail_fkid = model.RtgsDetail_fkid; abc.RTGSAmtPaying = model.RTGSAmtPaying; abc.ChequeAmtpaying = model.ChequeAmtpaying; abc.chequ_No = model.chequ_No; abc.cheqDetail_fkid = model.cheqDetail_fkid;
                    abc.CashPaidTO = model.CashPaidTO; abc.pendingAmt = model.pendingAmt;
                    abc.CashAmt = model.CashAmt;
                    abc.Status = 1;
                    if (model.PaymentMethod == 3)
                    {
                        abc.Status = 1;
                    }
                    if (model.pendingAmt == 0)
                    {
                        var alllist = _salesPayment.GetAll().Where(x => x.Purchase_fkid == model.Purchase_fkid).FirstOrDefault();
                        alllist.PaymentStatus = 1;
                        _salesPayment.Update(alllist);
                    }
                    _salesActualPay.Add(abc);
                    var alldata = _salesPayment.GetAll().Max(x => x.pkid);
                    return RedirectToAction("SalesDetails_Payment", "KisanSales", new { id = model.Purchase_fkid, Bill_fkid = model.invoice_fkid });
                }
                else
                {
                    var abc = _salesActualPay.Get(model.pkid);
                    abc.Purchase_fkid = model.Purchase_fkid; abc.invoice_fkid = model.invoice_fkid; abc.pendingAmt = model.pendingAmt; abc.Currentopeningbalance = model.Currentopeningbalance; abc.datetime = DateTime.Now;
                    abc.PaymentMethod = model.PaymentMethod;
                    abc.RtgsDetail_fkid = model.RtgsDetail_fkid; abc.RTGSAmtPaying = model.RTGSAmtPaying; abc.ChequeAmtpaying = model.ChequeAmtpaying; abc.chequ_No = model.chequ_No; abc.cheqDetail_fkid = model.cheqDetail_fkid;
                    abc.CashPaidTO = model.CashPaidTO;
                    abc.CashAmt = model.CashAmt;
                    abc.Status = 1;
                    if (model.PaymentMethod == 3)
                    {
                        abc.Status = 1;
                    }
                    if (model.pendingAmt == 0)
                    {
                        var alllist = _salesPayment.GetAll().Where(x => x.Purchase_fkid == model.Purchase_fkid).FirstOrDefault();
                        alllist.PaymentStatus = 1;
                        _salesPayment.Update(alllist);
                    }
                    var entry = _salesActualPay.GetAll().Where(x => x.invoice_fkid == model.invoice_fkid).OrderByDescending(x => x.pkid).FirstOrDefault();
                    if (entry != null)
                    {
                        entry.pendingAmt = model.pendingAmt;
                        _salesActualPay.Update(entry);
                    }
                    else
                    {
                        abc.pendingAmt = model.pendingAmt;
                    }
                    _salesActualPay.Update(abc);
                    return RedirectToAction("SalesDetails_Payment", "KisanSales", new { id = model.Purchase_fkid, Bill_fkid = model.invoice_fkid });
                }
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/ErrorLog.txt"));
            }
            return RedirectToAction("MainPage", "Home");
        }
        public ActionResult Delepaymentsales(int id, int Bill_fkid, string pkid)
        {
            try
            {
                int _pkid = Convert.ToInt32(pkid);
                var ddata = _salesActualPay.Get(_pkid);
                var dataasd = _salesActualPay.GetAll().Where(x => x.invoice_fkid == Bill_fkid).ToList();
                if (dataasd.Count() > 1)
                {
                    //_salesActualPay.Remove(_pkid, true);
                    tbl_SalesEntry_Master_Payment data = _salesActualPay.GetAll().Where(x => x.invoice_fkid == Bill_fkid).OrderByDescending(x => x.pkid).FirstOrDefault();
                    if (data.pkid == _pkid)
                    {
                        decimal actualp = (ddata.RTGSAmtPaying ?? ddata.ChequeAmtpaying ?? ddata.CashAmt ?? 0) + (ddata.pendingAmt ?? 0);
                        _salesActualPay.Remove(_pkid, true);
                        tbl_SalesEntry_Master_Payment PKdata = _salesActualPay.GetAll().Where(x => x.invoice_fkid == Bill_fkid).OrderByDescending(x => x.pkid).FirstOrDefault();
                        PKdata.pendingAmt = actualp;
                        PKdata.Currentopeningbalance = actualp;
                        _salesActualPay.Update(PKdata);
                    }
                    else
                    {
                        _salesActualPay.Remove(_pkid, true);
                        decimal deltedamt = ddata.RTGSAmtPaying ?? ddata.ChequeAmtpaying ?? ddata.CashAmt ?? 0;
                        if (ddata.pkid > data.pkid)
                        {

                        }
                        else
                        {
                            data.pendingAmt = (data.pendingAmt) + (deltedamt);
                            data.Currentopeningbalance = (data.Currentopeningbalance) + (deltedamt);
                            _salesActualPay.Update(data);
                        }
                    }
                }
                else
                {
                    int _id = dataasd.FirstOrDefault().pkid;
                    _salesActualPay.Remove(_id, true);
                }
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/ErrorLog.txt"));
            }
            return RedirectToAction("SalesDetails_Payment", "KisanSales", new { id = id, Bill_fkid = Bill_fkid });
        }
        public ActionResult PaymentBillList(int id)
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

                var v = (from a in _salesActualPay.GetAll().Where(x => x.invoice_fkid == id).ToList()
                         join b in _machinedata.GetAll() on a.Purchase_fkid equals b.pkid

                         select new
                         {
                             pkid = a.pkid,
                             holdername = (a.PaymentMethod == 1 ? _RTGSMASTER.GetAll().Where(x => x.pkid == a.RtgsDetail_fkid).FirstOrDefault().AccountHolder ?? "" : ""),
                             partyname = (a.PaymentMethod == 2 ? _RTGSMASTER.GetAll().Where(x => x.pkid == a.cheqDetail_fkid).FirstOrDefault().Cheq_Partyname ?? "" : ""),
                             casgpaidto = (a.PaymentMethod == 3 ? a.CashPaidTO ?? "" : ""),
                             paytype = _payementtype.Get(a.PaymentMethod).paymenttypename ?? "",
                             Netamt = a.Currentopeningbalance,
                             pendingamt = a.pendingAmt,
                             rtgs = a.RTGSAmtPaying ?? 0,
                             cheq = a.ChequeAmtpaying ?? 0,
                             adv = _salesPayment.Get(id).advance,
                             cash = a.CashAmt ?? 0,
                             advance = _salesPayment.Get(a.invoice_fkid).advance ?? 0,
                             purchasid = (int)a.Purchase_fkid,
                             invoiceid = (int)a.invoice_fkid,
                             datet = (DateTime)a.datetime
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.holdername.ToLower().Contains(search.ToLower()) || x.partyname.ToLower().Contains(search.ToLower()) || x.casgpaidto.ToLower().Contains(search.ToLower())) select b);
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
        public ActionResult GetPaymentSlip(int pkid)
        {
            string msg = "";
            string jsonErrorCode = "0";
            ReportDocument rd = new ReportDocument();
            try
            {
                var dataasd = _salesActualPay.Get(pkid);
                int invoiceid = (int)dataasd.invoice_fkid;
                string strReportName = "PaySleep.rpt";
                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports\\" + strReportName;
                rd.Load(strRptPath);
                var data = (from a in _salesPayment.GetAll().Where(x => x.pkid == invoiceid).ToList()
                            join b in _machinedata.GetAll() on a.Purchase_fkid equals b.pkid
                            join c in _RTGSMASTER.GetAll() on a.Purchase_fkid equals c.purchase_fkid
                            join d in _salesActualPay.GetAll() on a.pkid equals d.invoice_fkid
                            where a.Purchase_fkid == b.pkid && a.Purchase_fkid == c.purchase_fkid && a.pkid == d.invoice_fkid
                            select new
                            {
                                pkid = d.pkid,
                                slipno = "0000" + d.pkid,
                                Invoiceno = "0000" + a.pkid,
                                farmername = _farmer.Get(b.farmerid).fullname ?? "",
                                villagename = "Nagpur",
                                date = a.Date ?? default(DateTime),

                                drivername = a.drivername ?? "",

                                weight = b.NetWeight ?? 0,
                                rate = a.ratepercandy ?? 0,
                                totalamt = a.NetPayableAmount ?? 0,
                                balanceamt = d.pendingAmt ?? 0,
                                payableamt = d.RTGSAmtPaying ?? d.ChequeAmtpaying ?? d.CashAmt ?? 0,
                                bankname = c.Bank_name ?? "",
                                branchname = c.Branchname ?? "",
                                accno = c.Accountno ?? "",
                                ifsccode = c.Ifsccode ?? "",
                                holdername = c.AccountHolder ?? "",
                                cheqpartyname = c.Cheq_Partyname ?? "",
                                chequeno = d.chequ_No ?? ""
                            }).ToList();
                // rd.SetDatabaseLogon("sa", "temp123", "itg8", "KisanWeighning");
                rd.Database.Tables[0].SetDataSource(data);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                jsonErrorCode = "-2";
            }
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "PaymentSlip.pdf");
        }

        public ActionResult CompletedPayment()
        {
            return View();
        }
        public ActionResult CompletedPaymentList()
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
                var v = (from a in _salesPayment.GetAll().Where(x => x.PaymentStatus == 1)
                         join b in _salesActualPay.GetAll() on a.pkid equals b.invoice_fkid
                         select new
                         {
                             pkid = a.pkid,
                             invoiceno = "0000" + a.pkid,
                             rate = a.ratepercandy ?? 0,
                             partyname = _salesparty.Get(a.partyid).partyname ?? "",
                             invoicedt = a.Date ?? default(DateTime),
                             noballs = a.noofbales ?? 0,
                             paymentdate = b.datetime ?? default(DateTime),
                             netamt = a.NetPayableAmount ?? 0
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.partyname.ToLower().Contains(search.ToLower())) select b);
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
        [HttpGet]
        public ActionResult GetHoleDetails(int id)
        {
            try
            {
                id = (int)_salesPayment.Get(id).Purchase_fkid;
                var pdata = _machinedata.Get(id);
                ViewBag.purchaseid = id;
                ViewBag.partyid = pdata.farmerid;
                ViewBag.pro = pdata.producttypeid;
                ViewBag.saleid = pdata.pkid;
                int taxid = (int)_prodtax.GetAll().Where(x => x.Product_fkid == pdata.producttypeid).FirstOrDefault().tax_fkid;
                ViewBag.tax = _taxmaster.Get(taxid).SGST;
                ViewBag.netweighKG = pdata.NetWeight;
                ViewBag.netweighQtl = (pdata.NetWeight / 100);
                ViewBag.partyname = new SelectList(_salesparty.GetAll(), "pkid", "partyname");
                ViewBag.transport = new SelectList(_transport.GetAll(), "pkid", "transportname");
                ViewBag.broker = new SelectList(_broker.GetAll(), "pkid", "Brokername");
                ViewBag.paycon = new SelectList(_payementtype.GetAll(), "pkid", "paymenttypename");
                var model = _salesPayment.GetAll().Where(x => x.Purchase_fkid == id).FirstOrDefault();
                if (model != null)
                {
                    ViewBag.trockno = _truck.Get(model.truckid).truckno;
                    tbl_SalesEntry_Masterss abc = new tbl_SalesEntry_Masterss();
                    abc.pkid = model.pkid;
                    abc.Purchase_fkid = model.Purchase_fkid; abc.ProductType_fkid = model.ProductType_fkid;
                    abc.rateperqtl = model.rateperqtl; abc.ratepercandy = model.ratepercandy; abc.kilogram = model.kilogram; abc.qtl = model.qtl;
                    abc.amount = model.amount; abc.partyid = model.partyid; abc.prno = model.prno; abc.topr = model.topr; abc.pmark = model.pmark;
                    abc.deliveryfromdate = model.deliveryfromdate; abc.brokerid = model.brokerid; abc.transport_fkid = model.transport_fkid; abc.truckid = model.truckid;
                    abc.drivername = model.drivername; abc.saudadate = model.saudadate; abc.paycondition = model.paycondition; abc.FPbales = model.FPbales;
                    abc.InsuranceNo = model.InsuranceNo; abc.Quality = model.Quality; abc.lotno = model.lotno; abc.noofbales = model.noofbales; abc.grossweight = model.grossweight; abc.tareweight = model.tareweight;
                    abc.frightothercharges = model.frightothercharges; abc.advance = model.advance; abc.packagingcgaeges = model.packagingcgaeges; abc.totalamt = model.totalamt;
                    abc.SGST = model.SGST; abc.CGST = model.CGST; abc.IGST = model.IGST; abc.NetPayableAmount = model.NetPayableAmount; abc.amtinword = model.amtinword;
                    return View(abc);
                }
                else
                {
                    return View();
                }

            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/ErrorLog.txt"));
            }
            return View();
        }
        public ActionResult GetsalesPDF(int id)
        {
            string msg = "";
            string jsonErrorCode = "0";
            ReportDocument rd = new ReportDocument();
            try
            {
                var opdata = _salesPayment.Get(id);
                if (opdata.ProductType_fkid == 2)
                {

                    string strReportName = "SalesBalesInvoice.rpt";
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports\\" + strReportName;
                    rd.Load(strRptPath);
                    var data = (from a in _salesPayment.GetAll().Where(x => x.pkid == id)
                                join b in _machinedata.GetAll() on a.Purchase_fkid equals b.pkid
                                where a.Purchase_fkid == b.pkid
                                select new
                                {
                                    pkid = "0000" + a.pkid,
                                    partyname = (b.farmerid != 0 ? _salesparty.Get(b.farmerid).partyname : ""),
                                    GSTno = _salesparty.Get(b.farmerid).GSTNo ?? "",
                                    PRno = a.prno ?? "",
                                    TO = a.topr ?? "",
                                    Pmark = a.pmark ?? "",
                                    dispatchedate = a.Date ?? default(DateTime),
                                    FPbales = a.FPbales ?? "",
                                    quality = a.Quality ?? "",
                                    lotno = a.lotno ?? 0,
                                    grossweight = a.grossweight ?? 0,
                                    tareweight = a.tareweight ?? 0,
                                    qtl = a.qtl ?? 0,
                                    kg = a.kilogram ?? 0,
                                    amount = a.amount ?? 0,
                                    ratepercandy = a.ratepercandy ?? 0,
                                    rateperqtl = a.rateperqtl ?? 0,
                                    amountinwords = "",
                                    othercharges = a.frightothercharges ?? 0,
                                    totalamt = a.totalamt ?? 0,
                                    SGST = a.SGST ?? 0,
                                    CGST = a.CGST ?? 0,
                                    IGST = a.IGST ?? 0,
                                    advance = a.advance ?? 0,
                                    Netpayable = a.NetPayableAmount ?? 0,
                                    Insuranceno = a.InsuranceNo ?? "",
                                    truckno = (a.truckid != null ? _truck.Get(a.truckid ?? 0).truckno ?? "" : ""),
                                    transport = (a.transport_fkid != null ? _transport.Get(a.transport_fkid ?? 0).transportname ?? "--" : "--"),
                                    broker = (a.brokerid != null ? _broker.Get(a.brokerid).Brokername ?? "--" : "--"),
                                    saudate = a.saudadate ?? default(DateTime),
                                    invoicedate = a.Date ?? default(DateTime),
                                    paymentcondition = (a.paycondition != null ? _payementtype.Get(a.paycondition).paymenttypename ?? "--" : "--")
                                }).ToList();
                    // rd.SetDatabaseLogon("sa", "temp123", "itg8", "KisanWeighning");
                    rd.Database.Tables[0].SetDataSource(data);
                }
                else
                {
                    string strReportName = "SalesBalesInvoice.rpt";
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports\\" + strReportName;
                    rd.Load(strRptPath);
                    var data = (from a in _salesPayment.GetAll().Where(x => x.pkid == id)
                                join b in _machinedata.GetAll() on a.Purchase_fkid equals b.pkid
                                where a.Purchase_fkid == b.pkid
                                select new
                                {
                                    pkid = "0000" + a.pkid,
                                    partyname = (b.farmerid != 0 ? _salesparty.Get(b.farmerid).partyname : ""),
                                    GSTno = _salesparty.Get(b.farmerid).GSTNo ?? "",
                                    dispatchedate = a.Date ?? default(DateTime),
                                    FPbales = a.FPbales ?? "",
                                    quality = a.Quality ?? "",
                                    lotno = a.lotno ?? 0,
                                    qtl = a.qtl ?? 0,
                                    kg = a.kilogram ?? 0,

                                    amount = a.amount ?? 0,
                                    ratepercandy = a.ratepercandy ?? 0,
                                    amountinwords = "",
                                    othercharges = a.packagingcgaeges ?? 0,
                                    totalamt = a.totalamt ?? 0,
                                    SGST = a.SGST ?? 0,
                                    CGST = a.CGST ?? 0,
                                    IGST = a.IGST ?? 0,
                                    advance = a.advance ?? 0,
                                    Netpayable = a.NetPayableAmount ?? 0,
                                    Insuranceno = a.InsuranceNo ?? "",
                                    truckno = (a.truckid != 0 ? _truck.Get(a.truckid ?? 0).truckno ?? "" : ""),
                                    transport = (a.transport_fkid != 0 ? _transport.Get(a.transport_fkid ?? 0).transportname ?? "--" : "--"),
                                    broker = (a.brokerid != 0 ? _broker.Get(a.brokerid).Brokername ?? "--" : "--"),
                                    saudate = a.saudadate ?? default(DateTime),
                                    invoicedate = a.Date ?? default(DateTime),
                                    paymentcondition = (a.paycondition != null ? _payementtype.Get(a.paycondition).paymenttypename ?? "--" : "--")
                                }).ToList();
                    // rd.SetDatabaseLogon("sa", "temp123", "itg8", "KisanWeighning");
                    rd.Database.Tables[0].SetDataSource(data);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                jsonErrorCode = "-2";
            }
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "0000" + id + ".pdf");
        }
        [HttpGet]
        public ActionResult salesLedger()
        {

            ViewBag.farmer = new SelectList(_salesparty.GetAll().OrderBy(x => x.partyname), "pkid", "partyname");
            ViewBag.dealer = new SelectList(_broker.GetAll().OrderBy(x => x.Brokername), "pkid", "Brokername");
            return View();
        }
        public ActionResult ledger(string frid, string drid, string frdt, string todt)
        {
            int fid = (frid != "" ? Convert.ToInt32(frid) : 0);
            int tid = (drid != "" ? Convert.ToInt32(drid) : 0);
            DateTime frrdt = (frdt != "" ? Convert.ToDateTime(frdt) : default(DateTime));
            DateTime toodt = (todt != "" ? Convert.ToDateTime(todt) : default(DateTime));
            List<listpurchaseledger> abc = new List<listpurchaseledger>();
            var v = (from a in _salesPayment.GetAll()
                     join b in _machinedata.GetAll() on a.Purchase_fkid equals b.pkid
                     select new listpurchaseledger
                     {
                         pkid = a.pkid,
                         purchaseid = b.pkid,
                         invoiceno = "0000" + a.pkid,
                         fid = b.farmerid ?? 0,
                         did = a.brokerid ?? 0,
                         farmername = (b.farmerid != 0 ? _salesparty.GetAll().Where(x => x.pkid == b.farmerid).FirstOrDefault().partyname : ""),
                         dealername = (a.brokerid != null ? _broker.GetAll().Where(x => x.pkid == a.brokerid).FirstOrDefault().Brokername : ""),
                         datet = a.Date ?? default(DateTime),
                         netweight = b.NetWeight ?? 0,
                         Rate = a.ratepercandy ?? 0,
                         totalpaying = (_salesActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).ToList() != null ? _salesActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).Sum(x => x.RTGSAmtPaying ?? 0 + x.ChequeAmtpaying ?? 0 + x.CashAmt ?? 0) : 0),
                         advance = a.advance ?? 0,
                         totalamt = a.NetPayableAmount ?? 0,
                         balance = (_salesActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).Count() > 0 ? _salesActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).OrderByDescending(x => x.pkid).FirstOrDefault().pendingAmt : a.NetPayableAmount),
                     }).ToList();
            if (fid != 0)
            {
                v = v.Where(x => x.fid == fid).ToList();
            }
            if (tid != 0)
            {
                v = v.Where(x => x.did == tid).ToList();
            }
            if (frdt != "")
            {
                v = v.Where(x => x.datet >= frrdt && x.datet <= toodt).ToList();
            }
            ViewBag.summ = v.Sum(x => x.balance).Value;
            abc = v;
            return View(abc);
        }
        [HttpPost]
        public ActionResult salesLedger(purchaseledger model)
        {
            try
            {
                int fid = (model.farid != null ? Convert.ToInt32(model.farid) : 0);
                int tid = (model.drid != null ? Convert.ToInt32(model.drid) : 0);
                DateTime frrdt = (model.frdt != null ? Convert.ToDateTime(model.frdt) : default(DateTime));
                DateTime toodt = (model.todt != null ? Convert.ToDateTime(model.todt) : default(DateTime));
                List<listpurchaseledger> lsitExcel = new List<listpurchaseledger>();
                var v = (from a in _salesPayment.GetAll()
                         join b in _machinedata.GetAll() on a.Purchase_fkid equals b.pkid
                         select new listpurchaseledger
                         {
                             pkid = a.pkid,
                             purchaseid = b.pkid,
                             invoiceno = "0000" + a.pkid,
                             fid = b.farmerid ?? 0,
                             did = a.brokerid ?? 0,
                             farmername = (b.farmerid != 0 ? _salesparty.GetAll().Where(x => x.pkid == b.farmerid).FirstOrDefault().partyname : ""),
                             dealername = (a.brokerid != null ? _broker.GetAll().Where(x => x.pkid == a.brokerid).FirstOrDefault().Brokername : ""),
                             datet = a.Date ?? default(DateTime),
                             netweight = b.NetWeight ?? 0,
                             Rate = a.ratepercandy ?? 0,
                             totalpaying = (_salesActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).ToList() != null ? _salesActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).Sum(x => x.RTGSAmtPaying ?? 0 + x.ChequeAmtpaying ?? 0 + x.CashAmt ?? 0) : 0),
                             advance = a.advance ?? 0,
                             totalamt = a.NetPayableAmount ?? 0,
                             balance = (_salesActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).Count() > 0 ? _salesActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).OrderByDescending(x => x.pkid).FirstOrDefault().pendingAmt : a.NetPayableAmount),
                         }).ToList();
                if (fid != 0)
                {
                    v = v.Where(x => x.fid == fid).ToList();
                }
                if (tid != 0)
                {
                    v = v.Where(x => x.did == tid).ToList();
                }
                if (model.frdt != null)
                {
                    v = v.Where(x => x.datet >= frrdt && x.datet <= toodt).ToList();
                }
                lsitExcel = v;
                decimal summ = v.Sum(x => x.balance).Value;
                DataTable _table = new DataTable();
                DataTable _cheque = new DataTable();
                _table.TableName = "SalesLedger";
                _table.Columns.Add("S.No.", typeof(string));
                _table.Columns.Add("INVOICE NUMBER", typeof(string));
                _table.Columns.Add("DATE", typeof(string));
                _table.Columns.Add("PARTY NAME", typeof(string));
                _table.Columns.Add("BROKER NAME", typeof(string));
                _table.Columns.Add("NET WEIGHT", typeof(string));
                _table.Columns.Add("RATE PER CANDY", typeof(string));
                _table.Columns.Add("TOTAL AMOUNT", typeof(string));
                _table.Columns.Add("ADVANCE", typeof(string));
                _table.Columns.Add("TOTAL CREATED", typeof(string));
                _table.Columns.Add("BALANCE", typeof(string));


                long srno = 1;
                decimal MAStotalAmt = 0;
                decimal MASadvance = 0; decimal MAStotaldebit = 0; decimal MASbal = 0;
                foreach (var item in lsitExcel)
                {
                    _table.Rows.Add(srno, item.invoiceno.ToString(), item.datet.ToString(), item.farmername.ToString(),
                        item.dealername.ToString(), item.netweight.ToString(), item.Rate ?? 0,
                        item.totalamt.ToString(), item.advance.ToString(), item.totalpaying.ToString(), item.balance.ToString());
                    srno++;
                    MAStotalAmt = (decimal)((MAStotalAmt) + (item.totalamt));
                    MASadvance = (decimal)((MASadvance) + (item.advance));
                    MAStotaldebit = (decimal)((MAStotaldebit) + (item.totalpaying));
                    MASbal = (decimal)((MASbal) + (item.balance));
                }
                _table.Rows.Add("", "TOTAL", "", "", "", "", "", MAStotalAmt.ToString(), MASadvance.ToString(), MAStotaldebit.ToString(), MASbal.ToString());
                GridView gv = new GridView();

                gv.DataSource = _table;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=SalesLegder.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            catch
            {

            }
            return RedirectToAction("PurchaseLedger", "PooldataMachine");
        }
        [HttpGet]
        public ActionResult StockEntry()
        {
            try
            {
                var data = _purchasestock.GetAll();
                if (data.Count() == 0)
                {
                    var purch = _purchasepayment.GetAll().Where(x => x.stockread != 1);
                    foreach (var item in purch.OrderBy(x => x.currentdatetime).ToList())
                    {
                        tbl_PurchseStockMaster PStock = new tbl_PurchseStockMaster();
                        PStock.purchase_fkid = item.purchasemachine_fkid;
                        PStock.product_fkid = _machinedata.Get(item.purchasemachine_fkid).producttypeid;
                        PStock.ReceivedStock = item.qunatity;
                        try
                        {
                            PStock.previousStock = _purchasestock.GetAll().Where(x => x.Machineentrydate.Value.Date == item.currentdatetime.Value.Date).FirstOrDefault().TotalStock ?? 0;
                        }
                        catch
                        {
                            PStock.previousStock = 0;
                        }
                        PStock.TotalStock = (PStock.ReceivedStock) + (PStock.previousStock);
                        PStock.Machineentrydate = item.currentdatetime;
                        PStock.Inpudate = DateTime.Now; PStock.Lastmodifieddate = DateTime.Now;
                        var savpur = _purchasestock.GetAll().Where(x => x.Machineentrydate.Value.Date == PStock.Machineentrydate.Value.Date).ToList();
                        if (savpur.Count() == 0)
                        {
                            _purchasestock.Add(PStock);
                        }
                        else
                        {
                            tbl_PurchseStockMaster PPStock = _purchasestock.Get(savpur.FirstOrDefault().pkid);
                            PPStock.ReceivedStock = item.qunatity;
                            try
                            {
                                PPStock.previousStock = _purchasestock.GetAll().Where(x => x.Machineentrydate.Value.Date == item.currentdatetime.Value.Date).FirstOrDefault().TotalStock ?? 0;
                            }
                            catch
                            {
                                PPStock.previousStock = 0;
                            }
                            PPStock.TotalStock = (PPStock.ReceivedStock) + (PPStock.previousStock);
                            PPStock.Machineentrydate = item.currentdatetime;
                            _purchasestock.Update(PPStock);
                        }
                        tbl_PurchaseEntry_Master abc = _purchasepayment.Get(item.pkid);
                        abc.stockread = 1;
                        _purchasepayment.Update(abc);
                    }
                }
                else
                {
                    DateTime datefrom = (DateTime)data.OrderByDescending(x => x.pkid).FirstOrDefault().Machineentrydate;
                    var purch = _purchasepayment.GetAll().Where(x => x.stockread != 1);
                    foreach (var item in purch.OrderBy(x => x.currentdatetime).ToList())
                    {
                        tbl_PurchseStockMaster PStock = new tbl_PurchseStockMaster();
                        PStock.purchase_fkid = item.purchasemachine_fkid;
                        PStock.product_fkid = _machinedata.Get(item.purchasemachine_fkid).producttypeid;
                        PStock.ReceivedStock = item.qunatity;
                        try
                        {
                            PStock.previousStock = _purchasestock.GetAll().Where(x => x.Machineentrydate.Value.Date == item.currentdatetime.Value.Date).FirstOrDefault().TotalStock ?? 0;
                        }
                        catch
                        {
                            PStock.previousStock = 0;
                        }
                        PStock.TotalStock = (PStock.ReceivedStock) + (PStock.previousStock);
                        PStock.Machineentrydate = item.currentdatetime;
                        PStock.Inpudate = DateTime.Now; PStock.Lastmodifieddate = DateTime.Now;
                        var savpur = _purchasestock.GetAll().Where(x => x.Machineentrydate.Value.Date == PStock.Machineentrydate.Value.Date).ToList();
                        if (savpur.Count() == 0)
                        {
                            _purchasestock.Add(PStock);
                        }
                        else
                        {
                            tbl_PurchseStockMaster PPStock = _purchasestock.Get(savpur.FirstOrDefault().pkid);
                            PPStock.ReceivedStock = item.qunatity;
                            try
                            {
                                PPStock.previousStock = _purchasestock.GetAll().Where(x => x.Machineentrydate.Value.Date == item.currentdatetime.Value.Date).FirstOrDefault().TotalStock ?? 0;
                            }
                            catch
                            {
                                PPStock.previousStock = 0;
                            }
                            PPStock.TotalStock = (PPStock.ReceivedStock) + (PPStock.previousStock);
                            PPStock.Machineentrydate = item.currentdatetime;
                            _purchasestock.Update(PPStock);
                        }
                        tbl_PurchaseEntry_Master abc = _purchasepayment.Get(item.pkid);
                        abc.stockread = 1;
                        _purchasepayment.Update(abc);
                    }

                }
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/ErrorLog.txt"));
            }
            return View();
        }
        public ActionResult PurchaseStockList()
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
                var v = (from a in _purchasestock.GetAll()
                         join b in _productmaster.GetAll() on a.product_fkid equals b.pkid
                         select new
                         {
                             pkid = a.pkid,
                             prodname = b.productname ?? "",
                             receiveddt = a.Machineentrydate ?? default(DateTime),
                             previousstock = a.previousStock ?? 0,
                             receivedstock = a.ReceivedStock ?? 0,
                             Bales = (_salesstock.GetAll().Where(x => x.lastmodifieddate.Value.Date == a.Machineentrydate.Value.Date).Count() != 0 ? _salesstock.GetAll().Where(x => x.lastmodifieddate.Value.Date == a.Machineentrydate.Value.Date).FirstOrDefault().produceCottonbells : 0),
                             Seed = (_salesstock.GetAll().Where(x => x.lastmodifieddate.Value.Date == a.Machineentrydate.Value.Date).Count() != 0 ? _salesstock.GetAll().Where(x => x.lastmodifieddate.Value.Date == a.Machineentrydate.Value.Date).FirstOrDefault().produceCottonSeed : 0),
                             totalstock = a.TotalStock ?? 0
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.prodname.ToLower().Contains(search.ToLower())) select b);
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

        [HttpGet]
        public ActionResult StockProccess(string date)
        {
            try
            {
                #region
                var data = _purchasestock.GetAll();
                if (data.Count() == 0)
                {
                    var purch = _purchasepayment.GetAll().Where(x => x.stockread != 1);
                    foreach (var item in purch.OrderBy(x => x.currentdatetime).ToList())
                    {
                        tbl_PurchseStockMaster PStock = new tbl_PurchseStockMaster();
                        PStock.purchase_fkid = item.purchasemachine_fkid;
                        PStock.product_fkid = _machinedata.Get(item.purchasemachine_fkid).producttypeid;
                        PStock.ReceivedStock = item.qunatity;
                        try
                        {
                            PStock.previousStock = _purchasestock.GetAll().Where(x => x.Machineentrydate.Value.Date == item.currentdatetime.Value.Date).FirstOrDefault().TotalStock ?? 0;
                        }
                        catch
                        {
                            PStock.previousStock = 0;
                        }
                        PStock.TotalStock = (PStock.ReceivedStock) + (PStock.previousStock);
                        PStock.Machineentrydate = item.currentdatetime;
                        PStock.Inpudate = DateTime.Now; PStock.Lastmodifieddate = DateTime.Now;
                        var savpur = _purchasestock.GetAll().Where(x => x.Machineentrydate.Value.Date == PStock.Machineentrydate.Value.Date).ToList();
                        if (savpur.Count() == 0)
                        {
                            _purchasestock.Add(PStock);
                        }
                        else
                        {
                            tbl_PurchseStockMaster PPStock = _purchasestock.Get(savpur.FirstOrDefault().pkid);
                            PPStock.ReceivedStock = item.qunatity;
                            try
                            {
                                PPStock.previousStock = _purchasestock.GetAll().Where(x => x.Machineentrydate.Value.Date == item.currentdatetime.Value.Date).FirstOrDefault().TotalStock ?? 0;
                            }
                            catch
                            {
                                PPStock.previousStock = 0;
                            }
                            PPStock.TotalStock = (PPStock.ReceivedStock) + (PPStock.previousStock);
                            PPStock.Machineentrydate = item.currentdatetime;
                            _purchasestock.Update(PPStock);
                        }
                        tbl_PurchaseEntry_Master abc = _purchasepayment.Get(item.pkid);
                        abc.stockread = 1;
                        _purchasepayment.Update(abc);
                    }
                }
                else
                {
                    DateTime datefrom = (DateTime)data.OrderByDescending(x => x.pkid).FirstOrDefault().Machineentrydate;
                    var purch = _purchasepayment.GetAll().Where(x => x.stockread != 1);
                    foreach (var item in purch.OrderBy(x => x.currentdatetime).ToList())
                    {
                        tbl_PurchseStockMaster PStock = new tbl_PurchseStockMaster();
                        PStock.purchase_fkid = item.purchasemachine_fkid;
                        PStock.product_fkid = _machinedata.Get(item.purchasemachine_fkid).producttypeid;
                        PStock.ReceivedStock = item.qunatity;
                        try
                        {
                            PStock.previousStock = _purchasestock.GetAll().Where(x => x.Machineentrydate.Value.Date == item.currentdatetime.Value.Date).FirstOrDefault().TotalStock ?? 0;
                        }
                        catch
                        {
                            PStock.previousStock = 0;
                        }
                        PStock.TotalStock = (PStock.ReceivedStock) + (PStock.previousStock);
                        PStock.Machineentrydate = item.currentdatetime;
                        PStock.Inpudate = DateTime.Now; PStock.Lastmodifieddate = DateTime.Now;
                        var savpur = _purchasestock.GetAll().Where(x => x.Machineentrydate.Value.Date == PStock.Machineentrydate.Value.Date).ToList();
                        if (savpur.Count() == 0)
                        {
                            _purchasestock.Add(PStock);
                        }
                        else
                        {
                            tbl_PurchseStockMaster PPStock = _purchasestock.Get(savpur.FirstOrDefault().pkid);
                            PPStock.ReceivedStock = item.qunatity;
                            try
                            {
                                PPStock.previousStock = _purchasestock.GetAll().Where(x => x.Machineentrydate.Value.Date == item.currentdatetime.Value.Date).FirstOrDefault().TotalStock ?? 0;
                            }
                            catch
                            {
                                PPStock.previousStock = 0;
                            }
                            PPStock.TotalStock = (PPStock.ReceivedStock) + (PPStock.previousStock);
                            PPStock.Machineentrydate = item.currentdatetime;
                            _purchasestock.Update(PPStock);
                        }
                        tbl_PurchaseEntry_Master abc = _purchasepayment.Get(item.pkid);
                        abc.stockread = 1;
                        _purchasepayment.Update(abc);
                    }

                }
                #endregion
                var CHeckSales = _salesstock.GetAll().Where(x => x.lastmodifieddate.Value.Date == DateTime.Now.Date).ToList();
                if (date != null)
                {
                    DateTime D = Convert.ToDateTime(date);
                    CHeckSales = _salesstock.GetAll().Where(x => x.lastmodifieddate.Value.Date == D.Date).ToList();
                }
                if (CHeckSales.Count() != 0)
                {
                    tbl_SalesSockMasterss abc = new tbl_SalesSockMasterss();
                    abc.pkid = CHeckSales.FirstOrDefault().pkid;
                    abc.totalStock = CHeckSales.FirstOrDefault().totalStock;
                    abc.produceTotalStock = CHeckSales.FirstOrDefault().produceTotalStock;
                    abc.lastmodifieddate = CHeckSales.FirstOrDefault().lastmodifieddate;
                    abc.produceCottonbells = CHeckSales.FirstOrDefault().produceCottonbells;
                    abc.produceCottonSeed = CHeckSales.FirstOrDefault().produceCottonSeed;
                    abc.balancestock = CHeckSales.FirstOrDefault().balancestock;
                    return View(abc);
                }
                else
                {
                    if (date == null)
                    {
                        ViewBag.totalstock = 0;
                    }
                    else
                    {
                        DateTime D = Convert.ToDateTime(date);
                        ViewBag.totalstock = _purchasestock.GetAll().Where(x => x.Machineentrydate.Value.Date == D.Date).OrderByDescending(x => x.pkid).FirstOrDefault().TotalStock ?? 0;
                        ViewBag.d = D.ToString("MM/dd/yyyy");
                    }
                    return View();
                }
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/ErrorLog.txt"));
            }
            return View();
        }
        [HttpPost]
        public ActionResult StockProccess(tbl_SalesSockMasterss model)
        {
            var savpur = _salesstock.GetAll().Where(x => x.lastmodifieddate.Value.Date == model.lastmodifieddate.Value.Date).ToList();
            int pkid = 0;
            if (savpur.Count() != 0)
            {
                pkid = savpur.FirstOrDefault().pkid;
            }
            if (pkid == 0)
            {
                tbl_SalesSockMaster abc = new tbl_SalesSockMaster();
                abc.totalStock = model.totalStock;
                abc.produceTotalStock = model.produceTotalStock;
                abc.lastmodifieddate = model.lastmodifieddate;
                abc.produceCottonbells = model.produceCottonbells;
                abc.produceCottonSeed = model.produceCottonSeed;
                abc.balancestock = model.balancestock;
                _salesstock.Add(abc);
                tbl_PurchseStockMaster PUR = _purchasestock.GetAll().Where(x => x.Machineentrydate.Value.Date == model.lastmodifieddate.Value.Date).FirstOrDefault() ?? null;
                if (PUR != null)
                {
                    PUR.TotalStock = abc.balancestock;
                    _purchasestock.Update(PUR);
                }
                else
                {
                    PUR = _purchasestock.GetAll().Where(x => x.Machineentrydate.Value.Date <= model.lastmodifieddate.Value.Date).OrderByDescending(x => x.pkid).FirstOrDefault();
                    PUR.TotalStock = abc.balancestock;
                    _purchasestock.Update(PUR);
                }
            }
            else
            {
                tbl_SalesSockMaster abc = _salesstock.Get(pkid);
                abc.totalStock = model.totalStock;
                abc.produceTotalStock = model.produceTotalStock;
                abc.lastmodifieddate = model.lastmodifieddate;
                abc.produceCottonbells = model.produceCottonbells;
                abc.produceCottonSeed = model.produceCottonSeed;
                abc.balancestock = model.balancestock;
                _salesstock.Update(abc);
                tbl_PurchseStockMaster PUR = _purchasestock.GetAll().Where(x => x.Lastmodifieddate.Value.Date == model.lastmodifieddate.Value.Date).FirstOrDefault();
                if (PUR != null)
                {
                    PUR.TotalStock = abc.balancestock;
                    _purchasestock.Update(PUR);
                }
                else
                {
                    PUR = _purchasestock.GetAll().Where(x => x.Lastmodifieddate.Value.Date <= model.lastmodifieddate.Value.Date).OrderByDescending(x => x.pkid).FirstOrDefault();
                    PUR.TotalStock = abc.balancestock;
                    _purchasestock.Update(PUR);
                }
            }

            return RedirectToAction("StockProccess", "KisanSales");
        }
        public ActionResult SalesStockList()
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
                var v = (from a in _salesstock.GetAll()
                         select new
                         {
                             pkid = a.pkid,
                             date = a.lastmodifieddate ?? default(DateTime),
                             total = a.totalStock ?? 0,
                             balles = a.produceCottonbells ?? 0,
                             seed = a.produceCottonSeed ?? 0,
                             balance = a.balancestock ?? 0,

                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.date.ToString().Contains(search)) select b);
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
        [HttpGet]
        public ActionResult Currentsummary()
        {
            List<Currentstocklist> SLIST = new List<Currentstocklist>();
            var data = _salesstock.GetAll().OrderByDescending(x => x.lastmodifieddate).FirstOrDefault();
            for (int i = 1; i <= 3; i++)
            {
                if (i == 1)
                {
                    Currentstocklist abc = new Currentstocklist();
                    if (data != null)
                    {

                        abc.productname = "Cotton";
                        abc.date = data.lastmodifieddate;
                        abc.openningstock = data.totalStock;
                        abc.proocessBales = data.produceCottonbells;
                        abc.proccessseed = data.produceCottonSeed;
                        abc.closingstock = data.balancestock;
                    }
                    SLIST.Add(abc);
                }
                if (i == 2)
                {

                    if (data != null)
                    {

                        Currentstocklist abc = new Currentstocklist();
                        abc.productname = "Cotton Bales";
                        abc.date = data.lastmodifieddate;
                        abc.openningstock = data.produceCottonbells;
                        abc.proocessBales = 0;
                        abc.proccessseed = 0;
                        abc.closingstock = data.produceCottonbells;
                        SLIST.Add(abc);
                    }
                }
                if (i == 3)
                {
                    if (data != null)
                    {
                        Currentstocklist abc = new Currentstocklist();
                        abc.productname = "Cotton Seed";
                        abc.date = data.lastmodifieddate;
                        abc.openningstock = data.produceCottonSeed;
                        abc.proocessBales = 0;
                        abc.proccessseed = 0;
                        abc.closingstock = data.produceCottonSeed;

                        SLIST.Add(abc);
                    }
                }
            }

            return View(SLIST);
        }
        [HttpGet]
        public ActionResult totalsummary()
        {
            return View();
        }
        public ActionResult TotalStockList()
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
                var v = (from a in _salesstock.GetAll()
                         select new Currentstocklist
                         {
                             pkid = a.pkid,
                             productname = "Cotton",
                             date = a.lastmodifieddate ?? default(DateTime),
                             openningstock = a.totalStock ?? 0,
                             proocessBales = a.produceCottonbells ?? 0,
                             proccessseed = a.produceCottonSeed ?? 0,
                             closingstock = a.balancestock ?? 0,
                         });
                var w = (from a in _salesstock.GetAll()
                         select new Currentstocklist
                         {
                             pkid = a.pkid,
                             productname = "Cotton Bales",
                             date = a.lastmodifieddate ?? default(DateTime),
                             openningstock = a.produceCottonbells ?? 0,
                             proocessBales = 0,
                             proccessseed = 0,
                             closingstock = 0,
                         });
                var y = (from a in _salesstock.GetAll()
                         select new Currentstocklist
                         {
                             pkid = a.pkid,
                             productname = "Cotton Seed",
                             date = a.lastmodifieddate ?? default(DateTime),
                             openningstock = a.produceCottonSeed ?? 0,
                             proocessBales = 0,
                             proccessseed = 0,
                             closingstock = 0,
                         });
                List<Currentstocklist> abc = new List<Currentstocklist>();
                Currentstocklist qwe = new Currentstocklist();
                v = v.Union(w);
                v = v.Union(y);

                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.productname.ToLower().Contains(search.ToLower())) select b);
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

        [HttpGet]
        public ActionResult Expenses(int _id = 0)
        {
            ViewBag.pr = new SelectList(_expensesPurpose.GetAll(), "pkid", "purposename");
            ViewBag.pay = new SelectList(_payementtype.GetAll(), "pkid", "paymenttypename");

            if (_id > 0)
            {

                var model = _expenses.Get(_id);
                if (model.Payment_type == 1)
                {
                    ViewBag.holder = new SelectList(_RTGSMASTER.GetAll().Where(x => x.pkid == model.Pay_Infofkid).ToList(), "pkid", "AccountHolder");
                }
                else
                {
                    ViewBag.holder = new SelectList(_RTGSMASTER.GetAll().Where(x => x.pkid == model.Pay_Infofkid).ToList(), "pkid", "Cheq_Partyname");
                }

                tbl_ExpensesMasterss abc = new tbl_ExpensesMasterss();
                abc.pkid = model.pkid;
                abc.personname = model.personname;
                abc.purposeid = model.purposeid;
                abc.date = model.date;
                abc.Payment_type = model.Payment_type;
                abc.Pay_Infofkid = model.Pay_Infofkid;
                abc.amt = model.amt;
                abc.remark = model.remark;
                return View(abc);
            }
            else
            {
                ViewBag.holder = new SelectList(_RTGSMASTER.GetAll().Where(x => x.pkid == 47).ToList(), "pkid", "AccountHolder");
                ViewBag.cheholdername = new SelectList(_RTGSMASTER.GetAll().Where(x => x.pkid == 48).ToList(), "pkid", "Cheq_Partyname");
                return View();
            }
            return View();
        }
        [HttpPost]
        public ActionResult Expenses(tbl_ExpensesMasterss model)
        {
            try
            {
                if (model.pkid == 0)
                {
                    tbl_ExpensesMaster abc = new tbl_ExpensesMaster();
                    abc.personname = model.personname;
                    abc.purposeid = model.purposeid;
                    abc.date = model.date;
                    abc.Payment_type = model.Payment_type;
                    abc.Pay_Infofkid = model.Pay_Infofkid;
                    abc.amt = model.amt;
                    abc.remark = model.remark;
                    _expenses.Add(abc);
                }
                else
                {
                    tbl_ExpensesMaster abc = _expenses.Get(model.pkid);
                    abc.personname = model.personname;
                    abc.purposeid = model.purposeid;
                    abc.date = model.date;
                    abc.Payment_type = model.Payment_type;
                    abc.Pay_Infofkid = model.Pay_Infofkid;
                    abc.amt = model.amt;
                    abc.remark = model.remark;
                    _expenses.Update(abc);

                }
                return RedirectToAction("Expenses", "KisanSales");
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/ErrorLog.txt"));
            }
            return RedirectToAction("Expenses", "KisanSales");
        }
        [HttpGet]
        public ActionResult Expenseslist()
        {
            return View();
        }
        public ActionResult ExpensesListList()
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
                var v = (from a in _expenses.GetAll()
                         select new
                         {
                             pkid = a.pkid,
                             pername = a.personname ?? "",
                             date = a.date ?? DateTime.Now,
                             pur = _expensesPurpose.Get(a.purposeid).purposename ?? "",
                             pay = _payementtype.Get(a.Payment_type).paymenttypename ?? "",
                             recivername = (a.Payment_type == 1 ? _RTGSMASTER.Get(a.Pay_Infofkid).AccountHolder : _RTGSMASTER.Get(a.Pay_Infofkid).Cheq_Partyname),
                             amt = a.amt ?? 0,
                             remark = a.remark ?? "",
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.pername.ToLower().Contains(search.ToLower()) || x.recivername.ToLower().Contains(search.ToLower())) select b);
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

        [HttpGet]
        public ActionResult SalesOutstanding(int pkid = 0)
        {
            ViewBag.dt = DateTime.Now.ToShortDateString();
            ViewBag.farmer = new SelectList(_salesparty.GetAll().OrderBy(x => x.partyname), "pkid", "partyname");
            ViewBag.dealer = new SelectList(_broker.GetAll().OrderBy(x => x.Brokername), "pkid", "Brokername");
            ViewBag.paytype = new SelectList(_payementtype.GetAll(), "pkid", "paymenttypename");
            if (pkid == 0)
            {
                return View();
            }
            else
            {
                tbl_SalesOutStandingss abc = new tbl_SalesOutStandingss();
                tbl_SalesOutStanding model = _PurchaseOutStanding.Get(pkid);
                abc.partyid = model.partyid;
                abc.brokerid = model.brokerid;
                abc.outAmt = model.outAmt;
                abc.payingamt = model.payingamt;
                abc.SystDate = DateTime.Now;
                return View(abc);
            }
            return View();
        }
        [HttpPost]
        public ActionResult SalesOutstanding(tbl_SalesOutStandingss model)
        {
            try
            {
                if (model.pkid == 0)
                {
                    if (model.outAmt == null || model.outAmt == 0)
                    {
                        ViewBag.dt = DateTime.Now.ToShortDateString();
                        ViewBag.farmer = new SelectList(_salesparty.GetAll().OrderBy(x => x.partyname), "pkid", "partyname");
                        ViewBag.dealer = new SelectList(_broker.GetAll().OrderBy(x => x.Brokername), "pkid", "Brokername");
                        ViewBag.paytype = new SelectList(_payementtype.GetAll(), "pkid", "paymenttypename");
                        ViewBag.problem = "sucess";
                        return View(model);
                    }
                    tbl_SalesOutStanding abc = new tbl_SalesOutStanding();
                    abc.partyid = model.partyid;
                    abc.brokerid = model.brokerid;
                    abc.outAmt = model.outAmt;
                    abc.payingamt = model.payingamt;
                    abc.SystDate = DateTime.Now;
                    _PurchaseOutStanding.Add(abc);
                }
                else
                {
                    tbl_SalesOutStanding abc = new tbl_SalesOutStanding();
                    abc.pkid = model.pkid;
                    abc.partyid = model.partyid;
                    abc.brokerid = model.brokerid;
                    abc.outAmt = model.outAmt;
                    abc.payingamt = model.payingamt;
                    abc.SystDate = DateTime.Now;
                    _PurchaseOutStanding.Update(abc);
                }
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Path.Combine("~/ErrorLog.txt"));
            }
            return RedirectToAction("PurOuTCompete", "KisanSales");
        }
        public ActionResult SavePurpose(string[] poopupdatasave)
        {
            tbl_ExpensesPurposeMaster abc = new tbl_ExpensesPurposeMaster();
            abc.purposename = poopupdatasave[0];
            abc.mdate = DateTime.Now;
            _expensesPurpose.Add(abc);
            int mid = _expensesPurpose.GetAll().Max(x => x.pkid);
            var name = _expensesPurpose.Get(mid).purposename;
            string op = "<option value=" + mid + ">" + name + "</option>";
            return Json(op, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetExpensesList(string dt, int pr)
        {
            DateTime _p = Convert.ToDateTime(dt);
            var data = (from expense in _expenses.GetAll().Where(x => x.purposeid == pr && x.date == _p)
                        join ex in _expensesPurpose.GetAll() on expense.purposeid equals ex.pkid

                        where expense.purposeid == ex.pkid
                        select new
                        {
                            pkid = expense.pkid,
                            personname = expense.personname,
                            purposename = ex.purposename,
                            dta = expense.date,
                            pay = _payementtype.Get(expense.Payment_type).paymenttypename,
                            holder = "Kisan Ginning & Pressing",
                            amt = expense.amt,
                            remark = expense.remark,
                        }).OrderBy(m => m.dta).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult PurchaseOutstandingPayment()
        {
            return View();
        }
        public ActionResult PurchaseoutstandingList()
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
                var v = (from a in _PurchaseOutStanding.GetAll().Where(x => x.statusa != 1).ToList()

                         select new
                         {
                             pkid = a.pkid,
                             farmername = (a.partyid != null ? _salesparty.Get(a.partyid).partyname : ""),
                             dealername = (a.brokerid != null ? _broker.Get(a.brokerid).Brokername : ""),
                             paymentmode = (a.Payment_Method != null ? _payementtype.Get(a.Payment_Method).paymenttypename : ""),
                             paidto = a.Accoun_holder ?? a.CheqPartyname ?? a.CashPartyName ?? "",
                             details = a.Details ?? "",
                             paydate = a.SystDate,
                             amt = a.CheqAmt ?? a.CashAmt ?? a.RTGSAMT ?? 0
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.farmername.ToLower().Contains(search.ToLower())) select b);
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
        public ActionResult ComPurchaseoutstandingList()
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
                var v = (from a in _PurchaseOutStanding.GetAll()

                         select new
                         {
                             pkid = a.pkid,
                             farmername = (a.partyid != null ? _salesparty.Get(a.partyid).partyname : ""),
                             dealername = (a.brokerid != null ? _broker.Get(a.brokerid).Brokername : ""),
                             paydate = a.SystDate,
                             outamt = a.outAmt,
                             amt = a.payingamt,
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.farmername.ToLower().Contains(search.ToLower())) select b);
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

        public ActionResult PurOuTCompete()
        {
            return View();
        }
        public ActionResult DeleteOutstanding(int id)
        {
            try
            {
                tbl_SalesOutStanding abc = _PurchaseOutStanding.Get(id);
                _PurchaseOutStanding.Remove(abc);
                return RedirectToAction("PurOuTCompete", "KisanSales");
            }
            catch
            {
                return RedirectToAction("PurOuTCompete", "KisanSales");
            }
        }
        public ActionResult UpdatestatusOutstanding(string[] poopupdatasave)
        {
            try
            {
                var abc = _PurchaseOutStanding.Get(Convert.ToInt32(poopupdatasave[0]));
                abc.Details = poopupdatasave[1];
                abc.statusa = Convert.ToInt32(poopupdatasave[2]);
                var result = _PurchaseOutStanding.Update(abc);
                if (result == abc)
                {
                    TempData["fid"] = abc.partyid.ToString();
                    TempData["did"] = abc.brokerid.ToString();
                    TempData["PayAmt"] = abc.CashAmt ?? abc.CheqAmt ?? abc.RTGSAMT;
                    string data = GetInvoiceList();
                    decimal returnamt = Convert.ToDecimal(data);
                    if (returnamt != 0)
                    {
                        return Json(returnamt, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
        }
        public string GetInvoiceList()
        {
            string Farid = TempData["fid"].ToString();
            string deal = TempData["did"].ToString();
            decimal Payamt = Convert.ToDecimal(TempData["PayAmt"].ToString());
            decimal amt = 0;
            int Fid = 0;
            int Did = 0;
            if (Farid != "")
            {
                Fid = Convert.ToInt32(Farid);
            }
            if (deal != "")
            {
                Did = Convert.ToInt32(deal);
            }
            if (Fid != 0 && Did == 0)
            {
                var Invoice = (from a in _machinedata.GetAll().Where(x => x.farmerid == Fid).ToList()
                               join b in _salesPayment.GetAll().Where(x => x.PaymentStatus != 1) on a.pkid equals b.Purchase_fkid
                               where a.pkid == b.Purchase_fkid
                               select new
                               {
                                   invoiceid = b.pkid,
                                   netpay = b.NetPayableAmount ?? 0,
                                   paysta = b.PaymentStatus ?? 0,
                               }).ToList();
                foreach (var item in Invoice)
                {
                    var payment = _salesActualPay.GetAll().Where(x => x.invoice_fkid == item.invoiceid).ToList();
                    if (payment.Count() != 0)
                    {
                        var returnPayment = payment.Where(x => x.Status == 0).ToList();
                        if (returnPayment.Count() != 0)
                        {
                            foreach (var beta in returnPayment)
                            {
                                if (beta.returnAmt <= Payamt)
                                {
                                    beta.OutstandingStatus = 1;
                                    beta.Status = 1;
                                    beta.sendtobank = 1;
                                    Payamt = (Payamt) - ((decimal)beta.returnAmt);
                                    _salesActualPay.Update(beta);
                                }
                            }
                        }
                        decimal Pendamt = payment.OrderByDescending(x => x.pkid).FirstOrDefault().pendingAmt ?? 0;
                        if (Pendamt != 0)
                        {
                            var deta = payment.OrderByDescending(x => x.pkid).FirstOrDefault();
                            if (deta.pendingAmt <= Payamt)
                            {
                                deta.OutstandingStatus = 1;
                                deta.Status = 1;
                                deta.sendtobank = 1;
                                Payamt = (Payamt) - ((decimal)deta.pendingAmt);
                                _salesActualPay.Update(deta);
                                var inda = _salesPayment.Get(deta.invoice_fkid);
                                inda.PaymentStatus = 1;
                                _salesPayment.Update(inda);
                            }
                        }
                        else
                        {
                            var againreturnPayment = payment.Where(x => x.Status == 0).ToList();
                            if (againreturnPayment.Count() != 0)
                            {
                                foreach (var beta in againreturnPayment)
                                {
                                    var inda = _salesPayment.Get(beta.invoice_fkid);
                                    inda.PaymentStatus = 1;
                                    _salesPayment.Update(inda);
                                }
                            }
                        }
                    }
                    else
                    {
                        var ceta = _salesPayment.Get(item.invoiceid);
                        if (ceta.NetPayableAmount <= Payamt)
                        {
                            ceta.PaymentStatus = 1;
                            Payamt = (Payamt) - ((decimal)ceta.NetPayableAmount);
                            _salesPayment.Update(ceta);
                        }
                    }
                }
            }
            else if (Did != 0 && Fid == 0)
            {
                var Invoice = (from a in _machinedata.GetAll().ToList()
                               join b in _salesPayment.GetAll().Where(x => x.PaymentStatus != 1 && x.brokerid == Did) on a.pkid equals b.Purchase_fkid
                               where a.pkid == b.Purchase_fkid
                               select new
                               {
                                   invoiceid = b.pkid,
                                   netpay = b.NetPayableAmount ?? 0,
                                   paysta = b.PaymentStatus ?? 0,
                               }).ToList();
                foreach (var item in Invoice)
                {
                    var payment = _salesActualPay.GetAll().Where(x => x.invoice_fkid == item.invoiceid).ToList();
                    if (payment.Count() != 0)
                    {
                        var returnPayment = payment.Where(x => x.Status == 0).ToList();
                        if (returnPayment.Count() != 0)
                        {
                            foreach (var beta in returnPayment)
                            {
                                if (beta.returnAmt <= Payamt)
                                {
                                    beta.OutstandingStatus = 1;
                                    beta.Status = 1;
                                    beta.sendtobank = 1;
                                    Payamt = (Payamt) - ((decimal)beta.returnAmt);
                                    _salesActualPay.Update(beta);
                                }
                            }
                        }
                        decimal Pendamt = payment.OrderByDescending(x => x.pkid).FirstOrDefault().pendingAmt ?? 0;
                        if (Pendamt != 0)
                        {
                            var deta = payment.OrderByDescending(x => x.pkid).FirstOrDefault();
                            if (deta.pendingAmt <= Payamt)
                            {
                                deta.OutstandingStatus = 1;
                                deta.Status = 1;
                                deta.sendtobank = 1;
                                Payamt = (Payamt) - ((decimal)deta.pendingAmt);
                                _salesActualPay.Update(deta);
                                var inda = _salesPayment.Get(deta.invoice_fkid);
                                inda.PaymentStatus = 1;
                                _salesPayment.Update(inda);
                            }
                        }
                        else
                        {
                            var againreturnPayment = payment.Where(x => x.Status == 0).ToList();
                            if (againreturnPayment.Count() != 0)
                            {
                                foreach (var beta in againreturnPayment)
                                {
                                    var inda = _salesPayment.Get(beta.invoice_fkid);
                                    inda.PaymentStatus = 1;
                                    _salesPayment.Update(inda);
                                }
                            }
                        }

                    }
                    else
                    {
                        var ceta = _salesPayment.Get(item.invoiceid);
                        if (ceta.NetPayableAmount <= Payamt)
                        {
                            ceta.PaymentStatus = 1;
                            Payamt = (Payamt) - ((decimal)ceta.NetPayableAmount);
                            _salesPayment.Update(ceta);
                        }
                    }
                }
            }
            else
            {
                var Invoice = (from a in _machinedata.GetAll().Where(x => x.farmerid == Fid).ToList()
                               join b in _salesPayment.GetAll().Where(x => x.PaymentStatus != 1 && x.brokerid == Did) on a.pkid equals b.Purchase_fkid
                               where a.pkid == b.Purchase_fkid
                               select new
                               {
                                   invoiceid = b.pkid,
                                   netpay = b.NetPayableAmount ?? 0,
                                   paysta = b.PaymentStatus ?? 0,
                               }).ToList();
                foreach (var item in Invoice)
                {
                    var payment = _salesActualPay.GetAll().Where(x => x.invoice_fkid == item.invoiceid).ToList();
                    if (payment.Count() != 0)
                    {
                        var returnPayment = payment.Where(x => x.Status == 0).ToList();
                        if (returnPayment.Count() != 0)
                        {
                            foreach (var beta in returnPayment)
                            {
                                if (beta.returnAmt <= Payamt)
                                {
                                    beta.OutstandingStatus = 1;
                                    beta.Status = 1;
                                    beta.sendtobank = 1;
                                    Payamt = (Payamt) - ((decimal)beta.returnAmt);
                                    _salesActualPay.Update(beta);
                                }
                            }
                        }
                        decimal Pendamt = payment.OrderByDescending(x => x.pkid).FirstOrDefault().pendingAmt ?? 0;
                        if (Pendamt != 0)
                        {
                            var deta = payment.OrderByDescending(x => x.pkid).FirstOrDefault();
                            if (deta.pendingAmt <= Payamt)
                            {
                                deta.OutstandingStatus = 1;
                                deta.Status = 1;
                                deta.sendtobank = 1;
                                Payamt = (Payamt) - ((decimal)deta.pendingAmt);
                                _salesActualPay.Update(deta);
                                var inda = _salesPayment.Get(deta.invoice_fkid);
                                inda.PaymentStatus = 1;
                                _salesPayment.Update(inda);
                            }
                        }
                        else
                        {
                            var againreturnPayment = payment.Where(x => x.Status == 0).ToList();
                            if (againreturnPayment.Count() != 0)
                            {
                                foreach (var beta in againreturnPayment)
                                {
                                    var inda = _salesPayment.Get(beta.invoice_fkid);
                                    inda.PaymentStatus = 1;
                                    _salesPayment.Update(inda);
                                }
                            }
                        }
                    }
                    else
                    {
                        var ceta = _salesPayment.Get(item.invoiceid);
                        if (ceta.NetPayableAmount <= Payamt)
                        {
                            ceta.PaymentStatus = 1;
                            Payamt = (Payamt) - ((decimal)ceta.NetPayableAmount);
                            _salesPayment.Update(ceta);
                        }
                    }
                }
            }
            string data = Payamt.ToString();
            return data;
        }
        public ActionResult GetOutStanding(string Farid, string deal)
        {
            decimal amt = 0;
            int Fid = 0;
            int Did = 0;
            if (Farid != "")
            {
                Fid = Convert.ToInt32(Farid);
            }
            if (deal != "")
            {
                Did = Convert.ToInt32(deal);
            }
            if (Fid != 0 && Did == 0)
            {
                var Invoice = (from a in _machinedata.GetAll().Where(x => x.farmerid == Fid).ToList()
                               join b in _salesPayment.GetAll().Where(x => x.PaymentStatus != 1) on a.pkid equals b.Purchase_fkid
                               where a.pkid == b.Purchase_fkid
                               select new
                               {
                                   invoiceid = b.pkid,
                                   netpay = b.NetPayableAmount ?? 0,
                               }).ToList();
                foreach (var item in Invoice)
                {
                    var payment = _salesActualPay.GetAll().Where(x => x.invoice_fkid == item.invoiceid).ToList();
                    if (payment.Count() != 0)
                    {
                        decimal Pendamt = payment.OrderByDescending(x => x.pkid).FirstOrDefault().pendingAmt ?? 0;
                        var returnPayment = payment.Where(x => x.Status == 0).ToList();
                        if (returnPayment.Count() != 0)
                        {
                            foreach (var beta in returnPayment)
                            {
                                Pendamt = (decimal)beta.returnAmt;
                            }
                        }
                        amt = (amt + Pendamt);
                    }
                    else
                    {
                        amt = (amt + item.netpay);
                    }
                }
            }
            else if (Did != 0 && Fid == 0)
            {
                var Invoice = (from a in _machinedata.GetAll().ToList()
                               join b in _salesPayment.GetAll().Where(x => x.PaymentStatus != 1 && x.brokerid == Did) on a.pkid equals b.Purchase_fkid
                               where a.pkid == b.Purchase_fkid
                               select new
                               {
                                   invoiceid = b.pkid,
                                   netpay = b.NetPayableAmount ?? 0,
                               }).ToList();
                foreach (var item in Invoice)
                {
                    var payment = _salesActualPay.GetAll().Where(x => x.invoice_fkid == item.invoiceid).ToList();
                    if (payment.Count() != 0)
                    {
                        decimal Pendamt = payment.OrderByDescending(x => x.pkid).FirstOrDefault().pendingAmt ?? 0;
                        var returnPayment = payment.Where(x => x.Status == 0).ToList();
                        if (returnPayment.Count() != 0)
                        {
                            foreach (var beta in returnPayment)
                            {
                                Pendamt = (decimal)beta.returnAmt;
                            }
                        }
                        amt = (amt + Pendamt);
                    }
                    else
                    {
                        amt = (amt + item.netpay);
                    }
                }
            }
            else
            {
                var Invoice = (from a in _machinedata.GetAll().Where(x => x.farmerid == Fid).ToList()
                               join b in _salesPayment.GetAll().Where(x => x.PaymentStatus != 1 && x.brokerid == Did) on a.pkid equals b.Purchase_fkid
                               where a.pkid == b.Purchase_fkid
                               select new
                               {
                                   invoiceid = b.pkid,
                                   netpay = b.NetPayableAmount ?? 0,
                               }).ToList();
                foreach (var item in Invoice)
                {
                    var payment = _salesActualPay.GetAll().Where(x => x.invoice_fkid == item.invoiceid).ToList();
                    if (payment.Count() != 0)
                    {
                        decimal Pendamt = payment.OrderByDescending(x => x.pkid).FirstOrDefault().pendingAmt ?? 0;
                        var returnPayment = payment.Where(x => x.Status == 0).ToList();
                        if (returnPayment.Count() != 0)
                        {
                            foreach (var beta in returnPayment)
                            {
                                Pendamt = (decimal)beta.returnAmt;
                            }
                        }
                        amt = (amt + Pendamt);
                    }
                    else
                    {
                        amt = (amt + item.netpay);
                    }
                }
            }
            return Json(amt, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CashMaster()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CashMaster(tbl_CashReceiptss model)
        {
          decimal Totalcash = 0;
            if (model.pkid == 0)
            {
                tbl_CashReceipt abc = new tbl_CashReceipt();
                abc.personname = model.personname;
                abc.date = model.date;
                abc.cashAmt = model.cashAmt;
                abc.sourcename = model.sourcename;
                abc.currentdate = DateTime.Now;
                _cashreceipt.Add(abc);
                if (_cashmastereredd.GetAll().Count() > 0)
                {
                    if (_cashmastereredd.GetAll().FirstOrDefault().CurrentCash > 0)
                    {
                        tbl_CashMastered beta = _cashmastereredd.GetAll().FirstOrDefault();
                        beta.CurrentCash = (beta.CurrentCash + model.cashAmt);
                        _cashmastereredd.Update(beta);
                    }
                    else{
                        tbl_CashMastered beta = new tbl_CashMastered();
                        beta.CurrentCash = model.cashAmt;
                        //_cashmastereredd.Save(beta);
                    }
                }
                else
                {
                    //tbl_CashMastered beta = new tbl_Cash
                    // _cashmastereredd.Update(beta);Mastered();
                    //beta.CurrentCash = model.cashAmt;
                }
            }
            else
            {
                tbl_CashReceipt abc = _cashreceipt.Get(model.pkid);
                abc.personname = model.personname;
                abc.date = model.date;
                abc.cashAmt = model.cashAmt;
                abc.sourcename = model.sourcename;
                abc.currentdate = DateTime.Now;
                _cashreceipt.Update(abc);
            }
            return RedirectToAction("CashMaster", "KisanSales");
        }
        public ActionResult GetCashsourceList(string dt)
        {
            DateTime _p = Convert.ToDateTime(dt);
            var data = _cashreceipt.GetAll().Where(x => x.date == _p).OrderBy(x => x.pkid).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCashreceiptList()
        {
            var data = _cashreceipt.GetAll().Where(x => x.sourcename == "Invoice").OrderByDescending(x => x.pkid).FirstOrDefault();
            if (data == null)
            {
                var ceta = _salesActualPay.GetAll().Where(x => x.PaymentMethod == 3).ToList();
                if (ceta != null)
                {
                    foreach (var item in ceta)
                    {
                        tbl_CashReceipt abc = new tbl_CashReceipt();
                        abc.payment_fkid = item.pkid;
                        abc.personname = item.CashPaidTO;
                        abc.sourcename = "Invoice";
                        abc.mid = item.invoice_fkid;
                        abc.cashAmt = item.CashAmt ?? item.RTGSAmtPaying ?? item.ChequeAmtpaying ?? 0;
                        abc.date = item.datetime;
                        _cashreceipt.Add(abc);
                    }
                }
            }
            else
            {

                var ceta = _salesActualPay.GetAll().Where(x => x.pkid > data.payment_fkid && x.PaymentMethod == 3).ToList();
                if (ceta != null)
                {
                    foreach (var item in ceta)
                    {
                        tbl_CashReceipt abc = new tbl_CashReceipt();
                        abc.personname = item.CashPaidTO;
                        abc.sourcename = "Invoice";
                        abc.mid = item.invoice_fkid;
                        abc.cashAmt = item.CashAmt ?? item.RTGSAmtPaying ?? item.ChequeAmtpaying ?? 0;
                        abc.date = item.datetime;
                        _cashreceipt.Add(abc);
                    }
                }
            }
            return View();
        }
        public ActionResult CashreceiptList()
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
                var v = _cashreceipt.GetAll();
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.personname.ToLower().Contains(search.ToLower()) || x.sourcename.ToLower().Contains(search.ToLower())) select b);
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
        [HttpGet]
        public ActionResult CompletedPaymentR()
        {
            return View();
        }
        public ActionResult CompletedPaymentListSales()
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
                var v = (from a in _salesPayment.GetAll().Where(x => x.PaymentStatus == 1)
                         join b in _salesActualPay.GetAll().GroupBy(x => x.invoice_fkid) on a.pkid equals b.FirstOrDefault().invoice_fkid
                         select new
                         {
                             pkid = a.pkid,
                             invoiceno = "0000" + a.pkid,
                             invoicefkid = b.FirstOrDefault().invoice_fkid,
                             partyname = _salesparty.Get(a.partyid).partyname ?? "",
                             invoicedt = a.Date ?? default(DateTime),
                             totalamt = a.NetPayableAmount ?? 0,
                             balanceamt = b.FirstOrDefault().pendingAmt ?? 0,

                         });

                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.partyname.ToLower().Contains(search.ToLower())) select b);
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
        [HttpGet]
        public ActionResult GetHoleDetailsR(int id)
        {
            try
            {
                id = (int)_salesPayment.Get(id).Purchase_fkid;
                var pdata = _machinedata.Get(id);
                ViewBag.purchaseid = id;
                ViewBag.partyid = pdata.farmerid;
                ViewBag.pro = pdata.producttypeid;
                ViewBag.saleid = pdata.pkid;
                int taxid = (int)_prodtax.GetAll().Where(x => x.Product_fkid == pdata.producttypeid).FirstOrDefault().tax_fkid;
                ViewBag.tax = _taxmaster.Get(taxid).SGST;
                ViewBag.netweighKG = pdata.NetWeight;
                ViewBag.netweighQtl = (pdata.NetWeight / 100);
                ViewBag.partyname = new SelectList(_salesparty.GetAll(), "pkid", "partyname");
                ViewBag.transport = new SelectList(_transport.GetAll(), "pkid", "transportname");
                ViewBag.broker = new SelectList(_broker.GetAll(), "pkid", "Brokername");
                ViewBag.paycon = new SelectList(_payementtype.GetAll(), "pkid", "paymenttypename");
                var model = _salesPayment.GetAll().Where(x => x.Purchase_fkid == id).FirstOrDefault();
                if (model != null)
                {
                    ViewBag.trockno = _truck.Get(model.truckid).truckno;
                    tbl_SalesEntry_Masterss abc = new tbl_SalesEntry_Masterss();
                    abc.pkid = model.pkid;
                    abc.Purchase_fkid = model.Purchase_fkid; abc.ProductType_fkid = model.ProductType_fkid;
                    abc.rateperqtl = model.rateperqtl; abc.ratepercandy = model.ratepercandy; abc.kilogram = model.kilogram; abc.qtl = model.qtl;
                    abc.amount = model.amount; abc.partyid = model.partyid; abc.prno = model.prno; abc.topr = model.topr; abc.pmark = model.pmark;
                    abc.deliveryfromdate = model.deliveryfromdate; abc.brokerid = model.brokerid; abc.transport_fkid = model.transport_fkid; abc.truckid = model.truckid;
                    abc.drivername = model.drivername; abc.saudadate = model.saudadate; abc.paycondition = model.paycondition; abc.FPbales = model.FPbales;
                    abc.InsuranceNo = model.InsuranceNo; abc.Quality = model.Quality; abc.lotno = model.lotno; abc.noofbales = model.noofbales; abc.grossweight = model.grossweight; abc.tareweight = model.tareweight;
                    abc.frightothercharges = model.frightothercharges; abc.advance = model.advance; abc.packagingcgaeges = model.packagingcgaeges; abc.totalamt = model.totalamt;
                    abc.SGST = model.SGST; abc.CGST = model.CGST; abc.IGST = model.IGST; abc.NetPayableAmount = model.NetPayableAmount; abc.amtinword = model.amtinword;
                    return View(abc);
                }
                else
                {
                    return View();
                }

            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/ErrorLog.txt"));
            }
            return View();
        }

        public ActionResult Ou8ststandingpaymentList(string frid, string drid)
        {
            int fid = (frid != "" ? Convert.ToInt32(frid) : 0);
            int tid = (drid != "" ? Convert.ToInt32(drid) : 0);
            List<OutStandingPaymentList> abc = new List<OutStandingPaymentList>();
            var v = (from a in _salesPayment.GetAll()
                     join b in _machinedata.GetAll() on a.Purchase_fkid equals b.pkid
                     select new OutStandingPaymentList
                     {
                         pkid = a.pkid,
                         invoiceno = "0000" + a.pkid,
                         fid = b.farmerid ?? 0,
                         did = a.brokerid ?? 0,
                         farmername = (b.farmerid != 0 ? _salesparty.GetAll().Where(x => x.pkid == b.farmerid).FirstOrDefault().partyname : ""),
                         dealername = (a.brokerid != null ? _broker.GetAll().Where(x => x.pkid == a.brokerid).FirstOrDefault().Brokername : ""),
                         paymentdate = a.Date ?? default(DateTime),

                         creditedamount = (_salesActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).ToList() != null ? _salesActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).Sum(x => x.RTGSAmtPaying ?? 0 + x.ChequeAmtpaying ?? 0 + x.CashAmt ?? 0) : 0),

                         totalamount = a.NetPayableAmount ?? 0,
                         BalalnceAmount = (_salesActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).Count() > 0 ? _salesActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).OrderByDescending(x => x.pkid).FirstOrDefault().pendingAmt : a.NetPayableAmount),
                     }).ToList();
            if (fid != 0)
            {
                v = v.Where(x => x.fid == fid).ToList();
            }
            if (tid != 0)
            {
                v = v.Where(x => x.did == tid).ToList();
            }

            ViewBag.summ = v.Sum(x => x.BalalnceAmount).Value;
            abc = v;
            return View(abc);
        }
    }
}
