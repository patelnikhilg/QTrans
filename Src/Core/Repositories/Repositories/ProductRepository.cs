using QTrans.DataAccess;
using QTrans.Models;
using QTrans.Models.ResponseModel;
using QTrans.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Repositories.Repositories
{
    public class ProductRepository : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        private ProductDataAccess instance;
        private long UserId;
        #region "=================== Constructor =============================="
        public ProductRepository(long userid)
        {
            this.UserId = userid;
            this.instance = new ProductDataAccess();
        }

        ~ProductRepository()
        {
            this.Dispose(false);
        }
        #endregion

        public ResponseSingleModel<Product> VehicleRegistration(Product product, out string message)
        {
            var result = new ResponseSingleModel<Product>();
            long productId = 0;
            message = string.Empty;
            if (this.instance.InsertUpdateProduct(product, out productId, out message))
            {
                product.productId = productId;
                result.Status = Constants.WebApiStatusOk;
            }
            else
            {
                result.Status = Constants.WebApiStatusFail;
                result.Message = message;
            }
            result.Response = product;

            return result;
        }

        public ResponseSingleModel<ProductInstall> VehicleRegistration(ProductInstall install, out string message)
        {
            var result = new ResponseSingleModel<ProductInstall>();
            long dtlProductId = 0;
            message = string.Empty;
            if (this.instance.InsertUpdateInstallProduct(install, out dtlProductId, out message))
            {
                install.dtlProductId = dtlProductId;
                result.Status = Constants.WebApiStatusOk;
            }
            else
            {
                result.Status = Constants.WebApiStatusFail;
                result.Message = message;
            }
            result.Response = install;

            return result;
        }

        public ResponseSingleModel<Product> GetById(long productId)
        {
            var response = new ResponseSingleModel<Product>();
            var dt = this.instance.GetById(productId);
            response.Response = DataAccessUtility.ConvertToList<Product>(dt)[0];
            response.Status = Constants.WebApiStatusOk;

            return response;
        }

        public ResponseSingleModel<ProductInstallDetails> GetInstallProductById(long DtlProductId, long productId, long vehicleId)
        {
            var response = new ResponseSingleModel<ProductInstallDetails>();
            var dt = this.instance.GetInstallProductById(DtlProductId,productId, vehicleId);
            response.Response = DataAccessUtility.ConvertToList<ProductInstallDetails>(dt)[0];
            response.Status = Constants.WebApiStatusOk;

            return response;
        }

        public ResponseCollectionModel<ProductInstallDetails> GetInstallProductListByUserId(long userId, long installer)
        {
            var response = new ResponseCollectionModel<ProductInstallDetails>();
            var dt = this.instance.GetInstallProductListByUserId(userId, installer);
            response.Response = DataAccessUtility.ConvertToList<ProductInstallDetails>(dt);
            response.Status = Constants.WebApiStatusOk;

            return response;
        }

        public ResponseCollectionModel<Product> GetProductListByUserId(long userId)
        {
            var response = new ResponseCollectionModel<Product>();
            var dt = this.instance.GetProductListByUserId(userId);
            response.Response = DataAccessUtility.ConvertToList<Product>(dt);
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

                ////TODO: Clean all memeber and release resource.
            }

            // Free any unmanaged objects here.
            disposed = true;
        }

        #endregion
    }
}
