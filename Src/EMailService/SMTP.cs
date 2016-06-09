using MicroService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MicroService
{
    class SMTP
    {
        public bool SendEmail(verification verification)
        {
            bool result = true;
            string senderID = "SafeKids-Admin@xfusiontech.com";// use sender’s email id here..
            string senderPassword = "Saad2016"; // sender password here…
            try
            {
                MailMessage mail = new MailMessage("SafeKids-Admin@xfusiontech.com", verification.to);
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(senderID, senderPassword);
                client.Timeout = 1000 * 60;
                client.Host = "secure.emailsrvr.com";
                mail.Subject = "SafeKids Registration Verification Code";
                mail.IsBodyHtml = true;
                mail.Body = @"Hello "+verification.name+@",<br/><br/>
                    You received this email as part of the registration process for SafeKids.<br/><br/>
                    Your verification code for SafeKids is "+ verification.code + @".Please return to the SafeKids website and enter this code to complete your online registration.<br/><br/>
                    Thank you,<br/>
                    SafeKids Administration";
                client.Send(mail);
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
    }
}
