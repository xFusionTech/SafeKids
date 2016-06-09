using ADPQ.Entities.Model;
using ADPQ.Business.Business;
//using ADPQ.Entities.Model;
using System;
using System.Collections;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net.Http;
using System.Net;
using System.Web;

namespace MicroService.Controller
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]    
    public class UsersController : ApiController
    {
        UserBusiness ObjUser = new UserBusiness();

        public HttpResponseMessage Get(string id, int type)
        {
            try
            {
                bool result = ObjUser.CheckUserID(id, type);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = result });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // POST api/Login 
        [Route("api/Users/Login")]
        public HttpResponseMessage Login([FromBody] User LoginDetails)
        {
            var result = ObjUser.login(LoginDetails);
            return Request.CreateResponse(HttpStatusCode.OK, new { result, result.TokenId });
        }
        
    }
}
