using AMUW.Data.Model;
using AMUW.Data.Persistance;
using System.Data.Entity;

namespace AMUW.Data.Installer
{
    public class DataInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            var user = new User();
            user.Username = "Administrator";
            user.UserId = "da7ae7ea-be5b-4c12-98bc-7c78d4665ed0";
            base.Seed(context);
        }
    }
}
