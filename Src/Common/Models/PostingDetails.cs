//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QTrans.Models
{
    using System;
    using System.Collections.Generic;
    
    public  class PostingDetails
    {
        public PostingDetails()
        {
        }
    
        public PostingProfile postingProfile { get; set; }
        public long dtlpostingid { get; set; }
        public long postingid { get; set; }
        public int materialweight { get; set; }
        public string materialphotos { get; set; }
        public string packingdimension { get; set; }
        public int numberpckets { get; set; }
        public short vehicletype { get; set; }
        public short novehicle { get; set; }
        public Nullable<short> deliveryperiodday { get; set; }
        public System.DateTime pickupdatetime { get; set; }
        public Nullable<decimal> postamount { get; set; }
        public short onpickuppercentage { get; set; }
        public short onunloadingpercentage { get; set; }
        public int creditday { get; set; }
        public Nullable<System.DateTime> contractstartdatetime { get; set; }
        public Nullable<System.DateTime> contractenddatetime { get; set; }
        public System.DateTime ordertype { get; set; }
        public System.DateTime biddingactivatedatetime { get; set; }
        public System.DateTime biddingclosedatetime { get; set; }
        public short poststatus { get; set; }
        public bool gprs { get; set; }
        public bool menpowerloading { get; set; }
        public bool menpowerunloading { get; set; }
        public bool transporterinsurance { get; set; }
        public bool tolltaxinclude { get; set; }
        public string remark { get; set; }
        public bool loadingtype { get; set; }
        public Nullable<System.DateTime> createddate { get; set; }
        public Nullable<System.DateTime> modifydate { get; set; }
    
    }
}
