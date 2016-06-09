using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADPQ.Entities.Model
{
  public class RelationshipMap
    {
        public Guid RELATIONSHIP_MAP_ID { get; set; }
        public Guid Primary_Person_ID { get; set; }
        public Guid Secondary_Person_ID { get; set; }

        public int Relationship_Code { get; set; }
        [NotMapped]
        public Person_Name personname { get; set; }

    }
}
