﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QTrans.Models;

namespace QTrans.DataAccess
{
    public class PostingDataAccess : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        #region "=================== Constructor =============================="
        public PostingDataAccess()
        {
        }

        ~PostingDataAccess()
        {
            this.Dispose(false);
        }
        #endregion
        public bool InsertUpdatePosting(PostingProfile posting, out long identity, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_InsertUpdatePosting", true))
            {

                connector.AddInParameterWithValue("@PostingId", posting.postingid);
                connector.AddInParameterWithValue("@PostType", posting.posttype);
                connector.AddInParameterWithValue("@UserId", posting.userid);
                connector.AddInParameterWithValue("@SoureAddress", posting.soureaddress);
                connector.AddInParameterWithValue("@DestinationAdress", posting.destinationadress);
                connector.AddInParameterWithValue("@MaterialTypeId", posting.materialtypeid);
                connector.AddInParameterWithValue("@Description", posting.description);
                connector.AddInParameterWithValue("@PackageTypeId", posting.packagetypeid);
                connector.AddInParameterWithValue("@PackageTypeDesc", posting.packagetypedesc);
                connector.AddInParameterWithValue("@src_state", posting.src_state);
                connector.AddInParameterWithValue("@src_city", posting.src_city);
                connector.AddInParameterWithValue("@src_pincode", posting.src_pincode);
                connector.AddInParameterWithValue("@src_landmark", posting.src_landmark);
                connector.AddInParameterWithValue("@dst_state", posting.dst_state);
                connector.AddInParameterWithValue("@dst_city", posting.dst_city);
                connector.AddInParameterWithValue("@dst_pincode", posting.dst_pincode);
                connector.AddInParameterWithValue("@dst_landmark", posting.dst_landmark);
                connector.AddOutParameterWithType("@identity", SqlDbType.BigInt);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
                identity = posting.postingid == 0 ? Convert.ToInt64(connector.GetParamaeterValue("@identity")): posting.postingid;
            }

            return rowEffected > 0;
        }

        public bool InsertUpdatePostingDetails(PostingDetails postingDetails, out long identity, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_InsertUpdatePostingDetails", true))
            {
                connector.AddInParameterWithValue("@dtlpostingid", postingDetails.dtlpostingid);
                connector.AddInParameterWithValue("@postingid", postingDetails.postingid);
                connector.AddInParameterWithValue("@materialweight", postingDetails.materialweight);
                connector.AddInParameterWithValue("@materialphotos", postingDetails.materialphotos);
                connector.AddInParameterWithValue("@packingdimension", postingDetails.packingdimension);
                connector.AddInParameterWithValue("@numberpackets", postingDetails.numberpackets);
                connector.AddInParameterWithValue("@vehicletype", postingDetails.vehicletype);
                connector.AddInParameterWithValue("@novehicle", postingDetails.novehicle);
                connector.AddInParameterWithValue("@deliveryperiodday", postingDetails.deliveryperiodday);
                connector.AddInParameterWithValue("@pickupdatetime", postingDetails.pickupdatetime);
                connector.AddInParameterWithValue("@postamount", postingDetails.postamount);
                connector.AddInParameterWithValue("@onpickuppercentage", postingDetails.onpickuppercentage);
                connector.AddInParameterWithValue("@onunloadingpercentage", postingDetails.onunloadingpercentage);
                connector.AddInParameterWithValue("@creditday", postingDetails.creditday);
                connector.AddInParameterWithValue("@contractstartdatetime", postingDetails.contractstartdatetime);
                connector.AddInParameterWithValue("@contractenddatetime", postingDetails.contractenddatetime);
                connector.AddInParameterWithValue("@ordertype", postingDetails.ordertype);
                connector.AddInParameterWithValue("@biddingactivatedatetime", postingDetails.biddingactivatedatetime);
                connector.AddInParameterWithValue("@biddingclosedatetime", postingDetails.biddingclosedatetime);
                connector.AddInParameterWithValue("@poststatus", postingDetails.poststatus);
                connector.AddInParameterWithValue("@gprs", postingDetails.gprs);
                connector.AddInParameterWithValue("@menpowerloading", postingDetails.menpowerloading);
                connector.AddInParameterWithValue("@menpowerunloading", postingDetails.menpowerunloading);
                connector.AddInParameterWithValue("@transporterinsurance", postingDetails.transporterinsurance);
                connector.AddInParameterWithValue("@tolltaxinclude", postingDetails.tolltaxinclude);
                connector.AddInParameterWithValue("@remark", postingDetails.remark);
                connector.AddInParameterWithValue("@loadingtype", postingDetails.loadingtype);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                connector.AddOutParameterWithType("@identity", SqlDbType.BigInt);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
                identity = postingDetails.dtlpostingid == 0 ? Convert.ToInt64(connector.GetParamaeterValue("@identity")) : postingDetails.dtlpostingid;
            }

            return rowEffected > 0;
        }

