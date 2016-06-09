using ADPQ.Entities.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADPQ.Data.Repository
{
    public class MessageRepository
    {
        public bool Add(MessageModel Message)
        {
            try
            {
                var tzi = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                using (var db = new ADPQContext())
                {
                    MessageModel objmsg;
                    if (Message.Message_Header == new Guid("00000000-0000-0000-0000-000000000000"))
                    {
                        objmsg = new MessageModel
                        {
                            Person_ID = Message.Person_ID,
                            Message_ID = Guid.NewGuid(),
                            Message_Header = Guid.NewGuid(),
                            Message_Type = Message.Message_Type,
                            Message_To = Message.Message_To,
                            Message_Subject = Message.Message_Subject,
                            Message_Body = Message.Message_Body,
                            //Message_Timestamp = DateTime.Now,
                            Message_Timestamp = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi),
                            Message_IsRead = true
                        };
                    }
                    else
                    {
                        objmsg = new MessageModel
                        {
                            Person_ID = Message.Person_ID,
                            Message_ID = Guid.NewGuid(),
                            Message_Header = Message.Message_Header,
                            Message_Type = Message.Message_Type,
                            Message_To = Message.Message_To,
                            Message_Subject = Message.Message_Subject,
                            Message_Body = Message.Message_Body,
                            //    Message_Timestamp = DateTime.Now,
                            Message_Timestamp = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi),
                            Message_IsRead = true
                        };

                    }
                    db.Message.Add(objmsg);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }
           
        }

        public List<MessageModel> Get()
        {
            using (var db = new ADPQContext())
            {
                return db.Message.ToList();
            }
        }
        public bool Remove(Guid message_Header)
        {
            try
            {
                using (var db = new ADPQContext())
                {
                    List<MessageModel> messagesToRemove = db.Message.Where(m => m.Message_Header == message_Header).ToList();
                    db.Message.RemoveRange(messagesToRemove);
                    db.SaveChanges();
                    return true;
                }
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
                return false;
            }
        }

        public bool markRead(Guid message_ID)
        {
            using (var db = new ADPQContext())
            {
                List<MessageModel> msg = db.Message.Where(m => m.Message_ID == message_ID).ToList();
                if(msg.Count > 0)
                {
                    msg.First().Message_IsRead = true;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
