﻿using QTrans.Models;
using QTrans.Models.ResponseModel;
using QTrans.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QTrans.WebAPI.Controllers
{
    [RoutePrefix("api/tracking")]
    public class TrackingController : ApiController
    {
        [Route("SubmitLocationDetails")]
        [HttpPost]
        public IHttpActionResult SubmitLocationDetails([FromBody] LocationDetails locationDetails)
        {
            string message = string.Empty;
            TrackingCollection.Instance.LocationDetailSubmit(locationDetails);           
            return Ok(new { Constants.WebApiStatusOk, data = true });
        }

        [Route("DeviceLocation")]
        [HttpPost]
        public IHttpActionResult DeviceLocation([FromBody] DeviceMessage deviceMessage)
        {
            string message = string.Empty;            
            TrackingCollection.Instance.DeviceLocation(deviceMessage);
            return Ok(new { Constants.WebApiStatusOk, data = true });
        }


        [Route("DeviceLocationTest")]
        [HttpPost]
        public IHttpActionResult DeviceLocationTest([FromBody] string deviceMessage )
        {
            using (System.IO.StreamWriter file =new System.IO.StreamWriter(@"C:\Users\Public\TestFolder\WriteLines2.txt"))
            {
               
                        file.WriteLine("FromBody:" + deviceMessage);
                

            }
            return Ok(new { Constants.WebApiStatusOk, data = true });
        }
    }
}
