namespace Core;

public record FileSystemItemDto
{
    public long Id
    {
        get; set;
    }

    public long? ParentId
    {
        get; set;
    }

    public bool IsFolder
    {
        get; set;
    }

    public string Path
    {
        get; set;
    }

    public string Name
    {
        get; set;
    }

    public long CreationDate
    {
        get; set;
    }

    public string? Extension
    {
        get; set;
    }

    public int? Width
    {
        get; set;
    }

    public int? Height
    {
        get; set;
    }

    public long UpdatedAtDate
    {
        get; set;
    }
}