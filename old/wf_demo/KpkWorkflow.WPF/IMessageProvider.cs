namespace KpkWorkflow.WPF
{
    public interface IMessageProvider
    {
        MessageItem GetMessageItem(string key);
    }
}