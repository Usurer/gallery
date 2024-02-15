using Api;
using Api.Models;
using Core.Utils;
using Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace Tests.Integration
{
    public class BasicTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient client;

        public BasicTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            client = _factory.CreateClient();
        }

        [Theory]
        [InlineData("/internals/database/get")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<GalleryContext>();

                db.FileSystemItems.Add(new FileSystemItem
                {
                    CreationDate = DateTimeUtils.ToUnixTimestamp(DateTime.Now),
                    IsFolder = true,
                    Name = "Test",
                    Path = "Test path",
                });
                db.SaveChanges();
            }

            // Arrange
            //var client = _factory.CreateClient();
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var data = await response.Content.ReadFromJsonAsync<IEnumerable<FolderItemInfoModel>>();
            Assert.NotNull(data);
            Assert.Single(data);
        }
    }
}