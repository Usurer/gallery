using Api;
using Api.Models;
using System.Net.Http.Json;

namespace Tests.Integration.Folders
{
    public class FilesTests
    {
        private readonly CustomWebApplicationFactory<Program> Factory;
        private readonly HttpClient Client;

        public FilesTests()
        {
            Factory = new CustomWebApplicationFactory<Program>();
            Client = Factory.CreateClient();
        }

        private void SeedDb()
        {
            var roots = TestitemsGenerationUtils.GenerateRootFolders(5).ToArray();
            TestitemsGenerationUtils.AddToDb(Factory.Services, roots);

            foreach (var root in roots)
            {
                TestitemsGenerationUtils.AddToDb(Factory.Services, TestitemsGenerationUtils.GenerateChildren(root.Id, false, 10));
                TestitemsGenerationUtils.AddToDb(Factory.Services, TestitemsGenerationUtils.GenerateChildren(root.Id, true, 3));
            }
        }

        [Theory]
        [InlineData("/folders/1/files")]
        public async Task WhenIdProvided_ThenReturnsFolderFiles(string url)
        {
            // Arrange
            SeedDb();

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<IEnumerable<FolderItemInfoModel>>();
            Assert.NotNull(data);
            Assert.Equal(10, data.Count());
        }

        [Theory]
        [InlineData("/folders/files")]
        public async Task WhenIdNotProvided_ThenReturnsAllFiles(string url)
        {
            // Arrange
            SeedDb();

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<IEnumerable<FolderItemInfoModel>>();
            Assert.NotNull(data);
            Assert.Equal(50, data.Count());
        }

        [Theory]
        [InlineData("/folders/999/files")]
        public async Task WhenNonExistingIdProvided_ThenReturnsEmptyArray(string url)
        {
            // Arrange
            SeedDb();

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<IEnumerable<FolderItemInfoModel>>();
            Assert.NotNull(data);
            Assert.Empty(data);
        }
    }
}