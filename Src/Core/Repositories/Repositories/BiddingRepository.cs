using System.Collections.Generic;
using QTrans.DataAccess;
using QTrans.Models;
using QTrans.Models.ViewModel.Bidding;
using System.Linq;
using System;
using QTrans.Models.ViewModel.Common;
using QTrans.Models.ResponseModel;
using QTrans.Models.ViewModel.Posting;

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

        public ResponseSingleModel<BiddingProfile> BiddingSubmition(BiddingProfile bidding, out string message)
        {
            ResponseSingleModel<BiddingProfile> response = new ResponseSingleModel<BiddingProfile>();
            long biddingId = 0;
            message = string.Empty;
            if (this.instanceBidding.InsertUpdateBiddingDetails(bidding, out biddingId, out message))
            {
                bidding.biddingid = biddingId;
                foreach (var details in bidding.biddingDetails)
                {
                    details.biddingid = biddingId;
                }

                response.Response = bidding;
                response.Message = message;
                response.Status = Constants.WebApiStatusOk;
            }
            else
            {
                response.Message = message;
                response.Status = Constants.WebApiStatusFail;
            }

            return response;
        }      

        public ResponseSingleModel<BiddingProfile> GetBiddingDetailById(long biddingId)
        {
            var response = new ResponseSingleModel<BiddingProfile>();
            var ds = this.instanceBidding.GetById(biddingId);
            BiddingProfile biddingProfile = DataAccessUtility.ConvertToList<BiddingProfile>(ds.Tables[0])[0];
            biddingProfile.biddingDetails = DataAccessUtility.ConvertToList<BiddingDetails>(ds.Tables[1]);
            response.Response = biddingProfile;
            response.Status = Constants.WebApiStatusOk;

            return response;
        }

        /// <summary>
        /// Get list of bidding by posting and user id.
        /// </summary>
        /// <param name="postingId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public ResponseSingleModel<PostingDetailsBid> GetPostingDetailByDtlPostingId(long dtlpostingId)
        {
            var response = new ResponseSingleModel<PostingDetailsBid>();
            var dt = this.instanceBidding.GetPostingDetailsByDtlPostingId(dtlpostingId);
            var lst = DataAccessUtility.ConvertToList<PostingDetailsBid>(dt);
            response.Response = lst.Count > 0 ? lst[0] : null; 
            response.Status = lst.Count > 0 ? Constants.WebApiStatusOk: Constants.WebApiStatusFail;
            return response;
        }

        /// <summary>
        /// Get list of bidding done by userid.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public ResponseCollectionModel<BiddingProfile> GetBiddingDetailListByUserId(long userId)
        {
            var response = new ResponseCollectionModel<BiddingProfile>();
            var ds = this.instanceBidding.GetByUserId(userId);
            var lstbidding = DataAccessUtility.ConvertToList<BiddingProfile>(ds.Tables[0]);
            var lstBidDetails = DataAccessUtility.ConvertToList<BiddingDetails>(ds.Tables[1]);
            foreach (var bid in lstbidding)
            {
                bid.biddingDetails = lstBidDetails.Where(item => item.biddingid == bid.biddingid).ToList();
            }

            response.Response = lstbidding;
            response.Status = Constants.WebApiStatusOk;
            return response;
        }

        /// <summary>
        /// Get list of bidding done by userid.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public ResponseCollectionModel<BiddingListDetails> GetBiddingListByDtPostingId(long DtlPostingId)
        {
            var response = new ResponseCollectionModel<BiddingListDetails>();
            var ds = this.instanceBidding.GetBiddingListByDtlPostId(DtlPostingId);
            var lstbidding = DataAccessUtility.ConvertToList<BiddingListDetails>(ds.Tables[0]);
            var lstBidDetails = DataAccessUtility.ConvertToList<BiddingDetails>(ds.Tables[1]);
            foreach(var bid in lstbidding)
            {
                bid.biddingDetails = lstBidDetails.Where(item => item.biddingid == bid.biddingid).ToList(); 
            }

            response.Response = lstbidding;
            response.Status = Constants.WebApiStatusOk;
            return response;
        }

        /// <summary>
        /// Get list of bidding done by userid (false means Current posting otherwise close posting).
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isPast"></param>
        /// <returns></returns>
        public ResponseCollectionModel<PostingListBid> GetPostingList(long userId,bool isPast)
        {
            var response = new ResponseCollectionModel<PostingListBid>();
            var dt = this.instanceBidding.GetPostingList(userId,isPast);
            var lst = DataAccessUtility.ConvertToList<PostingListBid>(dt);
            response.Response = lst ;
            response.Status = Constants.WebApiStatusOk;
            return response;
        }

        /// <summary>
        /// Get Min Max Amount of bidding by DtlPostId.
        /// </summary>
        /// <param name="dtlpostId"></param>
        /// <returns>return bid min max amount and total bid</returns>
        public ResponseSingleModel<BidMinMaxAmount> GetBidMinMaxByDtlPostId(long dtlpostId)
        {
            var response = new ResponseSingleModel<BidMinMaxAmount>();
            var dt = this.instanceBidding.GetMinMaxBidAmount(dtlpostId);
            var result = DataAccessUtility.ConvertToList<BidMinMaxAmount>(dt);
             
            response.Response = result != null && result.Count > 0 ? result[0] : new BidMinMaxAmount() { dtlpostingid = dtlpostId }; 
            response.Status = Constants.WebApiStatusOk;
            return response;
        }
       
        public ResponseSingleModel<bool> SubmitRatingByDtlPostId(long dtlpostId,long userId,Int16 rating, string comments, Int16 isRate)
        {
            var response = new ResponseSingleModel<bool>();
            var result= this.instanceBidding.RatingByDtlPostUserId(dtlpostId, userId, rating, comments,isRate);
            response.Response = result;
            response.Status = Constants.WebApiStatusOk;
            return response;
        }

        public ResponseCollectionModel<Rating> PendingBidRatingByUserId(long userId)
        {
            var response = new ResponseCollectionModel<Rating>();
            var dt = this.instanceBidding.PendingBidRatingByUserId(userId);
            response.Response = DataAccessUtility.ConvertToList<Rating>(dt);
            response.Status = Constants.WebApiStatusOk;
            return response;
        }

        public ResponseSingleModel<bool> BiddingStatusByUserId(long dtlpostingId, long userId, Int16 BidStatus)
        {
            var response = new ResponseSingleModel<bool>();
            var result = this.instanceBidding.BiddingStatusByUserId(dtlpostingId, userId, BidStatus);
            response.Response = result;
            response.Status = Constants.WebApiStatusOk;
            return response;
        }

        public ResponseCollectionModel<PostStats> GetBiddingStatsByUserId(long userId)
        {
            var dt = this.instanceBidding.GetBiddingStatsByUserId(userId);
            var result = new ResponseCollectionModel<PostStats>();
            result.Response = DataAccessUtility.ConvertToList<PostStats>(dt); ;
            result.Status = Constants.WebApiStatusOk;
            return result;
        }

        /// <summary>
        /// Get list of bidding done by userid (false means Current posting otherwise close posting).
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isPast"></param>
        /// <returns></returns>
        public ResponseCollectionModel<PostingListBid> GetBidStatusByUserId(long userId, Int16 status)
        {
            var response = new ResponseCollectionModel<PostingListBid>();
            var dt = this.instanceBidding.GetBidStatusByUserId(userId, status);
            var lst = DataAccessUtility.ConvertToList<PostingListBid>(dt);
            response.Response = lst;
            response.Status = Constants.WebApiStatusOk;
            return response;
        }

    }
}
