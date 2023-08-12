using MaxWorld.Web.Repositories;

namespace MaxWorld.Web.Services
{
    /// <summary>
    /// 基礎 Service 類別，提供資料庫連線的能力
    /// </summary>
    public abstract class BaseService
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
