using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QTrans.Models;

namespace QTrans.DataAccess
{
    public class PostingDataAccess
    {
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
                connector.AddInParameterWithValue("@PackageTypeDesc  ", posting.packagetypedesc);
                connector.AddOutParameterWithValue("@identity", 0);
                connector.AddOutParameterWithValue("@Message", string.Empty);
                message = connector.GetParamaeterValue("@Message").ToString();
                identity = Convert.ToInt64(connector.GetParamaeterValue("@identity"));
                rowEffected = connector.ExceuteNonQuery();
            }

            return rowEffected > 0;
        }

        public bool InsertUpdatePostingDetails(PostingDetails postingDetails, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_InsertUpdatePostingDetails", true))
            {
                connector.AddInParameterWithValue("@DtlPostingId", postingDetails.dtlpostingid);
                connector.AddInParameterWithValue("@PostingId", postingDetails.postingid);
                connector.AddInParameterWithValue("@Materialweight", postingDetails.materialweight);
                connector.AddInParameterWithValue("@MaterialPhotos", postingDetails.materialphotos);
                connector.AddInParameterWithValue("@PackingDimension", postingDetails.packingdimension);
                connector.AddInParameterWithValue("@NumberPckets", postingDetails.numberpckets);
                connector.AddInParameterWithValue("@Vehicletype", postingDetails.vehicletype);
                connector.AddInParameterWithValue("@NoVehicle", postingDetails.novehicle);
                connector.AddInParameterWithValue("@DeliveryPeriodDay", postingDetails.deliveryperiodday);
                connector.AddInParameterWithValue("@PickupDatetime", postingDetails.pickupdatetime);
                connector.AddInParameterWithValue("@PostAmount", postingDetails.postamount);
                connector.AddInParameterWithValue("@OnPickupPercentage", postingDetails.onpickuppercentage);
                connector.AddInParameterWithValue("@OnUnloadingPercentage", postingDetails.onunloadingpercentage);
                connector.AddInParameterWithValue("@CreditDay", postingDetails.creditday);
                connector.AddInParameterWithValue("@ContractStartDatetime", postingDetails.contractstartdatetime);
                connector.AddInParameterWithValue("@ContractEndDatetime", postingDetails.contractenddatetime);
                connector.AddInParameterWithValue("@OrderType", postingDetails.ordertype);
                connector.AddInParameterWithValue("@BiddingActivateDatetime", postingDetails.biddingactivatedatetime);
                connector.AddInParameterWithValue("@BiddingCloseDateTime", postingDetails.biddingclosedatetime);
                connector.AddInParameterWithValue("@PostStatus", postingDetails.poststatus);
                connector.AddInParameterWithValue("@GPRS", postingDetails.gprs);
                connector.AddInParameterWithValue("@MenpowerLoading", postingDetails.menpowerloading);
                connector.AddInParameterWithValue("@MenpowerUnloading", postingDetails.menpowerunloading);
                connector.AddInParameterWithValue("@TransporterInsurance", postingDetails.transporterinsurance);
                connector.AddInParameterWithValue("@TollTaxInclude", postingDetails.tolltaxinclude);
                connector.AddInParameterWithValue("@Remark", postingDetails.remark);
                connector.AddInParameterWithValue("@LoadingType", postingDetails.loadingtype);
                connector.AddOutParameterWithValue("@Message", string.Empty);
                message = connector.GetParamaeterValue("@Message").ToString();
                rowEffected = connector.ExceuteNonQuery();
            }

            return rowEffected > 0;
        }

        public DataTable GetById(int postingId, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetPostingById", true))
            {
                connector.AddInParameterWithValue("@postingId", postingId);
                connector.AddOutParameterWithValue("@Message", string.Empty);
                message = connector.GetParamaeterValue("@Message").ToString();
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public DataSet GetByPostingDetailsId(long postingId, out string message)
        {
            DataSet ds = null;
            using (DBConnector connector = new DBConnector("Usp_GetPostingDetailsById", true))
            {
                connector.AddInParameterWithValue("@postingId", postingId);
                connector.AddOutParameterWithValue("@Message", string.Empty);
                message = connector.GetParamaeterValue("@Message").ToString();
                ds = connector.GetDataSet();
            }

            return ds;
        }
    }
}
