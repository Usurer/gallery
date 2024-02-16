using Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Configuration;

namespace Tests.Integration
{
    public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
    {
        private SqliteConnection Connection;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("tests");

            //base.ConfigureWebHost(builder);

            builder.ConfigureServices((context, services) =>
            {
                services.RemoveAll(typeof(DbContextOptions<GalleryContext>));
                services.RemoveAll(typeof(DbConnection));

                var connectionString = context.Configuration.GetConnectionString("sqlite");
                Connection = new SqliteConnection(connectionString);
                Connection.Open();

                services.AddDbContext<GalleryContext>((container, options) =>
                {
                    options.UseSqlite(Connection);
                });

                var sp = services.BuildServiceProvider();
            });
        }
    }
}