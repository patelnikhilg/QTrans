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
    using System.ComponentModel;

    public  class Company
    {
        public Company()
        {
        }
    
        public long companyid { get; set; }
        [DefaultValue("NA")]
        public string companyname { get; set; } = "DEMO";
        [DefaultValue("NA")]
        public string address { get; set; } = "NA";
        [DefaultValue("NA")]
        public string telenumber { get; set; } = "NA";
        public string alternettelnumber { get; set; } = "NA";
        [DefaultValue("NA")]
        public string country { get; set; } = "NA";
        [DefaultValue("NA")]
        public string state { get; set; } = "NA";
        [DefaultValue("NA")]
        public string city { get; set; } = "NA";
        public long userid { get; set; }
        [DefaultValue("NA")]
        public int comanytype { get; set; } = 0;
        public System.DateTime createddate { get; set; }
        public Nullable<System.DateTime> modifydate { get; set; }
    }
}
