namespace KpkWorkflow.WPF
{
    public class FileMessageProvider : IMessageProvider
    {
        public MessageItem GetMessageItem(string key)
        {
            return new FileMessageItem(key);
        }
    }

    public abstract class MessageItem
    {
        public abstract string Key { get; }
        public abstract string GetMessageFor();
    }

    public class FileMessageItem : MessageItem
    {
        private readonly string _key;

        public FileMessageItem(string key)
        {
            _key = key;
        }

        public override string Key
        {
            get { return _key; }
        }

        public override string GetMessageFor()
        {
            // todo get message from file
            return _key + " item";
        }
    }
}