using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Models.ViewModel.Bidding
{
    public class BidMinMaxAmount
    {
        public long dtlpostingid { get; set; }

        public int TotalBid { get; set; }

        public decimal Min { get; set; }

        public decimal Max { get; set; }
    }
}
