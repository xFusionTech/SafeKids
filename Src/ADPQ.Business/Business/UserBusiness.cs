using ADPQ.Business.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADPQ.Entities.Model;
using ADPQ.Data.Repository;

namespace ADPQ.Business.Business
{
    public class UserBusiness : UserBusinessContract
    {
        UserRepository repository = new UserRepository();

        public User GetUserProfile(Guid PERS_ID)
        {
            return repository.GetUserProfile(PERS_ID);
        }

        public User login(User User)
        {
             return repository.login(User);
        }

        public User getUserData(Guid PERS_ID, Guid Token)
        {
            return repository.getUserData(PERS_ID, Token);
        }

        public Guid Register(User NewUser)
        {
            return repository.Register(NewUser);
        }
        public bool UpdateProfile(User UpdateUser)
        {
            return repository.UpdateProfile(UpdateUser);

        }
        public bool ValidateToken(Guid AuthToken)
        {
            return repository.ValidateToken(AuthToken);
        }
        public Guid GetPER_ID(Guid Token)
        {
            return repository.GetPER_ID(Token);
        }
        public bool CheckUserID(string Username, int type)
        {
            return repository.CheckUserID(Username, type);
        }
    }
}
