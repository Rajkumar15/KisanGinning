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
    
    public partial class tbl_PurchseStockMaster
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
}
