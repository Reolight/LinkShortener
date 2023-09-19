using System.Configuration;
using LinkShortenerStore.Mappers;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;

namespace LinkShortenerStore;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddMySqlWithNHibernateStore(this IServiceCollection services, string connectionString)
    {
        var mapper = new ModelMapper();
        mapper.AddMapping<UrlMap>();
        HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

        var config = new NHibernate.Cfg.Configuration();
        config.DataBaseIntegration(c =>
        {
            c.Dialect<MySQL5Dialect>();
            c.SchemaAction = SchemaAutoAction.Update;
            c.ConnectionString = connectionString;
        });
        config.AddMapping(domainMapping);
        
        var sessionFactory = config.BuildSessionFactory();

        services.AddSingleton(sessionFactory);
        services.AddScoped<ISession>(factory => sessionFactory.OpenSession());
        return services;
    }
}