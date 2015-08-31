namespace NS.Loader
{
    /// <summary>
    /// источник данных для сканирования
    /// </summary>
    public interface ISource
    {
        string Take();
    }
}