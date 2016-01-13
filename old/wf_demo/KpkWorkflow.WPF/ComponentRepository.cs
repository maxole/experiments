using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using KpkWorkflow.Model;

namespace KpkWorkflow.WPF
{
    //public class ComponentRepository : IComponentRepository
    //{
    //    private readonly List<IComponent> _components;

    //    public ComponentRepository()
    //    {
    //        _components = new List<IComponent>();
    //    }

    //    public void RegisterComponents(IComponent component)
    //    {
    //        _components.Add(component);
    //    }

    //    public IComponent GetComponent(Type type)
    //    {
    //        return _components.First(type.IsInstanceOfType);
    //    }

    //    public IComponent CloneComponent(Type type, Action<IComponentInitalize> initalize = null)
    //    {
    //        var component = GetComponent(type);
    //        var clonable = component as IComponentClonable;
    //        if (clonable == null)
    //            throw new NotImplementedException("IComponentClonable");
    //        component = clonable.Clone();
    //        var init = component as IComponentInitalize;
    //        if (init != null && initalize != null)
    //            initalize(init);
    //        return component;
    //    }
    //}
}