using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Models.ViewModel.Bidding
{
    public class BidderSummary
    {

        public int RecommendedPosts { get; set; }

        public int OpenPosts { get; set; }

        public int YourBids { get; set; }

        public int ActiveOrders { get; set; }

        public int CompletedOrders { get; set; }

        public int CanceledOrders { get; set; }

        public int RejectedOrders { get; set; }

    }
}
