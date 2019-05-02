using System;
using System.Collections.Generic;
using System.Text;
using MappingMadeEasy.Standard.Nuget.CustomDataAttributes;

namespace MappingMadeEasyTest.TestModels
{
    [IsModel]
    public class SimpleTestModelAlternative
    {
        [MapToName("Name")]
        public string PersonName { get; set; }
        [MapToName("Age")]
        public int CurrentAge { get; set; }
        [MapToName("RandomData")]
        public List<string> RandomData { get; set; }
        [MapToName("Salary")]
        public double CurrentSalary { get; set; }
    }
}
