namespace Core.DTO;

public record FileItemDto : FileSystemItemDto
{
    public override bool IsFolder => false;

    public string Extension
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