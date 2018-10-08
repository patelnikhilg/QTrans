using System;
using System.Collections.Generic;

namespace QTrans.Models.ViewModel.Bidding
{
    public class BiddingListDetails
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public long biddingid { get; set; }
        public decimal amount { get; set; }
        public string biddercomment { get; set; }
        public string rating { get; set; }
        public DateTime LastModify { get; set; }

        public ICollection<BiddingDetails> biddingDetails { get; set; }
    }
}
