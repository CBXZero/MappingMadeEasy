using System;

namespace MappingMadeEasy.Standard.Nuget.CustomDataAttributes
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
