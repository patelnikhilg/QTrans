using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QTrans.WebAPI.Models
{
    public class BiddingParam
    {
        public long UserId { get; set; }

        public long DtlPostingId { get; set; }
    }

    public class BiddingConfirmParam
    {
        public long UserId { get; set; }

        public long BidUserId { get; set; }

        public long DtlPostingId { get; set; }

        public Int16 BidStatus { get; set; }
    }

    public class BiddingRCStatusParam
    {
        public long UserId { get; set; }

        public long BiddingId { get; set; }

        public Int16 BidStatus { get; set; }

        public string Reason { get; set; }
    }

    public class RatingParam
    {
        public long UserId { get; set; }

        public long DtlPostingId { get; set; }

        public Int16 Rating { get; set; }

        public string RatingComment { get; set; }

        public Int16 IsRate { get; set; }

        public long CreatedBy { get; set; }
    }

    public class BiddingDetailsParam
    {
        public long UserId { get; set; }

        public long biddingId { get; set; }
    }

    public class BiddingStatusParam
    {
        public long UserId { get; set; }
        
        public Int16 Status { get; set; }
    }


    public class BidTruckStatusParam
    {
        public long UserId { get; set; }

        public long dtlbiddingid { get; set; }
        public long vehicleId { get; set; }
        public string drivername { get; set; }
        public string mobilenumber { get; set; }

        public short Status { get; set; }
    }

    public class BiddingUserParam
    {
        public long UserId { get; set; }

        public bool IsPast { get; set; }
    }
}