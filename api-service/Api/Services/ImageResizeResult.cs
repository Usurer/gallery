namespace Api.Services
{
    public class ImageResizeResult
    {
        public required byte[] Data
        {
            get; set;
        }

        public required string MimeType
        {
            get; set;
        }

        public required string Name
        {
            get; set;
        }
    }
}