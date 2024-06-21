using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class Image
    {
        public long Id
        {
            get; set;
        }

        // TODO: Should be a secondary key, ref to Id
        public long? FileSystemItemId
        {
            get; set;
        }

        public FileSystemItem FileSystemItem
        {
            get; set;
        }

        public string? Extension
        {
            get; set;
        }

        public int? Width
        {
            get; set;
        }

        public int? Height
        {
            get; set;
        }
    }
}