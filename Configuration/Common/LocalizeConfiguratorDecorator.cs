using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Resources;

namespace ConfiguratorDecorator
{
    /// <summary>
    /// декоратор для локализации
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// стырено из интернета, дописано
    /// </remarks>
    public abstract class LocalizeConfiguratorDecorator<T> : ConfiguratorDecorator<T>, ICustomTypeDescriptor where T : ConfigurationSection
    {
        protected LocalizeConfiguratorDecorator()
        {
        }

        protected LocalizeConfiguratorDecorator(T configuration)
            : base(configuration)
        {
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return this;
		}

		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
            return TypeDescriptor.GetAttributes(this, true);
		}

		string ICustomTypeDescriptor.GetClassName()
		{
            return TypeDescriptor.GetClassName(this, true);
		}

		string ICustomTypeDescriptor.GetComponentName()
		{
            return TypeDescriptor.GetComponentName(this, true);
		}

		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
            return TypeDescriptor.GetConverter(this, true);
		}

		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
            return TypeDescriptor.GetDefaultEvent(this, true);
		}

		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
            return TypeDescriptor.GetDefaultProperty(this, true);
		}

		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
            return TypeDescriptor.GetEvents(this, attributes, true);
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
            return TypeDescriptor.GetEvents(this, true);
		}

		private PropertyDescriptorCollection _propCache;
		private FilterCache _filterCache;

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return ((ICustomTypeDescriptor)this).GetProperties(null);
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			bool filtering = (attributes != null && attributes.Length > 0);
			PropertyDescriptorCollection props = _propCache;
			FilterCache cache = _filterCache;

			// Use a cached version if we can
		    if (filtering && cache != null && cache.IsValid(attributes))
		        return cache.FilteredProperties;
		    if (!filtering && props != null)
		        return props;

		    // загрузка доступных ресурсов
		    var manifests = GetType().Assembly.GetManifestResourceNames();
		    var manager = manifests.Select(m => m.Replace(".resources", string.Empty))
                .Select(manifest => new ResourceManager(manifest, GetType().Assembly))
                .ToList();

		    // Create the property collection and filter if necessary
			props = new PropertyDescriptorCollection(null);
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this, attributes, true))
            {
                var attr = prop.Attributes.Cast<Attribute>().ToArray();

                // проверка атрибута локализации
		        var lo = prop.Attributes[typeof (LocalizableAttribute)];
		        if (lo != null && ((LocalizableAttribute) lo).IsLocalizable)
                    // создание локализуемой версии свойства
                    props.Add(new LocalizePropertDescription(prop, attr, manager));
		        else
                    // копирование свойства без изменений
		            props.Add(prop);
		    }

			// Store the computed properties
			if (filtering) 
			{
				cache = new FilterCache
				{
				    Attributes = attributes, 
                    FilteredProperties = props
				};
			    _filterCache = cache;
			} 
			else _propCache = props;

			return props;
		}

		private class FilterCache
		{
			public Attribute[] Attributes;
			public PropertyDescriptorCollection FilteredProperties;
			public bool IsValid(Attribute[] other)
			{
				if (other == null || Attributes == null) return false;
				if (Attributes.Length != other.Length) return false;
			    return !other.Where((t, i) => !Attributes[i].Match(t)).Any();
			}
		}
    }
}