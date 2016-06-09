using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADPQ.Entities.Model
{
   public class Token
    {
        public Guid TokenId { get; set; }
        public Guid Pers_Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastTokenCheck { get; set; }
        public bool IsActive { get; set; }
    }
}
