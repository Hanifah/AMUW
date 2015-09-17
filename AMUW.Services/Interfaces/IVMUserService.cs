using AMUW.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMUW.Services.Interfaces
{
    public interface IVMUserService
    {
        void AssignUserToVM(VMUser vmUser);
        List<VMUser> GetAll(int id);
        List<VMUser> GetByVMName(string vmName);
     }
}
