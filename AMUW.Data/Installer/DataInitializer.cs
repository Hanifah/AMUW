using AMUW.Data.Persistance;
using System.Data.Entity;

namespace AMUW.Data.Installer
{
    public class DataInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            base.Seed(context);
        }
    }
}
