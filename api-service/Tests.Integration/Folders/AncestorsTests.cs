using Api;
using Api.Models;
using System.Net.Http.Json;

namespace Tests.Integration.Folders
{
    public class AncestorsTests
    {
        private readonly CustomWebApplicationFactory<Program> Factory;
        private readonly HttpClient Client;

        public AncestorsTests()
        {
            Factory = new CustomWebApplicationFactory<Program>();
            Client = Factory.CreateClient();
        }

        [Fact]
        public async Task WhenActionAncestors_WithId_ReturnsAncestors()
        {
            // Arrange
            var roots = TestitemsGenerationUtils.GenerateRootFolders(5).ToArray();
            TestitemsGenerationUtils.AddToDb(Factory.Services, roots);

            var lvl0_ancestor = roots[0];

            var lvl1 = TestitemsGenerationUtils.GenerateChildren(lvl0_ancestor.Id, true, 3).ToArray();
            TestitemsGenerationUtils.AddToDb(Factory.Services, lvl1);
            var lvl1_ancestor = lvl1[0];

            var lvl2 = TestitemsGenerationUtils.GenerateChildren(lvl1_ancestor.Id, true, 3).ToArray();
            TestitemsGenerationUtils.AddToDb(Factory.Services, lvl2);
            var lvl2_ancestor = lvl2[0];

            var lvl3 = TestitemsGenerationUtils.GenerateChildren(lvl2_ancestor.Id, true, 3).ToArray();
            TestitemsGenerationUtils.AddToDb(Factory.Services, lvl3);

            foreach (var root in lvl3)
            {
                TestitemsGenerationUtils.AddToDb(Factory.Services, TestitemsGenerationUtils.GenerateChildren(root.Id, false, 10));
                TestitemsGenerationUtils.AddToDb(Factory.Services, TestitemsGenerationUtils.GenerateChildren(root.Id, true, 3));
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