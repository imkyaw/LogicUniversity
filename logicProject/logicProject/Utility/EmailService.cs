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
        public static void SendEmail(List<string> addresses, string subject = "", string message = "")
        {
            string email = "testing1496@gmail.com";
            string password = "Team8@logic$21";

            var loginInfo = new System.Net.NetworkCredential(email, password);
            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);

            msg.From = new MailAddress(email);
            foreach(string a in addresses)
            {
                msg.To.Add(new MailAddress(a));
            }
            
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
        }
    }
}