using LinkShortenerStore.Mappers;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Connection;
using NHibernate.Driver;
using Environment = NHibernate.Cfg.Environment;

namespace LinkShortenerStore;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddMySqlWithNHibernateStore(this IServiceCollection services, string connectionString)
    {
        var mapper = new ModelMapper();
        mapper.AddMapping<UrlMap>();
        
        HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

        var config = new Configuration();
        config.DataBaseIntegration(c =>
        {
            c.Dialect<MySQL5Dialect>();
            c.Driver<MySqlDataDriver>();
            c.SchemaAction = SchemaAutoAction.Validate;
            c.ConnectionString = connectionString;
            c.ConnectionProvider<DriverConnectionProvider>();
        });
        
        config.AddMapping(domainMapping);
        
        var sessionFactory = config.BuildSessionFactory();

        services.AddSingleton(sessionFactory);
        services.AddScoped<ISession>(factory => sessionFactory.OpenSession());
        return services;
    }
}