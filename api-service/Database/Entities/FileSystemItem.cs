using Database.Entities;

namespace Database;

public class FileSystemItem
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

    public long UpdatedAtDate
    {
        get; set;
    }

    public FileSystemItem? Parent
    {
        get; set;
    }

    public Image? Image
    {
        get; set;
    }
}