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

        private IEnumerable<FileSystemItem> GenerateRootFolders(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new FileSystemItem
                {
                    CreationDate = DateTimeUtils.ToUnixTimestamp(DateTime.Now),
                    IsFolder = true,
                    Name = $"Root Folder {i}",
                    Path = $@"testpath:\subfolder\roots\folder_{i}",
                };
            }
        }

        private IEnumerable<FileSystemItem> GenerateChildren(long parentId, bool generateFolders, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var fileName = $"file_{i}.jpg";
                var folderName = $"folder_{i}";
                var name = generateFolders ? folderName : fileName;
                var path = $@"testpath:\subfolder\roots\folder_{parentId}\{name}";

                var item = new FileSystemItem
                {
                    CreationDate = DateTimeUtils.ToUnixTimestamp(DateTime.Now),
                    IsFolder = generateFolders,
                    Name = name,
                    Path = path,
                    Extension = generateFolders ? null : ".jpg",
                    Height = generateFolders ? null : 100,
                    Width = generateFolders ? null : 200,
                    ParentId = parentId
                };

                yield return item;
            }
        }

        public BasicTests()
        {
            _factory = new CustomWebApplicationFactory<Program>();
            client = _factory.CreateClient();

            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<GalleryContext>();

                var roots = GenerateRootFolders(5).ToArray();

                db.FileSystemItems.AddRange(roots);
                db.SaveChanges();

                foreach (var root in roots)
                {
                    db.FileSystemItems.AddRange(GenerateChildren(root.Id, false, 10));
                }
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
            Assert.Equal(5 + 5 * 10, data.Count());
        }

        [Theory]
        [InlineData("/folders/listItems/")]
        public async Task Folders_WhenListItems_ReturnsOk(string url)
        {
            // Arrange
            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<IEnumerable<FolderItemInfoModel>>();

            Assert.NotNull(data);
            Assert.Equal(5, data.Count());
        }
    }
}