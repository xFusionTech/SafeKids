using ADPQ.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADPQ.Business.Contracts
{
    public interface UserBusinessContract
    {
        User login(User p);
        User getUserData(Guid PERS_ID, Guid Token);
        Guid Register(User NewUser);
        User GetUserProfile(Guid PERS_ID);
        bool UpdateProfile(User UpdateUser);
     //   Guid CreateToken(Guid PERS_ID);
        bool ValidateToken(Guid AuthToken);
        Guid GetPER_ID(Guid Token);
        bool CheckUserID(string Username, int type);
    }
  
}
