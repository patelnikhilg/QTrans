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

    public  class UserProfile
    {
        public UserProfile()
        {
            this.areaPreferences =  new HashSet<AreaPreference>();
        }
    
        public long userid { get; set; }
        [DefaultValue("NA")]
        public string emailaddress { get; set; } = "NA";
        [DefaultValue("NA")]
        public string Password { get; set; } = "NA";
        [DefaultValue("NA")]
        public string firstname { get; set; } = "NA";
        [DefaultValue("NA")]
        public string middlename { get; set; } = "NA";
        [DefaultValue("NA")]
        public string lastname { get; set; } = "NA";
        public string mobilenumber { get; set; }
        [DefaultValue("NA")]
        public string landlinenumber { get; set; } = "NA";
        public Nullable<System.DateTime> dob { get; set; } = DateTime.Now;
        [DefaultValue("NA")]
        public string addressline1 { get; set; } = "NA";
        [DefaultValue("NA")]
        public string addressline2 { get; set; } = "NA";
        public int pincode { get; set; } = 0;
        [DefaultValue("NA")]
        public string photo { get; set; } = "NA";
        [DefaultValue("NA")]
        public string country { get; set; } = "NA";
        [DefaultValue("NA")]
        public string state { get; set; } = "NA";
        [DefaultValue("NA")]
        public string district { get; set; } = "NA";
        [DefaultValue("NA")]
        public string city { get; set; } = "NA";
        [DefaultValue("NA")]
        public string area { get; set; } = "NA";

        public bool mobileverification { get; set; } = false;
        public bool emailverification { get; set; } = false;
        [DefaultValue("NA")]
        public string pan { get; set; } = "NA";
        [DefaultValue("NA")]
        public string gst { get; set; } = "NA";
        public Nullable<long> aadhaarno { get; set; } = 0;
        public DateTime createddate { get; set; }
        public Nullable<DateTime> modifydate { get; set; }

        public int OTP { get; set; } = 0;

        public string Token { get; set; }

        public ICollection<AreaPreference> areaPreferences { get; set; }
    }
}
