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
   
    public class HomeController : Controller
    {
        public readonly Irepository<tbl_UserDetails> _userdetail;
        public readonly Irepository<tbl_PurchaseEntry_Master> _purchasePayment;
        public readonly Irepository<tbl_pool_MachineData_Purchase> _machinedata;
        public readonly Irepository<tbl_PurchaseEntry_Master_Payment> _purchaseActualPay;
        public readonly Irepository<tbl_SalesEntry_Master_Payment> _salesActualPay;
        public readonly Irepository<tbl_SalesEntry_Master> _salesPayment;
        public readonly Irepository<tbl_ExpensesMaster> _expense;
        public HomeController(Irepository<tbl_UserDetails> userdetail, Irepository<tbl_PurchaseEntry_Master> purchasePayment, Irepository<tbl_pool_MachineData_Purchase> machinedata,
            Irepository<tbl_PurchaseEntry_Master_Payment> purchaseActualPay, Irepository<tbl_SalesEntry_Master_Payment> salesActualPay,
            Irepository<tbl_SalesEntry_Master> salesPayment, Irepository<tbl_ExpensesMaster> expense)
          {
              _userdetail = userdetail;
              _purchasePayment = purchasePayment;
              _machinedata = machinedata;
              _purchaseActualPay = purchaseActualPay;
              _salesActualPay = salesActualPay;
              _salesPayment = salesPayment;
              _expense = expense;
          }
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                var useremail = Session["UserEmail"].ToString();
                var data = _userdetail.GetAll();
                Session["UserFirstName"] = data.Where(x => x.emailid.ToLower() == useremail.ToLower()).FirstOrDefault().firstname;
            }
            catch {
                return RedirectToAction("login", "Account");
            }
            dashboarddata abc = new dashboarddata();
            var v = (from a in _purchasePayment.GetAll()
                     join b in _machinedata.GetAll() on a.purchasemachine_fkid equals b.pkid
                     select new listpurchaseledger
                     {
                         balance = (_purchaseActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).Count() > 0 ? _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).OrderByDescending(x => x.pkid).FirstOrDefault().pendingAmt ?? 0 : a.netpayable_amt),
                     }).ToList();
            decimal debit = v.Sum(x => x.balance).Value;
            ViewBag.debit = debit;
            var s = (from a in _salesPayment.GetAll()
                     join b in _machinedata.GetAll() on a.Purchase_fkid equals b.pkid
                     select new listpurchaseledger
                     {
                         balance = (_salesActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).Count() > 0 ? _salesActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).OrderByDescending(x => x.pkid).FirstOrDefault().pendingAmt : a.NetPayableAmount),
                     }).ToList();
            decimal credit = s.Sum(x => x.balance).Value;
            ViewBag.credit = credit;
            decimal expense = (_expense.GetAll().Where(x => x.date.Value.Date == DateTime.Now.Date).ToList() != null ? _expense.GetAll().Where(x => x.date.Value.Date == DateTime.Now.Date).ToList().Sum(x => x.amt).Value : 0);
            abc.debit = debit;
            abc.credit = credit;
            abc.totalexpenses = expense;
            abc.cotton = (decimal)125.20;
            ViewBag.expense = expense;
            ViewBag.co = "125.50";
            return View();           
        }      
        public ActionResult Mainpage()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Homedata()
        {
            dashboarddata abc = new dashboarddata();
            var v = (from a in _purchasePayment.GetAll()
                     join b in _machinedata.GetAll() on a.purchasemachine_fkid equals b.pkid
                     select new listpurchaseledger
                     {                      
                         balance = (_purchaseActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).Count() > 0 ? _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).OrderByDescending(x => x.pkid).FirstOrDefault().pendingAmt ?? 0 : a.netpayable_amt),
                     }).ToList();
           decimal debit = v.Sum(x => x.balance).Value;
           ViewBag.debit = debit;
           var s = (from a in _salesPayment.GetAll()
                    join b in _machinedata.GetAll() on a.Purchase_fkid equals b.pkid
                    select new listpurchaseledger
                    {
                        balance = (_salesActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).Count() > 0 ? _salesActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).OrderByDescending(x => x.pkid).FirstOrDefault().pendingAmt : a.NetPayableAmount),
                    }).ToList();
           decimal credit = s.Sum(x => x.balance).Value;
           ViewBag.credit = credit;
           decimal expense =(_expense.GetAll().Where(x => x.date.Value.Date == DateTime.Now.Date).ToList() != null ? _expense.GetAll().Where(x => x.date.Value.Date == DateTime.Now.Date).ToList().Sum(x => x.amt).Value : 0);
           abc.debit = debit;
           abc.credit = credit;
           abc.totalexpenses = expense;
           abc.cotton = (decimal)125.20;
           ViewBag.expense = expense;
           ViewBag.co = "125.50";
            return Json(abc, JsonRequestBehavior.AllowGet);
        }
    }
}