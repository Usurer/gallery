namespace Api.Models
{
    public record FolderItemInfoModel : ItemInfoModel
    {
        public override bool IsFolder => true;
    }
}