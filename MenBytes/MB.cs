using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace MenBytes
{
    public  static class MB
    {
        public const string Admin = "Admin";
        public const string User = "User";

        public const int OrderPendding = 1;

        public const int OrderReady = 2;
    
        public const int OrderComplete = 3;

        public const int OrderCancel = 4;

        public const string newStock = "New";
        public const string updateStock = "update";


        public static void sendEmailFunction(string toEmail, string subjct, string emailBody)
        {
            try
            {
                string senderEmail = System.Configuration.ConfigurationManager.AppSettings["senderEmail"].ToString();

                string senderPassword =
                    System.Configuration.ConfigurationManager.AppSettings["sendPassword"].ToString();

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);

                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                MailMessage mailMessage = new MailMessage(senderEmail, toEmail, subjct, emailBody);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = Encoding.UTF8;

                client.Send(mailMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
               
            }
        }
        
    }
}