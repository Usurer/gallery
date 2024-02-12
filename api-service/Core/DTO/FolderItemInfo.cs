namespace Core.DTO
{
    public record FolderItemInfo : ItemInfo
    {
        public override bool IsFolder => true;
    }
}