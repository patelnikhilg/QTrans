using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Models.ViewModel.Bidding
{
    public class PostingListBid
    {
        public long postingid { get; set; }
        public long dtlpostingid { get; set; }
        [DisplayName("Post Type")]
        public string posttype { get; set; }
        [DisplayName("City")]
        public string src_city { get; set; }
        [DisplayName("State")]
        public string src_state { get; set; }
        [DisplayName("Address")]
        public string soureaddress { get; set; }
        [DisplayName("LandMark")]
        public string src_landmark { get; set; }
        [DisplayName("Pincode")]
        public int src_pincode { get; set; }
        [DisplayName("City")]
        public string dst_city { get; set; }
        [DisplayName("State")]
        public string dst_state { get; set; }
        [DisplayName("Address")]
        public string destinationadress { get; set; }
        [DisplayName("LandMark")]
        public string dst_landmark { get; set; }
        [DisplayName("Pincode")]
        public int dst_pincode { get; set; }
        [DisplayName("Material Type")]
        public string materialtype { get; set; }

        [DisplayName("Package Type")]
        public string packagetype { get; set; }

        [DisplayName("Material Weight")]
        public int materialweight { get; set; }
        [DisplayName("Vehicle Type")]
        public string vehicletype { get; set; }
        [DisplayName("No of Vehicle")]
        public short novehicle { get; set; }
        [DisplayName("Pickup DateTime")]
        [DisplayFormat(DataFormatString = Constants.FormatShortDate)]
        public System.DateTime pickupdatetime { get; set; }

        [DisplayName("Order Type")]
        public short ordertype { get; set; }

        [DisplayName("Order Type")]
        public string ordertypeValue { get { return ((QTrans.Utility.OrderType)ordertype).ToString(); } }

        [DisplayFormat(DataFormatString = Constants.FormatShortDate)]
        [DisplayName("Bidding Activation DateTime")]
        public System.DateTime biddingactivatedatetime { get; set; }

        [DisplayName("Bidding Close DateTime")]
        [DisplayFormat(DataFormatString = Constants.FormatShortDate)]
        public System.DateTime biddingclosedatetime { get; set; }

        [DisplayName("Status of Post")]
        public short poststatus { get; set; }

        [DisplayName("Post Amount")]
        public Nullable<decimal> postamount { get; set; }
        [DisplayName("Bid Status")]
        public short BidStatus { get; set; }

        [DisplayName("Bid Status")]
        public string BidStatusValue { get { return ((QTrans.Utility.BiddingStatus)BidStatus).ToString(); } }

        public short rating { get; set; }
    }
}
