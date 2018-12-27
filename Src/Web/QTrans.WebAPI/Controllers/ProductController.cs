using QTrans.Logging;
using QTrans.Models;
using QTrans.Models.ResponseModel;
using QTrans.Models.ViewModel;
using QTrans.Repositories.Repositories;
using QTrans.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QTrans.WebAPI.Controllers
{
    [RoutePrefix("api/product")]
    public class ProductController : ApiController
    {
        private AppLogger log = new AppLogger("QTransAPILogger");

        [Route("GetProductById")]
        [HttpGet]
        public IHttpActionResult GetProductById([FromBody] ProductParam productParam)
        {
            string message = string.Empty;
            using (var productRepository = new ProductRepository(productParam.userId))
            {
                ResponseSingleModel<Product> result = null;
                if (productParam.productId > 0)
                {
                    result = productRepository.GetById(productParam.productId);
                }
                else
                {
                    log.Info("Product id is grater than zero");
                    return Ok(new { Constants.WebApiStatusFail, data = "Product id is grater than zero" });
                }

                return Ok(new { result.Status, data = result.Message });
            }
        }

        [Route("GetInstallProductById")]
        [HttpGet]
        public IHttpActionResult GetInstallProductById([FromBody] ProductParam productParam)
        {
            string message = string.Empty;
            using (var productRepository = new ProductRepository(productParam.userId))
            {
                ResponseSingleModel<ProductInstallDetails> result = null;
                if (productParam.productId > 0)
                {
                    result = productRepository.GetInstallProductById(productParam.DtlproductId,productParam.productId, productParam.vehicleId);
                }
                else
                {
                    log.Info("Product id is grater than zero");
                    return Ok(new { Constants.WebApiStatusFail, data = "Product id is grater than zero" });
                }

                return Ok(new { result.Status, data = result.Message });
            }
        }

        [Route("ProductRegistration")]
        [HttpPost]
        public IHttpActionResult ProductRegistration(long userId,[FromBody] Product product)
        {
            string message = string.Empty;
            using (var productRepository = new ProductRepository(userId))
            {
                var result = productRepository.ProductRegistration(product, out message);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("ProductInstallation")]
        [HttpPost]
        public IHttpActionResult ProductInstallation([FromBody] ProductInstall productInstall)
        {
            string message = string.Empty;
            using (var productRepository = new ProductRepository(productInstall.UserId))
            {
                var result = productRepository.ProductInstallation(productInstall, out message);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetProductListByUserId")]
        [HttpGet]
        public IHttpActionResult GetProductListByUserId(long userId)
        {
            string message = string.Empty;
            using (var productRepository = new ProductRepository(userId))
            {
                var result = productRepository.GetProductListByUserId(userId);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetInstallProductListByUserId")]
        [HttpGet]
        public IHttpActionResult GetInstallProductListByUserId(ProductInstallParam productInstall)
        {
            string message = string.Empty;
            using (var productRepository = new ProductRepository(productInstall.userId))
            {
                var result = productRepository.GetInstallProductListByUserId(productInstall.userId,productInstall.installerId);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }
    }
}
