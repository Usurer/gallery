using Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProtoBuf.Meta;

namespace Tests.Integration
{
    public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
    {
        // It is possible for the SQLite to get rid of in-memory db
        // as soon as it has no opened connections (which makes sense).
        // To prevent this I'm going to have this Connection opened until
        // the WebAppFactory is disposed.
        public SqliteConnection Connection;

        protected override void Dispose(bool disposing)
        {
            if (this.Connection != null)
            {
                this.Connection.Close();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Executed when the client is created by the Factory
        /// </summary>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //base.ConfigureWebHost(builder);
            builder.UseEnvironment("Development");

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

        protected void RecreateDb()
        {
        }
    }
}