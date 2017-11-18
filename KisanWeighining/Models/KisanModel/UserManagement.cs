using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KisanWeighining.Models.KisanModel
{
    public partial class tbl_UserDetailss
    {
        public int pkid { get; set; }
        public string User_fkid { get; set; }
        public string firstname { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string mobileno { get; set; }
        public string emailid { get; set; }
        public Nullable<int> stateid { get; set; }
        public Nullable<int> cityid { get; set; }
        public Nullable<int> department { get; set; }
    }
    public partial class tbl_FarmerMasterss
    {
        public int pkid { get; set; }
        public string fullname { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string mobileno { get; set; }
        public string villagename { get; set; }
    }
    public partial class tbl_productMasterss
    {
        public int pkid { get; set; }
        [Required]
        public string productname { get; set; }
        public string productdescription { get; set; }
        public Nullable<int> producttype { get; set; }
    }
    public partial class tbl_truckMasterss
    {
        public int pkid { get; set; }
        public string RFIDnumber { get; set; }
        public string truckno { get; set; }
        public Nullable<decimal> tairweight { get; set; }
        public Nullable<decimal> capcity { get; set; }
        public string trucktype { get; set; }
        public Nullable<int> salespurchase { get; set; }
    }
    public partial class tbl_DealerMasterss
    {
        public int pkid { get; set; }
        public string fullname { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string mobileno { get; set; }
        [EmailAddress]
        public string emailid { get; set; }
    }
    public partial class tbl_BrokerMasterss
    {
        public int pkid { get; set; }
        public string Brokername { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string mobile { get; set; }
        public string emailid { get; set; }
    }
    public partial class tbl_TransportMasterss
    {
        public int pkid { get; set; }
        public string transportname { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string mobileno { get; set; }
        public string emailid { get; set; }
        public string ownername { get; set; }
        public string ownermobileno { get; set; }
    }
    public partial class tbl_pool_MachineData_Purchasess
    {
        public int pkid { get; set; }
        public string TicketNo { get; set; }
        public Nullable<System.DateTime> ticketdate { get; set; }
        public Nullable<int> farmerid { get; set; }
        public string truckRFID { get; set; }
        public Nullable<System.DateTime> Intime { get; set; }
        public Nullable<System.DateTime> outtime { get; set; }
        public Nullable<int> producttypeid { get; set; }
        public Nullable<decimal> Grossweight { get; set; }
        public Nullable<decimal> TairWeight { get; set; }
        public Nullable<decimal> NetWeight { get; set; }
        public Nullable<long> machine_fkid { get; set; }
        public string transportname { get; set; }
        public string challanno { get; set; }
        public Nullable<decimal> challanweight { get; set; }
        public string field1 { get; set; }
        public string field2 { get; set; }
        public string field3 { get; set; }
        public string unit { get; set; }
        public Nullable<decimal> weighningcharges { get; set; }
        public Nullable<int> invoicestatus { get; set; }
    }
    public partial class tbl_PurchaseEntry_Masterss
    {
        public int pkid { get; set; }
        public Nullable<int> purchasemachine_fkid { get; set; }
        public string drivername { get; set; }
        public string farmermobileno { get; set; }
        public string drivermobileno { get; set; }
        public Nullable<decimal> rate { get; set; }
        public Nullable<decimal> qunatity { get; set; }
        public Nullable<decimal> RTGSCharges { get; set; }
        public Nullable<decimal> kata { get; set; }
        public Nullable<decimal> unloading { get; set; }
        public Nullable<decimal> bankcharges { get; set; }
        public Nullable<decimal> CD_dalali { get; set; }
        public Nullable<decimal> totaldeduction { get; set; }
        public Nullable<decimal> advance { get; set; }
        public Nullable<decimal> bhada { get; set; }
        public Nullable<decimal> loadingcharges { get; set; }
        public Nullable<decimal> netpayable_amt { get; set; }
        public Nullable<decimal> cd { get; set; }
        public Nullable<decimal> mapai { get; set; }
        public string advance_remark { get; set; }
        public Nullable<int> Dealer_fkid { get; set; }
        public Nullable<System.DateTime> currentdatetime { get; set; }
        public Nullable<int> PaymentStatus { get; set; }
        public Nullable<System.DateTime> paymentdays { get; set; }
        public string VillageName { get; set; }
        public Nullable<int> stockread { get; set; }
    }
    public partial class tbl_PurchaseEntry_Master_Paymentss
    {
        public int pkid { get; set; }
        public Nullable<int> Purchase_fkid { get; set; }
        public Nullable<int> invoice_fkid { get; set; }
        public Nullable<int> PaymentMethod { get; set; }
        public Nullable<int> RtgsDetail_fkid { get; set; }
        public Nullable<decimal> RTGSAmtPaying { get; set; }
        public Nullable<decimal> ChequeAmtpaying { get; set; }
        public string chequ_No { get; set; }
        public Nullable<int> cheqDetail_fkid { get; set; }
        public string CashPaidTO { get; set; }
        public Nullable<decimal> CashAmt { get; set; }
        public Nullable<decimal> pendingAmt { get; set; }
        public Nullable<System.DateTime> datetime { get; set; }
        public Nullable<int> returnFlag { get; set; }
        public Nullable<decimal> returnAmt { get; set; }
        public Nullable<System.DateTime> returndate { get; set; }
        public Nullable<decimal> Currentopeningbalance { get; set; }
        public Nullable<System.DateTime> ClearingDate { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> sendtobank { get; set; }
        public string DetailsTransaction { get; set; }
        public Nullable<int> OutstandingStatus { get; set; }
    }
    public partial class tbl_BankDetailsPerPurchasess
    {
        public int pkid { get; set; }
        public Nullable<int> purchase_fkid { get; set; }
        public string Bank_name { get; set; }
        public string Branchname { get; set; }
        public string AccountHolder { get; set; }
        public string Accountno { get; set; }
        public string Ifsccode { get; set; }
        public string Cheq_Partyname { get; set; }
    }
    public partial class sendtoBankreport
    {
        public string paymentmode { get; set; }
        public string Bank_name { get; set; }
        public string Branchname { get; set; }
        public string AccountHolder { get; set; }
        public string Accountno { get; set; }
        public string Ifsccode { get; set; }
        public Nullable<decimal> RTGSamt { get; set; }
        public string benaddress { get; set; }
        public string emailid { get; set; }
        public string mobileno { get; set; }
        public Nullable<decimal> totalamt { get; set; }
    }
    public partial class sendtoBankreportView
    {
        public int[] pkids { get; set; }
    }
    public partial class tbl_SalesEntry_Masterss
    {
        public int pkid { get; set; }
        public Nullable<int> Purchase_fkid { get; set; }
        public Nullable<int> ProductType_fkid { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> partyid { get; set; }
        public string prno { get; set; }
        public string topr { get; set; }
        public string pmark { get; set; }
        public Nullable<System.DateTime> deliveryfromdate { get; set; }
        public string FPbales { get; set; }
        public string Quality { get; set; }
        public Nullable<decimal> lotno { get; set; }
        public Nullable<decimal> noofbales { get; set; }
        public Nullable<decimal> grossweight { get; set; }
        public Nullable<decimal> tareweight { get; set; }
        public Nullable<decimal> qtl { get; set; }
        public Nullable<decimal> kilogram { get; set; }
        public Nullable<decimal> netweight { get; set; }
        public Nullable<decimal> amount { get; set; }
        public Nullable<decimal> ratepercandy { get; set; }
        public Nullable<decimal> rateperqtl { get; set; }
        public string deliveryspotmill { get; set; }
        public string amtinword { get; set; }
        public Nullable<decimal> frightothercharges { get; set; }
        public Nullable<decimal> totalamt { get; set; }
        public Nullable<decimal> totalamount { get; set; }
        public Nullable<decimal> SGST { get; set; }
        public Nullable<decimal> CGST { get; set; }
        public Nullable<decimal> IGST { get; set; }
        public Nullable<decimal> advance { get; set; }
        public Nullable<decimal> against { get; set; }
        public Nullable<decimal> NetPayableAmount { get; set; }
        public Nullable<int> brokerid { get; set; }
        public Nullable<System.DateTime> saudadate { get; set; }
        public Nullable<int> paycondition { get; set; }
        public Nullable<int> truckid { get; set; }
        public Nullable<int> transport_fkid { get; set; }
        public string drivername { get; set; }
        public string diversighn { get; set; }
        public Nullable<decimal> packagingcgaeges { get; set; }
        public Nullable<decimal> CashDiscount { get; set; }
        public Nullable<decimal> cashDiscountAmt { get; set; }
        public string InsuranceNo { get; set; }
        public Nullable<int> PaymentStatus { get; set; }
    }
    public partial class tbl_SalesEntry_Master_Paymentss
    {
        public int pkid { get; set; }
        public Nullable<int> Purchase_fkid { get; set; }
        public Nullable<int> invoice_fkid { get; set; }
        public Nullable<int> PaymentMethod { get; set; }
        public Nullable<int> RtgsDetail_fkid { get; set; }
        public Nullable<decimal> RTGSAmtPaying { get; set; }
        public Nullable<decimal> ChequeAmtpaying { get; set; }
        public string chequ_No { get; set; }
        public Nullable<int> cheqDetail_fkid { get; set; }
        public string CashPaidTO { get; set; }
        public Nullable<decimal> CashAmt { get; set; }
        public Nullable<decimal> pendingAmt { get; set; }
        public Nullable<System.DateTime> datetime { get; set; }
        public Nullable<int> returnFlag { get; set; }
        public Nullable<decimal> returnAmt { get; set; }
        public Nullable<System.DateTime> returndate { get; set; }
        public Nullable<decimal> Currentopeningbalance { get; set; }
        public Nullable<System.DateTime> ClearingDate { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> sendtobank { get; set; }
        public string DetailsTransaction { get; set; }
        public Nullable<int> OutstandingStatus { get; set; }
    }
    public partial class tbl_salesPartyRegistartionss
    {
        public int pkid { get; set; }
        public string partyname { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string mobileno { get; set; }
        public string emailid { get; set; }
        public string GSTNo { get; set; }
        public Nullable<System.DateTime> Partyregdate { get; set; }
        public string city { get; set; }
        public Nullable<int> Sale_fkid { get; set; }
    }
    public partial class Paymentcalss
    {
        public int pkid { get; set; }
        public string holdername { get; set; }
        public string partyname { get; set; }
        public string casgpaidto { get; set; }
        public string paytype { get; set; }
        public Nullable<decimal> Netamt { get; set; }
        public Nullable<decimal> pendingamt { get; set; }
        public Nullable<decimal> rtgs { get; set; }
        public Nullable<decimal> cheq { get; set; }
        public Nullable<decimal> cash { get; set; }
        public Nullable<decimal> advance { get; set; }
        public int purchasid { get; set; }
        public int invoiceid { get; set; }
        public DateTime datet { get; set; }
    }
    public partial class tbl_TaxMasterss
    {
        public int pkid { get; set; }
        public string Saletype { get; set; }
        public Nullable<decimal> taxper { get; set; }
        public Nullable<System.DateTime> datetime { get; set; }
        public Nullable<decimal> CGST { get; set; }
        public Nullable<decimal> SGST { get; set; }
        public Nullable<decimal> IGST { get; set; }
    }
    public partial class tbl_ProductTaxEntryss
    {
        public int pkid { get; set; }
        public Nullable<int> Product_fkid { get; set; }
        public string HSNno { get; set; }
        public Nullable<int> tax_fkid { get; set; }
    }
    public partial class listpurchaseledger
    {
        public int pkid { get; set; }
        public Nullable<int> purchaseid { get; set; }
        public string invoiceno { get; set; }
        public Nullable<int> fid { get; set; }
        public Nullable<int> did { get; set; }
        public string farmername { get; set; }
        public string dealername { get; set; }
        public Nullable<System.DateTime> datet { get; set; }
        public Nullable<decimal> netweight { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> advance { get; set; }
        public Nullable<decimal> totalamt { get; set; }
        public Nullable<decimal> balance { get; set; }
        public Nullable<decimal> totalpaying { get; set; }
    }
    public partial class purchaseledger
    {

        public Nullable<int> farid { get; set; }
        public Nullable<int> drid { get; set; }
        public Nullable<System.DateTime> frdt { get; set; }
        public Nullable<System.DateTime> todt { get; set; }
    }
 
    public partial class tbl_SalesSockMasterss
    {
        public int pkid { get; set; }
        public Nullable<int> stock_fkid { get; set; }
        public Nullable<decimal> produceCottonbells { get; set; }
        public Nullable<decimal> produceCottonSeed { get; set; }
        public Nullable<decimal> totalStock { get; set; }
        public Nullable<decimal> produceTotalStock { get; set; }
        public Nullable<decimal> balancestock { get; set; }
        public Nullable<System.DateTime> lastmodifieddate { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<int> cid { get; set; }
        public string mdata { get; set; }
    }
    public partial class tbl_PurchseStockMasterss
    {
        public int pkid { get; set; }
        public Nullable<int> purchase_fkid { get; set; }
        public Nullable<int> product_fkid { get; set; }
        public Nullable<decimal> previousStock { get; set; }
        public Nullable<decimal> ReceivedStock { get; set; }
        public Nullable<decimal> TotalStock { get; set; }
        public Nullable<System.DateTime> Inpudate { get; set; }
        public Nullable<System.DateTime> Machineentrydate { get; set; }
        public Nullable<System.DateTime> Lastmodifieddate { get; set; }
        public Nullable<decimal> Balancestock { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<int> cid { get; set; }
        public string mdata { get; set; }
    }
    public partial class Currentstocklist
    {
        public int pkid { get; set; }
        public string productname { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<decimal> openningstock { get; set; }
        public Nullable<decimal> purchasestock { get; set; }
        public Nullable<decimal> proocessBales { get; set; }
        public Nullable<decimal> proccessseed { get; set; }
        public Nullable<decimal> closingstock { get; set; }
    }
    public partial class tbl_ExpensesPurposeMasterss
    {
        public int pkid { get; set; }
        public string purposename { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
    }
    public partial class tbl_ExpensesMasterss
    {
        public int pkid { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<int> purposeid { get; set; }
        public string personname { get; set; }
        public Nullable<int> Payment_type { get; set; }
        public Nullable<decimal> amt { get; set; }
        public Nullable<int> Pay_Infofkid { get; set; }
        public string remark { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
    }
    public partial class tbl_PurchaseOutStandingss
    {
        public int pkid { get; set; }
        public Nullable<int> farid { get; set; }
        public Nullable<int> drid { get; set; }
        public Nullable<decimal> outAmt { get; set; }
        public Nullable<decimal> payingamt { get; set; }
        public string CheqPartyname { get; set; }
        public Nullable<decimal> CheqAmt { get; set; }
        public Nullable<decimal> CashAmt { get; set; }
        public Nullable<decimal> RTGSAMT { get; set; }
        public string CashPartyName { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string Accountno { get; set; }
        public string IFSC { get; set; }
        public Nullable<System.DateTime> SystDate { get; set; }
        public Nullable<int> Payment_Method { get; set; }
        public string CHequeno { get; set; }
        public string Accoun_holder { get; set; }
        public string Details { get; set; }
        public Nullable<int> statusa { get; set; }
        
    }
    public partial class tbl_SalesOutStandingss
    {
        public int pkid { get; set; }
        public Nullable<int> partyid { get; set; }
        public Nullable<int> brokerid { get; set; }
        public Nullable<decimal> outAmt { get; set; }
        public Nullable<decimal> payingamt { get; set; }
        public string CheqPartyname { get; set; }
        public Nullable<decimal> CheqAmt { get; set; }
        public Nullable<decimal> CashAmt { get; set; }
        public Nullable<decimal> RTGSAMT { get; set; }
        public string CashPartyName { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string Accountno { get; set; }
        public string IFSC { get; set; }
        public Nullable<System.DateTime> SystDate { get; set; }
        public Nullable<int> Payment_Method { get; set; }
        public string CHequeno { get; set; }
        public string Accoun_holder { get; set; }
        public string Details { get; set; }
        public Nullable<int> statusa { get; set; }
    }
    public partial class tbl_CashReceiptss
    {
        public int pkid { get; set; }
        public string personname { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<decimal> cashAmt { get; set; }
        public string sourcename { get; set; }
        public Nullable<System.DateTime> currentdate { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<int> payment_fkid { get; set; }
    }
    public partial class tbl_CashCreditMasterss
    {
        public int pkid { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<int> purchaseorsale { get; set; }
        public Nullable<decimal> amount { get; set; }
        public Nullable<int> invoiceidorexpensid { get; set; }
        public Nullable<System.DateTime> currentdatetime { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
        public Nullable<int> payment_fkid { get; set; }
    }
    public partial class OutStandingPaymentList
    {
        public int pkid { get; set; }
        public Nullable<int> fid { get; set; }
        public Nullable<int> did { get; set; }
        public string farmername { get; set; }
        public string dealername { get; set; }
        public string invoiceno { get; set; }
        public Nullable<decimal> totalamount { get; set; }
        public Nullable<decimal> creditedamount { get; set; }
        public Nullable<System.DateTime> paymentdate { get; set; }
        public Nullable<decimal> BalalnceAmount { get; set; }
    }

    public partial class dashboarddata
    {
        public Nullable<decimal> debit { get; set; }
        public Nullable<decimal> credit { get; set; }
        public Nullable<decimal> totalexpenses { get; set; }
        public Nullable<decimal> cotton { get; set; }
    }
}