using ADPQ.Business.Business;
using System;
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

   public class ChildController : ApiController
    {
        UserBusiness ObjUser = new UserBusiness();
        public HttpResponseMessage Get(Guid token, Guid id)
        {
            ChildBusiness ObjChild = new ChildBusiness();
            bool IsSessionActive = ObjUser.ValidateToken(token);
            if (IsSessionActive)
            {
                var result = ObjChild.GetChildList(id);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = result });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { result = false });
            }

        }
    }
}
