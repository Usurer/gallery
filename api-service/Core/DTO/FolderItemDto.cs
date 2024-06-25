namespace Core.DTO;

public record FolderItemDto : FileSystemItemDto
{
    public override bool IsFolder => true;
}