        public DataTable GetById(long postingId, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetPostingById", true))
            {
                connector.AddInParameterWithValue("@postingId", postingId);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                dt = connector.GetDataTable();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return dt;
        }

        public DataSet GetByPostingDetailsId(long postingId, out string message)
        {
            DataSet ds = null;
            using (DBConnector connector = new DBConnector("Usp_GetPostingDetailsById", true))
            {
                connector.AddInParameterWithValue("@postingId", postingId);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                ds = connector.GetDataSet();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return ds;
        }
        public DataSet GetByPostingPhotosById(long postingId, out string message)
        {
            DataSet ds = null;
            using (DBConnector connector = new DBConnector("Usp_GetPostingPhotosById", true))
            {
                connector.AddInParameterWithValue("@postingId", postingId);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                ds = connector.GetDataSet();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return ds;
        }
        public DataTable GetListPostingByUserId(long userId, bool isPast,out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetListPostingByUserId", true))
            {
                connector.AddInParameterWithValue("@userId", userId);
                connector.AddInParameterWithValue("@isPast", isPast);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                dt = connector.GetDataTable();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return dt;
        }

        public bool RatingByDtlPostUserId(long dtlpostId, long userId, Int16 rating, string comments, Int16 isRate, long CreatedBy)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_PostRatingByDtlPostUserId", true))
            {

                connector.AddInParameterWithValue("@dtlpostingid", dtlpostId);
                connector.AddInParameterWithValue("@UserId", userId);
                connector.AddInParameterWithValue("@rating", rating);
                connector.AddInParameterWithValue("@RatingComment", comments);
                connector.AddInParameterWithValue("@isRate", isRate);
                connector.AddInParameterWithValue("@CreatedBy", CreatedBy);
                rowEffected = connector.ExceuteNonQuery();
            }

            return rowEffected > 0;
        }

        public DataTable PendingPostRatingByUserId(long userId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_PendingPostRatingByUserId", true))
            {

                connector.AddInParameterWithValue("@UserId", userId);
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public DataTable GetPostingStatsByUserId(long userId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetPostingStatsByUserId", true))
            {

                connector.AddInParameterWithValue("@UserId", userId);
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public DataTable GetPostingStatusByUserId(long userId, Int16 PostStatus)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetPostingStatusByUserId", true))
            {

                connector.AddInParameterWithValue("@UserId", userId);
                connector.AddInParameterWithValue("@PostStatus", PostStatus);
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public bool UpdatePostingStatus(long dtlpostingId, Int16 PostStatus)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_UpdatePostingStatusById", true))
            {

                connector.AddInParameterWithValue("@DtlPostingId", dtlpostingId);
                connector.AddInParameterWithValue("@PostStatus", PostStatus);
                rowEffected = connector.ExceuteNonQuery();
            }

            return rowEffected > 0;
        }

        public DataTable GetPostingOrderByUserId(long userId, int Status)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetPostingOrderByUserId", true))
            {
                connector.AddInParameterWithValue("@userId", userId);
                connector.AddInParameterWithValue("@Status", Status);
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


        public int AddPostingPhoto(Int64 postingID, Int64 userID, string filePath, bool isDefault, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_AddPostingPhoto", true))
            {
                connector.AddInParameterWithValue("@PostingID", postingID);
                connector.AddInParameterWithValue("@photo", filePath);
                connector.AddInParameterWithValue("@UserId", userID);
                connector.AddInParameterWithValue("@IsDefault", isDefault);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return rowEffected;
        }

        public DataTable GetPostingSummary(long userId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Asp_GetPostingSummary", true))
            {
                connector.AddInParameterWithValue("@userId", userId);
                dt = connector.GetDataTable();
            }
            return dt;


        }

    }
}
