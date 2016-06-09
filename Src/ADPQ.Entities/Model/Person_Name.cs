using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADPQ.Entities.Model
{
    public class Person_Name
    {
        public Guid PERS_ID { get; set; }
        public string PERS_FNAME { get; set; }
        public string PERS_LNAME { get; set; }
        public string PERS_MNAME { get; set; }
        public string PERS_NAME_SFIX { get; set; }
        public string PERS_NAME_PFIX { get; set; }
        public Guid PERS_NM_ID { get; set; }
    }
}
