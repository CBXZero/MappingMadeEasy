using System.Collections.Generic;
using MappingMadeEasy.Standard.Nuget.CustomDataAttributes;

namespace MappingMadeEasyTest.TestModels
{
    public class TestModelWithoutAttribute
    {
        [MapToName("Name")]
        public string PersonName { get; set; }
        [MapToName("Age")]
        public int CurrentAge { get; set; }
        [MapToName("RandomData")]
        public List<string> RandomData { get; set; }
        public double CurrentSalary { get; set; }
    }
}