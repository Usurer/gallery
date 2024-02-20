namespace Core.DTO
{
    public class ScanTargetDto
    {
        public long Id
        {
            get; set;
        }

        public required string Path
        {
            get; set;
        }

        public required bool IsScanned
        {
            get; set;
        }

        public bool IsInvalid
        {
            get; set;
        }
    }
}