using ADPQ.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageMicroService.Model
{
    public class Message
    {
        public Guid Person_ID { get; set; }
        public Guid Message_ID { get; set; }
        public Guid Message_Header { get; set; }
        public string Message_Type { get; set; }
        public string Message_To { get; set; }
        public string Message_Subject { get; set; }
        public string Message_Body { get; set; }
        public DateTime Message_Timestamp { get; set; }
        public bool Message_IsRead { get; set; }
        public List<MessageModel> Message_Thread { get; set; }
        public bool Message_HasThread { get; set; }

        public Message(MessageModel message)
        {
            this.Person_ID = message.Person_ID;
            this.Message_ID = message.Message_ID;
            this.Message_Header = message.Message_Header;
            this.Message_Type = message.Message_Type;
            this.Message_To = message.Message_To;
            this.Message_Subject = message.Message_Subject;
            this.Message_Body = message.Message_Body;
            this.Message_Timestamp = message.Message_Timestamp;
            this.Message_IsRead = message.Message_IsRead;
        }
    }
}
