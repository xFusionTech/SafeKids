using Plivo.API;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService
{
    class SMS
    {
        public bool sendSMS(string toNumber, string code)
        {
            RestAPI plivo = new RestAPI("MAZJNJZWRLNTIXYWFHYT", "OTIxNmJhOGZiYTNkNTQ0MTUzNjIxMmM1NGFjZTQy");

            try
            {
                IRestResponse<MessageResponse> resp = plivo.send_message(new Dictionary<string, string>()
                {
                    { "src", "13022731378" }, // Sender's phone number with country code
                    { "dst", toNumber }, // Receiver's phone number wiht country code
                    { "text", "Verification code : "+code }, // Your SMS text message
                    { "url", "http://dotnettest.apphb.com/delivery_report"}, // The URL to which with the status of the message is sent
                    { "method", "POST"} // Method to invoke the url
                });
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
