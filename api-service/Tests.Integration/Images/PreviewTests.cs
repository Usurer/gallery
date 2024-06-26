using Api;
using Api.Models;
using Database;
using System.Net.Http.Json;

namespace Tests.Integration.Images
{
    public class PreviewTests
    {
        private readonly CustomWebApplicationFactory<Program> Factory;
        private readonly HttpClient Client;
        private FileSystemItem Image;

        public PreviewTests()
        {
            Factory = new CustomWebApplicationFactory<Program>();
            Client = Factory.CreateClient();
        }

        public void SeedDb()
        {
            var roots = TestitemsGenerationUtils.GenerateRootFolders(1).ToArray();
            TestitemsGenerationUtils.AddToDb(Factory.Services, roots);

            var images = TestitemsGenerationUtils.GenerateChildren(roots[0].Id, false, 1).ToArray();
            images[0].Path = TestitemsGenerationUtils.GetImagePath();
            TestitemsGenerationUtils.AddToDb(Factory.Services, images);
            Image = images[0];
        }

        [Fact]
        public async Task WhenIdProvided_ThenReturnsOkResult()
        {
            // Arrange
            SeedDb();

            // Act
            var response = await Client.GetAsync($@"/images/{Image.Id}/preview"
                + "?width=100"
            );

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task WhenNonExistantIdProvided_ThenReturns404Result()
        {
            // Arrange
            SeedDb();

            // Act
            var response = await Client.GetAsync($@"/images/{999}/preview"
                + "?timestamp=10"
                + "&width=100"
                + "&height=200"
            );

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}