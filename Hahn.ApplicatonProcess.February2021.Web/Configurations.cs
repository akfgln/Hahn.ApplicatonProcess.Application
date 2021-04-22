using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Hahn.ApplicatonProcess.February2021.Data;
using Hahn.ApplicatonProcess.February2021.Domain;
using Hahn.ApplicatonProcess.February2021.Domain.Common;
using Hahn.ApplicatonProcess.February2021.Domain.Maps;
using Hahn.ApplicatonProcess.February2021.Domain.Security;
using Hahn.ApplicatonProcess.February2021.Web.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hahn.ApplicatonProcess.February2021.Web
{
    public static class Configurations
    {
        public static void Setup(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureUow(services, configuration);
            ConfigureRepositories(services);
            ConfigureAutoMapper(services);
            ConfigureAuth(services);
        }

        private static void ConfigureAuth(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ITokenBuilder, TokenBuilder>();
            services.AddScoped<IPermissionContext, PermissionContext>();
        }

        private static void ConfigureAutoMapper(IServiceCollection services)
        {
            var mapperConfig = AutoMapperConfigurator.Configure();
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(x => mapper);
            services.AddTransient<IAutoMapper, AutoMapperAdapter>();
        }

        private static void ConfigureUow(IServiceCollection services, IConfiguration configuration)
        {
            //var connectionString = configuration["Data:main"];

            //services.AddEntityFrameworkSqlServer();

            //services.AddDbContext<HahnDbContext>(options =>
            //    options.UseSqlServer(connectionString));
            services.AddDbContext<HahnDbContext>(options =>
            options.UseInMemoryDatabase("HahnDb"));
            services.AddScoped<IUnitOfWork>(ctx => new EFUnitOfWork(ctx.GetRequiredService<HahnDbContext>()));
        }

        private static void ConfigureRepositories(IServiceCollection services)
        {
            var exampleProcessorType = typeof(AssetRepository);
            var types = (from t in exampleProcessorType.GetTypeInfo().Assembly.GetTypes()
                         where t.Namespace == exampleProcessorType.Namespace
                               && t.GetTypeInfo().IsClass
                               && t.GetTypeInfo().GetCustomAttribute<CompilerGeneratedAttribute>() == null
                         select t).ToArray();

            foreach (var type in types)
            {
                var interfaceQ = type.GetTypeInfo().GetInterfaces().First();
                services.AddScoped(interfaceQ, type);
            }
        }
    }
}
