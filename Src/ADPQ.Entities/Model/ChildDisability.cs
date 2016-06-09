using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADPQ.Entities.Model
{
    public class ChildDisability
    {
        public Guid DIS_PERS_ID { get; set; }
        public Guid DIS_ID { get; set; }
        public int DIS_TypeId { get; set; }
        public int DIS_UploadDoc_TypeId { get; set; }
        public string DIS_Document { get; set; }
        public string NOTE { get; set; }
        public string DIS_File_Name { get; set; }
    }
}
