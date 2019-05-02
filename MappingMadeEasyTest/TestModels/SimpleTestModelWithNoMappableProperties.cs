using System.Collections.Generic;

namespace MappingMadeEasyTest.TestModels
{
    public class SimpleTestModelWithNoMappableProperties
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public List<string> RandomData { get; set; }
        public double Salary { get; set; }
    }
}