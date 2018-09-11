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

        public BiddingProfile BiddingSubmition(BiddingProfile bidding, out string message)
        {
            long biddingId = 0;
            message = string.Empty;            
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

        /// <summary>
        /// Get list of bidding by posting and user id.
        /// </summary>
        /// <param name="postingId"></param>
        /// <param name="userId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<BiddingProfile> GetBiddingDetailById(long postingId,long userId, out string message)
        {
            message = string.Empty;
            var dt = this.instanceBidding.GetByPostingUserId(postingId, userId, out message);
            var lst = DataAccessUtility.ConvertToList<BiddingProfile>(dt);
            return lst.Count > 0 ? lst : null;
        }

        /// <summary>
        /// Get list of bidding done by userid.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<BiddingProfile> GetListBiddingDetailByUserId(long userId, out string message)
        {
            message = string.Empty;
            var dt = this.instanceBidding.GetByUserId(userId, out message);
            var lst = DataAccessUtility.ConvertToList<BiddingProfile>(dt);
            return lst;
        }
    }
}
