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
    using System.ComponentModel.DataAnnotations;

    public class BiddingDetails
    {
        public long dtlbiddingid { get; set; }
        [RegularExpression(Constants.RegexStringInput, ErrorMessage = Constants.StringNumeric)]
        [DisplayName("No of Vehicle")]
        public short vehicleno { get; set; }
        [RegularExpression(Constants.RegexStringInput, ErrorMessage = Constants.StringNumeric)]
        [DisplayName("Capacity of Vehicle")]
        public short capacity { get; set; }
        public Nullable<long> biddingid { get; set; }

        public short status { get; set; }

        public DateTime statusdatetime { get; set; }

        public long vehicleid { get; set; }
        public string drivername { get; set; }
        public string drivernumber { get; set; }


    }

    public class BiddinguserType
    {
        [RegularExpression(Constants.RegexStringInput, ErrorMessage = Constants.StringNumeric)]
        [DisplayName("No of Vehicle")]
        public short vehicleno { get; set; }

        [RegularExpression(Constants.RegexStringInput, ErrorMessage = Constants.StringNumeric)]
        [DisplayName("Capacity of Vehicle")]
        public short capacity { get; set; }
    }

    public class BiddingStat
    {
        public long dtlpostingid { get; set; }
        public decimal MaxBidAmount { get; set; }
        public decimal MinBidAmount { get; set; }
        public decimal AvgBidAmount { get; set; }
        public int TotalBids { get; set; }

    }
}
