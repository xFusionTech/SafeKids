using ADPQ.Entities.Model;
using ADPQ.Business.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net;
using System.Net.Http;
using MessageMicroService.Model;

namespace MessageMicroService.Controller
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MessageController : ApiController
    {
        UserBusiness ObjUser = new UserBusiness();
        MessageBusiness messageBusiness = new MessageBusiness();
        // GET api/Users 
        [Route("api/Message")]
        public HttpResponseMessage Get(Guid token)
        {
            bool IsSessionActive = ObjUser.ValidateToken(token);
            if (IsSessionActive)
            {
                Guid PersonId = ObjUser.GetPER_ID(token);
                //var result = messageBusiness.Get().Where(mess => mess.Person_ID == PersonId).OrderBy(mess => mess.Message_Header).OrderBy(mess => mess.Message_Timestamp).ToList();
                List<MessageModel> result = messageBusiness.Get().Where(mess => mess.Person_ID == PersonId).OrderByDescending(mess => mess.Message_Timestamp).ToList();

                List<Message> returnList = new List<Message>();
                List<Guid> addedHeaders = new List<Guid>();
                int totalUnread = 0;
                foreach(MessageModel res in result)
                {
                    if(!addedHeaders.Contains(res.Message_Header))
                    {
                        bool isFirst = true;
                        if (!res.Message_IsRead) totalUnread++;
                        Message newMessage = new Message(res);
                        addedHeaders.Add(res.Message_Header);
                        newMessage.Message_Thread = new List<MessageModel>();
                        List<MessageModel> thread = messageBusiness.Get().Where(m => m.Message_Header == res.Message_Header).OrderByDescending(m => m.Message_Timestamp).ToList();
                        if (thread.Count < 2) newMessage.Message_HasThread = false;
                        else newMessage.Message_HasThread = true;
                        foreach (MessageModel messageInThread in thread)
                        {
                            if (isFirst)
                                isFirst = false;
                            else
                            {
                                newMessage.Message_Thread.Add(messageInThread);
                            }
                        }
                        returnList.Add(newMessage);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { result = returnList, totalUnread = totalUnread });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { result = false });
            }
        }

        [Route("api/Message")]
        public HttpResponseMessage Get(Guid token, Guid id)
        {
            bool IsSessionActive = ObjUser.ValidateToken(token);
            if (IsSessionActive)
            {
                if(messageBusiness.Remove(id))
                {
                    return Get(token);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, false);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { result = false });
            }
        }

        // POST api/Users 
        [Route("api/Message")]
        public HttpResponseMessage Post(Guid token, [FromBody] MessageModel value)
        {
            bool IsSessionActive = ObjUser.ValidateToken(token);
            if (IsSessionActive)
            {
                value.Person_ID = ObjUser.GetPER_ID(token);
                var result = messageBusiness.Add(value);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { result = false });
            }
        }
        // POST api/Users 
        [Route("api/Message")]
        public HttpResponseMessage Post(Guid token, Guid id)
        {
            bool IsSessionActive = ObjUser.ValidateToken(token);
            if (IsSessionActive)
            {
                var result = messageBusiness.markRead(id);
                return Get(token);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { result = false });
            }
        }
    }
}
