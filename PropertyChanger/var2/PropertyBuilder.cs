using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Changer2
{
    public interface IBuilder
    {
        ISetter Setter<T>(T entity, string propertyName);
        IGetter Getter<T>(T entity, string propertyName);        
    }

    public class SimpleBuilder : IBuilder
    {
        public virtual ISetter Setter<T>(T entity, string propertyName)
        {
            return new Setter<T>(entity, propertyName);
        }

        public IGetter Getter<T>(T entity, string propertyName)
        {
            return new Setter<T>(entity, propertyName);
        }
    }

    public class DeepBuilder : SimpleBuilder
    {
        public override ISetter Setter<T>(T entity, string propertyName)
        {
            ISetter tmp = new Setter<T>(entity, propertyName);

            var type = entity.GetType();
            var settedProperty = type.GetProperty(propertyName);

            if (!settedProperty.IsDefined(typeof(ModifyAttribute), false))
                return tmp;

            return settedProperty.GetCustomAttributes(typeof(ModifyAttribute), false).OfType<ModifyAttribute>().Select(p => p.PropertyName)
                .Where(e => type.GetProperty(e).IsDefined(typeof(ModifyStrategyAttribute), false))
                .Aggregate(tmp, (foo2, s) => tmp = tmp.Then(s));
        }

        public void Recalc<T>(T entity)
        {
            new Recalc<T>(entity, this).Execute();
        }
    }

    public interface ISetter
    {
        void Set(object value);        
        ISetter Then(string propertyName);
    }

    public interface IGetter
    {
        object Get();
    }

    public class Recalc<T>
    {
        private readonly T _entity;
        private readonly IBuilder _builder;

        public Recalc(T entity, IBuilder builder)
        {
            _entity = entity;
            _builder = builder;
        }

        public void Execute()
        {
            _entity.GetType().GetProperties()
                .Where(p => p.IsDefined(typeof(ModifyAttribute), false))
                .ToList().ForEach(p =>
                {
                    var o = _builder.Getter(_entity, p.Name).Get();
                    _builder.Setter(_entity, p.Name).Set(o);
                });
        }
    }

    public class Setter<T> : ISetter, IGetter
    {
        private readonly T _entity;
        private readonly PropertyInfo _property;

        public Setter(T entity, string propertyName)
        {
            _entity = entity;
            _property = _entity.GetType().GetProperty(propertyName);
        }

        public void Set(object value)
        {
            var o = Convert.ChangeType(value, _property.PropertyType);
            _property.SetValue(_entity, o, new object[] { });
        }

        public object Get()
        {
            return _property.GetValue(_entity, null);
        }

        public ISetter Then(string propertyName)
        {
            return new DeepSetter<T>(this, _entity, propertyName);
        }
    }

    public class DeepSetter<T> : ISetter
    {
        private readonly ISetter _inheritSetter;

        private readonly T _entity;
        private readonly ISetter _setter;

        private readonly Type _type;
        private readonly MethodInfo _method;

        private readonly IEnumerable<PropertyInfo> _dependencProperty;

        public DeepSetter(ISetter inheritSetter, T entity, string propertyName)
        {
            _inheritSetter = inheritSetter;
            _entity = entity;

            _type = _entity.GetType();

            var algorithm = _type.GetCustomAttributes(typeof(AlgorithmAttribute), false).Cast<AlgorithmAttribute>().First().Algorithm;

            var dependencyOf = _type.GetProperty(propertyName).GetCustomAttributes(typeof(ModifyStrategyAttribute), false).Cast<ModifyStrategyAttribute>().First();
            _method = algorithm.GetMethod(dependencyOf.Method);

            _dependencProperty = _type.GetProperties().Where(p => dependencyOf.Parameters.Contains(p.Name));

            _setter = new Setter<T>(entity, propertyName);
        }

        public ISetter Then(string propertyName)
        {
            return new DeepSetter<T>(this, _entity, propertyName);
        }

        public void Set(object value)
        {
            _inheritSetter.Set(value);

            var param = _dependencProperty.Select(w => _type.GetProperty(w.Name).GetValue(_entity, null)).ToArray();            

            var result = _method.Invoke(null, param);

            _setter.Set(result);
        }
    }    
}