using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Utility
{
    public class SMSUtility
    {

        private string sendSMS(string number, string msg)
        {
            string url = string.Format(ConfigurationManager.AppSettings["SMSGateway"].ToString(), number, msg);
            // Create a request for the URL.   
            System.Net.WebRequest request = System.Net.WebRequest.Create(url);
            // If required by the server, set the credentials.  
            request.Credentials = System.Net.CredentialCache.DefaultCredentials;
            string responseFromServer = string.Empty;
            // Get the response.  
            using (System.Net.WebResponse response = request.GetResponse())
            {
                // Display the status.  
                Console.WriteLine(((System.Net.HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.  
                using (System.IO.Stream dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.  
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(dataStream))
                    {
                        // Read the content.  
                        responseFromServer = reader.ReadToEnd();
                        Console.WriteLine(responseFromServer);
                        // Clean up the streams and the response.  
                        reader.Close();
                        response.Close();
                    }
                }
            }
            // Display the content.  
            string[] strArrResponse;
            strArrResponse = responseFromServer.Split(new char[] { '|' });

            if (strArrResponse.Length > 0)
            {
                return strArrResponse[0] == "1701" ? "Success" : "Error";
            }
            else
            {
                return "Error";
            }
        }
    }
}
