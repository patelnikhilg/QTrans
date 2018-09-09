using System.Collections.Generic;
using QTrans.DataAccess;
using QTrans.Models;

namespace QTrans.Repositories
{
    public class BiddingRepository
    {
        private BiddingDataAccess instanceBidding;
        private long UserId;
        public BiddingRepository(long userid)
        {
            this.UserId = userid;
            this.instanceBidding = new BiddingDataAccess();
        }

        public BiddingProfile PostingCreation(long postingId, BiddingProfile bidding, out string message)
        {
            long biddingId = 0;
            message = string.Empty;
            bidding.postingid = postingId;
            BiddingProfile biddingDetails = null;
            if (this.instanceBidding.InsertUpdateBiddingDetails(bidding, out biddingId, out message))
            {
                biddingDetails.biddingid = biddingId;
                var dt = this.instanceBidding.GetById(biddingId, out message);
                var lst = DataAccessUtility.ConvertToList<BiddingProfile>(dt);
                biddingDetails = lst.Count > 0 ? lst[0] : null;
            }

            return biddingDetails;
        }

        public BiddingProfile GetBiddingDetailById(long biddingId, out string message)
        {
            message = string.Empty;
            BiddingProfile biddingDetails = null;
            var dt = this.instanceBidding.GetById(biddingId, out message);
            var lst = DataAccessUtility.ConvertToList<BiddingProfile>(dt);
            biddingDetails = lst.Count > 0 ? lst[0] : null;
            return biddingDetails;
        }

        public List<BiddingProfile> GetPostingListByPostingId(long postingId, out string message)
        {
            message = string.Empty;
            return null;
        }

        public List<BiddingProfile> GetBiddingListByUserId(long userId, out string message)
        {
            message = string.Empty;
            return null;
        }
    }
}
