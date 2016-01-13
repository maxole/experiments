using System;
using System.ComponentModel;
using KpkWorkflow.Model;

namespace KpkWorkflow.WPF
{
    //public class MessageComponent : IMessageComponent, IComponentClonable, IComponentInitalize, IComponentView
    //{
    //    private readonly IMessageRepository _repository;
        
    //    public string Message { get; set; }

    //    public MessageComponent(IMessageRepository repository)
    //    {
    //        _repository = repository;
    //    }

    //    public void Dispose()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public ISite Site { get; set; }

    //    public event EventHandler Disposed;

    //    public IComponent Clone()
    //    {
    //        return new MessageComponent(_repository)
    //        {
    //            Message = Message
    //        };
    //    }

    //    public void Initailize(EventArgs args)
    //    {
    //        var e = (NotificationEventArgs) args;
    //        Message = _repository.GetMessageByKey(e.MessageKey);
    //    }
    //}
}