﻿using System.Collections.Generic;
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
    public class BiddingRepository : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        private BiddingDataAccess instanceBidding;
        private long UserId;
        #region "=================== Constructor =============================="
        public BiddingRepository(long userid)
        {
            this.UserId = userid;
            this.instanceBidding = new BiddingDataAccess();
        }

        ~BiddingRepository()
        {
            this.Dispose(false);
        }
        #endregion

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

        //GetByUserIdDtlPostingId
        public ResponseSingleModel<BiddingProfile> GetBiddingDetailByUserIddtlPostingId(long UserId, long dtlPostingid)
        {
            var response = new ResponseSingleModel<BiddingProfile>();
            var ds = this.instanceBidding.GetByUserIdDtlPostingId(UserId,dtlPostingid);
            BiddingProfile biddingProfile;
            if (ds.Tables[0].Rows.Count >0)
            {
                biddingProfile = DataAccessUtility.ConvertToList<BiddingProfile>(ds.Tables[0])[0];
                if (ds.Tables[1].Rows.Count > 0)
                {
                    biddingProfile.biddingDetails = DataAccessUtility.ConvertToList<BiddingDetails>(ds.Tables[1]);
                }
                else
                    biddingProfile.biddingDetails = null;
            }
            else
            {
                biddingProfile = null;
            }
            response.Response = biddingProfile;
            response.Status = Constants.WebApiStatusOk;

            return response;
        }

        public ResponseSingleModel<bool> UpdateBidTruckStatus(long dtlbiddingid, short status, long vehicleId, string drivername, string mobilenumber)
        {
            var response = new ResponseSingleModel<bool>();
            var result = this.instanceBidding.UpdateBidTruckStatus(dtlbiddingid, status, vehicleId, drivername, mobilenumber);
            response.Response = result;
            response.Status = Constants.WebApiStatusOk;
            return response;
        }

        /// <summary>
        /// Get list of bidding by posting and user id.
        /// </summary>
        /// <param name="postingId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public ResponseSingleModel<PostingDetailsBid> GetPostingDetailByDtlPostingId(long dtlpostingId,long UserId)
        {
            var message = "";
            var response = new ResponseSingleModel<PostingDetailsBid>();
            var dt = this.instanceBidding.GetPostingDetailsByDtlPostingId(dtlpostingId,UserId);
            var lst = DataAccessUtility.ConvertToList<PostingDetailsBid>(dt.Tables[0]);

            var lstRateByTransporter = DataAccessUtility.ConvertToList<RatingByTransporter>(dt.Tables[1]);

            var dsPhotos = this.instanceBidding.GetByPostingPhotosById(dtlpostingId, out message);

            var lstPostingPhotos = DataAccessUtility.ConvertToList<PostingPhotos>(dsPhotos.Tables[0]);
            if (lst.Count > 0)
                lst[0].PostingPhotoList = lstPostingPhotos;

            response.Response = lst.Count > 0 ? lst[0] : null;
            response.Response.ratingByTransporter = lstRateByTransporter.Count > 0 ? lstRateByTransporter[0] : null;
            response.Status = lst.Count > 0 ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
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
            var lstBidStat = DataAccessUtility.ConvertToList<BiddingStat>(ds.Tables[2]);

            foreach (var bid in lstbidding)
            {
                bid.biddingDetails = lstBidDetails.Where(item => item.biddingid == bid.biddingid).ToList();
                foreach (var item in lstBidStat.Where(item => item.dtlpostingid == bid.dtlpostingid).ToList())
                {
                    bid.MinBidAmount =item.MinBidAmount ;
                    bid.MaxBidAmount = item.MaxBidAmount;
                    bid.AvgBidAmount = item.AvgBidAmount;
                    bid.TotalBids = item.TotalBids;
                }
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
            var message="";
            var response = new ResponseCollectionModel<BiddingListDetails>();
            var ds = this.instanceBidding.GetBiddingListByDtlPostId(DtlPostingId);
           
            var lstbidding = DataAccessUtility.ConvertToList<BiddingListDetails>(ds.Tables[0]);
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
        /// Get list of bidding done by userid (false means Current posting otherwise close posting).
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isPast"></param>
        /// <returns></returns>
        public ResponseCollectionModel<PostingListBid> GetPostingList(long userId, bool isPast)
        {
            var response = new ResponseCollectionModel<PostingListBid>();
            var dt = this.instanceBidding.GetPostingList(userId, isPast);
            var lst = DataAccessUtility.ConvertToList<PostingListBid>(dt);
            response.Response = lst;
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

        public ResponseSingleModel<bool> SubmitRatingByDtlPostId(long dtlpostId, long userId, Int16 rating, string comments, Int16 isRate)
        {
            var response = new ResponseSingleModel<bool>();
            var result = this.instanceBidding.RatingByDtlPostUserId(dtlpostId, userId, rating, comments, isRate);
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


        public ResponseSingleModel<bool> AssignTruck(long dtlbiddingId, long vehicleid, string drivername, string drivernumber)
        {
            var response = new ResponseSingleModel<bool>();
            var result = this.instanceBidding.AssignTruck(dtlbiddingId, vehicleid, drivername,drivernumber);
            response.Response = result;
            response.Status = Constants.WebApiStatusOk;
            return response;
        }


        public ResponseSingleModel<bool> BiddingStatusByBiddingId(long biddingId, long userId, Int16 BidStatus,string reason)
        {
            var response = new ResponseSingleModel<bool>();
            var result = this.instanceBidding.BiddingStatusByBiddingId(biddingId, userId, BidStatus, reason);
            response.Response = result;
            response.Status = Constants.WebApiStatusOk;
            return response;
        }

        public ResponseCollectionModel<PostingListBid> GetPostByUserPef(long userId)
        {
            var response = new ResponseCollectionModel<PostingListBid>();
            var dt = this.instanceBidding.GetPostByUserPef(userId);
            var lst = DataAccessUtility.ConvertToList<PostingListBid>(dt);
            response.Response = lst;
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
        public ResponseCollectionModel<OrderList> GetBiddingOrderByUserId(long userId, int Status)
        {
            var response = new ResponseCollectionModel<OrderList>();
            var dt = this.instanceBidding.GetBiddingOrderByUserId(userId, Status);
            var lstbidding = DataAccessUtility.ConvertToList<OrderList>(dt);
            response.Response = lstbidding;
            response.Status = Constants.WebApiStatusOk;
            return response;
        }

        public ResponseSingleModel<BidderSummary> GetBidderSummary(long userId)
        {
            
                var result = new ResponseSingleModel<BidderSummary>();
                var dt = this.instanceBidding.GetBidderSummary(UserId);
                var lst = DataAccessUtility.ConvertToList<BidderSummary>(dt);
                BidderSummary summary = lst.Count > 0 ? lst[0] : null;
                result.Response = summary;
                result.Status = summary != null ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
                result.Message = "";
                return result;

           
        }

        #region ========================= Dispose Method ==============
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed) return;

            if (disposing)
            {
                if (this.instanceBidding != null)
                {
                    this.instanceBidding.Dispose();
                    this.instanceBidding = null;
                }

                ////Clean all memeber and release resource.
            }

            // Free any unmanaged objects here.
            disposed = true;
        }
        #endregion

    }
}
