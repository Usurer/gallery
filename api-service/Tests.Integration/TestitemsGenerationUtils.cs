using Core.Utils;
using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Tests.Integration
{
    internal class TestitemsGenerationUtils
    {
        public const string BASE_PATH = @"testpath:\subfolder\roots";

        private static Func<string, string> CreatePath = (string path) => $@"{BASE_PATH}\{path}";

        public static IEnumerable<FileSystemItem> GenerateRootFolders(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return GenerateFolder(null, $"root_{i}");
            }
        }

        public static IEnumerable<FileSystemItem> GenerateChildren(long parentId, bool generateFolders, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var item = generateFolders 
                    ? GenerateFolder(parentId, i.ToString()) 
                    : GenerateFile(parentId, i.ToString());

                yield return item;
            }
        }

        private static FileSystemItem GenerateFile(long parentId, string namePostfix)
        {
            var fileName = @$"file_{namePostfix}.jpg";
            var path = CreatePath($@"folder_{parentId}\{fileName}");
            return new FileSystemItem
            {
                CreationDate = DateTimeUtils.ToUnixTimestamp(DateTime.Now),
                Name = fileName,
                Path = path,
                Extension = ".jpg",
                Height = 600,
                Width = 800,
                ParentId = parentId
            };
        }

        private static FileSystemItem GenerateFolder(long? parentId, string namePostfix)
        {
            var name = $"folder_{namePostfix}";
            var path = CreatePath($@"folder_{parentId}\{name}");
            return new FileSystemItem
            {
                CreationDate = DateTimeUtils.ToUnixTimestamp(DateTime.Now),
                Name = name,
                Path = path,
                IsFolder = true,
                ParentId = parentId
            };
        }
    }
}
