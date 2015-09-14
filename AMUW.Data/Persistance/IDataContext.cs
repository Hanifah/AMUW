using AMUW.Data.Model;
using System;
using System.Data.Entity;

namespace AMUW.Data.Persistance
{
    public interface IDataContext : IDisposable
    {
        IDbSet<User> Users { get; set; }
        IDbSet<VMUser> VMUsers { get; set; }
        Database Database { get; }
        int SaveChanges();
    }
}
