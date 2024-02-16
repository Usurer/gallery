using Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Tests.Integration
{
    public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
    {
        private SqliteConnection Connection;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //base.ConfigureWebHost(builder);
            builder.UseEnvironment("tests");

            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<GalleryContext>));
                services.RemoveAll(typeof(DbConnection));

                Connection = new SqliteConnection();
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