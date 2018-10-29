using QTrans.DataAccess;
using QTrans.Models;
using QTrans.Models.ResponseModel;
using QTrans.Models.ViewModel.Bidding;
using QTrans.Models.ViewModel.Common;
using QTrans.Models.ViewModel.Posting;
using QTrans.Utility;
using System;
using System.Collections.Generic;
using System.Data;

namespace QTrans.Repositories
{
    public class PostingRepository : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        private PostingDataAccess instance;
        private long UserId;
        #region "=================== Constructor =============================="
        public PostingRepository(long userid)
        {
            this.UserId = userid;
            this.instance = new PostingDataAccess();
        }

        ~PostingRepository()
        {
            this.Dispose(false);
        }
        #endregion
        public ResponseSingleModel<PostingProfile> PostingPorfileCreation(PostingProfile posting, out string message)
        {
            var result = new ResponseSingleModel<PostingProfile>();
            long postingId = 0;
            message = string.Empty;
            if (this.instance.InsertUpdatePosting(posting, out postingId, out message))
            {
                posting.postingid = postingId;
                result.Response = posting;
                result.Status = Constants.WebApiStatusOk;
                result.Message = message;
            }
            else
            {
                result.Status = Constants.WebApiStatusFail;
                result.Message = message;
            }

            return result;
        }

        public ResponseSingleModel<PostingDetails> PostingDetailCreation(PostingDetails postingDetails, out string message)
        {
            var result = new ResponseSingleModel<PostingDetails>();
            long dtlpostingid;
            message = string.Empty;
            if (this.instance.InsertUpdatePostingDetails(postingDetails, out dtlpostingid, out message))
            {
                var ds = this.instance.GetByPostingDetailsId(postingDetails.postingid, out message);
                var lstProfile = DataAccessUtility.ConvertToList<QTrans.Models.ViewModel.Posting.PostingProfileView>(ds.Tables[0]);
                var lstDetails = DataAccessUtility.ConvertToList<PostingDetails>(ds.Tables[1]);
                postingDetails = lstDetails.Count > 0 ? lstDetails[0] : null;
                postingDetails.postingProfile = lstProfile.Count > 0 ? lstProfile[0] : null;
                postingDetails.dtlpostingid = dtlpostingid;
                result.Response = postingDetails;
                result.Status = Constants.WebApiStatusOk;
                result.Message = message;
            }
            else
            {
                result.Status = Constants.WebApiStatusFail;
                result.Message = message;
            }

            return result;
        }

        public ResponseSingleModel<PostingDetails> GetPostingDetailById(long postingId, out string message)
        {
            var result = new ResponseSingleModel<PostingDetails>();
            message = string.Empty;
            PostingDetails postingDetails = null;
            var ds = this.instance.GetByPostingDetailsId(postingId, out message);
            var lstProfile = DataAccessUtility.ConvertToList<QTrans.Models.ViewModel.Posting.PostingProfileView>(ds.Tables[0]);
            var lstDetails = DataAccessUtility.ConvertToList<PostingDetails>(ds.Tables[1]);
            postingDetails = lstDetails.Count > 0 ? lstDetails[0] : null;
            postingDetails.postingProfile = lstProfile.Count > 0 ? lstProfile[0] : null;
            result.Response = postingDetails;
            result.Status = Constants.WebApiStatusOk;
            result.Message = message;
            return result;
        }

        public ResponseSingleModel<PostingDetails> GetPostingDetailByPostId(long postingId, out PostingProfile profile, out string message)
        {
            var result = new ResponseSingleModel<PostingDetails>();
            message = string.Empty;
            PostingDetails postingDetails = null;
            var ds = this.instance.GetByPostingDetailsId(postingId, out message);
            var lstProfile = DataAccessUtility.ConvertToList<PostingProfile>(ds.Tables[0]);
            var lstDetails = DataAccessUtility.ConvertToList<PostingDetails>(ds.Tables[1]);
            profile = lstProfile.Count > 0 ? lstProfile[0] : null;
            postingDetails = lstDetails.Count > 0 ? lstDetails[0] : null;
            result.Response = postingDetails;
            result.Status = Constants.WebApiStatusOk;
            result.Message = message;
            return result;
        }

