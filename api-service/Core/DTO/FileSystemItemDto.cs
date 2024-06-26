namespace Core.DTO;

public abstract record FileSystemItemDto
{
    public long Id
    {
        get; set;
    }

    public long? ParentId
    {
        get; set;
    }

    public abstract bool IsFolder
    {
        get;
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

    public long UpdatedAtDate
    {
        get; set;
    }
}