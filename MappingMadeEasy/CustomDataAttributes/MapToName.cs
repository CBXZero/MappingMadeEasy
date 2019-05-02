using System;
using System.Collections.Generic;
using System.Text;

namespace MappingMadeEasy.CustomDataAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MapToName : Attribute
    {
        public string Name { get; }

        public MapToName(string name)
        {
            Name = name;
        }
    }
}
