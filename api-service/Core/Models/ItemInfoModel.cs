namespace Core.Models
{
    public abstract record ItemInfoModel
    {
        public long Id
        {
            get; init;
        }

        public required string Name
        {
            get; init;
        }

        public DateTime CreationDate
        {
            get; init;
        }

        public abstract bool IsFolder
        {
            get;
        }

        public long UpdatedAtDate
        {
            get; init;
        }
    }
}