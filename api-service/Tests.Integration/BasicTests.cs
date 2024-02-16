using Api;
using Api.Models;
using Core.Utils;
using Database;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace Tests.Integration
{
    public class BasicTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient client;

        public BasicTests()
        {
            _factory = new CustomWebApplicationFactory<Program>();
            client = _factory.CreateClient();

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

                db.FileSystemItems.Add(new FileSystemItem
                {
                    CreationDate = DateTimeUtils.ToUnixTimestamp(DateTime.Now),
                    IsFolder = true,
                    Name = "Test 2",
                    Path = "Test path 2",
                    ParentId = 1
                });
                db.SaveChanges();
            }
        }

        [Theory]
        [InlineData("/internals/database/get")]
        public async Task Database_WhenGet_ReturnsOk(string url)
        {
            // Arrange
            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<IEnumerable<FolderItemInfoModel>>();

            Assert.NotNull(data);
            Assert.Equal(2, data.Count());
        }

        [Theory]
        [InlineData("/folders/listItems/1")]
        public async Task Folders_WhenListItems_ReturnsOk(string url)
        {
            // Arrange
            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<IEnumerable<FolderItemInfoModel>>();

            Assert.NotNull(data);
            Assert.Single(data);
        }
    }
}