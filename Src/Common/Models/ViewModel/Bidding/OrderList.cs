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
        public long postingid { get; set; }
        public long dtlpostingid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string src_city { get; set; }
        public string src_state { get; set; }
        public string dst_city { get; set; }
        public string dst_state { get; set; }
        public string materialtype { get; set; }
        [DisplayFormat(DataFormatString = Constants.FormatDateTime)]
        public DateTime pickupdatetime { get; set; }
        public long biddingid { get; set; }
        public long OrderNo { get { return this.biddingid; } }
        [DisplayFormat(DataFormatString = Constants.FormatDateTime)]
        public DateTime OrderDate { get; set; }
        public decimal amount { get; set; }
        public string biddercomment { get; set; }
        public string rating { get; set; }
        public int status { get; set; }
        public int RatingAvg { get; set; }
        public int TotalRating { get; set; }
        [DisplayFormat(DataFormatString = Constants.FormatDateTime)]
        public DateTime LastModify { get; set; }
    }
}
