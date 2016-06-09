using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADPQ.Entities.Model
{
   public class Person
    {
        public Guid PERS_ID { get; set; }
        public string Gender { get; set; }
        public int? Race_Ethencity { get; set; }
        public DateTime? DOB { get; set; }
        public bool? IsParent { get; set; }
        public string Social_Security_No { get; set; }
        public int? TIMETOCALL { get; set; }
        public string email { get; set; }

        public Person()
        {
        }

        public Person(Guid PERS_ID)
        {           
            this.PERS_ID = PERS_ID;
        }
    }
}
