using System.Collections.ObjectModel;
using System.Linq;
using KpkWorkflow.Model;

namespace KpkWorkflow.WPF
{
    public interface IComponentViewCollection : IListener<VisualNotificationEvent>
    {
    }

    public class ComponentViewCollection : ObservableCollection<IVisualPrototype>, IComponentViewCollection
    {        
        private bool Any<T>(T concrent) where T : IVisualPrototype
        {
            return Items.Any(i => concrent.GetType().IsInstanceOfType(i));
        }

        public void Handle(VisualNotificationEvent e)
        {
            var any = Any(e.Message);
            if (!any)
                Add(e.Message);
        }
    }
}
