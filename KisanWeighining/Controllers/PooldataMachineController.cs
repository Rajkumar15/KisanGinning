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
    public class PooldataMachineController : Controller
    {
        public readonly Irepository<tbl_RemoteConnection> _remote;
        public readonly Irepository<tbl_pool_MachineData_Purchase> _machinedata;
        public readonly Irepository<tbl_FarmerMaster> _farmer;
        public readonly Irepository<tbl_DealerMaster> _dealer;
        public readonly Irepository<tbl_truckMaster> _truck;
        public readonly Irepository<tbl_MenuMaster> _menumaster;
        public readonly Irepository<tbl_MasterMenuName> _Mastermenumaster;
        public readonly Irepository<tbl_PaymentType> _payementtype;
        public readonly Irepository<tbl_PurchaseEntry_Master> _purchasePayment;
        public readonly Irepository<tbl_PurchaseEntry_Master_Payment> _purchaseActualPay;
        public readonly Irepository<tbl_salesPartyRegistartion> _salesparty;
        public readonly Irepository<tbl_PurchaseOutStanding> _PurchaseOutStanding;
        public readonly Irepository<tbl_CashMastered> _cashmastereredd;

        public readonly Irepository<tbl_productMaster> _productmaster;
        public readonly Irepository<tbl_BankDetailsPerPurchase> _RTGSMASTER;
        public PooldataMachineController(Irepository<tbl_RemoteConnection> remote, Irepository<tbl_pool_MachineData_Purchase> machinedata,
            Irepository<tbl_FarmerMaster> farmer, Irepository<tbl_truckMaster> truck, Irepository<tbl_MenuMaster> menuuu, Irepository<tbl_MasterMenuName> Mastermenu,
            Irepository<tbl_PaymentType> paymenttype, Irepository<tbl_PurchaseEntry_Master> purchasepayement, Irepository<tbl_PurchaseEntry_Master_Payment> purchaseActualPay,
           Irepository<tbl_productMaster> productmaster, Irepository<tbl_BankDetailsPerPurchase> RTGSMASTER, Irepository<tbl_DealerMaster> dealer,
            Irepository<tbl_salesPartyRegistartion> salesparty, Irepository<tbl_PurchaseOutStanding> purchaseoutstanding, Irepository<tbl_CashMastered> cashmastereredd)
        {
            _remote = remote;
            _machinedata = machinedata;
            _farmer = farmer;
            _truck = truck;
            _menumaster = menuuu;
            _Mastermenumaster = Mastermenu;
            _payementtype = paymenttype;
            _purchasePayment = purchasepayement;
            _purchaseActualPay = purchaseActualPay;
            _PurchaseOutStanding = purchaseoutstanding;
            _productmaster = productmaster;
            _RTGSMASTER = RTGSMASTER;
            _dealer = dealer;
            _salesparty = salesparty;
            _cashmastereredd = cashmastereredd;
        }
        // GET: PooldataMachine
        public ActionResult Index()
        {
            foreach (var server in _remote.GetAll())
            {
                #region
                string connectionstring = "Data Source=" + server.serverName + ";Initial Catalog=" + server.dataBaseName + ";User ID=" + server.userName + ";Password=" + server.password + ";";
                //string connectionString=_db.tbl_accessConnection.FirstOrDefault().accessconnection;

                SqlConnection con = new SqlConnection(connectionstring);
                //OleDbConnection con= new OleDbConnection(connectionstring);
                SqlCommand cmd;
                string date1 = "";
                #region
                if (date1 != "")
                {

                    long idint = 0;
                    try
                    {
                        var da = _machinedata.GetAll();
                        idint = (long)da.Max(x => x.pkid);
                    }
                    catch
                    {
                        idint = 0;

                    }
                    if (date1 == "")
                    {
                        cmd = new SqlCommand("select * from tbl_WeighningMachineFill", con);
                    }
                    else
                    {

                        DateTime dt = DateTime.Now.Date;
                        string datestring = dt.ToString("MM-dd-yyyy");
                        cmd = new SqlCommand("select * from tbl_WeighningMachineFill where  CAST(cdateTime as date)='" + datestring + "' ", con);

                    }
                    try
                    {
                        con.Open();
                        SqlDataAdapter sda = new SqlDataAdapter();
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        con.Close();
                        using (var ctx = new DataLayer.KisanModel.KisanWeighningEntities())
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {

                                var data = ds.Tables[0].Rows[i][1];

                                tbl_pool_MachineData_Purchase smd = new tbl_pool_MachineData_Purchase();


                                var farmername = ds.Tables[0].Rows[i][1].ToString().Trim();
                                tbl_FarmerMaster abc = new tbl_FarmerMaster();
                                var examfar = _farmer.GetAll();
                                var lastexam = examfar.Where(x => x.fullname == farmername).ToList();
                                if (lastexam.Count() == 0)
                                {
                                    abc.fullname = farmername;
                                    _farmer.Add(abc);
                                    var fada = _farmer.GetAll();
                                    smd.farmerid = fada.Max(x => x.pkid);
                                }
                                else
                                {
                                    smd.farmerid = lastexam.FirstOrDefault().pkid;
                                }
                                var truckno = ds.Tables[0].Rows[i][2].ToString().Trim();
                                tbl_truckMaster abctruck = new tbl_truckMaster();
                                var examtru = _truck.GetAll();
                                var lastexamtruck = examtru.Where(x => x.truckno == truckno).ToList();
                                if (lastexamtruck.Count() == 0)
                                {
                                    var trackdaa = _truck.GetAll();
                                    int maxid = trackdaa.Max(x => x.pkid);
                                    abctruck.RFIDnumber = "00" + maxid;
                                    abctruck.truckno = truckno;
                                    _truck.Add(abctruck);
                                    var trackda = _truck.GetAll();
                                    smd.truckRFID = trackda.LastOrDefault().RFIDnumber;
                                }
                                else
                                {

                                    smd.truckRFID = lastexamtruck.FirstOrDefault().RFIDnumber;
                                }
                                smd.Intime = Convert.ToDateTime(ds.Tables[0].Rows[i][3].ToString().Trim());
                                smd.outtime = Convert.ToDateTime(ds.Tables[0].Rows[i][4].ToString().Trim());


                                smd.Grossweight = Convert.ToDecimal(ds.Tables[0].Rows[i][6].ToString().Trim());
                                smd.NetWeight = Convert.ToDecimal(ds.Tables[0].Rows[i][7].ToString().Trim());
                                smd.TairWeight = Convert.ToDecimal(smd.Grossweight - smd.NetWeight);
                                smd.producttypeid = Convert.ToInt32(ds.Tables[0].Rows[i][8].ToString().Trim());
                                _machinedata.Add(smd);
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        //itechDll.Commonfunction.LogError(ex, @"~/Errorlog.txt");
                        Commonfunction.LogError(ex, Path.Combine("~/ErrorLog.txt"));

                    }
                #endregion
                }
                else
                {

                    long idint = 0;
                    try
                    {
                        var da = _machinedata.GetAll();
                        idint = (long)da.Max(x => x.machine_fkid);
                    }
                    catch
                    {
                        idint = 0;

                    }
                    if (date1 == "")
                    {
                        cmd = new SqlCommand("select * from tbl_WeighningMachineFill where pkid >'" + idint + "' ", con);
                    }
                    else
                    {
                        DateTime dt = DateTime.Now.Date;
                        string datestring = dt.ToString("MM-dd-yyyy");
                        cmd = new SqlCommand("select * from tbl_WeighningMachineFill where pkid >'" + idint + "' ", con);
                    }
                    try
                    {
                        con.Open();
                        SqlDataAdapter sda = new SqlDataAdapter();
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        con.Close();
                        using (var ctx = new DataLayer.KisanModel.KisanWeighningEntities())
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {

                                var data = ds.Tables[0].Rows[i][1];

                                tbl_pool_MachineData_Purchase smd = new tbl_pool_MachineData_Purchase();

                                smd.machine_fkid = Convert.ToInt32(ds.Tables[0].Rows[i][0].ToString().Trim());
                                smd.TicketNo = ds.Tables[0].Rows[i][1].ToString().Trim();
                                smd.ticketdate = Convert.ToDateTime(ds.Tables[0].Rows[i][2].ToString().Trim());
                                var farmername = ds.Tables[0].Rows[i][3].ToString().Trim();
                                var material = ds.Tables[0].Rows[i][10].ToString().Trim();
                                #region
                                tbl_productMaster abcprod = new tbl_productMaster();
                                var prod = _productmaster.GetAll().Where(x => x.productname == material).ToList();
                                if (prod.Count() == 0)
                                {
                                    abcprod.productname = material;
                                    abcprod.productdescription = "Industry Product";
                                    _productmaster.Add(abcprod);
                                    var fadaaa = _productmaster.GetAll();
                                    smd.producttypeid = fadaaa.Max(x => x.pkid);
                                }
                                else
                                {
                                    smd.producttypeid = prod.FirstOrDefault().pkid;
                                }
                                #endregion
                                #region
                                if (smd.producttypeid == 1)
                                {
                                    tbl_FarmerMaster abc = new tbl_FarmerMaster();
                                    var lastexam = _farmer.GetAll().Where(x => x.fullname == farmername).ToList();
                                    if (lastexam.Count() == 0)
                                    {
                                        abc.fullname = farmername;
                                        _farmer.Add(abc);
                                        var fada = _farmer.GetAll();
                                        smd.farmerid = fada.Max(x => x.pkid);
                                    }
                                    else
                                    {
                                        smd.farmerid = lastexam.FirstOrDefault().pkid;
                                    }
                                }
                                else
                                {
                                    tbl_salesPartyRegistartion abc = new tbl_salesPartyRegistartion();
                                    var lastexam = _salesparty.GetAll().Where(x => x.partyname == farmername).ToList();
                                    if (lastexam.Count() == 0)
                                    {
                                        abc.partyname = farmername;
                                        _salesparty.Add(abc);
                                        var fada = _salesparty.GetAll();
                                        smd.farmerid = fada.Max(x => x.pkid);
                                    }
                                    else
                                    {
                                        smd.farmerid = lastexam.FirstOrDefault().pkid;
                                    }
                                }
                                #endregion
                                var truckno = ds.Tables[0].Rows[i][4].ToString().Trim();
                                #region
                                tbl_truckMaster abctruck = new tbl_truckMaster();
                                var lastexamtruck = _truck.GetAll().Where(x => x.truckno == truckno).ToList();
                                if (lastexamtruck.Count() == 0)
                                {
                                    var trackdaa = _truck.GetAll();
                                    int maxid = trackdaa.Max(x => x.pkid);
                                    maxid++;
                                    abctruck.RFIDnumber = "00" + maxid;
                                    abctruck.truckno = truckno;
                                    _truck.Add(abctruck);
                                    var trackda = _truck.GetAll();
                                    smd.truckRFID = trackda.LastOrDefault().RFIDnumber;
                                }
                                else
                                {

                                    smd.truckRFID = lastexamtruck.FirstOrDefault().RFIDnumber;
                                }
                                #endregion
                                smd.Intime = Convert.ToDateTime(ds.Tables[0].Rows[i][5].ToString().Trim());
                                smd.outtime = Convert.ToDateTime(ds.Tables[0].Rows[i][6].ToString().Trim());

                                smd.Grossweight = Convert.ToDecimal(ds.Tables[0].Rows[i][7].ToString().Trim());
                                smd.TairWeight = Convert.ToDecimal(ds.Tables[0].Rows[i][8].ToString().Trim());
                                smd.NetWeight = Convert.ToDecimal(ds.Tables[0].Rows[i][9].ToString().Trim());

                                smd.transportname = ds.Tables[0].Rows[i][11].ToString().Trim();
                                smd.challanno = ds.Tables[0].Rows[i][12].ToString().Trim();
                                var cw = ds.Tables[0].Rows[i][13].ToString().Trim();
                                if (cw != "")
                                {
                                    smd.challanweight = Convert.ToDecimal(cw);
                                }

                                smd.field1 = ds.Tables[0].Rows[i][14].ToString().Trim();
                                smd.field2 = ds.Tables[0].Rows[i][15].ToString().Trim();
                                smd.field3 = ds.Tables[0].Rows[i][16].ToString().Trim();
                                smd.unit = ds.Tables[0].Rows[i][17].ToString().Trim();
                                var wc = ds.Tables[0].Rows[i][18].ToString().Trim();
                                if (wc != "")
                                {
                                    smd.challanweight = Convert.ToDecimal(wc);
                                }
                                _machinedata.Add(smd);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        Commonfunction.LogError(ex, Server.MapPath("/ErrorLog.txt"));
                    }
                }
                #endregion
            }
            return RedirectToAction("Mainpage", "Home");
        }

        public ActionResult GetdeliveryList()
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
                var v = (from a in _machinedata.GetAll().Where(x => x.invoicestatus == null && x.producttypeid == 1).Take(20)
                         select new
                         {
                             pkid = a.pkid,
                             sleepno = a.TicketNo,
                             farmer = _farmer.Get(a.farmerid).fullname,
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
        public ActionResult PurchaseDetails(int id)
        {
            try
            {
                TempData["mainid"] = id;
                ViewBag.purchaseid = id;
                int fid = Convert.ToInt32(_machinedata.Get(id).farmerid);
                ViewBag.farmernm = _farmer.Get(fid).fullname;
                var ds = _machinedata.Get(id);
                ViewBag.netweight = (ds.NetWeight / 100);
                TempData["qty"] = ds.NetWeight;
                ViewBag.dealer = new SelectList(_dealer.GetAll(), "pkid", "fullname");
                var data = _purchasePayment.GetAll().Where(x => x.purchasemachine_fkid == id).ToList();
                if (data.Count() > 0)
                {
                    tbl_PurchaseEntry_Masterss abc = new tbl_PurchaseEntry_Masterss();
                    abc.pkid = data.FirstOrDefault().pkid;
                    abc.VillageName = data.FirstOrDefault().VillageName;
                    abc.drivername = data.FirstOrDefault().drivername; abc.drivermobileno = data.FirstOrDefault().drivermobileno;
                    abc.farmermobileno = data.FirstOrDefault().farmermobileno; abc.advance_remark = data.FirstOrDefault().advance_remark; abc.Dealer_fkid = data.FirstOrDefault().Dealer_fkid;
                    abc.rate = data.FirstOrDefault().rate; abc.qunatity = data.FirstOrDefault().qunatity; abc.RTGSCharges = data.FirstOrDefault().RTGSCharges; abc.kata = data.FirstOrDefault().kata;
                    abc.unloading = data.FirstOrDefault().unloading; abc.bankcharges = data.FirstOrDefault().bankcharges; abc.CD_dalali = data.FirstOrDefault().CD_dalali; abc.advance = data.FirstOrDefault().advance;
                    abc.bhada = data.FirstOrDefault().bhada; abc.loadingcharges = data.FirstOrDefault().loadingcharges;
                    abc.totaldeduction = data.FirstOrDefault().totaldeduction; abc.netpayable_amt = data.FirstOrDefault().netpayable_amt;
                    abc.cd = data.FirstOrDefault().cd; abc.mapai = data.FirstOrDefault().mapai;
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
        public ActionResult PurchaseDetails(tbl_PurchaseEntry_Masterss model)
        {
            try
            {
                var fkid = TempData["mainid"].ToString();
                model.purchasemachine_fkid = Convert.ToInt32(TempData["mainid"].ToString());
                var data = _purchasePayment.GetAll().Where(x => x.purchasemachine_fkid == model.purchasemachine_fkid).ToList();
                if (data.Count() == 0)
                {
                    tbl_PurchaseEntry_Master abc = new tbl_PurchaseEntry_Master();
                    abc.purchasemachine_fkid = model.purchasemachine_fkid;
                    abc.VillageName = model.VillageName;
                    abc.drivername = model.drivername; abc.drivermobileno = model.drivermobileno;
                    abc.farmermobileno = model.farmermobileno; abc.advance_remark = model.advance_remark; abc.Dealer_fkid = model.Dealer_fkid;
                    abc.rate = model.rate; abc.qunatity = model.qunatity; abc.RTGSCharges = model.RTGSCharges; abc.kata = model.kata;
                    abc.unloading = model.unloading; abc.bankcharges = model.bankcharges; abc.CD_dalali = model.CD_dalali; abc.advance = model.advance;
                    abc.bhada = model.bhada; abc.loadingcharges = model.loadingcharges;
                    abc.totaldeduction = model.totaldeduction; abc.netpayable_amt = model.netpayable_amt;
                    abc.cd = model.cd; abc.mapai = model.mapai;
                    abc.currentdatetime = DateTime.Now; abc.paymentdays = model.paymentdays;
                    _purchasePayment.Add(abc);
                    var dat = _machinedata.Get(model.purchasemachine_fkid);
                    dat.invoicestatus = 1;
                    _machinedata.Update(dat);
                    tbl_BankDetailsPerPurchase asd = new tbl_BankDetailsPerPurchase();
                    asd.purchase_fkid = model.purchasemachine_fkid;
                    asd.Cheq_Partyname = "Dummy";
                    _RTGSMASTER.Add(asd);
                    int id = _purchasePayment.GetAll().OrderByDescending(x => x.pkid).FirstOrDefault().pkid;
                    return RedirectToAction("GetPDF_Purchase", "PooldataMachine", new { invoiceid = id });
                }
                else
                {
                    var datadd = _purchasePayment.Get(model.pkid);
                    datadd.drivername = model.drivername; datadd.drivermobileno = model.drivermobileno; datadd.VillageName = model.VillageName;
                    datadd.farmermobileno = model.farmermobileno; datadd.advance_remark = model.advance_remark; datadd.Dealer_fkid = model.Dealer_fkid;
                    datadd.rate = model.rate; datadd.qunatity = model.qunatity; datadd.RTGSCharges = model.RTGSCharges; datadd.kata = model.kata;
                    datadd.unloading = model.unloading; datadd.bankcharges = model.bankcharges; datadd.CD_dalali = model.CD_dalali; datadd.advance = model.advance;
                    datadd.bhada = model.bhada; datadd.loadingcharges = model.loadingcharges;
                    datadd.totaldeduction = model.totaldeduction; datadd.netpayable_amt = model.netpayable_amt;
                    datadd.cd = model.cd; datadd.mapai = model.mapai;
                    datadd.currentdatetime = DateTime.Now; datadd.paymentdays = model.paymentdays;
                    _purchasePayment.Update(datadd);
                    return RedirectToAction("GetPDF_Purchase", "PooldataMachine", new { invoiceid = datadd.pkid });
                }
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/ErrorLog.txt"));
            }
            return View();
        }
        public ActionResult SaveRTGS(string[] poopupdatasave)
        {
            try
            {
                tbl_BankDetailsPerPurchase abc = new tbl_BankDetailsPerPurchase();
                abc.Bank_name = poopupdatasave[0]; abc.Branchname = poopupdatasave[1];
                abc.Accountno = poopupdatasave[2]; abc.AccountHolder = poopupdatasave[3];
                abc.Ifsccode = poopupdatasave[4]; abc.purchase_fkid = Convert.ToInt32(poopupdatasave[5]);
                if (abc.AccountHolder != "")
                {
                    if (abc.Accountno == "" || abc.AccountHolder == "" || abc.Ifsccode == "" || abc.Bank_name == "" || abc.Branchname == "" || abc.purchase_fkid == 0)
                    {
                        return Json("failed", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        _RTGSMASTER.Add(abc);
                        abc.Cheq_Partyname = poopupdatasave[6];
                        if (abc.Cheq_Partyname != null)
                        {
                            if (abc.Cheq_Partyname != "")
                            {
                                _RTGSMASTER.Add(abc);
                            }
                        }
                        return Json("success", JsonRequestBehavior.AllowGet);
                    }
                }
                abc.Cheq_Partyname = poopupdatasave[6];
                abc.AccountHolder = "";
                if (abc.Cheq_Partyname != null)
                {
                    if (abc.Cheq_Partyname == "")
                    {
                        return Json("failed", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        _RTGSMASTER.Add(abc);
                        return Json("success", JsonRequestBehavior.AllowGet);
                    }
                }               
            }
            catch
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
            return Json("failed", JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveRTGSPay(string[] poopupdatasave)
        {
            try
            {
                tbl_BankDetailsPerPurchase abc = new tbl_BankDetailsPerPurchase();
                abc.Bank_name = poopupdatasave[0]; abc.Branchname = poopupdatasave[1];
                abc.Accountno = poopupdatasave[2]; abc.AccountHolder = poopupdatasave[3];
                abc.Ifsccode = poopupdatasave[4]; abc.purchase_fkid = Convert.ToInt32(poopupdatasave[5]);
                if (abc.AccountHolder != "")
                {
                    if (abc.Accountno == "" || abc.AccountHolder == "" || abc.Ifsccode == "" || abc.purchase_fkid == 0)
                    {
                        return Json("failed", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        _RTGSMASTER.Add(abc);
                        int mid = _RTGSMASTER.GetAll().Max(x => x.pkid);
                        var name = _RTGSMASTER.Get(mid).AccountHolder;
                        string op = "<option value=" + mid + ">" + name + "</option>";
                        return Json(op, JsonRequestBehavior.AllowGet);
                    }
                }
                abc.Cheq_Partyname = poopupdatasave[6];
                abc.AccountHolder = "";
                if (abc.Cheq_Partyname != "")
                {
                    if (abc.Cheq_Partyname == "")
                    {
                        return Json("failed", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        _RTGSMASTER.Add(abc);
                        int mid = _RTGSMASTER.GetAll().Max(x => x.pkid);
                        var name = _RTGSMASTER.Get(mid).Cheq_Partyname;
                        string op = "<option value=" + mid + ">" + name + "</option>";
                        return Json(op, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult changefarmername(int pid, string fname)
        {
            try
            {
                int fid = 0;
                try
                {
                    fid = _farmer.GetAll().Where(x => x.fullname == fname).FirstOrDefault().pkid;
                }
                catch { }
                if (fid == 0)
                {
                    tbl_FarmerMaster abc = new tbl_FarmerMaster();
                    abc.fullname = fname;
                    _farmer.Add(abc);
                    int id = _farmer.GetAll().Max(x => x.pkid);
                    var data = _machinedata.Get(pid);
                    data.farmerid = id;
                    _machinedata.Update(data);
                    return Json(fname, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data = _machinedata.Get(pid);
                    data.farmerid = fid;
                    _machinedata.Update(data);
                    var fullname = _farmer.Get(fid).fullname;
                    return Json(fullname, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddDealer(string dname)
        {
            try
            {
                var dealername = _dealer.GetAll().Where(x => x.fullname == dname).ToList();
                if (dealername.Count() == 0)
                {
                    tbl_DealerMaster abc = new tbl_DealerMaster();
                    abc.fullname = dname;
                    _dealer.Add(abc);
                    int id = _dealer.GetAll().Max(x => x.pkid);
                    var name = _dealer.Get(id).fullname;
                    string da = "<option value=" + id + ">" + name + "</option>";
                    return Json(da, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("true", JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult getdateondays(int days)
        {
            try
            {
                DateTime date = DateTime.Now.AddDays(days);
                return Json(date, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/ErrorLog.txt"));
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult pendingpayment()
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
                var v = (from a in _purchasePayment.GetAll().Where(x => x.PaymentStatus != 1).ToList()
                         join b in _machinedata.GetAll() on a.purchasemachine_fkid equals b.pkid
                         select new
                         {
                             pkid = a.pkid,
                             purchaseid = b.pkid,
                             invoiceno = "0000" + a.pkid,
                             farmername = _farmer.GetAll().Where(x => x.pkid == b.farmerid).FirstOrDefault().fullname,
                             datet = a.currentdatetime,
                             netweight = b.NetWeight,
                             payble = (a.netpayable_amt ?? 0) + (a.advance ?? 0),
                             balance = a.netpayable_amt,
                         });

                //SORT
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.farmername.ToLower().Contains(search.ToLower())) select b);
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
        public ActionResult PurchaseDetails_Payment(int id, int Bill_fkid, string pkid)
        {
            int _pkid = Convert.ToInt32(pkid);
            ViewBag.purchasid = id;
            ViewBag.invoiseid = Bill_fkid;
            var data = _purchasePayment.Get(Bill_fkid);
            ViewBag.dt = data.currentdatetime;
            ViewBag.advance = data.advance;
            ViewBag.paytype = new SelectList(_payementtype.GetAll(), "pkid", "paymenttypename");
            ViewBag.holdername = new SelectList(_RTGSMASTER.GetAll().Where(x => x.purchase_fkid == id && x.AccountHolder != "").ToList(), "pkid", "AccountHolder");
            ViewBag.cheholdername = new SelectList(_RTGSMASTER.GetAll().Where(x => x.purchase_fkid == id && x.Cheq_Partyname != null).ToList(), "pkid", "Cheq_Partyname");
            ViewBag.netweight = _machinedata.Get(id).NetWeight;
            ViewBag.rate = data.rate;
            int farmerid = Convert.ToInt32(_machinedata.Get(id).farmerid);
            ViewBag.farmername = _farmer.Get(farmerid).fullname;
            ViewBag.payable = (data.netpayable_amt) + (data.advance ?? 0);
            var entry = _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == Bill_fkid).OrderByDescending(x => x.pkid).FirstOrDefault();
            ViewBag.AvailAmt = _cashmastereredd.GetAll().FirstOrDefault().CurrentCash??0;
            if (entry != null && pkid == null)
            {
                ViewBag.pendinamt = entry.pendingAmt;
            }
            else
            {
                var entryss = _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == Bill_fkid).OrderByDescending(x => x.pkid).FirstOrDefault();
                if (entryss == null)
                {
                    ViewBag.pendinamt = data.netpayable_amt;
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
                var model = _purchaseActualPay.Get(_pkid);
                tbl_PurchaseEntry_Master_Paymentss abc = new tbl_PurchaseEntry_Master_Paymentss();
                abc.Purchase_fkid = model.Purchase_fkid; abc.invoice_fkid = model.invoice_fkid; abc.pendingAmt = model.pendingAmt; abc.Currentopeningbalance = model.Currentopeningbalance; abc.datetime = DateTime.Now;
                abc.PaymentMethod = model.PaymentMethod;
                abc.RtgsDetail_fkid = model.RtgsDetail_fkid; abc.RTGSAmtPaying = model.RTGSAmtPaying; abc.ChequeAmtpaying = model.ChequeAmtpaying; abc.chequ_No = model.chequ_No; abc.cheqDetail_fkid = model.cheqDetail_fkid;
                abc.CashPaidTO = model.CashPaidTO; abc.pendingAmt = model.pendingAmt;
                abc.CashAmt = model.CashAmt;
                return View(abc);
            }
        }
        [HttpPost]
        public ActionResult PurchaseDetails_Payment(tbl_PurchaseEntry_Master_Paymentss model)
        {
            ViewBag.AvailAmt = _cashmastereredd.GetAll().FirstOrDefault().CurrentCash ?? 0;
            try
            {
                var previous = _purchaseActualPay.GetAll().Where(x => x.Purchase_fkid == model.Purchase_fkid).OrderByDescending(x => x.pkid).FirstOrDefault();
                if (previous != null)
                {
                    model.Currentopeningbalance = previous.pendingAmt;
                }
                else
                {
                    var da = _purchasePayment.Get(model.invoice_fkid);
                    model.Currentopeningbalance = da.netpayable_amt;
                }
                if (model.pkid == 0)
                {
                    tbl_PurchaseEntry_Master_Payment abc = new tbl_PurchaseEntry_Master_Payment();
                    abc.Purchase_fkid = model.Purchase_fkid; abc.invoice_fkid = model.invoice_fkid; abc.pendingAmt = model.pendingAmt; abc.Currentopeningbalance = model.Currentopeningbalance; abc.datetime = DateTime.Now;
                    abc.PaymentMethod = model.PaymentMethod;
                    abc.RtgsDetail_fkid = model.RtgsDetail_fkid; abc.RTGSAmtPaying = model.RTGSAmtPaying; abc.ChequeAmtpaying = model.ChequeAmtpaying; abc.chequ_No = model.chequ_No; abc.cheqDetail_fkid = model.cheqDetail_fkid;
                    abc.CashPaidTO = model.CashPaidTO; abc.pendingAmt = model.pendingAmt;
                    abc.CashAmt = model.CashAmt;
                    if (model.PaymentMethod == 3)
                    {
                        abc.Status = 1;
                    }
                    if (model.pendingAmt == 0)
                    {
                        var alllist = _purchasePayment.GetAll().Where(x => x.purchasemachine_fkid == model.Purchase_fkid).FirstOrDefault();
                        alllist.PaymentStatus = 1;
                        _purchasePayment.Update(alllist);
                    }
                    _purchaseActualPay.Add(abc);
                    var alldata = _purchasePayment.GetAll().Max(x => x.pkid);
                    if (model.PaymentMethod == 3)
                    {
                        tbl_CashMastered beta = _cashmastereredd.GetAll().FirstOrDefault();
                        beta.CurrentCash = (beta.CurrentCash - model.CashAmt);
                        beta.LastModified = DateTime.Now;
                        _cashmastereredd.Update(beta);
                    }
                    return RedirectToAction("PurchaseDetails_Payment", "PooldataMachine", new { id = model.Purchase_fkid, Bill_fkid = model.invoice_fkid });
                }
                else
                {
                    var abc = _purchaseActualPay.Get(model.pkid);
                    abc.Purchase_fkid = model.Purchase_fkid; abc.invoice_fkid = model.invoice_fkid; abc.pendingAmt = model.pendingAmt; abc.Currentopeningbalance = model.Currentopeningbalance; abc.datetime = DateTime.Now;
                    abc.PaymentMethod = model.PaymentMethod;
                    abc.RtgsDetail_fkid = model.RtgsDetail_fkid; abc.RTGSAmtPaying = model.RTGSAmtPaying; abc.ChequeAmtpaying = model.ChequeAmtpaying; abc.chequ_No = model.chequ_No; abc.cheqDetail_fkid = model.cheqDetail_fkid;
                    abc.CashPaidTO = model.CashPaidTO;
                    abc.CashAmt = model.CashAmt;
                    if (model.PaymentMethod == 3)
                    {
                        abc.Status = 1;
                    }
                    if (model.pendingAmt == 0)
                    {
                        var alllist = _purchasePayment.GetAll().Where(x => x.purchasemachine_fkid == model.Purchase_fkid).FirstOrDefault();
                        alllist.PaymentStatus = 1;
                        _purchasePayment.Update(alllist);
                    }
                    var entry = _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == model.invoice_fkid).OrderByDescending(x => x.pkid).FirstOrDefault();
                    if (entry != null)
                    {
                        entry.pendingAmt = model.pendingAmt;
                        _purchaseActualPay.Update(entry);
                    }
                    else
                    {
                        abc.pendingAmt = model.pendingAmt;
                    }
                    _purchaseActualPay.Update(abc);                   
                    return RedirectToAction("PurchaseDetails_Payment", "PooldataMachine", new { id = model.Purchase_fkid, Bill_fkid = model.invoice_fkid });
                }
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/ErrorLog.txt"));
            }
            return RedirectToAction("MainPage", "Home");
        }
        public ActionResult DeletePayment(int pkid)
        {
            var data = _purchaseActualPay.Get(pkid);
            var pid = data.Purchase_fkid;
            var invoid = data.invoice_fkid;
            try
            {
                var beta = _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == invoid).Where(x => x.pkid != pkid).OrderByDescending(x => x.pkid).FirstOrDefault();
                if (beta != null)
                {
                    decimal amt = 0;
                    if (data.PaymentMethod == 1)
                    {
                        amt = (decimal)data.RTGSAmtPaying;
                        beta.pendingAmt = (beta.pendingAmt + amt);
                        beta.Currentopeningbalance = (beta.Currentopeningbalance + amt);
                    }
                    if (data.PaymentMethod == 2)
                    {
                        amt = (decimal)data.ChequeAmtpaying;
                        beta.pendingAmt = (beta.pendingAmt + amt);
                        beta.Currentopeningbalance = (beta.Currentopeningbalance + amt);
                    }
                    if (data.PaymentMethod == 3)
                    {
                        amt = (decimal)data.CashAmt;
                        beta.pendingAmt = (beta.pendingAmt + amt);
                        beta.Currentopeningbalance = (beta.Currentopeningbalance + amt);
                    }
                }
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("/ErrorLog.txt"));
            }
            return RedirectToAction("PurchaseDetails_Payment", "PooldataMachine", new { id = pid, Bill_fkid = invoid });
        }
        public ActionResult DeletepaymentEntry(int id, int Bill_fkid, string pkid)
        {
            try
            {
                int _pkid = Convert.ToInt32(pkid);
                var ddata = _purchaseActualPay.Get(_pkid);
                var dataasd = _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == Bill_fkid).ToList();
                if (dataasd.Count() > 1)
                {
                   // _purchaseActualPay.Remove(_pkid, true);
                    tbl_PurchaseEntry_Master_Payment data = _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == Bill_fkid).OrderByDescending(x => x.pkid).FirstOrDefault();
                    if (data.pkid == _pkid)
                    {
                        decimal actualp = (ddata.RTGSAmtPaying ?? ddata.ChequeAmtpaying ?? ddata.CashAmt ?? 0) + (ddata.pendingAmt ?? 0);
                        _purchaseActualPay.Remove(_pkid, true);
                        tbl_PurchaseEntry_Master_Payment PKdata = _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == Bill_fkid).OrderByDescending(x => x.pkid).FirstOrDefault();
                        PKdata.pendingAmt = actualp;
                        PKdata.Currentopeningbalance = actualp;
                        _purchaseActualPay.Update(PKdata);
                    }
                    else
                    {
                        _purchaseActualPay.Remove(_pkid, true);
                        decimal deltedamt = ddata.RTGSAmtPaying ?? ddata.ChequeAmtpaying ?? ddata.CashAmt ?? 0;
                        if (ddata.pkid > data.pkid)
                        {

                        }
                        else
                        {
                            data.pendingAmt = (data.pendingAmt) + (deltedamt);
                            data.Currentopeningbalance = (data.Currentopeningbalance) + (deltedamt);
                            _purchaseActualPay.Update(data);
                        }
                    }
                    if (ddata.PaymentMethod == 3)
                    {
                        // SyncCashList();                      
                        tbl_CashMastered _cambo = _cashmastereredd.GetAll().FirstOrDefault();
                        _cambo.CurrentCash = (_cambo.CurrentCash + ddata.CashAmt);
                        _cambo.LastModified = DateTime.Now;
                        _cashmastereredd.Update(_cambo);
                    }
                }
                else
                {
                    int _id = dataasd.FirstOrDefault().pkid;
                    _purchaseActualPay.Remove(_id, true);
                    if (ddata.PaymentMethod == 3)
                    {
                        // SyncCashList();                      
                        tbl_CashMastered _cambo = _cashmastereredd.GetAll().FirstOrDefault();
                        _cambo.CurrentCash = (_cambo.CurrentCash + ddata.CashAmt);
                        _cambo.LastModified = DateTime.Now;
                        _cashmastereredd.Update(_cambo);
                    }
                }
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/ErrorLog.txt"));
            }
            return RedirectToAction("PurchaseDetails_Payment", "PooldataMachine", new { id = id, Bill_fkid = Bill_fkid });
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

                var v = (from a in _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == id).ToList()
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
                             adv = _purchasePayment.Get(id).advance,
                             cash = a.CashAmt ?? 0,
                             advance = _purchasePayment.Get(a.invoice_fkid).advance ?? 0,
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
        [HttpGet]
        public ActionResult PaymentList(string va)
        {
            if (va != "")
            {
                TempData["valu"] = va;
            }
            return View();
        }
        [HttpPost]
        public ActionResult PaymentList(sendtoBankreportView model)
        {
            try
            {
                string search = Request.Form.GetValues("pkids").FirstOrDefault();
                List<int> id = JsonConvert.DeserializeObject<List<int>>(search);
                List<sendtoBankreport> lsitExcel = new List<sendtoBankreport>();
                foreach (var data in id)
                {
                    int _data = Convert.ToInt32(data);
                    var datafield = _purchaseActualPay.Get(_data);
                    datafield.sendtobank = 1;
                    _purchaseActualPay.Update(datafield);

                    sendtoBankreport abc = new sendtoBankreport();
                    abc.paymentmode = _payementtype.Get(datafield.PaymentMethod).paymenttypename;
                    try
                    {

                        abc.Bank_name = _RTGSMASTER.Get(datafield.RtgsDetail_fkid).Bank_name ?? "";
                        abc.Branchname = _RTGSMASTER.Get(datafield.RtgsDetail_fkid).Branchname ?? "";
                        abc.AccountHolder = _RTGSMASTER.Get(datafield.RtgsDetail_fkid).AccountHolder ?? "";
                        abc.Accountno = _RTGSMASTER.Get(datafield.RtgsDetail_fkid).Accountno ?? "";
                        abc.Ifsccode = _RTGSMASTER.Get(datafield.RtgsDetail_fkid).Ifsccode ?? "";
                        abc.RTGSamt = datafield.RTGSAmtPaying ?? 0;
                        var meta = _purchasePayment.Get(datafield.invoice_fkid);
                        abc.mobileno = meta.farmermobileno ?? "";
                        abc.emailid = meta.farmermobileno ?? "";
                        int farmerid = _machinedata.Get(datafield.Purchase_fkid).farmerid ?? 0;
                        abc.benaddress = _farmer.Get(farmerid).address1??"";

                    }
                    catch
                    {
                        abc.Bank_name = "";
                        abc.Branchname = "";
                        abc.AccountHolder = "";
                        abc.Accountno = "";
                        abc.Ifsccode = "";
                        abc.RTGSamt = 0;
                    }
                   
                    abc.totalamt = datafield.RTGSAmtPaying;
                    lsitExcel.Add(abc);
                }

                DataTable _table = new DataTable();
                DataTable _cheque = new DataTable();
                _table.TableName = "GeneralReport";
                _table.Columns.Add("S.No.", typeof(string));
                _table.Columns.Add("AMOUNT", typeof(string));
                _table.Columns.Add("SENDER", typeof(string));
                _table.Columns.Add("ACCOUNT NO", typeof(string));
                _table.Columns.Add("BENEF ACCOUNT NO", typeof(string));
                _table.Columns.Add("BEN NAME", typeof(string));
                _table.Columns.Add("BEN ADDRESS", typeof(string));
                _table.Columns.Add("IFSC CODE", typeof(string));
                _table.Columns.Add("MOBILE NO", typeof(string));
                _table.Columns.Add("EMAIL ID", typeof(string));

           
                long srno = 1;
                foreach (var item in lsitExcel)
                {
                    if (item.AccountHolder != "")
                    {
                        _table.Rows.Add(srno, item.RTGSamt.ToString(), "KISAN GINNING AND PRESSING", "60190562666", item.Accountno.ToString(), item.AccountHolder.ToString(),
                            item.benaddress.ToString(), item.Ifsccode.ToString(), item.mobileno ??"",
                            item.emailid.ToString());
                        srno++;
                    }
                }
               
                GridView gv = new GridView();

                gv.DataSource = _table;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=RTGSBankList.xls");
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
            return RedirectToAction("BankModule", "PooldataMachine");
        }
        public ActionResult PaymentReadyList(string m)
        {
            try
            {
                m = TempData["valu"].ToString();
            }
            catch { }
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
                var v = (from a in _purchaseActualPay.GetAll().Where(x => x.PaymentMethod == 1 && x.sendtobank != 1 || x.PaymentMethod == 2 && x.sendtobank != 1).ToList()

                         select new
                         {
                             pkid = a.pkid,
                             pid = a.PaymentMethod,
                             paymenttype = _payementtype.Get(a.PaymentMethod).paymenttypename,
                             paidto = a.CashPaidTO,
                             rtgsid = getrtgsdetails(a.RtgsDetail_fkid ?? 0),
                             cheqid = getcheqdetails(a.cheqDetail_fkid ?? 0),
                             farmername = _farmer.Get(_machinedata.Get(a.Purchase_fkid).farmerid).fullname,
                             details = a.DetailsTransaction,
                             paymentdate = a.datetime,
                             clearingdate = a.ClearingDate,
                             amountpaid = a.RTGSAmtPaying ?? a.ChequeAmtpaying ?? a.CashAmt ?? 0,
                             dealer =(_purchasePayment.Get(a.invoice_fkid).Dealer_fkid != null ? _dealer.Get(_purchasePayment.Get(a.invoice_fkid).Dealer_fkid).fullname :""),
                         });
                if (m != "all")
                {
                    if (!string.IsNullOrEmpty(m) && !string.IsNullOrWhiteSpace(m))
                    {
                        v = (from b in v.Where(x => x.paymenttype.Trim() == m.Trim()) select b);
                    }
                }
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.paymenttype.ToLower().Contains(search.ToLower()) || x.farmername.ToLower().Contains(search.ToLower())) select b);
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
        public string getrtgsdetails(int id)
        {
            string name = "";
            if (id != 0)
            {
                name = _RTGSMASTER.Get(id).AccountHolder;
            }
            return name;
        }
        public string getcheqdetails(int id)
        {
            string name = "";
            if (id != 0)
            {
                name = _RTGSMASTER.Get(id).Cheq_Partyname;
            }
            return name;
        }
        [HttpGet]
        public ActionResult BankModule(string va)
        {
            if (va != "")
            {
                TempData["valu"] = va;
            }
            return View();
        }
        public ActionResult bankmoduleList(string m)
        {
            try
            {
                m = TempData["valu"].ToString().Trim();
            }
            catch { }
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
                var v = (from a in _purchaseActualPay.GetAll().Where(x => x.sendtobank == 1 && x.Status != 1 && x.returnFlag == null).ToList()

                         select new
                         {
                             pkid = a.pkid,
                             paymenttype = _payementtype.Get(a.PaymentMethod).paymenttypename,
                             paidto = a.CashPaidTO,
                             farmername = _farmer.Get(_machinedata.Get(a.Purchase_fkid).farmerid).fullname,
                             details = a.DetailsTransaction,
                             paymentdate = a.datetime,
                             clearingdate = a.ClearingDate,
                             amountpaid = a.RTGSAmtPaying ?? a.ChequeAmtpaying ?? a.CashAmt ?? 0
                         });
                if (m != "all")
                {
                    if (!string.IsNullOrEmpty(m) && !string.IsNullOrWhiteSpace(m))
                    {
                        v = (from b in v.Where(x => x.paymenttype == m) select b);
                    }
                }
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.paymenttype.ToLower().Contains(search.ToLower()) || x.farmername.ToLower().Contains(search.ToLower())) select b);
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
        public ActionResult ReturnBankModule(string va)
        {
            if (va != "")
            {
                TempData["valu"] = va;
            }
            return View();
        }
        public ActionResult returnbankmoduleList(string m)
        {
            try
            {
                m = TempData["valu"].ToString().Trim();
            }
            catch { }
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
                var v = (from a in _purchaseActualPay.GetAll().Where(x => x.sendtobank == 1 && x.Status != 1 && x.returnFlag == 0).ToList()

                         select new
                         {
                             pkid = a.pkid,
                             paymenttype = _payementtype.Get(a.PaymentMethod).paymenttypename,
                             paidto = a.CashPaidTO,
                             farmername = _farmer.Get(_machinedata.Get(a.Purchase_fkid).farmerid).fullname,
                             details = a.DetailsTransaction,
                             paymentdate = a.datetime,
                             clearingdate = a.ClearingDate,
                             amountpaid = a.RTGSAmtPaying ?? a.ChequeAmtpaying ?? a.CashAmt ?? 0
                         });
                if (m != "all")
                {
                    if (!string.IsNullOrEmpty(m) && !string.IsNullOrWhiteSpace(m))
                    {
                        v = (from b in v.Where(x => x.paymenttype == m) select b);
                    }
                }
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.paymenttype.ToLower().Contains(search.ToLower()) || x.farmername.ToLower().Contains(search.ToLower())) select b);
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
        public ActionResult Updatestatus(string[] poopupdatasave)
        {
            try
            {
                var abc = _purchaseActualPay.Get(Convert.ToInt32(poopupdatasave[0]));
                abc.ClearingDate = Convert.ToDateTime(poopupdatasave[1]);
                abc.DetailsTransaction = poopupdatasave[2];
                abc.Status = Convert.ToInt32(poopupdatasave[3]);
                _purchaseActualPay.Update(abc);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
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
                var v = (from a in _purchaseActualPay.GetAll().Where(x => x.Status == 1).ToList()

                         select new
                         {
                             pkid = a.pkid,
                             invoiceno = "0000" + a.invoice_fkid,
                             holdername = (a.PaymentMethod == 1 ? _RTGSMASTER.GetAll().Where(x => x.pkid == a.RtgsDetail_fkid).FirstOrDefault().AccountHolder ?? "" : ""),
                             partyname = (a.PaymentMethod == 2 ? _RTGSMASTER.GetAll().Where(x => x.pkid == a.cheqDetail_fkid).FirstOrDefault().Cheq_Partyname ?? "" : ""),
                             casgpaidto = (a.PaymentMethod == 3 ? a.CashPaidTO ?? "" : ""),
                             paymenttype = _payementtype.Get(a.PaymentMethod).paymenttypename,
                             paidto = a.CashPaidTO,
                             farmername = _farmer.Get(_machinedata.Get(a.Purchase_fkid).farmerid).fullname,
                             details = a.DetailsTransaction,
                             paymentdate = a.datetime,
                             clearingdate = a.ClearingDate,
                             amountpaid = a.RTGSAmtPaying ?? a.ChequeAmtpaying ?? a.CashAmt ?? 0
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.paymenttype.ToLower().Contains(search.ToLower()) || x.farmername.ToLower().Contains(search.ToLower())) select b);
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
        public ActionResult UpdateReturn(string[] poopupdatasave)
        {
            try
            {
                var abc = _purchaseActualPay.Get(Convert.ToInt32(poopupdatasave[0]));
                abc.returndate = Convert.ToDateTime(poopupdatasave[1]);
                abc.returnAmt = Convert.ToDecimal(poopupdatasave[2]);
                abc.returnFlag = 1;
                abc.Status = 0;
                _purchaseActualPay.Update(abc);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult ReconciliationList()
        {
            return View();
        }
        public ActionResult reconsiliationList()
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
                var v = (from a in _purchaseActualPay.GetAll().Where(x => x.returnFlag == 1 && x.Status == 0).ToList()

                         select new
                         {
                             pkid = a.pkid,
                             paymenttype = _payementtype.Get(a.PaymentMethod).paymenttypename,
                             paidto = a.CashPaidTO,
                             farmername = _farmer.Get(_machinedata.Get(a.Purchase_fkid).farmerid).fullname,
                             details = a.DetailsTransaction,
                             paymentdate = a.datetime,
                             returndate = a.returndate,
                             returnamount = a.returnAmt ?? 0
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.paymenttype.ToLower().Contains(search.ToLower()) || x.farmername.ToLower().Contains(search.ToLower())) select b);
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
        public ActionResult reconsiliationForm(int pkid)
        {
            try
            {
                var data = _purchaseActualPay.Get(pkid);
                ViewBag.purchasid = data.Purchase_fkid;
                ViewBag.invoiseid = data.invoice_fkid;
                var dataa = _purchasePayment.Get(data.Purchase_fkid);
                ViewBag.paytype = new SelectList(_payementtype.GetAll(), "pkid", "paymenttypename");
                ViewBag.holdername = new SelectList(_RTGSMASTER.GetAll().Where(x => x.purchase_fkid == data.Purchase_fkid && x.AccountHolder != "").ToList(), "pkid", "AccountHolder");
                ViewBag.cheholdername = new SelectList(_RTGSMASTER.GetAll().Where(x => x.purchase_fkid == data.Purchase_fkid && x.Cheq_Partyname != null).ToList(), "pkid", "Cheq_Partyname");
                ViewBag.AvailAmt = _cashmastereredd.GetAll().FirstOrDefault().CurrentCash ?? 0;
                int farmerid = Convert.ToInt32(_machinedata.Get(data.Purchase_fkid).farmerid);
                ViewBag.farmername = _farmer.Get(farmerid).fullname;
                ViewBag.pendinamt = data.RTGSAmtPaying ?? data.ChequeAmtpaying;
                ViewBag.pkid = pkid;
                return View();
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult reconsiliationForm(tbl_PurchaseEntry_Master_Paymentss model)
        {
            try
            {
                int pkid = model.pkid;
                var data = _purchaseActualPay.Get(pkid);
                ViewBag.purchasid = data.Purchase_fkid;
                ViewBag.invoiseid = data.invoice_fkid;
                var dataa = _purchasePayment.Get(data.Purchase_fkid);
                ViewBag.paytype = new SelectList(_payementtype.GetAll(), "pkid", "paymenttypename");
                ViewBag.holdername = new SelectList(_RTGSMASTER.GetAll().Where(x => x.purchase_fkid == data.Purchase_fkid && x.AccountHolder != "").ToList(), "pkid", "AccountHolder");
                ViewBag.cheholdername = new SelectList(_RTGSMASTER.GetAll().Where(x => x.purchase_fkid == data.Purchase_fkid && x.Cheq_Partyname != null).ToList(), "pkid", "Cheq_Partyname");
                int farmerid = Convert.ToInt32(_machinedata.Get(data.Purchase_fkid).farmerid);
                ViewBag.farmername = _farmer.Get(farmerid).fullname;
                ViewBag.pendinamt = data.RTGSAmtPaying ?? data.ChequeAmtpaying;
                ViewBag.pkid = pkid;
                var beta = _purchaseActualPay.Get(model.pkid);
                ViewBag.AvailAmt = _cashmastereredd.GetAll().FirstOrDefault().CurrentCash ?? 0;
                if ((beta.RTGSAmtPaying ?? beta.ChequeAmtpaying ?? beta.CashAmt) == (model.RTGSAmtPaying ?? model.ChequeAmtpaying ?? model.CashAmt))
                {
                    beta.datetime = DateTime.Now;
                    beta.PaymentMethod = model.PaymentMethod;
                    beta.RtgsDetail_fkid = model.RtgsDetail_fkid; beta.RTGSAmtPaying = model.RTGSAmtPaying; beta.ChequeAmtpaying = model.ChequeAmtpaying; beta.chequ_No = model.chequ_No; beta.cheqDetail_fkid = model.cheqDetail_fkid;
                    beta.CashPaidTO = model.CashPaidTO; beta.pendingAmt = model.pendingAmt;
                    beta.CashAmt = model.CashAmt;
                    beta.sendtobank = 0; beta.returnFlag = 0;
                    if (model.PaymentMethod == 3)
                    {
                        beta.Status = 1;
                    }
                    _purchaseActualPay.Update(beta);
                    if (model.PaymentMethod == 3)
                    {
                        tbl_CashMastered qeta = _cashmastereredd.GetAll().FirstOrDefault();
                        qeta.CurrentCash = (qeta.CurrentCash - model.CashAmt);
                        qeta.LastModified = DateTime.Now;
                        _cashmastereredd.Update(qeta);
                    }
                    return RedirectToAction("ReconciliationList", "PooldataMachine");
                }
                else
                {
                    ViewBag.er = "true";
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }
        public ActionResult GetPDF_Purchase(int invoiceid)
        {
            string msg = "";
            string jsonErrorCode = "0";
            ReportDocument rd = new ReportDocument();
            try
            {
                string strReportName = "PurchaseInvoice.rpt";
                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports\\" + strReportName;
                rd.Load(strRptPath);
                var data = (from a in _purchasePayment.GetAll().Where(x => x.pkid == invoiceid).ToList()
                            join b in _machinedata.GetAll() on a.purchasemachine_fkid equals b.pkid
                            join c in _RTGSMASTER.GetAll() on a.purchasemachine_fkid equals c.purchase_fkid
                            where a.purchasemachine_fkid == b.pkid && a.purchasemachine_fkid == c.purchase_fkid
                            select new
                         {
                             pkid = "0000" + a.pkid,
                             farmername = _farmer.Get(b.farmerid).fullname ?? "",
                             villagename = "Nagpur",
                             date = a.currentdatetime ?? default(DateTime),
                             farmermobileno = a.farmermobileno ?? "",
                             drivername = a.drivername ?? "",
                             drivermobileno = a.drivermobileno ?? "",
                             product = "COTTON",
                             quntel = a.qunatity ?? 0,
                             netweghtkg = b.NetWeight ?? 0,
                             rate = a.rate ?? 0,
                             amount = (a.qunatity) * (a.rate) ?? 0,
                             bankcharges = a.bankcharges ?? 0,
                             kata = a.kata ?? 0,
                             mapai = a.mapai ?? 0,
                             cd = a.cd ?? 0,
                             dalali = a.CD_dalali ?? 0,
                             unloading = a.unloading ?? 0,
                             totaldetuctionl = (a.kata ?? 0) + (a.mapai ?? 0) + (a.unloading ?? 0) + (a.bankcharges ?? 0) + (a.cd ?? 0) + (a.CD_dalali ?? 0),
                             billamount = ((a.qunatity) * (a.rate) ?? 0) - ((a.kata ?? 0) + (a.mapai ?? 0) + (a.unloading ?? 0) + (a.bankcharges ?? 0) + (a.cd ?? 0) + (a.CD_dalali ?? 0)),
                             advance = a.advance ?? 0,
                             bhada = a.bhada ?? 0,
                             loadingcharges = a.loadingcharges ?? 0,
                             netpayableamt = a.netpayable_amt ?? 0,
                             Amountdet = ((a.qunatity) * (a.rate) ?? 0) - ((a.kata ?? 0) + (a.mapai ?? 0) + (a.unloading ?? 0) + (a.bankcharges ?? 0) + (a.cd ?? 0) + (a.CD_dalali ?? 0)),
                             bankname = c.Bank_name ?? "",
                             branchname = c.Branchname ?? "",
                             accno = c.Accountno ?? "",
                             ifsccode = c.Ifsccode ?? "",
                             holdername = c.AccountHolder ?? "",
                             cheqpartyname = c.Cheq_Partyname ?? "",
                             Chequeno = (_purchaseActualPay.GetAll().Where(x => x.invoice_fkid == invoiceid).FirstOrDefault() != null ? _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == invoiceid).FirstOrDefault().chequ_No : ""),
                             Cashpaidto = (_purchaseActualPay.GetAll().Where(x => x.invoice_fkid == invoiceid).FirstOrDefault() != null ? _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == invoiceid).FirstOrDefault().CashPaidTO : ""),
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
            return File(stream, "application/pdf", "0000" + invoiceid + ".pdf");
        }
        public ActionResult GetPaymentSlip(int pkid)
        {
            string msg = "";
            string jsonErrorCode = "0";
            ReportDocument rd = new ReportDocument();
            try
            {
                var dataasd = _purchaseActualPay.Get(pkid);
                int invoiceid = (int)dataasd.invoice_fkid;
                string strReportName = "PaySleep.rpt";
                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports\\" + strReportName;
                rd.Load(strRptPath);
                var data = (from a in _purchasePayment.GetAll().Where(x => x.pkid == invoiceid).ToList()
                            join b in _machinedata.GetAll() on a.purchasemachine_fkid equals b.pkid
                            join c in _RTGSMASTER.GetAll() on a.purchasemachine_fkid equals c.purchase_fkid
                            join d in _purchaseActualPay.GetAll() on a.pkid equals d.invoice_fkid
                            where a.purchasemachine_fkid == b.pkid && a.purchasemachine_fkid == c.purchase_fkid && a.pkid == d.invoice_fkid
                            select new
                            {
                                pkid = d.pkid,
                                slipno = "0000" + d.pkid,
                                Invoiceno = "0000" + a.pkid,
                                farmername = _farmer.Get(b.farmerid).fullname ?? "",
                                villagename = "Nagpur",
                                date = a.currentdatetime ?? default(DateTime),
                                mobilefarmer = a.farmermobileno ?? "",
                                drivername = a.drivername ?? "",
                                drivermob = a.drivermobileno ?? "",
                                weight = b.NetWeight ?? 0,
                                rate = a.rate ?? 0,
                                deduction = (a.kata ?? 0 + a.mapai ?? 0 + a.unloading ?? 0 + a.bankcharges ?? 0 + a.cd ?? 0 + a.CD_dalali ?? 0),
                                totalamt = a.netpayable_amt ?? 0,
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
        [HttpGet]
        public ActionResult GetHoleDetails(int id)
        {
            try
            {
                var cdata = _purchaseActualPay.Get(id);
                int fid = Convert.ToInt32(_machinedata.Get(cdata.Purchase_fkid).farmerid);
                ViewBag.farmernm = _farmer.Get(fid).fullname;
                var ds = _machinedata.Get(cdata.Purchase_fkid);
                ViewBag.netweight = (ds.NetWeight / 100);
                TempData["qty"] = ds.NetWeight;
                ViewBag.dealer = new SelectList(_dealer.GetAll(), "pkid", "fullname");

                ViewBag.pending = cdata.Currentopeningbalance;
                ViewBag.invv = "0000" + cdata.invoice_fkid;
                ViewBag.paytm = _payementtype.Get(cdata.PaymentMethod).paymenttypename;

                if (cdata.PaymentMethod == 1)
                {
                    ViewBag.holdername = _RTGSMASTER.Get(cdata.RtgsDetail_fkid).AccountHolder;
                    ViewBag.amt = cdata.RTGSAmtPaying;
                    ViewBag.accno = _RTGSMASTER.Get(cdata.RtgsDetail_fkid).Accountno;
                }
                else if (cdata.PaymentMethod == 2)
                {
                    ViewBag.holdername = _RTGSMASTER.Get(cdata.cheqDetail_fkid).Cheq_Partyname;
                    ViewBag.amt = cdata.ChequeAmtpaying;
                    ViewBag.chek = cdata.chequ_No;
                }
                else
                {
                    ViewBag.holdername = cdata.CashPaidTO;
                    ViewBag.amt = cdata.CashAmt;

                }


                var data = _purchasePayment.GetAll().Where(x => x.pkid == cdata.invoice_fkid).ToList();
                tbl_PurchaseEntry_Masterss abc = new tbl_PurchaseEntry_Masterss();
                abc.pkid = data.FirstOrDefault().pkid;
                abc.drivername = data.FirstOrDefault().drivername; abc.drivermobileno = data.FirstOrDefault().drivermobileno;
                abc.farmermobileno = data.FirstOrDefault().farmermobileno; abc.advance_remark = data.FirstOrDefault().advance_remark; abc.Dealer_fkid = data.FirstOrDefault().Dealer_fkid;
                abc.rate = data.FirstOrDefault().rate; abc.qunatity = data.FirstOrDefault().qunatity; abc.RTGSCharges = data.FirstOrDefault().RTGSCharges; abc.kata = data.FirstOrDefault().kata;
                abc.unloading = data.FirstOrDefault().unloading; abc.bankcharges = data.FirstOrDefault().bankcharges; abc.CD_dalali = data.FirstOrDefault().CD_dalali; abc.advance = data.FirstOrDefault().advance;
                abc.bhada = data.FirstOrDefault().bhada; abc.loadingcharges = data.FirstOrDefault().loadingcharges;
                abc.totaldeduction = data.FirstOrDefault().totaldeduction; abc.netpayable_amt = data.FirstOrDefault().netpayable_amt;
                abc.cd = data.FirstOrDefault().cd; abc.mapai = data.FirstOrDefault().mapai; abc.paymentdays = data.FirstOrDefault().paymentdays;
                return View(abc);
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Path.Combine("~/ErrorLog.txt"));
            }
            return View();
        }
        [HttpGet]
        public ActionResult PurchaseLedger()
        {
            purchaseledger abc = new purchaseledger();
            ViewBag.farmer = new SelectList(_farmer.GetAll().OrderBy(x => x.fullname), "pkid", "fullname");
            ViewBag.dealer = new SelectList(_dealer.GetAll().OrderBy(x => x.fullname), "pkid", "fullname");
            return View();
        }
        public ActionResult ledger(string frid, string drid, string frdt, string todt)
        {
            int fid = (frid != "" ? Convert.ToInt32(frid) : 0);
            int tid = (drid != "" ? Convert.ToInt32(drid) : 0);
            DateTime frrdt = (frdt != "" ? Convert.ToDateTime(frdt) : default(DateTime));
            DateTime toodt = (todt != "" ? Convert.ToDateTime(todt) : default(DateTime));
            List<listpurchaseledger> abc = new List<listpurchaseledger>();
            var v = (from a in _purchasePayment.GetAll()
                     join b in _machinedata.GetAll() on a.purchasemachine_fkid equals b.pkid
                     select new listpurchaseledger
                                      {
                                          pkid = a.pkid,
                                          purchaseid = b.pkid,
                                          invoiceno = "0000" + a.pkid,
                                          fid = b.farmerid ?? 0,
                                          did = a.Dealer_fkid ?? 0,
                                          farmername = (b.farmerid != 0 ? _farmer.GetAll().Where(x => x.pkid == b.farmerid).FirstOrDefault().fullname : ""),
                                          dealername = (a.Dealer_fkid != null ? _dealer.GetAll().Where(x => x.pkid == a.Dealer_fkid).FirstOrDefault().fullname : ""),
                                          datet = a.currentdatetime ?? default(DateTime),
                                          netweight = b.NetWeight ?? 0,
                                          totalpaying = (_purchaseActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).ToList() != null ? _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).Sum(x=>x.RTGSAmtPaying??0+x.ChequeAmtpaying??0+x.CashAmt??0) : 0),
                                          Rate = a.rate ?? 0,
                                          advance = a.advance ?? 0,
                                          totalamt = a.netpayable_amt ?? 0,
                                          balance = (_purchaseActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).Count() > 0 ? _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).OrderByDescending(x => x.pkid).FirstOrDefault().pendingAmt??0 : a.netpayable_amt),
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

            abc = v;
            ViewBag.summ = v.Sum(x => x.balance).Value;
            return View(abc);
        }
        public ActionResult SaveRTGSPayExpenses(string[] poopupdatasave)
        {
            try
            {
                tbl_BankDetailsPerPurchase abc = new tbl_BankDetailsPerPurchase();
                abc.Bank_name = poopupdatasave[0]; abc.Branchname = poopupdatasave[1];
                abc.Accountno = poopupdatasave[2]; abc.AccountHolder = poopupdatasave[3];
                abc.Ifsccode = poopupdatasave[4]; abc.purchase_fkid = Convert.ToInt32(poopupdatasave[5]);
                if (abc.AccountHolder != "")
                {
                    if (abc.Accountno == "" || abc.AccountHolder == "" || abc.Ifsccode == "")
                    {
                        return Json("failed", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        _RTGSMASTER.Add(abc);
                        int mid = _RTGSMASTER.GetAll().Max(x => x.pkid);
                        var name = _RTGSMASTER.Get(mid).AccountHolder;
                        string op = "<option value=" + mid + ">" + name + "</option>";
                        return Json(op, JsonRequestBehavior.AllowGet);
                    }
                }
                abc.Cheq_Partyname = poopupdatasave[6];
                abc.AccountHolder = "";
                if (abc.Cheq_Partyname != "")
                {
                    if (abc.Cheq_Partyname == "")
                    {
                        return Json("failed", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        _RTGSMASTER.Add(abc);
                        int mid = _RTGSMASTER.GetAll().Max(x => x.pkid);
                        var name = _RTGSMASTER.Get(mid).Cheq_Partyname;
                        string op = "<option value=" + mid + ">" + name + "</option>";
                        return Json(op, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult PurchaseOutstanding(int pkid = 0)
        {
            ViewBag.dt = DateTime.Now.ToShortDateString();
            ViewBag.farmer = new SelectList(_farmer.GetAll().OrderBy(x => x.fullname), "pkid", "fullname");
            ViewBag.dealer = new SelectList(_dealer.GetAll().OrderBy(x => x.fullname), "pkid", "fullname");
            ViewBag.paytype = new SelectList(_payementtype.GetAll(), "pkid", "paymenttypename");
            if (pkid == 0)
            {              
                return View();
            }
            else
            {
                tbl_PurchaseOutStandingss abc = new tbl_PurchaseOutStandingss();
                tbl_PurchaseOutStanding model = _PurchaseOutStanding.Get(pkid);
                abc.farid = model.farid;
                abc.drid = model.drid;
                abc.outAmt = model.outAmt;
                if (model.Payment_Method == 1)
                {
                    abc.Accoun_holder = model.Accoun_holder;
                    abc.BankName = model.BankName;
                    abc.BranchName = model.BranchName;
                    abc.Accountno = model.Accountno;
                    abc.IFSC = model.IFSC;
                    abc.RTGSAMT = model.RTGSAMT;
                }
                else if (model.Payment_Method == 2)
                {
                    abc.CheqPartyname = model.CheqPartyname;
                    abc.CHequeno = model.CHequeno;
                    abc.CheqAmt = model.CheqAmt;
                }
                else
                {
                    abc.CashPartyName = model.CashPartyName;
                    abc.CashAmt = model.CashAmt;
                }
                abc.Payment_Method = model.Payment_Method;
                abc.Details = model.Details;
                abc.statusa = model.statusa;
                abc.SystDate = DateTime.Now;
                return View(abc);
            }
            return View();
        }
        [HttpPost]
        public ActionResult PurchaseOutstanding(tbl_PurchaseOutStandingss model)
        {
            try
            {
                if (model.pkid == 0)
                {
                    if (model.outAmt == null || model.outAmt == 0)
                    {
                        ViewBag.dt = DateTime.Now.ToShortDateString();
                        ViewBag.farmer = new SelectList(_farmer.GetAll().OrderBy(x => x.fullname), "pkid", "fullname");
                        ViewBag.dealer = new SelectList(_dealer.GetAll().OrderBy(x => x.fullname), "pkid", "fullname");
                        ViewBag.paytype = new SelectList(_payementtype.GetAll(), "pkid", "paymenttypename");
                        ViewBag.problem = "sucess";
                        return View(model);
                    }
                    tbl_PurchaseOutStanding abc = new tbl_PurchaseOutStanding();
                    abc.farid = model.farid;
                    abc.drid = model.drid;
                    abc.outAmt = model.outAmt;                  
                    if (model.Payment_Method == 1)
                    {
                        abc.Accoun_holder = model.Accoun_holder;
                        abc.BankName = model.BankName;
                        abc.BranchName = model.BranchName;
                        abc.Accountno = model.Accountno;
                        abc.IFSC = model.IFSC;
                        abc.RTGSAMT = model.RTGSAMT;  
                    }
                    else if (model.Payment_Method == 2)
                    {
                        abc.CheqPartyname = model.CheqPartyname;
                        abc.CHequeno = model.CHequeno;
                        abc.CheqAmt = model.CheqAmt;
                    }
                    else
                    {
                        abc.CashPartyName = model.CashPartyName;
                        abc.CashAmt = model.CashAmt;
                    }
                    abc.Payment_Method = model.Payment_Method;
                    abc.Details = model.Details;
                    abc.SystDate = DateTime.Now;
                    _PurchaseOutStanding.Add(abc);
                }
                else
                {
                    tbl_PurchaseOutStanding abc = new tbl_PurchaseOutStanding();
                    abc.pkid = model.pkid;
                    abc.farid = model.farid;
                    abc.drid = model.drid;
                    abc.outAmt = model.outAmt;
                    if (model.Payment_Method == 1)
                    {
                        abc.Accoun_holder = model.Accoun_holder;
                        abc.BankName = model.BankName;
                        abc.BranchName = model.BranchName;
                        abc.Accountno = model.Accountno;
                        abc.IFSC = model.IFSC;
                        abc.RTGSAMT = model.RTGSAMT;
                    }
                    else if (model.Payment_Method == 2)
                    {
                        abc.CheqPartyname = model.CheqPartyname;
                        abc.CHequeno = model.CHequeno;
                        abc.CheqAmt = model.CheqAmt;
                    }
                    else
                    {
                        abc.CashPartyName = model.CashPartyName;
                        abc.CashAmt = model.CashAmt;
                    }
                    abc.Payment_Method = model.Payment_Method;
                    abc.Details = model.Details;
                    abc.statusa = model.statusa;
                    abc.SystDate = DateTime.Now;
                    _PurchaseOutStanding.Update(abc);
                }
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Path.Combine("~/ErrorLog.txt"));
            }
            return RedirectToAction("PurchaseOutstandingPayment", "PooldataMachine");
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
                var v = (from a in _PurchaseOutStanding.GetAll().Where(x=>x.statusa != 1).ToList()

                         select new
                         {
                             pkid = a.pkid,
                             farmername = (a.farid != null ? _farmer.Get(a.farid).fullname:""),
                             dealername = (a.drid != null ? _farmer.Get(a.drid).fullname : ""),
                             paymentmode = (a.Payment_Method != null ? _payementtype.Get(a.Payment_Method).paymenttypename : ""),
                             paidto = a.Accoun_holder ?? a.CheqPartyname ?? a.CashPartyName??"",
                             details =a.Details??"",
                             paydate = a.SystDate,
                             amt = a.CheqAmt ?? a.CashAmt ?? a.RTGSAMT??0
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
                var v = (from a in _PurchaseOutStanding.GetAll().Where(x => x.statusa == 1)

                         select new
                         {
                             pkid = a.pkid,
                             farmername = (a.farid != null ? _farmer.Get(a.farid).fullname : ""),
                             dealername = (a.drid != null ? _farmer.Get(a.drid).fullname : ""),
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
        public ActionResult PurOuTCompete()
        {
            return View();
        }
        public ActionResult DeleteOutstanding(int id)
        {
            try
            {
                tbl_PurchaseOutStanding abc = _PurchaseOutStanding.Get(id);
                _PurchaseOutStanding.Remove(abc);
                return RedirectToAction("PurOuTCompete", "PooldataMachine");
            }
            catch
            {
                return RedirectToAction("PurOuTCompete", "PooldataMachine");
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
                   TempData["fid"] = abc.farid.ToString();
                   TempData["did"] = abc.drid.ToString();
                   TempData["PayAmt"] = abc.CashAmt ?? abc.CheqAmt ?? abc.RTGSAMT;
                   string data = GetInvoiceList();
                   decimal returnamt =Convert.ToDecimal(data);
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
                               join b in _purchasePayment.GetAll().Where(x => x.PaymentStatus != 1) on a.pkid equals b.purchasemachine_fkid
                               where a.pkid == b.purchasemachine_fkid
                               select new
                               {
                                   invoiceid = b.pkid,
                                   netpay = b.netpayable_amt ?? 0,
                                   paysta = b.PaymentStatus??0,
                               }).ToList();
                foreach (var item in Invoice)
                {
                    var payment = _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == item.invoiceid).ToList();
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
                                    _purchaseActualPay.Update(beta);
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
                                _purchaseActualPay.Update(deta);
                                var inda = _purchasePayment.Get(deta.invoice_fkid);
                                inda.PaymentStatus = 1;
                                _purchasePayment.Update(inda);
                            }
                        }
                        else { 
                          var againreturnPayment = payment.Where(x => x.Status == 0).ToList();
                          if (againreturnPayment.Count() != 0)
                          {
                              foreach (var beta in againreturnPayment)
                              {
                                  var inda = _purchasePayment.Get(beta.invoice_fkid);
                                  inda.PaymentStatus = 1;
                                  _purchasePayment.Update(inda);
                              }
                          }
                        }
                    }
                    else
                    {
                        var ceta = _purchasePayment.Get(item.invoiceid);
                        if (ceta.netpayable_amt <= Payamt)
                        {
                            ceta.PaymentStatus = 1;
                            Payamt = (Payamt) - ((decimal)ceta.netpayable_amt);
                            _purchasePayment.Update(ceta);
                        }
                    }
                }
            }
            else if (Did != 0 && Fid == 0)
            {
                var Invoice = (from a in _machinedata.GetAll().ToList()
                               join b in _purchasePayment.GetAll().Where(x => x.PaymentStatus != 1 && x.Dealer_fkid == Did) on a.pkid equals b.purchasemachine_fkid
                               where a.pkid == b.purchasemachine_fkid
                               select new
                               {
                                   invoiceid = b.pkid,
                                   netpay = b.netpayable_amt ?? 0,
                                   paysta = b.PaymentStatus ?? 0,
                               }).ToList();
                foreach (var item in Invoice)
                {
                    var payment = _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == item.invoiceid).ToList();
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
                                    _purchaseActualPay.Update(beta);
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
                                _purchaseActualPay.Update(deta);
                                var inda = _purchasePayment.Get(deta.invoice_fkid);
                                inda.PaymentStatus = 1;
                                _purchasePayment.Update(inda);
                            }
                        }
                        else
                        {
                            var againreturnPayment = payment.Where(x => x.Status == 0).ToList();
                            if (againreturnPayment.Count() != 0)
                            {
                                foreach (var beta in againreturnPayment)
                                {
                                    var inda = _purchasePayment.Get(beta.invoice_fkid);
                                    inda.PaymentStatus = 1;
                                    _purchasePayment.Update(inda);
                                }
                            }
                        }
                       
                    }
                    else
                    {
                        var ceta = _purchasePayment.Get(item.invoiceid);
                        if (ceta.netpayable_amt <= Payamt)
                        {
                            ceta.PaymentStatus = 1;
                            Payamt = (Payamt) - ((decimal)ceta.netpayable_amt);
                            _purchasePayment.Update(ceta);
                        }
                    }
                }
            }
            else
            {
                var Invoice = (from a in _machinedata.GetAll().Where(x => x.farmerid == Fid).ToList()
                               join b in _purchasePayment.GetAll().Where(x => x.PaymentStatus != 1 && x.Dealer_fkid == Did) on a.pkid equals b.purchasemachine_fkid
                               where a.pkid == b.purchasemachine_fkid
                               select new
                               {
                                   invoiceid = b.pkid,
                                   netpay = b.netpayable_amt ?? 0,
                                   paysta = b.PaymentStatus ?? 0,
                               }).ToList();
                foreach (var item in Invoice)
                {
                    var payment = _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == item.invoiceid).ToList();
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
                                    _purchaseActualPay.Update(beta);
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
                                _purchaseActualPay.Update(deta);
                                var inda = _purchasePayment.Get(deta.invoice_fkid);
                                inda.PaymentStatus = 1;
                                _purchasePayment.Update(inda);
                            }
                        }
                        else
                        {
                            var againreturnPayment = payment.Where(x => x.Status == 0).ToList();
                            if (againreturnPayment.Count() != 0)
                            {
                                foreach (var beta in againreturnPayment)
                                {
                                    var inda = _purchasePayment.Get(beta.invoice_fkid);
                                    inda.PaymentStatus = 1;
                                    _purchasePayment.Update(inda);
                                }
                            }
                        }
                    }
                    else
                    {
                        var ceta = _purchasePayment.Get(item.invoiceid);
                        if (ceta.netpayable_amt <= Payamt)
                        {
                            ceta.PaymentStatus = 1;
                            Payamt = (Payamt) - ((decimal)ceta.netpayable_amt);
                            _purchasePayment.Update(ceta);
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
                               join b in _purchasePayment.GetAll().Where(x => x.PaymentStatus != 1) on a.pkid equals b.purchasemachine_fkid
                               where a.pkid == b.purchasemachine_fkid
                               select new
                               {
                                   invoiceid = b.pkid,
                                   netpay = b.netpayable_amt ?? 0,
                               }).ToList();
                foreach (var item in Invoice)
                {
                    var payment = _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == item.invoiceid).ToList();
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
                               join b in _purchasePayment.GetAll().Where(x => x.PaymentStatus != 1 && x.Dealer_fkid == Did) on a.pkid equals b.purchasemachine_fkid
                               where a.pkid == b.purchasemachine_fkid
                               select new
                               {
                                   invoiceid = b.pkid,
                                   netpay = b.netpayable_amt ?? 0,
                               }).ToList();
                foreach (var item in Invoice)
                {
                    var payment = _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == item.invoiceid).ToList();
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
                               join b in _purchasePayment.GetAll().Where(x => x.PaymentStatus != 1 && x.Dealer_fkid == Did) on a.pkid equals b.purchasemachine_fkid
                               where a.pkid == b.purchasemachine_fkid
                               select new
                               {
                                   invoiceid = b.pkid,
                                   netpay = b.netpayable_amt ?? 0,
                               }).ToList();
                foreach (var item in Invoice)
                {
                    var payment = _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == item.invoiceid).ToList();
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
        public ActionResult CompletedListR()
        {
            return View();
        }
        public ActionResult CompletedPaymentListR()
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
                var v = (from a in _purchaseActualPay.GetAll().Where(x => x.Status == 1).GroupBy(x=>x.invoice_fkid).ToList()

                         select new
                         {
                             pkid = a.FirstOrDefault().pkid,
                             invoiceid = a.FirstOrDefault().invoice_fkid,
                             invoiceno = "0000" + a.FirstOrDefault().invoice_fkid,
                             partyname = (a.FirstOrDefault().PaymentMethod == 2 ? _RTGSMASTER.GetAll().Where(x => x.pkid == a.FirstOrDefault().cheqDetail_fkid).FirstOrDefault().Cheq_Partyname ?? "" : ""),
                             paymenttype = _payementtype.Get(a.FirstOrDefault().PaymentMethod).paymenttypename,
                             farmername = _farmer.Get(_machinedata.Get(a.FirstOrDefault().Purchase_fkid).farmerid).fullname,
                             paymentdate = a.FirstOrDefault().ClearingDate,

                             totalamt = (_purchasePayment.Get(a.FirstOrDefault().invoice_fkid).netpayable_amt ?? 0),
                             pending = a.FirstOrDefault().pendingAmt ?? 0
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.invoiceno.Contains(search) || x.farmername.Contains(search) || x.invoiceno.Contains(search)) select b);
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
        public ActionResult Summary(string id)
        {
            ViewBag.id = id;
            return View();
        }
        public ActionResult CompletedPaymentListinvoice(string id)
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
                int _id = Convert.ToInt32(id);
                var v = (from a in _purchaseActualPay.GetAll().Where(x => x.Status == 1 && x.invoice_fkid == _id).ToList()

                         select new
                         {
                             pkid = a.pkid,
                             invoiceno = "0000" + a.invoice_fkid,
                             holdername = (a.PaymentMethod == 1 ? _RTGSMASTER.GetAll().Where(x => x.pkid == a.RtgsDetail_fkid).FirstOrDefault().AccountHolder ?? "" : ""),
                             partyname = (a.PaymentMethod == 2 ? _RTGSMASTER.GetAll().Where(x => x.pkid == a.cheqDetail_fkid).FirstOrDefault().Cheq_Partyname ?? "" : ""),
                             casgpaidto = (a.PaymentMethod == 3 ? a.CashPaidTO ?? "" : ""),
                             paymenttype = _payementtype.Get(a.PaymentMethod).paymenttypename,
                             paidto = a.CashPaidTO,
                             farmername = _farmer.Get(_machinedata.Get(a.Purchase_fkid).farmerid).fullname,
                             details = a.DetailsTransaction,
                             paymentdate = a.datetime,
                             clearingdate = a.ClearingDate,
                             amountpaid = a.RTGSAmtPaying ?? a.ChequeAmtpaying ?? a.CashAmt ?? 0
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.paymenttype.ToLower().Contains(search.ToLower()) || x.farmername.ToLower().Contains(search.ToLower())) select b);
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
        [HttpPost]
        public ActionResult PurchaseLedger(purchaseledger model)
        {
            try
            {
                int fid = (model.farid != null ? Convert.ToInt32(model.farid) : 0);
                int tid = (model.drid != null ? Convert.ToInt32(model.drid) : 0);
                DateTime frrdt = (model.frdt != null ? Convert.ToDateTime(model.frdt) : default(DateTime));
                DateTime toodt = (model.todt != null ? Convert.ToDateTime(model.todt) : default(DateTime));
                List<listpurchaseledger> lsitExcel = new List<listpurchaseledger>();

                var v = (from a in _purchasePayment.GetAll()
                         join b in _machinedata.GetAll() on a.purchasemachine_fkid equals b.pkid
                         select new listpurchaseledger
                         {
                             pkid = a.pkid,
                             purchaseid = b.pkid,
                             invoiceno = "0000" + a.pkid,
                             fid = b.farmerid ?? 0,
                             did = a.Dealer_fkid ?? 0,
                             farmername = (b.farmerid != 0 ? _farmer.GetAll().Where(x => x.pkid == b.farmerid).FirstOrDefault().fullname : ""),
                             dealername = (a.Dealer_fkid != null ? _dealer.GetAll().Where(x => x.pkid == a.Dealer_fkid).FirstOrDefault().fullname : ""),
                             datet = a.currentdatetime ?? default(DateTime),
                             netweight = b.NetWeight ?? 0,
                             totalpaying = (_purchaseActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).ToList() != null ? _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).Sum(x => x.RTGSAmtPaying ?? 0 + x.ChequeAmtpaying ?? 0 + x.CashAmt ?? 0) : 0),
                             Rate = a.rate ?? 0,
                             advance = a.advance ?? 0,
                             totalamt = a.netpayable_amt ?? 0,
                             balance = (_purchaseActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).Count() > 0 ? _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).OrderByDescending(x => x.pkid).FirstOrDefault().pendingAmt ?? 0 : a.netpayable_amt),
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
                _table.TableName = "PurchaseLedger";
                _table.Columns.Add("S.No.", typeof(string));
                _table.Columns.Add("INVOICE NUMBER", typeof(string));
                _table.Columns.Add("DATE", typeof(string));
                _table.Columns.Add("FARMER NAME", typeof(string));
                _table.Columns.Add("DEALER NAME", typeof(string));
                _table.Columns.Add("NET WEIGHT", typeof(string));
                _table.Columns.Add("RATE", typeof(string));
                _table.Columns.Add("TOTAL AMOUNT", typeof(string));
                _table.Columns.Add("ADVANCE", typeof(string));
                _table.Columns.Add("TOTAL DEBIT", typeof(string));
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
                Response.AddHeader("content-disposition", "attachment; filename=PurchaseLegder.xls");
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

        public ActionResult OutStandingPaymentList(string frid,string drid)
        {
            int fid = (frid != "" ? Convert.ToInt32(frid) : 0);
            int tid = (drid != "" ? Convert.ToInt32(drid) : 0);
            List<OutStandingPaymentList> abc = new List<OutStandingPaymentList>();
            var v = (from a in _purchasePayment.GetAll()
                     join b in _machinedata.GetAll() on a.purchasemachine_fkid equals b.pkid
                     select new OutStandingPaymentList
                     {
                         pkid = a.pkid,                       
                         invoiceno = "0000" + a.pkid,                      
                         farmername = (b.farmerid != 0 ? _farmer.GetAll().Where(x => x.pkid == b.farmerid).FirstOrDefault().fullname : ""),
                         dealername = (a.Dealer_fkid != null ? _dealer.GetAll().Where(x => x.pkid == a.Dealer_fkid).FirstOrDefault().fullname : ""),
                         fid = b.farmerid ?? 0,
                         did = a.Dealer_fkid ?? 0,
                         paymentdate = a.currentdatetime ?? default(DateTime),                    
                         creditedamount = (_purchaseActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).ToList() != null ? _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).Sum(x => x.RTGSAmtPaying ?? 0 + x.ChequeAmtpaying ?? 0 + x.CashAmt ?? 0) : 0),                     
                         totalamount = a.netpayable_amt ?? 0,
                         BalalnceAmount = (_purchaseActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).Count() > 0 ? _purchaseActualPay.GetAll().Where(x => x.invoice_fkid == a.pkid).OrderByDescending(x => x.pkid).FirstOrDefault().pendingAmt ?? 0 : a.netpayable_amt),
                     }).ToList();
            if (fid != 0)
            {
                v = v.Where(x => x.fid == fid).ToList();
            }
            if (tid != 0)
            {
                v = v.Where(x => x.did == tid).ToList();
            }
          
            abc = v;
            ViewBag.summ = v.Sum(x => x.BalalnceAmount).Value;
            return View(abc);
        }

    }
}