using System;
using System.ComponentModel;
using System.Windows.Input;
using ClientWorkflow.WPF;
using KpkWorkflow.Model;

namespace KpkWorkflow.WPF
{
    //public class ErrorMessageComponent : IErrorMessageComponent, IComponentClonable, IComponentInitalize, IComponentView
    //{
    //    private readonly IMessageRepository _repository;
    //    private readonly IEventBroker _eventBroker;

    //    public string Message { get; set; }
    //    public ICommand AbortCommand { get; set; }

    //    public ErrorMessageComponent(IMessageRepository repository, IEventBroker eventBroker)
    //    {
    //        _repository = repository;
    //        _eventBroker = eventBroker;
    //    }

    //    public void Dispose()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public ISite Site { get; set; }
    //    public event EventHandler Disposed;

    //    public bool CanAbort { get; set; }

    //    private void Abort()
    //    {
    //        _eventBroker.Publish(new WorkflowAbortEvent());
    //    }

    //    public IComponent Clone()
    //    {
    //        return new ErrorMessageComponent(_repository, _eventBroker)
    //        {
    //            Message = Message,
    //            CanAbort = CanAbort,
    //            AbortCommand = AbortCommand
    //        };
    //    }

    //    public void Initailize(EventArgs args)
    //    {
    //        var e = (NotificationEventArgs)args;
    //        Message = _repository.GetMessageByKey(e.MessageKey);
    //        AbortCommand = new RelayCommand(
    //            param => Abort(),
    //            param => CanAbort);
    //        CanAbort = true;
    //    }
    //}
}