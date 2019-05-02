using System;
using System.Collections.Generic;
using MappingMadeEasy.Standard.Nuget.CustomDataAttributes;

namespace MappingMadeEasyTest.TestModels
{
    [IsModel]
    public class SimpleTestModelWithSystemStruct
    {
        [MapToName("Name")]
        public string Name { get; set; }
        [MapToName("Age")]
        public int Age { get; set; }
        [MapToName("RandomData")]
        public List<string> RandomData { get; set; }
        [MapToName("Salary")]
        public double Salary { get; set; }
        [MapToName("Id")]
        public Guid Id { get; set; }
    }
}