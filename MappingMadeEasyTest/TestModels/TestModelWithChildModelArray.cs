using System.Collections.Generic;
using MappingMadeEasy.Standard.Nuget.CustomDataAttributes;

namespace MappingMadeEasyTest.TestModels
{
    public class TestModelWithChildModelArray
    {
        [MapToName("Name")]
        public string Name { get; set; }
        [MapToName("Age")]
        public int Age { get; set; }
        [MapToName("TestModel")]
        public SimpleTestModel[] TestModels { get; set; }
    }
}