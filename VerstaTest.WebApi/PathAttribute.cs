namespace VerstaTest.WebApi;

public class PathAttribute : Attribute
{
    public string Path { get; }
    public PathAttribute(string path)
    {
        Path = path;
    }
}
