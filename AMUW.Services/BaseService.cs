using AMUW.Data.Persistance;
using StructureMap;

namespace AMUW.Services
{
    public class BaseService
    {
        protected IDataContext DataContext
        {
            get { return ObjectFactory.GetInstance<IDataContext>(); }
        }
    }
}
