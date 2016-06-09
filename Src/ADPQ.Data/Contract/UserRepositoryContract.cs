using ADPQ.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADPQ.Data.Contract
{
    public interface UserRepositoryContract
    {
        User login (User p);
        User getUserData(Guid PERS_ID, Guid Token);
        Guid Register(User NewUser);
        User GetUserProfile(Guid PERS_ID);
        bool UpdateProfile(User UpdateUser);
        bool ValidateToken(Guid AuthToken);
        Guid GetPER_ID(Guid Token);
        bool CheckUserID(string Username, int type);
    }
}
