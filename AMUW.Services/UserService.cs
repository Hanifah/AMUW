using AMUW.Services.Interfaces;
using System;
using AMUW.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace AMUW.Services
{
    public class UserService : BaseService, IUserService
    {
        public void Register(User user)
        {
            DataContext.Users.Add(user);
            DataContext.SaveChanges();
        }

        public List<User> GetAll()
        {
            return DataContext.Users.ToList();
        }

        public int GetId(string userId)
        {
            return DataContext.Users.FirstOrDefault(x => x.UserId == userId).Id;
        }
    }
}
