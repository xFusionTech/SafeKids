using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADPQ.Entities.Model
{
    public class RelationshipProof
    {
        public Guid Relationship_Proof_ID{get;set;}
        public Guid Primary_Person_ID { get; set; }
        public Guid Secondary_Person_ID { get; set; }
     //   public int RLTCODE { get; set; }
        public int Proof_Type { get; set; }
        public string Proof_Doc { get; set; }
        public string Proof_File_Name { get; set; }
    }
}
