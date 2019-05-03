using System.Collections.Generic;
using MappingMadeEasy.Standard.Nuget.CustomDataAttributes;

namespace MappingMadeEasyTest.TestModels
{
    public class TestModelWithChildModelArrayAlternative
    {
        [MapToName("Name")]
        public string Name { get; set; }
        [MapToName("Age")]
        public int Age { get; set; }
        [MapToName("TestModel")]
        public SimpleTestModelAlternative[] TestModels { get; set; }
    }
}