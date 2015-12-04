namespace Core.Scanner
{
    public interface ISource
    {
        string Take();
        string Schema();
    }
}