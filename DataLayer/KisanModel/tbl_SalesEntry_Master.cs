//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer.KisanModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_SalesEntry_Master
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
}
