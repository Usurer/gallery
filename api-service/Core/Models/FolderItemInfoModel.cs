namespace Core.Models
{
    public record FolderItemInfoModel : ItemInfoModel
    {
        public override bool IsFolder => true;
    }
}