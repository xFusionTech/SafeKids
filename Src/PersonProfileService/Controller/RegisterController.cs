using ADPQ.Business.Business;
using ADPQ.Entities.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MicroService.Controller
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RegisterController : ApiController
    {
        UserBusiness ObjUser = new UserBusiness();
        // GET api/Register 


        public HttpResponseMessage Post([FromBody] User UserDetails)
        {
            Guid Token = ObjUser.Register(UserDetails);
            Guid Pers_ID = ObjUser.GetPER_ID(Token);
            User userData = ObjUser.getUserData(Pers_ID, Token);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = userData });
        }

        // GET api/Register/id/token
        public HttpResponseMessage Get(Guid token, Guid id)
        {
            bool IsSessionActive = ObjUser.ValidateToken(token);
            if (IsSessionActive)
            {
                var result = ObjUser.GetUserProfile(id);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = result });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { result = false });
            }

        }
        // PUT api/Register/5 
        [HttpPut]
        public HttpResponseMessage Put(Guid token, [FromBody] User Updateuser)
        {
            bool IsSessionActive = ObjUser.ValidateToken(token);
            if (IsSessionActive)
            {
                var result = ObjUser.UpdateProfile(Updateuser);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = result });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { result = false });
            }

        }
    }
}