        public ResponseSingleModel<PostingProfileView> GetPostingProfileById(long postingId, out string message)
        {
            var result = new ResponseSingleModel<PostingProfileView>();
            message = string.Empty;
            var dt = this.instance.GetById(postingId, out message);
            var lstProfile = DataAccessUtility.ConvertToList<QTrans.Models.ViewModel.Posting.PostingProfileView>(dt);
            result.Response = lstProfile != null && lstProfile.Count > 0 ? lstProfile[0] : null; ;
            result.Status = Constants.WebApiStatusOk;
            result.Message = message;
            return result;
        }

        public ResponseCollectionModel<PostingList> GetPostingListByUserId(long userId, bool isPast, out string message)
        {
            var result = new ResponseCollectionModel<PostingList>();
            message = string.Empty;
            var dt = this.instance.GetListPostingByUserId(userId, isPast, out message);
            result.Response = DataAccessUtility.ConvertToList<PostingList>(dt);
            result.Status = Constants.WebApiStatusOk;
            result.Message = message;
            return result;
        }

        public ResponseSingleModel<bool> SubmitRatingByDtlPostId(long dtlpostId, long userId, Int16 rating, string comments, Int16 isRate, long CreatedBy)
        {
            var result = new ResponseSingleModel<bool>();
            result.Response = this.instance.RatingByDtlPostUserId(dtlpostId, userId, rating, comments, isRate, CreatedBy);
            result.Status = Constants.WebApiStatusOk;
            return result;
        }

        public ResponseCollectionModel<Rating> PendingPostRatingByUserId(long userId)
        {
            var result = new ResponseCollectionModel<Rating>();
            var dt = this.instance.PendingPostRatingByUserId(userId);
            result.Response = DataAccessUtility.ConvertToList<Rating>(dt);
            result.Status = Constants.WebApiStatusOk;
            return result;
        }

        public ResponseCollectionModel<PostStats> GetPostingStatsByUserId(long userId)
        {
            var dt = this.instance.GetPostingStatsByUserId(userId);
            var result = new ResponseCollectionModel<PostStats>();
            result.Response = DataAccessUtility.ConvertToList<PostStats>(dt); ;
            result.Status = Constants.WebApiStatusOk;
            return result;
        }

        public ResponseCollectionModel<PostStatusList> GetPostingStatusByUserId(long userId, Int16 PostStatus)
        {
            var result = new ResponseCollectionModel<PostStatusList>();
            var dt = this.instance.GetPostingStatusByUserId(userId, PostStatus);
            result.Response = DataAccessUtility.ConvertToList<PostStatusList>(dt);
            result.Status = Constants.WebApiStatusOk;
            return result;
        }

        public ResponseSingleModel<bool> UpdatePostingStatus(long dtlpostId, Int16 PostStatus)
        {
            var result = new ResponseSingleModel<bool>();
            result.Response = this.instance.UpdatePostingStatus(dtlpostId, PostStatus);
            result.Status = Constants.WebApiStatusOk;
            return result;
        }

        public ResponseCollectionModel<OrderList> GetPostingOrderByUserId(long userId, int Status)
        {
            var response = new ResponseCollectionModel<OrderList>();
            var dt = this.instance.GetPostingOrderByUserId(userId, Status);
            var lstbidding = DataAccessUtility.ConvertToList<OrderList>(dt);          
            response.Response = lstbidding;
            response.Status = Constants.WebApiStatusOk;
            return response;
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
                if (this.instance != null)
                {
                    this.instance.Dispose();
                    this.instance = null;
                }

                ////Clean all memeber and release resource.
            }

            // Free any unmanaged objects here.
            disposed = true;
        }
        #endregion
    }
}
