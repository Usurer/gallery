namespace Core.DTO
{
    public class CollectionMetadataDto
    {
        public long? RootId
        {
            get; set;
        }

        public int ItemsCount
        {
            get; set;
        }

        public required Dictionary<DateTime, int> ItemsPerMonth
        {
            get; set;
        }
    }
}