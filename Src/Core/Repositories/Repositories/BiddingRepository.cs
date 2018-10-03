using System.Collections.Generic;
using QTrans.DataAccess;
using QTrans.Models;
using QTrans.Models.ViewModel.Bidding;

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
                var dt = this.instanceBidding.GetById(biddingId);
                var lst = DataAccessUtility.ConvertToList<BiddingProfile>(dt);
                biddingDetails = lst.Count > 0 ? lst[0] : null;
            }

            return biddingDetails;
        }      

        public BiddingProfile GetBiddingDetailById(long biddingId)
        {
            BiddingProfile biddingDetails = null;
            var dt = this.instanceBidding.GetById(biddingId);
            var lst = DataAccessUtility.ConvertToList<BiddingProfile>(dt);
            biddingDetails = lst.Count > 0 ? lst[0] : null;
            return biddingDetails;
        }

        /// <summary>
        /// Get list of bidding by posting and user id.
        /// </summary>
        /// <param name="postingId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public PostingDetailsBid GetPostingDetailByDtlPostingId(long dtlpostingId)
        {
            var dt = this.instanceBidding.GetPostingDetailsByDtlPostingId(dtlpostingId);
            var lst = DataAccessUtility.ConvertToList<PostingDetailsBid>(dt);
            return lst.Count > 0 ? lst[0] : null;
        }

        /// <summary>
        /// Get list of bidding done by userid.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<BiddingProfile> GetBiddingDetailListByUserId(long userId)
        {
            var dt = this.instanceBidding.GetByUserId(userId);
            var lst = DataAccessUtility.ConvertToList<BiddingProfile>(dt);
            return lst;
        }


        /// <summary>
        /// Get list of bidding done by userid (false means Current posting otherwise close posting).
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isPast"></param>
        /// <returns></returns>
        public List<PostingListBid> GetPostingList(long userId,bool isPast)
        {
            var dt = this.instanceBidding.GetPostingList(userId,isPast);
            var lst = DataAccessUtility.ConvertToList<PostingListBid>(dt);
            return lst;
        }
    }
}
