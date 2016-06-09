using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net.Http;
using MicroService.Model;
using System.Net;

namespace MicroService.Controller
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CodeVerificationController : ApiController
    {
        SMTP smtpObj = new SMTP();
        //SMS smsObj = new SMS();

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] verification verification)
        {
            bool result = false;
            if(verification.type == "email")
            {
                result = smtpObj.SendEmail(verification);
            }
            if(verification.type == "text")
            {
                string to = "1" + verification.to;
                //result = smsObj.sendSMS(to, verification.code);
            }
            return Request.CreateResponse(HttpStatusCode.OK, new { result });
        }
    }
}
