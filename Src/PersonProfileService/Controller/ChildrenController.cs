using ADPQ.Business.Business;
using ADPQ.Entities.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class ChildrenController : ApiController
    {
        ChildBusiness ObjChild = new ChildBusiness();
        UserBusiness ObjUser = new UserBusiness();
        public HttpResponseMessage Post(Guid token, [FromBody] User LoginDetails)
        {
            bool IsSessionActive = ObjUser.ValidateToken(token);
            if (IsSessionActive)
            {
                Guid result = ObjChild.Add(LoginDetails);
              
                return Request.CreateResponse(HttpStatusCode.OK, new { result = result });

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { result = false });
            }

        }

        public HttpResponseMessage Get(Guid token, Guid id,[FromUri] Guid PERS_Id)
        {
            bool IsSessionActive = ObjUser.ValidateToken(token);
            if (IsSessionActive)
            {
                var result = ObjChild.Get(id, PERS_Id);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = result });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { result = false });
            }

        }

        [HttpPut]
        public HttpResponseMessage Put(Guid token, [FromBody] User Updateuser)
        {
            bool IsSessionActive = ObjUser.ValidateToken(token);
            if (IsSessionActive)
            {
                var result = ObjChild.Edit(Updateuser);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = result });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { result = false });
            }

        }


        [HttpDelete]
        public HttpResponseMessage Delete(Guid token,Guid id, string Filename, string Type)
        {
            bool IsSessionActive = ObjUser.ValidateToken(token);
            if (IsSessionActive)
            {
                string UploadPath = ConfigurationManager.AppSettings["UploadPath"] + id +"\\"+ Filename;

                if (System.IO.File.Exists(UploadPath))
                {
                    var res = ObjChild.Delete(Filename, Type);

                    if(res)
                      System.IO.File.Delete(UploadPath);
                    else
                        return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new { result = false });

                    return Request.CreateResponse(HttpStatusCode.OK, new { result = true });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { result = false });
                }               
                
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { result = false });
            }

        }
    }
}
