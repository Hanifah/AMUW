using AMUW.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMUW.Services.Interfaces
{
    public interface IUserService
    {
        void Register(User user);
        List<User> GetAll();
        int GetId(string userId);
    }
}
