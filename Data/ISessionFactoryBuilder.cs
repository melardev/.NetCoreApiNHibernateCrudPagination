using NHibernate;

namespace ApiCoreNHibernateCrudPagination.Data
{
    public interface ISessionFactoryBuilder
    {
        ISessionFactory GetSessionFactory();
    }
}