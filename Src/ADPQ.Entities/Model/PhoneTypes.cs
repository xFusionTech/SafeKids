using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADPQ.Entities.Model
{
    public class PhoneTypes
    {
       public Guid PHON_TYP_ID { get; set; }
       public string PHON_TYPE_CD { get; set; }
       public string PHON_TYP_DESC { get; set;}
       public Guid PERS_ID { get; set; }
      
    }
}
