using Api;
using Api.Models;
using Core.Utils;
using Database;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace Tests.Integration
{
    public class FoldersControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> Factory;
        private readonly HttpClient Client;

        public void AddToDb(IEnumerable<FileSystemItem> items)
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<GalleryContext>();

                db.FileSystemItems.AddRange(items);
                db.SaveChanges();
            }
        }

        public FoldersControllerTest()
        {
            Factory = new CustomWebApplicationFactory<Program>();
            Client = Factory.CreateClient();
        }

        [Theory]
        [InlineData("/folders/")]
        public async Task WhenActionGet_WithNoId_ReturnsRootFolders(string url)
        {
            // Arrange
            var roots = TestitemsGenerationUtils.GenerateRootFolders(5).ToArray();
            AddToDb(roots);
            foreach (var root in roots)
            {
                AddToDb(TestitemsGenerationUtils.GenerateChildren(root.Id, false, 10));
            }

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
        public async Task WhenActionGet_WithId_ReturnsSubfolders(string url)
        {
            // Arrange
            var roots = TestitemsGenerationUtils.GenerateRootFolders(5).ToArray();
            AddToDb(roots);

            foreach (var root in roots)
            {
                AddToDb(TestitemsGenerationUtils.GenerateChildren(root.Id, false, 10));
                AddToDb(TestitemsGenerationUtils.GenerateChildren(root.Id, true, 3));
            }

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<IEnumerable<FolderItemInfoModel>>();

            Assert.NotNull(data);
            Assert.Equal(3, data.Count());
        }

        [Theory]
        [InlineData("/folders/1/files")]
        public async Task WhenActionFiles_WithId_ReturnsFolderFiles(string url)
        {
            // Arrange
            var roots = TestitemsGenerationUtils.GenerateRootFolders(5).ToArray();
            AddToDb(roots);

            foreach (var root in roots)
            {
                AddToDb(TestitemsGenerationUtils.GenerateChildren(root.Id, false, 10));
                AddToDb(TestitemsGenerationUtils.GenerateChildren(root.Id, true, 3));
            }

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
        public async Task WhenActionFiles_WithoutId_ReturnsAllFiles(string url)
        {
            // Arrange
            var roots = TestitemsGenerationUtils.GenerateRootFolders(5).ToArray();
            AddToDb(roots);

            foreach (var root in roots)
            {
                AddToDb(TestitemsGenerationUtils.GenerateChildren(root.Id, false, 10));
                AddToDb(TestitemsGenerationUtils.GenerateChildren(root.Id, true, 3));
            }

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<IEnumerable<FolderItemInfoModel>>();

            Assert.NotNull(data);
            Assert.Equal(50, data.Count());
        }

        [Fact]
        public async Task WhenActionAncestors_WithId_ReturnsAncestors()
        {
            // Arrange
            var roots = TestitemsGenerationUtils.GenerateRootFolders(5).ToArray();
            AddToDb(roots);

            var lvl0_ancestor = roots[0];

            var lvl1 = TestitemsGenerationUtils.GenerateChildren(lvl0_ancestor.Id, true, 3).ToArray();
            AddToDb(lvl1);
            var lvl1_ancestor = lvl1[0];

            var lvl2 = TestitemsGenerationUtils.GenerateChildren(lvl1_ancestor.Id, true, 3).ToArray();
            AddToDb(lvl2);
            var lvl2_ancestor = lvl2[0];

            var lvl3 = TestitemsGenerationUtils.GenerateChildren(lvl2_ancestor.Id, true, 3).ToArray();
            AddToDb(lvl3);

            foreach (var root in lvl3)
            {
                AddToDb(TestitemsGenerationUtils.GenerateChildren(root.Id, false, 10));
                AddToDb(TestitemsGenerationUtils.GenerateChildren(root.Id, true, 3));
            }

            var url = $@"/folders/{lvl3[0].Id}/ancestors";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            var data = (await response.Content.ReadFromJsonAsync<IEnumerable<FolderItemInfoModel>>()).ToArray();

            Assert.NotNull(data);
            Assert.Equal(3, data.Count());
            Assert.Equal(lvl2_ancestor.Id, data[0].Id);
            Assert.Equal(lvl1_ancestor.Id, data[1].Id);
            Assert.Equal(lvl0_ancestor.Id, data[2].Id);
        }

        [Fact]
        public async Task WhenActionAncestors_WithIncorrectId_ReturnsError()
        {
            // Arrange
            var url = $@"/folders/9999/ancestors";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            object data = await response.Content.ReadAsStringAsync();

            Assert.NotNull(data);
        }
    }
}