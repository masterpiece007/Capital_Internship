
using Capital_Internship.Repository;
using Capital_Internship.Repository.Abstract;
using Capital_Internship.Repository.Concrete;
using Capital_Internship.Service.Abstract;
using Capital_Internship.Service.Concrete;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

namespace Capital_Internship.Helpers
{

    public static class IServiceExtensions
    {
        public static void AddScopedServices(this IServiceCollection services)
        {
            services.AddScoped<IProgramsRepository, ProgramsRepository>();
            services.AddScoped<IProgramsService, ProgramsService>();
            services.AddMemoryCache();
        }
        public static void AddSwaggerSetup(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
        public static async Task AddCosmosDBSetup(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = $"AccountEndpoint={configuration.GetValue<string>("AccountEndpoint")};AccountKey={configuration.GetValue<string>("AccountKey")}";
            var databaseName = configuration.GetValue<string>("DatabaseName");

            services.AddDbContextFactory<AppDbContext>(optionsBuilder =>
            optionsBuilder.UseCosmos(
                //connectionString: configuration.GetConnectionString("DefaultConnection"),
                connectionString: connectionString,
                databaseName: databaseName,
                cosmosOptionsAction: options =>
                {
                    options.ConnectionMode(Microsoft.Azure.Cosmos.ConnectionMode.Direct);
                    options.MaxRequestsPerTcpConnection(16);
                    options.MaxTcpConnectionsPerEndpoint(32);
                }));
            var cosmosDb = new CosmosDBConfig(configuration);
            await cosmosDb.Setup();
        }

        public static void AddSerilog(this IServiceCollection services, IConfiguration configuration, WebApplicationBuilder builder)
        {
         
            var logFilePath = configuration.GetSection("Serilog:ConnectionStrings:LogFilePath").Value;

            var IsLoggedToFile = bool.Parse(configuration.GetSection("Serilog:IsLoggedToFile").Value);

            Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();
            builder.Host.UseSerilog(((ctx, lc) =>
            {
                lc.MinimumLevel.Information();
                lc.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
                lc.MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Warning);
                lc.MinimumLevel.Override("Microsoft.AspNetCore.Mvc.RazorPages", LogEventLevel.Warning);
                lc.MinimumLevel.Override("Microsoft.AspNetCore.Mvc.ViewFeatures", LogEventLevel.Warning);
                lc.MinimumLevel.Override("Microsoft.AspNetCore.StaticFiles", LogEventLevel.Warning);
                lc.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Migrations", LogEventLevel.Warning);
                lc.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database", LogEventLevel.Warning);
                lc.MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Infrastructure", LogEventLevel.Warning);
                lc.MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning);
                lc.ReadFrom.Configuration(ctx.Configuration);
                if (IsLoggedToFile)
                    lc.WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day);
            }));
        }

        public static async void AddAllSections(this IServiceCollection services, IConfiguration configuration)
        {
            AddScopedServices(services);
            AddSwaggerSetup(services);
            await AddCosmosDBSetup(services, configuration);
        }

    }

}
