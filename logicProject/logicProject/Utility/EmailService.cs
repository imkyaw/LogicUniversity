using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;


namespace logicProject.Utility
{
    public class EmailService
    {
        public async Task<bool> SendEmailAsync(string emailTo, string mailbody, string subject)
        {
            var from = new MailAddress("testing1496@gmail.com");
            var to = new MailAddress(emailTo);

            var useDefaultCredentials = true;
            var enableSsl = false;
            //var replyto = ""; // set here your email; 
            var userName = string.Empty;
            var password = string.Empty;
            var port = 25;
            var host = "localhost";

            userName = "testing1496@gmail.com"; // setup here the username; 
            password = "passwordlogic"; // setup here the password; 
            bool.TryParse("true", out useDefaultCredentials); //setup here if it uses defaault credentials 
            bool.TryParse("true", out enableSsl); //setup here if it uses ssl 
            int.TryParse("587", out port); //setup here the port 
            host = "smtp.gmail.com"; //setup here the host 

            using (var mail = new MailMessage())
            {
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = mailbody;
                mail.IsBodyHtml = true;

                //mail.ReplyToList.Add(new MailAddress(replyto, "My Email"));
                //mail.ReplyToList.Add(from);
                //mail.DeliveryNotificationOptions = DeliveryNotificationOptions.Delay |
                //                                   DeliveryNotificationOptions.OnFailure |
                //                                   DeliveryNotificationOptions.OnSuccess;

                using (var client = new SmtpClient())
                {
                    //client.Host = host;
                    //client.EnableSsl = enableSsl;
                    //client.Port = port;
                    //client.Credentials = new NetworkCredential(userName, password);
                    await client.SendMailAsync(mail);
                }
            }

            return true;
        }
    }
}