using FileSystem;
using Api.Services;
using Core;
using Database;
using EasyCaching.Disk;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.Globalization;

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
            builder.Services.AddScoped<IScansStateService, ScansStateService>();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            builder.Services.AddEasyCaching(options =>
            {
                options.WithMessagePack("disk");
                options.UseInMemory("in-memory");

                options.UseDisk(config =>
                {
                    config.DBConfig = new DiskDbOptions { BasePath = "C:\\Coding\\Meaningful Projects\\Gallery\\_cache" };
                }, "disk");
            });

            builder.Services.AddScoped<ImageResizeService>();

            //builder.Services.AddHostedService<ScheduledScanService>();

            builder.Services.AddSingleton<IScansProcessingService, ScansProcessingService>();

            WebApplication app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<GalleryContext>();
                dbContext.Database.EnsureCreated();
            }

            app.UseCors();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
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