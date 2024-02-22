using FileSystem;
using Api.Services;
using Core;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.Globalization;
using Database.Extensions;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Not needed, can as well just pass args to CreateBuilder, i'll leave it here just a reminder
            var options = new WebApplicationOptions
            {
                Args = args,
            };
            WebApplicationBuilder builder = WebApplication.CreateBuilder(options);

            AddLogging(builder);

            IConfigurationSection fileSystemConfigSection = builder.Configuration.GetSection(FileSystemOptions.FileSystem);

            builder.Services.AddControllers();

            AddSwagger(builder);

            builder.Services.AddSqliteDbStorage(builder.Configuration);

            builder.Services.Configure<FileSystemOptions>(fileSystemConfigSection);
            builder.Services.AddFileSystemServices();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            builder.Services.AddCoreServices();

            //builder.Services.AddHostedService<ScheduledScanService>();

            builder.Services.AddSingleton<IScansProcessingService, ScansProcessingService>();

            WebApplication app = builder.Build();

            app.Services.UseSqliteDb();

            app.UseCors();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                // TODO: This will throw an exception, lol. Figure out and fix
                app.UseExceptionHandler();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void AddSwagger(WebApplicationBuilder builder)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
            });
        }

        private static void AddLogging(WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();
            builder.Host.UseSerilog((builderContext, serviceProvider, configuration) =>
            {
                configuration
                    .MinimumLevel.Information()
                    .MinimumLevel.Override(nameof(Microsoft.AspNetCore.Routing), LogEventLevel.Verbose)
                    .WriteTo.Console(
                        restrictedToMinimumLevel: LogEventLevel.Information,
                        formatProvider: CultureInfo.InvariantCulture
                    )
                    .WriteTo.File(
                        restrictedToMinimumLevel: LogEventLevel.Verbose,
                        formatter: new JsonFormatter(),
                        path: "./logs/log.txt",
                        rollingInterval: RollingInterval.Day
                    );
            });
        }
    }
}