using Api;
using Api.Models;
using System.Net.Http.Json;

namespace Tests.Integration.Folders
{
    public class GetTests
    {
        private readonly CustomWebApplicationFactory<Program> Factory;
        private readonly HttpClient Client;

        public GetTests()
        {
            Factory = new CustomWebApplicationFactory<Program>();
            Client = Factory.CreateClient();
        }

        public void SeedDb()
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
        [InlineData("/folders/")]
        public async Task WhenNoIdProvided_ThenReturnsRootFolders(string url)
        {
            // Arrange
            SeedDb();

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<IEnumerable<FolderItemInfoModel>>();

            Assert.NotNull(data);
            Assert.Equal(5, data.Count());
        }

        [Theory]
        [InlineData("/folders/1")]
        public async Task WhenIdProvided_ReturnsSubfolders(string url)
        {
            // Arrange
            SeedDb();

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<IEnumerable<FolderItemInfoModel>>();

            Assert.NotNull(data);
            Assert.Equal(3, data.Count());
        }

        [Theory]
        [InlineData("/folders/999")]
        public async Task WhenNonExistentIdProvided_ReturnsEmptyArray(string url)
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