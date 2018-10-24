using QTrans.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace QTrans.DataAccess
{
    public class BiddingDataAccess : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        #region "=================== Constructor =============================="
        public BiddingDataAccess()
        {
        }

        ~BiddingDataAccess()
        {
            this.Dispose(false);
        }
        #endregion

        public bool InsertUpdateBiddingDetails(BiddingProfile bidding, out long identity, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_InsertBidding", true))
            {
                List<BiddinguserType> userType = new List<BiddinguserType>();
                foreach (var details in bidding.biddingDetails)
                {
                    userType.Add(new BiddinguserType() { vehicleno = details.vehicleno, capacity = details.capacity });
                }
                connector.AddInParameterWithValue("@biddingid", bidding.biddingid);
                connector.AddInParameterWithValue("@dtlpostingid", bidding.dtlpostingid);
                connector.AddInParameterWithValue("@userid", bidding.userid);
                connector.AddInParameterWithValue("@amount", bidding.amount);
                connector.AddInParameterWithValue("@biddercomment", bidding.biddercomment);
                connector.AddInParameterWithValue("@status", bidding.status);
                connector.AddInParameterWithValue("@servicecharges", bidding.servicecharges);
                connector.AddInParameterWithValue("@paymentmethod", bidding.paymentmethod);
                connector.AddInParameterWithValue("@rating", bidding.rating);
                connector.AddInParameterWithValue("@cancellationreason", bidding.cancellationreason);
                connector.AddInParameterWithValue("@biddingDetails", DataAccessUtility.ToDataTable<BiddinguserType>(userType));
                connector.AddOutParameterWithType("@identity", SqlDbType.BigInt);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
                identity = bidding.biddingid == 0 ? Convert.ToInt64(connector.GetParamaeterValue("@identity")) : bidding.biddingid;
            }

            return rowEffected > 0;
        }

        public DataSet GetById(long biddingId)
        {
            DataSet ds = null;
            using (DBConnector connector = new DBConnector("Usp_GetBiddingDetailsById", true))
            {
                connector.AddInParameterWithValue("@biddingId", biddingId);
                ds = connector.GetDataSet();
            }

            return ds;
        }

        public DataTable GetPostingDetailsByDtlPostingId(long dtlpostingId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetBiddingDetailsByDtlPostingId", true))
            {
                connector.AddInParameterWithValue("@DtlpostingId", dtlpostingId);
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public DataSet GetByUserId(long userId)
        {
            DataSet ds = null;
            using (DBConnector connector = new DBConnector("Usp_GetBiddingDetailsByUserId", true))
            {
                connector.AddInParameterWithValue("@userId", userId);
                ds = connector.GetDataSet();
            }

            return ds;
        }

        public DataSet GetBiddingListByDtlPostId(long dtlPostingId)
        {
            DataSet ds = null;
            using (DBConnector connector = new DBConnector("Usp_GetBiddingListByDtlPostingId", true))
            {
                connector.AddInParameterWithValue("@DtlPostingId", dtlPostingId);
                ds = connector.GetDataSet();
            }

            return ds;
        }

        public DataTable GetPostingList(long userId, bool isPast)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetPostingList", true))
            {
                connector.AddInParameterWithValue("@UserId", userId);
                connector.AddInParameterWithValue("@isPast", isPast);
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public DataTable GetMinMaxBidAmount(long dtlpostId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetMinMaxBidAmounById", true))
            {
                connector.AddInParameterWithValue("@dtlpostingId", dtlpostId);
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public bool RatingByDtlPostUserId(long dtlpostId, long userId, Int16 rating, string comments, Int16 isRate)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_RatingByDtlPostUserId", true))
            {

                connector.AddInParameterWithValue("@dtlpostingid", dtlpostId);
                connector.AddInParameterWithValue("@UserId", userId);
                connector.AddInParameterWithValue("@rating", rating);
                connector.AddInParameterWithValue("@RatingComment", comments);
                connector.AddInParameterWithValue("@isRate", isRate);
                rowEffected = connector.ExceuteNonQuery();
            }

            return rowEffected > 0;
        }

        public DataTable PendingBidRatingByUserId(long userId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_PendingBidRatingByUserId", true))
            {

                connector.AddInParameterWithValue("@UserId", userId);
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public DataTable GetBiddingStatsByUserId(long userId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetBidStatsByUserId", true))
            {

                connector.AddInParameterWithValue("@UserId", userId);
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public DataTable GetBidStatusByUserId(long userId, Int16 PostStatus)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetBidStatusByUserId", true))
            {

                connector.AddInParameterWithValue("@UserId", userId);
                connector.AddInParameterWithValue("@PostStatus", PostStatus);
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public bool BiddingStatusByUserId(long dtlpostingId, long userId, Int16 BidStatus)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_BiddingStatusByUserId", true))
            {

                connector.AddInParameterWithValue("@DtlPostingId", dtlpostingId);
                connector.AddInParameterWithValue("@UserId", userId);
                connector.AddInParameterWithValue("@BidStatus", BidStatus);
                rowEffected = connector.ExceuteNonQuery();
            }

            return rowEffected > 0;
        }

        public DataTable GetPostByUserPef(long userId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetPostByUserPef", true))
            {
                connector.AddInParameterWithValue("@UserId", userId);
                dt = connector.GetDataTable();
            }

            return dt;
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

                ////TODO: Clean all memeber and release resource.
            }

            // Free any unmanaged objects here.
            disposed = true;
        }

        #endregion

    }
}
