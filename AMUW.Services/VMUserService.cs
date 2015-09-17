using System;
using System.Collections.Generic;
using AMUW.Data.Model;
using AMUW.Services.Interfaces;
using System.Linq;

namespace AMUW.Services
{
    public class VMUserService : BaseService, IVMUserService
    {
        public void AssignUserToVM(VMUser vmUser)
        {
            DataContext.VMUsers.Add(vmUser);
            DataContext.SaveChanges();
        }

        public List<VMUser> GetAll(int id)
        {
            return DataContext.VMUsers.Where(x => x.UserId == id).ToList();    
        }

        public List<VMUser> GetByVMName(string vmName)
        {
            return DataContext.VMUsers.Where(x => x.VMName == vmName).ToList();
        }
     }
}
