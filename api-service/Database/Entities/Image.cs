namespace Database.Entities
{
    public class Image
    {
        public long FileSystemItemId
        {
            get; set;
        }

        public required FileSystemItem FileSystemItem
        {
            get; set;
        }

        public required string Extension
        {
            get; set;
        }

        public int Width
        {
            get; set;
        }

        public int Height
        {
            get; set;
        }
    }
}