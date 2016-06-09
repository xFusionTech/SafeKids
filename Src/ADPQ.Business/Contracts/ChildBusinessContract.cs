using ADPQ.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADPQ.Business.Contracts
{
    public interface ChildBusinessContract
    {
        Guid Add(User child);
        bool Edit(User child);
        User Get(Guid childId,Guid personId);
        List<RelationshipMap> GetChildList(Guid PERS_ID);
        bool Delete(string filename, string Type);
    }
}
