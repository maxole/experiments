using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;

namespace ConfiguratorDecorator
{
    /// <summary>
    /// локализованное свойство
    /// </summary>
    /// <remarks>
    /// стырено с интернета, дописано
    /// </remarks>
    public class LocalizePropertDescription : PropertyDescriptor
    {
        private readonly PropertyDescriptor _property;
        private readonly List<ResourceManager> _manager;

        public LocalizePropertDescription(PropertyDescriptor descr, Attribute[] attributes, List<ResourceManager> manager)
            : base(descr.Name, attributes)
        {
            _property = descr;
            _manager = manager;
        }
        /// <summary>
        /// локализуемая версия категории
        /// </summary>
        public override string Category
        {
            get { return GetString(() => Category); }
        }
        /// <summary>
        /// локализуемая версия описания
        /// </summary>
        public override string Description
        {
            get { return GetString(() => Description); }
        }
        /// <summary>
        /// локализуемая версия отображаемого имени
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return GetString(() => DisplayName);                
            }
        }

        private string GetString(Expression<Func<string>> memberName)
        {
// ReSharper disable PossibleNullReferenceException
            var member = memberName.Body as MemberExpression;
            var propInfo = member.Member as PropertyInfo;
            return GetString(propInfo.Name);
// ReSharper restore PossibleNullReferenceException
        }

        private string GetString(string memberName)
        {
            var value = memberName;
            try
            {
                var c = string.Format("{0}_{1}_{2}", ComponentType.Name, Name, memberName);
                if (_manager != null)
                {
                    foreach (var manager in _manager)
                    {
                        value = manager.GetString(c);
                        if (value != null)
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
            }
            return value ?? memberName;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            return _property.GetValue(component);
        }

        public override void ResetValue(object component)
        {
        }

        public override void SetValue(object component, object value)
        {
            _property.SetValue(component, value);
            OnValueChanged(component, EventArgs.Empty);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get { return _property.ComponentType; }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get { return _property.PropertyType; }
        }

        public override int GetHashCode()
        {
            return _property.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as LocalizePropertDescription;
            return other != null && other._property.Equals(_property);
        }
    }
}