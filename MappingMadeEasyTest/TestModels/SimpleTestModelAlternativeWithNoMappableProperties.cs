using System.Collections.Generic;

namespace MappingMadeEasyTest.TestModels
{
    public class SimpleTestModelAlternativeWithNoMappableProperties
    {
        public string PersonName { get; set; }
        public int CurrentAge { get; set; }
        public List<string> RandomData { get; set; }
        public double CurrentSalary { get; set; }
    }
}