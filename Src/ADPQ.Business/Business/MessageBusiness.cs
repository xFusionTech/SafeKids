using ADPQ.Entities.Model;
using ADPQ.Data.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADPQ.Business.Business
{
    public class MessageBusiness
    {
        MessageRepository repository = new MessageRepository();
        public bool Add(MessageModel Message)
        {
            return repository.Add(Message);
        }

        public List<MessageModel> Get()
        {
            return repository.Get();
        }

        public bool Remove(Guid message_Header)
        {
            return repository.Remove(message_Header);
        }

        public bool markRead(Guid message_ID)
        {
            return repository.markRead(message_ID);
        }
    }
}
