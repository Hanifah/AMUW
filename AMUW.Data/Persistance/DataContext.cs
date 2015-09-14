using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMUW.Data.Model;

namespace AMUW.Data.Persistance
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext() : base("DefaultConnection") { }
        public IDbSet<User> Users { get; set; }
        public IDbSet<VMUser> VMUsers { get; set; }
    }
}
