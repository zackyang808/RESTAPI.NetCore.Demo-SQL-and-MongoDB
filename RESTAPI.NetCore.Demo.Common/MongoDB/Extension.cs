using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace RESTAPI.NetCore.Demo.Common.MongoDB
{
    public static class Extension
    {
        public static void AddMongoDB(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDBOptions>(configuration.GetSection("MongoDBOptions"));

            services.AddSingleton<MongoClient>(c =>
            {
                var options = c.GetService<IOptions<MongoDBOptions>>();
                return new MongoClient(options.Value.ConnectionString);
            });

            services.AddScoped<IMongoDatabase>(c =>
            {
                var options = c.GetService<IOptions<MongoDBOptions>>();
                var client = c.GetService<MongoClient>();
                return client.GetDatabase(options.Value.Database);
            });
        }
    }
}
