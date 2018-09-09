using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Utility
{
    public class SmtpMailUtility
    {

        public static bool SendMail(string To, string from, string subject, string body, bool isHtml)
        {
            try
            {
                MailMessage mail = new MailMessage(from, To);
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("user@gmail.com", "password");
                client.Host = "smtp.gmail.com";
                
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = isHtml;
                client.Send(mail);

                return true;
            }
            catch(Exception exp)
            {
                ////TODO: Log exp
            }

            return false;
        }
    }
}
