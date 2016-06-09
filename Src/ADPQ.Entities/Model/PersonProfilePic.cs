using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADPQ.Entities.Model
{
   public class PersonProfilePic
    {
        public Guid Profile_File_Id { get; set; }
        public Guid PERS_ID { get; set; }
        public string DIS_PIC_ID { get; set;}
        public string Profile_File_Name { get; set; }
        [NotMapped]
        public string image { get; set; }
    }
}
