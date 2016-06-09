using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADPQ.Entities.Model
{
    public class MessageModel
    {
        public MessageModel() { }

        //public int ID { get; set; }
        public Guid Person_ID { get; set; }
        public Guid Message_ID { get; set; }
        public Guid Message_Header { get; set; }
        public string Message_Type { get; set; }
        public string Message_To { get; set; }
        public string Message_Subject { get; set; }
        public string Message_Body { get; set; }
        public DateTime Message_Timestamp { get; set; }
        public bool Message_IsRead { get; set; }
    }
}
