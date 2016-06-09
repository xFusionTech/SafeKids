using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADPQ.Entities.Model
{
    public class Address
    {
        public Guid ADR_ID { get; set; }
        public string ADR_LINE1 { get; set; }
        public string ADR_LINE2 { get; set; }
        public string ADR_CITY { get; set; }
        public string ADR_STATE_CD { get; set; }
        public string ADR_CNTRY { get; set; }
        public int ADR_ZIP_CD { get; set; }
        public int ADR_ZIP_EXTN { get; set; }
        public int LOC_ID { get; set; }
        public bool MAILING_ADR { get; set; }
        public Guid PERS_ID { get; set; }
        public string ADR_DESC { get; set; }
    }
}
