using System.Collections.Generic;
using QTrans.DataAccess;
using QTrans.Models;
using QTrans.Models.ViewModel.Bidding;
using System.Linq;
using System;

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
            if (this.instanceBidding.InsertUpdateBiddingDetails(bidding, out biddingId, out message))
            {
                bidding.biddingid = biddingId;
                foreach(var details in bidding.biddingDetails)
                {
                    details.biddingid = biddingId;
                }
               // var dt = this.instanceBidding.GetById(biddingId);
               // var lst = DataAccessUtility.ConvertToList<BiddingProfile>(dt);
               // bidding = lst.Count > 0 ? lst[0] : null;
            }

            return bidding;
        }      

        public BiddingProfile GetBiddingDetailById(long biddingId)
        {
            var ds = this.instanceBidding.GetById(biddingId);
            BiddingProfile biddingProfile = DataAccessUtility.ConvertToList<BiddingProfile>(ds.Tables[0])[0];
            biddingProfile.biddingDetails = DataAccessUtility.ConvertToList<BiddingDetails>(ds.Tables[1]);
            return biddingProfile;
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
            var ds = this.instanceBidding.GetByUserId(userId);
            var lstbidding = DataAccessUtility.ConvertToList<BiddingProfile>(ds.Tables[0]);
            var lstBidDetails = DataAccessUtility.ConvertToList<BiddingDetails>(ds.Tables[1]);
            foreach (var bid in lstbidding)
            {
                bid.biddingDetails = lstBidDetails.Where(item => item.biddingid == bid.biddingid).ToList();
            }
            return lstbidding;
        }

        /// <summary>
        /// Get list of bidding done by userid.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<BiddingListDetails> GetBiddingListByDtPostingId(long DtlPostingId)
        {
            var ds = this.instanceBidding.GetBiddingListByDtlPostId(DtlPostingId);
            var lstbidding = DataAccessUtility.ConvertToList<BiddingListDetails>(ds.Tables[0]);
            var lstBidDetails = DataAccessUtility.ConvertToList<BiddingDetails>(ds.Tables[1]);
            foreach(var bid in lstbidding)
            {
                bid.biddingDetails = lstBidDetails.Where(item => item.biddingid == bid.biddingid).ToList(); 
            }

            return lstbidding;
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

        /// <summary>
        /// Get Min Max Amount of bidding by DtlPostId.
        /// </summary>
        /// <param name="dtlpostId"></param>
        /// <returns>return bid min max amount and total bid</returns>
        public BidMinMaxAmount GetBidMinMaxByDtlPostId(long dtlpostId)
        {
            var dt = this.instanceBidding.GetMinMaxBidAmount(dtlpostId);
            var result = DataAccessUtility.ConvertToList<BidMinMaxAmount>(dt);
            return result != null && result.Count > 0 ? result[0] : new BidMinMaxAmount() { dtlpostingid = dtlpostId };
        }

       
        public bool SubmitRatingByDtlPostId(long dtlpostId,long userId,Int16 rating, string comments)
        {
            return this.instanceBidding.RatingByDtlPostUserId(dtlpostId, userId, rating, comments);
        }
    }
}
