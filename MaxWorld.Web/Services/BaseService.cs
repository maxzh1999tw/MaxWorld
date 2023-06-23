using MaxWorld.Web.Repositories;

namespace MaxWorld.Web.Services
{
    public class BaseService
    {
        protected Repository Repository { get; set; }

        public BaseService(BaseServiceArgument baseServiceArgument)
        {
            Repository = baseServiceArgument.Repository;
        }
    }

    public class BaseServiceArgument
    {
        public Repository Repository { get; set; }

        public BaseServiceArgument(Repository repository)
        {
            Repository = repository;
        }
    }
}
