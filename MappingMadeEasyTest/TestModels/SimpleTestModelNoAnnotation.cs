using System.Collections.Generic;
using MappingMadeEasy.Standard.Nuget.CustomDataAttributes;

namespace MappingMadeEasyTest.TestModels
{
    public class SimpleTestModelNoAnnotation
    {
        [MapToName("Name")]
        public string Name { get; set; }
        [MapToName("Age")]
        public int Age { get; set; }
        [MapToName("RandomData")]
        public List<string> RandomData { get; set; }
        [MapToName("Salary")]
        public double Salary { get; set; }
    }
}