using System;
using System.Collections.Generic;

namespace PluggedConfiguration
{
    public class ParserConfiguration
    {
        public IList<ParserConfigurationItem> Items { get; private set; }

        public ParserConfiguration()
        {
            Items = new List<ParserConfigurationItem>
            {
                new ParserConfigurationItem {Instance = "1", Id = 1, Name = "M1"},
                new ParserConfigurationItem {Instance = "2", Id = 2, Name = "M2"},
                new ParserConfigurationItem {Instance = "3", Id = 3, Name = "M3"},
                new ParserConfigurationItem {Instance = "default", Id = 0, Name = "0"},
            };
        }
    }

    public class ParserConfigurationItem
    {
        public string Instance { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class ParserConfigurationAttribute : Attribute
    {
        public ParserConfigurationAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
