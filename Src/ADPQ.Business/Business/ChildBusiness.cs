using ADPQ.Business.Contracts;
using ADPQ.Data.Repository;
using ADPQ.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADPQ.Business.Business
{
   public class ChildBusiness : ChildBusinessContract
    {
        ChildRepository repository = new ChildRepository();

        public Guid Add(User child)
        {
            return repository.Add(child);
        }

        public bool Edit(User child)
        {
            return repository.Edit(child);
        }

        public User Get(Guid childId,Guid personId)
        {
            return repository.Get(childId,personId);
        }
        public List<RelationshipMap> GetChildList(Guid PERS_ID)
        {

            return repository.GetChildList(PERS_ID);

        }
       public bool Delete(string filename, string Type)
        {
            return repository.Delete(filename, Type);
        }



    }
}
