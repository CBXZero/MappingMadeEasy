using System;
using System.Collections.Generic;
using MappingMadeEasy.Standard.Nuget.CustomDataAttributes;

namespace MappingMadeEasyTest.TestModels
{
    [IsModel]
    public class SimpleTestModelWithSystemStructAlternate
    {
        [MapToName("Name")]
        public string PersonName { get; set; }
        [MapToName("Age")]
        public int CurrentAge { get; set; }
        [MapToName("RandomData")]
        public List<string> RandomData { get; set; }
        [MapToName("Salary")]
        public double CurrentSalary { get; set; }
        [MapToName("Id")]
        public Guid PrimaryId { get; set; }
    }
}