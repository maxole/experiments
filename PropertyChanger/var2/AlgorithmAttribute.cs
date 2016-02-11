using System;

namespace Changer2
{
    public class AlgorithmAttribute : Attribute
    {
        public Type Algorithm { get; private set; }

        public AlgorithmAttribute(Type algorithm)
        {
            Algorithm = algorithm;
        }
    }

    public class ModifyStrategyAttribute : Attribute
    {
        public string Method { get; private set; }
        public string[] Parameters { get; private set; }

        public ModifyStrategyAttribute(string method, string parameters)
            : this(method, new[] { parameters })
        {

        }

        public ModifyStrategyAttribute(string method, string[] parameters)
        {
            Method = method;
            Parameters = parameters;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ModifyAttribute : Attribute
    {
        public string PropertyName { get; private set; }

        public ModifyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}