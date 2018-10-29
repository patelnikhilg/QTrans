using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Models.ViewModel.Bidding
{
    public class OrderList
    {
        public long userid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public long biddingid { get; set; }
        public decimal amount { get; set; }
        public string biddercomment { get; set; }
        public string rating { get; set; }
        public int status { get; set; }
        public int RatingAvg { get; set; }
        public int TotalRating { get; set; }
        public DateTime LastModify { get; set; }
    }
}
